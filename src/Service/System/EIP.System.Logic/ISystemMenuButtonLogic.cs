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
using EIP.System.Models.Dtos.MenuButton;

namespace EIP.System.Logic
{
    /// <summary>
    /// 功能项业务逻辑
    /// </summary>
    public interface ISystemMenuButtonLogic : IAsyncLogic<SystemMenuButton>
    {
        /// <summary>
        /// 根据模块获取功能项信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemMenuButtonLogic_Cache")]
        Task<OperateStatus<PagedResults<SystemMenuButtonFindOutput>>> Find(SystemMenuButtonFindInput input);

        /// <summary>
        /// 保存功能项信息
        /// </summary>
        /// <param name="menuButton">功能项信息</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemMenuButtonLogic_Cache,ISystemPermissionLogic_Cache")]
        Task<OperateStatus> Save(SystemMenuButton menuButton);
        /// <summary>
        /// 保存功能项信息
        /// </summary>
        /// <param name="menuButton">功能项信息</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemMenuButtonLogic_Cache,ISystemPermissionLogic_Cache")]
        Task<OperateStatus> SaveAll(SystemMenuButtonSaveAllInput input);

        /// <summary>
        /// 删除功能项
        /// </summary>
        /// <param name="input">功能项信息</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemMenuButtonLogic_Cache,ISystemPermissionLogic_Cache")]
        Task<OperateStatus> Delete(IdInput<string> input);

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="input">Id</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemMenuButtonLogic_Cache")]
        Task<OperateStatus<SystemMenuButton>> FindById(IdInput input);

        /// <summary>
        /// 冻结
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemMenuButtonLogic_Cache,ISystemPermissionLogic_Cache")]
        Task<OperateStatus> IsFreeze(IdInput input);

        /// <summary>
        /// 是否显示到列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemMenuButtonLogic_Cache,ISystemPermissionLogic_Cache")]
        Task<OperateStatus> IsShowTable(IdInput input);
    }
}