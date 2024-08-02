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
using EIP.Common.Models.Attributes.MicroOrm;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace EIP.Base.Models.Entities.System
{
    /// <summary>
    /// 文件存储表
    /// </summary>
    [Serializable]
    [Table("System_File")]
    public class SystemFile
    {

        /// <summary>
        /// 自增Id
        /// </summary>
        [Key, Identity, JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// 主键Id
        /// </summary>
        public Guid FileId { get; set; }

        /// <summary>
        /// 存储类型:公告,新闻等
        /// </summary>		
        public short Type { get; set; }

        /// <summary>
        /// 后缀
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDelete { get; set; } = false;

        /// <summary>
        /// 对应服务表Id
        /// </summary>		
        public string CorrelationId { get; set; }

        /// <summary>
        /// 文件名
        /// </summary>		
        public string Name { get; set; }

        /// <summary>
        /// 后缀
        /// </summary>		
        public string Suffix { get; set; }

        /// <summary>
        /// 文件大小
        /// </summary>		
        public decimal Size { get; set; }

        /// <summary>
        /// 文件保存路径
        /// </summary>		
        public string Path { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string ContentType { get; set; }

        /// <summary>
        /// 创建日期
        /// </summary>		
		public DateTime? CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 创建人
        /// </summary>		
        public Guid? CreateUserId { get; set; }
    }
}
