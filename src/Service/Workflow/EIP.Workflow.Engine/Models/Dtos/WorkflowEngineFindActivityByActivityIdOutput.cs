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
    /// 
    /// </summary>

    public class WorkflowEngineFindActivityByTaskIdOutput
    {
       
        /// <summary>
        /// 活动Id
        /// </summary>
        public Guid ActivityId { get; set; }

        /// <summary>
        /// 表单Id
        /// </summary>
        public Guid FormId { get; set; }

        /// <summary>
        /// 活动配置信息
        /// </summary>
        public string Json { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ColumnJson { get; set; }

        /// <summary>
        /// 流程实例Id
        /// </summary>
        public Guid ProcessInstanceId { get; set; }

        /// <summary>
        /// 活动状态
        /// </summary>
        public int Status { get; set; }
    }
}