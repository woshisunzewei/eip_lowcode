/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/5/29 15:57:26
* 文件名: DashboardBizComponentRepository
* 描述: 业务组件表数据访问接口实现
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
    /// 业务组件表数据访问接口实现
    /// </summary>
    public class DashboardBizComponentRepository :IDashboardBizComponentRepository
    {
         /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResults<DashboardBizComponentFindOutput>> Find(DashboardBizComponentFindInput input)
        {
            var sql = new StringBuilder($@"SELECT  
                                                  id,
                                                  name,
                                                  code,
                                                  type,
                                                  cover_picture coverPicture,
                                                  vue_content vueContent,
                                                  setting_content settingContent,
                                                  order_num orderNum,
                                                  remark,
                                                   @rowNumber, @recordCount FROM dashboard_biz_component @where ");
            
            if (input.Sidx.IsNullOrEmpty())
            {
                input.Sidx = " Id ";
            }
            return new SqlMapperUtil().PagingQuerySqlAsync<DashboardBizComponentFindOutput>(sql.ToString(), input);
        }
    }
}