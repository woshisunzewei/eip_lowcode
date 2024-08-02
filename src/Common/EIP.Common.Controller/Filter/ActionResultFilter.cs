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
using EIP.Common.Controller.Attribute;
using EIP.Common.Controller.Extension;
using EIP.Common.Core.Extension;
using EIP.Common.Log;
using EIP.Common.Log.Handler;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace EIP.Common.Controller.Filter
{
    public class ActionResultFilter : ActionFilterAttribute
    {
        /// <summary>
        /// 用户操作日志
        /// </summary>
        private OperationLogHandler _operationLogHandler;
        private readonly IHttpContextAccessor _accessor;
        public ActionResultFilter(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        /// <summary>
        /// 执行完毕
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            base.OnActionExecuted(context);
            if(_operationLogHandler != null)
            {
                _operationLogHandler.ActionExecuted();
            }
        }

        /// <summary>
        /// 开始执行
        /// </summary>
        /// <param name="context"></param>
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            base.OnActionExecuting(context);

            var descriptionAttribute = context.ActionDescriptor.FilterDescriptors;
            if (!descriptionAttribute.Any())
                return;
            var remarkAttribute = descriptionAttribute.FirstOrDefault(f => f.Filter is RemarkAttribute);
            if (remarkAttribute != null)
            {
                if (((RemarkAttribute)remarkAttribute.Filter).WriteLog)
                {
                    var request = context.HttpContext.Request;
                    var currentUser = _accessor.CurrentUser();
                    _operationLogHandler = new OperationLogHandler(request, new LogUser
                    {
                        UserId = currentUser.UserId.ToString(),
                        Code = currentUser.Code,
                        Name = currentUser.Name,
                        RemoteIp = currentUser.RemoteIp,
                        RemoteIpAddress = currentUser.RemoteIpAddress,
                        UserAgent = currentUser.UserAgent,
                        OsInfo = currentUser.OsInfo,
                        Browser = currentUser.Browser,
                    })
                    {
                        Log = {
                            ControllerName = ((ControllerActionDescriptor) context.ActionDescriptor).ControllerName,
                            ActionName = ((ControllerActionDescriptor) context.ActionDescriptor).ActionName
                        }
                    };
                    _operationLogHandler.Log.Remark = ((RemarkAttribute)remarkAttribute.Filter).Describe;
                    _operationLogHandler.Log.RemarkFrom = ((RemarkAttribute)remarkAttribute.Filter).From.ToString();
                }
            }

            var fieldFilterAttribute = descriptionAttribute.FirstOrDefault(f => f.Filter is FieldFilterAttribute);
            if (fieldFilterAttribute != null)
            {
                //过滤参数防止Sql注入和Xss攻击
                IList<ParameterDescriptor> actionParameters = context.ActionDescriptor.Parameters;
                //遍历参数集合
                foreach (var p in actionParameters)
                {
                    if (context.ActionArguments[p.Name] != null)
                    {
                        //当参数等于字符串
                        if (p.ParameterType.Equals(typeof(string)))
                        {
                            context.ActionArguments[p.Name] = context.ActionArguments[p.Name].ToString().Xss();
                        }
                        else if (p.ParameterType.IsClass)//当参数等于类
                        {
                            ModelFieldFilter(p.Name, p.ParameterType, context.ActionArguments[p.Name]);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 遍历修改类的字符串属性
        /// </summary>
        /// <param name="key">类名</param>
        /// <param name="t">数据类型</param>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        private object ModelFieldFilter(string key, Type t, object obj)
        {
            //获取类的属性集合
            if (obj != null)
            {
                //获取类的属性集合
                var pps = t.GetProperties();
                foreach (var pp in pps)
                {
                    if (pp.GetValue(obj) != null)
                    {
                        //当属性等于字符串
                        if (pp.PropertyType.Equals(typeof(string)) && pp.CanWrite)
                        {
                            string value = pp.GetValue(obj).ToString();
                            pp.SetValue(obj, value.Xss());
                        }
                        else if (pp.PropertyType.IsClass && pp.CanWrite)//当属性等于类进行递归
                        {
                            pp.SetValue(obj, ModelFieldFilter(pp.Name, pp.PropertyType, pp.GetValue(obj)));
                        }
                    }
                }
            }
            return obj;
        }
        /// <summary>
        /// 结果返回开始
        /// </summary>
        /// <param name="context"></param>
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            base.OnResultExecuting(context);
        }

        /// <summary>
        /// 结果返回完毕
        /// </summary>
        /// <param name="context"></param>
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            base.OnResultExecuted(context);
            var response = context.HttpContext.Response;
            if (_operationLogHandler != null && _operationLogHandler.Log.Remark.IsNotNullOrEmpty())
            {
                var descriptionAttribute = context.ActionDescriptor.FilterDescriptors;
                _operationLogHandler.Log.ResponseStatus = response.StatusCode.ToString();
                _operationLogHandler.ResultExecuted();
                if (context.Result is JsonResult result)
                {
                    //序列化json数据
                    string json = JsonConvert.SerializeObject(result.Value);
                    _operationLogHandler.Log.ResponseData = json;
                }

                if (context.Result is OkObjectResult objectResult)
                {
                    _operationLogHandler.Log.ResponseData = JsonConvert.SerializeObject(objectResult);
                }
                _operationLogHandler.WriteLog();
            }
        }
    }
}