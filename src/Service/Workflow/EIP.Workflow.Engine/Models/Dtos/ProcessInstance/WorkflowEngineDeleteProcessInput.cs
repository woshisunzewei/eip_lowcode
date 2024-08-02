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

using System;

namespace EIP.Workflow.Engine.Models.Dtos.ProcessInstance
{
    /// <summary>
    /// 删除后的任务将被放入系统回收箱，管理员可以【恢复】或【彻底删除】回收箱中的任务。
    /// </summary>
    public class WorkflowEngineDeleteByTaskIdInput : WorkflowEngineProcessBaseInput
    {
        /// <summary>
        /// 任务Id
        /// </summary>
        public Guid TaskId { get; set; }

        /// <summary>
        /// 处理人员Id
        /// </summary>		
        public Guid DoUserId { get; set; }

        /// <summary>
        /// 处理人员名字
        /// </summary>		
        public string DoUserName { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>		
        public Guid DoUserOrganizationId { get; set; }

        /// <summary>
        /// 创建用户组织机构
        /// </summary>		
        public string DoUserOrganizationName { get; set; }

        /// <summary>
        /// 意见
        /// </summary>
        public string Comment { get; set; }
    }

    /// <summary>
    /// 删除后的任务将被放入系统回收箱，管理员可以【恢复】或【彻底删除】回收箱中的任务。
    /// </summary>
    public class WorkflowEngineDeleteByProcessInstanceIdInput : WorkflowEngineProcessBaseInput
    {
        /// <summary>
        /// 处理人员Id
        /// </summary>		
        public Guid DoUserId { get; set; }

        /// <summary>
        /// 处理人员名字
        /// </summary>		
        public string DoUserName { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>		
        public Guid DoUserOrganizationId { get; set; }

        /// <summary>
        /// 创建用户组织机构
        /// </summary>		
        public string DoUserOrganizationName { get; set; }

        /// <summary>
        /// 意见
        /// </summary>
        public string Comment { get; set; }
    }
}