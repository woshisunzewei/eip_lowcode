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
    /// System_PermissionUser表实体类
    /// </summary>
    [Serializable]
    [Table("System_PermissionUser")]
    public class SystemPermissionUser 
    {
        /// <summary>
        /// 自增Id
        /// </summary>
        [Key, Identity, JsonIgnore]
        public int Id { get; set; }

        /// <summary>
        /// 人员归属类型:角色0,组织机构1,岗位2,组3,人员4(用于查询某用户具有哪些岗位、组等)
        /// </summary>		
        public short PrivilegeMaster { get; set; }

        /// <summary>
        /// 对应类型Id(角色Id,岗位Id,组Id,人员Id)
        /// </summary>		
        public Guid PrivilegeMasterValue { get; set; }

        /// <summary>
        /// 人员Id
        /// </summary>		
        public Guid PrivilegeMasterUserId { get; set; }

        /// <summary>
        /// 是否为关联组织
        /// </summary>
        public bool? IsRelationOrganization { get; set; }
    }
}
