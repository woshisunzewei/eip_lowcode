/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2018/10/30 22:40:15
* 文件名: 
* 描述: 
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/

using System;

namespace EIP.Common.Message
{
    /// <summary>
    /// 消息记录表
    /// </summary>
    [Serializable]
    public class SystemMessageLog
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
        public DateTime CreateTime { get; set; }=DateTime.Now;
        
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

        /// <summary>
        /// 自定义数据
        /// </summary>
        public string CustomData { get; set; }
    }
}
