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
using EIP.System.Models.Dtos.MenuButton;

namespace EIP.System.Repository.Impl
{
    /// <summary>
    /// ģ�鰴ť
    /// </summary>
    public class SystemMenuButtonRepository : ISystemMenuButtonRepository
    {
        /// <summary>
        /// ����ģ���ȡ��������Ϣ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResults<SystemMenuButtonFindOutput>> Find(SystemMenuButtonFindInput input)
        {
            var sql = new StringBuilder();
            sql.Append("SELECT menuButton.Shape,menuButton.TriggerType,menuButton.Api,menuButton.Automation,menuButton.Prints,menuButton.Export,menuButton.CreateTime,menuButton.CreateUserName,menuButton.UpdateTime,menuButton.UpdateUserName, menuButton.Style,menuButton.Script,menuButton.MenuButtonId,menuButton.MenuName,menuButton.Icon,menuButton.Theme,menuButton.Name,menuButton.Method,menuButton.OrderNo,menuButton.IsFreeze,menuButton.IsShowTable,menuButton.Remark,menu.ParentIdsName MenuNames,@rowNumber, @recordCount  FROM System_MenuButton menuButton LEFT JOIN System_Menu menu ON menu.MenuId=menuButton.MenuId @where ");

            if (input.Id.HasValue)
            {
                sql.Append($" AND menuButton.MenuId='{input.Id}'");
            }
            if (input.Sidx.IsNullOrEmpty())
            {
                input.Sidx = " menuButton.OrderNo";
            }
            return new SqlMapperUtil().PagingQuerySqlAsync<SystemMenuButtonFindOutput>(sql.ToString(), input);
        }

        /// <summary>
        /// ����ģ���ȡ��������Ϣ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<IEnumerable<SystemMenuButtonOutput>> FindHaveMenuButtonPermission(IdInput input)
        {
            var sql = new StringBuilder();
            sql.Append($@"SELECT button.*,menu.ParentIdsName MenuNames FROM system_menubutton button
            LEFT JOIN System_Permission per on MenuButtonId = per.PrivilegeAccessValue
            INNER JOIN System_PermissionUser perUser on per.PrivilegeMasterValue = perUser.PrivilegeMasterValue and PrivilegeMasterUserId=@userId and per.PrivilegeAccess = 1
            LEFT JOIN System_Menu menu on menu.MenuId = button.MenuId
            ORDER BY button.OrderNo ");
            return new SqlMapperUtil().SqlWithParams<SystemMenuButtonOutput>(sql.ToString(), new
            {
                userId = input.Id
            });
        }

        /// <summary>
        /// ����ģ���ȡ��������Ϣ
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        public Task<IEnumerable<SystemMenuButtonOutput>> FindMenuButtonByMenuId(IList<Guid> menuId = null)
        {
            var sql = new StringBuilder();
            sql.Append("SELECT menuButton.*,menu.ParentIdsName MenuNames FROM System_MenuButton menuButton LEFT JOIN System_Menu menu ON menu.MenuId=menuButton.MenuId  ");
            if (menuId != null && menuId.Any())
            {
                sql.Append($" WHERE menu.MenuId in ({menuId.ExpandAndToString().InSql()})");
            }
            return new SqlMapperUtil().SqlWithParams<SystemMenuButtonOutput>(sql.ToString());
        }

        /// <summary>
        /// ����ģ��Id���û�Id��ȡ��ťȨ������
        /// </summary>
        /// <param name="viewRote"></param>
        /// <returns></returns>
        public Task<IEnumerable<SystemMenuButtonByViewRote>> FindMenuButton(ViewRote viewRote)
        {
             string sql = $@"
                        SELECT mb.Remark,mb.Shape,mb.isShowTable,mb.Automation,mb.Prints,mb.Export,mb.MenuButtonId,mb.Name,Script,Method,mb.Icon,mb.Theme,mb.OrderNo,mb.Style,mb.Api,mb.TriggerType FROM System_MenuButton mb
                        LEFT JOIN System_Permission per on MenuButtonId=per.PrivilegeAccessValue
                        LEFT JOIN System_PermissionUser perUser on per.PrivilegeMasterValue=perUser.PrivilegeMasterValue
                        LEFT JOIN System_Menu menu on menu.MenuId=mb.MenuId  
                        WHERE mb.MenuId=@MenuId and PrivilegeMasterUserId=@userId and per.PrivilegeAccess=1 and mb.IsFreeze=0
                        GROUP BY mb.Prints,mb.Export,mb.Remark,mb.Shape,mb.isShowTable,mb.Automation,mb.Name,Script,mb.Icon,mb.Theme,mb.OrderNo,Method,mb.Style,mb.Api,mb.TriggerType
                        ORDER BY mb.OrderNo";
            return new SqlMapperUtil().SqlWithParams<SystemMenuButtonByViewRote>(sql,
                new { viewRote.UserId, viewRote.MenuId });
        }
    }
}