/**************************************************************
* Copyright (C) www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2018/10/30 22:40:15
* 文件名: 
* 描述: 
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.System.Models.Dtos.ShortCut;

namespace EIP.System.Logic
{
    /// <summary>
    /// 系统快捷方式
    /// </summary>
    public interface ISystemShortCutLogic : IAsyncLogic<SystemShortCut>
	{
		/// <summary>
		/// 保存
		/// </summary>
		/// <param name="input">系统快捷方式</param>
		/// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemShortCutLogic_Cache")]
        Task<OperateStatus> Save(SystemShortCutSaveInput input);

        /// <summary>
        /// 保存排序号
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemShortCutLogic_Cache")]
        Task<OperateStatus> SaveOrderNo(IEnumerable<SystemShortCut> input);

        /// <summary>
        /// 根据用户Id获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemShortCutLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemShortCutFindByUserIdOutput>>> FindByUserId(SystemShortCutFindByUserIdInput input);

        /// <summary>
        /// 清空所有快捷模块
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemShortCutLogic_Cache")]
        Task<OperateStatus> DeleteAll(SystemShortCutDeleteAllInput input);

        /// <summary>
        /// 根据模块Id清除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemShortCutLogic_Cache")]
        Task<OperateStatus> Delete(SystemShortCut input);
    }
}
