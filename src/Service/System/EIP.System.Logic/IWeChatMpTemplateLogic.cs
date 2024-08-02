using EIP.Base.Models.Entities.WeChat;
using EIP.System.Models.Dtos.WeChat.MpTemplate;

namespace EIP.WeChat.Logic
{
    /// <summary>
    /// 
    /// </summary>   
    public interface IWeChatMpTemplateLogic : IAsyncLogic<WeChatMpTemplate>
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<OperateStatus> Save(WeChatMpTemplate input);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<OperateStatus> Delete(IdInput<string> input);

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<OperateStatus<WeChatMpTemplate>> FindById(IdInput input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        Task<OperateStatus<PagedResults<WeChatMpTemplatePagingOutput>>> Find(WeChatMpTemplatePagingInput input);
    }
}