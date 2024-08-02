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

namespace EIP.System.Models.Dtos.Permission
{
    /// <summary>
    /// 保存权限用户
    /// </summary>
    public class SystemPermissionSaveUserInput
    {
        /// <summary>
        /// 用户
        /// </summary>
        public Guid U { get; set; }

        /// <summary>
        /// 特性Id
        /// </summary>
        public Guid PrivilegeMasterValue { get; set; }

        /// <summary>
        /// 特性Id
        /// </summary>
        public int PrivilegeMaster { get; set; }
    }
}
