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

namespace EIP.Workflow.Engine.Models.Dtos
{
    /// <summary>
    /// 获取退回活动输入参数
    /// </summary>
    public class WorkflowEngineFindReturnActivityInput
    {
        /// <summary>
        /// 流程实例Id
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
    }
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowEngineFindReturnActivityOutput
    {
        /// <summary>
        /// 活动Id
        /// </summary>
        public Guid ActivityId { get; set; }

        /// <summary>
        /// 活动名字
        /// </summary>
        public string Title { get; set; }
    }
}
