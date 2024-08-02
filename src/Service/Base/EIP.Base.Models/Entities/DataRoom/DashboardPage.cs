/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/5/29 15:57:27
* 文件名: DashboardPage
* 描述: 页面基本信息表
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
    /// 页面基本信息表
    /// </summary>
    [Serializable]
    [Table("dashboard_page")]
    public class DashboardPage
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key, Identity, JsonIgnore]
        public int id { get; set; }

            /// <summary>
        /// 页面中文名称
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// 页面编码，页面唯一标识符
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 封面图片文件路径
        /// </summary>
        public string cover_picture { get; set; }

        /// <summary>
        /// 页面图标
        /// </summary>
        public string icon { get; set; }

        /// <summary>
        /// 图标颜色
        /// </summary>
        public string icon_color { get; set; }

        /// <summary>
        /// 页面类型
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 组件布局，记录组件的相对位置和顺序
        /// </summary>
        public string layout { get; set; }

        /// <summary>
        /// 表单属性，只有表单类型时才有这个值
        /// </summary>
        public string config { get; set; }

        /// <summary>
        /// 父级目录编码
        /// </summary>
        public string parent_code { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int order_num { get; set; }

        /// <summary>
        /// 备忘
        /// </summary>
        public string remark { get; set; }

        /// <summary>
        /// 模型编码
        /// </summary>
        public string model_code { get; set; }

        /// <summary>
        /// 所属应用编码
        /// </summary>
        public string app_code { get; set; }

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
        /// 删除标识符 1 删除 0未删
        /// </summary>
        public int del_flag { get; set; }


    }
}