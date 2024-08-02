namespace EIP.Common.Message.Email.Dto
{
    /// <summary>
    /// 发送邮件实体
    /// </summary>
    public class SendEmailInput : MessageBaseInput
    {
        /// <summary>
        /// 登录账号
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// SMTP服务器
        /// </summary>
        public string Host { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        /// Ssl
        /// </summary>
        public bool Ssl { get; set; }

        /// <summary>
        /// 登录账号
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 登录密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 邮件发送名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 接收者名称
        /// </summary>
        public string ToName { get; set; }

        /// <summary>
        /// 接收者邮箱地址
        /// </summary>
        public string ToAddress { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Message { get; set; }
    }
}