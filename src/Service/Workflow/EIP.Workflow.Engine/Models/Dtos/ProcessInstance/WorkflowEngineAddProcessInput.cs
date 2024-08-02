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

namespace EIP.Workflow.Engine.Models.Dtos.ProcessInstance
{
    /// <summary>
    /// 加签参数
    /// </summary>
    public class WorkflowEngineAddProcessInput
    {
        /// <summary>
        /// 流程Id
        /// </summary>
        public Guid ProcessId { get; set; }

        /// <summary>
        /// 实例Id
        /// </summary>
        public Guid ProcessInstanceId { get; set; }

        /// <summary>
        /// 任务Id
        /// </summary>
        public Guid TaskId { get; set; }

        /// <summary>
        /// 活动Id
        /// </summary>
        public Guid ActivityId { get; set; }

        /// <summary>
        /// 活动类别
        /// </summary>
        public string ActivityType { get; set; }

        #region 开始活动具备
        /// <summary>
        /// 重要等级
        /// </summary>
        public int Urgency { get; set; }

        /// <summary>
        /// 流程标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>		
        public Guid DoUserOrganizationId { get; set; }

        /// <summary>
        /// 创建用户组织机构
        /// </summary>		
        public string DoUserOrganizationName { get; set; }
        #endregion

        /// <summary>
        /// 处理人
        /// </summary>		
        public Guid DoUserId { get; set; }

        /// <summary>
        /// 处理人名称
        /// </summary>		
        public string DoUserName { get; set; }

        /// <summary>
        /// 意见
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// 扩展属性
        /// </summary>
        public WorkflowEngineAddProcessExtendInput Extend { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowEngineAddProcessExtendInput
    {
        /// <summary>
        /// 类型
        /// </summary>
        public EnumTaskAddType Type { get; set; }

        /// <summary>
        /// 加签后
        /// </summary>
        public EnumTaskAddApprove Approve { get; set; }

        /// <summary>
        /// 加签人员
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 归属任务Id,加签只管最顶级任务Id
        /// </summary>
        public Guid BelongToTaskId { get; set; }

        /// <summary>
        /// 归属任务Id,加签只管最顶级任务Id
        /// </summary>
        public Guid FromTaskId { get; set; }
    }
}