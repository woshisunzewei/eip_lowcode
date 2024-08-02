namespace EIP.Common.Message.DingTalk.Dto
{
    /// <summary>
    /// 企业微信
    /// </summary>
    public class SendWeiXinWorkInput : MessageBaseInput
    {
        /// <summary>
        /// 手机号
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 跳转地址
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// 短信信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
    }
}