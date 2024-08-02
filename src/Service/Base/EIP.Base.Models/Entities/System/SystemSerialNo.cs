/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2023/8/22 7:58:35
* 文件名: SystemSerialNo
* 描述: 编号规则
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
    /// 编号规则
    /// </summary>
    [Serializable]
    [Table("system_serialno")]
    public class SystemSerialNo
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key, Identity, JsonIgnore]
        public int Id { get; set; }

            /// <summary>
        /// 
        /// </summary>
        public Guid SerialNoId { get; set; }

        /// <summary>
        /// 配置Id
        /// </summary>
        public Guid ConfigId { get; set; }

        /// <summary>
        /// 字段名称
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// 表达式
        /// </summary>
        public string Rule { get; set; }

        /// <summary>
        /// 最新值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 重新开始时间
        /// </summary>
        public string RepeatTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 创建用户Id
        /// </summary>		
        public Guid CreateUserId { get; set; }

        /// <summary>
        /// 创建人员名称
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 创建用户Id
        /// </summary>		
        public Guid? UpdateUserId { get; set; }

        /// <summary>
        /// 修改人员名称
        /// </summary>
        public string UpdateUserName { get; set; }
    }
}