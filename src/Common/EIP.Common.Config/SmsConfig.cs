using System.Linq;

namespace EIP.Common.Config
{
    /// <summary>
    /// 全局参数
    /// </summary>
    public static class SmsConfig
    {
        private static readonly string Key = "SystemSmsTemplate";

        /// <summary>
        /// 通过传入代码获取相关短信配置
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public static SystemSmsTemplate GetByCode(string code)
        {
            return (RedisHelper.LRange<SystemSmsTemplate>(Key, 0, -1)).FirstOrDefault();
        }

        /// <summary>
        /// 删除全局变量并写入
        /// </summary>
        /// <param name="config">缓存键名称</param>
        /// <returns></returns>
        public static bool DeleteValue(SystemSmsTemplate config)
        {
            var key = Key + config.Code;
            var haveKey = RedisHelper.Exists(key);
            if (haveKey)
            {
                RedisHelper.Del(key);
            }
            RedisHelper.RPush(key, config);
            return false;
        }
    }

    /// <summary>
    /// 短信模板
    /// </summary>
    public class SystemSmsTemplate
    {
        /// <summary>
        /// 来源:aliyun,tencent
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// AppId
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// AppKey
        /// </summary>		
        public string AppKey { get; set; }

        /// <summary>
        /// 代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 短信签名:阿里云发送短信需要
        /// </summary>
        public string Sign { get; set; }

        /// <summary>
        /// 模板Id
        /// </summary>		
        public string TemplateId { get; set; }

        /// <summary>
        /// 模板
        /// </summary>		
        public string Template { get; set; }

        /// <summary>
        /// 短信类型，0 为普通短信，1 营销短信
        /// </summary>
        public short Type { get; set; }

        /// <summary>
        /// 国家码，如 86 为中国
        /// </summary>
        public string NationCode { get; set; }

    }
}
