/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2023/4/22 23:09:59
* 文件名: SystemSmsconfig
* 描述: 短信配置
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
    /// 短信配置
    /// </summary>
    [Serializable]
    [Table("system_smsconfig")]
    public class SystemSmsconfig
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key, Identity, JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid SmsConfigId { get; set; }

        /// <summary>
        /// 配置名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 来源
        /// </summary>
        public string Provide { get; set; }

        /// <summary>
        /// AppId
        /// </summary>
        public string AppId { get; set; }

        /// <summary>
        /// AppKey
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        /// 冻结
        /// </summary>
        public bool IsFreeze { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid CreateUserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid? UpdateUserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UpdateUserName { get; set; }


    }
}