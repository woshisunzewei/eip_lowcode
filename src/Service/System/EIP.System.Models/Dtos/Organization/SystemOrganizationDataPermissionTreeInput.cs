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
using EIP.Common.Core.Auth;
using EIP.Common.Models.Paging;
using System;

namespace EIP.System.Models.Dtos.Organization
{
    /// <summary>
    /// 获取
    /// </summary>
    public class SystemOrganizationDataPermissionInput : QueryParam
    {
        /// <summary>
        /// Id
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// 登录人信息
        /// </summary>
        public PrincipalUser PrincipalUser { get; set; }

        /// <summary>
        /// 是否包括自己
        /// </summary>
        public bool HaveSelf { get; set; } = true;

        /// <summary>
        /// 1当前部门及下级部门
        /// 2下级部门
        /// 3当前公司
        /// 4当前公司及下级部门
        /// 5当前公司至登录人部门
        /// </summary>
        public int? Range { get; set; }
    }
}