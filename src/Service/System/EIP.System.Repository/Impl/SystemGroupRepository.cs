using EIP.System.Models.Dtos.Group;

namespace EIP.System.Repository.Impl
{
    /// <summary>
    /// 组
    /// </summary>
    public class SystemGroupRepository : ISystemGroupRepository
    {
        /// <summary>
        /// 查询归属某组织下的组信息
        /// </summary>
        /// <param name="input">组织机构Id</param>
        /// <returns>组信息</returns>
        public Task<PagedResults<SystemGroupFindOutput>> FindGroupByOrganizationId(SystemGroupFindInput input)
        {
            var sql = new StringBuilder();
            sql.Append(
                @"SELECT gr.*,org.ParentIdsName,@rowNumber, @recordCount 
                         FROM System_Group gr LEFT JOIN System_Organization org ON org.OrganizationId=gr.OrganizationId @where ");
            if (!input.Id.IsNullOrEmptyGuid())
            {
                sql.Append($" AND gr.OrganizationId in(select OrganizationId from System_Organization where ParentIds like '%{input.Id}%')");
            }
            if (input.DataSql.IsNotNullOrEmpty())
            {
                sql.Append(@" AND " + input.DataSql + "");
            }
            if (input.Sidx.IsNullOrEmpty())
            {
                input.Sidx = " gr.OrganizationId";
            }
            return new SqlMapperUtil().PagingQuerySqlAsync<SystemGroupFindOutput>(sql.ToString(),input);
        }
    }
}