/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/5/29 15:57:27
* 文件名: DashboardPageTemplate
* 描述: 页面模板表
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
    /// 页面模板表
    /// </summary>
    [Serializable]
    [Table("dashboard_page_template")]
    public class DashboardPageTemplate
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key, Identity, JsonIgnore]
        public int id { get; set; }

            /// <summary>
        /// 模板名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 模板分类
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 模板配置
        /// </summary>
        public string config { get; set; }

        /// <summary>
        /// 缩略图
        /// </summary>
        public string thumbnail { get; set; }

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
        /// 删除标记0:保留，1:删除
        /// </summary>
        public int del_flag { get; set; }


    }
}