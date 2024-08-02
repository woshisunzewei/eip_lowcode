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
using EIP.System.Models.Dtos.MenuButton;

namespace EIP.System.Logic
{
    /// <summary>
    /// Ȩ��
    /// </summary>
    public interface ISystemPermissionLogic : IAsyncLogic<SystemPermission>
    {
        /// <summary>
        /// ����״̬ΪTrue��ģ����Ϣ
        /// </summary>
        /// <param name="input">Ȩ������</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemPermissionLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemPermission>>> FindPermissionByPrivilegeMasterValue(SystemPermissionByPrivilegeMasterValueInput input);

        /// <summary>
        /// ����Ȩ����Ϣ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemPermissionLogic_Cache,IWorkflowPermissionLogic_Cache")]
        Task<OperateStatus> SavePermission(SystemPermissionSaveInput input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemPermissionLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemPermissionFindMenuByUserIdOutput>>> MenusTree(SystemPermissionMenuInput input);

        /// <summary>
        /// �����û�Id��ȡ�û����е�ģ��Ȩ��
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemPermissionLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemPermissionFindMenuByUserIdRouterOutput>>> Menus(SystemPermissionMenuInput input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemPermissionLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemMenuFindAgileOutput>>> MenusAgile(SystemPermissionMenuInput input);

        /// <summary>
        /// �����û�Id��ȡ�û����е�ģ��Ȩ��
        /// </summary>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemPermissionLogic_Cache")]
        Task<OperateStatus<IEnumerable<BaseTree>>> FindSystemPermissionMenuAllByUserId();

        /// <summary>
        /// ���ݽ�ɫId,��λId,��Id,��ԱId��ȡ���е�ģ����Ϣ
        /// </summary>
        /// <param name="input">�������</param>
        /// <returns>����ģ����Ϣ</returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemPermissionLogic_Cache")]
        Task<OperateStatus<IEnumerable<BaseTree>>> FindMenuHavePermissionByPrivilegeMasterValue(SystemPermissiontMenuHaveByPrivilegeMasterValueInput input);

        /// <summary>
        /// ������Ȩ���Ͳ�ѯ��Ӧӵ�еĹ�����ģ����Ϣ
        /// </summary>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemPermissionLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemPermissionFindMenuButtonByPrivilegeMasterOutput>>> FindMenuButtonByPrivilegeMaster(SystemPermissionFindMenuButtonByPrivilegeMasterInput input);

        /// <summary>
        /// ������Ȩ���Ͳ�ѯ��Ӧӵ�еĹ�����ģ����Ϣ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemPermissionLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemMenuButton>>> FindMenuButtonByPrivilegeMasterAll(
            SystemPermissionFindMenuButtonByPrivilegeMasterInput input);

        /// <summary>
        /// ������Ȩ���Ͳ�ѯ��Ӧӵ�е�������Ϣ
        /// </summary>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemPermissionLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemPermissionFindDataByPrivilegeMasterOutput>>> FindDataByPrivilegeMaster(SystemPermissionFindDataByPrivilegeMasterInput input);

        /// <summary>
        /// ������Ȩ���Ͳ�ѯ��Ӧӵ�е�������Ϣ
        /// </summary>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemPermissionLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemData>>> FindDataByPrivilegeMasterAll(SystemPermissionFindDataByPrivilegeMasterInput input);

        /// <summary>
        /// ��ȡ��¼��Ա��Ӧģ���µĹ�����
        /// </summary>
        /// <param name="viewRote">·����Ϣ</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemPermissionLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemMenuButtonByViewRote>>> FindMenuButton(ViewRote viewRote);

        /// <summary>
        /// ��ȡ��ɫ����Ⱦ��е�Ȩ��
        /// </summary>
        /// <param name="privilegeMaster">����:��ɫ����Ա�����</param>
        /// <param name="privilegeMasterValue">��Ӧֵ</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemPermissionLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemPermission>>> FindSystemPermissionsByPrivilegeMasterValueAndValue(
            EnumPrivilegeMaster privilegeMaster,
            Guid? privilegeMasterValue = null);

        /// <summary>
        /// ��ȡ����Ȩ��Sql
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemPermissionLogic_Cache")]
        Task<OperateStatus<string>> FindDataPermissionSql(ViewRote input);

    }
}