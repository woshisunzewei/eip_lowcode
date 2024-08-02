using EIP.System.Models.Dtos.WeChat.User;

namespace EIP.WeChat.Logic
{
    /// <summary>
    /// 
    /// </summary>   
    public interface IWeChatUserLogic : IAsyncLogic<SystemUserInfoThree>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        Task<OperateStatus<PagedResults<WeChatUserPagingOutput>>> Find(WeChatUserPagingInput input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        Task<OperateStatus> BindUser(WeChatUserBindInput input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<OperateStatus> UnBindUser(IdInput<string> input);
    }
}