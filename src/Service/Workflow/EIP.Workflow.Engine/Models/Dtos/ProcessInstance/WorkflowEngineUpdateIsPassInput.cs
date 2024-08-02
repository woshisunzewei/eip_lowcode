using System;
using System.Collections.Generic;

namespace EIP.Workflow.Engine.Models.Dtos.ProcessInstance
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowEngineUpdateIsPassInput
    {
        /// <summary>
        /// 更新活动Id
        /// </summary>
        public List<Guid> Activities { get; set; } = new List<Guid>();

        /// <summary>
        /// 更新连线Id
        /// </summary>
        public List<Guid> Links { get; set; } = new List<Guid>();
    }
}