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
    /// 流程实例:所有发送流程主表
    /// </summary>
    [Serializable]
	[Table("Workflow_ProcessInstance")]
    public class WorkflowProcessInstance
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        [Key, Identity, JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// 流程实例Id
        /// </summary>		
        public Guid ProcessInstanceId{ get; set; }
       
        /// <summary>
        /// 流程Id
        /// </summary>		
		public Guid ProcessId{ get; set; }
       
        /// <summary>
        /// 流水号:若流程无编码规则则使用年月日时分秒确定
        /// </summary>		
		public string Sn{ get; set; }
       
        /// <summary>
        /// 流程标题
        /// </summary>		
		public string Title{ get; set; }
       
        /// <summary>
        /// 流程状态,正常,终止,中止,即将到期
        /// </summary>		
		public short? Status{ get; set; }

        /// <summary>
        /// 当前状态描述:如当前审批节点及审批人
        /// </summary>
        public string StatusRemark { get; set; }

        /// <summary>
        /// 类型:草稿,范本,保存,流程中
        /// </summary>		
		public short Type{ get; set; }
       
        /// <summary>
        /// 紧急程度:正常 重要 紧急
        /// </summary>		
		public short? Urgency{ get; set; }
       
        /// <summary>
        /// 创建时间
        /// </summary>		
		public DateTime CreateTime{ get; set; }
       
        /// <summary>
        /// 创建人
        /// </summary>		
		public Guid CreateUserId{ get; set; }

        /// <summary>
        /// 创建人员
        /// </summary>		
		public string CreateUserName{ get; set; }

        /// <summary>
        /// 创建人
        /// </summary>		
        public Guid CreateUserOrganizationId { get; set; }

        /// <summary>
        /// 创建用户组织机构
        /// </summary>		
		public string CreateUserOrganizationName{ get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>		
        public Guid UpdateUserId { get; set; }

        /// <summary>
        /// 创建人员
        /// </summary>		
        public string UpdateUserName { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>		
        public Guid UpdateUserOrganizationId { get; set; }

        /// <summary>
        /// 创建用户组织机构
        /// </summary>		
        public string UpdateUserOrganizationName { get; set; }

        /// <summary>
        /// 来源于某个任务Id
        /// </summary>
        public Guid? FromTaskId { get; set; }

        /// <summary>
        /// 表单数据
        /// </summary>
        public string FormData { get; set; }
    } 
}
