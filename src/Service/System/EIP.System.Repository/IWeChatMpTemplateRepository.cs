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

namespace EIP.WeChat.Repository
{
    /// <summary>
    /// ΢�Ź��ں�ģ��
    /// </summary>
    public interface IWeChatMpTemplateRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<PagedResults<WeChatMpTemplatePagingOutput>> Find(WeChatMpTemplatePagingInput input);
    }
}