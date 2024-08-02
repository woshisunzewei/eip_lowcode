using System;
using System.Collections.Generic;
using System.Text;

namespace EIP.Workflow.Engine.Models.Dtos.ProcessInstance
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowEngineEndProcessInput
    {
        /// <summary>
        /// 任务Id
        /// </summary>
        public Guid TaskId { get; set; }

        /// <summary>
        /// 处理人
        /// </summary>		
        public Guid DoUserId { get; set; }

        /// <summary>
        /// 处理人名称
        /// </summary>		
        public string DoUserName { get; set; }

        /// <summary>
        /// 意见
        /// </summary>
        public string Comment { get; set; }
    }
}
