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

using EIP.System.Models.Dtos.OrganizationThree;

namespace EIP.System.Repository.Impl
{
    /// <summary>
    /// ��֯�ܹ�
    /// </summary>
    public class SystemOrganizationThreeRepository : ISystemOrganizationThreeRepository
    {
        /// <summary>
        /// ���Ӳ�ѯ��ҳ��ʽ
        /// </summary>
        /// <param name="input">��ѯ����</param>
        /// <returns>��ҳ</returns>
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