using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Dapper;
using EIP.Common.Repository.AdoNet;

namespace EIP.Common.Repository.MicroOrm
{
    /// <summary>
    /// Base Repository
    /// </summary>
    public partial class DapperRepository<TEntity>
        where TEntity : class
    {
        public Task<int> DeleteByIdsAsync(string ids)
        {
            var queryResult = SqlGenerator.GetDeleteByIds(ids);
            return Connection.ExecuteAsync(queryResult.GetSql(), queryResult.Param);
        }

        public Task<IEnumerable<TEntity>> FindByIdsAsync(string ids)
        {
            var queryResult = SqlGenerator.GetSelectByIds(ids);
            return Connection.QueryAsync<TEntity>(queryResult.GetSql(), queryResult.Param);
        }

        /// <summary>
        /// 使用SqlBulkCopy批量进行插入数据
        /// </summary>
        /// <typeparam name="T">实体对象</typeparam>
        /// <param name="dbs"></param>
        /// <param name="entitys">实体对象集合</param>
        public virtual int BulkCopyInsert<T>(IList<T> instances) where T : new()
        {
            int result = 1;
            using (var destinationConnection = (SqlConnection)Connection)
            {
                using (var bulkCopy = new SqlBulkCopy(destinationConnection))
                {
                    Type type = instances[0].GetType();
                    object classAttr = type.GetCustomAttributes(false)[0];
                    if (classAttr is TableAttribute)
                    {
                        TableAttribute tableAttr = classAttr as TableAttribute;
                        bulkCopy.DestinationTableName = tableAttr.Name; //要插入的表的表明 
                    }
                    ModelHandler<T> mh = new ModelHandler<T>();
                    DataTable dt = mh.FillDataTable(instances);
                    if (dt != null && dt.Rows.Count != 0)
                    {
                        bulkCopy.WriteToServer(dt);
                    }
                }
            }
            return result;
        }
    }
}
