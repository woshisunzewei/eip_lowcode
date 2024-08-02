using EIP.Common.Config;
using EIP.Common.Core.Resource;
using EIP.Common.Message.Email.Dto;
using EIP.Common.Models;
using EIP.Common.Models.Resx;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Linq;

namespace EIP.Common.Message.Email
{
    /// <summary>
    /// MimeKit邮件发送
    /// </summary>
    public class MimeKitMail
    {
        public OperateStatus SendEmail(SendEmailInput input)
        {
            OperateStatus operateStatus = new OperateStatus();
            try
            {
                var configs = GlobalParams.GetValuesByName();
                var address = configs.FirstOrDefault(f => f.Key == ResourceSystemConfig.邮箱登录名)?.Value;
                var host = configs.FirstOrDefault(f => f.Key == ResourceSystemConfig.SMTP服务器)?.Value;
                var port = Convert.ToInt32(configs.FirstOrDefault(f => f.Key == ResourceSystemConfig.SMTP端口)?.Value);
                var ssl = Convert.ToBoolean(configs.FirstOrDefault(f => f.Key == ResourceSystemConfig.SMTPSSL)?.Value);
                var userName = configs.FirstOrDefault(f => f.Key == ResourceSystemConfig.邮箱登录名)?.Value;
                var password = configs.FirstOrDefault(f => f.Key == ResourceSystemConfig.邮箱密码)?.Value;
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(input.Name, address));
                message.To.Add(new MailboxAddress(input.ToName, input.ToAddress));
                message.Subject = input.Subject;
                message.Body = new TextPart("plain")
                {
                    Text = input.Message
                };
                using (var client = new SmtpClient())
                {
                    // For demo-purposes, accept all SSL certificates (in case the server supports STARTTLS)
                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    client.Connect(host, port, ssl);
                    // Note: only needed if the SMTP server requires authentication
                    client.Authenticate(userName, password);
                    client.Send(message);
                    client.Disconnect(true);
                }
                operateStatus.Code = ResultCode.Success;
                operateStatus.Msg = Chs.Successful;
            }
            catch (Exception ex)
            {
                operateStatus.Msg = ex.Message;
            }
            return operateStatus;
        }
    }
}