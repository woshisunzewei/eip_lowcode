/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/5/29 15:57:29
* 文件名: DsDatasource
* 描述: 数据源配置表
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
    /// 数据源配置表
    /// </summary>
    [Serializable]
    [Table("ds_datasource")]
    public class DsDatasource
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key, Identity, JsonIgnore]
        public int id { get; set; }

            /// <summary>
        /// 数据源名称
        /// </summary>
        public string source_name { get; set; }

        /// <summary>
        /// 数据源类型
        /// </summary>
        public string source_type { get; set; }

        /// <summary>
        /// 连接驱动
        /// </summary>
        public string driver_class_name { get; set; }

        /// <summary>
        /// 连接url
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 主机
        /// </summary>
        public string host { get; set; }

        /// <summary>
        /// 端口
        /// </summary>
        public int? port { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string username { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string password { get; set; }

        /// <summary>
        /// 表名
        /// </summary>
        public string table_name { get; set; }

        /// <summary>
        /// 模块编码
        /// </summary>
        public string module_code { get; set; }

        /// <summary>
        /// 是否可编辑，0 不可编辑 1 可编辑
        /// </summary>
        public int editable { get; set; }

        /// <summary>
        /// 
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
        /// 删除标识
        /// </summary>
        public int del_flag { get; set; }


    }
}