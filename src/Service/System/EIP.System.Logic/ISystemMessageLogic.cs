

using EIP.Common.Message.Sms.Dto;

namespace EIP.System.Logic
{
    public interface ISystemMessageLogic
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        Task SendSms(SendSmsInput message);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        void SendEmail(SendEmailInput message);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        Task SendWebSite(SendWebSiteInput message);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        void SendWeiXinMp(SendWeiXinMpInput input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task SendWeiXinWork(SendWeiXinWorkInput input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task SendDingTalk(SendDingTalkInput input);
    }
}