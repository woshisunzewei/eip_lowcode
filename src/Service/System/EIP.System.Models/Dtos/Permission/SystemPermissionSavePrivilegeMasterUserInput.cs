using EIP.System.Models.Enums;
using System;

namespace EIP.System.Models.Dtos.Permission
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemPermissionSavePrivilegeMasterUserInput
    {
        /// <summary>
        /// 用户json字符串
        /// </summary>
        public string PrivilegeMasterUser { get; set; }
        /// <summary>
        /// 角色信息
        /// </summary>
        public Guid PrivilegeMasterValue { get; set; }
        /// <summary>
        /// 归属人员类型:企业、角色、岗位、组
        /// </summary>
        public EnumPrivilegeMaster PrivilegeMaster { get; set; }
    }
}
