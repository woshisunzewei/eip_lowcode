using System;

namespace EIP.System.Models.Dtos.UserThree
{
    /// <summary>
    /// 转移到系统
    /// </summary>
    public class SystemUserThreeToSystemInput
    {
        /// <summary>
        /// 用户Ids
        /// </summary>
        public string UserIds { get; set; }

        /// <summary>
        /// 组织架构
        /// </summary>
        public Guid OrganizationId { get; set; }
    }
}
