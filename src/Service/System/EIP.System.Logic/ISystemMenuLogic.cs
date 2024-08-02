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

namespace EIP.System.Logic
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISystemMenuLogic : IAsyncLogic<SystemMenu>
    {
        #region 模块

        /// <summary>
        /// 根据状态为True的模块信息
        /// </summary>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemMenuLogic_Cache")]
        Task<OperateStatus<IEnumerable<BaseTree>>> Tree(bool? isAppMenu = null);

        /// <summary>
        /// 获取权限树菜单
        /// </summary>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemMenuLogic_Cache")]
        Task<OperateStatus<IEnumerable<BaseTree>>> FindPermissionMenu(EnumPrivilegeAccess privilegeAccesst);

        /// <summary>
        /// 根据父级查询下级
        /// </summary>
        /// <param name="menu">父级id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemMenuLogic_Cache,ISystemPermissionLogic_Cache,ISystemDataBaseLogic_Cache,ISystemAgileLogic_Cache")]
        Task<OperateStatus<Guid>> Save(SystemMenu input);

        /// <summary>
        /// 菜单移动保存
        /// </summary>
        /// <param name="menu">父级id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemMenuLogic_Cache,ISystemPermissionLogic_Cache,ISystemDataBaseLogic_Cache,ISystemAgileLogic_Cache")]
        Task<OperateStatus<Guid>> SaveMove(SystemMenu input);

        /// <summary>
        /// 删除模块及下级数据
        /// </summary>
        /// <param name="input">父级id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemMenuLogic_Cache,ISystemPermissionLogic_Cache")]
        Task<OperateStatus> Delete(IdInput<string> input);

        /// <summary>
        /// 获取显示在模块列表上数据
        /// </summary>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemMenuLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemMenuFindOutput>>> Find(SystemMenuFindInput input);

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="input">Id</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemMenuLogic_Cache")]
        Task<OperateStatus<SystemMenu>> FindById(IdInput input);

        /// <summary>
        /// 是否显示菜单
        /// </summary>
        /// <param name="input">Id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemMenuLogic_Cache,ISystemPermissionLogic_Cache")]
        Task<OperateStatus> IsShowMenu(IdInput input);

        /// <summary>
        /// 是否具有模块权限
        /// </summary>
        /// <param name="input">Id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemMenuLogic_Cache,ISystemPermissionLogic_Cache")]
        Task<OperateStatus> HaveMenuPermission(IdInput input);

        /// <summary>
        /// 是否具有数据权限
        /// </summary>
        /// <param name="input">Id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemMenuLogic_Cache,ISystemPermissionLogic_Cache")]
        Task<OperateStatus> HaveDataPermission(IdInput input);

        /// <summary>
        /// 是否具有字段权限
        /// </summary>
        /// <param name="input">Id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemMenuLogic_Cache,ISystemPermissionLogic_Cache")]
        Task<OperateStatus> HaveFieldPermission(IdInput input);

        /// <summary>
        /// 是否具有功能项权限
        /// </summary>
        /// <param name="input">Id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemMenuLogic_Cache,ISystemPermissionLogic_Cache")]
        Task<OperateStatus> HaveButtonPermission(IdInput input);

        /// <summary>
        /// 冻结
        /// </summary>
        /// <param name="input">Id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemMenuLogic_Cache,ISystemPermissionLogic_Cache")]
        Task<OperateStatus> IsFreeze(IdInput input);
        #endregion

    }
}