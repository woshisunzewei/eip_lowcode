/**************************************************************
* Copyright (C) 2022 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2022/01/12 22:40:15
* 文件名: 
* 描述: 
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Base.Models.Entities.WeChat;
using EIP.System.Models.Dtos.WeChat.MpTemplateSend;
using ApiHandlerWapper = Senparc.Weixin.MP.ApiHandlerWapper;

namespace EIP.WeChat.Logic.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class WeChatMpTemplateSendLogic : DapperAsyncLogic<WeChatMpTemplateSend>, IWeChatMpTemplateSendLogic
    {
        /// <summary>
        /// 根据Id删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> Delete(IdInput<string> input)
        {
            OperateStatus operateStatus = new OperateStatus();
            foreach (var item in input.Id.Split(','))
            {
                operateStatus = await DeleteAsync(f => f.TemplateSendId == Guid.Parse(item));
            }
            return operateStatus;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> Save(WeChatMpTemplateSend input)
        {
            var editData = await FindAsync(f => f.TemplateSendId == input.TemplateSendId);
            if (editData != null)
            {
                editData.Id = input.Id;
                return await UpdateAsync(input);
            }
            input.CreateTime = DateTime.Now;
            input.TemplateSendId = CombUtil.NewComb();
            return await InsertAsync(input);
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> Send(IdInput input)
        {
            var editData = await FindAsync(f => f.TemplateSendId == input.Id);
            OperateStatus operateStatus = new OperateStatus();
            //获取所有OpenId
            try
            {
                List<string> openIds = new List<string>();
                operateStatus = await GetAllOpenIds(openIds, "");
                //写入人员表
                if (operateStatus.Code != ResultCode.Success)
                {
                    return operateStatus;
                }
                else
                {
                    using (var fix = new SqlDatabaseFixture())
                    {
                        editData.Number = openIds.Count();
                        var allSendUser = await fix.Db.WeChatMpTemplateSendUser.FindAllAsync(f => f.TemplateSendId == input.Id);
                        //写入表
                        List<WeChatMpTemplateSendUser> weChatMpTemplateSendUsers = new List<WeChatMpTemplateSendUser>();
                        foreach (var item in openIds)
                        {
                            //是否已存在
                            var sendUser = allSendUser.FirstOrDefault(f => f.OpenId == item);
                            if (sendUser == null)
                            {
                                weChatMpTemplateSendUsers.Add(new WeChatMpTemplateSendUser
                                {
                                    TemplateSendId = input.Id,
                                    TemplateSendUserId = CombUtil.NewComb(),
                                    OpenId = item,
                                    CreateTime = DateTime.Now,
                                });
                            }
                        }
                        if (weChatMpTemplateSendUsers.Any())
                        {
                            var insert = MySqlBulkLoaderHelper.BulkInsert(weChatMpTemplateSendUsers);
                        }
                        //查询所有未发送
                        var noSendUser = await fix.Db.WeChatMpTemplateSendUser.FindAllAsync(f => f.TemplateSendId == input.Id && f.SendTime == "");
                        editData.HaveSend = editData.Number - noSendUser.Count();
                        var template = await fix.Db.WeChatMpTemplate.FindAsync(f => f.TemplateId == editData.TemplateId);
                        TemplateMessageData data = new TemplateMessageData();
                        dynamic jsonObj = JsonConvert.DeserializeObject(editData.Data);
                        string url = "";
                        foreach (var obj in jsonObj)
                        {
                            if (obj.Name != "url")
                            {
                                data.Add(obj.Name, new TemplateMessageDataValue(obj.Value));
                            }
                            else
                            {
                                url = obj.Value;
                            }
                        }
                        foreach (var item in noSendUser)
                        {
                            //解析发送
                            var result = ApiHandlerWapper.TryCommonApi(accessToken =>
                            {
                                string urlFormat = Config.ApiMpHost + "/cgi-bin/message/template/send?access_token={0}";
                                var submitData = new
                                {
                                    touser = item.OpenId,
                                    template_id = template.Code,
                                    url = url,
                                    data = data,
                                };
                                return CommonJsonSend.Send(accessToken, urlFormat, submitData, timeOut: Config.TIME_OUT);
                            }, Config.SenparcWeixinSetting.WeixinAppId);
                            if (result.errcode == ReturnCode.请求成功)
                            {
                                //更新
                                item.SendTime = DateTime.Now.ToYyyyMMddHHmmss();
                                await fix.Db.WeChatMpTemplateSendUser.UpdateAsync(item);
                                editData.HaveSend += 1;
                                await fix.Db.WeChatMpTemplateSend.UpdateAsync(editData);
                            }
                        }
                    }
                }
                operateStatus.Msg = "发送完成";
                operateStatus.Code = ResultCode.Success;
            }
            catch (Exception ex)
            {
                operateStatus.Code = ResultCode.Error;
                operateStatus.Msg = ex.Message;
            }
            return operateStatus;
        }

        /// <summary>
        /// 发送消息预览
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> SendPreview(WechatMpTemplateSendPreviewInput input)
        {
            var editData = await FindAsync(f => f.TemplateSendId == input.TemplateSendId);
            OperateStatus operateStatus = new OperateStatus();
            try
            {
                using (var fix = new SqlDatabaseFixture())
                {
                    //查询所有未发送
                    var template = await fix.Db.WeChatMpTemplate.FindAsync(f => f.TemplateId == editData.TemplateId);
                    TemplateMessageData data = new TemplateMessageData();
                    dynamic jsonObj = JsonConvert.DeserializeObject(editData.Data);
                    string url = "";
                    foreach (var obj in jsonObj)
                    {
                        if (obj.Name != "url")
                        {
                            data.Add(obj.Name, new TemplateMessageDataValue(obj.Value));
                        }
                        else
                        {
                            url = obj.Value;
                        }
                    }
                    //解析发送
                    var result = ApiHandlerWapper.TryCommonApi(accessToken =>
                    {
                        string urlFormat = Config.ApiMpHost + "/cgi-bin/message/template/send?access_token={0}";
                        var submitData = new
                        {
                            touser = input.OpenId,
                            template_id = template.Code,
                            url = url,
                            data = data,
                        };
                        return CommonJsonSend.Send(accessToken, urlFormat, submitData, timeOut: Config.TIME_OUT);
                    }, Config.SenparcWeixinSetting.WeixinAppId);
                    if (result.errcode == ReturnCode.请求成功)
                    {
                        operateStatus.Msg = "发送完成";
                        operateStatus.Code = ResultCode.Success;
                    }
                    else
                    {
                        operateStatus.Msg = result.errmsg;
                    }
                }
            }
            catch (Exception ex)
            {
                operateStatus.Code = ResultCode.Error;
                operateStatus.Msg = ex.Message;
            }
            return operateStatus;
        }
        /// <summary>
        /// 获取所有OpenIds
        /// </summary>
        /// <returns></returns>
        private async Task<OperateStatus> GetAllOpenIds(List<string> openIds, string nextOpenId)
        {
            OperateStatus operateStatus = new OperateStatus();
            string appId = Config.SenparcWeixinSetting.WeixinAppId;
            var openIdResult = await UserApi.GetAsync(appId, nextOpenId);
            if (openIdResult.errcode == ReturnCode.请求成功)
            {
                //写入
                foreach (var item in openIdResult.data.openid)
                {
                    openIds.Add(item);
                }
                if (openIds.Count != openIdResult.count)
                {
                    await GetAllOpenIds(openIds, openIds[openIds.Count - 1]);
                }
                operateStatus.Code = ResultCode.Success;
                operateStatus.Msg = "查询成功";
            }
            else
            {
                operateStatus.Msg = openIdResult.errmsg;
            }
            return operateStatus;
        }
    }
}