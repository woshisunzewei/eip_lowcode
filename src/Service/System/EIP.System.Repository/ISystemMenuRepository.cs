using EIP.System.Models.Dtos.Menu;

namespace EIP.System.Repository
{
    /// <summary>
    /// 模块
    /// </summary>
    public interface ISystemMenuRepository
    {
        /// <summary>
        /// 根据父级获取下面模块
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IEnumerable<SystemMenuFindOutput>> Find(SystemMenuFindInput input);

        /// <summary>
        /// 获取权限树菜单
        /// </summary>
        /// <param name="privilegeAccess"></param>
        /// <returns></returns>
        Task<IEnumerable<BaseTree>> FindPermissionMenu(EnumPrivilegeAccess privilegeAccess);
    }
}