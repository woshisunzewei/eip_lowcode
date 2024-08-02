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
using System.Collections.Generic;

namespace EIP.System.Models.Dtos.DataBase
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemDataBaseSaveFormTableFieldInput
    {
        /// <summary>
        /// 表单Id
        /// </summary>
        public Guid ConfigId { get; set; }

        /// <summary>
        /// 字段信息
        /// </summary>
        public string Columns { get; set; }
    }
   
    /// <summary>
    /// 
    /// </summary>
    public class SystemDataBaseSaveFormTableFieldDetailInput
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public string DataType { get; set; }

        /// <summary>
        /// 字段可否为空
        /// </summary>
        public bool Null { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 帮助
        /// </summary>
        public string Help { get; set; }

        /// <summary>
        /// 控件类型:默认为文本
        /// </summary>
        public string ControlType { get; set; } = "text";

        /// <summary>
        /// 值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 单选
        /// </summary>
        public bool IsSingle { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string MaxLength { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<SystemDataBaseSaveFormTableFieldDetailInput> Children { get; set; } = new List<SystemDataBaseSaveFormTableFieldDetailInput>();

    }

    /// <summary>
    /// 
    /// </summary>
    public class SystemDataBaseColumnJsonInput
    {
        /// <summary>
        /// 字段名称
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public dynamic Options { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<SystemDataBaseColumnJsonBatchOutput> List { get; set; } = new List<SystemDataBaseColumnJsonBatchOutput>();
    }

    /// <summary>
    /// 
    /// </summary>
    public class SystemDataBaseColumnJsonBatchOutput
    {
        public string Type { get; set; }

        public string Model { get; set; }
    }
}