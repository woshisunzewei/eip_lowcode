using EIP.System.Models.Dtos.Notice;

namespace EIP.System.Logic
{
    /// <summary>
    /// 公告
    /// </summary>
    public interface ISystemNoticeLogic : IAsyncLogic<SystemNotice>
	{
        /// <summary>
        /// 获取公告信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemNoticeLogic_Cache")]
        Task<OperateStatus<PagedResults<SystemNoticeFindPagingOutput>>> FindPaging(SystemNoticeFindPagingInput input);

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="input">公告</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemNoticeLogic_Cache")]
        Task<OperateStatus> Save(SystemNotice input);

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemNoticeLogic_Cache")]
        Task<OperateStatus<SystemNotice>> FindById(IdInput input);

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="input">新闻</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemNoticeLogic_Cache")]
        Task<OperateStatus> Delete(IdInput<string> input);
    }
}
