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
using EIP.Common.Log;
using EIP.Common.Log.Handler;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace EIP.Common.Controller.Filter
{
    /// <summary>
    /// 全局异常过滤器
    /// </summary>
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILoggerFactory _loggerFactory;
        private readonly IHttpContextAccessor _accessor;
        public ExceptionFilter(ILoggerFactory loggerFactory,
            IHttpContextAccessor accessor)
        {
            _loggerFactory = loggerFactory;
            _accessor = accessor;
        }

        public void OnException(ExceptionContext context)
        {
            IHttpContextAccessor ac = new HttpContextAccessor();
            var descriptionAttribute = context.ActionDescriptor.FilterDescriptors;
            string remark = "", remarkFrom = "";
            foreach (var attr in descriptionAttribute)
            {
                var info = attr.Filter.ToString();
                if (info == "EIP.Common.Controller.Attribute.RemarkAttribute")
                {
                    remark = ((RemarkAttribute)attr.Filter).Describe;
                    remarkFrom = ((RemarkAttribute)attr.Filter).From.ToString();
                }
            }
            var currentUser = ac.CurrentUser();
            var logger = _loggerFactory.CreateLogger(context.Exception.TargetSite.ReflectedType);
            logger.LogError(new EventId(context.Exception.HResult),
                context.Exception,
                context.Exception.Message);
            ExceptionLogHandler handler = new ExceptionLogHandler(context.Exception, _accessor, new LogUser
            {
                UserId = currentUser.UserId.ToString(),
                Code = currentUser.Code,
                Name = currentUser.Name,
                RemoteIp = currentUser.RemoteIp,
                RemoteIpAddress = currentUser.RemoteIpAddress,
                UserAgent = currentUser.UserAgent,
                OsInfo = currentUser.OsInfo,
                Browser = currentUser.Browser,
            }, remark, remarkFrom);
            handler.WriteLog();
        }
    }
}
