using System;

namespace EIP.System.Models.Dtos.Agile
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemAgileFindColumnJsonDataFromNameInput
    {
        /// <summary>
        /// 来源名称
        /// </summary>
        public string DataFromName { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SystemAgileFindColumnJsonDataFromNameOutput
    {
        /// <summary>
        /// 字段Json
        /// </summary>
        public string ColumnJson { get; set; }

        /// <summary>
        /// 菜单Id
        /// </summary>
        public Guid? MenuId { get; set; }
    }
}
