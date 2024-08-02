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

namespace EIP.Workflow.Engine.Models.Dtos.Form
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowEngineFormSaveFormTableFieldInput
    {
        /// <summary>
        /// 表单Id
        /// </summary>
        public Guid FormId { get; set; }

        /// <summary>
        /// 字段信息
        /// </summary>
        public string Columns { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowEngineFormSaveFormTableFieldDetailInput
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// 字段类型
        /// </summary>
        public string ColumnType { get; set; }

        /// <summary>
        /// 字段可否为空
        /// </summary>
        public bool ColumnNull { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string ColumnDescription { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 是否为单选
        /// </summary>
        public string IsSingle { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 子表
        /// </summary>
        public string SubTable { get; set; }

        /// <summary>
        /// 子表描述
        /// </summary>
        public string SubTableDescription { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowEngineFormSaveFormTableFieldSubTableDetailInput
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        public string ColumnName { get; set; }

        /// <summary>
        /// 备注名称
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 编辑模式值
        /// </summary>
        public SystemDataBaseSaveFormTableFieldSubTableDetailEditModeInput EditMode { get; set; }
    }
    /// <summary>
    /// 编辑类型
    /// </summary>
    public class SystemDataBaseSaveFormTableFieldSubTableDetailEditModeInput
    {
        /// <summary>
        /// 类型:默认为文本类型
        /// </summary>
        public string Type { get; set; } = "text";

        /// <summary>
        /// 配置
        /// </summary>
        public string Config { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowEngineFormSaveFormTableFieldSubTableConfigInput
    {
        /// <summary>
        /// 是否单选
        /// </summary>
        public string IsSingle { get; set; }
    }
}