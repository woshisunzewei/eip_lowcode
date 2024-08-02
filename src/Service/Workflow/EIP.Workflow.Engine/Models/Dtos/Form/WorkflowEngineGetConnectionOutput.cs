using EIP.Common.Core.Util;

namespace EIP.Workflow.Engine.Models.Dtos.Form
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowEngineGetConnectionOutput
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int OrderNo { get; set; } = 0;

        /// <summary>
        /// 数据库名
        /// </summary>
        public string CatalogName { get; set; } = "EIP";

        /// <summary>
        /// 数据库类型
        /// </summary>
        public string Type { get; set; } = "mssql";

        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 连接字符串
        /// </summary>
        public string Connection { get; set; }
           
    }
}