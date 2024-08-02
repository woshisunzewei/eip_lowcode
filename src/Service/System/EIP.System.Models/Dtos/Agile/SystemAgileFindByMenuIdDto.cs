using System;

namespace EIP.System.Models.Dtos.Agile
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemAgileFindByMenuIdInput
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid MenuId { get; set; }

        /// <summary>
        /// 生成类型:1列表配置,2表单配置,3卡片配置
        /// </summary>
        public int? ConfigType { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SystemAgileFindByMenuIdOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid ConfigId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Theme { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string DataFromName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid DataBaseId { get; set; }
    }
}
