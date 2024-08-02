using EIP.System.Models.Dtos.Menu;

namespace EIP.System.Repository.Impl
{
    /// <summary>
    /// 模块
    /// </summary>
    public class SystemMenuRepository : ISystemMenuRepository
    {
        /// <summary>
        /// 根据
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
        /// 获取树形结构菜单
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
                case EnumPrivilegeAccess.模块权限:
                    sql.Append(" and HaveMenuPermission=1 ");
                    break;
                case EnumPrivilegeAccess.模块按钮:
                    sql.Append(" and HaveButtonPermission=1 ");
                    break;
                case EnumPrivilegeAccess.数据权限:
                    sql.Append(" and HaveDataPermission=1 ");
                    break;
                case EnumPrivilegeAccess.字段权限:
                    sql.Append(" and HaveFieldPermission=1 ");
                    break;
            }
            sql.Append(" order by OrderNo ");
            return new SqlMapperUtil().SqlWithParams<BaseTree>(sql.ToString());
        }
    }
}