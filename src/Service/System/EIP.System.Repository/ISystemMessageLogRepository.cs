using EIP.System.Models.Dtos.Message;

namespace EIP.System.Repository
{
    /// <summary>
    /// 消息记录表
    /// </summary>
    public interface ISystemMessageLogRepository
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="input">分页参数</param>
        /// <returns></returns>
        Task<PagedResults<SystemMessageLogFindPagingOutput>> FindPaging(SystemMessageLogFindPagingInput input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResults<SystemMessageLogFindPagingOutput>> FindMyAllPaging(SystemMessageLogFindPagingInput input);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="input">分页参数</param>
        /// <returns></returns>
        Task<PagedResults<SystemMessageLogFindMonitorPagingOutput>> FindMonitorPaging(SystemMessageLogFindMonitorPagingInput input);
    }
}
