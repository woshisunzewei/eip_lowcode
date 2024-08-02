using EIP.Common.Models.Tree;
using System.Collections.Generic;

namespace EIP.Workflow.Engine.Models.Dtos.ProcessInstance
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowEngineFindAddctivityChosenUserOutput
    {

        /// <summary>
        /// 
        /// </summary>
        public IEnumerable<JsTreeEntity> Users { get; set; }
    }
}
