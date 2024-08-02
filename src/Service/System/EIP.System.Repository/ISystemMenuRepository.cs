using EIP.System.Models.Dtos.Menu;

namespace EIP.System.Repository
{
    /// <summary>
    /// ģ��
    /// </summary>
    public interface ISystemMenuRepository
    {
        /// <summary>
        /// ���ݸ�����ȡ����ģ��
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IEnumerable<SystemMenuFindOutput>> Find(SystemMenuFindInput input);

        /// <summary>
        /// ��ȡȨ�����˵�
        /// </summary>
        /// <param name="privilegeAccess"></param>
        /// <returns></returns>
        Task<IEnumerable<BaseTree>> FindPermissionMenu(EnumPrivilegeAccess privilegeAccess);
    }
}