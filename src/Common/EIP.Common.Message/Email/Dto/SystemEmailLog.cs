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

namespace EIP.Common.Message.Email.Dto
{
    /// <summary>
    /// 邮件日志
    /// </summary>
    [Serializable]
    public class SystemEmailLog
    {
        /// <summary>
        /// 消息Id
        /// </summary>		
        public long EmailLogId { get; set; }

        /// <summary>
        /// 邮件名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 接收人员名称
        /// </summary>
        public string ToName { get; set; }

        /// <summary>
        /// 接收人员地址
        /// </summary>
        public string ToAddress { get; set; }

        /// <summary>
        /// 主题
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// 消息
        /// </summary>		
        public string Message { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime CreateTime { get; set; } = DateTime.Now;
        
        /// <summary>
        /// 子系统代码
        /// </summary>		
        public string SubSystemCode { get; set; }

        /// <summary>
        /// 自定义数据
        /// </summary>
        public string CustomData { get; set; }

        /// <summary>
        /// 响应代码
        /// </summary>
        public string ResponseCode { get; set; }

        /// <summary>
        /// 响应消息
        /// </summary>
        public string ResponseMessage { get; set; }

    }
}
