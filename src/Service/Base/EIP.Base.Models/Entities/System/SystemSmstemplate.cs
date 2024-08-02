/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2023/4/22 23:09:59
* 文件名: SystemSmstemplate
* 描述: 短信维护
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
    /// 短信维护
    /// </summary>
    [Serializable]
    [Table("system_smstemplate")]
    public class SystemSmstemplate
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key, Identity, JsonIgnore]
        public int Id { get; set; }

            /// <summary>
        /// 
        /// </summary>
        public Guid SmsTemplateId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid SmsConfigId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 短信代码,系统内使用根据此代码获取供应商提供的代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 短信签名
        /// </summary>
        public string Sign { get; set; }

        /// <summary>
        /// 代码:运营商平台审核后提供
        /// </summary>
        public string TemplateId { get; set; }

        /// <summary>
        /// 模板
        /// </summary>
        public string Template { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 短信类型，0 为普通短信，1 营销短信
        /// </summary>
        public int? Type { get; set; }

        /// <summary>
        /// 国家码，如 86 为中国
        /// </summary>
        public string NationCode { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid? CreateUserId { get; set; }

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