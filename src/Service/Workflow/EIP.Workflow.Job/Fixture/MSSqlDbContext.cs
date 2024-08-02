using EIP.Common.Core.Util;
using EIP.Common.Models.Resx;
using EIP.Common.Repository.MicroOrm;
using EIP.Common.Repository.MicroOrm.DbContext;
using EIP.Common.Repository.MicroOrm.SqlGenerator;
using EIP.Workflow.Job.Entities;
using System.Data;
using System.Data.SqlClient;

namespace EIP.Workflow.Job.Fixture
{
    /// <summary>
    /// 
    /// </summary>
    public class MsSqlDbContext : DapperDbContext, IDbContext
    {
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbConnection"></param>
        public MsSqlDbContext(IDbConnection dbConnection)
            : base(dbConnection) { }

        /// <summary>
        /// 得到驱动类型
        /// </summary>
        /// <returns></returns>
        private static SqlProvider GetSqlProvider()
        {
            switch (ConfigurationUtil.GetDbConnectionType())
            {
                case ResourceDataBaseType.Mysql:
                    return SqlProvider.MySQL;
                case ResourceDataBaseType.Postgresql:
                    return SqlProvider.PostgreSQL;
                case ResourceDataBaseType.Dm:
                    return SqlProvider.Dm;
                case ResourceDataBaseType.Kingbase:
                    return SqlProvider.Kingbase;
                default:
                    return SqlProvider.MSSQL;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectionString"></param>
        public MsSqlDbContext(string connectionString)
            : base(new SqlConnection(connectionString))
        {
        }

        /// <summary>
        /// 测试
        /// </summary>
        private IDapperRepository<FormDemo> _formDemo;
        /// <summary>
        /// 测试
        /// </summary>
        public IDapperRepository<FormDemo> FormDemo => _formDemo ?? (_formDemo = new DapperRepository<FormDemo>(Connection,  new SqlGenerator<FormDemo>(GetSqlProvider())));

    }
}