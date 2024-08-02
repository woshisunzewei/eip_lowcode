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
using EIP.Common.Models.Paging;
using System;

namespace EIP.Workflow.Engine.Models.Dtos.Monitor
{
    /// <summary>
    /// 监控列表输入
    /// </summary>
    public class WorkflowEngineFindMonitorListInput : QueryParam
    {
        
    }

    /// <summary>
    /// 流程监控列表输出
    /// </summary>
    public class WorkflowEngineFindMonitorListOuput 
    {
        /// <summary>
        ///实例Id 
        /// </summary>
        public Guid ProcessInstanceId { get; set; }

        /// <summary>
        /// 流水号:若流程无编码规则则使用年月日时分秒确定
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 流程标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 紧急程度:正常 重要 紧急
        /// </summary>
        public short Urgency { get; set; }

        /// <summary>
        /// 流程状态,正常,终止,中止,即将到期
        /// </summary>		
        public short? Status { get; set; }

        /// <summary>
        /// 当前状态描述:如当前审批节点及审批人
        /// </summary>
        public string StatusRemark { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public string CreateUserName { get; set; }
    }

    /// <summary>
    /// 监控列表输入
    /// </summary>
    public class WorkflowEngineFindMonitorDeleteListInput : QueryParam
    {

    }

    /// <summary>
    /// 流程监控列表输出
    /// </summary>
    public class WorkflowEngineFindMonitorDeleteListOuput 
    {
        /// <summary>
        ///实例Id 
        /// </summary>
        public Guid ProcessInstanceId { get; set; }

        /// <summary>
        /// 流水号:若流程无编码规则则使用年月日时分秒确定
        /// </summary>
        public string Sn { get; set; }

        /// <summary>
        /// 流程标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 删除时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 紧急程度:正常 重要 紧急
        /// </summary>
        public short Urgency { get; set; }

        /// <summary>
        /// 当前状态描述:如当前审批节点及审批人
        /// </summary>
        public string StatusRemark { get; set; }
        /// <summary>
        /// 删除人员
        /// </summary>
        public string UpdateUserName { get; set; }

    }
}