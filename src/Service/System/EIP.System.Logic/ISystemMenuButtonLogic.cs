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
using EIP.System.Models.Dtos.MenuButton;

namespace EIP.System.Logic
{
    /// <summary>
    /// ������ҵ���߼�
    /// </summary>
    public interface ISystemMenuButtonLogic : IAsyncLogic<SystemMenuButton>
    {
        /// <summary>
        /// ����ģ���ȡ��������Ϣ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemMenuButtonLogic_Cache")]
        Task<OperateStatus<PagedResults<SystemMenuButtonFindOutput>>> Find(SystemMenuButtonFindInput input);

        /// <summary>
        /// ���湦������Ϣ
        /// </summary>
        /// <param name="menuButton">��������Ϣ</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemMenuButtonLogic_Cache,ISystemPermissionLogic_Cache")]
        Task<OperateStatus> Save(SystemMenuButton menuButton);
        /// <summary>
        /// ���湦������Ϣ
        /// </summary>
        /// <param name="menuButton">��������Ϣ</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemMenuButtonLogic_Cache,ISystemPermissionLogic_Cache")]
        Task<OperateStatus> SaveAll(SystemMenuButtonSaveAllInput input);

        /// <summary>
        /// ɾ��������
        /// </summary>
        /// <param name="input">��������Ϣ</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemMenuButtonLogic_Cache,ISystemPermissionLogic_Cache")]
        Task<OperateStatus> Delete(IdInput<string> input);

        /// <summary>
        /// ����Id��ȡ
        /// </summary>
        /// <param name="input">Id</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemMenuButtonLogic_Cache")]
        Task<OperateStatus<SystemMenuButton>> FindById(IdInput input);

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemMenuButtonLogic_Cache,ISystemPermissionLogic_Cache")]
        Task<OperateStatus> IsFreeze(IdInput input);

        /// <summary>
        /// �Ƿ���ʾ���б�
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemMenuButtonLogic_Cache,ISystemPermissionLogic_Cache")]
        Task<OperateStatus> IsShowTable(IdInput input);
    }
}