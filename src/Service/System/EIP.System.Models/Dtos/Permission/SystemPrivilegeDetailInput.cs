﻿/**************************************************************
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
using EIP.Common.Models.Dtos;
using EIP.System.Models.Enums;

namespace EIP.System.Models.Dtos.Permission
{
    /// <summary>
    /// 查询系统权限详情输入参数
    /// </summary>
    public class SystemPrivilegeDetailInput : IdInput
    {
        /// <summary>
        /// 权限归属类型
        /// </summary>
        public EnumPrivilegeAccess Access { get; set; }
    }
}