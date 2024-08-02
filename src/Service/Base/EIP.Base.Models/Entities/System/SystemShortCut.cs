/**************************************************************
* Copyright (C) www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
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
using EIP.Common.Models.Attributes.MicroOrm.Joins;
using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EIP.Base.Models.Entities.System
{
    /// <summary>
    /// 系统快捷方式
    /// </summary>
    [Serializable]
	[Table("System_ShortCut")]
    public class SystemShortCut
    {
        /// <summary>
        /// 
        /// </summary>
        [Key, Identity, JsonIgnore]
        public int Id { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public Guid ShortCutId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>		
		public Guid UserId{ get; set; }
       
        /// <summary>
        /// 模块Id
        /// </summary>		
		public Guid MenuId{ get; set; }

        /// <summary>
        /// 排序号
        /// </summary>
        public int OrderNo { get; set; } = 0;

        /// <summary>
        /// 类型
        /// </summary>
        public short Type { get; set; } = 0;

        /// <summary>
        /// 
        /// </summary>
        public DateTime CreateTime { get; set; } = DateTime.Now;

        /// <summary>
        /// 菜单
        /// </summary>
        [InnerJoin("System_Menu", "MenuId", "MenuId")]
        [JsonIgnore]
        public SystemMenu SystemMenu { get; set; }
    } 
}
