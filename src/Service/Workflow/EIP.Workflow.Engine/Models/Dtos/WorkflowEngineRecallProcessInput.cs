using System;

namespace EIP.Workflow.Engine.Models.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowEngineRecallProcessInput
    {
        /// <summary>
        /// 任务Id
        /// </summary>
        public Guid TaskId { get; set; }

        /// <summary>
        /// 处理人员Id
        /// </summary>		
        public Guid DoUserId { get; set; }

        /// <summary>
        /// 处理人员名字
        /// </summary>		
        public string DoUserName { get; set; }
    }
}