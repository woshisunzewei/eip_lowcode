using EIP.Workflow.Models.Enums;
using System;
using System.Collections.Generic;

namespace EIP.Workflow.Engine.Models.Dtos.ProcessInstance
{
    /// <summary>
    /// 确定处理人后发起开始流程
    /// </summary>
    public class WorkflowEngineStartProcessRunInput : WorkflowEngineProcessBaseInput
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
        /// 更新活动Id
        /// </summary>
        public List<Guid> Activities { get; set; } = new List<Guid>();

        /// <summary>
        /// 更新连线Id
        /// </summary>
        public List<Guid> Links { get; set; } = new List<Guid>();

        /// <summary>
        /// 下一步
        /// </summary>
        public string NextTaskString { get; set; }


        /// <summary>
        /// 需要发送任务的
        /// </summary>
        public List<WorkflowEngineProcessingPersonOutput> NextTask { get; set; } =
            new List<WorkflowEngineProcessingPersonOutput>();

        /// <summary>
        /// 通知下标,多个逗号分割
        /// </summary>
        public string Notice { get; set; }

    }
}