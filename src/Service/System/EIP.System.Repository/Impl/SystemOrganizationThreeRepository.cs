/**************************************************************
* Copyright (C) 2022 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2022/01/12 22:40:15
* 文件名: 
* 描述: 
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/

using EIP.System.Models.Dtos.OrganizationThree;

namespace EIP.System.Repository.Impl
{
    /// <summary>
    /// 组织架构
    /// </summary>
    public class SystemOrganizationThreeRepository : ISystemOrganizationThreeRepository
    {
        /// <summary>
        /// 复杂查询分页方式
        /// </summary>
        /// <param name="input">查询参数</param>
        /// <returns>分页</returns>
        public Task<PagedResults<SystemOrganizationThree>> Find(SystemOrganizationThreePagingInput input)
        {
            var sql = new StringBuilder($@"SELECT *,@rowNumber, @recordCount  FROM system_organization_three  @where and Type={input.Type} ");

            if (input.Sidx.IsNullOrEmpty())
            {
                input.Sidx = " Id ";
            }
            return new SqlMapperUtil().PagingQuerySqlAsync<SystemOrganizationThree>(sql.ToString(), input);
        }
    }
}