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
    /// 代码生成器表实体类
    /// </summary>
    [Serializable]
    [Table("System_Agile")]
    public class SystemAgile
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        [Key, Identity, JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
        public Guid ConfigId { get; set; }

        /// <summary>
        /// 表单名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 数据来源
        /// </summary>
        public string DataFromName { get; set; }

        /// <summary>
        /// 数据库Id
        /// </summary>
        public Guid DataBaseId { get; set; }

        /// <summary>
        /// 保存Json配置
        /// </summary>
        public string SaveJson { get; set; }

        /// <summary>
        /// 发布Json配置
        /// </summary>
        public string PublicJson { get; set; }

        /// <summary>
        /// 字段Json，页面设计器具有
        /// </summary>
        public string ColumnJson { get; set; }

        /// <summary>
        /// 是否冻结
        /// </summary>
        public bool IsFreeze { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int OrderNo { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public bool IsDelete { get; set; }

        /// <summary>
        /// 生成类型:1列表配置,2表单配置,3移动端首页/工作台,4移动端自定义,5大屏设计
        /// </summary>
        public short ConfigType { get; set; } = 0;

        /// <summary>
        /// 数据来源类型,1表,2视图,3存储过程
        /// </summary>
        public short DataFrom { get; set; }

        /// <summary>
        /// 关联编辑配置Id
        /// </summary>
        public Guid? EditConfigId { get; set; }

        /// <summary>
        /// 表单类型:1设计器,2自定义
        /// </summary>
        public int? FormCategory { get; set; }

        /// <summary>
        /// 自定义表单Pc地址
        /// </summary>
        public string FormPcUrl { get; set; }

        /// <summary>
        /// 自定义表单手机地址
        /// </summary>
        public string FormMobileUrl { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid? MenuId { get; set; }

        /// <summary>
        /// 缩略图
        /// </summary>
        public string Thumbnail { get; set; }

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