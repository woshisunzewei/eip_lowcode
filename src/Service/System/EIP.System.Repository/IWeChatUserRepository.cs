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
using EIP.System.Models.Dtos.WeChat.User;

namespace EIP.WeChat.Repository
{
    /// <summary>
    /// ΢����Ȩ�û�
    /// </summary>
    public interface IWeChatUserRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<PagedResults<WeChatUserPagingOutput>> Find(WeChatUserPagingInput input);
    }
}