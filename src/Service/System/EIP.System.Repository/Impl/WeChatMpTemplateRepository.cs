/**************************************************************
* Copyright (C) 2022 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2022/01/12 22:40:15
* 文件名: 
* 描述: 
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.System.Models.Dtos.WeChat.MpTemplate;

namespace EIP.WeChat.Repository.Impl
{
    /// <summary>
    /// 微信公众号模版
    /// </summary>
    public class WeChatMpTemplateRepository : IWeChatMpTemplateRepository
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResults<WeChatMpTemplatePagingOutput>> Find(WeChatMpTemplatePagingInput input)
        {
            var sql = new StringBuilder(
                   "SELECT TemplateId,Code,Title,IndustryOne,IndustryTwo,CreateTime,@rowNumber, @recordCount from wechat_mp_template @where ");
            if (input.DataSql.IsNotNullOrEmpty())
            {
                sql.Append(@" AND " + input.DataSql + "");
            }
            if (input.Sidx.IsNullOrEmpty())
            {
                input.Sidx = " CreateTime ";
            }
            return new SqlMapperUtil().PagingQuerySqlAsync<WeChatMpTemplatePagingOutput>(sql.ToString(), input);
        }
    }
}