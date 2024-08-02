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
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Authentication;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Newtonsoft.Json;

namespace EIP.Common.Core.Util
{
    /// <summary>
    /// 请求工具
    /// </summary>
    public static class RequestUtil
    {
        /// <summary>
        /// 请求与响应的超时时间
        /// </summary>
        public static int Timeout { get; set; } = 100000;
        /// <summary>
        /// 发起POST同步请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="contentType">application/xml、application/json、application/text、application/x-www-form-urlencoded</param>
        /// <param name="headers">填充消息头</param>        
        /// <returns></returns>
        public static async Task<string> HttpPost(string url, string postData = null, Dictionary<string, string> headers = null, string contentType = "application/json", int timeOut = 30)
        {
            postData = postData ?? "";
            if (contentType == "application/x-www-form-urlencoded")
            {
                postData = JsonUrlEncode(postData);
            }
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback += (sender, cert, chain, sslPolicyErrors) => { return true; };
            clientHandler.SslProtocols = SslProtocols.None;
            using (HttpClient client = new HttpClient(clientHandler))
            {
                if (headers != null)
                {
                    foreach (var header in headers)
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
                using (HttpContent httpContent = new StringContent(postData, Encoding.UTF8))
                {
                    if (contentType != null)
                        httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);

                    HttpResponseMessage response = await client.PostAsync(url, httpContent);
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }
        /// <summary>
        /// 发起POST同步请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="postData"></param>
        /// <param name="contentType">application/xml、application/json、application/text、application/x-www-form-urlencoded</param>
        /// <param name="headers">填充消息头</param>        
        /// <returns></returns>
        public static async Task< string> HttpGet(string url, string postData = null, Dictionary<string, string> headers = null, string contentType = "application/json", int timeOut = 30)
        {
            postData = postData ?? "";
            if (contentType == "application/x-www-form-urlencoded")
            {
                postData = JsonUrlEncode(postData);
            }
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback += (sender, cert, chain, sslPolicyErrors) => { return true; };
            clientHandler.SslProtocols = SslProtocols.None;
            using (HttpClient client = new HttpClient(clientHandler))
            {
                if (headers != null)
                {
                    foreach (var header in headers)
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                }
                using (HttpContent httpContent = new StringContent(postData, Encoding.UTF8))
                {
                    if (contentType != null)
                        httpContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(contentType);

                    HttpResponseMessage response =await client.GetAsync(url);
                    return await response.Content.ReadAsStringAsync();
                }
            }
        }
        /// <summary>
        /// json转urlencode
        /// </summary>
        /// <returns></returns>
        public static string JsonUrlEncode(string json)
        {
            Dictionary<string, object> dic = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            StringBuilder builder = new StringBuilder();
            foreach (KeyValuePair<string, object> item in dic)
            {
                builder.Append(GetFormDataContent(item, ""));
            }
            return builder.ToString().TrimEnd('&');
        }

        /// <summary>
        /// 递归转formdata
        /// </summary>
        /// <param name="item"></param>
        /// <param name="preStr"></param>
        /// <returns></returns>
        private static string GetFormDataContent(KeyValuePair<string, object> item, string preStr)
        {
            StringBuilder builder = new StringBuilder();
            if (string.IsNullOrEmpty(item.Value?.ToString()))
            {
                builder.AppendFormat("{0}={1}", string.IsNullOrEmpty(preStr) ? item.Key : (preStr + "[" + item.Key + "]"), System.Web.HttpUtility.UrlEncode((item.Value == null ? "" : item.Value.ToString()).ToString()));
                builder.Append("&");
            }
            else
            {
                //如果是数组
                if (item.Value.GetType().Name.Equals("JArray"))
                {
                    var children = JsonConvert.DeserializeObject<List<object>>(item.Value.ToString());
                    for (int j = 0; j < children.Count; j++)
                    {
                        Dictionary<string, object> childrendic = JsonConvert.DeserializeObject<Dictionary<string, object>>(JsonConvert.SerializeObject(children[j]));
                        foreach (var row in childrendic)
                        {
                            builder.Append(GetFormDataContent(row, string.IsNullOrEmpty(preStr) ? (item.Key + "[" + j + "]") : (preStr + "[" + item.Key + "][" + j + "]")));
                        }
                    }

                }
                //如果是对象
                else if (item.Value.GetType().Name.Equals("JObject"))
                {
                    Dictionary<string, object> children = JsonConvert.DeserializeObject<Dictionary<string, object>>(item.Value.ToString());
                    foreach (var row in children)
                    {
                        builder.Append(GetFormDataContent(row, string.IsNullOrEmpty(preStr) ? item.Key : (preStr + "[" + item.Key + "]")));
                    }
                }
                //字符串、数字等
                else
                {
                    builder.AppendFormat("{0}={1}", string.IsNullOrEmpty(preStr) ? item.Key : (preStr + "[" + item.Key + "]"), System.Web.HttpUtility.UrlEncode((item.Value == null ? "" : item.Value.ToString()).ToString()));
                    builder.Append("&");
                }
            }

            return builder.ToString();
        }

        /// <summary>
        /// 执行HTTP POST请求。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="data">请求参数:</param>
        /// <param name="header">请求头:"Authorization:Bearer xxxx"</param>
        /// <returns>HTTP响应</returns>
        public static string EventDoByApiPost(string url, object data, string header = null)
        {
            var req = GetWebRequest(url, "POST");
            WebHeaderCollection headers = req.Headers;
            if (header != null)
                headers.Add(header);
            req.ContentType = "application/json";
            req.Accept = "application/json";
            var postData = Encoding.UTF8.GetBytes(data == null ? string.Empty : JsonConvert.SerializeObject(data));
            var reqStream = req.GetRequestStream();
            reqStream.Write(postData, 0, postData.Length);
            reqStream.Close();
            var rsp = (HttpWebResponse)req.GetResponse();
            var encoding = Encoding.GetEncoding(rsp.CharacterSet);
            return GetResponseAsString(rsp, encoding);
        }
        /// <summary>
        /// 执行HTTP GET请求。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="charset">编码字符集</param>
        /// <param name="header">请求头:"Authorization:Bearer xxxx"</param>
        /// <returns>HTTP响应</returns>
        public static string EventDoByApiGet(string url, IDictionary<string, string> parameters, string charset = "utf-8", string header = null)
        {
            if (parameters != null && parameters.Count > 0)
                if (url.Contains("?"))
                    url = url + "&" + BuildQuery(parameters, charset);
                else
                    url = url + "?" + BuildQuery(parameters, charset);
            var req = GetWebRequest(url, "GET");
            WebHeaderCollection headers = req.Headers;
            if (header != null)
                headers.Add(header);
            req.ContentType = "application/x-www-form-urlencoded;charset=" + charset;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var rsp = (HttpWebResponse)req.GetResponse();
            var encoding = Encoding.GetEncoding(rsp.CharacterSet);
            return GetResponseAsString(rsp, encoding);
        }
        /// <summary>
        /// 执行HTTP POST请求。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="data">请求参数:</param>
        /// <param name="header">请求头:"Authorization:Bearer xxxx"</param>
        /// <returns>HTTP响应</returns>
        public static async Task<string> Post(string url, object data, string header = null)
        {
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Add("header", header);
            HttpResponseMessage response = await client.PostAsync(url, new StringContent(JsonConvert.SerializeObject(data)));
            string reslut = await response.Content.ReadAsStringAsync();
            return reslut;
        }

        /// <summary>
        /// 执行HTTP GET请求。
        /// </summary>
        /// <param name="url">请求地址</param>
        /// <param name="parameters">请求参数</param>
        /// <param name="charset">编码字符集</param>
        /// <param name="header">请求头:"Authorization:Bearer xxxx"</param>
        /// <returns>HTTP响应</returns>
        public static async Task<string> Get(string url, IDictionary<string, string> parameters, string charset = "utf-8", string header = null, TimeSpan? timeout = null)
        {
            try
            {
                if (parameters != null && parameters.Count > 0)
                    if (url.Contains("?"))
                        url = url + "&" + BuildQuery(parameters, charset);
                    else
                        url = url + "?" + BuildQuery(parameters, charset);
                HttpClient client = new HttpClient();
                client.Timeout = timeout.HasValue ? (TimeSpan)timeout : TimeSpan.FromSeconds(3);
                client.DefaultRequestHeaders.Add("header", header);
                HttpResponseMessage response = await client.GetAsync(url);
                string reslut = await response.Content.ReadAsStringAsync();
                return reslut;
            }
            catch (System.Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="method"></param>
        /// <param name="header">请求头:"Authorization:Bearer xxxx"</param>
        /// <returns></returns>
        public static HttpWebRequest GetWebRequest(string url, string method, string header = null)
        {
            var req = (HttpWebRequest)WebRequest.Create(url);
            WebHeaderCollection headers = req.Headers;
            if (header != null)
                headers.Add(header);
            req.Method = method;
            req.KeepAlive = true;
            req.UserAgent = "Aop4Net";
            req.Timeout = Timeout;
            return req;
        }

        /// <summary>
        /// 把响应流转换为文本。
        /// </summary>
        /// <param name="rsp">响应流对象</param>
        /// <param name="encoding">编码方式</param>
        /// <returns>响应文本</returns>
        public static string GetResponseAsString(HttpWebResponse rsp, Encoding encoding)
        {
            var result = new StringBuilder();
            Stream stream = null;
            StreamReader reader = null;
            try
            {
                // 以字符流的方式读取HTTP响应
                stream = rsp.GetResponseStream();
                reader = new StreamReader(stream, encoding);
                // 按字符读取并写入字符串缓冲
                var ch = -1;
                while ((ch = reader.Read()) > -1)
                {
                    // 过滤结束符
                    var c = (char)ch;
                    if (c != '\0')
                        result.Append(c);
                }
            }
            finally
            {
                // 释放资源
                reader?.Close();
                stream?.Close();
                rsp?.Close();
            }
            return result.ToString();
        }

        /// <summary>
        /// 组装普通文本请求参数。
        /// </summary>
        /// <param name="parameters">Key-Value形式请求参数字典</param>
        /// <param name="charset"></param>
        /// <returns>URL编码后的请求数据</returns>
        public static string BuildQuery(IDictionary<string, string> parameters, string charset = "utf-8")
        {
            var postData = new StringBuilder();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var hasParam = false;

            var dem = parameters.GetEnumerator();
            while (dem.MoveNext())
            {
                var name = dem.Current.Key;
                var value = dem.Current.Value;
                // 忽略参数名或参数值为空的参数
                if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(value))
                {
                    if (hasParam)
                        postData.Append("&");

                    postData.Append(name);
                    postData.Append("=");
                    var encodedValue = HttpUtility.UrlEncode(value, Encoding.GetEncoding(charset));
                    postData.Append(encodedValue);
                    hasParam = true;
                }
            }
            return postData.ToString();
        }
    }
}