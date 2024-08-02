/**************************************************************
* Copyright (C) 2022 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2022/01/12 22:40:15
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
    /// System_DataLog表实体类
    /// </summary>
    [Serializable]
    [Table("System_Data")]
    public class SystemData
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        [Key,Identity, JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// 主键Id
        /// </summary>
        public Guid DataId { get; set; }

        /// <summary>
        /// 模块Id
        /// </summary>		
        public Guid MenuId { get; set; }

        /// <summary>
        /// 模块名称
        /// </summary>
        public string MenuName { get; set; }

        /// <summary>
        /// 权限名称
        /// </summary>		
        public string Name { get; set; }

        /// <summary>
        /// 规则Json
        /// </summary>		
        public string RuleJson { get; set; } = string.Empty;

        /// <summary>
        /// 规则Html
        /// </summary>
        public string RuleHtml { get; set; } = string.Empty;

        /// <summary>
        /// 冻结
        /// </summary>
        public bool IsFreeze { get; set; } = false;

        /// <summary>
        /// 排序
        /// </summary>
        public int OrderNo { get; set; } = 0;

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; } = string.Empty;

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
