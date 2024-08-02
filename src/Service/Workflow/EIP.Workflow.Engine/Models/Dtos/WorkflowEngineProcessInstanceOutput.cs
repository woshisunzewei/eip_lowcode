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
using EIP.System.Models.Dtos.DataBase;
using EIP.Base.Models.Entities.Workflow;
using System.Collections.Generic;

namespace EIP.Workflow.Engine.Models.Dtos
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowEngineProcessInstanceOutput
    {
        /// <summary>
        /// 实例
        /// </summary>
        public WorkflowProcessInstance Instance { get; set; }

        /// <summary>
        /// 流程运行中节点
        /// </summary>
        public IList<WorkflowProcessInstanceActivity> InstanceActivities { get; set; }

        /// <summary>
        /// 流程运行中连线
        /// </summary>
        public IList<WorkflowProcessInstanceLink> InstanceLinks { get; set; }

        /// <summary>
        /// 流程运行中任务
        /// </summary>
        public IList<WorkflowProcessInstanceTask> InstanceTasks { get; set; }

        /// <summary>
        /// 控件
        /// </summary>
        public IList<SystemDataBaseSaveBusinessDataColumns> Columns { get; set; }=new List<SystemDataBaseSaveBusinessDataColumns>();

        /// <summary>
        /// 流程运行中节点
        /// </summary>
        public IList<WorkflowProcessActivity> Activities { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class WorkflowEngineProcessOutput
    {
        /// <summary>
        /// 实例
        /// </summary>
        public WorkflowProcess Process { get; set; }

        /// <summary>
        /// 流程运行中节点
        /// </summary>
        public IEnumerable<WorkflowProcessActivity> ProcessActivities { get; set; }

        /// <summary>
        /// 流程运行中连线
        /// </summary>
        public IEnumerable<WorkflowProcessLink> ProcessLinks { get; set; }

    }
}
