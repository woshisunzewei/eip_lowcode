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
using EIP.Common.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Linq;
using EIP.Common.Core.Extension;
using EIP.Common.Core.Util;
using System;

namespace EIP.Common.Controller.Middlewares
{
    /// <summary>
    /// 请求中间件验证:必须设置为Post请求,都要携带token
    /// </summary>
    public class RequestProviderMiddleware
    {
        public readonly RequestDelegate Next;
        /// <summary>
        /// 构造函数注入
        /// </summary>
        /// <param name="next"></param>
        public RequestProviderMiddleware(RequestDelegate next)
        {
            Next = next;
        }
        /// <summary>
        /// 引入
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method.Equals("OPTIONS"))
            {
                await Next(context);
            }
            else
            {
                var validHeaders = context.Request.Headers["X-Real-V"].FirstOrDefault();
                //if (validHeaders.IsNullOrEmpty())
                //{
                //    await ReturnBadRequest(context, "请求失败,请稍后重试");
                //}
                //else
                //{
                    if (validHeaders.IsNotNullOrEmpty())
                    {
                        //解密后查看时间戳是否正确
                        RSACryptoService rsa = new RSACryptoService();
                        string v = rsa.Decrypt(validHeaders);
                        var time = v.Replace(ConfigurationUtil.GetSection("Tfs:VKey"), "");
                        try
                        {
                            var validTime = long.Parse(time);
                            if (DateTimeUtil.GetTimeStamp() - validTime > 5)
                            {
                                await ReturnBadRequest(context, "请求失败,请稍后重试");
                            }
                        }
                        catch (Exception)
                        {
                            await ReturnBadRequest(context, "请求失败,请稍后重试");
                        }
                    }
                //}


                //是否请求的获取Token接口
                //if (!context.Request.Path.Equals("/api/Account/Login", StringComparison.Ordinal))
                //{
                //    var tokenHeader = context.Request.Headers["Authorization"].FirstOrDefault();
                //    if (tokenHeader == null)
                //    {
                //        await ReturnBadRequest(context, "请设置头Authorization");
                //    }
                //    else
                //    {
                //        await Next(context);
                //    }
                //}
                //else
                //{
                await Next(context);
                //}
            }
        }

        /// <summary>
        /// 请求时错误
        /// </summary>
        /// <param name="context"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task ReturnBadRequest(HttpContext context, string message)
        {
            //返回代码401
            context.Response.StatusCode = 401;
            context.Response.ContentType = "application/json;charset=utf-8";
            var setting = new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            };
            await context.Response.WriteAsync(JsonConvert.SerializeObject(
                new OperateStatus
                {
                    Msg = message
                }, setting));
        }
    }
}