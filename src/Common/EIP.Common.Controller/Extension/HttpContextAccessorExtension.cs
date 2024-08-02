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
using Microsoft.AspNetCore.Http;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace EIP.Common.Controller.Extension
{
    /// <summary>
    /// 扩展
    /// </summary>
    public static class HttpContextAccessorExtension
    {
        /// <summary>
        /// 获取当前登录人员信息
        /// </summary>
        /// <param name="httpContextAccessor"></param>
        /// <returns></returns>
        public static PrincipalUser CurrentUser(this IHttpContextAccessor httpContextAccessor)
        {
            var user = httpContextAccessor?.HttpContext?.User;
            PrincipalUser currentUser = new PrincipalUser();
            if (user != null && user.Identity.IsAuthenticated)
            {
                currentUser.UserId = Guid.Parse(user.FindFirst(JwtRegisteredClaimNames.Jti)?.Value);
                currentUser.Name = user.FindFirst("Name")?.Value;
                currentUser.Code = user.FindFirst("Code")?.Value;
                currentUser.RemoteIp = user.FindFirst("RemoteIp")?.Value;
                currentUser.RemoteIpAddress = user.FindFirst("RemoteIpAddress")?.Value;
                currentUser.UserAgent = user.FindFirst("UserAgent")?.Value;
                currentUser.OsInfo = user.FindFirst("OsInfo")?.Value;
                currentUser.Browser = user.FindFirst("Browser")?.Value;
                currentUser.OrganizationId = user.FindFirst("OrganizationId")?.Value == "" ? Guid.Empty : Guid.Parse(user?.FindFirst("OrganizationId")?.Value);
                currentUser.OrganizationName = user.FindFirst("OrganizationName")?.Value;
                currentUser.LoginId = Guid.Parse(user.FindFirst("LoginId")?.Value);
            }
            else
            {
                currentUser.UserId=Guid.Empty;
                currentUser.Name = "匿名用户";
            }
            return currentUser;
        }
    }
}