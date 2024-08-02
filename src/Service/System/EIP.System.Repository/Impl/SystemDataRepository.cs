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
using EIP.System.Models.Dtos.Data;

namespace EIP.System.Repository.Impl
{
    /// <summary>
    /// 数据权限
    /// </summary>
    public class SystemDataRepository :  ISystemDataRepository
    {
        /// <summary>
        /// 根据模块id获取数据权限定义
        /// </summary>
        /// <param name="input">模块id</param>
        /// <returns></returns>
        public Task<PagedResults<SystemDataFindOutput>> Find(SystemDataFindInput input)
        {
            var sql = new StringBuilder(
                "SELECT data.CreateTime,data.CreateUserName,data.UpdateTime,data.UpdateUserName, data.DataId,data.MenuName,data.Name,data.IsFreeze,data.OrderNo,data.Remark,menu.ParentIdsName MenuNames,@rowNumber, @recordCount  FROM System_Data data LEFT JOIN System_Menu menu ON data.MenuId=menu.MenuId @where");
            if (input.Id.HasValue)
            {
                sql.Append($" AND data.MenuId='{input.Id}'");
            }
            if (input.Sidx.IsNullOrEmpty())
            {
                input.Sidx = " data.OrderNo";
            }
            return new SqlMapperUtil().PagingQuerySqlAsync<SystemDataFindOutput>(sql.ToString(), input);
        }

        /// <summary>
        /// 根据模块获取数据权限信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<IEnumerable<SystemDataOutput>> FindHaveDataPermission(IdInput input)
        {
            var sql = new StringBuilder();
            sql.Append($@"SELECT data.*,menu.ParentIdsName MenuNames FROM System_Data data
            LEFT JOIN System_Permission per on data.DataId = per.PrivilegeAccessValue
            INNER JOIN System_PermissionUser perUser on per.PrivilegeMasterValue = perUser.PrivilegeMasterValue and PrivilegeMasterUserId=@userId and per.PrivilegeAccess = 3
            LEFT JOIN System_Menu menu on menu.MenuId = data.MenuId
            ORDER BY data.OrderNo ");
            return new SqlMapperUtil().SqlWithParams<SystemDataOutput>(sql.ToString(), new
            {
                userId = input.Id
            });
        }

        /// <summary>
        /// 根据模块id获取数据权限定义
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<SystemDataOutput>> FindDataByMenuId(IList<Guid> menuId = null)
        {
            var sql = new StringBuilder(
                "SELECT data.*,menu.ParentIdsName MenuNames FROM System_Data data LEFT JOIN System_Menu menu ON data.MenuId=menu.MenuId ");
            if (menuId != null && menuId.Any())
            {
                sql.Append($" WHERE menu.MenuId in ({menuId.ExpandAndToString().InSql()})");
            }
            sql.Append(" ORDER BY data.OrderNo");
            return new SqlMapperUtil().SqlWithParams<SystemDataOutput>(sql.ToString());
        }
    }
}