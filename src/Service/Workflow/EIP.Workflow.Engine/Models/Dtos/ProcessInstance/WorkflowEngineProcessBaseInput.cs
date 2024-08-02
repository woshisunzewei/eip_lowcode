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
using EIP.System.Models.Dtos.DataBase;
using System.Collections.Generic;
using EIP.Workflow.Models.Enums;

namespace EIP.Workflow.Engine.Models.Dtos.ProcessInstance
{
    /// <summary>
    /// 流程基础类
    /// </summary>
    public class WorkflowEngineProcessBaseInput
    {
        /// <summary>
        /// 当前活动Id
        /// </summary>
        public Guid ActivityId { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public EnumAcitvityType Type { get; set; }

        /// <summary>
        /// 自定义数据
        /// </summary>
        public string CustomData { get; set; }

        /// <summary>
        /// 属于哪一个流程实例
        /// </summary>		
        public Guid ProcessInstanceId { get; set; }

        /// <summary>
        /// 表单Id
        /// </summary>
        public Guid? FormId { get; set; }

        /// <summary>
        /// 控件
        /// </summary>
        public IList<SystemDataBaseSaveBusinessDataColumns> Columns { get; set; }

    }
}