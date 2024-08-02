/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/6/1 16:54:21
* 文件名: BigScreenMapRepository
* 描述: 地图数据维护表数据访问接口实现
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
using EIP.Big.Models.Dtos.ScreenMap;
using System.Text;
using System.Threading.Tasks;

namespace EIP.DataRoom.Repository.Impl
{
    /// <summary>
    /// 地图数据维护表数据访问接口实现
    /// </summary>
    public class BigScreenMapRepository :IBigScreenMapRepository
    {
         /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResults<BigScreenMapFindOutput>> Find(BigScreenMapFindInput input)
        {
            var sql = new StringBuilder($@"SELECT  
                                                  id,
                                                  parent_id,
                                                  map_code,
                                                  name,
                                                  geo_json,
                                                  level,
                                                  uploaded_geo_json,
                                                  update_date,
                                                  create_date,
                                                  create_by,
                                                  update_by,
                                                  del_flag,
                                                   @rowNumber, @recordCount FROM big_screen_map @where ");
            
            if (input.Sidx.IsNullOrEmpty())
            {
                input.Sidx = " Id ";
            }
            return new SqlMapperUtil().PagingQuerySqlAsync<BigScreenMapFindOutput>(sql.ToString(), input);
        }
    }
}