/**************************************************************
* Copyright (C) www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2019/2/13 22:17:08
* 文件名: WorkflowArchivesDto
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

namespace EIP.Workflow.Models.Dtos.Archives
{
    /// <summary>
    /// 输入
    /// </summary>
    public class WorkflowArchiveInput : QueryParam
    {
    }

    /// <summary>
    /// 输出
    /// </summary>
    public class WorkflowArchiveOutput
    {
        /// <summary>
        /// 归档Id
        /// </summary>		
        public Guid ArchiveId { get; set; }

        /// <summary>
        /// 流程实例名称
        /// </summary>
        public string ProcessInstanceTitle { get; set; }

        /// <summary>
        /// 流程名称
        /// </summary>
        public string ProcessName { get; set; }

        /// <summary>
        /// 活动步骤名称
        /// </summary>
        public string ActivityTitle { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 处理人名称
        /// </summary>
        public string DoUserName { get; set; }
    }
}
