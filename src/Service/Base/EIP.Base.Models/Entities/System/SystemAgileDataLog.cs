/**************************************************************
* Copyright (C) 2022 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2018/11/9 9:21:04
* 文件名: 
* 描述: 代码生成器
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
    /// 敏捷开发数据日志
    /// </summary>
    [Serializable]
    [Table("System_Agile_DataLog")]
    public class SystemAgileDataLog
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        [Key, Identity, JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        public Guid AgileDataLogId { get; set; }

        /// <summary>
        /// 敏捷配置Id
        /// </summary>
        public Guid ConfigId { get; set; }

        /// <summary>
        /// 敏捷配置Id
        /// </summary>
        public Guid RelationId { get; set; }

        /// <summary>
        /// 类型，1新增，2修改，3删除
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// 内容:Json格式[{filed:'字段',name:'字段名称',before:'处理前',after:'处理后'}]
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Before { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string After { get; set; }

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

    }
}