using EIP.Common.Message.Sms.Dto;
using ApiHandlerWapper = Senparc.Weixin.MP.ApiHandlerWapper;

namespace EIP.Workflow.Logic.Impl
{
    public class SystemMessageLogic : ISystemMessageLogic
    {
        private readonly IHubContext<WebSiteHub> _hub;
        private readonly ISystemConfigLogic _configLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hub"></param>
        /// <param name="messageLogic"></param>
        public SystemMessageLogic(ISystemConfigLogic configLogic, IHubContext<WebSiteHub> hub)
        {
            _configLogic = configLogic;
            _hub = hub;
        }

        /// <summary>
        /// 保存短信日志
        /// </summary>
        /// <param name="message"></param>
        public async Task SendSms(SendSmsInput input)
        {
            OperateStatus operateStatus = new OperateStatus();
            string message = input.Message;
            string response = string.Empty;
            bool isSend = false;
            try
            {
                foreach (var item in input.Phone.Split(','))
                {
                    switch (input.Provide)
                    {
                        case 0:
                            {
                                var smsTemplate = SmsConfig.GetByCode(input.Code);
                                AliyunSms aliyunSms = new AliyunSms(smsTemplate);
                                var result = aliyunSms.Send(new SendSmsInput
                                {
                                    Code = input.Code,
                                    Phone = item,
                                    TemplParams = input.TemplParams,
                                    Ext = input.Ext,
                                });
                                response = JsonConvert.SerializeObject(result);
                                operateStatus.Msg = result.HttpResponse;
                                message = JsonConvert.SerializeObject(input.TemplParams);
                                isSend = true;
                            }
                            break;
                        case 2:
                            {
                                var smsTemplate = SmsConfig.GetByCode(input.Code);
                                TencentCloudSms sender = new TencentCloudSms(smsTemplate);
                                var sendResult = sender.SendSms(new SendSmsInput
                                {
                                    Code = input.Code,
                                    Phone = item,
                                    TemplParams = input.TemplParams,
                                    Ext = input.Ext,
                                    Message = input.Message,
                                    Extend = input.Extend,
                                });
                                response = JsonConvert.SerializeObject(sendResult);
                                operateStatus.Msg = sendResult.errmsg;
                                message = JsonConvert.SerializeObject(input.TemplParams);
                                isSend = true;
                            }
                            break;
                        case 4://凌凯
                            Dictionary<string, string> parameters = new Dictionary<string, string>();
                            parameters.Add("CorpID", ConfigurationUtil.GetSection("EIP_SMS:LingKai:CorpID"));
                            parameters.Add("Pwd", ConfigurationUtil.GetSection("EIP_SMS:LingKai:Pwd"));
                            parameters.Add("Mobile", item);
                            parameters.Add("Content", input.Message.Trim());
                            parameters.Add("Cell", "1");
                            string url = "https://sdk3.028lk.com:9988/BatchSend2.aspx";
                            response = await RequestUtil.Get(url, parameters, "gb2312");
                            message = JsonConvert.SerializeObject(parameters);
                            operateStatus.Msg = response;
                            isSend = true;
                            break;

                        default:
                            break;
                    }
                    operateStatus.Code = ResultCode.Success;

                    //写入数据库
                    var smsLog = new Common.Message.Sms.Dto.SystemSmsLog
                    {
                        SmsLogId = CombUtil.NewComb(),
                        Request = JsonConvert.SerializeObject(input),
                        Response = response,
                        Message = message,
                        Phone = item,
                        Provide = input.Provide,
                        Code = input.Code,
                        CreateTime = DateTime.Now.ToYyyyMMddHHmmss(),
                        IsSend = isSend
                    };
                    Logger logger = new Logger("SaveSmsLog");
                    var ei = new EIPLogEventInfo(LogLevel.Info, "SaveSmsLog", "保存短信日志");
                    foreach (var log in smsLog.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
                        ei.Properties[log.Name] = log.GetValue(smsLog, null);
                    logger.Log(ei);
                }
            }
            catch (Exception ex)
            {
                message = ex.Message;
            }

        }

        /// <summary>
        /// 保存邮件日志
        /// </summary>
        /// <param name="message"></param>
        public void SendEmail(SendEmailInput message)
        {
            Logger logger = new Logger("SaveEmailLog");
            var ei = new EIPLogEventInfo(LogLevel.Info, "SaveEmailLog", "保存邮件日志");
            foreach (var item in message.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
                ei.Properties[item.Name] = item.GetValue(message, null);
            logger.Log(ei);
        }

        /// <summary>
        /// 保存站内消息
        /// </summary>
        /// <param name="message"></param>
        public async Task SendWebSite(SendWebSiteInput message)
        {
            try
            {
                using (var fix = new SqlDatabaseFixture())
                {
                    SystemMessageLog systemMessageLog = new SystemMessageLog()
                    {
                        MessageLogId = message.MessageLogId,
                        Title = message.Title,
                        Message = message.Message,
                        ReceiverId = message.ReceiverId,
                        ReceiverName = message.ReceiverName,
                        ReceiverType = message.ReceiverType.ToShort(),
                        OpenType = message.OpenType,
                        OpenUrl = message.ReturnUrl,
                        CreateTime = DateTime.Now,
                        CustomData = message.CustomData,
                    };
                    await fix.Db.SystemMessageLog.InsertAsync(systemMessageLog);
                    //查找接收人员
                    foreach (var receiverId in message.ReceiverId.Split(','))
                    {
                        var user = OnlineUsers.Users.FirstOrDefault(w => w.UserId == Guid.Parse(receiverId));
                        if (user != null)
                        {
                            await _hub.Clients.Client(user.ConnectionId).SendAsync("SendWebSiteMessage", JsonConvert.SerializeObject(systemMessageLog));
                        }
                    }
                    //通知人员
                }
            }
            catch (Exception ex)
            {
            }
        }

        /// <summary>
        /// 微信公众号
        /// </summary>
        /// <param name="input"></param>
        public async void SendWeiXinMp(SendWeiXinMpInput input)
        {
            //解析发送
            //获取需要发送用户的OpenId
            using (var fix = new SqlDatabaseFixture())
            {
                var userSql = $"select wu.openId,wu1.UserId from wechat_user wu left join wechat_user wu1 on wu.Unionid=wu1.Unionid where wu.Type = 1 and wu1.UserId in ({input.ReceiverId.InSql()}) ";
                var users = await fix.Db.Connection.QueryAsync<SystemUserInfoThree>(userSql);
                TemplateModel_MiniProgram miniProgram = null;
                if (input.MiniAppId.IsNotNullOrEmpty())
                {
                    miniProgram = new TemplateModel_MiniProgram
                    {
                        appid = input.MiniAppId,
                        pagepath = input.ReturnUrl
                    };
                }
                IDictionary<string, object> propertyDics = new Dictionary<string, object>();
                foreach (var item in input.Parameter)
                {
                    propertyDics.Add(item.Name, new TemplateDataItem("" + item.Value + "", item.Color));
                }

                foreach (var item in users)
                {
                    var result = ApiHandlerWapper.TryCommonApi(accessToken =>
                    {
                        string urlFormat = Config.ApiMpHost + "/cgi-bin/message/template/send?access_token={0}";
                        var submitData = new TemplateModel()
                        {
                            touser = item.PlatformUserId,
                            template_id = input.Code,
                            url = input.ReturnUrl,
                            miniprogram = miniProgram,
                            data = propertyDics,
                        };
                        return CommonJsonSend.Send(accessToken, urlFormat, submitData, timeOut: Config.TIME_OUT);
                    }, input.MpAppId);
                    if (result.errcode == ReturnCode.请求成功)
                    {

                    }
                }
            }
        }

        /// <summary>
        /// 钉钉
        /// </summary>
        /// <param name="input"></param>
        public async Task SendDingTalk(SendDingTalkInput input)
        {
            var appKey = await _configLogic.FindByKey(EnumConfigKey.dingTalkAppKey.ToString());
            var appSecret = await _configLogic.FindByKey(EnumConfigKey.dingTalkAppSecret.ToString());
            var agentId = await _configLogic.FindByKey(EnumConfigKey.dingTalkAgentId.ToString());
            //解析发送
            using (var fix = new SqlDatabaseFixture())
            {
                //获取token
                AlibabaCloud.SDK.Dingtalkoauth2_1_0.Client client = CreateClient();
                AlibabaCloud.SDK.Dingtalkoauth2_1_0.Models.GetAccessTokenRequest getAccessTokenRequest = new AlibabaCloud.SDK.Dingtalkoauth2_1_0.Models.GetAccessTokenRequest()
                {
                    AppKey = appKey,
                    AppSecret = appSecret,
                };
                try
                {
                    var accessToken = client.GetAccessToken(getAccessTokenRequest);
                    //Post调用
                    foreach (var item in input.ReceiverId.Split(','))
                    {
                        var dingTalkUser = await fix.Db.SystemUserInfoThree.FindAsync(f => f.SystemUserId == Guid.Parse(item));
                        if (dingTalkUser != null && dingTalkUser.PlatformUserId.IsNotNullOrEmpty())
                        {
                            //调用发送
                            var sendResult = await RequestUtil.Post($"https://oapi.dingtalk.com/topapi/message/corpconversation/asyncsend_v2?access_token={accessToken.Body.AccessToken}", new
                            {
                                agent_id = agentId,
                                userid_list = dingTalkUser.PlatformUserId,
                                msg = new
                                {
                                    msgtype = input.ReturnUrl.IsNotNullOrEmpty() ? "link" : "text",
                                    text = new
                                    {
                                        content = input.Message
                                    },
                                    link = new
                                    {
                                        messageUrl = input.ReturnUrl,
                                        title = input.Title,
                                        text = input.Message,
                                        picUrl = "@lALOACZwe2Rk"
                                    },
                                }
                            });
                        }
                    }

                }
                catch (TeaException err)
                {
                    if (!AlibabaCloud.TeaUtil.Common.Empty(err.Code) && !AlibabaCloud.TeaUtil.Common.Empty(err.Message))
                    {
                        // err 中含有 code 和 message 属性，可帮助开发定位问题
                    }
                }
                catch (Exception _err)
                {
                    TeaException err = new TeaException(new Dictionary<string, object>
                {
                    { "message", _err.Message }
                });
                    if (!AlibabaCloud.TeaUtil.Common.Empty(err.Code) && !AlibabaCloud.TeaUtil.Common.Empty(err.Message))
                    {
                        // err 中含有 code 和 message 属性，可帮助开发定位问题
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static AlibabaCloud.SDK.Dingtalkoauth2_1_0.Client CreateClient()
        {
            AlibabaCloud.OpenApiClient.Models.Config config = new AlibabaCloud.OpenApiClient.Models.Config();
            config.Protocol = "https";
            config.RegionId = "central";
            return new AlibabaCloud.SDK.Dingtalkoauth2_1_0.Client(config);
        }

        /// <summary>
        /// 企业微信
        /// </summary>
        /// <param name="input"></param>
        public async Task SendWeiXinWork(SendWeiXinWorkInput input)
        {
            //解析发送
            using (var fix = new SqlDatabaseFixture())
            {
                try
                {
                    //Post调用
                    foreach (var item in input.ReceiverId.Split(','))
                    {
                        var weChatWorkUser = await fix.Db.SystemUserInfoThree.SetSelect(s => new { s.PlatformUserId }).FindAsync(f => f.SystemUserId == Guid.Parse(item));
                        if (weChatWorkUser != null && weChatWorkUser.PlatformUserId.IsNotNullOrEmpty())
                        {
                            var weixinSetting = Config.SenparcWeixinSetting.WorkSetting;
                            var appKey = AccessTokenContainer.BuildingKey(weixinSetting.WeixinCorpId, weixinSetting.WeixinCorpSecret);
                            var result = MassApi.SendTextCard(appKey, weixinSetting.WeixinCorpAgentId, input.Title, input.Message, input.ReturnUrl, "查看详情", weChatWorkUser.PlatformUserId);
                        }
                        else
                        {
                            //使用用户帐号发送
                            var systemUserInfo = await fix.Db.SystemUserInfo.SetSelect(s => new { s.Code }).FindAsync(f => f.UserId == Guid.Parse(item));
                            if (systemUserInfo != null && systemUserInfo.Code.IsNotNullOrEmpty())
                            {
                                var weixinSetting = Config.SenparcWeixinSetting.WorkSetting;
                                var appKey = AccessTokenContainer.BuildingKey(weixinSetting.WeixinCorpId, weixinSetting.WeixinCorpSecret);
                                var result = MassApi.SendTextCard(appKey, weixinSetting.WeixinCorpAgentId, input.Title, input.Message, input.ReturnUrl, "查看详情", systemUserInfo.Code);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }
    }
}