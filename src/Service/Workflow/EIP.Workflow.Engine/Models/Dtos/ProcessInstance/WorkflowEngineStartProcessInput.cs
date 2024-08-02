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

using EIP.Workflow.Models.Enums;
using System;
using System.Collections.Generic;

namespace EIP.Workflow.Engine.Models.Dtos.ProcessInstance
{
    /// <summary>
    /// 流程基础类
    /// </summary>
    public class WorkflowEngineStartProcessInput: WorkflowEngineProcessBaseInput
    {
        /// <summary>
        /// 流程Id
        /// </summary>
        public Guid ProcessId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 重要等级
        /// </summary>
        public EnumProcessInstanceUrgency Urgency { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>		
        public Guid CreateUserId { get; set; }

        /// <summary>
        /// 创建人员
        /// </summary>		
        public string CreateUserName { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>		
        public Guid CreateUserOrganizationId { get; set; }

        /// <summary>
        /// 创建用户组织机构
        /// </summary>		
        public string CreateUserOrganizationName { get; set; }

        /// <summary>
        /// 文件Id
        /// </summary>
        public IList<Guid> FileIds { get; set; }

        /// <summary>
        /// 流水号:若流程无编码规则则使用年月日时分秒确定
        /// </summary>		
        public string Sn { get; set; }
    }
}