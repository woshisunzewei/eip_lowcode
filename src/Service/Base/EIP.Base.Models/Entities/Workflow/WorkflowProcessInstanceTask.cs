/**************************************************************
* Copyright (C) 2022 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2022/01/12 22:40:15
* 文件名: 
* 描述: 
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Common.Models.Attributes.MicroOrm;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EIP.Base.Models.Entities.Workflow
{
    /// <summary>
    /// 实例任务信息表
    /// </summary>
    [Serializable]
    [Table("Workflow_ProcessInstance_Task")]
    public class WorkflowProcessInstanceTask
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        [Key, Identity, JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// 主键
        /// </summary>		
        public Guid TaskId { get; set; }

        /// <summary>
        /// 属于哪一个流程实例
        /// </summary>		
        public Guid ProcessInstanceId { get; set; }

        /// <summary>
        /// 活动Id
        /// </summary>		
        public Guid ActivityId { get; set; }

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
        public string Comment { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>		
        public short Status { get; set; }

        /// <summary>
        /// 上一个任务Id
        /// </summary>		
        public Guid? PrevTaskId { get; set; }

        /// <summary>
        /// 类型:加签活动,
        /// </summary>
        public short ActivityType { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 扩展属性,如加签类型
        /// </summary>
        public string Extend { get; set; }

        /// <summary>
        /// 自定义数据
        /// </summary>
        public string CustomData { get; set; }

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
