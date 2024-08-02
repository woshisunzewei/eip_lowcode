using EIP.System.Models.Dtos.UserThree;


namespace EIP.System.Logic
{
    /// <summary>
    /// 
    /// </summary>   
    public interface ISystemUserInfoThreeLogic : IAsyncLogic<SystemUserInfoThree>
    {
        #region 钉钉
        /// <summary>
        /// 同步人员
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemUserThreeLogic_Cache")]
        Task<OperateStatus> DingTalkAsync();
        #endregion

        #region 企业微信
        /// <summary>
        /// 同步人员
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        OperateStatus WeChatWorkAsync();

        /// <summary>
        /// 同步人员根据标签
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        OperateStatus<GetTagMemberResult> WeChatWorkAsyncTag(int tagId);

        /// <summary>
        /// 获取所有标签
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        OperateStatus<GetTagListResult> WeChatWorkTagList();
        #endregion
       

        /// <summary>
        /// 绑定用户(已在系统中导入用户)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemUserThreeLogic_Cache")]
        OperateStatus Bind();

        /// <summary>
        /// 同步人员
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemUserThreeLogic_Cache")]
        Task<OperateStatus> AsyncToSystem(int type);

        /// <summary>
        /// 同步人员
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemUserThreeLogic_Cache")]
        Task<OperateStatus> TransferToSystem(SystemUserThreeToSystemInput input);

        /// <summary>
        /// 同步人员
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemUserThreeLogic_Cache")]
        Task<OperateStatus> BindUserId(UserThreeBindInput input);

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="input">分页参数</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemUserThreeLogic_Cache")]
        Task<OperateStatus<PagedResults<UserThreePagingOutput>>> Find(UserThreePagingInput input);

    }
}