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
    /// 获取草稿箱输入参数
    /// </summary>
    public class WorkflowEngineFindDraftInput: QueryParam
    {
        /// <summary>
        /// 创建者
        /// </summary>
        public Guid UserId { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class WorkflowEngineFindDraftOutput 
    {
        /// <summary>
        /// 流程实例Id
        /// </summary>
        public Guid ProcessInstanceId { get; set; }

        /// <summary>
        /// 流程Id
        /// </summary>
        public Guid ProcessId { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime UpdateTime { get; set; }

        /// <summary>
        /// 归属用户
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 紧急程度:正常 重要 紧急
        /// </summary>
        public short Urgency { get; set; }
    }
}
