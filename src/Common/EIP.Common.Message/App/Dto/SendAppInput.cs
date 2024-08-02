using System;

namespace EIP.Common.Message.Email.Dto
{
    /// <summary>
    /// 发送App信息实体
    /// </summary>
    public class SendAppInput : MessageBaseInput
    {
        /// <summary>
        /// 主题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Url { get; set; }
    }

    /// <summary>
    /// 发送App信息实体
    /// </summary>
    public class SendUserAppPushInput
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid UserAppPushId { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ClientId { get; set; }
    }
}