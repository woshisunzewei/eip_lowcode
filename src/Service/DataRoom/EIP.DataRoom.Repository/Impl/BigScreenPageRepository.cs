/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/6/1 16:54:21
* 文件名: BigScreenPageRepository
* 描述: 页面基本信息表数据访问接口实现
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
using EIP.Big.Models.Dtos.ScreenPage;
using System.Text;
using System.Threading.Tasks;

namespace EIP.DataRoom.Repository.Impl
{
    /// <summary>
    /// 页面基本信息表数据访问接口实现
    /// </summary>
    public class BigScreenPageRepository :IBigScreenPageRepository
    {
         /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResults<BigScreenPageFindOutput>> Find(BigScreenPageFindInput input)
        {
            var sql = new StringBuilder($@"SELECT  
                                                  id,
                                                  name,
                                                  code,
                                                  cover_picture coverPicture,
                                                  type,
                                                  config,
                                                  parent_code parentCode,
                                                  order_num orderNum,
                                                  remark,
                                                  app_code appCode,
                                                   @rowNumber, @recordCount FROM big_screen_page @where ");
            
            if (input.Sidx.IsNullOrEmpty())
            {
                input.Sidx = " Id ";
            }
            return new SqlMapperUtil().PagingQuerySqlAsync<BigScreenPageFindOutput>(sql.ToString(), input);
        }
    }
}