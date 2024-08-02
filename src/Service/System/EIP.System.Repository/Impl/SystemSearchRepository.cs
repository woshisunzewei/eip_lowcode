/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/7/9 22:28:57
* 文件名: SystemSearchRepository
* 描述: 数据访问接口实现
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.System.Models.Dtos.Search;

namespace EIP.System.Repository.Impl
{
    /// <summary>
    /// 数据访问接口实现
    /// </summary>
    public class SystemSearchRepository :ISystemSearchRepository
    {
         /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResults<SystemSearchFindOutput>> Find(SystemSearchFindInput input)
        {
            var sql = new StringBuilder($@"SELECT  
                                                  SearchId,
                                                  Name,
                                                  Config,
                                                  CreateTime,
                                                   @rowNumber, @recordCount FROM system_search @where ");
            if (input.UserId.HasValue)
            {
                sql.Append($" and CreateUserId='{input.UserId}' ");
            }
            if (input.MenuId.HasValue)
            {
                sql.Append($" and MenuId='{input.MenuId}' ");
            }
            if (input.Sidx.IsNullOrEmpty())
            {
                input.Sidx = " Id ";
            }
            return new SqlMapperUtil().PagingQuerySqlAsync<SystemSearchFindOutput>(sql.ToString(), input);
        }
    }
}