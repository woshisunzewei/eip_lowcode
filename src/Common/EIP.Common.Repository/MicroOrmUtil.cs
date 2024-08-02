using Dm;
using EIP.Common.Models.Attributes;
using EIP.Common.Models.Resx;
using EIP.Common.Repository.DbContexts;
using EIP.Common.Core.Util;
using Kdbndp;
using MySqlConnector;
using Npgsql;
using System;
using System.Data.SqlClient;
using System.Reflection;

namespace EIP.Common.Repository.MicroOrm
{
    public class MicroOrmUtil<TEntity> : IDisposable where TEntity : class
    {
        /// <summary>
        /// 
        /// </summary>
        public readonly string ConnectionName = "EIP";
        /// <summary>
        /// 初始化
        /// </summary>
        /// <param name="read">是否为读操作,支持读写分离,若是读操作则使用算法得到读的连接字符串</param>
        public MicroOrmUtil()
        {
            var connectionString = ConfigurationUtil.GetDbConnectionString();
            var connectionType = ConfigurationUtil.GetDbConnectionType();
            switch (connectionType)
            {
                case ResourceDataBaseType.Mysql:
                    Db = new SqlDbContext<TEntity>(new MySqlConnection(connectionString));
                    break;
                case ResourceDataBaseType.Dm:
                    Db = new SqlDbContext<TEntity>(new DmConnection(connectionString));
                    break;
                case ResourceDataBaseType.Kingbase:
                    Db = new SqlDbContext<TEntity>(new KdbndpConnection(connectionString));
                    break;
                case ResourceDataBaseType.Postgresql:
                    Db = new SqlDbContext<TEntity>(new NpgsqlConnection(connectionString));
                    break;
                default://mssql
                    Db = new SqlDbContext<TEntity>(new SqlConnection(connectionString));
                    break;
            }
        }

        public SqlDbContext<TEntity> Db { get; }

        public void Dispose()
        {
            Db?.Dispose();
        }
    }
}