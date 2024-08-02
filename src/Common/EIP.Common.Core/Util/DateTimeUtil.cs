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

namespace EIP.Common.Core.Util
{
    public static class DateTimeUtil
    {
        /// <summary>
        /// 毫秒转天时分秒
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public static string FormatTime(long ms)
        {
            int ss = 1000;
            int mi = ss * 60;
            int hh = mi * 60;
            int dd = hh * 24;

            long day = ms / dd;
            long hour = (ms - day * dd) / hh;
            long minute = (ms - day * dd - hour * hh) / mi;
            long second = (ms - day * dd - hour * hh - minute * mi) / ss;
            long milliSecond = ms - day * dd - hour * hh - minute * mi - second * ss;

            string sDay = day < 10 ? "0" + day : "" + day; //天
            string sHour = hour < 10 ? "0" + hour : "" + hour;//小时
            string sMinute = minute < 10 ? "0" + minute : "" + minute;//分钟
            string sSecond = second < 10 ? "0" + second : "" + second;//秒
            string sMilliSecond = milliSecond < 10 ? "0" + milliSecond : "" + milliSecond;//毫秒
            sMilliSecond = milliSecond < 100 ? "0" + sMilliSecond : "" + sMilliSecond;

            return string.Format("{0} 天 {1} 小时 {2} 分 {3} 秒", sDay, sHour, sMinute, sSecond);
        }

        /// <summary>
        /// 取得某月的第一天
        /// </summary>
        /// <param name="datetime">要取得月份第一天的时间</param>
        /// <returns></returns>
        public static DateTime FirstDayOfMonth(DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day);
        }

        /// <summary>
        /// 取得某月的最后一天
        /// </summary>
        /// <param name="datetime">要取得月份最后一天的时间</param>
        /// <returns></returns>
        public static DateTime LastDayOfMonth(DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day).AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// 取得上个月第一天
        /// </summary>
        /// <param name="datetime">要取得上个月第一天的当前时间</param>
        /// <returns></returns>
        public static DateTime FirstDayOfPreviousMonth(DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day).AddMonths(-1);
        }

        /// <summary>
        /// 取得上个月的最后一天
        /// </summary>
        /// <param name="datetime">要取得上个月最后一天的当前时间</param>
        /// <returns></returns>
        public static DateTime LastDayOfPrdviousMonth(DateTime datetime)
        {
            return datetime.AddDays(1 - datetime.Day).AddDays(-1);
        }

        /// <summary>
        /// 将客户端时间转换为服务端本地时间
        /// </summary>
        /// <param name="clientTime">客户端时间</param>
        /// <returns>返回服务端本地时间</returns>
        public static DateTime ToServerLocalTime(this DateTime clientTime)
        {
            return TimeZoneInfo.ConvertTime(clientTime, TimeZoneInfo.Local);
        }
        /// <summary>
        /// 格式化为年月日时分秒24小时制
        /// </summary>
        /// <param name="time">需要格式化时间</param>
        /// <returns>格式化后时间</returns>
        public static string ToYyyyMMddHHmmss(this DateTime time)
        {
            return time.ToString("yyyy-MM-dd HH:mm:ss");
        }
        
        /// <summary>
        /// 将服务端本地时间转换为Utc时间
        /// </summary>
        /// <param name="serverTime">服务端时间</param>
        /// <returns>返回服务端本地时间</returns>
        public static DateTime ToUtcTime(this DateTime serverTime)
        {
            return TimeZoneInfo.ConvertTime(serverTime, TimeZoneInfo.Utc);
        }

        /// <summary>
        /// 时间添加分钟
        /// </summary>
        /// <param name="time">需要格式化时间</param>
        /// <param name="minute"></param>
        /// <returns>格式化后时间</returns>
        public static DateTime? ToAddMinuteDateTime(this DateTime time, double? minute)
        {
            if (minute == null)
            {
                return null;
            }
            return time.AddMinutes((double)minute);
        }

        /// <summary>
        /// 时间戳计时开始时间
        /// </summary>
        private static DateTime timeStampStartTime = new DateTime(1970, 1, 1, 0, 0, 0);

        /// <summary>
        /// 获得时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStamp()
        {
            return (long)(DateTime.Now.ToUniversalTime() - timeStampStartTime).TotalSeconds;
        }
       
        /// <summary>
        /// DateTime转换为13位时间戳（单位：毫秒）
        /// </summary>
        /// <param name="dateTime"> DateTime</param>
        /// <returns>13位时间戳（单位：毫秒）</returns>
        public static long GetTimeStampLong()
        {
            return (long)(DateTime.Now.ToUniversalTime() - timeStampStartTime).TotalMilliseconds;
        }
        /// <summary>
        /// 获得时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetTimeStamp(this DateTime dateTime)
        {
            return (long)(dateTime.ToUniversalTime() - timeStampStartTime).TotalSeconds;
        }

        /// <summary>
        /// DateTime转换为13位时间戳（单位：毫秒）
        /// </summary>
        /// <param name="dateTime"> DateTime</param>
        /// <returns>13位时间戳（单位：毫秒）</returns>
        public static long GetTimeStampLong(this DateTime dateTime)
        {
            return (long)(dateTime.ToUniversalTime() - timeStampStartTime).TotalMilliseconds;
        }

        /// <summary>
        /// 获得时间戳
        /// </summary>
        /// <returns></returns>
        public static int TotalSeconds(int day)
        {
            return Convert.ToInt32((DateTime.Now.AddDays(day)-DateTime.Now).TotalSeconds);
        }
        /// <summary>
        /// 获得时间戳
        /// </summary>
        /// <returns></returns>
        public static string GetTimeFormat()
        {
            return DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        /// <summary>
        /// 获取时间差
        /// </summary>
        /// <param name="dateTime">时间</param>
        /// <returns>时间差</returns>
        public static string GetTime(DateTime dateTime)
        {
            var time = DateTime.Now - dateTime;
            
            if (time.TotalHours > 24)
            {
                return Math.Floor(time.TotalDays) + "天前";
            }
            if (time.TotalHours > 1)
            {
                return Math.Floor(time.TotalHours) + "小时前";
            }
            if (time.TotalMinutes > 1)
            {
                return Math.Floor(time.TotalMinutes) + "分钟前";
            }
            return Math.Floor(time.TotalSeconds) + "秒前";
        }
    }
}