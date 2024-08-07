/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2018/10/30 22:40:15
* 文件名: 
* 描述: 
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Common.Core.Auth;
using EIP.Common.Core.Util;
using Microsoft.AspNetCore.Http;
using System;

namespace EIP.Common.Log.Handler
{
    /// <summary>
    /// 操作处理
    /// </summary>
    public class OperationLogHandler : BaseHandler<OperateLog>
    {
        /// <summary>
        /// 操作日志
        /// </summary>
        public OperationLogHandler(HttpRequest request,
            LogUser logUser) : base("SystemOperationLog")
        {
            Log = new OperateLog
            {
                OperationLogId = CombUtil.NewComb(),
                CreateTime = DateTime.Now.ToYyyyMMddHHmmss(),
                RequestContentLength = request.ContentLength == null ? 0 : (long)request.ContentLength,
                RequestType = request.Method,
                RequestData = RequestData(request),
                RemoteIp=logUser.RemoteIp,
                RemoteIpAddress=logUser.RemoteIpAddress,
                UserAgent = logUser.UserAgent,
                OsInfo = logUser.OsInfo,
                Browser = logUser.Browser,
                Url = request.Path.Value,
                CreateUserName = logUser.Name,
                CreateUserCode = logUser.Code,
                CreateUserId = logUser.UserId,
            };
        }

        /// <summary>
        /// 执行时间
        /// </summary>
        public void ActionExecuted()
        {
            Log.ActionExecutionTime = (DateTime.Now - DateTime.Parse(Log.CreateTime)).TotalSeconds;
        }

        /// <summary>
        /// 执行时间
        /// </summary>
        public void ResultExecuted()
        {
            Log.ResultExecutionTime = (DateTime.Now - DateTime.Parse(Log.CreateTime)).TotalSeconds;
        }
    }
}