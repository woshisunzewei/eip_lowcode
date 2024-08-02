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

namespace EIP.Common.Message.Sms.Dto
{
    /// <summary>
    /// 短信日志记录
    /// </summary>
    [Serializable]
    public class SystemSmsLog
    {
        /// <summary>
        /// 消息Id
        /// </summary>		
        public Guid SmsLogId { get; set; }

        /// <summary>
        /// 通知人
        /// </summary>
        public string Phone { get; set; }

        /// <summary>
        /// 发送消息
        /// </summary>		
        public string Message { get; set; }

        /// <summary>
        /// 服务商
        /// </summary>
        public int Provide { get; set; }

        /// <summary>
        /// 短信代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>		
        public string CreateTime { get; set; }
        
        /// <summary>
        /// 子系统代码
        /// </summary>		
        public string SubSystemCode { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        public string Request { get; set; }

        /// <summary>
        /// 输出参数
        /// </summary>		
        public string Response { get; set; }

        /// <summary>
        /// 是否已发送
        /// </summary>
        public bool IsSend { get; set; } = false;
    }
}
