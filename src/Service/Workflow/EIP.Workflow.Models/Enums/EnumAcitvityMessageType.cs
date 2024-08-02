using System.ComponentModel;

namespace EIP.Workflow.Models.Enums
{
    /// <summary>
    /// 活动通知消息类型
    /// </summary>
    public enum EnumAcitvityMessageType
    {
        /// <summary>
        /// 站内
        /// </summary>
        [Description("system")]
        system,
        /// <summary>
        /// 邮件
        /// </summary>
        [Description("email")]
        email,
        /// <summary>
        /// 短信
        /// </summary>
        [Description("sms")]
        sms,
        /// <summary>
        /// App
        /// </summary>
        [Description("app")]
        app,
        /// <summary>
        /// 微信
        /// </summary>
        [Description("webchat")]
        webchat,
        /// <summary>
        /// 钉钉
        /// </summary>
        [Description("dingding")]
        dingding,
    }
}