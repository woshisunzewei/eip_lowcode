using System;

namespace EIP.System.Models.Dtos.AgileAutomation
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemAgileAutomationFindEditOutput
    {
        /// <summary>
        /// 主键
        /// </summary>
        public Guid ConfigId { get; set; }

        /// <summary>
        /// 表单名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 字段Json，页面设计器具有
        /// </summary>
        public string ColumnJson { get; set; }

    }
}
