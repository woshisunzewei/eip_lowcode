/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/6/1 13:10:46
* 文件名: BigScreenPageTemplate
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
    [Table("big_screen_page")]
    public class BigScreenPage
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
        /// 页面类型
        /// </summary>
        public string type { get; set; }

        /// <summary>
        /// 页面配置
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
        /// 删除标记0:保留，1:删除
        /// </summary>
        public int del_flag { get; set; }


    }
}