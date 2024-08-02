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

using EIP.System.Models.Dtos.UserThree;

namespace EIP.System.Repository.Impl
{
    /// <summary>
    /// 系统人员管理
    /// </summary>
    public class SystemUserInfoThreeRepository : ISystemUserInfoThreeRepository
    {
        /// <summary>
        /// 复杂查询分页方式
        /// </summary>
        /// <param name="input">查询参数</param>
        /// <returns>分页</returns>
        public Task<PagedResults<UserThreePagingOutput>> Find(UserThreePagingInput input)
        {
            var sql = new StringBuilder(
                 $"SELECT userinfo_three.*,systemUserInfo.UserId BindUserId,systemUserInfo.HeadImage BindHeadImage,systemUserInfo.Name BindUserName, @rowNumber, @recordCount  FROM system_userinfo_three userinfo_three left join System_UserInfo systemUserInfo on userinfo_three.SystemUserId=systemUserInfo.UserId   @where  and Type={input.Type} ");
            if (input.Id.IsNotNullOrEmpty())
            {
                sql.Append($" and userinfo_three.OrganizationIds like '%{input.Id.Xss().FilterSql()}%' ");
            }
            if (input.Sidx.IsNullOrEmpty())
            {
                input.Sidx = " userinfo_three.Id ";
            }
            return new SqlMapperUtil().PagingQuerySqlAsync<UserThreePagingOutput>(sql.ToString(), input);
        }
    }
}