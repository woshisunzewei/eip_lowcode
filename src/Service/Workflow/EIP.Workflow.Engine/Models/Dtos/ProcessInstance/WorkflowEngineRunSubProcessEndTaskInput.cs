using EIP.Base.Models.Entities.Workflow;
using System;

namespace EIP.Workflow.Engine.Models.Dtos.ProcessInstance
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowEngineRunSubProcessEndTaskInput
    {
        /// <summary>
        /// 实例
        /// </summary>
        public WorkflowProcessInstance Instance { get; set; }

        /// <summary>
        /// 当前表单Id
        /// </summary>
        public Guid? FormId { get; set; }
        /// <summary>
        /// 流程实例Id
        /// </summary>		
        public Guid ProcessInstanceId { get; set; }
        /// <summary>
        /// 处理人
        /// </summary>		
        public Guid DoUserId { get; set; }

        /// <summary>
        /// 处理人名称
        /// </summary>		
        public string DoUserName { get; set; }
    }
}