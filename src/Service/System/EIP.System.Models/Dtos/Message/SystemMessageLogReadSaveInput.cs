using System;

namespace EIP.System.Models.Dtos.Message
{
    /// <summary>
    /// 保存消息阅读
    /// </summary>
    public class SystemMessageLogReadSaveInput
    {
        /// <summary>
        /// 人员Id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 创建人Id
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 消息阅读Id
        /// </summary>
        public string MessageLogIds { get; set; }
    }
}