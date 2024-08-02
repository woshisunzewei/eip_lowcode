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

using EIP.System.Models.Dtos.Material;

namespace EIP.System.Repository.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemMaterialRepository : ISystemMaterialRepository
    {
        /// <summary>
        /// 复杂查询分页方式
        /// </summary>
        /// <param name="input">查询参数</param>
        /// <returns>分页</returns>
        public Task<PagedResults<SystemMaterial>> Find(SystemMaterialFindInput input)
        {
            var sql = new StringBuilder($"SELECT *,@rowNumber, @recordCount FROM System_Material @where and IsDelete=0 ");
                sql.Append(input.ParentId.HasValue ? $" and ParentId='{input.ParentId}' " : " and ParentId is null ");

            if (input.Name.Trim().IsNotNullOrEmpty())
            {
                sql.Append($" and Name like '%{input.Name.Trim()}%' ");
            }

            if (input.Sidx.IsNullOrEmpty())
            {
                input.Sidx = " Id ";
            }
            return new SqlMapperUtil().PagingQuerySqlAsync<SystemMaterial>(sql.ToString(), input);
        }
    }
}