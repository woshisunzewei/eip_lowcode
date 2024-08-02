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
using EIP.Common.Models.Attributes;
using EIP.Common.Models.Attributes.MicroOrm;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EIP.Base.Models.Entities.Workflow
{
    /// <summary>
    /// 
    /// </summary>
	[Serializable]
    [Table("Workflow_Comment")]
    public class WorkflowComment
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        [Key, Identity, JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// 
        /// </summary>		
        public Guid CommentId { get; set; }

        /// <summary>
        /// 评论内容
        /// </summary>		
        public string Content { get; set; }

        /// <summary>
        /// 类型:0管理员添加、1员工添加
        /// </summary>		
        public int Type { get; set; }

        /// <summary>
        /// 排序
        /// </summary>		
        public int OrderNo { get; set; } = 0;

        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 创建者Id
        /// </summary>		
        public Guid? CreateUserId { get; set; }

        /// <summary>
        /// 创建人员名字
        /// </summary>		
        public string CreateUserName { get; set; }

        /// <summary>
        /// 最有一次修改人员时间
        /// </summary>		
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 最后一次修改人员Id
        /// </summary>		
        public Guid? UpdateUserId { get; set; }

        /// <summary>
        /// 最后修改人员
        /// </summary>		
        public string UpdateUserName { get; set; }
    }
}
