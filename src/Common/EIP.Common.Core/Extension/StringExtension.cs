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
using AngleSharp.Dom;
using EIP.Common.Core.Extension.Xss;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Collections.Generic;

namespace EIP.Common.Core.Extension
{
    public static class StringExtension
    {
        /// <summary>
        /// 将object转换为long，若失败则返回0
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static long ParseToLong(this object obj)
        {
            try
            {
                return long.Parse(obj.ToString());
            }
            catch
            {
                return 0L;
            }
        }

        /// <summary>
        /// 将object转换为long，若失败则返回指定值
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static long ParseToLong(this string str, long defaultValue)
        {
            try
            {
                return long.Parse(str);
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 将object转换为double，若失败则返回0
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static double ParseToDouble(this object obj)
        {
            try
            {
                return double.Parse(obj.ToString());
            }
            catch
            {
                return 0;
            }
        }

        /// <summary>
        /// 将object转换为double，若失败则返回指定值
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static double ParseToDouble(this object str, double defaultValue)
        {
            try
            {
                return double.Parse(str.ToString());
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// 将string转换为DateTime，若失败则返回日期最小值
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static DateTime ParseToDateTime(this string str)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    return DateTime.MinValue;
                }
                if (str.Contains('-') || str.Contains('/'))
                {
                    return DateTime.Parse(str);
                }
                else
                {
                    int length = str.Length;
                    switch (length)
                    {
                        case 4:
                            return DateTime.ParseExact(str, "yyyy", System.Globalization.CultureInfo.CurrentCulture);

                        case 6:
                            return DateTime.ParseExact(str, "yyyyMM", System.Globalization.CultureInfo.CurrentCulture);

                        case 8:
                            return DateTime.ParseExact(str, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);

                        case 10:
                            return DateTime.ParseExact(str, "yyyyMMddHH", System.Globalization.CultureInfo.CurrentCulture);

                        case 12:
                            return DateTime.ParseExact(str, "yyyyMMddHHmm", System.Globalization.CultureInfo.CurrentCulture);

                        case 14:
                            return DateTime.ParseExact(str, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);

                        default:
                            return DateTime.ParseExact(str, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                    }
                }
            }
            catch
            {
                return DateTime.MinValue;
            }
        }

        /// <summary>
        /// 将string转换为DateTime，若失败则返回默认值
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime ParseToDateTime(this string str, DateTime? defaultValue)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(str))
                {
                    return defaultValue.GetValueOrDefault();
                }
                if (str.Contains('-') || str.Contains('/'))
                {
                    return DateTime.Parse(str);
                }
                else
                {
                    int length = str.Length;
                    switch (length)
                    {
                        case 4:
                            return DateTime.ParseExact(str, "yyyy", System.Globalization.CultureInfo.CurrentCulture);

                        case 6:
                            return DateTime.ParseExact(str, "yyyyMM", System.Globalization.CultureInfo.CurrentCulture);

                        case 8:
                            return DateTime.ParseExact(str, "yyyyMMdd", System.Globalization.CultureInfo.CurrentCulture);

                        case 10:
                            return DateTime.ParseExact(str, "yyyyMMddHH", System.Globalization.CultureInfo.CurrentCulture);

                        case 12:
                            return DateTime.ParseExact(str, "yyyyMMddHHmm", System.Globalization.CultureInfo.CurrentCulture);

                        case 14:
                            return DateTime.ParseExact(str, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);

                        default:
                            return DateTime.ParseExact(str, "yyyyMMddHHmmss", System.Globalization.CultureInfo.CurrentCulture);
                    }
                }
            }
            catch
            {
                return defaultValue.GetValueOrDefault();
            }
        }
        /// <summary>
        /// 替换
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string RTrim(this string str)
        {
            for (int i = str.Length; i >= 0; i--)
            {
                if (str[i].Equals(" ") || str[i].Equals("\r") || str[i].Equals("\n"))
                {
                    str.Remove(i, 1);
                }
            }
            return str;
        }

        /// <summary>
        /// 判断字符串是否相等
        /// </summary>
        /// <param name="text1"></param>
        /// <param name="text2"></param>
        /// <returns></returns>
        public static bool EqualsEx(this string text1, string text2)
        {
            return string.Equals(text1, text2, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// 根据传入的字符串组装为符合更新或者删除sql语句in部分的字符串(字符串必须以‘,’分割)
        /// </summary>
        /// <param name="value">扩展类型</param>
        /// <returns>替换后的字符串</returns>
        /// <remarks>2015-05-15 by 孙泽伟</remarks>
        public static string InSql(this string value)
        {
            string param = "";
            if (!string.IsNullOrEmpty(value))
            {
                var strList = value.Split(',');
                param = strList.Aggregate(param, (current, str) => current + ("'" + str + "',"));
            }
            return param.TrimEnd(',');
        }

        /// <summary>
        /// 判断字符串是否空
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsNotNullOrEmpty(this string text)
        {
            return !string.IsNullOrEmpty(text);
        }

        /// <summary>
        /// 指示指定的字符串是 null、空还是仅由空白字符组成。
        /// </summary>
        public static bool IsNotNullOrWhiteSpace(this string value)
        {
            return !string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// 判断是否包含字符串
        /// </summary>
        /// <param name="text"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool ContainsEx(this string text, string value)
        {
            return text.IndexOf(value, StringComparison.CurrentCultureIgnoreCase) >= 0;
        }

        /// <summary>
        /// 判断是否以指定字符串开头
        /// </summary>
        /// <param name="text"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool StartWithEx(this string text, string value)
        {
            return text.StartsWith(value, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// 判断是否以指定字符串结尾
        /// </summary>
        /// <param name="text"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool EndWithEx(this string text, string value)
        {
            return text.EndsWith(value, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// 判断字符串是否空
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string text)
        {
            return string.IsNullOrEmpty(text);
        }

        /// <summary>
        /// 指示指定的字符串是 null、空还是仅由空白字符组成。
        /// </summary>
        public static bool IsNullOrWhiteSpace(this string value)
        {
            return string.IsNullOrWhiteSpace(value);
        }

        /// <summary>
        /// 以指定字符串作为分隔符将指定字符串分隔成数组
        /// </summary>
        /// <param name="value">要分割的字符串</param>
        /// <param name="strSplit">字符串类型的分隔符</param>
        /// <param name="removeEmptyEntries">是否移除数据中元素为空字符串的项</param>
        /// <returns>分割后的数据</returns>
        public static string[] Split(this string value, string strSplit, bool removeEmptyEntries = false)
        {
            return value.Split(new[] { strSplit },
                removeEmptyEntries ? StringSplitOptions.RemoveEmptyEntries : StringSplitOptions.None);
        }

        /// <summary>
        /// 替换Html标签
        /// </summary>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <returns>替换后的字符串</returns>
        public static string ReplaceHtmlTag(this string value, int length = 0)
        {
            if (value.IsNullOrEmpty()) return value;
            string strText = Regex.Replace(value, "<[^>]+>", "");
            strText = Regex.Replace(strText, "&[^;]+;", "");
            if (length > 0 && strText.Length > length)
                return strText.Substring(0, length);
            return strText;
        }
        /// <summary>
        /// 替换Html标签
        /// </summary>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <returns>替换后的字符串</returns>
        public static string ReplaceEditor(this string value)
        {
            return value.Replace("<p>", "").Replace("</p>", "").Replace(" class=\"non-editable\" contenteditable=\"false\"", "");
        }
        /// <summary>
        /// 替换Html标签
        /// </summary>
        /// <param name="value"></param>
        /// <param name="length"></param>
        /// <returns>替换后的字符串</returns>
        public static HtmlNodeCollection GetNodes(this string value)
        {
            value = value.Replace("<p>", "").Replace("</p>", "").Replace(" class=\"non-editable\" contenteditable=\"false\"", "");
            var doc = new HtmlDocument();
            doc.LoadHtml(value);
            return doc.DocumentNode.SelectNodes("//span");
        }

        /// <summary>
        /// 首字母小写
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ReplaceFistLower(this string value)
        {
            return value.Substring(0, 1).ToLower() + value.Substring(1);
        }
        /// <summary>
        /// 首字母大写
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ReplaceFistUpper(this string value)
        {
            return value.Substring(0, 1).ToUpper() + value.Substring(1);
        }
        /// <summary>
        /// 首字母小写
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string Xss(this string value)
        {
            HtmlSanitizer sanitizer = new HtmlSanitizer();
            sanitizer = new HtmlSanitizer();
            //sanitizer.AllowedTags.Add("img");//标签白名单
            //sanitizer.AllowedAttributes.Add("src");//标签属性白名单,默认没有class标签属性      
            //sanitizer.AllowedCssProperties.Add("font-family");//CSS属性白名单
            value = sanitizer.Sanitize(value);
            return value;

        }

        /// <summary>
        /// json字符串转table
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static DataTable JsonToDataTable(this string json)
        {
            DataTable table = new DataTable();
            //JsonStr为Json字符串
            JArray array = JsonConvert.DeserializeObject(json) as JArray;//反序列化为数组
            if (array.Count > 0)
            {
                StringBuilder columns = new StringBuilder();

                JObject objColumns = array[0] as JObject;
                //构造表头
                foreach (JToken jkon in objColumns.AsEnumerable<JToken>())
                {
                    string name = ((JProperty)(jkon)).Name;
                    columns.Append(name + ",");
                    table.Columns.Add(name);
                }
                //向表中添加数据
                for (int i = 0; i < array.Count; i++)
                {
                    DataRow row = table.NewRow();
                    JObject obj = array[i] as JObject;
                    foreach (JToken jkon in obj.AsEnumerable<JToken>())
                    {

                        string name = ((JProperty)(jkon)).Name;
                        string value = ((JProperty)(jkon)).Value.ToString();
                        row[name] = value;
                    }
                    table.Rows.Add(row);
                }
            }
            return table;
        }

        /// <summary>
        /// json字符串转table
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static IEnumerable<IDictionary<string, object>> JsonToDictionary(this string json)
        {
            IList<IDictionary<string, object>> table = new List<IDictionary<string, object>>();
            //JsonStr为Json字符串
            JArray array = JsonConvert.DeserializeObject(json) as JArray;//反序列化为数组
            if (array.Count > 0)
            {
                //向表中添加数据
                for (int i = 0; i < array.Count; i++)
                {
                    IDictionary<string, object> keyValues=new Dictionary<string, object>();
                    JObject obj = array[i] as JObject;
                    foreach (JToken jkon in obj.AsEnumerable<JToken>())
                    {
                        string name = ((JProperty)(jkon)).Name;
                        string value = ((JProperty)(jkon)).Value.ToString();
                        keyValues.Add(name, value);
                    }
                    table.Add(keyValues);
                }
            }
            return table;
        }
        /// <summary>
        /// 过滤Sql
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string FilterSql(this string sqlParameter)
        {
            if (string.IsNullOrEmpty(sqlParameter)) return string.Empty;
            sqlParameter = sqlParameter.Trim();
            sqlParameter = sqlParameter.Replace("--", "", StringComparison.OrdinalIgnoreCase);
            sqlParameter = sqlParameter.Replace("'", "", StringComparison.OrdinalIgnoreCase);
            sqlParameter = sqlParameter.Replace("!", "", StringComparison.OrdinalIgnoreCase);
            sqlParameter = sqlParameter.Replace("@@", "", StringComparison.OrdinalIgnoreCase);
            sqlParameter = sqlParameter.Replace("^", "", StringComparison.OrdinalIgnoreCase);
            sqlParameter = sqlParameter.Replace("<", "", StringComparison.OrdinalIgnoreCase);
            sqlParameter = sqlParameter.Replace(">", "", StringComparison.OrdinalIgnoreCase);
            sqlParameter = sqlParameter.Replace("%", "", StringComparison.OrdinalIgnoreCase);
            sqlParameter = sqlParameter.Replace("=", "", StringComparison.OrdinalIgnoreCase);
            sqlParameter = sqlParameter.Replace(";", "", StringComparison.OrdinalIgnoreCase);

            //操作
            //sqlParameter = sqlParameter.Replace("insert", "", StringComparison.OrdinalIgnoreCase);
            //sqlParameter = sqlParameter.Replace("update", "", StringComparison.OrdinalIgnoreCase);
            //sqlParameter = sqlParameter.Replace("delete", "", StringComparison.OrdinalIgnoreCase);
            //sqlParameter = sqlParameter.Replace("drop", "", StringComparison.OrdinalIgnoreCase);
            //sqlParameter = sqlParameter.Replace("exec", "", StringComparison.OrdinalIgnoreCase);
            //sqlParameter = sqlParameter.Replace("create", "", StringComparison.OrdinalIgnoreCase);
            //sqlParameter = sqlParameter.Replace("union", "", StringComparison.OrdinalIgnoreCase);
            //sqlParameter = sqlParameter.Replace("select", "", StringComparison.OrdinalIgnoreCase);
            //sqlParameter = sqlParameter.Replace("execute", "", StringComparison.OrdinalIgnoreCase);
            //sqlParameter = sqlParameter.Replace("backup", "", StringComparison.OrdinalIgnoreCase);
            //sqlParameter = sqlParameter.Replace("or", "", StringComparison.OrdinalIgnoreCase);

            //命令
            sqlParameter = sqlParameter.Replace("xp_", "", StringComparison.OrdinalIgnoreCase);
            sqlParameter = sqlParameter.Replace("sp_", "", StringComparison.OrdinalIgnoreCase);
            sqlParameter = sqlParameter.Replace("db_", "", StringComparison.OrdinalIgnoreCase);
            sqlParameter = sqlParameter.Replace("is_", "", StringComparison.OrdinalIgnoreCase);
            sqlParameter = sqlParameter.Replace("host_", "", StringComparison.OrdinalIgnoreCase);

            //表
            sqlParameter = sqlParameter.Replace("sysdatabases", "", StringComparison.OrdinalIgnoreCase);
            sqlParameter = sqlParameter.Replace("sysobjects", "", StringComparison.OrdinalIgnoreCase);
            sqlParameter = sqlParameter.Replace("syscolumns", "", StringComparison.OrdinalIgnoreCase);
            sqlParameter = sqlParameter.Replace("tempdb", "", StringComparison.OrdinalIgnoreCase);

            //函数
            //sqlParameter = sqlParameter.Replace("asc", "", StringComparison.OrdinalIgnoreCase);
            //sqlParameter = sqlParameter.Replace("abc", "", StringComparison.OrdinalIgnoreCase);
            //sqlParameter = sqlParameter.Replace("unicode", "", StringComparison.OrdinalIgnoreCase);
            //sqlParameter = sqlParameter.Replace("nchar", "", StringComparison.OrdinalIgnoreCase);
            //sqlParameter = sqlParameter.Replace("substring", "", StringComparison.OrdinalIgnoreCase);
            //sqlParameter = sqlParameter.Replace("use", "", StringComparison.OrdinalIgnoreCase);
            //sqlParameter = sqlParameter.Replace("count", "", StringComparison.OrdinalIgnoreCase);
            //sqlParameter = sqlParameter.Replace("len", "", StringComparison.OrdinalIgnoreCase);
            //sqlParameter = sqlParameter.Replace("ascii", "", StringComparison.OrdinalIgnoreCase);
            //sqlParameter = sqlParameter.Replace("cast", "", StringComparison.OrdinalIgnoreCase);
            //sqlParameter = sqlParameter.Replace("exists", "", StringComparison.OrdinalIgnoreCase);
            //sqlParameter = sqlParameter.Replace("is_member", "", StringComparison.OrdinalIgnoreCase);
            //sqlParameter = sqlParameter.Replace("is_srvrolemember", "", StringComparison.OrdinalIgnoreCase);

            //关键词
            //sqlParameter = sqlParameter.Replace("and", "", StringComparison.OrdinalIgnoreCase);
            //sqlParameter = sqlParameter.Replace("where", "", StringComparison.OrdinalIgnoreCase);
            //sqlParameter = sqlParameter.Replace("xtype", "", StringComparison.OrdinalIgnoreCase);
            //sqlParameter = sqlParameter.Replace("inner", "", StringComparison.OrdinalIgnoreCase);
            //sqlParameter = sqlParameter.Replace("join", "", StringComparison.OrdinalIgnoreCase);
            //sqlParameter = sqlParameter.Replace("output ", "", StringComparison.OrdinalIgnoreCase);
            //sqlParameter = sqlParameter.Replace("with", "", StringComparison.OrdinalIgnoreCase);
            //sqlParameter = sqlParameter.Replace("master", "", StringComparison.OrdinalIgnoreCase);
            //sqlParameter = sqlParameter.Replace("truncate", "", StringComparison.OrdinalIgnoreCase);
            //sqlParameter = sqlParameter.Replace("declare", "", StringComparison.OrdinalIgnoreCase);
            //sqlParameter = sqlParameter.Replace("array", "", StringComparison.OrdinalIgnoreCase);
            //sqlParameter = sqlParameter.Replace("alter", "", StringComparison.OrdinalIgnoreCase);
            //sqlParameter = sqlParameter.Replace("database", "", StringComparison.OrdinalIgnoreCase);
            //sqlParameter = sqlParameter.Replace("set", "", StringComparison.OrdinalIgnoreCase);
            //sqlParameter = sqlParameter.Replace("dbid", "", StringComparison.OrdinalIgnoreCase);
            //sqlParameter = sqlParameter.Replace("top", "", StringComparison.OrdinalIgnoreCase);
            //sqlParameter = sqlParameter.Replace("delay ", "", StringComparison.OrdinalIgnoreCase);
            //sqlParameter = sqlParameter.Replace("waitfor", "", StringComparison.OrdinalIgnoreCase);
            //sqlParameter = sqlParameter.Replace("order", "", StringComparison.OrdinalIgnoreCase);
            //sqlParameter = sqlParameter.Replace("sysadmin", "", StringComparison.OrdinalIgnoreCase);
            //sqlParameter = sqlParameter.Replace("for", "", StringComparison.OrdinalIgnoreCase);
            //sqlParameter = sqlParameter.Replace("@echo", "", StringComparison.OrdinalIgnoreCase);
            //sqlParameter = sqlParameter.Replace("procedure", "", StringComparison.OrdinalIgnoreCase);
            //sqlParameter = sqlParameter.Replace("assembly", "", StringComparison.OrdinalIgnoreCase);
            return sqlParameter.Xss();
        }

        /// <summary>
        /// 反解密
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string UnEscape(this string value)
        {
            return HttpUtility.UrlDecode(value);
        }
        /// <summary>
        /// 将Base64字符串转换为图片并保存:base64字符串图片为png
        /// </summary>
        /// <param name="value"></param>
        /// <param name="path">保存图片的路径</param>
        public static void ConvertBase64ToImage(this string value, string path)
        {
            string filepath = Path.GetDirectoryName(path);
            // 如果不存在就创建file文件夹  
            if (!Directory.Exists(filepath))
            {
                if (filepath != null) Directory.CreateDirectory(filepath);
            }
            var match = Regex.Match(value, "data:image/png;base64,([\\w\\W]*)$");
            if (match.Success)
            {
                value = match.Groups[1].Value;
            }
            var photoBytes = Convert.FromBase64String(value);
            File.WriteAllBytes(path, photoBytes);
        }

        #region 脱敏
        /// <summary>
        /// 子掩码转换
        /// </summary>
        /// <param name="value">扩展类型</param>
        /// <returns>替换后的字符串</returns>
        public static string ConvertMask(this string value, string type)
        {
            if (value.IsNullOrEmpty())
            {
                return value;
            }
            switch (type)
            {
                case "all":
                case "money":
                    return HideSensitiveInfo(value, 0, 0);
                case "name":
                    return HideSensitiveInfo(value, 1, 1);
                case "phone":
                    return HideSensitiveInfo(value, 3, 4);
                case "idcard":
                    return HideSensitiveInfo(value, 0, 4);
                case "address":
                    return HideSensitiveInfo(value, 4, 4);
                case "carNo":
                    return HideSensitiveInfo(value, 1, 2);
                case "email":
                    return HideEmailDetails(value);
                default:
                    break;
            }
            return value;
        }

        /// <summary>
        /// 隐藏敏感信息
        /// </summary>
        /// <param name="info">信息实体</param>
        /// <param name="left">左边保留的字符数</param>
        /// <param name="right">右边保留的字符数</param>
        /// <param name="basedOnLeft">当长度异常时，是否显示左边 </param>
        /// <returns></returns>
        public static string HideSensitiveInfo(this string info, int left, int right, bool basedOnLeft = true)
        {
            if (String.IsNullOrEmpty(info))
            {
                return "";
            }
            StringBuilder sbText = new StringBuilder();
            int hiddenCharCount = info.Length - left - right;
            if (hiddenCharCount > 0)
            {
                string prefix = info.Substring(0, left), suffix = info.Substring(info.Length - right);
                sbText.Append(prefix);
                for (int i = 0; i < hiddenCharCount; i++)
                {
                    sbText.Append("*");
                }
                sbText.Append(suffix);
            }
            else
            {
                if (basedOnLeft)
                {
                    if (info.Length > left && left > 0)
                    {
                        sbText.Append(info.Substring(0, left) + "****");
                    }
                    else
                    {
                        sbText.Append(info.Substring(0, 1) + "****");
                    }
                }
                else
                {
                    if (info.Length > right && right > 0)
                    {
                        sbText.Append("****" + info.Substring(info.Length - right));
                    }
                    else
                    {
                        sbText.Append("****" + info.Substring(info.Length - 1));
                    }
                }
            }
            return sbText.ToString();
        }

        /// <summary>
        /// 隐藏敏感信息
        /// </summary>
        /// <param name="info">信息</param>
        /// <param name="sublen">信息总长与左子串（或右子串）的比例</param>
        /// <param name="basedOnLeft">当长度异常时，是否显示左边，默认true，默认显示左边</param>
        /// <code>true</code>显示左边，<code>false</code>显示右边
        /// <returns></returns>
        public static string HideSensitiveInfo(this string info, int sublen = 3, bool basedOnLeft = true)
        {
            if (String.IsNullOrEmpty(info))
            {
                return "";
            }
            if (sublen <= 1)
            {
                sublen = 3;
            }
            int subLength = info.Length / sublen;
            if (subLength > 0 && info.Length > (subLength * 2))
            {
                string prefix = info.Substring(0, subLength), suffix = info.Substring(info.Length - subLength);
                return prefix + "****" + suffix;
            }
            else
            {
                if (basedOnLeft)
                {
                    string prefix = subLength > 0 ? info.Substring(0, subLength) : info.Substring(0, 1);
                    return prefix + "****";
                }
                else
                {
                    string suffix = subLength > 0 ? info.Substring(info.Length - subLength) : info.Substring(info.Length - 1);
                    return "****" + suffix;
                }
            }
        }

        /// <summary>
        /// 隐藏邮件详情
        /// </summary>
        /// <param name="email">邮件地址</param>
        /// <param name="left">邮件头保留字符个数，默认值设置为3</param>
        /// <returns></returns>
        private static string HideEmailDetails(this string email, int left = 3)
        {
            if (String.IsNullOrEmpty(email))
            {
                return "";
            }
            if (Regex.IsMatch(email, @"\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"))//如果是邮件地址
            {
                int suffixLen = email.Length - email.LastIndexOf('@');
                return HideSensitiveInfo(email, left, suffixLen, false);
            }
            else
            {
                return HideSensitiveInfo(email);
            }
        }
        #endregion

        #region 匹配方法
        /// <summary>  
        /// 验证字符串是否匹配正则表达式描述的规则  
        /// </summary>  
        /// <param name="inputStr">待验证的字符串</param>  
        /// <param name="patternStr">正则表达式字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsMatch(this string inputStr, string patternStr)
        {
            return IsMatch(inputStr, patternStr, false, false);
        }

        /// <summary>  
        /// 验证字符串是否匹配正则表达式描述的规则  
        /// </summary>  
        /// <param name="inputStr">待验证的字符串</param>  
        /// <param name="patternStr">正则表达式字符串</param>  
        /// <param name="ifIgnoreCase">匹配时是否不区分大小写</param>  
        /// <returns>是否匹配</returns>  

        public static bool IsMatch(this string inputStr, string patternStr, bool ifIgnoreCase, bool ifValidateWhiteSpace)
        {
            if (!ifValidateWhiteSpace && string.IsNullOrWhiteSpace(inputStr))
                return false;//如果不要求验证空白字符串而此时传入的待验证字符串为空白字符串，则不匹配  
            Regex regex = null;
            if (ifIgnoreCase)
                regex = new Regex(patternStr, RegexOptions.IgnoreCase);//指定不区分大小写的匹配  
            else
                regex = new Regex(patternStr);
            return regex.IsMatch(inputStr);
        }

        ///<summary>
        ///正则替换字符串
        ///</summary>
        ///<param name="inputStr">待替换的字符串</param>
        ///<param name="patternStr">正则表达式字符串</param>
        ///<param name="replaceStr">替换字符串</param>
        ///<returns>返回替换后的字符串</returns>

        public static string RegexRepalce(this string inputStr, string patternStr, string replaceStr)
        {
            Regex regex = null;
            regex = new Regex(patternStr);
            return regex.Replace(inputStr, replaceStr);

        }

        ///<summary>
        ///正则替换字符串
        ///</summary>
        ///<param name="inputStr">待替换的字符串</param>
        ///<param name="patternStr">正则表达式字符串</param>
        ///<param name="replaceStr">替换字符串</param>
        /// <param name="ifIgnoreCase">匹配时是否不区分大小写</param> 
        ///<returns>返回替换后的字符串</returns>
        public static string RegexReplace(this string inputStr, string patternStr, string replaceStr, bool ifIgnoreCase, bool ifValidateWhiteSpace)
        {
            if (!ifValidateWhiteSpace && string.IsNullOrWhiteSpace(inputStr))
                return inputStr;//如果不要求验证空白字符串而此时传入的待验证字符串为空白字符串，则不匹配  
            Regex regex = null;
            if (ifIgnoreCase)
                regex = new Regex(patternStr, RegexOptions.IgnoreCase);//指定不区分大小写的匹配  
            else
                regex = new Regex(patternStr);
            return regex.Replace(inputStr, replaceStr);
        }
        #endregion

        #region 验证方法
        /// <summary>  
        /// 验证数字(double类型)  
        /// [可以包含负号和小数点]  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsNumber(this string input)
        {
            string pattern = @"^-?\d+$|^(-?\d+)(\.\d+)?$";
            return IsMatch(input, pattern);
        }

        /// <summary>  
        /// 验证整数  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsInteger(this string input)
        {
            string pattern = @"^-?\d+$";
            return IsMatch(input, pattern);
        }

        /// <summary>  
        /// 验证正整数  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsIntegerPositive(this string input)
        {
            string pattern = @"^[0-9]*[1-9][0-9]*$";
            return IsMatch(input, pattern);
        }

        /// <summary>  
        /// 验证自然数
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsNaturalNumber(this string input)
        {
            string pattern = @"^(0|([1-9]\d{0,2}))$";
            return IsMatch(input, pattern);
        }
        /// <summary>  
        /// 验证自然数
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsFloat(this string input)
        {
            string pattern = @"^(-?\d+)(\.\d+)?$";
            return IsMatch(input, pattern);
        }


        /// <summary>  
        /// 验证小数  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsDecimal(this string input)
        {
            string pattern = @"^([-+]?[1-9]\d*\.\d+|-?0\.\d*[1-9]\d*)$";
            return IsMatch(input, pattern);
        }

        /// <summary>  
        /// 验证只包含英文字母  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsEnglishCharacter(this string input)
        {
            string pattern = @"^[A-Za-z]+$";
            return IsMatch(input, pattern);
        }

        /// <summary>  
        /// 替换英文字母  
        /// </summary>  
        /// <param name="input">待替换的字符串</param>  
        /// <returns>替换后的字符串</returns> 
        public static string ReplaceEnglishCharacter(this string input)
        {
            string pattern = "[A-Za-z]";
            string replaceStr = "*";
            return RegexRepalce(input, pattern, replaceStr);
        }

        /// <summary>  
        /// 验证只包含数字和英文字母  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsIntegerAndEnglishCharacter(this string input)
        {
            string pattern = @"^[0-9A-Za-z]+$";
            return IsMatch(input, pattern);
        }

        /// <summary>  
        /// 验证只包含汉字  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsChineseCharacter(this string input)
        {
            string pattern = @"^[\u4e00-\u9fa5]+$";
            return IsMatch(input, pattern);
        }

        /// <summary>  
        /// 验证字符串包含内容  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <param name="withEnglishCharacter">是否包含英文字母</param>  
        /// <param name="withNumber">是否包含数字</param>  
        /// <param name="withChineseCharacter">是否包含汉字</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsStringInclude(string input, bool withEnglishCharacter, bool withNumber, bool withChineseCharacter)
        {
            if (!withEnglishCharacter && !withNumber && !withChineseCharacter)
                return false;//如果英文字母、数字和汉字都没有，则返回false  
            StringBuilder patternString = new StringBuilder();
            patternString.Append("^[");
            if (withEnglishCharacter)
                patternString.Append("a-zA-Z");
            if (withNumber)
                patternString.Append("0-9");
            if (withChineseCharacter)
                patternString.Append(@"\u4E00-\u9FA5");
            patternString.Append("]+$");
            return IsMatch(input, patternString.ToString());
        }


        /// <summary>  
        /// 验证yyyy-mm-dd日期  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsDateTime(this string input)
        {
            string pattern = @"^((((1[6-9]|[2-9]\d)\d{2})-(0?[13578]|1[02])-(0?[1-9]|[12]\d|3[01]))|(((1[6-9]|[2-9]\d)\d{2})-(0?[13456789]|1[012])-(0?[1-9]|[12]\d|30))|(((1[6-9]|[2-9]\d)\d{2})-0?2-(0?[1-9]|1\d|2[0-8]))|(((1[6-9]|[2-9]\d)(0[48]|[2468][048]|[13579][26])|((16|[2468][048]|[3579][26])00))-0?2-29-))$";
            return IsMatch(input, pattern);
        }

        /// <summary>  
        /// 验证固定电话号码  
        /// [3位或4位区号；区号可以用小括号括起来；区号可以省略；区号与本地号间可以用减号或空格隔开；可以有3位数的分机号，分机号前要加减号]  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsTelePhoneNumber(this string input)
        {
            string pattern = @"^1[3|4|5|7|8]\d{9}$";
            return IsMatch(input, pattern);
        }

        /// <summary>  
        /// 验证手机号码  
        /// [可匹配"(+86)013325656352"，括号可以省略，+号可以省略，(+86)可以省略，11位手机号前的0可以省略；11位手机号第二位数可以是3、4、5、8中的任意一个]  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsMobilePhoneNumber(this string input)
        {
            string pattern = @"^((\+)?86|((\+)?86)?)0?1[3458]\d{9}$";
            return IsMatch(input, pattern);
        }

        /// <summary>  
        /// 验证电话号码（可以是固定电话号码或手机号码）  
        /// [固定电话：[3位或4位区号；区号可以用小括号括起来；区号可以省略；区号与本地号间可以用减号或空格隔开；可以有3位数的分机号，分机号前要加减号]]  
        /// [手机号码：[可匹配"(+86)013325656352"，括号可以省略，+号可以省略，(+86)可以省略，手机号前的0可以省略；手机号第二位数可以是3、4、5、8中的任意一个]]  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsPhoneNumber(this string input)
        {
            if (IsTelePhoneNumber(input) || IsMobilePhoneNumber(input))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>  
        /// 验证邮政编码  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsZipCode(this string input)
        {
            string pattern = @"^\d{6}$";
            return IsMatch(input, pattern);
        }

        /// <summary>  
        /// 验证电子邮箱  
        /// [@字符前可以包含字母、数字、下划线和点号；@字符后可以包含字母、数字、下划线和点号；@字符后至少包含一个点号且点号不能是最后一个字符；最后一个点号后只能是字母或数字]  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsEmail(this string input)
        {
            ////邮箱名以数字或字母开头；邮箱名可由字母、数字、点号、减号、下划线组成；邮箱名（@前的字符）长度为3～18个字符；邮箱名不能以点号、减号或下划线结尾；不能出现连续两个或两个以上的点号、减号。  
            //string pattern = @"^[a-zA-Z0-9]((?<!(\.\.|--))[a-zA-Z0-9\._-]){1,16}[a-zA-Z0-9]@([0-9a-zA-Z][0-9a-zA-Z-]{0,62}\.)+([0-9a-zA-Z][0-9a-zA-Z-]{0,62})\.?|((25[0-5]|2[0-4]\d|[01]?\d\d?)\.){3}(25[0-5]|2[0-4]\d|[01]?\d\d?)$";  
            string pattern = @"^([\w-\.]+)@([\w-\.]+)(\.[a-zA-Z0-9]+)$";
            return IsMatch(input, pattern);
        }

        /// <summary>  
        /// 验证网址（可以匹配IPv4地址但没对IPv4地址进行格式验证；IPv6暂时没做匹配）  
        /// [允许省略"://"；可以添加端口号；允许层级；允许传参；域名中至少一个点号且此点号前要有内容]  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsURL(this string input)
        {
            ////每级域名由字母、数字和减号构成（第一个字母不能是减号），不区分大小写，单个域长度不超过63，完整的域名全长不超过256个字符。在DNS系统中，全名是以一个点“.”来结束的，例如“www.nit.edu.cn.”。没有最后的那个点则表示一个相对地址。   
            ////没有例如"http://"的前缀，没有传参的匹配  
            //string pattern = @"^([0-9a-zA-Z][0-9a-zA-Z-]{0,62}\.)+([0-9a-zA-Z][0-9a-zA-Z-]{0,62})\.?$";  

            //string pattern = @"^(((file|gopher|news|nntp|telnet|http|ftp|https|ftps|sftp)://)|(www\.))+(([a-zA-Z0-9\._-]+\.[a-zA-Z]{2,6})|([0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}))(/[a-zA-Z0-9\&%_\./-~-]*)?$";  
            string pattern = @"^([a-zA-Z]+://)?([\w-\.]+)(\.[a-zA-Z0-9]+)(:\d{0,5})?/?([\w-/]*)\.?([a-zA-Z]*)\??(([\w-]*=[\w%]*&?)*)$";
            return IsMatch(input, pattern);
        }

        /// <summary>  
        /// 验证IPv4地址  
        /// [第一位和最后一位数字不能是0或255；允许用0补位]  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsIPv4(this string input)
        {
            //string pattern = @"^(25[0-4]|2[0-4]\d]|[01]?\d{2}|[1-9])\.(25[0-5]|2[0-4]\d]|[01]?\d?\d)\.(25[0-5]|2[0-4]\d]|[01]?\d?\d)\.(25[0-4]|2[0-4]\d]|[01]?\d{2}|[1-9])$";  
            //return IsMatch(input, pattern);  
            string[] IPs = input.Split('.');
            if (IPs.Length != 4)
                return false;
            int n = -1;
            for (int i = 0; i < IPs.Length; i++)
            {
                if (i == 0 || i == 3)
                {
                    if (int.TryParse(IPs[i], out n) && n > 0 && n < 255)
                        continue;
                    else
                        return false;
                }
                else
                {
                    if (int.TryParse(IPs[i], out n) && n >= 0 && n <= 255)
                        continue;
                    else
                        return false;
                }
            }
            return true;
        }

        /// <summary>  
        /// 验证IPv6地址  
        /// [可用于匹配任何一个合法的IPv6地址]  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsIPv6(this string input)
        {
            string pattern = @"^\s*((([0-9A-Fa-f]{1,4}:){7}([0-9A-Fa-f]{1,4}|:))|(([0-9A-Fa-f]{1,4}:){6}(:[0-9A-Fa-f]{1,4}|((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3})|:))|(([0-9A-Fa-f]{1,4}:){5}(((:[0-9A-Fa-f]{1,4}){1,2})|:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3})|:))|(([0-9A-Fa-f]{1,4}:){4}(((:[0-9A-Fa-f]{1,4}){1,3})|((:[0-9A-Fa-f]{1,4})?:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){3}(((:[0-9A-Fa-f]{1,4}){1,4})|((:[0-9A-Fa-f]{1,4}){0,2}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){2}(((:[0-9A-Fa-f]{1,4}){1,5})|((:[0-9A-Fa-f]{1,4}){0,3}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){1}(((:[0-9A-Fa-f]{1,4}){1,6})|((:[0-9A-Fa-f]{1,4}){0,4}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(:(((:[0-9A-Fa-f]{1,4}){1,7})|((:[0-9A-Fa-f]{1,4}){0,5}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:)))(%.+)?\s*$";
            return IsMatch(input, pattern);
        }

        /// <summary>  
        /// 身份证上数字对应的地址  
        /// </summary>  
        //enum IDAddress  
        //{  
        //    北京 = 11, 天津 = 12, 河北 = 13, 山西 = 14, 内蒙古 = 15, 辽宁 = 21, 吉林 = 22, 黑龙江 = 23, 上海 = 31, 江苏 = 32, 浙江 = 33,  
        //    安徽 = 34, 福建 = 35, 江西 = 36, 山东 = 37, 河南 = 41, 湖北 = 42, 湖南 = 43, 广东 = 44, 广西 = 45, 海南 = 46, 重庆 = 50, 四川 = 51,  
        //    贵州 = 52, 云南 = 53, 西藏 = 54, 陕西 = 61, 甘肃 = 62, 青海 = 63, 宁夏 = 64, 新疆 = 65, 台湾 = 71, 香港 = 81, 澳门 = 82, 国外 = 91  
        //}  

        /// <summary>  
        /// 验证一代身份证号（15位数）  
        /// [长度为15位的数字；匹配对应省份地址；生日能正确匹配]  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsIDCard15(this string input)
        {
            //验证是否可以转换为15位整数  
            long l = 0;
            if (!long.TryParse(input, out l) || l.ToString().Length != 15)
            {
                return false;
            }
            //验证省份是否匹配  
            //1~6位为地区代码，其中1、2位数为各省级政府的代码，3、4位数为地、市级政府的代码，5、6位数为县、区级政府代码。  
            string address = "11,12,13,14,15,21,22,23,31,32,33,34,35,36,37,41,42,43,44,45,46,50,51,52,53,54,61,62,63,64,65,71,81,82,91,";
            if (!address.Contains(input.Remove(2) + ","))
            {
                return false;
            }
            //验证生日是否匹配  
            string birthdate = input.Substring(6, 6).Insert(4, "/").Insert(2, "/");
            DateTime dt;
            if (!DateTime.TryParse(birthdate, out dt))
            {
                return false;
            }
            return true;
        }

        /// <summary>  
        /// 验证二代身份证号（18位数，GB11643-1999标准）  
        /// [长度为18位；前17位为数字，最后一位(校验码)可以为大小写x；匹配对应省份地址；生日能正确匹配；校验码能正确匹配]  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsIDCard18(this string input)
        {
            //验证是否可以转换为正确的整数  
            long l = 0;
            if (!long.TryParse(input.Remove(17), out l) || l.ToString().Length != 17 || !long.TryParse(input.Replace('x', '0').Replace('X', '0'), out l))
            {
                return false;
            }
            //验证省份是否匹配  
            //1~6位为地区代码，其中1、2位数为各省级政府的代码，3、4位数为地、市级政府的代码，5、6位数为县、区级政府代码。  
            string address = "11,12,13,14,15,21,22,23,31,32,33,34,35,36,37,41,42,43,44,45,46,50,51,52,53,54,61,62,63,64,65,71,81,82,91,";
            if (!address.Contains(input.Remove(2) + ","))
            {
                return false;
            }
            //验证生日是否匹配  
            string birthdate = input.Substring(6, 8).Insert(6, "/").Insert(4, "/");
            DateTime dt;
            if (!DateTime.TryParse(birthdate, out dt))
            {
                return false;
            }
            //校验码验证  
            //校验码：  
            //（1）十七位数字本体码加权求和公式   
            //S = Sum(Ai * Wi), i = 0, ... , 16 ，先对前17位数字的权求和   
            //Ai:表示第i位置上的身份证号码数字值   
            //Wi:表示第i位置上的加权因子   
            //Wi: 7 9 10 5 8 4 2 1 6 3 7 9 10 5 8 4 2   
            //（2）计算模   
            //Y = mod(S, 11)   
            //（3）通过模得到对应的校验码   
            //Y: 0 1 2 3 4 5 6 7 8 9 10   
            //校验码: 1 0 X 9 8 7 6 5 4 3 2   
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] Wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] Ai = input.Remove(17).ToCharArray();
            int sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(Wi[i]) * int.Parse(Ai[i].ToString());
            }
            int y = -1;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != input.Substring(17, 1).ToLower())
            {
                return false;
            }
            return true;
        }

        /// <summary>  
        /// 验证身份证号（不区分一二代身份证号）  
        /// </summary>  
        /// <param name="input">待验证的字符串</param>  
        /// <returns>是否匹配</returns>  
        public static bool IsIDCard(this string input)
        {
            if (input.Length == 18)
                return IsIDCard18(input);
            else if (input.Length == 15)
                return IsIDCard15(input);
            else
                return false;
        }
        #endregion
    }
}