/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2023/7/1 16:35:32
* 文件名: SystemDatasource
* 描述: 数据源管理
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

namespace EIP.Base.Models.Entities.System
{
    /// <summary>
    /// 数据源管理
    /// </summary>
    [Serializable]
    [Table("system_datasource")]
    public class SystemDataSource
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key, Identity, JsonIgnore]
        public int Id { get; set; }

            /// <summary>
        /// 
        /// </summary>
        public Guid DataSourceId { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 数据源Id
        /// </summary>
        public Guid DataBaseId { get; set; }

        /// <summary>
        /// 配置信息
        /// </summary>
        public string Config { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int? OrderNo { get; set; }

        /// <summary>
        /// 冻结
        /// </summary>
        public bool? IsFreeze { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid? CreateUserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid? UpdateUserId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string UpdateUserName { get; set; }
    }
}