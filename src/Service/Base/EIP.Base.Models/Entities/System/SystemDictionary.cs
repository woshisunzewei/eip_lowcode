/**************************************************************
* Copyright (C) 2022 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2022/01/12 22:40:15
* 文件名: 
* 描述: 
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Common.Models.Attributes;
using EIP.Common.Models.Attributes.MicroOrm;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EIP.Base.Models.Entities.System
{
    /// <summary>
    /// System_Dictionary表实体类
    /// </summary>
    [Serializable]
    [Table("System_Dictionary")]
    [Db("EIP")]
    public class SystemDictionary
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        [Key, Identity, JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// 主键Id
        /// </summary>
        public Guid DictionaryId { get; set; }

        /// <summary>
        /// 父级id
        /// </summary>		
        public Guid? ParentId { get; set; }

        /// <summary>
        ///  父级名称
        /// </summary>
        public string ParentName { get; set; }

        /// <summary>
        /// 名称
        /// </summary>		
        public string Name { get; set; }

        /// <summary>
        /// 值
        /// </summary>		
        public string Value { get; set; } = string.Empty;

        /// <summary>
        /// 是否允许删除(系统默认配置字段不允许删除)
        /// </summary>		
        public bool CanbeDelete { get; set; }

        /// <summary>
        ///是否冻结
        /// </summary>		
        public bool IsFreeze { get; set; }

        /// <summary>
        /// 排序
        /// </summary>		
		public int OrderNo { get; set; } = 0;

        /// <summary>
        /// 备注
        /// </summary>		
		public string Remark { get; set; } = string.Empty;

        /// <summary>
        /// 上级所有字符串Id
        /// </summary>		
        public string ParentIds { get; set; } = string.Empty;

        /// <summary>
        /// 上级所有字符串
        /// </summary>
        public string ParentIdsName { get; set; } = string.Empty;

        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 创建用户Id
        /// </summary>		
        public Guid CreateUserId { get; set; }

        /// <summary>
        /// 创建人员名称
        /// </summary>
        public string CreateUserName { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>		
        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 创建用户Id
        /// </summary>		
        public Guid? UpdateUserId { get; set; }

        /// <summary>
        /// 修改人员名称
        /// </summary>
        public string UpdateUserName { get; set; }
    }
}
