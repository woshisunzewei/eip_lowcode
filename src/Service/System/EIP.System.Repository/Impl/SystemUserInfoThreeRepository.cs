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

using EIP.System.Models.Dtos.UserThree;

namespace EIP.System.Repository.Impl
{
    /// <summary>
    /// ϵͳ��Ա����
    /// </summary>
    public class SystemUserInfoThreeRepository : ISystemUserInfoThreeRepository
    {
        /// <summary>
        /// ���Ӳ�ѯ��ҳ��ʽ
        /// </summary>
        /// <param name="input">��ѯ����</param>
        /// <returns>��ҳ</returns>
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