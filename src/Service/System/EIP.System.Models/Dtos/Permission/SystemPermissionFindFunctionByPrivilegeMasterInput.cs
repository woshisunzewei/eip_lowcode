﻿using EIP.System.Models.Enums;
using System;

namespace EIP.System.Models.Dtos.Permission
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemPermissionFindMenuButtonByPrivilegeMasterInput
    {
        /// <summary>
        /// 
        /// </summary>
        public Guid PrivilegeMasterValue { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public EnumPrivilegeMaster PrivilegeMaster { get; set; }

        /// <summary>
        /// 当前登录人员
        /// </summary>
        public Guid UserId { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public class SystemPermissionFindMenuButtonByPrivilegeMasterOutput
    {
        /// <summary>
        /// 
        /// </summary>
        public string MenuNames { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public Guid MenuId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool Exist { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Guid MenuButtonId { get; set; }
    }
}