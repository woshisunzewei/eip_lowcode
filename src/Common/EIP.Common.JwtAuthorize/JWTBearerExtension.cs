/**************************************************************
* Copyright (C) 2018 www.eipflow.com 孙泽伟版权所有(盗版必究)
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
using EIP.Common.Core.Extension;
using EIP.Common.Core.Util;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Net.NetworkInformation;
using System.Text;

namespace EIP.Common.JwtAuthorize
{
    /// <summary>
    /// EIP.Common.JwtAuthorize extension
    /// </summary>
    public static class JwtBearerExtension
    {
        private static string policyName = "permission";
        private static string defaultScheme = "Bearer";
        private static bool isHttps = false;
        private static bool requireExpirationTime = true;
        private static string secret = "fxI0smpydotFuqyJO5cmKUKgOASHJStbsczUTbtuaLddRhVE72p58EScfP1mkMTV";
        private static string issuer = GetHostName();
        private static string audience = "EIPAudience";
        /// <summary>
        /// 获取电脑计算机名
        /// </summary>
        /// <returns></returns>
        public static string GetHostName()
        {
            //本地计算机网络连接信息
            IPGlobalProperties computerProperties = IPGlobalProperties.GetIPGlobalProperties();
            //获取本机电脑名
            var hostName = computerProperties.HostName;
            return !string.IsNullOrEmpty(hostName) ? hostName : "EIPIssuer";

        }
        /// <summary>
        /// In the Ocelot Project, the Startup. Cs class ConfigureServices method is called
        /// </summary>
        /// <param name="services">Service Collection</param>  
        /// <returns></returns>
        public static AuthenticationBuilder AddOcelotJwtAuthorize(this IServiceCollection services)
        {
            SetParams();
            var keyByteArray = Encoding.ASCII.GetBytes(secret);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = false,
                ValidAudience = audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = requireExpirationTime
            };
            return services.AddAuthentication(options =>
            {
                options.DefaultScheme = defaultScheme;
            })
             .AddJwtBearer(defaultScheme, opt =>
             {
                 opt.RequireHttpsMetadata = isHttps;
                 opt.TokenValidationParameters = tokenValidationParameters;
             });
        }

        /// <summary>
        /// In the API Project, the Startup. Cs class ConfigureServices method is called
        /// </summary>
        /// <param name="services">Service Collection</param>
        /// <param name="validatePermission">validate permission action</param>
        /// <returns></returns>
        public static AuthenticationBuilder AddApiJwtAuthorize(this IServiceCollection services, Func<HttpContext, bool> validatePermission)
        {
            SetParams();
            var keyByteArray = Encoding.ASCII.GetBytes(secret);
            var signingKey = new SymmetricSecurityKey(keyByteArray);
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = false,
                ValidAudience = audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                RequireExpirationTime = requireExpirationTime
            };
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var permissionRequirement = new JwtAuthorizationRequirement(
                issuer,
                audience,
                signingCredentials
                );

            permissionRequirement.ValidatePermission = validatePermission;

            services.AddSingleton<IAuthorizationHandler, PermissionHandler>();
            services.AddSingleton(permissionRequirement);
            return services.AddAuthorization(options =>
            {
                options.AddPolicy(policyName,
                          policy => policy.Requirements.Add(permissionRequirement));

            })
         .AddAuthentication(options =>
         {
             options.DefaultScheme = defaultScheme;
         })
         .AddJwtBearer(defaultScheme, o =>
         {
             o.RequireHttpsMetadata = isHttps;
             o.TokenValidationParameters = tokenValidationParameters;
         });
        }
        /// <summary>
        /// In the Authorize Project, the Startup. Cs class ConfigureServices method is called
        /// </summary>
        /// <param name="services">Service Collection</param>
        /// <returns></returns>
        public static IServiceCollection AddTokenJwtAuthorize(this IServiceCollection services)
        {
            try
            {
                SetParams();
                var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secret)), SecurityAlgorithms.HmacSha256);
                var permissionRequirement = new JwtAuthorizationRequirement(
                   issuer,
                   audience,
                   signingCredentials
                    );
                services.AddSingleton<ITokenBuilder, TokenBuilder>();
                return services.AddSingleton(permissionRequirement);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public static void SetParams()
        {
            var configuration = ConfigurationUtil.Configuration;
            if (configuration == null)
            {
                throw new OcelotJwtAuthoizeException("can't find JwtAuthorize section in appsetting.json");
            }
            var config = configuration.GetSection("EIP:Jwt");
            if (config != null)
            {
                if (config["Secret"].IsNotNullOrEmpty())
                {
                    secret = config["Secret"];
                }
                if (config["Issuer"].IsNotNullOrEmpty())
                {
                    issuer = config["Issuer"];
                }
                if (config["Audience"].IsNotNullOrEmpty())
                {
                    audience = config["Audience"];
                }
            }
        }
    }
}
