using System;

namespace EIP.Workflow.Engine.Models.Dtos.ProcessInstance
{
    /// <summary>
    /// 流程实例运行后返回值
    /// </summary>
    public class WorkflowEngineProcessRunOutput
    {
        /// <summary>
        /// 是否归档
        /// </summary>
        public bool IsArchive { get; set; } = false;

        /// <summary>
        /// 当前任务Id
        /// </summary>
        public Guid TaskId { get; set; }

        /// <summary>
        /// 流程图
        /// </summary>
        public string DesignJson { get; set; }
    }
}