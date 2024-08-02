using System.Collections.Generic;

namespace EIP.Common.Message.Sms.Dto
{
    /// <summary>
    /// 短信发送
    /// </summary>
    public class SendSmsInput : MessageBaseInput
    {
        /// <summary>
        /// 短信信息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 模板参数:名字,参数
        /// </summary>
        public Dictionary<string,string> TemplParams { get; set; }

        /// <summary>
        /// 腾讯短信:扩展码，可填空
        /// </summary>
        public string Extend { get; set; }

        /// <summary>
        /// 腾讯短信:服务端原样返回的参数，可填空
        /// </summary>
        public string Ext { get; set; }

        /// <summary>
        /// 0阿里云,2腾讯云,4凌凯
        /// </summary>
        public int Provide { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// 短信配置代码
        /// </summary>
        public string Code { get; set; }
    }
}