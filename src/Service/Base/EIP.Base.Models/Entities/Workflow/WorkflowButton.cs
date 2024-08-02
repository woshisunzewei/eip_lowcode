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

namespace EIP.Base.Models.Entities.Workflow
{
    /// <summary>
    /// Workflow_Button表实体类
    /// </summary>
	[Serializable]
    [Table("Workflow_Button")]
    public class WorkflowButton
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        [Key, Identity, JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// 按钮Id
        /// </summary>
        public Guid ButtonId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>		
        public string Name { get; set; }

        /// <summary>
        /// 图标
        /// </summary>		
        public string Icon { get; set; } = string.Empty;

        /// <summary>
        /// 主题
        /// </summary>		
        public string Theme { get; set; } = string.Empty;

        /// <summary>
        /// 脚本
        /// </summary>		
        public string Script { get; set; } = string.Empty;

        /// <summary>
        /// 备注
        /// </summary>		
        public string Remark { get; set; } = string.Empty;

        /// <summary>
        /// 排序字段
        /// </summary>		
        public int OrderNo { get; set; } = 0;

        /// <summary>
        /// 手机端是否显示
        /// </summary>
        public bool MobileShow { get; set; } = true;

        /// <summary>
        /// 按钮类型
        /// </summary>
        public string Style { get; set; }
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
