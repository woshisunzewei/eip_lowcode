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
    /// 组织机构
    /// </summary>
    public class SystemOrganizationRepository : ISystemOrganizationRepository
    {
        /// <summary>
        /// 获取数据权限
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<IEnumerable<BaseTree>> FindDataPermissionTree(SystemOrganizationDataPermissionInput input)
        {
            var sql = new StringBuilder();
            sql.Append($"SELECT OrganizationId id,ParentId parent,Name text,ParentIds parents,Nature icon FROM System_Organization org WHERE 1=1 AND {input.DataSql} ORDER BY OrderNo");
            return new SqlMapperUtil().SqlWithParams<BaseTree>(sql.ToString());
        }

        /// <summary>
        /// 根据父级查询下级
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<IEnumerable<SystemOrganizationOutput>> FindByParentId(SystemOrganizationDataPermissionInput input)
        {
            var sql = new StringBuilder();
            sql.Append("select * from System_Organization org where 1=1 ");
            if (input.DataSql.IsNotNullOrEmpty())
            {
                sql.Append("AND " + input.DataSql);
            }
            sql.Append(input.Sql);
            return new SqlMapperUtil().SqlWithParams<SystemOrganizationOutput>(sql.ToString());
        }
    }
}