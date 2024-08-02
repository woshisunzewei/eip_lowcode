using EIP.System.Models.Dtos.Message;

namespace EIP.System.Logic
{
    /// <summary>
    /// 消息阅读记录表,记录那些人员什么时候已经查看
    /// </summary>
    public interface ISystemMessageLogReadLogic : IAsyncLogic<SystemMessageLogRead>
    {
        /// <summary>
        /// 保存消息阅读状态
        /// </summary>
        /// <param name="input">消息阅读记录表,记录那些人员什么时候已经查看</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemMessageLogReadLogic_Cache,ISystemMessageLogLogic_Cache")]
        Task<OperateStatus> SaveRead(SystemMessageLogReadSaveInput input);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="input">分页参数</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemMessageLogReadLogic_Cache")]
        Task<OperateStatus<PagedResults<SystemMessageLogReadFindPagingOutput>>> FindPaging(SystemMessageLogReadFindPagingInput input);

        /// <summary>
        /// 删除已阅读信息
        /// </summary>
        /// <param name="input">已阅读Id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemMessageLogReadLogic_Cache")]
        Task<OperateStatus> DeleteHaveRead(SystemMessageLogDeleteHaveReadInput input);
    }
}
