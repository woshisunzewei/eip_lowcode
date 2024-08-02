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
    /// 表单控件
    /// </summary>
    public class WorkflowEngineFormControls
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// 是否为单选
        /// </summary>
        public string IsSingle { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public string Default { get; set; }

        /// <summary>
        /// 子表
        /// </summary>
        public IEnumerable<WorkflowEngineFormSubtableControl> Subtable { get; set; }
    }

    /// <summary>
    /// 子表
    /// </summary>
    public class WorkflowEngineFormSubtableControl
    {

    }
}
