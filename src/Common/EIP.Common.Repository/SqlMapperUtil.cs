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
using Dapper;
using Dm;
using EIP.Common.Core.Extension;
using EIP.Common.Core.Util;
using EIP.Common.Models.Paging;
using EIP.Common.Models.Resx;
using EIP.Common.Repository.MicroOrm;
using Kdbndp;
using MySqlConnector;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIP.Common.Repository
{
    public class SqlMapperUtil
    {
        public string ConnectionName = "EIP";
        public string ConnectionType = string.Empty;
        public string ConnectionString = string.Empty;
        public SqlMapperUtil()
        { }

        public SqlMapperUtil(string connectionName, string connectionType, string connectionString)
        {
            ConnectionName = connectionName;
            ConnectionType = connectionType;
            ConnectionString = connectionString;
        }

        public SqlMapperUtil(string connectionName, string connectionType) :
            this(connectionName, connectionType, string.Empty)
        {
            ConnectionName = connectionName;
            ConnectionType = connectionType;
        }

        public SqlMapperUtil(string connectionName) :
            this(connectionName, string.Empty)
        {

        }

        /// <summary>
        /// 读取数据库连接
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="read">是否为读操作,支持读写分离,若是读操作则使用算法得到读的连接字符串</param>
        /// <returns></returns>
        private IDbConnection GetConnectoin<T>(bool read = true)
        {
            string connectionString = ConnectionString.IsNullOrEmpty() ? ConfigurationUtil.GetDbConnectionString() : ConnectionString;
            string connectionType = ConnectionType.IsNullOrEmpty() ? ConfigurationUtil.GetDbConnectionType() : ConnectionType;
            DbConnection connection;
            switch (connectionType)
            {
                case ResourceDataBaseType.Mysql:
                    connection = new MySqlConnection(connectionString);
                    break;
                case ResourceDataBaseType.Postgresql:
                    connection = new NpgsqlConnection(connectionString);
                    break;
                case ResourceDataBaseType.Dm:
                    connection = new DmConnection(connectionString);
                    break;
                case ResourceDataBaseType.Kingbase:
                    connection = new KdbndpConnection(connectionString);
                    break;
                default://mssql
                    connection = new SqlConnection(connectionString);
                    break;
            }
            if (connection.State != ConnectionState.Open)
                connection.Open();
            return connection;
        }

        #region 分页

        /// <summary>
        /// 复杂查询分页
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="querySql">查询语句</param>
        /// <param name="queryParam">查询参数</param>
        /// <param name="parameters"></param>
        /// <returns>分页结果</returns>
        /// <remarks>
        /// 注意事项：
        /// 1.sql语句中需要加上@where、@orderBy、@rowNumber、@recordCount标记
        /// 如: "select *, @rowNumber, @recordCount from ADM_Rule @where"
        /// 2.实体中需增加扩展属性，作记录总数输出：RecordCount
        /// 3.标记解释:
        /// @where：      查询条件
        /// @orderBy：    排序
        /// @x：          分页记录起点
        /// @y：          分页记录终点
        /// @recordCount：记录总数
        /// @rowNumber：  行号
        /// 4.示例参考:
        /// </remarks>
        public Task<PagedResults<T>> PagingQuerySqlAsync<T>(string querySql, QueryParam queryParam, DynamicParameters parameters = null)
        {
            var connectionType = ConnectionType.IsNullOrEmpty() ? ConfigurationUtil.GetDbConnectionType() : ConnectionType;
            switch (connectionType)
            {
                case ResourceDataBaseType.Mysql:
                    return PagingQueryMySqlAsync<T>(querySql, queryParam, parameters);
                case ResourceDataBaseType.Dm:
                    return PagingQueryDmAsync<T>(querySql, queryParam, parameters);
                case ResourceDataBaseType.Postgresql:
                    break;
            }
            return PagingQuerySqlServerAsync<T>(querySql, queryParam, parameters);
        }

        /// <summary>
        /// MySql分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="querySql"></param>
        /// <param name="queryParam"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private Task<PagedResults<T>> PagingQueryMySqlAsync<T>(string querySql, QueryParam queryParam, DynamicParameters parameters = null)
        {
            var sql = queryParam.IsReport ?
                  $@"select * from ({querySql}) seq "
                  : $@"select * from ({querySql}) seq where seq.rownum between @x and @y";
            var currentPage = queryParam.Current; //当前页号
            var pageSize = queryParam.Size; //每页记录数
            var lower = ((currentPage - 1) * pageSize) + 1; //记录起点
            var upper = currentPage * pageSize; //记录终点
            if (parameters == null)
            {
                parameters = new DynamicParameters();
            }
            parameters.Add("x", lower);
            parameters.Add("y", upper);
            var orderString = $"{queryParam.Sidx} {queryParam.Sord}";
            var recordCount = sql.Split("@recordCount");
            var selectSql = recordCount[0].Trim().TrimEnd(',');
            sql = selectSql + " " + recordCount[1];
            sql = sql.Replace("@rowNumber", " row_number() over (order by @orderBy) as rownum ")
                .Replace("@orderBy", orderString)
                .Replace("@where", " WHERE 1=1 " + (string.IsNullOrWhiteSpace(queryParam.Sql) ? string.Empty : queryParam.Sql));

            var querySqlRecordCount = querySql.Split("@recordCount");
            var recordCountSql = querySql.Replace(querySqlRecordCount[0], ";select count(*) ").Replace("@recordCount", "  ").Replace("@where", " WHERE 1=1 " + (string.IsNullOrWhiteSpace(queryParam.Sql) ? string.Empty : queryParam.Sql));
            //总数量
            sql += recordCountSql;
            using (IDbConnection db = GetConnectoin<T>())
            {
                var queryMulti = db.QueryMultiple(sql, parameters);
                var data = queryMulti.Read<T>().ToList();
                var pagerInfo = new PagerInfo();
                pagerInfo.RecordCount = queryMulti.Read<long>().Sum();
                return Task.Factory.StartNew(() => new PagedResults<T> { Data = data, PagerInfo = pagerInfo });
            }
        }

        /// <summary>
        /// 达梦分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="querySql"></param>
        /// <param name="queryParam"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private Task<PagedResults<T>> PagingQueryDmAsync<T>(string querySql, QueryParam queryParam, DynamicParameters parameters = null)
        {
            var sql = queryParam.IsReport ?
                  $@"select * from ({querySql}) seq "
                  : $@"select * from ({querySql}) seq where seq.row_num between :x and :y";
            var currentPage = queryParam.Current; //当前页号
            var pageSize = queryParam.Size; //每页记录数
            var lower = ((currentPage - 1) * pageSize) + 1; //记录起点
            var upper = currentPage * pageSize; //记录终点
            if (parameters == null)
            {
                parameters = new DynamicParameters();
            }
            parameters.Add("x", lower);
            parameters.Add("y", upper);
            var orderString = $"{queryParam.Sidx} {queryParam.Sord}";
            var recordCount = sql.Split("@recordCount");
            var selectSql = recordCount[0].Trim().TrimEnd(',');
            sql = selectSql + " " + recordCount[1];
            sql = sql.Replace("@rowNumber", " row_number() over (order by @orderBy) as row_num ")
                .Replace("@orderBy", orderString)
                .Replace("@where", " WHERE 1=1 " + (string.IsNullOrWhiteSpace(queryParam.Sql) ? string.Empty : queryParam.Sql));

            var querySqlRecordCount = querySql.Split("@recordCount");
            var recordCountSql = querySql.Replace(querySqlRecordCount[0], ";select count(*) ").Replace("@recordCount", "  ").Replace("@where", " WHERE 1=1 " + (string.IsNullOrWhiteSpace(queryParam.Sql) ? string.Empty : queryParam.Sql));
            //总数量
            sql += recordCountSql;
            using (IDbConnection db = GetConnectoin<T>())
            {
                var queryMulti = db.QueryMultiple(sql, parameters);
                var data = queryMulti.Read<T>().ToList();
                var pagerInfo = new PagerInfo();
                pagerInfo.RecordCount = queryMulti.Read<long>().Sum();
                return Task.Factory.StartNew(() => new PagedResults<T> { Data = data, PagerInfo = pagerInfo });
            }
        }


        /// <summary>
        /// SqlServer分页
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="querySql"></param>
        /// <param name="queryParam"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private Task<PagedResults<T>> PagingQuerySqlServerAsync<T>(string querySql, QueryParam queryParam, DynamicParameters parameters = null)
        {
            var sql = queryParam.IsReport ?
                $@"select * from ({querySql}) seq "
                : $@"select * from ({querySql}) seq where seq.rownum between @x and @y";
            var currentPage = queryParam.Current; //当前页号
            var pageSize = queryParam.Size; //每页记录数
            var lower = ((currentPage - 1) * pageSize) + 1; //记录起点
            var upper = currentPage * pageSize; //记录终点
            if (parameters == null)
            {
                parameters = new DynamicParameters();
            }
            parameters.Add("x", lower);
            parameters.Add("y", upper);
            var orderString = $"{queryParam.Sidx} {queryParam.Sord}";
            var recordCount = sql.Split("@recordCount");
            var selectSql = recordCount[0].Trim().TrimEnd(',');
            sql = selectSql + " " + recordCount[1];
            sql = sql.Replace("@rowNumber", " row_number() over (order by @orderBy) as rownum ")
                .Replace("@orderBy", orderString)
                .Replace("@where", " WHERE 1=1 " + (string.IsNullOrWhiteSpace(queryParam.Sql) ? string.Empty : queryParam.Sql));

            var querySqlRecordCount = querySql.Split("@recordCount");
            var recordCountSql = querySql.Replace(querySqlRecordCount[0], ";select count(*) ").Replace("@recordCount", "  ").Replace("@where", " WHERE 1=1 " + (string.IsNullOrWhiteSpace(queryParam.Sql) ? string.Empty : queryParam.Sql));
            //总数量
            sql += recordCountSql;
            using (IDbConnection db = GetConnectoin<T>())
            {
                var queryMulti = db.QueryMultiple(sql, parameters);
                var data = queryMulti.Read<T>().ToList();
                var pagerInfo = new PagerInfo();
                pagerInfo.RecordCount = queryMulti.Read<long>().Sum();
                return Task.Factory.StartNew(() => new PagedResults<T> { Data = data, PagerInfo = pagerInfo });
            }
        }


        /// <summary>
        /// 复杂查询分页
        /// </summary>
        /// <typeparam name="T">类型</typeparam>
        /// <param name="queryParam">查询参数</param>
        /// <returns>分页结果</returns>
        /// <remarks>
        /// 注意事项：
        /// 1.sql语句中需要加上@where、@orderBy、@rowNumber、@recordCount标记
        /// 如: "select *, @rowNumber, @recordCount from ADM_Rule @where"
        /// 2.实体中需增加扩展属性，作记录总数输出：RecordCount
        /// 3.标记解释:
        /// @where：      查询条件
        /// @orderBy：    排序
        /// @x：          分页记录起点
        /// @y：          分页记录终点
        /// @recordCount：记录总数
        /// @rowNumber：  行号
        /// 4.示例参考:
        /// </remarks>
        public Task<PagedResults<T>> PagingQuerySqlSingleTableAsync<T>(QueryParam queryParam) where T : class
        {
            using (IDbConnection connection = GetConnectoin<T>())
            {
                var repository = new DapperRepository<T>(connection);
                var parameters = new DynamicParameters();
                var querySql = "select *,@rowNumber,@recordCount from " + repository.SqlGenerator.TableName + " @where ";
                var sql = queryParam.IsReport ?
                    $@"select * from ({querySql}) seq "
                    : $@"select * from ({querySql}) seq where seq.rownum between @x and @y";
                var currentPage = queryParam.Current; //当前页号
                var pageSize = queryParam.Size; //每页记录数
                var lower = (currentPage - 1) * pageSize + 1; //记录起点
                var upper = currentPage * pageSize; //记录终点
                parameters.Add("x", lower);
                parameters.Add("y", upper);

                var orderString = $"{queryParam.Sidx} {queryParam.Sord}";
                var recordCount = sql.Split("@recordCount");
                var selectSql = recordCount[0].Trim().TrimEnd(',');
                sql = selectSql + " " + recordCount[1];
                sql = sql.Replace("@rowNumber", " row_number() over (order by @orderBy) as rownum ")
                    .Replace("@orderBy", orderString)
                    .Replace("@where", " WHERE 1=1 " + (string.IsNullOrWhiteSpace(queryParam.Sql) ? string.Empty : queryParam.Sql));

                var querySqlRecordCount = querySql.Split("@recordCount");
                var recordCountSql = querySql.Replace(querySqlRecordCount[0], ";select count(*) ").Replace("@recordCount", "  ").Replace("@where", " WHERE 1=1 " + (string.IsNullOrWhiteSpace(queryParam.Sql) ? string.Empty : queryParam.Sql));
                //总数量
                sql += recordCountSql;
                using (IDbConnection db = GetConnectoin<T>())
                {
                    var queryMulti = db.QueryMultiple(sql, parameters);
                    var data = queryMulti.Read<T>().ToList();
                    var pagerInfo = new PagerInfo();
                    pagerInfo.RecordCount = queryMulti.Read<long>().Sum();
                    return Task.Factory.StartNew(() => new PagedResults<T> { Data = data, PagerInfo = pagerInfo });
                }
            }
        }

        /// <summary>
        /// Pg分页, 
        /// </summary>
        /// <param name="queryParam"></param>
        /// <param name="connection"></param>
        /// <returns></returns>
        private Task<PagedResults<T>> PagingQueryProcSingleTablePngSqlAsync<T>(QueryParam queryParam, IDbConnection connection) where T : class
        {
            try
            {
                var repository = new DapperRepository<T>(connection);
                StringBuilder stringBuilder = new StringBuilder();
                if (queryParam.Fields.IsNotNullOrEmpty())
                {
                    queryParam.Fields = " * " + queryParam.Fields;
                }
                if (queryParam.Filters.IsNotNullOrEmpty())
                {
                    queryParam.Fields = " WHERE 1=1 " + queryParam.Fields;
                }
                if (queryParam.Sidx.IsNotNullOrEmpty())
                {
                    queryParam.Sidx = " ORDER BY " + queryParam.Sidx;
                }
                if (queryParam.Group.IsNotNullOrEmpty())
                {
                    queryParam.Group = " GROUP BY " + queryParam.Group;
                }

                stringBuilder.Append($" SELECT {queryParam.Fields} FROM {repository.SqlGenerator.TableName} {queryParam.Fields} {queryParam.Sidx} {queryParam.Group} LIMIT {queryParam.Size} offset {queryParam.Current - 1 * queryParam.Size}");
                var data = connection.QueryAsync<T>(stringBuilder.ToString());
                var num = ($" SELECT {queryParam.Fields} FROM {repository.SqlGenerator.TableName} {queryParam.Fields} {queryParam.Sidx} {queryParam.Group} LIMIT {queryParam.Size} offset {queryParam.Current - 1 * queryParam.Size}");
                return null;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// 单表存储过程分页
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="queryParam">分页参数</param>
        /// <returns>返回值</returns>
        public Task<PagedResults<T>> PagingQueryProcAsync<T>(QueryParam queryParam)
        {
            try
            {
                using (IDbConnection connection = GetConnectoin<T>())
                {
                    var parms = new DynamicParameters();
                    parms.Add("TableName", queryParam.TableName);
                    parms.Add("Sidx", queryParam.Sidx);//排序字段
                    parms.Add("Field", queryParam.Fields.IsNullOrEmpty() ? "*" : queryParam.Fields);
                    parms.Add("Filters", queryParam.Sql + queryParam.Where);
                    parms.Add("PageIndex", queryParam.Current);
                    parms.Add("PageSize", queryParam.Size);
                    parms.Add("GroupBy", queryParam.Group.IsNullOrEmpty() ? "" : queryParam.Group);
                    parms.Add("Sort", queryParam.Sidx + " " + queryParam.Sord);
                    parms.Add("RecordCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
                    var pagerInfo = new PagerInfo();
                    var data = StoredProcWithParams<T>("System_Proc_Paging", parms).Result.ToList();
                    if (data.Any())
                    {
                        pagerInfo.RecordCount = parms.Get<int>("RecordCount");
                    }
                    return Task.Factory.StartNew(() => new PagedResults<T>
                    {
                        Data = data,
                        PagerInfo = pagerInfo
                    });
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        #endregion

        #region 查询
        /// <summary>
        /// 执行Sql语句带参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> SqlWithParams<T>(string sql, dynamic parms = null)
        {
            using (IDbConnection db = GetConnectoin<T>())
            {
                var connectionType = ConnectionType.IsNullOrEmpty() ? ConfigurationUtil.GetDbConnectionType() : ConnectionType;
                switch (connectionType)
                {
                    case ResourceDataBaseType.Dm:
                    case ResourceDataBaseType.Kingbase:
                        sql = sql.Replace("@", ":");
                        break;
                }
                return await db.QueryAsync<T>(sql, (object)parms);
            }
        }
        /// <summary>
        /// 执行Sql语句带参数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public async Task<SqlMapper.GridReader> SqlWithParamsQueryMultiple<T>(string sql, dynamic parms = null)
        {
            using (IDbConnection db = GetConnectoin<T>())
            {
                return await db.QueryMultipleAsync(sql, (object)parms);
            }
        }
        /// <summary>
        /// 执行语句返回bool
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public async Task<bool> SqlWithParamsBool<T>(string sql, dynamic parms = null)
        {
            var connectionType = ConnectionType.IsNullOrEmpty() ? ConfigurationUtil.GetDbConnectionType() : ConnectionType;
            switch (connectionType)
            {
                case ResourceDataBaseType.Dm:
                case ResourceDataBaseType.Kingbase:
                    sql = sql.Replace("@", ":");
                    break;
            }
            using (IDbConnection db = GetConnectoin<T>())
            {
                return (await db.QueryAsync<T>(sql, (object)parms)).Any();
            }
        }

        /// <summary>
        /// 执行语句返回第一个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public async Task<T> SqlWithParamsSingle<T>(string sql, dynamic parms = null)
        {
            var connectionType = ConnectionType.IsNullOrEmpty() ? ConfigurationUtil.GetDbConnectionType() : ConnectionType;
            switch (connectionType)
            {
                case ResourceDataBaseType.Dm:
                case ResourceDataBaseType.Kingbase:
                    sql = sql.Replace("@", ":");
                    break;
            }
            using (IDbConnection db = GetConnectoin<T>())
            {
                return await db.QueryFirstOrDefaultAsync<T>(sql, (object)parms);
            }
        }

        #endregion

        #region 增加删除修改

        /// <summary>
        /// 执行增加删除修改语句
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <param name="parms">参数信息</param>
        /// <param name="isSetConnectionStr">是否需要重置连接字符串</param>
        /// <returns>影响数</returns>
        public async Task<int> InsertUpdateOrDeleteSql<T>(string sql, dynamic parms = null, bool isSetConnectionStr = true)
        {
            using (var db = GetConnectoin<T>(false))
            {
                return await db.ExecuteAsync(sql, (object)parms);
            }
        }

        /// <summary>
        /// 执行增加删除修改语句
        /// </summary>
        /// <param name="sql">Sql语句</param>
        /// <param name="parms">参数信息</param>
        /// <param name="isSetConnectionStr">是否需要重置连接字符串</param>
        /// <returns>影响数</returns>
        public async Task<bool> InsertUpdateOrDeleteSqlBool<T>(string sql, dynamic parms = null, bool isSetConnectionStr = true)
        {
            using (var db = GetConnectoin<T>(false))
            {
                return await db.ExecuteAsync(sql, (object)parms) > 0;
            }
        }

        #endregion

        #region 存储过程

        /// <summary>
        /// 存储过程查询所有值:同步
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procName"></param>
        /// <param name="parms"></param>
        /// <param name="isSetConnectionStr"></param>
        /// <returns></returns>
        public IEnumerable<T> StoredProcWithParamsSync<T>(string procName, dynamic parms, bool isSetConnectionStr = true)
        {
            using (var db = GetConnectoin<T>())
            {
                return db.Query<T>(procName, (object)parms);
            }
        }

        /// <summary>
        /// 存储过程查询所有值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procName">The procname.</param>
        /// <param name="parms">The parms.</param>
        /// <param name="isSetConnectionStr"></param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> StoredProcWithParams<T>(string procName, dynamic parms, bool isSetConnectionStr = true)
        {
            using (IDbConnection db = GetConnectoin<T>())
            {
                return await db.QueryAsync<T>(procName, (object)parms, commandType: CommandType.StoredProcedure);
            }
        }
        /// <summary>
        /// 存储过程查询所有值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procName">The procname.</param>
        /// <param name="parms">The parms.</param>
        /// <param name="isSetConnectionStr"></param>
        /// <returns></returns>
        public async Task<DataTable> StoredProcWithParamsDataTable<T>(string procName, dynamic parms, bool isSetConnectionStr = true)
        {
            using (IDbConnection db = GetConnectoin<T>())
            {
                var reader = await db.ExecuteReaderAsync(procName, (object)parms, commandType: CommandType.StoredProcedure);
                DataTable table = new DataTable();
                table.Load(reader);
                return table;
            }
        }

        /// <summary>
        /// 增删改查存储过程
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="parms"></param>
        /// <returns></returns>
        public Task<int> InsertUpdateOrDeleteStoredProc<T>(string procName, dynamic parms = null)
        {
            using (var db = GetConnectoin<T>(false))
            {
                return db.ExecuteAsync(procName, (object)parms);
            }
        }

        /// <summary>
        /// 返回存储过程第一个
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="procName"></param>
        /// <param name="parms"></param>
        /// <param name="isSetConnectionStr"></param>
        /// <returns></returns>
        public Task<T> StoredProcWithParamsSingle<T>(string procName, dynamic parms = null,
            bool isSetConnectionStr = true)
        {
            using (var db = GetConnectoin<T>())
            {
                return db.QueryFirstAsync<T>(procName, (object)parms);
            }
        }
        #endregion


        /// <summary>
        /// 获取表单单号信息
        /// </summary>
        /// <param name="code">表单单号简称</param>
        /// <param name="flag">0 获取单号 1获取并更新单号</param>
        /// <returns>单号信息</returns>
        public T GetSystem_SerialNo<T>(string code, int flag = 0)
        {
            try
            {
                using (IDbConnection connection = GetConnectoin<T>())
                {
                    var parms = new DynamicParameters();
                    parms.Add("sCode", code);
                    parms.Add("flag", flag);
                    var list = connection.QueryAsync<T>("GetSystem_SerialNo", parms, commandType: CommandType.StoredProcedure).Result.ToList();
                    return list.FirstOrDefault();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}