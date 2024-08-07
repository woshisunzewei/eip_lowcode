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
using System;

namespace EIP.Common.Log.Handler
{
    /// <summary>
    /// 登录日志
    /// </summary>
    public class LoginLogHandler : BaseHandler<LoginLog>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="principalUser">登录用户</param>
        /// <param name="loginLogId">登录用户</param>
        public LoginLogHandler(PrincipalUser principalUser,
            Guid loginLogId) : base("SystemLoginLog")
        {
            Log = new LoginLog
            {
                LoginLogId = loginLogId,
                RemoteIp = principalUser.RemoteIp,
                RemoteIpAddress = principalUser.RemoteIpAddress,
                UserAgent = principalUser.UserAgent,
                OsInfo=principalUser.OsInfo,
                Browser=principalUser.Browser,
                CreateUserId = principalUser.UserId.ToString(),
                CreateUserName = principalUser.Name,
                CreateTime = DateTime.Now.ToYyyyMMddHHmmss(),
                Source = principalUser.Source,
                CreateUserCode = principalUser.Code,
                LoginTime = DateTime.Now.ToYyyyMMddHHmmss()
            };
        }
    }

    /// <summary>
    /// 登录退出日志
    /// </summary>
    public class LoginOutLogHandler : BaseHandler<LoginOutLog>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="principalUser">登录用户</param>
        /// <param name="loginLogId">登录用户</param>
        public LoginOutLogHandler(PrincipalUser principalUser,
            Guid loginLogId) : base("SystemLoginOutLog")
        {
            Log = new LoginOutLog
            {
                LoginLogId = loginLogId,
                LoginOutTime = DateTime.Now.ToYyyyMMddHHmmss()
            };
        }
    }
}