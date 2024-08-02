using EIP.Common.Models.Paging;

namespace EIP.System.Models.Dtos.OrganizationThree
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemOrganizationThreePagingInput : QueryParam
    {
        /// <summary>
        /// 类型
        /// </summary>
        public int Type { get; set; }
    }
}
