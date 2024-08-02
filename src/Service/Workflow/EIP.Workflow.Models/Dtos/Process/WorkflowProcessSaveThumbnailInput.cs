using System;

namespace EIP.Workflow.Models.Dtos.Process
{
    /// <summary>
    /// 保存缩略图
    /// </summary>
    public class WorkflowProcessSaveThumbnailInput
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid ProcessId { get; set; }

        /// <summary>
        /// 缩略图
        /// </summary>
        public string Thumbnail { get; set; }
    }
}
