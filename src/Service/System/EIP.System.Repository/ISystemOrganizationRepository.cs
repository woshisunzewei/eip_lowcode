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
    /// ��֯����
    /// </summary>
    public interface ISystemOrganizationRepository 
    {
        /// <summary>
        /// ��ȡ��������Ȩ�޵���֯��������
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IEnumerable<BaseTree>> FindDataPermissionTree(SystemOrganizationDataPermissionInput input);

        /// <summary>
        /// ���ݸ�����ѯ�¼�
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IEnumerable<SystemOrganizationOutput>> FindByParentId(SystemOrganizationDataPermissionInput input);
    }
}