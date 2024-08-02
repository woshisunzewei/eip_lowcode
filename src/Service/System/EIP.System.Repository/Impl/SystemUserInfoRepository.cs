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

namespace EIP.System.Repository.Impl
{
    /// <summary>
    /// 系统人员管理
    /// </summary>
    public class SystemUserInfoRepository : ISystemUserInfoRepository
    {
        /// <summary>
        /// 复杂查询分页方式
        /// </summary>
        /// <param name="input">查询参数</param>
        /// <returns>分页</returns>
        public Task<PagedResults<SystemUserOutput>> Find(SystemUserPagingInput input)
        {
            var sql = new StringBuilder(
                 "SELECT userInfo.UserId," +
                 "userInfo.OrganizationId," +
                 "userInfo.Code," +
                 "userInfo.Name," +
                 "userInfo.Mobile," +
                 "userInfo.HeadImage," +
                 "userInfo.OtherContactInformation," +
                 "userInfo.Email," +
                 "userInfo.LastVisitTime," +
                 "userInfo.IsFreeze," +
                 "userInfo.Remark," +
                 "userInfo.Nature," +

                 "userInfo.CreateTime," +
                 "userInfo.CreateUserName," +
                 "userInfo.UpdateTime," +
                 "userInfo.UpdateUserName," +

                 "userInfo.OrganizationName," +
                 "userInfo.UserOrganizationNames," +
                 "org.ParentIdsName,org.ParentIds,@rowNumber, @recordCount  " +
                 " FROM System_UserInfo userInfo LEFT JOIN System_PermissionUser perUser ON perUser.PrivilegeMasterUserId = userInfo.UserId " +
                 "LEFT JOIN System_Organization org on org.OrganizationId = userInfo.OrganizationId @where AND userInfo.IsAdmin=0 ");
            if (input.DataSql.IsNotNullOrEmpty())
            {
                sql.Append(@" AND " + input.DataSql + "");
            }
            sql.Append($@" AND perUser.PrivilegeMaster='{input.PrivilegeMaster.GetHashCode()}' and perUser.IsRelationOrganization=0  ");

            if (!input.PrivilegeMasterValue.IsNullOrEmptyGuid())
            {
                sql.Append($@" AND perUser.PrivilegeMasterValue in (select OrganizationId from System_Organization where ParentIds like '%{input.PrivilegeMasterValue}%') ");
            }
            if (input.TopOrg.IsNotNullOrEmpty())
            {
                sql.Append("AND org.ParentIds  like '%" + input.TopOrg.Xss().FilterSql() + "%'");
            }
            if(input.SpecificOrg.IsNotNullOrEmpty())
            {
                List<string> list = new List<string>();
                foreach (var item in input.SpecificOrg.Split(','))
                {
                    list.Add($"  userInfo.OrganizationId in (select OrganizationId from system_organization where ParentIds like '%{item}%') ");
                }
                sql.Append($" AND ({list.ExpandAndToString(" or ")}) ");
            }
            if (input.Sidx.IsNullOrEmpty())
            {
                input.Sidx = " userInfo.OrganizationId ";
            }
            sql.Append(" GROUP BY userInfo.UpdateUserName,userInfo.UpdateTime,userInfo.CreateUserName,userInfo.CreateTime,userInfo.OrganizationName,userInfo.UserOrganizationNames,userInfo.HeadImage,userInfo.UserId,userInfo.OrganizationId,userInfo.Code,userInfo.Name,userInfo.Mobile,userInfo.OtherContactInformation,userInfo.Email,userInfo.LastVisitTime,userInfo.IsFreeze,userInfo.Remark,userInfo.Nature,org.ParentIdsName,org.ParentIds");
            return new SqlMapperUtil().PagingQuerySqlAsync<SystemUserOutput>(sql.ToString(), input);
        }
        /// <summary>
        /// 复杂查询分页方式
        /// </summary>
        /// <param name="input">查询参数</param>
        /// <returns>分页</returns>
        public Task<IEnumerable<SystemUserFindCommonOutput>> FindCommon(SystemUserFindCommonInput input)
        {
            var sql = new StringBuilder(
                 "SELECT userInfo.UserId," +
                 "userInfo.OrganizationId," +
                 "userInfo.Name," +
                 "userInfo.HeadImage," +
                 "userInfo.Sex," +
                 "userInfo.Code," +
                 "userInfo.OrganizationName," +
                 "org.ParentIdsName  " +
                 " FROM System_UserInfo userInfo" +
                 " LEFT JOIN System_PermissionUser perUser ON perUser.PrivilegeMasterUserId = userInfo.UserId " +
                 " LEFT JOIN System_Organization org on org.OrganizationId = userInfo.OrganizationId where  userInfo.IsAdmin=0 ");
            if (input.DataSql.IsNotNullOrEmpty())
            {
                sql.Append(@" AND " + input.DataSql + "");
            }

            if (input.PrivilegeMaster.HasValue)
            {
                sql.Append($@" AND perUser.PrivilegeMaster='{input.PrivilegeMaster.GetHashCode()}' ");
            }

            if (!input.PrivilegeMasterValue.IsNullOrEmptyGuid())
            {
                sql.Append($@" AND perUser.PrivilegeMasterValue in (select OrganizationId from System_Organization where ParentIds like '%{input.PrivilegeMasterValue}%') ");
            }

            sql.Append(" GROUP BY userInfo.HeadImage,userInfo.OrganizationName,userInfo.Code,userInfo.Sex,userInfo.UserId,userInfo.OrganizationId,userInfo.Name,org.ParentIdsName");
            return new SqlMapperUtil().SqlWithParams<SystemUserFindCommonOutput>(sql.ToString());
        }
        /// <summary>
        /// 复杂查询分页方式
        /// </summary>
        /// <param name="dataSql">查询参数</param>
        /// <returns>分页</returns>
        public Task<IEnumerable<SystemUserChosenOutput>> FindAllUser(string dataSql)
        {
            var sql = new StringBuilder(
                 "SELECT userInfo.UserId," +
                 "userInfo.OrganizationId," +
                 "userInfo.Code," +
                 "userInfo.Name," +
                 "org.ParentIdsName OrganizationName FROM System_UserInfo userInfo LEFT JOIN System_Organization org on org.OrganizationId = userInfo.OrganizationId where 1=1 ");
            if (dataSql.IsNotNullOrEmpty())
            {
                sql.Append(@" AND " + dataSql + "");
            }
            sql.Append($@" AND userInfo.IsAdmin=0 ");

            sql.Append(" GROUP BY userInfo.HeadImage,userInfo.UserId,userInfo.OrganizationId,userInfo.Code,userInfo.Name,userInfo.Mobile,userInfo.OtherContactInformation,userInfo.Email,userInfo.LastVisitTime,userInfo.IsFreeze,userInfo.Remark,userInfo.Nature,userInfo.OrganizationId,org.ParentIdsName");
            return new SqlMapperUtil().SqlWithParams<SystemUserChosenOutput>(sql.ToString());
        }
    }
}