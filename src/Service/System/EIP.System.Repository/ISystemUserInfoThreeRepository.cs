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
using EIP.System.Models.Dtos.UserThree;

namespace EIP.System.Repository
{
    /// <summary>
    /// �û���Ϣ
    /// </summary>
    public interface ISystemUserInfoThreeRepository
    {
        /// <summary>
        /// ���Ӳ�ѯ��ҳ��ʽ
        /// </summary>
        /// <param name="input">��ѯ����</param>
        /// <returns>��ҳ</returns>
        Task<PagedResults<UserThreePagingOutput>> Find(UserThreePagingInput input);

    }
}