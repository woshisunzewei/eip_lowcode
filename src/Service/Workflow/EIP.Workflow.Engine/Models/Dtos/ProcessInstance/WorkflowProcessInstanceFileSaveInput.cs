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
using System.Collections.Generic;

namespace EIP.Workflow.Engine.Models.Dtos.ProcessInstance
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowProcessInstanceFileSaveInput
    {
        /// <summary>
        /// 实例Id
        /// </summary>
        public Guid ProcessInstanceId { get; set; }

        /// <summary>
        /// 文件Id
        /// </summary>
        public IList<Guid> FileIds { get; set; }
    }
}
