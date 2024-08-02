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
using EIP.System.Models.Dtos.Data;

namespace EIP.System.Repository.Impl
{
    /// <summary>
    /// ����Ȩ��
    /// </summary>
    public class SystemDataRepository :  ISystemDataRepository
    {
        /// <summary>
        /// ����ģ��id��ȡ����Ȩ�޶���
        /// </summary>
        /// <param name="input">ģ��id</param>
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
        /// ����ģ���ȡ����Ȩ����Ϣ
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
        /// ����ģ��id��ȡ����Ȩ�޶���
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