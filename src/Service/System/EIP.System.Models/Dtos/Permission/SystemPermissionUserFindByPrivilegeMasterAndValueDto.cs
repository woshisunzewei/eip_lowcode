/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
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
using EIP.Common.Models.Attributes.MicroOrm.Joins;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EIP.System.Models.Dtos.Permission
{
    /// <summary>
    /// 根据人员归属类型及对应类型值获取对应人员
    /// </summary>
    [Table("System_PermissionUser")]
    public class SystemPermissionUserFindByPrivilegeMasterAndValueOutput
    {
        /// <summary>
        /// 人员归属类型:角色0,组织机构1,岗位2,组3,人员4(用于查询某用户具有哪些岗位、组等)
        /// </summary>
        [Key]
        public short PrivilegeMaster { get; set; }

        /// <summary>
        /// 对应类型Id(角色Id,岗位Id,组Id,人员Id)
        /// </summary>		
        public Guid PrivilegeMasterValue { get; set; }

        /// <summary>
        /// 用户
        /// </summary>
        [LeftJoin("System_UserInfo", "PrivilegeMasterUserId", "UserId")]
        public List<SystemPermissionUserFindByPrivilegeMasterAndValueUsersOutput> Users { get; set; }
    }

    /// <summary>
    /// 用户
    /// </summary>
    public class SystemPermissionUserFindByPrivilegeMasterAndValueUsersOutput
    {
        /// <summary>
        /// 人员Id
        /// </summary>		
        [Key]
        public Guid UserId { get; set; }

        /// <summary>
        /// 组织机构Id
        /// </summary>
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// 用户真实姓名
        /// </summary>		
        public string Name { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>		
        public string Mobile { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>		
        public string Email { get; set; }

        /// <summary>
        /// 组织机构名称
        /// </summary>		
        public string OrganizationName { get; set; }

        /// <summary>
        /// 用户登录名
        /// </summary>		
        public string Code { get; set; }

        /// <summary>
        /// 头像地址
        /// </summary>		
        public string HeadImage { get; set; }

    }
}