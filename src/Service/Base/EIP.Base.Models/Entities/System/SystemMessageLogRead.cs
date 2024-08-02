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
using EIP.Common.Models.Attributes.MicroOrm;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EIP.Base.Models.Entities.System
{
    /// <summary>
    /// 消息阅读记录表,记录那些人员什么时候已经查看
    /// </summary>
    [Serializable]
    //[NeedCache]
    [Table("System_MessageLogRead")]
    public class SystemMessageLogRead
    {
        [Key, Identity, JsonIgnore]

        public long Id { get; set; }
        /// <summary>
        /// 消息Id
        /// </summary>		
        public Guid MessageLogIdReadId { get; set; }

        /// <summary>
        /// 消息Id
        /// </summary>
        public Guid MessageLogId { get; set; }

        /// <summary>
        /// 阅读用户Id
        /// </summary>		
        public Guid ReadUserId { get; set; }

        /// <summary>
        /// 阅读人员名称
        /// </summary>
        public string ReadUserName { get; set; }

        /// <summary>
        /// 阅读时间
        /// </summary>		
        public DateTime ReadTime { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>		
        public DateTime? DeleteTime { get; set; }
    }
}
