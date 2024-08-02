using System;

namespace EIP.Workflow.Engine.Models.Dtos
{
    /// <summary>
    /// 通知地址
    /// </summary>
    public class WorkflowEngineFindSendNoticeUrlOutput
    {
        /// <summary>
        /// 通知人Id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 通知地址
        /// </summary>
        public string Url { get; set; }
    }
}
