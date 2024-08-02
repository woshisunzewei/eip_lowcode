namespace EIP.System.Models.Dtos.Agile
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemAgileFindFormColumnsJsonOutput
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 类型:子表subtable
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public SystemAgileFindFormColumnsJsonOptionsOutput Options { get; set; }
    }

    public class SystemAgileFindFormColumnsJsonOptionsOutput
    {
        public bool Hidden { get; set; } = false;
    }
}
