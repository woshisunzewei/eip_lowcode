/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/6/1 16:39:27
* 文件名: BigScreenPagePreview
* 描述: 页面预览缓存表，每日定时删除
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
    /// 页面预览缓存表，每日定时删除
    /// </summary>
    [Serializable]
    [Table("big_screen_page_preview")]
    public class BigScreenPagePreview
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key, Identity, JsonIgnore]
        public int id { get; set; }

            /// <summary>
        /// 页面编码，页面唯一标识符
        /// </summary>
        public string code { get; set; }

        /// <summary>
        /// 页面配置
        /// </summary>
        public String config { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime create_date { get; set; }


    }
}