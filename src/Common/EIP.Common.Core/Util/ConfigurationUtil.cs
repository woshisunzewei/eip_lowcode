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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.IO;

namespace EIP.Common.Core.Util
{
    public class ConfigurationUtil
    {
        public static IConfiguration Configuration;

        public static T GetSection<T>(string key) where T : class, new()
        {
            var obj = new ServiceCollection()
                .AddOptions()
                .Configure<T>(Configuration.GetSection(key))
                .BuildServiceProvider()
                .GetService<IOptions<T>>()
                .Value;
            return obj;
        }
      
        public static string GetSection(string key)
        {
            return Configuration.GetValue<string>(key);
        }

        /// <summary>
        /// 获取数据库类型
        /// </summary>
        /// <returns></returns>
        public static string GetDbConnectionType()
        {
            return GetSection("EIP:DbConnectionType").ToLower();
        }

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <returns></returns>
        public static string GetDbConnectionString()
        {
            return GetSection("EIP:DbConnectionString");
        }

        /// <summary>
        /// 获取Redis连接
        /// </summary>
        /// <returns></returns>
        public static string GetRedisConnectionString()
        {
            return GetSection("EIP:RedisConnectionString");
        }

        /// <summary>
        /// 获取密码值
        /// </summary>
        /// <returns></returns>
        public static string GetPasswordKey()
        {
            return GetSection("EIP:PasswordKey");
        }

        /// <summary>
        /// 获取零时路径
        /// </summary>
        /// <returns></returns>
        public static string GetTempPath()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TempFile");
            if (!string.IsNullOrWhiteSpace(path) && !Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            return path;
        }
    }
}