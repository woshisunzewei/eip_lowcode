using EIP.System.Models.Dtos.Post;

namespace EIP.System.Repository.Impl
{
    /// <summary>
    /// ��λ���
    /// </summary>
    public class SystemPostRepository : ISystemPostRepository
    {
        /// <summary>
        /// ��ѯ����ĳ��֯�µĸ�λ��Ϣ
        /// </summary>
        /// <param name="input">��֯����PostId</param>
        /// <returns>��λ��Ϣ</returns>
        public Task<PagedResults<SystemPostFindOutput>> FindPostByOrganizationId(SystemPostFindInput input)
        {
            var sql = new StringBuilder();
            sql.Append(
                @"SELECT post.*,org.ParentIdsName,@rowNumber, @recordCount 
                         FROM System_Post post LEFT JOIN System_Organization org ON org.OrganizationId=post.OrganizationId @where");
            if ( !input.Id.IsNullOrEmptyGuid())
            {
                sql.Append($@" AND post.OrganizationId in(select OrganizationId from System_Organization where ParentIds like '%{input.Id}%') ");
            }
            if (input.DataSql.IsNotNullOrEmpty())
            {
                sql.Append(@" AND " + input.DataSql + "");
            }
            if (input.Sidx.IsNullOrEmpty())
            {
                input.Sidx = " post.OrganizationId";
            }
            return new SqlMapperUtil().PagingQuerySqlAsync<SystemPostFindOutput>(sql.ToString(),input);
        }
    }
}