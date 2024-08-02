using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Http;
using Aliyun.Acs.Core.Profile;
using EIP.Common.Config;
using EIP.Common.Core.Extension;
using EIP.Common.Message.Sms.Dto;

namespace EIP.Common.Message.Sms.Aliyun
{
    public class AliyunSms
    {
        private readonly SystemSmsTemplate _smsTemplate;
        public AliyunSms(SystemSmsTemplate smsTemplate)
        {
            _smsTemplate = smsTemplate;
        }
        public CommonResponse Send(SendSmsInput input)
        {
            CommonResponse response = new CommonResponse();
            var accessKeyId = _smsTemplate.AppId;
            var accessKeySecret = _smsTemplate.AppKey;
            var sign = _smsTemplate.Sign;
            IClientProfile profile = DefaultProfile.GetProfile("ap-northeast-1", accessKeyId, accessKeySecret);
            DefaultAcsClient client = new DefaultAcsClient(profile);
            CommonRequest request = new CommonRequest();
            request.Method = MethodType.POST;
            request.Domain = "dysmsapi.aliyuncs.com";
            request.Version = "2017-05-25";
            request.Action = "SendSms";
            // request.Protocol = ProtocolType.HTTP;
            request.AddQueryParameters("PhoneNumbers", input.Phone); //接收短信的手机号码
            request.AddQueryParameters("SignName", sign); //短信签名名称
            request.AddQueryParameters("TemplateCode", _smsTemplate.TemplateId); //短信模板CODE
            request.AddQueryParameters("TemplateParam", input.TemplParams.ListToJsonString()); //短信模板变量对应的实际值，JSON格式
            try
            {
                response = client.GetCommonResponse(request);
            }
            catch (ServerException e)
            {

            }
            catch (ClientException e)
            {

            }
            return response;
        }
    }
}
