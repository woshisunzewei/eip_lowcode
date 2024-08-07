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

using EIP.Common.Core.Util;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using NLog;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace EIP.Common.Log.Handler
{
    /// <summary>
    /// 说  明:日志记录基类
    /// 备  注:
    /// 编写人:孙泽伟-2015/04/01
    /// </summary>
    /// <typeparam name="TLog">记录日志实体</typeparam>
    public abstract class BaseHandler<TLog>
    {
        #region 构造函数
        /// <summary>
        /// 构造函数初始化
        /// </summary>
        /// <param name="loggerConfig"></param>
        protected BaseHandler(string loggerConfig)
        {
            LoggerConfig = loggerConfig;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 写入日志,虚函数.可进行重写
        /// </summary>
        public virtual void WriteLog()
        {
            if (string.IsNullOrEmpty(LoggerConfig))
            {
                return;
            }
            Task.Factory.StartNew(() => WriteToDb());
        }

        /// <summary>
        /// 利用NLog写入日志到数据库
        /// </summary>
        private void WriteToDb()
        {
            Logger logger = new Logger(LoggerConfig);
            var ei = new EIPLogEventInfo(LogLevel.Info, LoggerConfig, "EIP日志");
            foreach (var item in Log.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
                ei.Properties[item.Name] = item.GetValue(Log, null);
            logger.Log(ei);
        }

        /// <summary>
        /// 获取请求数据
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        protected string RequestData(HttpRequest request)
        {
            string requestData = string.Empty;
            IList<RequestData> datas = new List<RequestData>();
            //判断请求类型
            if (request.Method == HttpMethods.Post)
            {
                //判断请求类型
                var contentType = request.ContentType;
                //内容长度
                var contentLength = request.ContentLength;
                if (contentType == null)
                {
                    //请求长度
                    if (contentLength == 0)
                    {
                        return requestData;
                    }
                }
                //判断是表单提交
                else if (contentType.Contains("form-data") || contentType.Contains("application/x-www-form-urlencoded"))
                {
                    if (request.Form.Count > 0)
                    {
                        foreach (var key in request.Form.Keys)
                        {
                            datas.Add(new RequestData
                            {
                                Name = key,
                                Value = request.Form[key]
                            });
                        }
                    }
                    requestData = JsonConvert.SerializeObject(request.Form);
                    if (request.Form.Files.Count > 0)
                    {
                        datas.Add(new RequestData
                        {
                            Name = "附件",
                            Value = JsonConvert.SerializeObject(request.Form.Files)
                        });
                    }
                    if (datas.Count > 0)
                        requestData = JsonConvert.SerializeObject(datas);
                }
                else if (contentType.Contains("application/json"))
                {
                    requestData = FileUtil.ToString(request.Body, isCloseStream: false);
                }
            }
            else if (request.Method == HttpMethods.Get)
            {
                requestData = request.QueryString.Value;
            }
            return requestData;
        }
        #endregion

        #region 属性

        /// <summary>
        /// 需要启动的日志模式名称
        /// </summary>
        protected string LoggerConfig { get; set; }

        public TLog Log { get; set; }

        #endregion
    }

    public class RequestData
    {
        public string Name { get; set; }

        public object Value { get; set; }
    }
}