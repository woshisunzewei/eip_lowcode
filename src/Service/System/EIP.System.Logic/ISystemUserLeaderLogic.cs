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
using EIP.System.Models.Dtos.UserLeader;

namespace EIP.System.Logic
{
    /// <summary>
    /// �û�ҵ���߼�
    /// </summary>
    public interface ISystemUserLeaderLogic : IAsyncLogic<SystemUserLeader>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemUserLeaderLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemUserLeader>>> GetUserLeader(IdInput input);

        /// <summary>
        /// �����ϼ�
        /// </summary>
        /// <param name="userId">��ԱId��</param>
        /// <param name="userLeaders">�����쵼Id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemUserLeaderLogic_Cache")]
        Task<OperateStatus> Save(SystemUserLeaderSaveInput input);

        /// <summary>
        /// ���������¼�
        /// </summary>
        /// <param name="userId">��ԱId��</param>
        /// <param name="userSubordinate">�����¼�Id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemUserLeaderLogic_Cache")]
        Task<OperateStatus> SaveSubordinate(SystemUserLeaderSaveInput input);

        /// <summary>
        /// ��ȡ�ҵ��¼�
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemUserLeaderLogic_Cache")]
        Task<OperateStatus<IList<SystemUserLeadersOutput>>> FindSubordinate(IdInput input);
    }
}