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
using System;
using System.Collections.Generic;
using EIP.System.Models.Enums;

namespace EIP.System.Models.Dtos.Permission
{
    /// <summary>
    /// 保存明细
    /// </summary>
    public class SystemPrivilegeDetailOutput
    {
        /// <summary>
        /// 角色
        /// </summary>
        public IList<SystemPrivilegeDetailListOutput> Role { get; set; }

        /// <summary>
        /// 组
        /// </summary>
        public IList<SystemPrivilegeDetailListOutput> Group { get; set; }

        /// <summary>
        /// 岗位
        /// </summary>
        public IList<SystemPrivilegeDetailListOutput> Post { get; set; }

        /// <summary>
        /// 人员
        /// </summary>
        public IList<SystemPrivilegeDetailListOutput> User { get; set; }

        /// <summary>
        /// 组织机构
        /// </summary>
        public IList<SystemPrivilegeDetailListOutput> Organization { get; set; } 
    }

    /// <summary>
    /// 明细
    /// </summary>
    public class SystemPrivilegeDetailListOutput
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 组织机构
        /// </summary>
        public string Organization { get; set; }

        /// <summary>
        /// 组织机构Id
        /// </summary>
        public Guid OrganizationId { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        public Guid PrivilegeMasterValue { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public EnumPrivilegeMaster PrivilegeMaster { get; set; }
    }
}