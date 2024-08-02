/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/6/1 16:39:27
* 文件名: BigScreenPageTemplateRepository
* 描述: 页面模板表数据访问接口实现
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
using EIP.Big.Models.Dtos.ScreenPageTemplate;
using System.Text;
using System.Threading.Tasks;

namespace EIP.DataRoom.Repository.Impl
{
    /// <summary>
    /// 页面模板表数据访问接口实现
    /// </summary>
    public class BigScreenPageTemplateRepository :IBigScreenPageTemplateRepository
    {
         /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResults<BigScreenPageTemplateFindOutput>> Find(BigScreenPageTemplateFindInput input)
        {
            var sql = new StringBuilder($@"SELECT  
                                                  id,
                                                  name,
                                                  type,
                                                  config,
                                                  thumbnail,
                                                  remark,
                                                  update_date,
                                                  create_date,
                                                  create_by,
                                                  update_by,
                                                  del_flag,
                                                   @rowNumber, @recordCount FROM big_screen_page_template @where ");
            
            if (input.Sidx.IsNullOrEmpty())
            {
                input.Sidx = " Id ";
            }
            return new SqlMapperUtil().PagingQuerySqlAsync<BigScreenPageTemplateFindOutput>(sql.ToString(), input);
        }
    }
}