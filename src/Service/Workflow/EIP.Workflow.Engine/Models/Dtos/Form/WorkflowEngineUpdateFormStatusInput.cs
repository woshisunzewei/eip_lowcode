using EIP.Workflow.Models.Enums;
using System;

namespace EIP.Workflow.Engine.Models.Dtos.Form
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowEngineUpdateFormStatusInput
    {
        /// <summary>
        /// 实例Id
        /// </summary>
        public Guid ProcessInstanceId { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public EnumProcessInstanceStatus Status { get; set; }

        /// <summary>
        /// 流水号
        /// </summary>
        public string Sn { get; set; }
    }
}