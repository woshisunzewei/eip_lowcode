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
using EIP.System.Models.Dtos.UserLeader;

namespace EIP.System.Logic
{
    /// <summary>
    /// 用户业务逻辑
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
        /// 保存上级
        /// </summary>
        /// <param name="userId">人员Id等</param>
        /// <param name="userLeaders">所有领导Id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemUserLeaderLogic_Cache")]
        Task<OperateStatus> Save(SystemUserLeaderSaveInput input);

        /// <summary>
        /// 保存所有下级
        /// </summary>
        /// <param name="userId">人员Id等</param>
        /// <param name="userSubordinate">所有下级Id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemUserLeaderLogic_Cache")]
        Task<OperateStatus> SaveSubordinate(SystemUserLeaderSaveInput input);

        /// <summary>
        /// 获取我的下级
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemUserLeaderLogic_Cache")]
        Task<OperateStatus<IList<SystemUserLeadersOutput>>> FindSubordinate(IdInput input);
    }
}