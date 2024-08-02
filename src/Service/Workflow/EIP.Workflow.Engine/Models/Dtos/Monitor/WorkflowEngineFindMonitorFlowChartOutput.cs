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
using EIP.Base.Models.Entities.Workflow;
using System;
using System.Collections.Generic;

namespace EIP.Workflow.Engine.Models.Dtos.Monitor
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowEngineFindMonitorFlowOutput
    {
        /// <summary>
        /// 设计Json
        /// </summary>
        public string DesignJson { get; set; }

        /// <summary>
        /// 步骤实例
        /// </summary>
        public IList<WorkflowProcessInstanceTask> InstanceProcess { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowEngineFindMonitorFlowStatesOutput
    {
        /// <summary>
        /// 活动id
        /// </summary>
        public Guid activityid { get; set; }

        /// <summary>
        /// 是否通过
        /// </summary>
        public bool ispass { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public short taskstatus { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string type { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public WorkflowEngineGetMonitorFlowStatesTextOutput text { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public WorkflowEngineGetMonitorFlowStatesAttrOutput attr { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public WorkflowEngineGetMonitorFlowStatesPropsOutput props { get; set; }

    }
    
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowEngineGetMonitorFlowStatesPropsOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public WorkflowEngineGetMonitorFlowStatesPropsBaseOutput @base { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public WorkflowEngineGetMonitorFlowStatesPropsUserOutput user { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowEngineGetMonitorFlowStatesPropsBaseOutput
    {
        /// <summary>
        /// 意见显示:
        /// </summary>
        public bool IsOpinion { get; set; }

        /// <summary>
        /// 审签类型
        /// </summary>
        public int CommentType { get; set; }

        /// <summary>
        /// 是否归档
        /// </summary>
        public bool IsArchive { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowEngineGetMonitorFlowStatesPropsUserOutput
    {
        /// <summary>
        /// 处理策略:
        /// 列表中的第一处理人
        /// 发送给列表中的所有处理人
        /// </summary>
        public int approveUserStrategy { get; set; }

        /// <summary>
        /// 无对应处理人时
        /// 不能提交
        /// 跳过本步骤
        /// </summary>
        public int approveUserNoDo { get; set; }

        /// <summary>
        /// 自动同意规则
        /// 处理人就是提交人
        /// 处理人和上一步相同
        /// 处理人已经审批过
        /// </summary>
        public string approveUserAutoAgree { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowEngineGetMonitorFlowStatesTextOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public string text { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowEngineGetMonitorFlowStatesAttrOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public decimal x { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal y { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal width { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal height { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class WorkflowEngineFindMonitorFlowLinksOutput
    {
        /// <summary>
        /// 是否通过
        /// </summary>
        public bool ispass { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public WorkflowEngineGetMonitorFlowLinksPropsOutput props { get; set; }
    }
   
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowEngineGetMonitorFlowLinksPropsOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public WorkflowEngineGetMonitorFlowLinksTextPropsBaseOutput @base { get; set; }
    }


    /// <summary>
    /// 连线
    /// </summary>
    public class WorkflowEngineGetMonitorFlowLinksTextPropsBaseOutput
    {
        /// <summary>
        /// 判断条件
        /// </summary>
        public string judge { get; set; }
        /// <summary>
        /// 表单Id
        /// </summary>
        public Guid? formId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int type { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class WorkflowEngineFindMonitorFlowNodeListOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object text { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object icon { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public object props { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public decimal x { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal y { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal width { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public decimal height { get; set; }

        public short status { get; set; }

        public string remark { get; set; }

        public bool ispass { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class WorkflowEngineFindMonitorFlowLinkListOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string sourceId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string targetId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string label { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public object props { get; set; }

        public string remark { get; set; }

        public WorkflowEngineFindMonitorFlowLinkListClsOutput cls { get; set; }

    }

    public class WorkflowEngineFindMonitorFlowLinkListClsOutput
    {
        public string linkType { get; set; }
        public string linkColor { get; set; }
        public string linkThickness { get; set; }
    }

}
