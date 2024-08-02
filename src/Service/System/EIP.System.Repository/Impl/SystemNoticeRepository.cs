using EIP.System.Models.Dtos.Notice;

namespace EIP.System.Repository.Impl
{
    /// <summary>
    /// 公告
    /// </summary>
    public class SystemNoticeRepository : ISystemNoticeRepository
    {
        /// <summary>
        /// 分页查询公告信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResults<SystemNoticeFindPagingOutput>> FindPaging(SystemNoticeFindPagingInput input)
        {
            var sql = new StringBuilder();
            sql.Append("select *,@rowNumber, @recordCount from System_Notice @where ");

            if (input.Sidx.IsNullOrEmpty())
            {
                input.Sidx = "CreateTime";
            }
            return new SqlMapperUtil().PagingQuerySqlAsync<SystemNoticeFindPagingOutput>(sql.ToString(), input);
        }
    }
}
