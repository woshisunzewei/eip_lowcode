using EIP.System.Models.Dtos.Role;

namespace EIP.System.Repository
{
    /// <summary>
    /// ��ɫ
    /// </summary>
    public interface ISystemRoleRepository 
    {
        /// <summary>
        /// ������֯������ȡ��ɫ��Ϣ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResults<SystemRolesFindOutput>> FindPagingRole(SystemRolesFindInput input);
    }
}