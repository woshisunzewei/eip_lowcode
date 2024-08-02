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
using EIP.System.Models.Dtos.WeChat.User;

namespace EIP.WeChat.Repository.Impl
{
    /// <summary>
    /// 微信授权用户
    /// </summary>
    public class WeChatUserRepository : IWeChatUserRepository
    {
        /// <summary>
        /// 获取微信授权用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResults<WeChatUserPagingOutput>> Find(WeChatUserPagingInput input)
        {
            var sql = new StringBuilder(
                   "SELECT u.*,userinfo.Name UserName,@rowNumber, @recordCount from wechat_user u " +
                   " left join system_userinfo userinfo on userinfo.UserId=u.UserId" +
                   " LEFT JOIN System_Organization org on org.OrganizationId = userInfo.OrganizationId @where ");
            if (input.DataSql.IsNotNullOrEmpty())
            {
                sql.Append(@" AND " + input.DataSql + "");
            }
            if (input.Sidx.IsNullOrEmpty())
            {
                input.Sidx = " u.Id ";
            }
            return new SqlMapperUtil().PagingQuerySqlAsync<WeChatUserPagingOutput>(sql.ToString(), input);
        }
    }
}