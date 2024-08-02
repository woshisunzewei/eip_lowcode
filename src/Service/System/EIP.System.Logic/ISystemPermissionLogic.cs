/**************************************************************
* Copyright (C) 2022 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2022/01/12 22:40:15
* 文件名: 
* 描述: 
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.System.Models.Dtos.Menu;
using EIP.System.Models.Dtos.MenuButton;

namespace EIP.System.Logic
{
    /// <summary>
    /// 权限
    /// </summary>
    public interface ISystemPermissionLogic : IAsyncLogic<SystemPermission>
    {
        /// <summary>
        /// 根据状态为True的模块信息
        /// </summary>
        /// <param name="input">权限类型</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemPermissionLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemPermission>>> FindPermissionByPrivilegeMasterValue(SystemPermissionByPrivilegeMasterValueInput input);

        /// <summary>
        /// 保存权限信息
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
        /// 根据用户Id获取用户具有的模块权限
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
        /// 根据用户Id获取用户具有的模块权限
        /// </summary>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemPermissionLogic_Cache")]
        Task<OperateStatus<IEnumerable<BaseTree>>> FindSystemPermissionMenuAllByUserId();

        /// <summary>
        /// 根据角色Id,岗位Id,组Id,人员Id获取具有的模块信息
        /// </summary>
        /// <param name="input">输入参数</param>
        /// <returns>树形模块信息</returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemPermissionLogic_Cache")]
        Task<OperateStatus<IEnumerable<BaseTree>>> FindMenuHavePermissionByPrivilegeMasterValue(SystemPermissiontMenuHaveByPrivilegeMasterValueInput input);

        /// <summary>
        /// 根据特权类型查询对应拥有的功能项模块信息
        /// </summary>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemPermissionLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemPermissionFindMenuButtonByPrivilegeMasterOutput>>> FindMenuButtonByPrivilegeMaster(SystemPermissionFindMenuButtonByPrivilegeMasterInput input);

        /// <summary>
        /// 根据特权类型查询对应拥有的功能项模块信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemPermissionLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemMenuButton>>> FindMenuButtonByPrivilegeMasterAll(
            SystemPermissionFindMenuButtonByPrivilegeMasterInput input);

        /// <summary>
        /// 根据特权类型查询对应拥有的数据信息
        /// </summary>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemPermissionLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemPermissionFindDataByPrivilegeMasterOutput>>> FindDataByPrivilegeMaster(SystemPermissionFindDataByPrivilegeMasterInput input);

        /// <summary>
        /// 根据特权类型查询对应拥有的数据信息
        /// </summary>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemPermissionLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemData>>> FindDataByPrivilegeMasterAll(SystemPermissionFindDataByPrivilegeMasterInput input);

        /// <summary>
        /// 获取登录人员对应模块下的功能项
        /// </summary>
        /// <param name="viewRote">路由信息</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemPermissionLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemMenuButtonByViewRote>>> FindMenuButton(ViewRote viewRote);

        /// <summary>
        /// 获取角色，组等具有的权限
        /// </summary>
        /// <param name="privilegeMaster">类型:角色，人员，组等</param>
        /// <param name="privilegeMasterValue">对应值</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemPermissionLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemPermission>>> FindSystemPermissionsByPrivilegeMasterValueAndValue(
            EnumPrivilegeMaster privilegeMaster,
            Guid? privilegeMasterValue = null);

        /// <summary>
        /// 获取数据权限Sql
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemPermissionLogic_Cache")]
        Task<OperateStatus<string>> FindDataPermissionSql(ViewRote input);

    }
}