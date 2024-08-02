/**************************************************************
* Copyright (C) www.eipflow.com 孙泽伟版权所有(盗版必究)
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
using EIP.Common.Models.Attributes.MicroOrm;
using MiniExcelLibs.Attributes;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EIP.Base.Models.Entities.System
{
    /// <summary>
    /// 短信日志记录
    /// </summary>
    [Serializable]
    [Table("System_SmsLog")]
    public class SystemSmsLog
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        [Key, Identity, JsonIgnore]
        [ExcelColumn(Ignore = true)]
        public int Id { get; set; }

        /// <summary>
        /// 主键Id
        /// </summary>
        [ExcelColumn(Ignore = true)]
        public Guid SmsLogId { get; set; }

        /// <summary>
        /// 通知人
        /// </summary>
        [ExcelColumn(Name = "手机号")]
        public string Phone { get; set; }

        /// <summary>
        /// 发送消息
        /// </summary>		
        [ExcelColumn(Name = "发送消息")]
        public string Message { get; set; }

        /// <summary>
        /// 服务商:阿里云0，腾讯云2，凌凯4
        /// </summary>
        [ExcelColumn(Name = "服务商")]
        public string Provide { get; set; }

        /// <summary>
        /// 短信代码
        /// </summary>
        [ExcelColumn(Name = "短信代码")]
        public string Code { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>		
        [ExcelColumn(Name = "创建时间")]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        [ExcelColumn(Ignore = true)]
        public string Request { get; set; }

        /// <summary>
        /// 输出参数
        /// </summary>		
        [ExcelColumn(Name = "输出参数")]
        public string Response { get; set; }

        /// <summary>
        /// 发送标识
        /// </summary>
        [ExcelColumn(Name = "发送标识")]
        public bool IsSend { get; set; } = false;

        /// <summary>
        /// 发送标识
        /// </summary>
        [ExcelColumn(Name = "发送标识")]
        public bool IsSuccess { get; set; } = false;

        /// <summary>
        /// 
        /// </summary>
        public Guid ToUserId { get; set; }
    }
}
