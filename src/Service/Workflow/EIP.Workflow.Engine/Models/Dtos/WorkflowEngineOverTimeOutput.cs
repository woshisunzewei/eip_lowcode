using System;

namespace EIP.Workflow.Engine.Models.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowEngineOverTimeOutput
    {
        /// <summary>
        /// 是否通知
        /// </summary>
        public bool? OverTimeReminding { get; set; } = null;

        /// <summary>
        /// 超时分钟
        /// </summary>
        public double? OverTimeMinute { get; set; } = null;

        /// <summary>
        /// 
        /// </summary>
        public DateTime? OverTime { get; set; } = null;
    }
}