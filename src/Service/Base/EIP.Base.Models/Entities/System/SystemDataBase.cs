/**************************************************************
* Copyright (C) 2018 www.sf-info.cn 盛峰版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 3365690)
* 创建时间: 2018/10/30 22:40:15
* 文件名: 
* 描述: 
* 
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
    /// 数据库
    /// </summary>
    [Serializable]
    [Table("System_DataBase")]
    public class SystemDataBase
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        [Key, Identity, JsonIgnore]
        public int Id { get; set; }

        /// <summary>   
        /// 主键Id
        /// </summary>
        public Guid DataBaseId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int OrderNo { get; set; } = 0; 

        /// <summary>
        /// 数据库配置名称,需要在配置文件中配置数据库连接
        /// </summary>
        public string ConnectionString { get; set; }

        /// <summary>
        /// 数据库类型：支持mysql,mssql
        /// </summary>
        public string ConnectionType { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 创建人
        /// </summary>		
        public Guid CreateUserId { get; set; }

        /// <summary>
        /// 创建人名称
        /// </summary>		
        public string CreateUserName { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>		
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>		
        public Guid? UpdateUserId { get; set; }

        /// <summary>
        /// 修改人名称
        /// </summary>		
        public string UpdateUserName { get; set; }
    }
}