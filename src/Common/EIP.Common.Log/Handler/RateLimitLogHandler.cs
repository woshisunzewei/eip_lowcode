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

using EIP.Common.Core.Util;
using Microsoft.AspNetCore.Http;
using System;

namespace EIP.Common.Log.Handler
{
    /// <summary>
    /// 限流日志
    /// </summary>
    public class RateLimitLogHandler : BaseHandler<RateLimitLog>
    {
        /// <summary>
        /// 限流日志
        /// </summary>
        public RateLimitLogHandler(HttpRequest request,
            LogUser logUser) : base("SystemRateLimitLog")
        {
            Log = new RateLimitLog
            {
                RateLimitLogId = Guid.NewGuid(),
                RequestContentLength = request.ContentLength == null ? 0 : (long)request.ContentLength,
                RequestData = RequestData(request),
                RequestType = request.Method,
                Url = request.Path.Value,
                CreateTime = DateTime.Now.ToYyyyMMddHHmmss(),
                CreateUserName = logUser.Name,
                CreateUserCode = logUser.Code,
                CreateUserId = logUser.UserId,
            };
        }
    }
}