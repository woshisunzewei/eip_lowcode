namespace EIP.System.Models.Dtos.AgileDataLog
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemAgileDataLogValue
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public dynamic Data { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Description { get; set; }

    }
}
