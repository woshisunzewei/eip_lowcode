﻿
namespace EIP.Common.OAuth.Utils
{
    public class AppSettingUtils
    {
        /// <summary>
        /// 根据Key取Value值
        /// </summary>
        /// <param name="key"></param>
        public static string GetStrValue(string key)
        {
            var value = "";
#if NET45
            value = System.Configuration.ConfigurationManager.AppSettings[key];
#else
            value = EIP.Common.OAuth.Utils.ConfigurationManager.AppSettings[key];
#endif
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value.ToString().Trim();
            }
            return value;
        }
    }
}