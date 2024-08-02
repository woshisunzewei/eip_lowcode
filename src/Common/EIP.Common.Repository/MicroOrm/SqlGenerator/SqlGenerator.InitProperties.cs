using EIP.Common.Models.Attributes.MicroOrm;
using EIP.Common.Models.Attributes.MicroOrm.Joins;
using EIP.Common.Repository.MicroOrm.Config;
using EIP.Common.Repository.MicroOrm.Extensions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;

namespace EIP.Common.Repository.MicroOrm.SqlGenerator
{
    /// <inheritdoc />
    public partial class SqlGenerator<TEntity>
        where TEntity : class
    {
        private void InitProperties()
        {
            var entityType = typeof(TEntity);
            var entityTypeInfo = entityType.GetTypeInfo();
            var tableAttribute = entityTypeInfo.GetCustomAttribute<TableAttribute>();

            TableName = MicroOrmConfig.TablePrefix + (tableAttribute != null ? tableAttribute.Name : entityTypeInfo.Name);

            TableSchema = tableAttribute != null ? tableAttribute.Schema : string.Empty;
            #region EIP扩展
            InitAllProperties(entityType);
            InitSqlJoinProperties(entityType);
            var props = AllProperties.Where(ExpressionHelper.GetPrimitivePropertiesPredicate()).ToArray();
            InitSqlProperties(entityType, props);
            InitKeyProperties(entityType, props);
            #endregion
            // Use identity as key pattern
            var identityProperty = props.FirstOrDefault(p => p.GetCustomAttributes<IdentityAttribute>().Any());
            if (identityProperty == null && MicroOrmConfig.AllowKeyAsIdentity)
            {
                identityProperty = props.FirstOrDefault(p => p.GetCustomAttributes<KeyAttribute>().Any());
            }

            IdentitySqlProperty = identityProperty != null ? new SqlPropertyMetadata(identityProperty) : null;

            var dateChangedProperty = props.FirstOrDefault(p => p.GetCustomAttributes<UpdatedAtAttribute>().Any());
            if (dateChangedProperty != null && (dateChangedProperty.PropertyType == typeof(DateTime) || dateChangedProperty.PropertyType == typeof(DateTime?)))
            {
                UpdatedAtProperty = dateChangedProperty;
                UpdatedAtPropertyMetadata = new SqlPropertyMetadata(UpdatedAtProperty);
            }
        }

        #region  EIP缓存
        /// <summary>
        /// 是否缓存:需要在初始化时进行判断
        /// </summary>
        public bool IsCache { get; protected set; }
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, SqlPropertyMetadata[]> KeySqlPropertiesCaches = new ConcurrentDictionary<RuntimeTypeHandle, SqlPropertyMetadata[]>();
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, PropertyInfo[]> AllPropertiesCaches = new ConcurrentDictionary<RuntimeTypeHandle, PropertyInfo[]>();
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, SqlJoinPropertyMetadata[]> AllSqlJoinPropertiesCaches = new ConcurrentDictionary<RuntimeTypeHandle, SqlJoinPropertyMetadata[]>();
        private static readonly ConcurrentDictionary<RuntimeTypeHandle, SqlPropertyMetadata[]> AllSqlPropertiesCaches = new ConcurrentDictionary<RuntimeTypeHandle, SqlPropertyMetadata[]>();
        /// <summary>
        /// 初始化属性
        /// </summary>
        /// <param name="type"></param>
        private void InitAllProperties(Type type)
        {
            if (AllPropertiesCaches.TryGetValue(type.TypeHandle, out PropertyInfo[] pis))
            {
                AllProperties = pis;
            }
            else
            {
                AllProperties = type.FindClassProperties().Where(q => q.CanWrite).ToArray();
                AllPropertiesCaches[type.TypeHandle] = AllProperties;
            }
        }

        private void InitKeyProperties(Type type, PropertyInfo[] props)
        {
            if (KeySqlPropertiesCaches.TryGetValue(type.TypeHandle, out SqlPropertyMetadata[] pis))
            {
                KeySqlProperties = pis;
            }
            else
            {
                KeySqlProperties = props.Where(p => p.GetCustomAttributes<KeyAttribute>().Any())
                    .Select(p => new SqlPropertyMetadata(p)).ToArray();
                KeySqlPropertiesCaches[type.TypeHandle] = KeySqlProperties;
            }
        }

        /// <summary>
        /// 初始化属性
        /// </summary>
        /// <param name="type"></param>
        private void InitSqlJoinProperties(Type type)
        {
            if (AllSqlJoinPropertiesCaches.TryGetValue(type.TypeHandle, out SqlJoinPropertyMetadata[] pis))
            {
                SqlJoinProperties = pis;
            }
            else
            {
                var joinProperties = AllProperties.Where(p => p.GetCustomAttributes<JoinAttributeBase>().Any()).ToArray();
                SqlJoinProperties = GetJoinPropertyMetadata(joinProperties);
                AllSqlJoinPropertiesCaches[type.TypeHandle] = SqlJoinProperties;
            }
        }

        /// <summary>
        ///     Get join/nested properties
        /// </summary>
        /// <returns></returns>
        private static SqlJoinPropertyMetadata[] GetJoinPropertyMetadata(PropertyInfo[] joinPropertiesInfo)
        {
            // Filter and get only non collection nested properties
            var singleJoinTypes = joinPropertiesInfo.Where(p => !p.PropertyType.IsConstructedGenericType).ToArray();

            var joinPropertyMetadatas = new List<SqlJoinPropertyMetadata>();

            foreach (var propertyInfo in singleJoinTypes)
            {
                var joinInnerProperties = propertyInfo.PropertyType.GetProperties().Where(q => q.CanWrite)
                    .Where(ExpressionHelper.GetPrimitivePropertiesPredicate());
                joinPropertyMetadatas.AddRange(joinInnerProperties.Where(p => !p.GetCustomAttributes<NotMappedAttribute>().Any())
                    .Select(p => new SqlJoinPropertyMetadata(propertyInfo, p)).ToArray());
            }

            return joinPropertyMetadatas.ToArray();
        }

        /// <summary>
        /// 初始化属性
        /// </summary>
        /// <param name="type"></param>
        /// <param name="props"></param>
        private void InitSqlProperties(Type type, PropertyInfo[] props)
        {
            if (AllSqlPropertiesCaches.TryGetValue(type.TypeHandle, out SqlPropertyMetadata[] pis))
            {
                SqlProperties = pis;
                IsCache = true;
            }
            else
            {
                SqlProperties = props.Where(p => !p.GetCustomAttributes<NotMappedAttribute>().Any())
                    .Select(p => new SqlPropertyMetadata(p)).ToArray();
                AllSqlPropertiesCaches[type.TypeHandle] = SqlProperties;
                IsCache = false;
            }
        }

        #endregion
    }
}
