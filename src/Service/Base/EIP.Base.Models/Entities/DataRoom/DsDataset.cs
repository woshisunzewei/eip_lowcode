/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/5/29 15:57:28
* 文件名: DsDataset
* 描述: 数据集表
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Common.Models.Attributes.MicroOrm;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EIP.Base.Models.Entities.DataRoom
{
    /// <summary>
    /// 数据集表
    /// </summary>
    [Serializable]
    [Table("ds_dataset")]
    public class DsDataset
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key, Identity, JsonIgnore]
        public int id { get; set; }

            /// <summary>
        /// 名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 种类ID
        /// </summary>
        public string type_id { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 数据集类型（自定义数据集 custom、模型数据集model、原始数据集original、API数据集api、JSON数据集 json）
        /// </summary>
        public string dataset_type { get; set; }

        /// <summary>
        /// 模块编码
        /// </summary>
        public string module_code { get; set; }

        /// <summary>
        /// 是否可编辑，0 不可编辑 1 可编辑
        /// </summary>
        public int editable { get; set; }

        /// <summary>
        /// 数据源ID
        /// </summary>
        public int? source_id { get; set; }

        /// <summary>
        /// 是否对执行结构缓存 0 不缓存 1 缓存
        /// </summary>
        public int cache { get; set; }

        /// <summary>
        /// 数据集配置
        /// </summary>
        public string config { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime update_date { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime create_date { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        public int? create_by { get; set; }

        /// <summary>
        /// 更新人
        /// </summary>
        public int? update_by { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        public int del_flag { get; set; }


    }
}