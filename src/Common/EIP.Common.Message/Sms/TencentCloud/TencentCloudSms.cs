﻿using EIP.Common.Config;
using EIP.Common.Core.Extension;
using EIP.Common.Message.Sms.Dto;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;

namespace EIP.Common.Message.Sms.TencentCloud
{
    public class TencentCloudSms
    {
        readonly int _sdkappid;
        readonly string _appkey;
        string url = "https://yun.tim.qq.com/v5/tlssmssvr/sendsms";
        private readonly SystemSmsTemplate _smsTemplate;
        private readonly TencentCloudSmsSenderUtil _util = new TencentCloudSmsSenderUtil();
        public TencentCloudSms(int sdkappid, string appkey)
        {
            _sdkappid = sdkappid;
            _appkey = appkey;
        }
        public TencentCloudSms(SystemSmsTemplate smsTemplate)
        {
            _smsTemplate = smsTemplate;
        }
        /// <summary>
        /// 发送短信
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public TencentCloudSmsSingleSenderResult SendSms(SendSmsInput input)
        {
            //通过代码获取短信配置信息
            return _smsTemplate.TemplateId.IsNotNullOrEmpty() ? SendWithParam(input) : Send(input);
        }

        /**
         * 普通单发短信接口，明确指定内容，如果有多个签名，请在内容中以【】的方式添加到信息内容中，否则系统将使用默认签名
         * @param type 短信类型，0 为普通短信，1 营销短信
         * @param nationCode 国家码，如 86 为中国
         * @param phoneNumber 不带国家码的手机号
         * @param msg 信息内容，必须与申请的模板格式一致，否则将返回错误
         * @param extend 扩展码，可填空
         * @param ext 服务端原样返回的参数，可填空
         * @return SmsSingleSenderResult
         */
        public TencentCloudSmsSingleSenderResult Send(SendSmsInput input)
        {
            /*
            请求包体
            {
                "tel": {
                    "nationcode": "86", 
                    "mobile": "13788888888"
                },
                "type": 0, 
                "msg": "你的验证码是1234", 
                "sig": "fdba654e05bc0d15796713a1a1a2318c", 
                "time": 1479888540,
                "extend": "",
                "ext": ""
            }
            应答包体
            {
                "result": 0,
                "errmsg": "OK", 
                "ext": "", 
                "sid": "xxxxxxx", 
                "fee": 1
            }
            */
            if (0 != _smsTemplate.Type && 1 != _smsTemplate.Type)
            {
                throw new Exception("type " + _smsTemplate.Type + " error");
            }
            if (null == input.Extend)
            {
                input.Extend = "";
            }
            if (null == input.Ext)
            {
                input.Ext = "";
            }

            long random = _util.GetRandom();
            long curTime = _util.GetCurTime();

            // 按照协议组织 post 请求包体
            JObject data = new JObject();

            JObject tel = new JObject();
            tel.Add("nationcode", _smsTemplate.NationCode);
            tel.Add("mobile", input.Phone);

            data.Add("tel", tel);
            data.Add("msg", input.Message);
            data.Add("type", _smsTemplate.Type);
            data.Add("sig", _util.StrToHash(String.Format(
                "appkey={0}&random={1}&time={2}&mobile={3}",
                _appkey, random, curTime, input.Phone)));
            data.Add("time", curTime);
            data.Add("extend", input.Extend);
            data.Add("ext", input.Ext);

            string wholeUrl = url + "?sdkappid=" + _sdkappid + "&random=" + random;
            HttpWebRequest request = _util.GetPostHttpConn(wholeUrl);
            byte[] requestData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
            request.ContentLength = requestData.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(requestData, 0, requestData.Length);
            requestStream.Close();

            // 接收返回包
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
            string responseStr = streamReader.ReadToEnd();
            streamReader.Close();
            responseStream.Close();
            TencentCloudSmsSingleSenderResult result;
            if (HttpStatusCode.OK == response.StatusCode)
            {
                result = _util.ResponseStrToSingleSenderResult(responseStr);
            }
            else
            {
                result = new TencentCloudSmsSingleSenderResult();
                result.result = -1;
                result.errmsg = "http error " + response.StatusCode + " " + responseStr;
            }
            return result;
        }

        /**
         * 指定模板单发
         * @param nationCode 国家码，如 86 为中国
         * @param phoneNumber 不带国家码的手机号
         * @param templId 模板 id
         * @param templParams 模板参数列表，如模板 {1}...{2}...{3}，那么需要带三个参数
         * @param extend 扩展码，可填空
         * @param ext 服务端原样返回的参数，可填空
         * @return SmsSingleSenderResult
         */
        public TencentCloudSmsSingleSenderResult SendWithParam(SendSmsInput input)
        {
            /*
            请求包体
            {
                "tel": {
                    "nationcode": "86",
                    "mobile": "13788888888"
                },
                "sign": "腾讯云",
                "tpl_id": 19,
                "params": [
                    "验证码", 
                    "1234",
                    "4"
                ],
                "sig": "fdba654e05bc0d15796713a1a1a2318c",
                "time": 1479888540,
                "extend": "",
                "ext": ""
            }
            应答包体
            {
                "result": 0,
                "errmsg": "OK", 
                "ext": "", 
                "sid": "xxxxxxx", 
                "fee": 1
            }
            */
            if (null == _smsTemplate.Sign)
            {
                _smsTemplate.Sign = "";
            }
            if (null == input.Extend)
            {
                input.Extend = "";
            }
            if (null == input.Ext)
            {
                input.Ext = "";
            }

            long random = _util.GetRandom();
            long curTime = _util.GetCurTime();

            // 按照协议组织 post 请求包体
            JObject data = new JObject();

            JObject tel = new JObject();
            tel.Add("nationcode", _smsTemplate.NationCode);
            tel.Add("mobile", input.Phone);

            data.Add("tel", tel);
            data.Add("sig", _util.CalculateSigForTempl(_appkey, random, curTime, input.Phone));
            data.Add("tpl_id", _smsTemplate.TemplateId);
            data.Add("params", _util.SmsParamsToJSONArray(input.TemplParams.Select(s => s.Value).ToList()));
            data.Add("sign", _smsTemplate.Sign);
            data.Add("time", curTime);
            data.Add("extend", input.Extend);
            data.Add("ext", input.Ext);

            string wholeUrl = url + "?sdkappid=" + _sdkappid + "&random=" + random;
            HttpWebRequest request = _util.GetPostHttpConn(wholeUrl);
            byte[] requestData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
            request.ContentLength = requestData.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(requestData, 0, requestData.Length);
            requestStream.Close();

            // 接收返回包
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
            string responseStr = streamReader.ReadToEnd();
            streamReader.Close();
            responseStream.Close();
            TencentCloudSmsSingleSenderResult result;
            if (HttpStatusCode.OK == response.StatusCode)
            {
                result = _util.ResponseStrToSingleSenderResult(responseStr);
            }
            else
            {
                result = new TencentCloudSmsSingleSenderResult();
                result.result = -1;
                result.errmsg = "http error " + response.StatusCode + " " + responseStr;
            }
            return result;
        }
    }

    public class TencentCloudSmsSingleSenderResult
    {
        /*
        {
            "result": 0,
            "errmsg": "OK", 
            "ext": "", 
            "sid": "xxxxxxx", 
            "fee": 1
        }
         */
        public int result { set; get; }
        public string errmsg { set; get; }
        public string ext { set; get; }
        public string sid { set; get; }
        public int fee { set; get; }

        public override string ToString()
        {
            return string.Format(
                "SmsSingleSenderResult\nresult {0}\nerrMsg {1}\next {2}\nsid {3}\nfee {4}",
                result, errmsg, ext, sid, fee);
        }
    }

    public class TencentCloudSmsMultiSender
    {
        int sdkappid;
        string appkey;
        string url = "https://yun.tim.qq.com/v5/tlssmssvr/sendmultisms2";

        TencentCloudSmsSenderUtil util = new TencentCloudSmsSenderUtil();

        public TencentCloudSmsMultiSender(int sdkappid, string appkey)
        {
            this.sdkappid = sdkappid;
            this.appkey = appkey;
        }

        /**
         * 普通群发短信接口，明确指定内容，如果有多个签名，请在内容中以【】的方式添加到信息内容中，否则系统将使用默认签名
         * 【注意】海外短信无群发功能
         * @param type 短信类型，0 为普通短信，1 营销短信
         * @param nationCode 国家码，如 86 为中国
         * @param phoneNumbers 不带国家码的手机号列表
         * @param msg 信息内容，必须与申请的模板格式一致，否则将返回错误
         * @param extend 扩展码，可填空
         * @param ext 服务端原样返回的参数，可填空
         * @return SmsMultiSenderResult
         */
        public TencentCloudSmsMultiSenderResult Send(
            int type,
            string nationCode,
            List<string> phoneNumbers,
            string msg,
            string extend,
            string ext)
        {
            /*
            请求包体
            {
                "tel": [
                    {
                        "nationcode": "86", 
                        "mobile": "13788888888"
                    }, 
                    {
                        "nationcode": "86", 
                        "mobile": "13788888889"
                    }
                ], 
                "type": 0, 
                "msg": "你的验证码是1234", 
                "sig": "fdba654e05bc0d15796713a1a1a2318c",
                "time": 1479888540,
                "extend": "", 
                "ext": ""
            }
            应答包体
            {
                "result": 0, 
                "errmsg": "OK", 
                "ext": "", 
                "detail": [
                    {
                        "result": 0, 
                        "errmsg": "OK", 
                        "mobile": "13788888888", 
                        "nationcode": "86", 
                        "sid": "xxxxxxx", 
                        "fee": 1
                    }, 
                    {
                        "result": 0, 
                        "errmsg": "OK", 
                        "mobile": "13788888889", 
                        "nationcode": "86", 
                        "sid": "xxxxxxx", 
                        "fee": 1
                    }
                ]
            }
            */
            if (0 != type && 1 != type)
            {
                throw new Exception("type " + type + " error");
            }
            if (null == extend)
            {
                extend = "";
            }
            if (null == ext)
            {
                ext = "";
            }

            long random = util.GetRandom();
            long curTime = util.GetCurTime();

            // 按照协议组织 post 请求包体
            JObject data = new JObject();
            data.Add("tel", util.PhoneNumbersToJSONArray(nationCode, phoneNumbers));
            data.Add("type", type);
            data.Add("msg", msg);
            data.Add("sig", util.CalculateSig(appkey, random, curTime, phoneNumbers));
            data.Add("time", curTime);
            data.Add("extend", extend);
            data.Add("ext", ext);

            string wholeUrl = url + "?sdkappid=" + sdkappid + "&random=" + random;
            HttpWebRequest request = util.GetPostHttpConn(wholeUrl);
            byte[] requestData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
            request.ContentLength = requestData.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(requestData, 0, requestData.Length);
            requestStream.Close();

            // 接收返回包
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
            string responseStr = streamReader.ReadToEnd();
            streamReader.Close();
            responseStream.Close();
            TencentCloudSmsMultiSenderResult result;
            if (HttpStatusCode.OK == response.StatusCode)
            {
                result = util.ResponseStrToMultiSenderResult(responseStr);
            }
            else
            {
                result = new TencentCloudSmsMultiSenderResult();
                result.result = -1;
                result.errmsg = "http error " + response.StatusCode + " " + responseStr;
            }
            return result;
        }

        /**
         * 指定模板群发
         * 【注意】海外短信无群发功能
         * @param nationCode 国家码，如 86 为中国
         * @param phoneNumbers 不带国家码的手机号列表
         * @param templId 模板 id
         * @param params 模板参数列表
         * @param sign 签名，如果填空，系统会使用默认签名
         * @param extend 扩展码，可以填空
         * @param ext 服务端原样返回的参数，可以填空
         * @return SmsMultiSenderResult
         */
        public TencentCloudSmsMultiSenderResult SendWithParam(
            String nationCode,
            List<string> phoneNumbers,
            int templId,
            List<string> templParams,
            string sign,
            string extend,
            string ext)
        {
            /*
            请求包体
            {
                "tel": [
                    {
                        "nationcode": "86", 
                        "mobile": "13788888888"
                    }, 
                    {
                        "nationcode": "86", 
                        "mobile": "13788888889"
                    }
                ], 
                "type": 0, 
                "msg": "你的验证码是1234", 
                "sig": "fdba654e05bc0d15796713a1a1a2318c",
                "time": 1479888540,
                "extend": "", 
                "ext": ""
            }
            应答包体
            {
                "result": 0, 
                "errmsg": "OK", 
                "ext": "", 
                "detail": [
                    {
                        "result": 0, 
                        "errmsg": "OK", 
                        "mobile": "13788888888", 
                        "nationcode": "86", 
                        "sid": "xxxxxxx", 
                        "fee": 1
                    }, 
                    {
                        "result": 0, 
                        "errmsg": "OK", 
                        "mobile": "13788888889", 
                        "nationcode": "86", 
                        "sid": "xxxxxxx", 
                        "fee": 1
                    }
                ]
            }
            */
            if (null == sign)
            {
                sign = "";
            }
            if (null == extend)
            {
                extend = "";
            }
            if (null == ext)
            {
                ext = "";
            }

            long random = util.GetRandom();
            long curTime = util.GetCurTime();

            // 按照协议组织 post 请求包体
            JObject data = new JObject();
            data.Add("tel", util.PhoneNumbersToJSONArray(nationCode, phoneNumbers));
            data.Add("sig", util.CalculateSigForTempl(appkey, random, curTime, phoneNumbers));
            data.Add("tpl_id", templId);
            data.Add("params", util.SmsParamsToJSONArray(templParams));
            data.Add("sign", sign);
            data.Add("time", curTime);
            data.Add("extend", extend);
            data.Add("ext", ext);

            string wholeUrl = url + "?sdkappid=" + sdkappid + "&random=" + random;
            HttpWebRequest request = util.GetPostHttpConn(wholeUrl);
            byte[] requestData = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(data));
            request.ContentLength = requestData.Length;
            Stream requestStream = request.GetRequestStream();
            requestStream.Write(requestData, 0, requestData.Length);
            requestStream.Close();

            // 接收返回包
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding("utf-8"));
            string responseStr = streamReader.ReadToEnd();
            streamReader.Close();
            responseStream.Close();
            TencentCloudSmsMultiSenderResult result;
            if (HttpStatusCode.OK == response.StatusCode)
            {
                result = util.ResponseStrToMultiSenderResult(responseStr);
            }
            else
            {
                result = new TencentCloudSmsMultiSenderResult();
                result.result = -1;
                result.errmsg = "http error " + response.StatusCode + " " + responseStr;
            }
            return result;
        }
    }

    public class TencentCloudSmsMultiSenderResult
    {
        /*
        {
            "result": 0, 
            "errmsg": "OK", 
            "ext": "", 
            "detail": [
                {
                    "result": 0, 
                    "errmsg": "OK", 
                    "mobile": "13788888888", 
                    "nationcode": "86", 
                    "sid": "xxxxxxx", 
                    "fee": 1
                }, 
                {
                    "result": 0, 
                    "errmsg": "OK", 
                    "mobile": "13788888889", 
                    "nationcode": "86", 
                    "sid": "xxxxxxx", 
                    "fee": 1
                }
            ]
        }
            */
        public class Detail
        {
            public int result { get; set; }
            public string errmsg { get; set; }
            public string mobile { get; set; }
            public string nationcode { get; set; }
            public string sid { get; set; }
            public int fee { get; set; }

            public override string ToString()
            {
                return string.Format(
                        "\tDetail result {0} errmsg {1} mobile {2} nationcode {3} sid {4} fee {5}",
                        result, errmsg, mobile, nationcode, sid, fee);
            }
        }

        public int result;
        public string errmsg = "";
        public string ext = "";
        public IList<Detail> detail;

        public override string ToString()
        {
            if (null != detail)
            {
                return String.Format(
                        "SmsMultiSenderResult\nresult {0}\nerrmsg {1}\next {2}\ndetail:\n{3}",
                        result, errmsg, ext, String.Join("\n", detail));
            }
            else
            {
                return String.Format(
                     "SmsMultiSenderResult\nresult {0}\nerrmsg {1}\next {2}\n",
                     result, errmsg, ext);
            }
        }
    }

    public class TencentCloudSmsSenderUtil
    {
        Random random = new Random();

        public HttpWebRequest GetPostHttpConn(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            return request;
        }

        public long GetRandom()
        {
            return random.Next(999999) % 900000 + 100000;
        }

        public long GetCurTime()
        {
            Int32 unixTimestamp = (Int32)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1))).TotalSeconds;
            return unixTimestamp;
        }

        // 将二进制的数值转换为 16 进制字符串，如 "abc" => "616263"
        private static string ByteArrayToHex(byte[] byteArray)
        {
            string returnStr = "";
            if (byteArray != null)
            {
                for (int i = 0; i < byteArray.Length; i++)
                {
                    returnStr += byteArray[i].ToString("x2");
                }
            }
            return returnStr;
        }

        public string StrToHash(string str)
        {
            SHA256 sha256 = SHA256Managed.Create();
            byte[] resultByteArray = sha256.ComputeHash(System.Text.Encoding.UTF8.GetBytes(str));
            return ByteArrayToHex(resultByteArray);
        }

        // 将单发回包解析成结果对象
        public TencentCloudSmsSingleSenderResult ResponseStrToSingleSenderResult(string str)
        {
            TencentCloudSmsSingleSenderResult result = JsonConvert.DeserializeObject<TencentCloudSmsSingleSenderResult>(str);
            return result;
        }

        // 将群发回包解析成结果对象
        public TencentCloudSmsMultiSenderResult ResponseStrToMultiSenderResult(string str)
        {
            TencentCloudSmsMultiSenderResult result = JsonConvert.DeserializeObject<TencentCloudSmsMultiSenderResult>(str);
            return result;
        }

        public JArray SmsParamsToJSONArray(List<string> templParams)
        {
            JArray smsParams = new JArray();
            foreach (string templParamsElement in templParams)
            {
                smsParams.Add(templParamsElement);
            }
            return smsParams;
        }

        public JArray PhoneNumbersToJSONArray(string nationCode, List<string> phoneNumbers)
        {
            JArray tel = new JArray();
            int i = 0;
            do
            {
                JObject telElement = new JObject();
                telElement.Add("nationcode", nationCode);
                telElement.Add("mobile", phoneNumbers.ElementAt(i));
                tel.Add(telElement);
            } while (++i < phoneNumbers.Count);

            return tel;
        }

        public string CalculateSigForTempl(
            string appkey,
            long random,
            long curTime,
            List<string> phoneNumbers)
        {
            string phoneNumbersString = phoneNumbers.ElementAt(0);
            for (int i = 1; i < phoneNumbers.Count; i++)
            {
                phoneNumbersString += "," + phoneNumbers.ElementAt(i);
            }
            return StrToHash(String.Format(
                "appkey={0}&random={1}&time={2}&mobile={3}",
                appkey, random, curTime, phoneNumbersString));
        }

        public string CalculateSigForTempl(
            string appkey,
            long random,
            long curTime,
            string phoneNumber)
        {
            List<string> phoneNumbers = new List<string>();
            phoneNumbers.Add(phoneNumber);
            return CalculateSigForTempl(appkey, random, curTime, phoneNumbers);
        }

        public string CalculateSig(
            string appkey,
            long random,
            long curTime,
            List<string> phoneNumbers)
        {
            string phoneNumbersString = phoneNumbers.ElementAt(0);
            for (int i = 1; i < phoneNumbers.Count; i++)
            {
                phoneNumbersString += "," + phoneNumbers.ElementAt(i);
            }
            return StrToHash(String.Format(
                    "appkey={0}&random={1}&time={2}&mobile={3}",
                    appkey, random, curTime, phoneNumbersString));
        }
    }

}
