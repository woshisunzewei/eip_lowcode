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
using EIP.System.Models.Dtos.Menu;

namespace EIP.System.Logic
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISystemMenuLogic : IAsyncLogic<SystemMenu>
    {
        #region ģ��

        /// <summary>
        /// ����״̬ΪTrue��ģ����Ϣ
        /// </summary>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemMenuLogic_Cache")]
        Task<OperateStatus<IEnumerable<BaseTree>>> Tree(bool? isAppMenu = null);

        /// <summary>
        /// ��ȡȨ�����˵�
        /// </summary>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemMenuLogic_Cache")]
        Task<OperateStatus<IEnumerable<BaseTree>>> FindPermissionMenu(EnumPrivilegeAccess privilegeAccesst);

        /// <summary>
        /// ���ݸ�����ѯ�¼�
        /// </summary>
        /// <param name="menu">����id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemMenuLogic_Cache,ISystemPermissionLogic_Cache,ISystemDataBaseLogic_Cache,ISystemAgileLogic_Cache")]
        Task<OperateStatus<Guid>> Save(SystemMenu input);

        /// <summary>
        /// �˵��ƶ�����
        /// </summary>
        /// <param name="menu">����id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemMenuLogic_Cache,ISystemPermissionLogic_Cache,ISystemDataBaseLogic_Cache,ISystemAgileLogic_Cache")]
        Task<OperateStatus<Guid>> SaveMove(SystemMenu input);

        /// <summary>
        /// ɾ��ģ�鼰�¼�����
        /// </summary>
        /// <param name="input">����id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemMenuLogic_Cache,ISystemPermissionLogic_Cache")]
        Task<OperateStatus> Delete(IdInput<string> input);

        /// <summary>
        /// ��ȡ��ʾ��ģ���б�������
        /// </summary>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemMenuLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemMenuFindOutput>>> Find(SystemMenuFindInput input);

        /// <summary>
        /// ����Id��ȡ
        /// </summary>
        /// <param name="input">Id</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemMenuLogic_Cache")]
        Task<OperateStatus<SystemMenu>> FindById(IdInput input);

        /// <summary>
        /// �Ƿ���ʾ�˵�
        /// </summary>
        /// <param name="input">Id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemMenuLogic_Cache,ISystemPermissionLogic_Cache")]
        Task<OperateStatus> IsShowMenu(IdInput input);

        /// <summary>
        /// �Ƿ����ģ��Ȩ��
        /// </summary>
        /// <param name="input">Id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemMenuLogic_Cache,ISystemPermissionLogic_Cache")]
        Task<OperateStatus> HaveMenuPermission(IdInput input);

        /// <summary>
        /// �Ƿ��������Ȩ��
        /// </summary>
        /// <param name="input">Id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemMenuLogic_Cache,ISystemPermissionLogic_Cache")]
        Task<OperateStatus> HaveDataPermission(IdInput input);

        /// <summary>
        /// �Ƿ�����ֶ�Ȩ��
        /// </summary>
        /// <param name="input">Id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemMenuLogic_Cache,ISystemPermissionLogic_Cache")]
        Task<OperateStatus> HaveFieldPermission(IdInput input);

        /// <summary>
        /// �Ƿ���й�����Ȩ��
        /// </summary>
        /// <param name="input">Id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemMenuLogic_Cache,ISystemPermissionLogic_Cache")]
        Task<OperateStatus> HaveButtonPermission(IdInput input);

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="input">Id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemMenuLogic_Cache,ISystemPermissionLogic_Cache")]
        Task<OperateStatus> IsFreeze(IdInput input);
        #endregion

    }
}