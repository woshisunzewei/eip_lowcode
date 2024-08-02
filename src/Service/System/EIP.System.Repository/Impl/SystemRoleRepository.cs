using EIP.System.Models.Dtos.Role;

namespace EIP.System.Repository.Impl
{
    /// <summary>
    /// ��ɫ
    /// </summary>
    public class SystemRoleRepository :  ISystemRoleRepository
    {
        /// <summary>
        /// ������֯������ȡ��ɫ��Ϣ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResults<SystemRolesFindOutput>> FindPagingRole(SystemRolesFindInput input)
        {
            var sql =
                new StringBuilder(
                    @"SELECT role.*,org.ParentIdsName,@rowNumber, @recordCount  FROM System_Role role
                    LEFT JOIN System_Organization org ON org.OrganizationId=role.OrganizationId @where");
        
            if (!input.Id.IsNullOrEmptyGuid())
            {
                sql.Append($@" AND role.OrganizationId in(select OrganizationId from System_Organization where ParentIds like '%{input.Id}%') ");
            }
            if (input.DataSql.IsNotNullOrEmpty())
            {
                sql.Append(@" AND " + input.DataSql + "");
            }
            if (input.Sidx.IsNullOrEmpty())
            {
                input.Sidx = "role.OrganizationId,role.OrderNo";
            }
            
            return new SqlMapperUtil().PagingQuerySqlAsync<SystemRolesFindOutput>(sql.ToString(),input);
        }
    }
}