/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2023/6/30 14:30:08
* 文件名: SystemReportUnit
* 描述: 系统报表模块单元
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
    /// 系统报表模块单元
    /// </summary>
    [Serializable]
    [Table("system_report_unit")]
    public class SystemReportUnit
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key, Identity, JsonIgnore]
        public int Id { get; set; }

            /// <summary>
        /// 
        /// </summary>
        public Guid UnitId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid UnitTypeId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Config { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int? OrderNo { get; set; }

        /// <summary>
        /// 刷新
        /// </summary>
        public bool IsRefresh { get; set; }

        /// <summary>
        /// 刷新时间
        /// </summary>
        public int RefreshTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsLogin { get; set; }

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