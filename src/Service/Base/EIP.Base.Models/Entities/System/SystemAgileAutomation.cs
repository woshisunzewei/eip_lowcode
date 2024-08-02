/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/2/3 22:14:39
* 文件名: SystemAgileAutomation
* 描述: 自动化构建
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
    /// 自动化构建
    /// </summary>
    [Serializable]
    [Table("system_agile_automation")]
    public class SystemAgileAutomation
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key, Identity, JsonIgnore]
        public int Id { get; set; }

            /// <summary>
        /// 配置Id
        /// </summary>
        public Guid ConfigId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 保存Json
        /// </summary>
        public string SaveJson { get; set; }

        /// <summary>
        /// 发布Json
        /// </summary>
        public string PublicJson { get; set; }

        /// <summary>
        /// 工作表Id
        /// </summary>
        public Guid? TableId { get; set; }

        /// <summary>
        /// 工作表触发类型
        /// </summary>
        public int? TableTriggerType { get; set; }

        /// <summary>
        /// 冻结
        /// </summary>
        public bool IsFreeze { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int OrderNo { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; } = false;

        /// <summary>
        /// 触发类型:1表单，2定时
        /// </summary>
        public int? ConfigType { get; set; }

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