/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/2/3 22:14:39
* 文件名: SystemAgileAutomationRepository
* 描述: 自动化构建数据访问接口实现
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
namespace EIP.System.Repository.Impl
{
    /// <summary>
    /// 自动化构建数据访问接口实现
    /// </summary>
    public class SystemAgileAutomationRepository :ISystemAgileAutomationRepository
    {
         /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResults<SystemAgileAutomationFindOutput>> Find(SystemAgileAutomationFindInput input)
        {
            var sql = new StringBuilder($@"select *,  @rowNumber, @recordCount from( SELECT  
                                                  agile.Name TableName,
                                                  automation.TableId,
                                                  automation.ConfigId,
                                                  automation.Name,
                                                  automation.IsFreeze,
                                                  automation.Remark,
                                                  automation.OrderNo,
                                                  automation.ConfigType,
                                                  automation.CreateTime,
                                                  automation.CreateUserName,
                                                  automation.UpdateTime,
                                                  automation.UpdateUserName FROM system_agile_automation automation left join system_agile agile on agile.ConfigId=automation.TableId) a @where ");
            if (input.TableId.HasValue)
            {
                sql.Append($" and TableId='{input.TableId}' ");
            }
            if (input.Sidx.IsNullOrEmpty())
            {
                input.Sidx = " CreateTime ";
            }
            return new SqlMapperUtil().PagingQuerySqlAsync<SystemAgileAutomationFindOutput>(sql.ToString(), input);
        }
    }
}