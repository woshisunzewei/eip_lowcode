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
using EIP.Workflow.Models.Enums;
using System;
using System.Collections.Generic;

namespace EIP.Workflow.Engine.Models.Dtos.ProcessInstance
{
    /// <summary>
    /// 流程处理人输出
    /// </summary>
    public class WorkflowEngineProcessingPersonOutput
    {
        /// <summary>
        /// 活动
        /// </summary>
        public WorkflowProcessInstanceActivity Activity { get; set; }

        /// <summary>
        /// 所有人
        /// </summary>
        public IList<WorkflowEngineProcessingPersonDetailOutput> Persons { get; set; } = new List<WorkflowEngineProcessingPersonDetailOutput>();
    }
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowEngineProcessingPersonDetailOutput
    {
        /// <summary>
        /// 处理人
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 选择处理人员模式,单选或多选
        /// </summary>
        public string Pattern { get; set; }

        /// <summary>
        /// 组织机构名称
        /// </summary>		
        public string OrganizationName { get; set; }

        /// <summary>
        /// 用户登录名
        /// </summary>		
        public string Code { get; set; }

        /// <summary>
        /// 头像地址
        /// </summary>		
        public string HeadImage { get; set; }

        /// <summary>
        /// 选择处理人员模式标记
        /// </summary>
        public string PatternName { get; set; }

        /// <summary>
        /// 处理人类型：所有成员,组织,人员,角色,发起者,发起者直线领导等
        /// </summary>
        public short UserTypeSelect { get; set; }
    }

    /// <summary>
    /// 流程处理人
    /// </summary>
    public class WorkflowEngineActivityProcessingPersonOutput
    {
        /// <summary>
        /// 处理人类型：所有成员,组织,人员,角色,发起者,发起者直线领导等
        /// </summary>
        public EnumActivityProcessorType ApproveUserTypeSelect { get; set; }

        /// <summary>
        /// 范围选择名称,多个使用逗号分隔
        /// </summary>
        public string ApproveUserTypeSelectRangeName { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string ApproveUserName { get; set; }

        /// <summary>
        ///范围选择Id,多个使用逗号分隔
        /// </summary>
        public string ApproveUserTypeSelectRangeId { get; set; }

        /// <summary>
        /// 选择处理人员模式:单选,多选
        /// </summary>
        public string ApproveUserTypeSelectPattern { get; set; }

        /// <summary>
        /// 处理人与被处理人相同范围选择
        /// </summary>
        public short? ApproveUserTypeSelectOrganizationRange { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class ApproveUserTypeSelectRangeIdDto
    {
        /// <summary>
        /// 
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
    }
}