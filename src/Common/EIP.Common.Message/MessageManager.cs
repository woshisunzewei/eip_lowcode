using EIP.Common.Message.Email.Dto;
using EIP.Common.Message.Sms.Dto;
using EIP.Common.Message.WebSite.Dto;

namespace EIP.Common.Message
{
    /// <summary>
    /// 消息管理
    /// </summary>
    public interface IMessageManagerLogic
    {
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="input"></param>
        void SendSms(SendSmsInput input);

        /// <summary>
        /// 发送站内消息
        /// </summary>
        /// <param name="input"></param>
        void SendWebSite(SendWebSiteInput input);

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="input"></param>
        void SendEmail(SendEmailInput input);

        ///// <summary>
        ///// 发送微信公众号信息
        ///// </summary>
        ///// <param name="input"></param>
        //void SendOffiAccount(SendOffiAccountInput input);

        /// <summary>
        /// 发送App
        /// </summary>
        /// <param name="input"></param>
        void SendApp(SendAppInput input);
    }
}