namespace EIP.System.Models.Dtos.User
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemUserFindTopOrgInput
    {
        /// <summary>
        /// 
        /// </summary>
        public string TopOrg { get; set; }

        /// <summary>
        /// 特定组织ID,多个逗号分割:某些情况需要找特定组织
        /// </summary>
        public string SpecificOrg { get; set; }
    }
}
