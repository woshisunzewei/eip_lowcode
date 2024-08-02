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
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace EIP.Common.JwtAuthorize
{
    /// <summary>
    /// TokenBuilder
    /// </summary>
    public class TokenBuilder : ITokenBuilder
    {
        /// <summary>
        /// JwtAuthorizationRequirement
        /// </summary>
        readonly JwtAuthorizationRequirement _jwtAuthorizationRequirement;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="jwtAuthorizationRequirement"></param>
        public TokenBuilder(JwtAuthorizationRequirement jwtAuthorizationRequirement)
        {
            _jwtAuthorizationRequirement = jwtAuthorizationRequirement;
        }
        /// <summary>
        /// get the token of jwt
        /// </summary>
        /// <param name="claims">claim array</param>
        /// <param name="expires">expires</param>
        /// <returns></returns>
        public Token BuildJwtToken(IEnumerable<Claim> claims, DateTime? expires = null)
        {
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                issuer: _jwtAuthorizationRequirement.Issuer,
                audience: _jwtAuthorizationRequirement.Audience,
                claims: claims.ToArray(),
                notBefore: now,
                expires: expires,
                signingCredentials: _jwtAuthorizationRequirement.SigningCredentials
            );
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            var responseJson = new Token
            {
                TokenValue = encodedJwt,
                Expires = expires,
                TokenType = "Bearer"
            };
            return responseJson;
        }
        /// <summary>
        /// get the token of jwt
        /// </summary>
        /// <param name="claims">claim array</param>
        /// <param name="notBefore">not Before time</param>
        /// <param name="expires">expires</param>
        /// <returns></returns>
        public Token BuildJwtToken(IEnumerable<Claim> claims, DateTime notBefore, DateTime? expires = null)
        {
            var jwt = new JwtSecurityToken(
                issuer: _jwtAuthorizationRequirement.Issuer,
                audience: _jwtAuthorizationRequirement.Audience,
                claims: claims.ToArray(),
                notBefore: notBefore,
                expires: expires,
                signingCredentials: _jwtAuthorizationRequirement.SigningCredentials
            );
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            var responseJson = new Token
            {
                TokenValue = encodedJwt,
                Expires = expires,
                TokenType = "Bearer"
            };
            return responseJson;
        }
   
        /// <summary>
        /// get the token of jwt
        /// </summary>
        /// <param name="claims">claim array</param>
        /// <param name="ip">ip</param>
        /// <param name="notBefore">not Before time</param>
        /// <param name="expires">expires</param>
        /// <returns></returns>
        public Token BuildJwtToken(IEnumerable<Claim> claims, string ip, DateTime? notBefore = null, DateTime? expires = null)
        {
            var claimList = new List<Claim>(claims);
            //claimList.Add(new Claim("ip", ip));
            var now = notBefore.HasValue ? notBefore.Value : DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                issuer: _jwtAuthorizationRequirement.Issuer,
                audience: _jwtAuthorizationRequirement.Audience,
                claims: claimList.ToArray(),
                notBefore: notBefore,
                expires: expires,
                signingCredentials: _jwtAuthorizationRequirement.SigningCredentials
            );
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            var responseJson = new Token
            {
                TokenValue = encodedJwt,
                Expires = expires,
                TokenType = "Bearer"
            };
            return responseJson;
        }
        /// <summary>
        /// back token
        /// </summary>
        public class Token
        {
            /// <summary>
            /// Token Value
            /// </summary>
            public string TokenValue
            { get; set; }
            /// <summary>
            /// Expires (unit second)
            /// </summary>
            public DateTime? Expires
            { get; set; }
            /// <summary>
            /// token type
            /// </summary>
            public string TokenType
            { get; set; }
        }
    }
}
