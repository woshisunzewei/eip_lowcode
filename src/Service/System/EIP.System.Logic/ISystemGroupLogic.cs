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
using EIP.System.Models.Dtos.Group;
using EIP.System.Models.Dtos.Role;

namespace EIP.System.Logic
{
    /// <summary>
    /// 组
    /// </summary>
    public interface ISystemGroupLogic : IAsyncLogic<SystemGroup>
    {
        /// <summary>
        /// 根据组织机构获取组信息
        /// </summary>
        /// <param name="input">组织机构</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemGroupLogic_Cache")]
        Task<OperateStatus<PagedResults<SystemGroupFindOutput>>> Find(SystemGroupFindInput input);

        /// <summary>
        /// 删除组信息
        /// </summary>
        /// <param name="input">组Id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemGroupLogic_Cache")]
        Task<OperateStatus> Delete(IdInput<string> input);

        /// <summary>
        /// 保存组信息
        /// </summary>
        /// <param name="group">岗位信息</param>
        /// <param name="belongTo">归属</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemGroupLogic_Cache")]
        Task<OperateStatus> Save(SystemGroup group,
            EnumGroupBelongTo belongTo);

        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="input">复制信息</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemGroupLogic_Cache")]
        Task<OperateStatus> Copy(SystemCopyInput input);

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="input">Id</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemGroupLogic_Cache")]
        Task<OperateStatus<SystemGroup>> FindById(IdInput input);

        /// <summary>
        /// 冻结
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemGroupLogic_Cache")]
        Task<OperateStatus> IsFreeze(IdInput input);

        /// <summary>
        /// 获取所有未冻结组
        /// </summary>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemGroupLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemGroup>>> All();
    }
}