﻿/**************************************************************
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
using System;
using System.Net;
using System.Threading.Tasks;

namespace EIP.Common.Controller.Middlewares
{
    /// <summary>
    /// 错误中间件
    /// </summary>
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Method.Equals("OPTIONS"))
            {
                await _next(context);
            }
            else
            {
                try
                {
                    await _next(context);
                }
                catch (Exception ex)
                {
                    var statusCode = context.Response.StatusCode;
                    if (ex is ArgumentException)
                    {
                        statusCode = HttpStatusCode.OK.GetHashCode();
                    }
                    await HandleExceptionAsync(context, ex.Message);
                }
                finally
                {
                    var statusCode = context.Response.StatusCode;
                    var msg = "";
                    if (statusCode == HttpStatusCode.Gone.GetHashCode())
                    {
                        msg = "未授权";
                    }
                    else if (statusCode == HttpStatusCode.NotFound.GetHashCode())
                    {
                        msg = "未找到服务";
                    }
                    else if (statusCode == HttpStatusCode.BadGateway.GetHashCode())
                    {
                        msg = "请求错误";
                    }
                    else if (statusCode == HttpStatusCode.Forbidden.GetHashCode())
                    {
                        msg = "权限不足";
                    }
                    else if (statusCode != HttpStatusCode.OK.GetHashCode())
                    {
                        msg = "未知错误";
                    }

                    if (!string.IsNullOrWhiteSpace(msg))
                    {
                        await HandleExceptionAsync(context, msg);
                    }
                }
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, string msg)
        {
            OperateStatus operateStatus = new OperateStatus
            {
                Msg = msg
            };
            var sta = context.Response.HasStarted;
            if (sta)
            {
                return Task.FromResult(operateStatus);
            }
            var setting = new JsonSerializerSettings
            {
                ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver()
            };
            var result = JsonConvert.SerializeObject(operateStatus, setting);
            context.Response.ContentType = "application/json;charset=utf-8";
            return context.Response.WriteAsync(result);
        }
    }
}