using EIP.Common.Models.Paging;
using System;

namespace EIP.System.Models.Dtos.Message
{
    /// <summary>
    /// 消息监控日志
    /// </summary>
    public class SystemMessageLogFindMonitorPagingInput: QueryParam
    {
        
    }

    /// <summary>
    /// 消息监控日志
    /// </summary>
    public class SystemMessageLogFindMonitorPagingOutput
    {
        /// <summary>
        /// 消息Id
        /// </summary>		
        public Guid MessageLogId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 消息
        /// </summary>		
        public string Message { get; set; }

        /// <summary>
        /// 接收者Id
        /// </summary>		
        public string ReceiverId { get; set; }

        /// <summary>
        /// 接收者名称
        /// </summary>
        public string ReceiverName { get; set; }

        /// <summary>
        /// 通知类型:0人员,1所有人,2角色,3组织架构,4组,5岗位
        /// </summary>
        public short ReceiverType { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 子系统代码
        /// </summary>		
        public string SubSystemCode { get; set; }

        /// <summary>
        /// 打开方式
        /// </summary>
        public short OpenType { get; set; }

        /// <summary>
        /// 打开地址
        /// </summary>
        public string OpenUrl { get; set; }
    }
}