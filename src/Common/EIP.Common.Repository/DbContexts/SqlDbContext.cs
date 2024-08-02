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

using EIP.Common.Models.Resx;
using EIP.Common.Core.Util;
using EIP.Common.Repository.MicroOrm;
using EIP.Common.Repository.MicroOrm.DbContext;
using EIP.Common.Repository.MicroOrm.SqlGenerator;
using System.Data;

namespace EIP.Common.Repository.DbContexts
{
    public class SqlDbContext<TEntity> : DapperDbContext, IDbContext<TEntity> where TEntity : class
    {
        public SqlDbContext(IDbConnection dbConnection) :
            base(dbConnection)
        {
        }

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

        private IDapperRepository<TEntity> _repository;

        public IDapperRepository<TEntity> Repository => _repository ??= new DapperRepository<TEntity>(Connection, new SqlGenerator<TEntity>(GetSqlProvider()));

    }
}