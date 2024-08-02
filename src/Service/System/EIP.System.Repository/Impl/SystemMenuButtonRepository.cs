/**************************************************************
* Copyright (C) 2022 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2022/01/12 22:40:15
* 文件名: 
* 描述: 
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.System.Models.Dtos.MenuButton;

namespace EIP.System.Repository.Impl
{
    /// <summary>
    /// 模块按钮
    /// </summary>
    public class SystemMenuButtonRepository : ISystemMenuButtonRepository
    {
        /// <summary>
        /// 根据模块获取功能项信息
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
        /// 根据模块获取功能项信息
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
        /// 根据模块获取功能项信息
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
        /// 根据模块Id和用户Id获取按钮权限数据
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