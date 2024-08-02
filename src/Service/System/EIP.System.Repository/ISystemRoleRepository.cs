using EIP.System.Models.Dtos.Role;

namespace EIP.System.Repository
{
    /// <summary>
    /// 角色
    /// </summary>
    public interface ISystemRoleRepository 
    {
        /// <summary>
        /// 根据组织机构获取角色信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResults<SystemRolesFindOutput>> FindPagingRole(SystemRolesFindInput input);
    }
}