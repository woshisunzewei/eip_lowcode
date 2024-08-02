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
using System;
using System.Collections.Generic;
using System.Security.Claims;
using static EIP.Common.JwtAuthorize.TokenBuilder;

namespace EIP.Common.JwtAuthorize
{
    /// <summary>
    /// TokenBuilder interface
    /// </summary>
    public interface ITokenBuilder
    {
        /// <summary>
        /// get the token of jwt
        /// </summary>
        /// <param name="claims">claim array</param>
        /// <returns></returns>
        Token BuildJwtToken(IEnumerable<Claim> claims, DateTime? expires = null);
        /// <summary>
        /// get the token of jwt
        /// </summary>
        /// <param name="claims">claim array</param>
        /// <param name="notBefore">not Before time</param>
        /// <param name="expires">expires</param>
        /// <returns></returns>
        Token BuildJwtToken(IEnumerable<Claim> claims, DateTime notBefore, DateTime? expires = null);

        /// <summary>
        /// get the token of jwt
        /// </summary>
        /// <param name="claims">claim array</param>
        /// <param name="ip">ip</param>
        /// <param name="notBefore">not Before time</param>
        /// <param name="expires">expires</param>
        /// <returns></returns>
        Token BuildJwtToken(IEnumerable<Claim> claims, string ip, DateTime? notBefore = null, DateTime? expires = null);
    }
}
