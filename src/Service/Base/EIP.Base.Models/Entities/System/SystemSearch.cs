/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/7/9 22:28:57
* 文件名: SystemSearch
* 描述: 
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
    /// 
    /// </summary>
    [Serializable]
    [Table("system_search")]
    public class SystemSearch
    {
        /// <summary>
        /// 主键
        /// </summary>
        [Key, Identity, JsonIgnore]
        public int Id { get; set; }

            /// <summary>
        /// 查询记录Id
        /// </summary>
        public Guid SearchId { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 菜单Id
        /// </summary>
        public Guid? MenuId { get; set; }

        /// <summary>
        /// 配置项
        /// </summary>
        public string Config { get; set; }

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