namespace EIP.DataRoom.Models.Dtos
{
    public class DataSourceSaveInput
    {
        public int? id { get; set; }

        /// <summary>
        /// 数据源名称
        /// </summary>
        public string sourceName { get; set; }

        /// <summary>
        /// 数据源类型
        /// </summary>
        public string sourceType { get; set; }

        /// <summary>
        /// 连接驱动
        /// </summary>
        public string driverClassName { get; set; }

        /// <summary>
        /// 连接url
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 主机
        /// </summary>
        public string? host { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int? port { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string username { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        public string? table_name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string? remark { get; set; }

    }
}
