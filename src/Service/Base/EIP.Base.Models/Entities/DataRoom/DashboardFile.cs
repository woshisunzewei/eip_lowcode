/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/5/29 15:57:26
* 文件名: DashboardFile
* 描述: 文件表
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
    /// 文件表
    /// </summary>
    [Serializable]
    [Table("dashboard_file")]
    public class DashboardFile
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key, Identity, JsonIgnore]
        public int id { get; set; }

            /// <summary>
        /// 模块/类型
        /// </summary>
        public string module { get; set; }

        /// <summary>
        /// 原文件名
        /// </summary>
        public string original_name { get; set; }

        /// <summary>
        /// 新文件名
        /// </summary>
        public string new_name { get; set; }

        /// <summary>
        /// 后缀名(如: txt、png、doc、java等)
        /// </summary>
        public string extension { get; set; }

        /// <summary>
        /// 路径
        /// </summary>
        public string path { get; set; }

        /// <summary>
        /// 访问路径
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>
        public long size { get; set; }

        /// <summary>
        /// 下载次数
        /// </summary>
        public int download_count { get; set; }

        /// <summary>
        /// 上传用户
        /// </summary>
        public string user_name { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime create_date { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        public DateTime update_date { get; set; }

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

        /// <summary>
        /// 桶名称
        /// </summary>
        public string bucket { get; set; }


    }
}