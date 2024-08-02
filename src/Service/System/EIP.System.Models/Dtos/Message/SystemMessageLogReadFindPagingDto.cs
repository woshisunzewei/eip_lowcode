using EIP.Common.Models.Paging;
using System;

namespace EIP.System.Models.Dtos.Message
{
    /// <summary>
    /// 消息阅读分页获取
    /// </summary>
    public class SystemMessageLogReadFindPagingInput : QueryParam
    {
        /// <summary>
        /// 当前人员Id
        /// </summary>
        public Guid? UserId { get; set; }
    }

    /// <summary>
    /// 消息阅读分页获取
    /// </summary>
    public class SystemMessageLogReadFindPagingOutput
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 消息Id
        /// </summary>
        public Guid MessageId { get; set; }

        /// <summary>
        /// 阅读时间
        /// </summary>
        public DateTime ReadTime { get; set; }

        /// <summary>
        /// 阅读人员
        /// </summary>
        public string ReadUserName { get; set; }

        /// <summary>
        /// 阅读Id
        /// </summary>
        public long MessageReadId { get; set; }
    }
}