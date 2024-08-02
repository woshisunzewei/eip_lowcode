using EIP.System.Models.Dtos.Group;

namespace EIP.System.Repository
{
    /// <summary>
    /// ����
    /// </summary>
    public interface ISystemGroupRepository 
    {
        /// <summary>
        /// ��ѯ����ĳ��֯�µ�����Ϣ
        /// </summary>
        /// <param name="input">��֯����PostId</param>
        /// <returns>����Ϣ</returns>
        Task<PagedResults<SystemGroupFindOutput>> FindGroupByOrganizationId(SystemGroupFindInput input);

    }
}