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

namespace EIP.Workflow.Engine.Models.Dtos
{
    /// <summary>
    /// 已处理事项查询参数
    /// </summary>
    public class WorkflowEngineFindHaveDoInput : QueryParam
    {
        /// <summary>
        /// 用户id
        /// </summary>
        public Guid UserId { get; set; }
    }

    /// <summary>
    /// 已处理事项输出参数
    /// </summary>
    public class WorkflowEngineFindHaveDoOutput
    { /// <summary>
      /// 任务Id
      /// </summary>
        public Guid TaskId { get; set; }
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
        /// 流程类型简称
        /// </summary>
        public string ShortName { get; set; }

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
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid ProcessId { get; set; }


        /// <summary>
        /// 
        /// </summary>
        public Guid? FormId { get; set; }

        /// <summary>
        /// 完成时间
        /// </summary>
        public DateTime? CompletedTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int InstanceStatus { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// 活动标题
        /// </summary>
        public string ActivityTitle { get; set; }
    }
}