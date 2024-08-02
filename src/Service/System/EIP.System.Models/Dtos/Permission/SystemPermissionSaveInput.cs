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
using EIP.System.Models.Enums;
using System;
using System.Collections.Generic;

namespace EIP.System.Models.Dtos.Permission
{
    /// <summary>
    /// 保存权限输入参数
    /// </summary>
    public class SystemPermissionSaveInput 
    {
        /// <summary>
        /// 添加类型:模块、功能项
        /// </summary>
        public EnumPrivilegeAccess PrivilegeAccess { get; set; }

        /// <summary>
        /// 添加的类型:角色、组织机构、组、岗位、人员
        /// </summary>
        public EnumPrivilegeMaster PrivilegeMaster { get; set; }

        /// <summary>
        /// 权限信息
        /// </summary>
        public IList<Guid> Permissiones { get; set; }

        /// <summary>
        /// 角色、组织机构、组、岗位、人员对应的主键Id
        /// </summary>
        public Guid PrivilegeMasterValue { get; set; }

        /// <summary>
        /// 对应模块Id(字段权限、数据权限才有该字段)
        /// </summary>
        public Guid? PrivilegeMenuId { get; set; }  

        /// <summary>
        /// 权限信息
        /// </summary>
        public string MenuPermissions { get; set; }
    }
}