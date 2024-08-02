using EIP.System.Models.Dtos.Message;

namespace EIP.System.Repository.Impl
{
    /// <summary>
    /// 消息阅读记录表,记录那些人员什么时候已经查看
    /// </summary>
    public class SystemMessageReadRepository : ISystemMessageLogReadRepository
    {
        /// <summary>
        /// 查询分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResults<SystemMessageLogReadFindPagingOutput>> FindPaging(SystemMessageLogReadFindPagingInput input)
        {
            var sql = new StringBuilder(
                @"select mess.Title,mess.MessageId,messRead.ReadTime,messRead.ReadUserName,messRead.MessageReadId,@rowNumber, @recordCount from System_MessageLog mess
                  left join System_MessageRead messRead on mess.MessageId = messRead.MessageId
                  @where and messRead.DeleteTime is null ");
            if (!input.UserId.IsNullOrEmptyGuid())
            {
                sql.Append($" and messRead.ReadUserId ='{input.UserId}' ");
            }

            if (input.Sidx.IsNullOrEmpty())
            {
                input.Sidx = " mess.CreateTime ";
            }
            return new SqlMapperUtil().PagingQuerySqlAsync<SystemMessageLogReadFindPagingOutput>(sql.ToString(), input);
        }
    }
}
