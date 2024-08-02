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

namespace EIP.System.Models.Dtos.Permission
{
    /// <summary>
    ///  根据角色Id,岗位Id,组Id,人员Id获取具有的模块信息
    /// </summary>
    public class SystemPermissiontMenuHaveByPrivilegeMasterValueInput 
    {
        /// <summary>
        /// 根据角色Id,岗位Id,组Id,人员Id
        /// </summary>
        public Guid PrivilegeMasterValue { get; set; }

        /// <summary>
        /// 权限类型:角色、岗位、组、人员
        /// </summary>
        public EnumPrivilegeMaster PrivilegeMaster { get; set; }

        /// <summary>
        /// 权限归属:模块,功能项,字段,数据权限
        /// 需要排除模块信息:不在此类型范围类的不给与显示
        /// </summary>
        public EnumPrivilegeAccess? PrivilegeAccess { get; set; }
    }
}