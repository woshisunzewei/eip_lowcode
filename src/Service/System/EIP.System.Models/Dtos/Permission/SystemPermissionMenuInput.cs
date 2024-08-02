using System;

namespace EIP.System.Models.Dtos.Permission
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemPermissionMenuInput
    {
        /// <summary>
        /// 当前用户
        /// </summary>
        public Guid? UserId { get; set; } = null;

        /// <summary>
        /// 配置Id
        /// </summary>
        public Guid? ConfigId { get; set; } = null;

        /// <summary>
        /// 是否显示移动端
        /// </summary>
        public bool IsShowMobile { get; set; } = false;
    }
}