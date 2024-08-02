using EIP.Base.Models.Entities.Workflow;
using System.Collections.Generic;

namespace EIP.Workflow.Engine.Models.Dtos.ProcessInstance
{
    /// <summary>
    /// 启动子流程输入参数
    /// </summary>
    public class WorkflowEngineStatrSubProcessInput
    {
        /// <summary>
        /// 活动
        /// </summary>
        public WorkflowProcessInstanceActivity Activity { get; set; }

        /// <summary>
        /// 当前步骤处理人
        /// </summary>
        public IList<WorkflowEngineProcessingPersonDetailOutput> Persons { get; set; } = new List<WorkflowEngineProcessingPersonDetailOutput>();

        /// <summary>
        /// 当前实例
        /// </summary>
        public WorkflowEngineSaveInstanceTaskInput InstanceTask { get; set; }
    }
}