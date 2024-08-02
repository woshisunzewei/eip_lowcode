using System;

namespace EIP.Workflow.Engine.Models.Dtos.ProcessInstance
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowEngineFindTaskByProcessInstanceIdInput
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid ProcessInstanceId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid ReceiveUserId { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class WorkflowEngineFindTaskByProcessInstanceIdOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid ActivityId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid TaskId { get; set; }
    }
}
