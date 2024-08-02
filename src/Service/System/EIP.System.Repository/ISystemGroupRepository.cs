using EIP.System.Models.Dtos.Group;

namespace EIP.System.Repository
{
    /// <summary>
    /// 分组
    /// </summary>
    public interface ISystemGroupRepository 
    {
        /// <summary>
        /// 查询归属某组织下的组信息
        /// </summary>
        /// <param name="input">组织机构PostId</param>
        /// <returns>组信息</returns>
        Task<PagedResults<SystemGroupFindOutput>> FindGroupByOrganizationId(SystemGroupFindInput input);

    }
}