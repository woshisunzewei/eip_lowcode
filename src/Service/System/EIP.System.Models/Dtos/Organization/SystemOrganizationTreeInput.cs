using System;

namespace EIP.System.Models.Dtos.Organization
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemOrganizationTreeInput
    {
        /// <summary>
        /// 顶部组织
        /// </summary>
        public Guid? TopOrg { get; set; }

        /// <summary>
        /// 特定组织ID,多个逗号分割:某些情况需要找特定组织
        /// </summary>
        public string SpecificOrg { get; set; }
    }
}
