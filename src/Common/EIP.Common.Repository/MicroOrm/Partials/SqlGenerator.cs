using EIP.Common.Core.Extension;
using System.Linq;

namespace EIP.Common.Repository.MicroOrm.SqlGenerator
{
    /// <inheritdoc />
    public partial class SqlGenerator<TEntity> : ISqlGenerator<TEntity> where TEntity : class
    {
        public virtual SqlQuery GetDeleteByIds(string ids)
        {
            var sqlQuery = new SqlQuery();
            sqlQuery.SqlBuilder.Append("DELETE FROM " + TableName + " WHERE " + string.Join(" ", KeySqlProperties.Select(p => p.ColumnName + " IN (" + ids.InSql() + ")")));
            return sqlQuery;
        }

        public virtual SqlQuery GetSelectByIds(string ids)
        {
            var sqlQuery = new SqlQuery();
            sqlQuery.SqlBuilder.Append("SELECT * FROM " + TableName + " WHERE " + string.Join(" ", KeySqlProperties.Select(p => p.ColumnName + " IN (" + ids.InSql() + ")")));
            return sqlQuery;
        }
    }
}