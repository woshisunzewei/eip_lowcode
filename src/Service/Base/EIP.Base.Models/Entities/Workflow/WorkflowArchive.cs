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
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EIP.Base.Models.Entities.Workflow
{
    /// <summary>
    /// Workflow_Archives表实体类
    /// </summary>
	[Serializable]
    [Table("Workflow_Archive")]
    public class WorkflowArchive
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        [Key, Identity, JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// 归档Id
        /// </summary>		
        public Guid ArchiveId { get; set; }

        /// <summary>
        /// 任务Id
        /// </summary>		
        public Guid TaskId { get; set; }

        /// <summary>
        /// 正文
        /// </summary>		
        public string Html { get; set; }

        /// <summary>
        /// 审批内容
        /// </summary>		
        public string Comment { get; set; }

        /// <summary>
        /// 此时运行的流程Json图
        /// </summary>		
        public string DesignJson { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime CreateTime { get; set; } = DateTime.Now;
    }
}
