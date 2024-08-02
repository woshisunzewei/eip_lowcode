/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2018/10/30 22:40:15
* 文件名: 
* 描述: 
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using Dm;
using EIP.Common.Models.Resx;
using EIP.Common.Core.Util;
using Kdbndp;
using MySqlConnector;
using Npgsql;
using System;
using System.Data.SqlClient;
using EIP.Common.Core.Extension;

namespace EIP.Base.Repository.Fixture
{
    /// <summary>
    /// 
    /// </summary>
    public class SqlDatabaseFixture : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        public SqlDatabaseFixture(SqlDatabaseFixtureInput input = null)
        {
            string connectionString = ConfigurationUtil.GetDbConnectionString();
            string connectionType = ConfigurationUtil.GetDbConnectionType();
            //若有传入连接字符串,则以传入为准
            if (input != null)
            {
                if (input.ConnectionString.IsNotNullOrEmpty())
                {
                    connectionString = input.ConnectionString;
                }
                if (input.ConnectionType.IsNotNullOrEmpty())
                {
                    connectionType = input.ConnectionType;
                }
            }
            //if (read)
            //{
            //    //得到读连接字符串，此处还需加入负载算法
            //    connectionString = ConfigurationUtil.GetSection<ConnectionString>("EIP").SlaveConnectionString[0].ConnectionString;
            //}
            //else
            //{
            //}
            
            switch (connectionType)
            {
                case ResourceDataBaseType.Mysql:
                    Db = new SqlDbContext(new MySqlConnection(connectionString));
                    break;
                case ResourceDataBaseType.Dm:
                    Db = new SqlDbContext(new DmConnection(connectionString));
                    break;
                case ResourceDataBaseType.Kingbase:
                    Db = new SqlDbContext(new KdbndpConnection(connectionString));
                    break;
                case ResourceDataBaseType.Postgresql:
                    Db = new SqlDbContext(new NpgsqlConnection(connectionString));
                    break;
                default://mssql
                    Db = new SqlDbContext(new SqlConnection(connectionString));
                    break;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public SqlDbContext Db { get; }

        /// <summary>
        /// 
        /// </summary>
        public void Dispose()
        {
            Db.Dispose();
        }
    }

    public class SqlDatabaseFixtureInput()
    {
        public string ConnectionString { get; set; }
        public string ConnectionType { get; set; }
        public bool Read { get; set; }
    }
}