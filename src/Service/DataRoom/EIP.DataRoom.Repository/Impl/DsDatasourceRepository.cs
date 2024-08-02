/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/5/29 15:57:29
* 文件名: DsDatasourceRepository
* 描述: 数据源配置表数据访问接口实现
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Common.Core.Extension;
using EIP.Common.Models.Paging;
using EIP.Common.Repository;
using EIP.DataRoom.Models.Dtos;
using System.Text;

namespace EIP.DataRoom.Repository.Impl
{
    /// <summary>
    /// 数据源配置表数据访问接口实现
    /// </summary>
    public class DsDatasourceRepository :IDsDatasourceRepository
    {
         /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResults<DsDatasourceFindOutput>> Find(DsDatasourceFindInput input)
        {
            var sql = new StringBuilder($@"SELECT  
                                                  id,
                                                  source_name sourceName,
                                                  source_type sourceType,
                                                  driver_class_name driverClassName,
                                                  url,
                                                  host,
                                                  port,
                                                  username,
                                                  password,
                                                  table_name tableName,
                                                  module_code moduleCode,
                                                  remark,
                                                   @rowNumber, @recordCount FROM ds_datasource @where ");
            
            if (input.Sidx.IsNullOrEmpty())
            {
                input.Sidx = " Id ";
            }
            return new SqlMapperUtil().PagingQuerySqlAsync<DsDatasourceFindOutput>(sql.ToString(), input);
        }
    }
}