namespace EIP.Common.Message.WebSite.Dto
{
    /// <summary>
    /// 发送站内消息
    /// </summary>
    public class SendWebSiteInput : MessageBaseInput
    {
        /// <summary>
        /// 消息内容
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 消息标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 打开方式
        /// </summary>
        public short OpenType { get; set; }

        /// <summary>
        /// 打开地址
        /// </summary>
        public string ReturnUrl { get; set; }


    }
}