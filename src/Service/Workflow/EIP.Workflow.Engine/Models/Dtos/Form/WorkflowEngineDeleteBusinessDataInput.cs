using System;
using EIP.Common.Models.Dtos;

namespace EIP.Workflow.Engine.Models.Dtos.Form
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowEngineDeleteBusinessDataInput : IdInput<string>
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid CodeGenerationId { get; set; }
    }
}