/**************************************************************
* Copyright (C) 2022 www.eipflow.com ����ΰ��Ȩ����(����ؾ�)
*
* ����: ����ΰ(QQ 1039318332)
* ����ʱ��: 2022/01/12 22:40:15
* �ļ���: 
* ����: 
* 
* �޸���ʷ
* �޸��ˣ�
* ʱ�䣺
* �޸�˵����
*
**************************************************************/
using EIP.System.Models.Dtos.WeChat.MpTemplate;

namespace EIP.WeChat.Repository.Impl
{
    /// <summary>
    /// ΢�Ź��ں�ģ��
    /// </summary>
    public class WeChatMpTemplateRepository : IWeChatMpTemplateRepository
    {
        /// <summary>
        /// ��ȡ
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