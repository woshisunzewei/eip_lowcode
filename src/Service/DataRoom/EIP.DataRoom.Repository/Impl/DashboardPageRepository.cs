/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/5/29 15:57:27
* 文件名: DashboardPageRepository
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
using EIP.DataRoom.Models.Dtos;
using System.Text;

namespace EIP.DataRoom.Repository.Impl
{
    /// <summary>
    /// 页面基本信息表数据访问接口实现
    /// </summary>
    public class DashboardPageRepository : IDashboardPageRepository
    {
        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResults<DashboardPageFindOutput>> Find(DashboardPageFindInput input)
        {
            var sql = new StringBuilder($@"SELECT  
                                                  id,
                                                  name,
                                                  code,
                                                  cover_picture coverPicture,
                                                  icon,
                                                  icon_color iconColor,
                                                  type,
                                                  layout,
                                                  config,
                                                  parent_code parentCode,
                                                  order_num orderNum,
                                                  remark,
                                                  model_code modelCode,
                                                  app_code appCode,
                                                
                                                   @rowNumber, @recordCount FROM dashboard_page @where ");

            if (input.type.IsNotNullOrEmpty())
            {
                sql.Append($" and Type='{input.type}' ");
            }

            if (input.Sidx.IsNullOrEmpty())
            {
                input.Sidx = " Id ";
            }
            return new SqlMapperUtil().PagingQuerySqlAsync<DashboardPageFindOutput>(sql.ToString(), input);
        }
    }
}