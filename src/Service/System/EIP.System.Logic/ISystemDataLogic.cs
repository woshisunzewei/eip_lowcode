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
using EIP.System.Models.Dtos.Data;

namespace EIP.System.Logic
{
    /// <summary>
    /// 数据权限业务逻辑
    /// </summary>
    public interface ISystemDataLogic : IAsyncLogic<SystemData>
    {
        /// <summary>
        /// 根据模块Id获取数据权限规则
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemDataLogic_Cache")]
        Task<OperateStatus<PagedResults<SystemDataFindOutput>>> Find(SystemDataFindInput input);

        /// <summary>
        /// 保存数据权限规则
        /// </summary>
        /// <param name="input">数据权限规则</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemDataLogic_Cache,ISystemPermissionLogic_Cache")]
        Task<OperateStatus> Save(SystemData input);

        /// <summary>
        /// 删除数据权限规则信息
        /// </summary>
        /// <param name="input">数据权限规则Id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemDataLogic_Cache,ISystemPermissionLogic_Cache")]
        Task<OperateStatus> Delete(IdInput<string> input);

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="input">Id</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemDataLogic_Cache")]
        Task<OperateStatus<SystemData>> FindById(IdInput input);

        /// <summary>
        /// 冻结
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemDataLogic_Cache")]
        Task<OperateStatus> IsFreeze(IdInput input);
    }
}