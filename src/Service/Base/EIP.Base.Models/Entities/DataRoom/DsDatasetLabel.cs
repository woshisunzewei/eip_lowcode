/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/5/29 15:57:28
* 文件名: DsDatasetLabel
* 描述: 数据集与标签关联表
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
    /// 数据集与标签关联表
    /// </summary>
    [Serializable]
    [Table("ds_dataset_label")]
    public class DsDatasetLabel
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key, Identity, JsonIgnore]
        public int id { get; set; }

            /// <summary>
        /// 数据集ID
        /// </summary>
        public int? dataset_id { get; set; }

        /// <summary>
        /// 标签ID
        /// </summary>
        public int? label_id { get; set; }


    }
}