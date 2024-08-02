/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/6/1 16:54:21
* 文件名: BigScreenFileRepository
* 描述: 文件表数据访问接口实现
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
using EIP.Big.Models.Dtos.ScreenFile;
using System.Text;
using System.Threading.Tasks;

namespace EIP.DataRoom.Repository.Impl
{
    /// <summary>
    /// 文件表数据访问接口实现
    /// </summary>
    public class BigScreenFileRepository :IBigScreenFileRepository
    {
         /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResults<BigScreenFileFindOutput>> Find(BigScreenFileFindInput input)
        {
            var sql = new StringBuilder($@"SELECT  
                                                  id,
                                                  module,
                                                  original_name originalName,
                                                  new_name newName,
                                                  extension,
                                                  path,
                                                  url,
                                                  size,
                                                  download_count downloadCount,
                                                  bucket,
                                                   @rowNumber, @recordCount FROM big_screen_file @where ");
            
            if (input.Sidx.IsNullOrEmpty())
            {
                input.Sidx = " Id ";
            }
            return new SqlMapperUtil().PagingQuerySqlAsync<BigScreenFileFindOutput>(sql.ToString(), input);
        }
    }
}