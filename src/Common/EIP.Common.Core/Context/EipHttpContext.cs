using EIP.Common.Core.Auth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace EIP.Common.Core.Context
{
    /// <summary>
    /// EIP上下文
    /// </summary>
    public class EipHttpContext
    {
        private static IHttpContextAccessor _accessor;
        public static HttpContext Current => _accessor.HttpContext;

        internal static void Configure(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        /// <summary>
        /// 获取当前登录人员信息
        /// </summary>
        /// <returns></returns>
        public static PrincipalUser CurrentUser()
        {
            if (Current != null)
            {
                var user = Current.User;
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
                    currentUser.IsAdmin = bool.Parse(user.FindFirst("IsAdmin")?.Value);
                }
                else
                {
                    currentUser.UserId = Guid.Empty;
                    currentUser.Name = "匿名用户";
                }
                return currentUser;
            }
            return new PrincipalUser();
        }
    }

    public static class StaticHttpContextExtensions
    {
        public static void AddHttpContextAccessor(this IServiceCollection services)
        {
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        }

        public static IApplicationBuilder UseStaticHttpContext(this IApplicationBuilder app)
        {
            var httpContextAccessor = app.ApplicationServices.GetRequiredService<IHttpContextAccessor>();
            EipHttpContext.Configure(httpContextAccessor);
            return app;
        }
    }
}