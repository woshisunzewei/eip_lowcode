/**************************************************************
* Copyright (C) 2022 www.eipflow.com ����ΰ��Ȩ����(����ؾ�)
*
* ����: ����ΰ(QQ 1039318332)
* ����ʱ��: 2022/01/12 22:40:15
* �ļ���: 
* ����: 
* 
* �޸���ʷ
* �޸��ˣ�
* ʱ�䣺
* �޸�˵����
*
**************************************************************/
using EIP.System.Models.Dtos.Menu;
using EIP.System.Models.Dtos.MenuButton;

namespace EIP.System.Repository.Impl
{
    /// <summary>
    /// Ȩ��
    /// </summary>
    public class SystemPermissionRepository : ISystemPermissionRepository
    {
        /// <summary>
        /// ����Ȩ�޹���Id��ѯģ��Ȩ����Ϣ
        /// </summary>
        /// <param name="input">Ȩ������:ģ�顢��������ݡ��ֶΡ��ļ�</param>
        /// <returns></returns>
        public Task<IEnumerable<SystemPermission>> FindPermissionByPrivilegeMasterValue(
            SystemPermissionByPrivilegeMasterValueInput input)
        {
            var sql = new StringBuilder($"SELECT PrivilegeAccessValue,PrivilegeMenuId FROM System_Permission WHERE PrivilegeAccess=@privilegeAccess");
            sql.Append(input.PrivilegeMaster == EnumPrivilegeMaster.��Ա
                    ? $" AND PrivilegeMasterValue IN (SELECT PrivilegeMasterValue FROM System_PermissionUser WHERE PrivilegeMasterUserId=@privilegeMasterValue)"
                    : $" AND PrivilegeMasterValue=@privilegeMasterValue");
            if (!input.PrivilegeMenuId.IsNullOrEmptyGuid())
            {
                sql.Append($" AND PrivilegeMenuId=@privilegeMenuId");
            }
            sql.Append(" GROUP BY PrivilegeAccessValue,PrivilegeMenuId");
            return new SqlMapperUtil().SqlWithParams<SystemPermission>(sql.ToString(),
                new
                {
                    privilegeAccess = (byte)input.PrivilegeAccess,
                    privilegeMasterValue = input.PrivilegeMasterValue,
                    privilegeMenuId = input.PrivilegeMenuId
                });
        }

        /// <summary>
        /// �����û�Id��ȡ�û����е�ģ��Ȩ��
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<SystemPermissionFindMenuByUserIdOutput>> FindSystemPermissionMenuByAdmin(SystemPermissionMenuInput input)
        {
            var where = "";
            if (input.IsShowMobile){
                where += " and menu.IsShowMobile=1  ";
            }
            var sql = new StringBuilder(
                $@"SELECT  menu.Image,menu.MenuId Id,menu.ParentId ,menu.Name text,menu.Icon,menu.Theme,menu.Path ,menu.OpenType,menu.Router,menu.Params FROM System_Menu menu WHERE menu.IsShowMenu=1 and menu.IsFreeze=0 {where} ORDER BY menu.OrderNo");
            return new SqlMapperUtil().SqlWithParams<SystemPermissionFindMenuByUserIdOutput>(sql.ToString());
        }

        /// <summary>
        /// �����û�Id��ȡ�û����е�ģ��Ȩ��
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<IEnumerable<SystemPermissionFindMenuByUserIdOutput>> FindSystemPermissionMenuByUserId(SystemPermissionMenuInput input)
        {
            var where = "";
            if (input.IsShowMobile)
            {
                where += " and menu.IsShowMobile=1  ";
            }
            var sql = new StringBuilder(
                    $@"SELECT menu.Image,menu.MenuId id,menu.ParentId ,menu.name text,menu.icon,menu.Theme,menu.Path ,menu.OpenType ,menu.Router,menu.Params FROM System_Menu menu
                                                    LEFT JOIN System_Permission per ON per.PrivilegeAccessValue=menu.MenuId
                                                    LEFT JOIN System_PermissionUser perUser ON per.PrivilegeMasterValue=perUser.PrivilegeMasterValue                                                   
                                                    WHERE per.PrivilegeAccess=@privilegeAccess and menu.IsShowMenu=1 and menu.IsFreeze=0 and perUser.PrivilegeMasterUserId=@userId {where}
                                                  ");
            sql.Append(@"  GROUP BY  menu.Image,menu.MenuId,menu.ParentId,menu.name,menu.icon,menu.Theme,menu.Path,menu.OrderNo,menu.remark,menu.OpenType,menu.Router,menu.Params
            ORDER BY menu.OrderNo");
            return new SqlMapperUtil().SqlWithParams<SystemPermissionFindMenuByUserIdOutput>(sql.ToString(),
                new { privilegeAccess = (byte)EnumPrivilegeAccess.ģ��Ȩ��, userId = input.UserId });
        }

        /// <summary>
        /// �����û�Id��ȡ�û����е�ģ��Ȩ��
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<IEnumerable<SystemMenuFindAgileOutput>> FindSystemPermissionMenuAgileByUserId(SystemPermissionMenuInput input)
        {
            var sql = new StringBuilder(
                    @"select ConfigId,PublicJson,ColumnJson,ConfigType,Name,Remark,MenuId from System_Agile where MenuId in ( SELECT menu.MenuId FROM System_Menu menu
                                                    LEFT JOIN System_Permission per ON per.PrivilegeAccessValue=menu.MenuId
                                                    LEFT JOIN System_PermissionUser perUser ON per.PrivilegeMasterValue=perUser.PrivilegeMasterValue                                                   
                                                    WHERE  menu.IsShowMenu=1 and menu.IsFreeze=0  and IsAgileMenu=true and path!='' and path is not null  ");
            if (input.ConfigId.HasValue)
            {
                sql.Append($" and ConfigId=@configId ");
            }
            else
            {
                sql.Append($"  and perUser.PrivilegeMasterUserId=@userId and per.PrivilegeAccess=@privilegeAccess  ");
            }
            sql.Append(" GROUP BY menu.MenuId ) ");
            return new SqlMapperUtil().SqlWithParams<SystemMenuFindAgileOutput>(sql.ToString(),
                new { privilegeAccess = (byte)EnumPrivilegeAccess.ģ��Ȩ��, userId = input.UserId, configId = input.ConfigId });
        }
        /// <summary>
        /// ����ģ��Id���û�Id��ȡ��ťȨ������
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public Task<IEnumerable<SystemMenuButtonByViewRote>> FindMenuButton(Guid userId,string menuId)
        {
            string sql = $@"SELECT Script FROM System_MenuButton mb
                        LEFT JOIN System_Permission per on MenuButtonId=per.PrivilegeAccessValue
                        LEFT JOIN System_PermissionUser perUser on per.PrivilegeMasterValue=perUser.PrivilegeMasterValue
                        LEFT JOIN System_Menu menu on menu.MenuId=mb.MenuId  
                        WHERE mb.MenuId in({menuId}) and PrivilegeMasterUserId=@userId and per.PrivilegeAccess=1 and mb.IsFreeze=0
                        GROUP BY mb.Name,Script,mb.Icon,mb.Theme,mb.OrderNo,Method,mb.Style,mb.Api,mb.TriggerType
                        ORDER BY mb.OrderNo";
            return new SqlMapperUtil().SqlWithParams<SystemMenuButtonByViewRote>(sql,
                new { userId });
        }
        /// <summary>
        /// �����û�Id��ȡ�û����е�ģ��Ȩ��
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<IEnumerable<BaseTree>> FindSystemPermissionMenuAllByUserId(Guid userId)
        {
            var sql = new StringBuilder(
                    $@"SELECT menu.MenuId id,menu.ParentId parent,menu.name text,menu.icon,menu.Theme FROM System_Menu menu
                                                    LEFT JOIN System_Permission per ON per.PrivilegeAccessValue=menu.MenuId
                                                    LEFT JOIN System_PermissionUser perUser ON per.PrivilegeMasterValue=perUser.PrivilegeMasterValue                                                   
                                                    WHERE per.PrivilegeAccess=@privilegeAccess and menu.HaveMenuPermission=1 and menu.IsFreeze=@isFreeze and perUser.PrivilegeMasterUserId=@userId 
                                                    GROUP BY menu.MenuId,menu.ParentId,menu.name,menu.icon,menu.Theme,menu.Path,menu.OrderNo,menu.remark,menu.OpenType
                                                    ORDER BY menu.OrderNo");
            return new SqlMapperUtil().SqlWithParams<BaseTree>(sql.ToString(),
                new { privilegeAccess = (byte)EnumPrivilegeAccess.ģ��Ȩ��, isFreeze = false, userId });
        }
        /// <summary>
        /// �����û�Id��ȡ�û����е�ģ��Ȩ��
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<IEnumerable<BaseTree>> FindSystemPermissionMobileMenuAllByUserId(Guid userId)
        {
            var sql = new StringBuilder(
                    $@"SELECT menu.MobileMenuId id,menu.ParentId parent,menu.name text,menu.icon FROM System_MobileMenu menu
                                                    LEFT JOIN System_Permission per ON per.PrivilegeAccessValue=menu.MobileMenuId
                                                    LEFT JOIN System_PermissionUser perUser ON per.PrivilegeMasterValue=perUser.PrivilegeMasterValue                                                   
                                                    WHERE per.PrivilegeAccess=@privilegeAccess and menu.HaveMenuPermission=1 and menu.IsFreeze=@isFreeze and perUser.PrivilegeMasterUserId=@userId 
                                                    GROUP BY menu.MobileMenuId,menu.ParentId,menu.name,menu.icon,menu.Path,menu.OrderNo,menu.remark,menu.OpenType
                                                    ORDER BY menu.OrderNo");
            return new SqlMapperUtil().SqlWithParams<BaseTree>(sql.ToString(),
                new { privilegeAccess = (byte)EnumPrivilegeAccess.�ƶ���ģ��, isFreeze = false, userId });
        }

        /// <summary>
        /// ���ݽ�ɫId,��λId,��Id,��ԱId��ȡ���е�ģ����Ϣ
        /// </summary>
        /// <param name="input">�������</param>
        /// <returns>����ģ����Ϣ</returns>
        /// GetMenuPermissionByPrivilegeMasterValue
        public Task<IEnumerable<BaseTree>> FindMenuHavePermissionByPrivilegeMasterValue(SystemPermissiontMenuHaveByPrivilegeMasterValueInput input)
        {
            var sql =
                new StringBuilder($"SELECT MenuId id,ParentId parent,name text,icon,Theme FROM System_Menu menu WHERE MenuId IN( SELECT PrivilegeAccessValue  FROM System_Permission WHERE PrivilegeAccess=@privilegeAccess AND ");
            sql.Append(
                input.PrivilegeMaster == EnumPrivilegeMaster.��Ա
                    ? $" PrivilegeMasterValue IN (SELECT PrivilegeMasterValue FROM System_PermissionUser WHERE PrivilegeMasterUserId=@privilegeMasterValue) "
                    : $" PrivilegeMasterValue=@privilegeMasterValue  ");
            sql.Append(" GROUP BY PrivilegeAccessValue) AND menu.IsFreeze=0");
            if (input.PrivilegeAccess != null)
            {
                switch (input.PrivilegeAccess)
                {
                    case EnumPrivilegeAccess.ģ�鰴ť:
                        sql.Append(" AND menu.HaveButtonPermission=1");
                        break;
                    case EnumPrivilegeAccess.����Ȩ��:
                        sql.Append(" AND menu.HaveDataPermission=1");
                        break;
                    case EnumPrivilegeAccess.�ֶ�Ȩ��:
                        sql.Append(" AND menu.HaveFieldPermission=1");
                        break;
                    case EnumPrivilegeAccess.ģ��Ȩ��:
                        sql.Append(" AND menu.HaveMenuPermission=1");
                        break;
                }
            }
            sql.Append("  ORDER BY OrderNo");
            return new SqlMapperUtil().SqlWithParams<BaseTree>(sql.ToString(),
                new
                {
                    privilegeAccess = EnumPrivilegeAccess.ģ��Ȩ��,
                    privilegeMasterValue = input.PrivilegeMasterValue
                });
        }
        /// <summary>
        /// ���ݽ�ɫId,��λId,��Id,��ԱId��ȡ���е�ģ����Ϣ
        /// </summary>
        /// <param name="input">�������</param>
        /// <returns>����ģ����Ϣ</returns>
        /// GetMenuPermissionByPrivilegeMasterValue
        public Task<IEnumerable<BaseTree>> FindMobileMenuHavePermissionByPrivilegeMasterValue(SystemPermissiontMenuHaveByPrivilegeMasterValueInput input)
        {
            var sql =
                new StringBuilder($"SELECT MobileMenuId id,ParentId parent,name text,icon FROM System_MobileMenu menu WHERE MobileMenuId IN( SELECT PrivilegeAccessValue  FROM System_Permission WHERE PrivilegeAccess=@privilegeAccess AND ");
            sql.Append(
                input.PrivilegeMaster == EnumPrivilegeMaster.��Ա
                    ? $" PrivilegeMasterValue IN (SELECT PrivilegeMasterValue FROM System_PermissionUser WHERE PrivilegeMasterUserId=@privilegeMasterValue) "
                    : $" PrivilegeMasterValue=@privilegeMasterValue  ");
            sql.Append(" GROUP BY PrivilegeAccessValue) AND menu.IsFreeze=0");
            if (input.PrivilegeAccess != null)
            {
                switch (input.PrivilegeAccess)
                {
                    case EnumPrivilegeAccess.ģ�鰴ť:
                        sql.Append(" AND menu.HaveButtonPermission=1");
                        break;
                    case EnumPrivilegeAccess.����Ȩ��:
                        sql.Append(" AND menu.HaveDataPermission=1");
                        break;
                    case EnumPrivilegeAccess.�ֶ�Ȩ��:
                        sql.Append(" AND menu.HaveFieldPermission=1");
                        break;
                    case EnumPrivilegeAccess.ģ��Ȩ��:
                        sql.Append(" AND menu.HaveMenuPermission=1");
                        break;
                }
            }
            sql.Append("  ORDER BY OrderNo");
            return new SqlMapperUtil().SqlWithParams<BaseTree>(sql.ToString(),
                new
                {
                    privilegeAccess = EnumPrivilegeAccess.�ƶ���ģ��,
                    privilegeMasterValue = input.PrivilegeMasterValue
                });
        }
        /// <summary>
        /// ��ȡ���û�ӵ�е�����Ȩ��
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<IEnumerable<SystemData>> FindDataPermission(ViewRote input)
        {
            string sql = $@"SELECT data.Name, RuleHtml,RuleJson,data.OrderNo FROM System_Data data
                           LEFT JOIN System_Permission per on DataId = per.PrivilegeAccessValue
                           INNER JOIN System_PermissionUser perUser on per.PrivilegeMasterValue = perUser.PrivilegeMasterValue and PrivilegeMasterUserId=@UserId and per.PrivilegeAccess = 3
                           LEFT JOIN System_Menu menu on menu.MenuId = data.MenuId
                           WHERE menu.MenuId=@MenuId
                           GROUP BY data.Name, RuleHtml,RuleJson,data.OrderNo
                           ORDER BY data.OrderNo";
            return new SqlMapperUtil().SqlWithParams<SystemData>(sql, new
            {
                input.UserId,
                input.MenuId
            });
        }
    }
}