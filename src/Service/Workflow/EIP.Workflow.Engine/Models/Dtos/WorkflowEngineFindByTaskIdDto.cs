using EIP.Base.Models.Entities.Workflow;
using System;
using System.Collections.Generic;
using System.Text;

namespace EIP.Workflow.Engine.Models.Dtos
{
    public class WorkflowEngineFindByTaskIdOutput: WorkflowProcessInstanceTask
    {
        /// <summary>
        /// 
        /// </summary>
        public string HeadImage { get; set; }
    }
}
