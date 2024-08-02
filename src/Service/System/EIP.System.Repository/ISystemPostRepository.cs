using EIP.System.Models.Dtos.Post;

namespace EIP.System.Repository
{
    /// <summary>
    /// ��λ
    /// </summary>
    public interface ISystemPostRepository 
    {
        /// <summary>
        /// ��ѯ����ĳ��֯�µĸ�λ��Ϣ
        /// </summary>
        /// <param name="input">��֯����PostId</param>
        /// <returns>��λ��Ϣ</returns>
        Task<PagedResults<SystemPostFindOutput>> FindPostByOrganizationId(SystemPostFindInput input);
    }
}