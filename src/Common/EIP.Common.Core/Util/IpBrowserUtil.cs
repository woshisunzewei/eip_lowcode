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
using Microsoft.AspNetCore.Http;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;

namespace EIP.Common.Core.Util
{
    public static class IpBrowserUtil
    {
        /// <summary>
        /// 获取客户端Ip
        /// </summary>
        /// <param name="accessor"></param>
        /// <returns></returns>
        public static string GetRemoteIp(IHttpContextAccessor accessor)
        {
            var remoteIp = accessor.HttpContext.Request.Headers["X-Real-IP"].FirstOrDefault();
            if (string.IsNullOrEmpty(remoteIp))
            {
                remoteIp = accessor.HttpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
                if (string.IsNullOrEmpty(remoteIp))
                {
                    remoteIp = accessor.HttpContext.Connection.RemoteIpAddress.ToString();
                }
                else
                {
                    remoteIp = remoteIp.Split(":")[0];
                }
            }
            if (remoteIp == "::1")
            {
                remoteIp = "127.0.0.1";
            }
            return remoteIp;
        }

        /// <summary>
        /// 获取客户端Ip物理地址
        /// </summary>
        /// <param name="accessor"></param>
        /// <returns></returns>
        public static async Task<string> GetRemoteIpAddress(IHttpContextAccessor accessor)
        {
            var apiUrl = "http://whois.pconline.com.cn/ip.jsp";
            var str = new ConcurrentDictionary<string, string>();
            str.TryAdd("ip", GetRemoteIp(accessor));
            return (await RequestUtil.Get(apiUrl, str)).Replace("\n", string.Empty).Replace("\r", string.Empty);
        }
    }
}