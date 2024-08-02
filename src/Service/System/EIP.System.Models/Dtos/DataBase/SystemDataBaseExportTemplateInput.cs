using System;

namespace EIP.System.Models.Dtos.DataBase
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemDataBaseExportTemplateInput
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid ButtonId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid RelationId { get; set; }

        /// <summary>
        /// 代码生成Id
        /// </summary>
        public Guid ConfigId { get; set; }

        /// <summary>
        /// 导出文件名称
        /// </summary>
        public string ReportName { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool Pdf { get; set; } = false;
    }
}
