using System.Collections.Generic;
using System.Linq;

namespace EIP.Common.Config
{
    /// <summary>
    /// 全局参数
    /// </summary>
    public static class GlobalParams
    {
        private static readonly string Key = "SystemConfig";

        /// <summary>
        /// 根据配置名称,子系统代码获取对应系统配置信息
        /// </summary>
        /// <param name="name"></param>
        /// <param name="subSystemCode"></param>
        /// <returns></returns>
        public static string GetValueByName(string name)
        {
            var data = RedisHelper.LRange<GlobalConfig>(Key, 0, -1);
            var get = data.FirstOrDefault(f => f.Key == name);
            return get?.Value;
        }

        /// <summary>
        /// 获取所有配置
        /// </summary>
        /// <param name="subSystemCode"></param>
        /// <returns></returns>
        public static IList<GlobalConfig> GetValuesByName()
        {
            return RedisHelper.LRange<GlobalConfig>(Key, 0, -1); ;
        }

        /// <summary>
        /// 写入全局参数值
        /// </summary>
        /// <param name="config">配置值</param>
        /// <param name="subSystemCode">子系统代码</param>
        /// <returns></returns>
        public static void SetValue(IList<GlobalConfig> config)
        {
            RedisHelper.RPush(Key, config);
        }

        /// <summary>
        /// 删除全局变量并写入
        /// </summary>
        /// <param name="config">缓存键名称</param>
        /// <param name="subSystemCode">子系统代码</param>
        /// <returns></returns>
        public static bool DeleteValue(IList<GlobalConfig> config)
        {
            var haveKey = RedisHelper.Exists(Key);
            if (haveKey)
            {
                RedisHelper.Del(Key);
            }
            if (config.Any())
            {
                foreach (var c in config)
                {
                    RedisHelper.LPush(Key, c);
                }
            }
            return false;
        }
    }

    /// <summary>
    /// 参数值
    /// </summary>
    public class GlobalConfig
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        /// 值
        /// </summary>		
        public string Value { get; set; }

    }
}
