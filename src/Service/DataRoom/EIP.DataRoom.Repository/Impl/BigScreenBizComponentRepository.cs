/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/6/1 16:39:27
* 文件名: BigScreenBizComponentRepository
* 描述: 业务组件表数据访问接口实现
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Big.Models.Dtos.ScreenBizComponent;
using EIP.Common.Core.Extension;
using EIP.Common.Models.Paging;
using EIP.Common.Repository;
using System.Text;

namespace EIP.DataRoom.Repository.Impl
{
    /// <summary>
    /// 业务组件表数据访问接口实现
    /// </summary>
    public class BigScreenBizComponentRepository :IBigScreenBizComponentRepository
    {
         /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResults<BigScreenBizComponentFindOutput>> Find(BigScreenBizComponentFindInput input)
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
                                                  module_code moduleCode,
                                                   @rowNumber, @recordCount FROM big_screen_biz_component @where ");
            
            if (input.Sidx.IsNullOrEmpty())
            {
                input.Sidx = " Id ";
            }
            return new SqlMapperUtil().PagingQuerySqlAsync<BigScreenBizComponentFindOutput>(sql.ToString(), input);
        }
    }
}