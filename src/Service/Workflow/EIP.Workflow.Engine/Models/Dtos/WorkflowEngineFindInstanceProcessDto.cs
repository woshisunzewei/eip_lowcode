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
    /// 流程实例过程输入
    /// </summary>
    public class WorkflowEngineFindInstanceProcessInput
    {
        /// <summary>
        /// 流程实例Id
        /// </summary>
        public Guid ProcessInstanceId { get; set; }
    }

    /// <summary>
    /// 流程实例过程输出
    /// </summary>
    public class WorkflowEngineFindInstanceProcessOutput
    {
        public Guid TaskId { get; set; }
        /// <summary>
        /// 活动Id
        /// </summary>
        public Guid? ActivityId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 发送人员Id
        /// </summary>		
        public Guid SendUserId { get; set; }

        /// <summary>
        /// 发送流程人员名称
        /// </summary>		
        public string SendUserName { get; set; }

        /// <summary>
        /// 任务接收人Id
        /// </summary>		
        public Guid ReceiveUserId { get; set; }

        /// <summary>
        /// 任务接收人名字
        /// </summary>		
        public string ReceiveUserName { get; set; }

        /// <summary>
        /// 接受时间
        /// </summary>		
        public DateTime ReceiveTime { get; set; }

        /// <summary>
        /// 处理人员Id
        /// </summary>		
        public Guid? DoUserId { get; set; }

        /// <summary>
        /// 处理人员登录名
        /// </summary>		
        public string DoUserCode { get; set; }

        /// <summary>
        /// 处理人员名字
        /// </summary>		
        public string DoUserName { get; set; }

        /// <summary>
        /// 完成时间
        /// </summary>		
        public DateTime? CompletedTime { get; set; }
        /// <summary>
        /// 意见
        /// </summary>
        public string Comment { get; set; } = string.Empty;

        /// <summary>
        /// 状态
        /// </summary>
        public short Status { get; set; }

        /// <summary>
        /// 任务列表:如知会等
        /// </summary>
        public short? ActivityType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string HeadImage { get; set; }

        /// <summary>
        /// 审核人员Id
        /// </summary>		
        public Guid? ApproveUserId { get; set; }

        /// <summary>
        /// 审核人员名称
        /// </summary>		
        public string ApproveUserName { get; set; }

        /// <summary>
        /// 审核时间
        /// </summary>		
        public DateTime? ApproveTime { get; set; }

        /// <summary>
        /// 审核结果:0通过,1拒绝
        /// </summary>		
        public int? ApproveStatus { get; set; }

        /// <summary>
        /// 审核意见
        /// </summary>
        public string ApproveComment { get; set; }
    }
}