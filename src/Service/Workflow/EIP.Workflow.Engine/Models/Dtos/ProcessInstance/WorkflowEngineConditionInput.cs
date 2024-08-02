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
using EIP.System.Models.Dtos.DataBase;
using System;
using System.Collections.Generic;

namespace EIP.Workflow.Engine.Models.Dtos.ProcessInstance
{
    /// <summary>
    /// 流程引擎条件解析
    /// </summary>
    public class WorkflowEngineConditionInput
    {
        /// <summary>
        /// 条件
        /// </summary>
        public string Judge { get; set; }

        /// <summary>
        /// 表
        /// </summary>
        public string Table { get; set; }

        /// <summary>
        /// 实例
        /// </summary>
        public Guid ProcessInstanceId { get; set; }

        /// <summary>
        /// 控件
        /// </summary>
        public IList<SystemDataBaseSaveBusinessDataColumns> Columns { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ConnectionType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string ConnectionString { get; set; }
    }
}