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
using EIP.System.Models.Dtos.Data;

namespace EIP.System.Logic
{
    /// <summary>
    /// ����Ȩ��ҵ���߼�
    /// </summary>
    public interface ISystemDataLogic : IAsyncLogic<SystemData>
    {
        /// <summary>
        /// ����ģ��Id��ȡ����Ȩ�޹���
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemDataLogic_Cache")]
        Task<OperateStatus<PagedResults<SystemDataFindOutput>>> Find(SystemDataFindInput input);

        /// <summary>
        /// ��������Ȩ�޹���
        /// </summary>
        /// <param name="input">����Ȩ�޹���</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemDataLogic_Cache,ISystemPermissionLogic_Cache")]
        Task<OperateStatus> Save(SystemData input);

        /// <summary>
        /// ɾ������Ȩ�޹�����Ϣ
        /// </summary>
        /// <param name="input">����Ȩ�޹���Id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemDataLogic_Cache,ISystemPermissionLogic_Cache")]
        Task<OperateStatus> Delete(IdInput<string> input);

        /// <summary>
        /// ����Id��ȡ
        /// </summary>
        /// <param name="input">Id</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemDataLogic_Cache")]
        Task<OperateStatus<SystemData>> FindById(IdInput input);

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemDataLogic_Cache")]
        Task<OperateStatus> IsFreeze(IdInput input);
    }
}