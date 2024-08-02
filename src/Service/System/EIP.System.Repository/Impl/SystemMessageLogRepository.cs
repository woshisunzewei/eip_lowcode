using EIP.System.Models.Dtos.Message;

namespace EIP.System.Repository.Impl
{
    /// <summary>
    /// 消息记录表
    /// </summary>
    public class SystemMessageLogRepository : ISystemMessageLogRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResults<SystemMessageLogFindMonitorPagingOutput>> FindMonitorPaging(SystemMessageLogFindMonitorPagingInput input)
        {
            DynamicParameters parameters = new DynamicParameters();
            var sql = new StringBuilder(@"select mess.*,@rowNumber, @recordCount  from System_MessageLog mess @where ");
            if (input.Sidx.IsNullOrEmpty())
            {
                input.Sidx = " mess.CreateTime ";
            }
            return new SqlMapperUtil().PagingQuerySqlAsync<SystemMessageLogFindMonitorPagingOutput>(sql.ToString(), input, parameters);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResults<SystemMessageLogFindPagingOutput>> FindMyAllPaging(SystemMessageLogFindPagingInput input)
        {
            DynamicParameters parameters = new DynamicParameters();
            var sql = new StringBuilder(@"select mess.MessageLogId,mess.Message,mess.Title,mess.CreateTime,(select count(1) from System_MessageLogRead re where re.ReadUserId = @userId and re.MessageLogId=mess.MessageLogId) ReadCount,@rowNumber, @recordCount  from System_MessageLog mess @where                   ");
            sql.Append($" and mess.ReceiverId like '%{input.UserId}%'");
            parameters.Add("userId", input.UserId);
            if (input.Sidx.IsNullOrEmpty())
            {
                input.Sidx = " (select count(1) from System_MessageLogRead re where re.ReadUserId = @userId and re.MessageLogId=mess.MessageLogId),CreateTime  ";
            }
            return new SqlMapperUtil().PagingQuerySqlAsync<SystemMessageLogFindPagingOutput>(sql.ToString(), input, parameters);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResults<SystemMessageLogFindPagingOutput>> FindPaging(SystemMessageLogFindPagingInput input)
        {
            var inSql = input.HaveDo ? " in " : " not in ";
            DynamicParameters parameters = new DynamicParameters();
            var sql = new StringBuilder(@"select mess.OpenUrl, mess.MessageLogId,mess.Title,mess.Message,mess.CreateTime,@rowNumber, @recordCount  from System_MessageLog mess @where                   ");
            sql.Append($" and mess.ReceiverId like '%{input.UserId}%' and mess.MessageLogId {inSql} (select MessageLogId from System_MessageLogRead where ReadUserId = @userId)");
            parameters.Add("userId", input.UserId);
            if (input.Sidx.IsNullOrEmpty())
            {
                input.Sidx = " mess.CreateTime ";
            }
            return new SqlMapperUtil().PagingQuerySqlAsync<SystemMessageLogFindPagingOutput>(sql.ToString(), input, parameters);
        }
    }
}
