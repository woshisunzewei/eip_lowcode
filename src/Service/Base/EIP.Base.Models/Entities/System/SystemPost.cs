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
    /// System_Post表实体类
    /// </summary>
	[Serializable]
	[Table("System_Post")]
    public class SystemPost
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        [Key, Identity, JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// 主键
        /// </summary>
		public Guid PostId{ get; set; }
       
        /// <summary>
        /// 组织机构Id
        /// </summary>		
		public Guid OrganizationId{ get; set; }

        /// <summary>
        /// 组织机构名称
        /// </summary>		
        public string OrganizationName { get; set; }

        /// <summary>
        /// 组名称
        /// </summary>		
		public string Name{ get; set; }
       
        /// <summary>
        /// 状态
        /// </summary>		
		public short? State{ get; set; }
       
        /// <summary>
        /// 排序
        /// </summary>		
		public int  OrderNo { get; set; }=0;
       
        /// <summary>
        /// 备注
        /// </summary>		
		public string Remark{ get; set; }
       
        /// <summary>
        /// 冻结
        /// </summary>		
		public bool IsFreeze{ get; set; }

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
