using System;
using System.Collections.Generic;

namespace EIP.Workflow.Engine.Models.Dtos.ProcessInstance
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowEngineTaskProcessOutput
    {
        /// <summary>
        /// 更新活动Id
        /// </summary>
        public List<Guid> Activities { get; set; } = new List<Guid>();

        /// <summary>
        /// 更新连线Id
        /// </summary>
        public List<Guid> Links { get; set; } = new List<Guid>();

        /// <summary>
        /// 人员
        /// </summary>
        public List<WorkflowEngineProcessingPersonOutput> Person { get; set; } = new List<WorkflowEngineProcessingPersonOutput>();

        /// <summary>
        /// 是否必须选择人员:若为true则直接必须选择处理人员,若为false，则不用判断是否选择人员,不会弹出选择框
        /// </summary>
        public bool NeedChosenPerson { get; set; } = true;
    }
}