/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/6/1 16:39:27
* 文件名: BigScreenBizComponent
* 描述: 业务组件表
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
    /// 业务组件表
    /// </summary>
    [Serializable]
    [Table("big_screen_biz_component")]
    public class BigScreenBizComponent
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key, Identity, JsonIgnore]
        public int id { get; set; }

            /// <summary>
        /// 业务组件中文名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 业务组件编码，唯一标识符
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 分组
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 封面图片文件路径
        /// </summary>
        public string cover_picture { get; set; }

        /// <summary>
        /// vue组件内容
        /// </summary>
        public String vue_content { get; set; }

        /// <summary>
        /// 组件配置内容
        /// </summary>
        public String setting_content { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int order_num { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string remark { get; set; }

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
        /// 模块编码
        /// </summary>
        public string module_code { get; set; }

        /// <summary>
        /// 删除标识
        /// </summary>
        public int del_flag { get; set; }


    }
}