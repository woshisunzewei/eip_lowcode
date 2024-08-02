/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/5/29 15:57:28
* 文件名: DsDatasetRepository
* 描述: 数据集表数据访问接口实现
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
    /// 数据集表数据访问接口实现
    /// </summary>
    public class DsDatasetRepository :IDsDatasetRepository
    {
         /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResults<DsDatasetFindOutput>> Find(DsDatasetFindInput input)
        {
            var sql = new StringBuilder($@"SELECT  
                                                  id,
                                                  name,
                                                  code,
                                                  type_id typeId,
                                                  remark,
                                                  dataset_type datasetType,
                                                  module_code moduleCode,
                                                  editable,
                                                  source_id sourceId,
                                                  cache,
                                                  config,
                                                   @rowNumber, @recordCount FROM ds_dataset @where ");
            
            if (input.Sidx.IsNullOrEmpty())
            {
                input.Sidx = " Id ";
            }
            return new SqlMapperUtil().PagingQuerySqlAsync<DsDatasetFindOutput>(sql.ToString(), input);
        }
    }
}