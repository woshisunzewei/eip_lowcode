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
using EIP.System.Models.Dtos.Role;

namespace EIP.System.Logic
{
    /// <summary>
    /// ��ɫ
    /// </summary>
    public interface ISystemRoleLogic : IAsyncLogic<SystemRole>
    {
        /// <summary>
        /// ������֯����Id��ѯ��ɫ��Ϣ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemRoleLogic_Cache")]
        Task<OperateStatus<PagedResults<SystemRolesFindOutput>>> Find(SystemRolesFindInput input);

        /// <summary>
        /// ������ɫ
        /// </summary>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemRoleLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemRoleSelectOutput>>> Select();

        /// <summary>
        /// �����λ��Ϣ
        /// </summary>
        /// <param name="role">��λ��Ϣ</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemRoleLogic_Cache")]
        Task<OperateStatus> Save(SystemRole role);

        /// <summary>
        /// ɾ����ɫ��Ϣ
        /// </summary>
        /// <param name="input">��ɫId</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemRoleLogic_Cache")]
        Task<OperateStatus> Delete(IdInput<string> input);

        /// <summary>
        /// ��ɫ����
        /// </summary>
        /// <param name="input">��ɫId</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemRoleLogic_Cache")]
        Task<OperateStatus> Copy(SystemCopyInput input);

        /// <summary>
        /// ��ȡ���н�ɫ
        /// </summary>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemRoleLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemRole>>> All();

        /// <summary>
        /// ����Id��ȡ
        /// </summary>
        /// <param name="input">��ɫId</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemRoleLogic_Cache")]
        Task<OperateStatus<SystemRole>> FindById(IdInput input);

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemRoleLogic_Cache")]
        Task<OperateStatus> IsFreeze(IdInput input);
    }
}