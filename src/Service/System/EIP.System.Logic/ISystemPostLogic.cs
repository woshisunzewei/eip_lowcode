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
using EIP.System.Models.Dtos.Post;
using EIP.System.Models.Dtos.Role;

namespace EIP.System.Logic
{
    /// <summary>
    /// ��λ
    /// </summary>
    public interface ISystemPostLogic : IAsyncLogic<SystemPost>
    {
        /// <summary>
        /// ������֯������ȡ��λ��Ϣ
        /// </summary>
        /// <param name="input">��֯����Id</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemPostLogic_Cache")]
        Task<OperateStatus<PagedResults<SystemPostFindOutput>>> Find(SystemPostFindInput input);

        /// <summary>
        /// ɾ����λ��Ϣ
        /// </summary>
        /// <param name="input">��λ��ϢId</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemPostLogic_Cache")]
        Task<OperateStatus> Delete(IdInput<string> input);

        /// <summary>
        /// �����λ��Ϣ
        /// </summary>
        /// <param name="post">��λ��Ϣ</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemPostLogic_Cache")]
        Task<OperateStatus> Save(SystemPost post);

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="input">������Ϣ</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemPostLogic_Cache")]
        Task<OperateStatus> Copy(SystemCopyInput input);

        /// <summary>
        /// ����Id��ȡ
        /// </summary>
        /// <param name="input">Id</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemPostLogic_Cache")]
        Task<OperateStatus<SystemPost>> FindById(IdInput input);

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemPostLogic_Cache")]
        Task<OperateStatus> IsFreeze(IdInput input);

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemPostLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemPost>>> All();

    }
}