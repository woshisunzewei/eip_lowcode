using EIP.System.Models.Dtos.Post;

namespace EIP.System.Repository
{
    /// <summary>
    /// 岗位
    /// </summary>
    public interface ISystemPostRepository 
    {
        /// <summary>
        /// 查询归属某组织下的岗位信息
        /// </summary>
        /// <param name="input">组织机构PostId</param>
        /// <returns>岗位信息</returns>
        Task<PagedResults<SystemPostFindOutput>> FindPostByOrganizationId(SystemPostFindInput input);
    }
}