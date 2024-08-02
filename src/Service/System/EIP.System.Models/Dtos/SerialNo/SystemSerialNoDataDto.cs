namespace EIP.System.Models.Dtos.SerialNo
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemSerialNoDataDto
    {
        /// <summary>
        /// 类型:datetime,number,column
        /// </summary>
        public string Type { get; set; }
        
        /// <summary>
        /// 位数
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// 编号超出位数后继续递增
        /// </summary>
        public bool Incremental { get; set; }

        /// <summary>
        /// 开始值
        /// </summary>
        public int BeginNumber { get; set; }

        /// <summary>
        /// 重复 day week month year
        /// </summary>
        public string Repeat { get; set; }

        /// <summary>
        /// 日期格式
        /// </summary>
        public string Format { get; set; }

        /// <summary>
        /// 字段
        /// </summary>
        public string Column { get; set; }
    }
}
