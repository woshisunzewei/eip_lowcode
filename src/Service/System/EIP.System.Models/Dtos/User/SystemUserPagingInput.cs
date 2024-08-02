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
using EIP.Common.Models.Paging;
using EIP.System.Models.Enums;
using System;

namespace EIP.System.Models.Dtos.User
{
    /// <summary>
    /// 用户信息Dto
    /// </summary>
    public class SystemUserPagingInput : QueryParam
    {
        /// <summary>
        /// 对应Id值
        /// </summary>
        public Guid? PrivilegeMasterValue { get; set; }

        /// <summary>
        /// 归属人员类型:组织机构、角色、岗位、组
        /// </summary>
        public EnumPrivilegeMaster PrivilegeMaster { get; set; }

        /// <summary>
        /// 顶级组织架构
        /// </summary>
        public string TopOrg { get; set; }

        /// <summary>
        /// 特定组织ID,多个逗号分割:某些情况需要找特定组织
        /// </summary>
        public string SpecificOrg { get; set; }
    }
}