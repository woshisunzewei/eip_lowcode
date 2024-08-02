/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2018/11/29 21:12:52
* 文件名: SearchFilterGroupUtil
* 描述: 
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Common.Core.Extension;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace EIP.Common.Models.Paging
{
    /// <summary>
    /// 高级查询json反序列化对象
    /// </summary>
    public class Filter
    {
        /// <summary>
        /// 查询条件AND、OR
        /// </summary>
        public string groupOp { get; set; }

        /// <summary>
        /// 查询字段
        /// </summary>
        public List<FilterRules> rules { get; set; } = new List<FilterRules>();

        /// <summary>
        /// 嵌套查询
        /// </summary>
        public List<Filter> groups { get; set; } = new List<Filter>();
    }

    /// <summary>
    /// 高级查询字段明细
    /// </summary>
    public class FilterRules
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        public string field { get; set; }

        /// <summary>
        /// 查询条件
        /// </summary>
        public string op { get; set; }

        /// <summary>
        /// 关键字
        /// </summary>
        public string data { get; set; }
    }

    /// <summary>
    /// 高级查询，通过grid传入的filter参数进行解析
    /// </summary>
    public static class SearchFilterUtil
    {
        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static string ConvertFiltersWhere(string filter)
        {
            if (filter.IsNullOrEmpty())
                return "";
            string sql = ConvertFilters(filter.JsonStringToObject<Filter>());
            string and = sql.IsNotNullOrEmpty() ? sql : "1=1";
            return $" AND {and} ";
        }
        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static Filter ConvertFilters(string filter)
        {
            if (filter.IsNullOrEmpty())
                return new Filter();
            return filter.JsonStringToObject<Filter>();
        }


        /// <summary>
        /// 处理SQL查询条件，含子查询
        /// </summary>
        /// <param name="filter">查询对象</param>
        /// <returns></returns>
        public static string ConvertFilters(Filter filter)
        {
            if (filter == null)
                return "";

            //标识是否为首次加载有效字段条件，作为添加AND或OR关键字
            bool isFirst = true;

            StringBuilder where = new StringBuilder("");

            //处理字段查询明细
            if (filter.rules != null && filter.rules.Count > 0)
            {
                foreach (var item in filter.rules)
                {
                    if (!string.IsNullOrEmpty(item.field) && !string.IsNullOrEmpty(item.op))
                    {
                        if (isFirst != true)
                        {
                            //非首个条件添加AND或者OR
                            where.AppendFormat(" {0} ", filter.groupOp);
                        }
                        where.Append(ConvertFilters(item));
                        isFirst = false;
                    }
                }
            }

            //处理嵌套查询
            if (filter.groups != null && filter.groups.Count > 0)
            {
                foreach (var item in filter.groups)
                {
                    string child = ConvertFilters(item);
                    if (!string.IsNullOrEmpty(child))
                    {
                        if (isFirst != true)
                        {
                            //非首个条件添加AND或者OR
                            where.AppendFormat(" {0} ", filter.groupOp);
                        }
                        where.AppendFormat(child);
                        isFirst = false;
                    }
                }
            }

            //返回
            return where.Length > 0 ? $" ({where}) " : "";
        }

        /// <summary>
        /// 处理单个字段查询，匹配数据类型及查询方式
        /// </summary>
        /// <param name="rule">查询字段对象</param>
        /// <returns></returns>
        private static string ConvertFilters(FilterRules rule)
        {
            string phoneKey = "Ph376A@e^270Ks_~";
            if (string.IsNullOrEmpty(rule.op) || string.IsNullOrEmpty(rule.field))
            {
                return "";
            }
            //Sql注入替换后数据
            string data = rule.data.FilterSql();
            StringBuilder searchCase = new StringBuilder();
            switch (rule.op)
            {
                case "eq": //等于
                    searchCase.Append(GetFilter(rule.field, " ='" + data + "'"));
                    break;
                case "eqphone": //等于
                    searchCase.Append(GetFilter(rule.field, " ='" + Encrypt(data, phoneKey) + "'"));
                    break;
                case "ne": //不等于
                    searchCase.Append(GetFilter(rule.field, " !='" + data + "'"));
                    break;
                case "bw": //以...开始
                    searchCase.Append(GetFilter(rule.field, " like '" + data + "%'"));
                    break;
                case "bn": //不以...开始
                    searchCase.Append(GetFilter(rule.field, " not like '" + data + "%'"));
                    break;
                case "ew": //结束于
                    searchCase.Append(GetFilter(rule.field, " like '%" + data + "'"));
                    break;
                case "en": //不结束于
                    searchCase.Append(GetFilter(rule.field, " not like '%" + data + "'"));
                    break;
                case "lt": //小于
                    searchCase.Append(GetFilter(rule.field, " <'" + data + "'"));
                    break;
                case "le": //小于等于
                    searchCase.Append(GetFilter(rule.field, " <='" + data + "'"));
                    break;
                case "gt": //大于
                    searchCase.Append(GetFilter(rule.field, " >'" + data + "'"));
                    break;
                case "ge": //大于等于
                    searchCase.Append(GetFilter(rule.field, " >='" + data + "'"));
                    break;
                case "in": //包括
                    searchCase.Append(GetFilter(rule.field, " in ('" + data + "')"));
                    break;
                case "ni": //不包含
                    searchCase.Append(GetFilter(rule.field, " not in ('" + data + "')"));
                    break;
                case "cn"://包含
                    searchCase.Append(GetFilter(rule.field, " like '%" + data + "%' "));
                    break;
                case "nc"://不包含
                    searchCase.Append(GetFilter(rule.field, " not like '%" + data + "%'"));
                    break;
                case "nu"://空值
                    searchCase.Append(GetFilter(rule.field, " is null"));
                    break;
                case "nn"://非空值
                    searchCase.Append(GetFilter(rule.field, " is not null"));
                    break;
                case "dateeq"://时间等于
                    if (data.Length == 4)//年
                    {
                        searchCase.Append(GetFilter(rule.field, " between '" + data + "-01-01 00:00:00' AND '" + data + "-12-31 23:59:59'"));
                    }
                    else if (data.Length == 7) //年月
                    {
                        string endTime = Convert.ToDateTime(data).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
                        searchCase.Append(GetFilter(rule.field, " between '" + data + "-01 00:00:00' AND '" + endTime + " 23:59:59'"));
                    }
                    else if (data.Length == 10) //年月日
                    {
                        searchCase.Append(GetFilter(rule.field, " between '" + data + " 00:00:00' AND '" + data + " 23:59:59'"));
                    }
                    else if (data.Length == 13) //年月日时
                    {
                        searchCase.Append(GetFilter(rule.field, " between '" + data + " :00:00' AND '" + data + " :59:59'"));
                    }
                    else if (data.Length == 16) //年月日时分
                    {
                        searchCase.Append(GetFilter(rule.field, " between '" + data + " :00' AND '" + data + " :59'"));
                    }
                    else
                    {
                        searchCase.Append(GetFilter(rule.field, " ='" + data + "'"));
                    }
                    break;
                case "datelt"://小于
                    if (data.Length == 4)//年
                    {
                        searchCase.Append(GetFilter(rule.field, " <'" + data + "-01-01 00:00:00'"));
                    }
                    else if (data.Length == 7) //年月
                    {
                        searchCase.Append(GetFilter(rule.field, " <'" + data + "-01 00:00:00' '"));
                    }
                    else if (data.Length == 10) //年月日
                    {
                        searchCase.Append(GetFilter(rule.field, " <'" + data + " 00:00:00'"));
                    }
                    else if (data.Length == 13) //年月日时
                    {
                        searchCase.Append(GetFilter(rule.field, " <'" + data + " :00:00'"));
                    }
                    else if (data.Length == 16) //年月日时分
                    {
                        searchCase.Append(GetFilter(rule.field, " <'" + data + " :00'"));
                    }
                    else
                    {
                        searchCase.Append(GetFilter(rule.field, " <'" + data + "'"));
                    }
                    break;
                case "datele"://小于等于
                    if (data.Length == 4)//年
                    {
                        searchCase.Append(GetFilter(rule.field, " <='" + data + "-01-01 00:00:00'"));
                    }
                    else if (data.Length == 7) //年月
                    {
                        searchCase.Append(GetFilter(rule.field, " <='" + data + "-01 00:00:00' '"));
                    }
                    else if (data.Length == 10) //年月日
                    {
                        searchCase.Append(GetFilter(rule.field, " <='" + data + " 00:00:00'"));
                    }
                    else if (data.Length == 13) //年月日时
                    {
                        searchCase.Append(GetFilter(rule.field, " <='" + data + " :00:00'"));
                    }
                    else if (data.Length == 16) //年月日时分
                    {
                        searchCase.Append(GetFilter(rule.field, " <='" + data + " :00'"));
                    }
                    else
                    {
                        searchCase.Append(GetFilter(rule.field, " <='" + data + "'"));
                    }
                    break;
                case "dategt"://大于
                    if (data.Length == 4)//年
                    {
                        searchCase.Append(GetFilter(rule.field, " >'" + data + "-01-01 00:00:00'"));
                    }
                    else if (data.Length == 7) //年月
                    {
                        searchCase.Append(GetFilter(rule.field, " >'" + data + "-01 00:00:00' '"));
                    }
                    else if (data.Length == 10) //年月日
                    {
                        searchCase.Append(GetFilter(rule.field, " >'" + data + " 00:00:00'"));
                    }
                    else if (data.Length == 13) //年月日时
                    {
                        searchCase.Append(GetFilter(rule.field, " >'" + data + " :00:00'"));
                    }
                    else if (data.Length == 16) //年月日时分
                    {
                        searchCase.Append(GetFilter(rule.field, " >'" + data + " :00'"));
                    }
                    else
                    {
                        searchCase.Append(GetFilter(rule.field, " >'" + data + "'"));
                    }
                    break;
                case "datege"://大于等于
                    if (data.Length == 4)//年
                    {
                        searchCase.Append(GetFilter(rule.field, " >='" + data + "-01-01 00:00:00'"));
                    }
                    else if (data.Length == 7) //年月
                    {
                        searchCase.Append(GetFilter(rule.field, " >='" + data + "-01 00:00:00' '"));
                    }
                    else if (data.Length == 10) //年月日
                    {
                        searchCase.Append(GetFilter(rule.field, " >='" + data + " 00:00:00'"));
                    }
                    else if (data.Length == 13) //年月日时
                    {
                        searchCase.Append(GetFilter(rule.field, " >='" + data + " :00:00'"));
                    }
                    else if (data.Length == 16) //年月日时分
                    {
                        searchCase.Append(GetFilter(rule.field, " >='" + data + " :00'"));
                    }
                    else
                    {
                        searchCase.Append(GetFilter(rule.field, " >='" + data + "'"));
                    }
                    break;
                case "daterange"://针对时间特别处理
                    var dateTime = data.Split('|');
                    if (dateTime.Length == 2)
                    {
                        string beginTime = dateTime[0];
                        string endTime = dateTime[1];
                        if (beginTime.Length == 4) //年
                        {
                            beginTime = beginTime + "-01-01 00:00:00";
                            endTime = endTime + "-01-01 00:00:00";
                        }
                        else if (beginTime.Length == 7) //年月
                        {
                            beginTime = beginTime + "-01 00:00:00";
                            endTime = endTime + "-01 00:00:00";
                        }
                        else if (beginTime.Length == 10) //年月日
                        {
                            beginTime = beginTime + "00:00:00";
                            endTime = endTime + "00:00:00";
                        }
                        searchCase.Append(GetFilter(rule.field, " between '" + beginTime + "' AND '" + endTime + "'"));
                    }
                    break;
            }
            return searchCase.ToString();
        }

        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="field"></param>
        /// <param name="formula"></param>
        /// <returns></returns>
        private static string GetFilter(string field, string formula)
        {
            string search = string.Empty;
            StringBuilder searchCase = new StringBuilder();
            var split = field.Split(',');
            if (split.Length > 1)
            {
                foreach (var fi in field.Split(','))
                {
                    search += fi + formula + " OR " /* " like '%" + data + "%' OR "*/;
                }
                searchCase.Append("(" + search.Substring(0, search.Length - 4) + ")");
            }
            else
            {
                searchCase.Append(field + formula);
            }
            return searchCase.ToString();
        }

        /// <summary>
        /// 替换SQL注入特殊字符
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static string StripSqlInjection(string sql)
        {
            if (!string.IsNullOrEmpty(sql))
            {
                //过滤 ' --
                const string pattern1 = @"(\%27)|(\')|(\-\-)";
                //防止执行 ' or
                const string pattern2 = @"((\%27)|(\'))\s*((\%6F)|o|(\%4F))((\%72)|r|(\%52))";
                //防止执行sql server 内部存储过程或扩展存储过程
                const string pattern3 = @"\s+exec(\s|\+)+(s|x)p\w+";
                sql = Regex.Replace(sql, pattern1, string.Empty, RegexOptions.IgnoreCase);
                sql = Regex.Replace(sql, pattern2, string.Empty, RegexOptions.IgnoreCase);
                sql = Regex.Replace(sql, pattern3, string.Empty, RegexOptions.IgnoreCase);
            }
            return sql;
        }

        //默认密钥向量 
        private static byte[] Keys = { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };
        /// <summary> 
        /// DES加密字符串 
        /// </summary> 
        /// <param name="encryptString">待加密的字符串</param> 
        /// <param name="encryptKey">加密密钥,要求为16位</param> 
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns> 
        public static string Encrypt(string encryptString, string encryptKey = "Eip963Ace#321Key")
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 16));
                byte[] rgbIv = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                var dcsp = Aes.Create();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dcsp.CreateEncryptor(rgbKey, rgbIv), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch (Exception ex)
            {
                return ex.Message + encryptString;
            }
        }
    }
}
