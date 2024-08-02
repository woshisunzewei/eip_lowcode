/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/5/29 15:57:27
* 文件名: DashboardPageTemplateRepository
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
using EIP.DataRoom.Models.Dtos;
using System.Text;

namespace EIP.DataRoom.Repository.Impl
{
    /// <summary>
    /// 页面模板表数据访问接口实现
    /// </summary>
    public class DashboardPageTemplateRepository :IDashboardPageTemplateRepository
    {
         /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResults<DashboardPageTemplateFindOutput>> Find(DashboardPageTemplateFindInput input)
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
                                                   @rowNumber, @recordCount FROM dashboard_page_template @where ");
            
            if (input.Sidx.IsNullOrEmpty())
            {
                input.Sidx = " Id ";
            }
            return new SqlMapperUtil().PagingQuerySqlAsync<DashboardPageTemplateFindOutput>(sql.ToString(), input);
        }
    }
}