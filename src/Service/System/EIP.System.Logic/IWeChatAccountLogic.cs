namespace EIP.WeChat.Logic
{
    /// <summary>
    /// 
    /// </summary>   
    public interface IWeChatAccountLogic : IAsyncLogic<SystemUserInfoThree>
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<OperateStatus> Save(SystemUserInfoThree input);

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
        Task<OperateStatus<SystemUserInfoThree>> FindById(IdInput input);
    }
}