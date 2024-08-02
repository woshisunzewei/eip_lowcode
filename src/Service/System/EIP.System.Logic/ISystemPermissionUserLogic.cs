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
namespace EIP.System.Logic
{
    /// <summary>
    /// Ȩ���û�ҵ���߼��ӿ�
    /// </summary>
    public interface ISystemPermissionUserLogic : IAsyncLogic<SystemPermissionUser>
    {
        /// <summary>
        /// ��������û�:��֯��������λ���顢��Ա
        /// </summary>
        /// <param name="master">����</param>
        /// <param name="value">ҵ���Id������֯����Id����Id����λId����ԱId��</param>
        /// <param name="userIds">Ȩ������:��֯�������顢��λ����ԱId</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemPermissionUserLogic_Cache")]
        Task<OperateStatus> SavePermissionUser(EnumPrivilegeMaster master,
            Guid value,
            IList<Guid> userIds);

        /// <summary>
        /// ��������û�:��֯��������λ���顢��Ա���Ƚ���ɾ��,�ٽ�����ӡ�
        /// </summary>
        /// <param name="master">����</param>
        /// <param name="value">ҵ���Id������֯����Id����Id����λId����ԱId��</param>
        /// <param name="userIds">Ȩ������:��֯�������顢��λ����ԱId</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemPermissionUserLogic_Cache")]
        Task<OperateStatus> SavePermissionUserBeforeDelete(EnumPrivilegeMaster master,
            Guid value,
            IList<Guid> userIds);

        /// <summary>
        /// �����û�Ȩ������
        /// </summary>
        /// <param name="privilegeMaster">����</param>
        /// <param name="privilegeMasterUserId">ҵ���Id������֯����Id����Id����λId����ԱId��</param>
        /// <param name="privilegeMasterValue">Ȩ������:��ɫId</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemPermissionUserLogic_Cache")]
        Task<OperateStatus> SavePermissionMasterValueBeforeDelete(EnumPrivilegeMaster privilegeMaster,
            Guid privilegeMasterUserId,
            IList<Guid> privilegeMasterValue);

        /// <summary>
        /// ɾ���û�
        /// </summary>
        /// <param name="privilegeMasterUserId">�û�Id</param>
        /// <param name="privilegeMasterValue">��������Id:��֯��������ɫ����λ����</param>
        /// <param name="privilegeMaster">������Ա����:��֯��������ɫ����λ����</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemPermissionUserLogic_Cache")]
        Task<OperateStatus> DeletePrivilegeMasterUser(Guid privilegeMasterUserId,
            Guid privilegeMasterValue,
            EnumPrivilegeMaster privilegeMaster);

        /// <summary>
        /// ɾ���û���ӦȨ������
        /// </summary>
        /// <param name="privilegeMasterUserId">�û�Id</param>
        /// <param name="privilegeMaster">������Ա����:��֯��������ɫ����λ����</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemPermissionUserLogic_Cache")]
        Task<OperateStatus> DeletePrivilegeMasterUser(Guid privilegeMasterUserId,
            EnumPrivilegeMaster privilegeMaster);
        /// <summary>
        /// ɾ���û���ӦȨ������
        /// </summary>
        /// <param name="userId">�û�Id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemPermissionUserLogic_Cache")]
        Task<OperateStatus> DeletePrivilegeMasterUser(Guid userId);
        /// <summary>
        /// ��ȡģ�顢�ֶζ�Ӧӵ������Ϣ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemPermissionUserLogic_Cache")]
        Task<OperateStatus<SystemPrivilegeDetailOutput>> FindSystemPrivilegeDetailOutputsByAccessAndValue(SystemPrivilegeDetailInput input);

        /// <summary>
        /// ������Ȩ���ͼ���Ȩid��ȡ��Ȩ�û���Ϣ
        /// </summary>
        /// <param name="privilegeMaster">��Ȩ����</param>
        /// <param name="privilegeMasterValue">��Ȩid</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemPermissionUserLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemPermissionUser>>> FindPermissionUsersByPrivilegeMasterAdnPrivilegeMasterValue(
            EnumPrivilegeMaster privilegeMaster,
            Guid? privilegeMasterValue = null);
    }
}