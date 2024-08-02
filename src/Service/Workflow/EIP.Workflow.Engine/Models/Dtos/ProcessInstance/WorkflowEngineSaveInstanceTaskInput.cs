using System;
using System.Collections.Generic;

namespace EIP.Workflow.Engine.Models.Dtos.ProcessInstance
{
    /// <summary>
    /// 保存实例任务
    /// </summary>
    public class WorkflowEngineSaveInstanceTaskInput
    {
        /// <summary>
        /// 当前步骤任务Id
        /// </summary>
        public Guid TaskId { get; set; }

        /// <summary>
        /// 保存人员Id
        /// </summary>
        public Guid CreateUserId { get; set; }

        /// <summary>
        /// 保存人员名称
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
        /// 保存创建时间
        /// </summary>
        public DateTime CreateDateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 上一个任务Id,用于归属查询,前后关系
        /// </summary>
        public Guid? PrevTaskId { get; set; }

        /// <summary>
        /// 流程实例Id
        /// </summary>
        public Guid ProcessInstanceId { get; set; }

        /// <summary>
        /// 需要发送任务的用户集合
        /// </summary>
        public IList<WorkflowEngineProcessingPersonOutput> Persons { get; set; } = new List<WorkflowEngineProcessingPersonOutput>();

        /// <summary>
        /// 自定义数据
        /// </summary>
        public string CustomData { get; set; }
    }
}