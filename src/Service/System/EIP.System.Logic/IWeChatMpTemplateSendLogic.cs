using EIP.Base.Models.Entities.WeChat;
using EIP.System.Models.Dtos.WeChat.MpTemplateSend;

namespace EIP.WeChat.Logic
{
    /// <summary>
    /// 
    /// </summary>   
    public interface IWeChatMpTemplateSendLogic : IAsyncLogic<WeChatMpTemplateSend>
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<OperateStatus> Save(WeChatMpTemplateSend input);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<OperateStatus> Delete(IdInput<string> input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<OperateStatus> Send(IdInput input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<OperateStatus> SendPreview(WechatMpTemplateSendPreviewInput input);
    }
}