using EIP.Common.Message.Enums;
using EIP.Common.Core.Util;
using System;

namespace EIP.Common.Message
{
    /// <summary>
    /// 消息基础实体
    /// </summary>
    public class MessageBaseInput
    {
        public Guid MessageLogId = CombUtil.NewComb();

        /// <summary>
        /// 接收类型:0人员,1所有人,2角色,3组织架构,4组,5岗位
        /// </summary>
        public EnumReceiverType ReceiverType { get; set; }

        /// <summary>
        /// 接收者,多个使用逗号分隔
        /// </summary>
        public string ReceiverId { get; set; }

        /// <summary>
        /// 接收者名字,多个使用逗号分隔
        /// </summary>
        public string ReceiverName { get; set; }

        /// <summary>
        /// 自定义数据
        /// </summary>
        public string CustomData { get; set; }
    }
}