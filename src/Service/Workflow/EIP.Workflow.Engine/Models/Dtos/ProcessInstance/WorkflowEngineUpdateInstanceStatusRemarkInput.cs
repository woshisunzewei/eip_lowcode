/**************************************************************
* Copyright (C) www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2019/1/24 22:03:12
* 文件名: WorkflowEngineUpdateInstanceStatusRemarkInput
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
    /// 
    /// </summary>
    public class WorkflowEngineUpdateInstanceStatusRemarkInput
    {
        /// <summary>
        /// 事例Id
        /// </summary>
        public Guid ProcessInstanceId { get; set; }

        /// <summary>
        /// 状态值说明
        /// </summary>
        public string StatusRemark { get; set; }
    }
}
