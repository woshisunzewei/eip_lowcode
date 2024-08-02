using System;
using System.Collections.Generic;

namespace EIP.Common.Message.WeiXinMp.Dto
{
    /// <summary>
    /// 微信公众号消息
    /// </summary>
    public class SendWeiXinMpInput : MessageBaseInput
    {
        /// <summary>
        /// 公众号AppId
        /// </summary>
        public string MpAppId { get; set; }

        /// <summary>
        /// 小程序AppId
        /// </summary>
        public string MiniAppId { get; set; }

        /// <summary>
        /// 模版代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 跳转地址
        /// </summary>
        public string ReturnUrl { get; set; }

        /// <summary>
        /// 参数配置情况
        /// </summary>
        public IList<SendWeiXinMpNoticeMessageOutput> Parameter { get; set; }
    }
    public class SendWeiXinMpNoticeMessageOutput
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 颜色
        /// </summary>
        public string Color { get; set; }
    }
    
}