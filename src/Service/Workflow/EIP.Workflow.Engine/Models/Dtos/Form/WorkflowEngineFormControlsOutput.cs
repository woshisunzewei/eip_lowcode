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

using System.Collections.Generic;

namespace EIP.Workflow.Engine.Models.Dtos.Form
{
    /// <summary>
    /// 表单控件输出值
    /// </summary>
    public class WorkflowEngineFormControlsOutput
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }
    }

    /// <summary>
    /// 子表
    /// </summary>
    public class WorkflowEngineFormSubtableControlOutput
    {
        /// <summary>
        /// 插入表数据
        /// </summary>
        public string DataBaseTable { get; set; }

        /// <summary>
        /// 数据
        /// </summary>
        public IList<WorkflowEngineFormSubtableControlDetailOutput> KeyValue { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowEngineFormSubtableControlDetailOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public IList<WorkflowEngineFormControlsOutput> Detail { get; set; }
    }
}