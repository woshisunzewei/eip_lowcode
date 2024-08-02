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
using System;
using System.Collections.Generic;

namespace EIP.Workflow.Engine.Models.Dtos.ProcessInstance
{
    /// <summary>
    /// 流程运行类
    /// </summary>
    public class WorkflowEngineRunProcessInput : WorkflowEngineProcessBaseInput
    {
        /// <summary>
        /// 活动Id
        /// </summary>
        public Guid TaskId { get; set; }

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
        /// 流程意见
        /// </summary>		
        public string Comment { get; set; }

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
        /// 是否必须选择人员:若为true则直接必须选择处理人员,若为false，则不用判断是否选择人员,不会弹出选择框
        /// </summary>
        public bool NeedChosenPerson { get; set; } = true;

        /// <summary>
        /// 通知下标,多个逗号分割
        /// </summary>
        public string Notice { get; set; }

        /// <summary>
        /// 控件
        /// </summary>
        public IList<SystemDataBaseSaveBusinessDataColumns> Columns { get; set; }

    }
}