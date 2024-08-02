using EIP.System.Models.Dtos.Message;

namespace EIP.System.Logic
{
    /// <summary>
    /// 消息记录表
    /// </summary>
    public interface ISystemMessageLogLogic : IAsyncLogic<SystemMessageLog>
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="input">分页参数</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemMessageLogLogic_Cache")]
        Task<OperateStatus<PagedResults<SystemMessageLogFindPagingOutput>>> FindPaging(SystemMessageLogFindPagingInput input);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="input">分页参数</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemMessageLogLogic_Cache")]
        Task<OperateStatus<PagedResults<SystemMessageLogFindMonitorPagingOutput>>> FindMonitorPaging(SystemMessageLogFindMonitorPagingInput input);

        /// <summary>
        /// 获取我的未读集合
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemMessageLogLogic_Cache")]
        Task<OperateStatus<PagedResults<SystemMessageLogFindPagingOutput>>> FindMyAllPaging(SystemMessageLogFindPagingInput input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemMessageLogLogic_Cache")]
        Task<OperateStatus<SystemMessageLog>> FindById(IdInput input);
    }
}
