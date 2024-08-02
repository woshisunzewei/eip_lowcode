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
using EIP.System.Models.Dtos.Group;
using EIP.System.Models.Dtos.Role;

namespace EIP.System.Logic
{
    /// <summary>
    /// ��
    /// </summary>
    public interface ISystemGroupLogic : IAsyncLogic<SystemGroup>
    {
        /// <summary>
        /// ������֯������ȡ����Ϣ
        /// </summary>
        /// <param name="input">��֯����</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemGroupLogic_Cache")]
        Task<OperateStatus<PagedResults<SystemGroupFindOutput>>> Find(SystemGroupFindInput input);

        /// <summary>
        /// ɾ������Ϣ
        /// </summary>
        /// <param name="input">��Id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemGroupLogic_Cache")]
        Task<OperateStatus> Delete(IdInput<string> input);

        /// <summary>
        /// ��������Ϣ
        /// </summary>
        /// <param name="group">��λ��Ϣ</param>
        /// <param name="belongTo">����</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemGroupLogic_Cache")]
        Task<OperateStatus> Save(SystemGroup group,
            EnumGroupBelongTo belongTo);

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="input">������Ϣ</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemGroupLogic_Cache")]
        Task<OperateStatus> Copy(SystemCopyInput input);

        /// <summary>
        /// ����Id��ȡ
        /// </summary>
        /// <param name="input">Id</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemGroupLogic_Cache")]
        Task<OperateStatus<SystemGroup>> FindById(IdInput input);

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemGroupLogic_Cache")]
        Task<OperateStatus> IsFreeze(IdInput input);

        /// <summary>
        /// ��ȡ����δ������
        /// </summary>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemGroupLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemGroup>>> All();
    }
}