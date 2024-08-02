/**************************************************************
* Copyright (C) 2022 www.eipflow.com ����ΰ��Ȩ����(����ؾ�)
*
* ����: ����ΰ(QQ 1039318332)
* ����ʱ��: 2022/01/12 22:40:15
* �ļ���: 
* ����: 
* 
* �޸���ʷ
* �޸��ˣ�
* ʱ�䣺
* �޸�˵����
*
**************************************************************/
using EIP.System.Models.Dtos.WeChat.User;

namespace EIP.WeChat.Repository.Impl
{
    /// <summary>
    /// ΢����Ȩ�û�
    /// </summary>
    public class WeChatUserRepository : IWeChatUserRepository
    {
        /// <summary>
        /// ��ȡ΢����Ȩ�û�
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