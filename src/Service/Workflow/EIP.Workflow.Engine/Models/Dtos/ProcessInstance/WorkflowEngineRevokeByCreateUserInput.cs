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
    /// 当前任务节点的上一步节点完成人发现办理有误需撤销，调用此方法，重新回到上一步节点。
    /// </summary>
    public class WorkflowEngineRevokeByCreateUserInput
    {
        /// <summary>
        /// 实例Id
        /// </summary>
        public string ProcessInstanceId { get; set; }

        /// <summary>
        /// 处理人
        /// </summary>		
        public Guid DoUserId { get; set; }

        /// <summary>
        /// 处理人名称
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