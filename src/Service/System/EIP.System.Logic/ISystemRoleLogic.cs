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
using EIP.System.Models.Dtos.Role;

namespace EIP.System.Logic
{
    /// <summary>
    /// 角色
    /// </summary>
    public interface ISystemRoleLogic : IAsyncLogic<SystemRole>
    {
        /// <summary>
        /// 根据组织机构Id查询角色信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemRoleLogic_Cache")]
        Task<OperateStatus<PagedResults<SystemRolesFindOutput>>> Find(SystemRolesFindInput input);

        /// <summary>
        /// 下拉角色
        /// </summary>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemRoleLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemRoleSelectOutput>>> Select();

        /// <summary>
        /// 保存岗位信息
        /// </summary>
        /// <param name="role">岗位信息</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemRoleLogic_Cache")]
        Task<OperateStatus> Save(SystemRole role);

        /// <summary>
        /// 删除角色信息
        /// </summary>
        /// <param name="input">角色Id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemRoleLogic_Cache")]
        Task<OperateStatus> Delete(IdInput<string> input);

        /// <summary>
        /// 角色复制
        /// </summary>
        /// <param name="input">角色Id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemRoleLogic_Cache")]
        Task<OperateStatus> Copy(SystemCopyInput input);

        /// <summary>
        /// 获取所有角色
        /// </summary>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemRoleLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemRole>>> All();

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="input">角色Id</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemRoleLogic_Cache")]
        Task<OperateStatus<SystemRole>> FindById(IdInput input);

        /// <summary>
        /// 冻结
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemRoleLogic_Cache")]
        Task<OperateStatus> IsFreeze(IdInput input);
    }
}