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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using System;

namespace EIP.Common.JwtAuthorize
{

    /// <summary>
    ///jwt authorizationrequirement
    /// </summary>
    public class JwtAuthorizationRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// validate permission Func
        /// </summary>
        public Func<HttpContext, bool> ValidatePermission
        { get; internal set; }       

        /// <summary>
        /// issuer
        /// </summary>
        public string Issuer { get; set; }
        /// <summary>
        /// audience
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// signing credentials
        /// </summary>
        public SigningCredentials SigningCredentials { get; set; }


        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="claimType">claim type</param>
        /// <param name="issuer">issuer</param>
        /// <param name="audience">audience</param>
        /// <param name="signingCredentials">signing credentials</param>
        /// <param name="expiration">expiration</param>
        public JwtAuthorizationRequirement(string issuer, string audience, SigningCredentials signingCredentials)
        {          
            Issuer = issuer;
            Audience = audience;            
            SigningCredentials = signingCredentials;
        }
    }
}
