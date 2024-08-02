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
using EIP.Common.Models.Attributes;
using EIP.Workflow.Models.Enums;
using System;

namespace EIP.Workflow.Engine.Models.Dtos
{
    /// <summary>
    /// 根据类型和流程Id获取所有合适的活动信息
    /// </summary>
    public class WorkflowEngineFindActivityByProcessIdAndTypeInput
    {
        /// <summary>
        /// 流程Id
        /// </summary>
        public Guid ProcessId { get; set; }

        /// <summary>
        /// 活动类型
        /// </summary>
        public EnumAcitvityType? Type { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class WorkflowEngineFindActivityByProcessIdAndTypeOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid ActivityId { get; set; }

        /// <summary>
        /// 流程实例Id
        /// </summary>		
        public Guid ProcessId { get; set; }

        /// <summary>
        /// 表单Id
        /// </summary>		
        public Guid? FormId { get; set; }


        /// <summary>
        /// 类型:start round,end round ,task等
        /// </summary>		
        public string Type { get; set; }

        /// <summary>
        /// 步骤Json
        /// </summary>		
        public string Json { get; set; }

        /// <summary>
        /// 表单设计器
        /// </summary>		
        public string PublicJson { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ColumnJson { get; set; }
    }
}