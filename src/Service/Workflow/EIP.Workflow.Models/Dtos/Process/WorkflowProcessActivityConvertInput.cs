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
using System;

namespace EIP.Workflow.Models.Dtos.Process
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowProcessActivityConvertInput
    {
        /// <summary>
        /// 活动Id
        /// </summary>
        public Guid ActivityId { get; set; }

        /// <summary>
        /// 基础
        /// </summary>
        public WorkflowProcessActivityConvertBaseInput Base { get; set; }

        /// <summary>
        /// 类型:start round,end round ,task等
        /// </summary>		
        public string Type { get; set; }

        /// <summary>
        /// Json字符串
        /// </summary>
        public string Json { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
    }

    /// <summary>
    /// 基础信息
    /// </summary>
    public class WorkflowProcessActivityConvertBaseInput
    {
        /// <summary>
        /// 表单Id
        /// </summary>		
        public Guid? FormId { get; set; }
    }
}