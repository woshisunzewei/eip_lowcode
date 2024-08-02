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
namespace EIP.System.Repository.Impl
{
    /// <summary>
    /// ��֯����
    /// </summary>
    public class SystemOrganizationRepository : ISystemOrganizationRepository
    {
        /// <summary>
        /// ��ȡ����Ȩ��
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<IEnumerable<BaseTree>> FindDataPermissionTree(SystemOrganizationDataPermissionInput input)
        {
            var sql = new StringBuilder();
            sql.Append($"SELECT OrganizationId id,ParentId parent,Name text,ParentIds parents,Nature icon FROM System_Organization org WHERE 1=1 AND {input.DataSql} ORDER BY OrderNo");
            return new SqlMapperUtil().SqlWithParams<BaseTree>(sql.ToString());
        }

        /// <summary>
        /// ���ݸ�����ѯ�¼�
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<IEnumerable<SystemOrganizationOutput>> FindByParentId(SystemOrganizationDataPermissionInput input)
        {
            var sql = new StringBuilder();
            sql.Append("select * from System_Organization org where 1=1 ");
            if (input.DataSql.IsNotNullOrEmpty())
            {
                sql.Append("AND " + input.DataSql);
            }
            sql.Append(input.Sql);
            return new SqlMapperUtil().SqlWithParams<SystemOrganizationOutput>(sql.ToString());
        }
    }
}