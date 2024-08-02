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
    /// ��֯����
    /// </summary>
    public interface ISystemOrganizationLogic : IAsyncLogic<SystemOrganization>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<OperateStatus<IEnumerable<BaseTree>>> FindDataPermissionTree(SystemOrganizationDataPermissionInput input);

        /// <summary>
        /// ͬ����ȡ����������
        /// </summary>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemOrganizationLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemOrganizationChartOutput>>> FindOrganizationAllTree();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="topOrg"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemOrganizationLogic_Cache")]
        Task<OperateStatus<IEnumerable<BaseTree>>> FindOrganizationTree(SystemOrganizationTreeInput input);

        /// <summary>
        /// ���ݸ�����ѯ�¼�
        /// </summary>
        /// <param name="input">����id</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemOrganizationLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemOrganizationOutput>>> Find(SystemOrganizationDataPermissionInput input);

        /// <summary>
        /// ��ȡ��������Ȩ�޵���֯��������
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemOrganizationLogic_Cache")]
        Task<OperateStatus<IEnumerable<BaseTree>>> FindDataPermission(
            SystemOrganizationDataPermissionInput input);

        /// <summary>
        /// ������֯����
        /// </summary>
        /// <param name="input">��֯����</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemOrganizationLogic_Cache")]
        Task<OperateStatus> Save(SystemOrganization input);

        /// <summary>
        /// ɾ����֯�����¼�����
        /// </summary>
        /// <param name="input">����id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemOrganizationLogic_Cache")]
        Task<OperateStatus> Delete(IdInput<string> input);

        /// <summary>
        /// ����Id��ȡ
        /// </summary>
        /// <param name="input">Id</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemOrganizationLogic_Cache")]
        Task<OperateStatus<SystemOrganization>> FindById(IdInput input);

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemOrganizationLogic_Cache")]
        Task<OperateStatus> IsFreeze(IdInput input);

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemOrganizationLogic_Cache")]
        Task<OperateStatus<List<string>>> Import(IList<SystemOrganizationImportDto> input);
    }
}