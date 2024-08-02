using EIP.System.Models.Dtos.Menu;

namespace EIP.System.Repository.Impl
{
    /// <summary>
    /// ģ��
    /// </summary>
    public class SystemMenuRepository : ISystemMenuRepository
    {
        /// <summary>
        /// ����
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<IEnumerable<SystemMenuFindOutput>> Find(SystemMenuFindInput input)
        {
            var sql = new StringBuilder();
            sql.Append(" select menu.ParentId, menu.MenuId,menu.Name,menu.Image,menu.Icon,menu.Theme,menu.OpenType,menu.Path,menu.CanbeDelete,menu.Remark,menu.OrderNo,menu.HaveMenuPermission,menu.HaveDataPermission,menu.HaveFieldPermission ,menu.HaveButtonPermission,menu.IsFreeze,menu.IsShowMenu,menu.IsAgileMenu,menu.AgileMenuType,menu.IframePath from system_menu menu  where 1=1 ");
            sql.Append(input.Sql);
            return new SqlMapperUtil().SqlWithParams<SystemMenuFindOutput>(sql.ToString());
        }

        /// <summary>
        /// ��ȡ���νṹ�˵�
        /// </summary>
        /// <param name="privilegeAccess"></param>
        /// <returns></returns>
        public Task<IEnumerable<BaseTree>> FindPermissionMenu(EnumPrivilegeAccess privilegeAccess)
        {
            StringBuilder sql = new StringBuilder();
            sql.Append(
                "select MenuId id,ParentId parent,Name text,icon,theme from System_Menu where IsFreeze=0");
            switch (privilegeAccess)
            {
                case EnumPrivilegeAccess.ģ��Ȩ��:
                    sql.Append(" and HaveMenuPermission=1 ");
                    break;
                case EnumPrivilegeAccess.ģ�鰴ť:
                    sql.Append(" and HaveButtonPermission=1 ");
                    break;
                case EnumPrivilegeAccess.����Ȩ��:
                    sql.Append(" and HaveDataPermission=1 ");
                    break;
                case EnumPrivilegeAccess.�ֶ�Ȩ��:
                    sql.Append(" and HaveFieldPermission=1 ");
                    break;
            }
            sql.Append(" order by OrderNo ");
            return new SqlMapperUtil().SqlWithParams<BaseTree>(sql.ToString());
        }
    }
}