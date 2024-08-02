/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2023/7/1 16:35:32
* 文件名: SystemDatasourceRepository
* 描述: 数据源管理数据访问接口实现
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.System.Models.Dtos.Datasource;

namespace EIP.System.Repository.Impl
{
    /// <summary>
    /// 数据源管理数据访问接口实现
    /// </summary>
    public class SystemDatasourceRepository : ISystemDatasourceRepository
    {
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResults<SystemDatasourceFindOutput>> Find(SystemDatasourceFindInput input)
        {
            string likeSql = "";
            //判断数据库类型
            var connectionType = ConfigurationUtil.GetDbConnectionType();
            switch (connectionType)
            {
                case ResourceDataBaseType.Mysql:
                    likeSql = "(select count(*)DataSourceNumber from system_agile where PublicJson like CONCAT('%', datasource.DataSourceId, '%')) DataSourceNumber";
                    break;
                default:
                    likeSql = "(select count(*)DataSourceNumber from system_agile where PublicJson like '%'+CONVERT(varchar(1024), datasource.DataSourceId)+'%')DataSourceNumber";
                    break;
            }
            var sql = new StringBuilder($@"select *,@rowNumber, @recordCount from (SELECT  
                                                  datasource.Id,
                                                  datasource.DataSourceId,
                                                  datasource.Name,
                                                  datasource.Config,
                                                  datasource.Type,
                                                  datasource.DataBaseId,
                                                  db.Name DataBaseName,
                                                  datasource.OrderNo,
                                                  datasource.IsFreeze,
                                                  datasource.Remark,
                                                  datasource.CreateTime,
                                                  datasource.CreateUserName,
                                                  datasource.UpdateTime,
                                                  datasource.UpdateUserName,
												  {likeSql}
                                                  FROM system_datasource datasource left join system_database db on datasource.DataBaseId=db.DataBaseId)a  @where ");

            if (input.Sidx.IsNullOrEmpty())
            {
                input.Sidx = " Id ";
            }
            return new SqlMapperUtil().PagingQuerySqlAsync<SystemDatasourceFindOutput>(sql.ToString(), input);
        }
    }
}