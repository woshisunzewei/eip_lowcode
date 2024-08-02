/**************************************************************
* Copyright (C) www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2019/1/24 21:47:11
* 文件名: WorkflowEngineSaveInstanceStartTaskInput
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
    public class WorkflowEngineSaveInstanceStartTaskInput
    {
        /// <summary>
        /// 保存人员Id
        /// </summary>
        public Guid CreateUserId { get; set; }
        
        /// <summary>
        /// 保存人员名称
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 保存创建时间
        /// </summary>
        public DateTime CreateDateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 当前活动Id
        /// </summary>
        public Guid ActivityId { get; set; }

        /// <summary>
        /// 流程实例Id
        /// </summary>
        public Guid ProcessInstanceId { get; set; }

        /// <summary>
        /// 自定义数据
        /// </summary>
        public string CustomData { get; set; }
    }
}
