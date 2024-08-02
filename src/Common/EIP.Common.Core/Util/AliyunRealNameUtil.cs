﻿using System;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace EIP.Common.Core.Util
{
    public class AliyunRealName
    {
        public int code { get; set; }

        public string msg { get; set; }

        public AliyunRealNameResult data { get; set; }
    }
    public class AliyunRealNameResult
    {
        public int result { get; set; }

        public string desc { get; set; }

        public string sex { get; set; }

        public string birthday { get; set; }

        public string address { get; set; }
    }

    public static class AliyunRealNameUtil
    {
        private const String host = "https://puhui.shumaidata.com";
        private const String path = "/id_card/check/puhui";
        private const String method = "GET";
        private const String appcode = "c8e0c49733ca40e693b5463359b0f2e6";

        public static string Check(string idcard, string name)
        {
            String querys = $"idcard={idcard}&name={name}";
            String bodys = "";
            String url = host + path;
            HttpWebRequest httpRequest = null;
            HttpWebResponse httpResponse = null;

            if (0 < querys.Length)
            {
                url = url + "?" + querys;
            }

            if (host.Contains("https://"))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                httpRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
            }
            else
            {
                httpRequest = (HttpWebRequest)WebRequest.Create(url);
            }
            httpRequest.Method = method;
            httpRequest.Headers.Add("Authorization", "APPCODE " + appcode);
            if (0 < bodys.Length)
            {
                byte[] data = Encoding.UTF8.GetBytes(bodys);
                using (Stream stream = httpRequest.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            try
            {
                httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            }
            catch (WebException ex)
            {
                httpResponse = (HttpWebResponse)ex.Response;
            }
      
            Stream st = httpResponse.GetResponseStream();
            StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
            return reader.ReadToEnd();
        }

        public static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
    }
}
