using System;

namespace EIP.Workflow.Engine.Models.Dtos.Monitor
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowEngineFindMonitorDetailOutput
    {
        /// <summary>
        /// 地址
        /// </summary>
        public string FormPcUrl { get; set; }

        /// <summary>
        /// 配置
        /// </summary>
        public string Json { get; set; }

        /// <summary>
        /// 活动Id
        /// </summary>
        public Guid ActivityId { get; set; }

        /// <summary>
        /// 表单Id
        /// </summary>
        public Guid FormId { get; set; }
    }
}