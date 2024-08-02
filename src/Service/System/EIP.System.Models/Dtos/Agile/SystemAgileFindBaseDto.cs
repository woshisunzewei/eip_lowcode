using System;

namespace EIP.System.Models.Dtos.Agile
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemAgileFindBaseInput
    {
        /// <summary>
        /// 生成类型:1列表配置,2表单配置
        /// </summary>
        public int? ConfigType { get; set; }

        /// <summary>
        /// 配置Id
        /// </summary>
        public Guid? ConfigId { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SystemAgileFindBaseOutput
    {
        /// <summary>
        /// 配置Id
        /// </summary>
        public Guid ConfigId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }
}
