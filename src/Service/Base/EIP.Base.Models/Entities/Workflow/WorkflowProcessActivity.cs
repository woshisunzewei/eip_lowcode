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
    /// 工作流活动表
    /// </summary>
    [Serializable]
    [Table("Workflow_ProcessActivity")]
    public class WorkflowProcessActivity
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        [Key, Identity, JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// 活动Id
        /// </summary>		
        public Guid ActivityId { get; set; }

        /// <summary>
        /// 流程实例Id
        /// </summary>		
        public Guid ProcessId { get; set; }
        
        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 表单Id
        /// </summary>		
        public Guid? FormId { get; set; }

        /// <summary>
        /// Json字符串
        /// </summary>
        public string Json { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Title { get; set; }

    }
}
