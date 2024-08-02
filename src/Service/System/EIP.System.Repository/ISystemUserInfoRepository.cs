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
namespace EIP.System.Repository
{
    /// <summary>
    /// �û���Ϣ
    /// </summary>
    public interface ISystemUserInfoRepository
    {

        /// <summary>
        /// ���Ӳ�ѯ��ҳ��ʽ
        /// </summary>
        /// <param name="input">��ѯ����</param>
        /// <returns>��ҳ</returns>
        Task<PagedResults<SystemUserOutput>> Find(SystemUserPagingInput input);

        /// <summary>
        /// �û�������Ϣ
        /// </summary>
        /// <param name="input">��ѯ����</param>
        /// <returns>��ҳ</returns>
        Task<IEnumerable<SystemUserFindCommonOutput>> FindCommon(SystemUserFindCommonInput input);

        /// <summary>
        /// ���Ӳ�ѯ��ҳ��ʽ
        /// </summary>
        /// <param name="dataSql">��ѯ����</param>
        /// <returns>��ҳ</returns>
        Task<IEnumerable<SystemUserChosenOutput>> FindAllUser(string dataSql);
    }
}