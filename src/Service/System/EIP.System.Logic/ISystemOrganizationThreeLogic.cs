using EIP.System.Models.Dtos.OrganizationThree;

namespace EIP.System.Logic
{
    /// <summary>
    /// 
    /// </summary>   
    public interface ISystemOrganizationThreeLogic : IAsyncLogic<SystemOrganizationThree>
    {
        /// <summary>
        /// 获取组织架构
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemOrganizationThreeLogic_Cache")]
        OperateStatus<List<BaseTree>> Tree(int type);

        /// <summary>
        /// 查询组织
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemOrganizationThreeLogic_Cache")]
        Task<OperateStatus<PagedResults<SystemOrganizationThree>>> Find(SystemOrganizationThreePagingInput input);

        /// <summary>
        /// 同步组织到系统
        /// </summary>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemOrganizationThreeLogic_Cache")]
        Task<OperateStatus> AsyncToSystem(int type);
      
        #region 钉钉
        /// <summary>
        /// 同步获取钉钉组织架构
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemOrganizationThreeLogic_Cache")]
        Task<OperateStatus> DingTalkAsync();
        #endregion

        #region 企业微信
        /// <summary>
        /// 同步获取企业微信组织架构
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemOrganizationThreeLogic_Cache")]
        Task<OperateStatus> WeChatWorkAsync();

        #endregion
    }
}