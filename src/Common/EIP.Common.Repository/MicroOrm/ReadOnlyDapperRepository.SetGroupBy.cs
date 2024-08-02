using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using EIP.Common.Repository.MicroOrm.SqlGenerator.Filters;

namespace EIP.Common.Repository.MicroOrm
{
    /// <summary>
    ///     Base Repository
    /// </summary>
    public partial class ReadOnlyDapperRepository<TEntity>
        where TEntity : class
    {
        public virtual IReadOnlyDapperRepository<TEntity> SetGroupBy()
        {
            FilterData.OrderInfo = null;
            return this;
        }


        public virtual IReadOnlyDapperRepository<TEntity> SetGroupBy(bool permanent,
            Expression<Func<TEntity, object>> expr)
        {
            return SetGroupBy<TEntity>(permanent, expr);
        }


        public virtual IReadOnlyDapperRepository<TEntity> SetGroupBy<T>(bool permanent,
            Expression<Func<T, object>> expr)
        {
            var order = FilterData.GroupInfo ?? new GroupInfo();

            var type = typeof(T);
            switch (expr.Body.NodeType)
            {
                case ExpressionType.Convert:
                    {
                        if (expr.Body is UnaryExpression { Operand: MemberExpression expression })
                        {
                            order.Columns = new List<string> { GetProperty(expression, type) };
                        }

                        break;
                    }
                case ExpressionType.MemberAccess:
                    order.Columns = new List<string> { GetProperty(expr.Body, type) };
                    break;
                default:
                    {
                        var cols = (expr.Body as NewExpression)?.Arguments;
                        var propertyNames = cols?.Select(expression => GetProperty(expression, type)).ToList();
                        order.Columns = propertyNames;
                        break;
                    }
            }

            order.Permanent = permanent;

            FilterData.GroupInfo = order;

            return this;
        }


        public virtual IReadOnlyDapperRepository<TEntity> SetGroupBy(Expression<Func<TEntity, object>> expr)
        {
            return SetGroupBy(false, expr);
        }


        public virtual IReadOnlyDapperRepository<TEntity> SetOrderBy<T>(Expression<Func<T, object>> expr)
        {
            return SetGroupBy(false, expr);
        }
    }
}
