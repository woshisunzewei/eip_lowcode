﻿/**************************************************************
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
using System.Collections.Generic;

namespace EIP.Workflow.Engine.Models.Dtos.Form
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowEngineFormSaveDataInput
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public Guid UserId { get; set; }

        /// <summary>
        /// 用户名代码
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// 用户名称
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户组织结构Id
        /// </summary>
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// 用户组织结构名称
        /// </summary>
        public string OrganizationName { get; set; }

        /// <summary>
        /// 表单Id
        /// </summary>
        public Guid FormId { get; set; }
        
        /// <summary>
        /// 实例Id
        /// </summary>
        public Guid ProcessInstanceId { get; set; }

        /// <summary>
        /// 活动类型
        /// </summary>
        public string ActivityType { get; set; }

        /// <summary>
        ///  若从移动端来则用字符串
        /// </summary>
        public string ControlsString { get; set; }

        /// <summary>
        /// 控件
        /// </summary>
        public IList<WorkflowEngineFormControls> Controls { get; set; }
    }
}