using EIP.Common.Core.Util;

namespace EIP.System.Models.Dtos.DataBase
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemDataBaseInput
    {
        /// <summary>
        /// 数据库类型
        /// </summary>
        public string ConnectionType { get; set; } = ConfigurationUtil.GetDbConnectionType();

        /// <summary>
        /// 链接字符串
        /// </summary>
        public string ConnectionString { get; set; } = ConfigurationUtil.GetDbConnectionString();
    }
}
