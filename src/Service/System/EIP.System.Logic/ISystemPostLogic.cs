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
using EIP.System.Models.Dtos.Post;
using EIP.System.Models.Dtos.Role;

namespace EIP.System.Logic
{
    /// <summary>
    /// 岗位
    /// </summary>
    public interface ISystemPostLogic : IAsyncLogic<SystemPost>
    {
        /// <summary>
        /// 根据组织机构获取岗位信息
        /// </summary>
        /// <param name="input">组织机构Id</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemPostLogic_Cache")]
        Task<OperateStatus<PagedResults<SystemPostFindOutput>>> Find(SystemPostFindInput input);

        /// <summary>
        /// 删除岗位信息
        /// </summary>
        /// <param name="input">岗位信息Id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemPostLogic_Cache")]
        Task<OperateStatus> Delete(IdInput<string> input);

        /// <summary>
        /// 保存岗位信息
        /// </summary>
        /// <param name="post">岗位信息</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemPostLogic_Cache")]
        Task<OperateStatus> Save(SystemPost post);

        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="input">复制信息</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemPostLogic_Cache")]
        Task<OperateStatus> Copy(SystemCopyInput input);

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="input">Id</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemPostLogic_Cache")]
        Task<OperateStatus<SystemPost>> FindById(IdInput input);

        /// <summary>
        /// 冻结
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemPostLogic_Cache")]
        Task<OperateStatus> IsFreeze(IdInput input);

        /// <summary>
        /// 获取所有
        /// </summary>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemPostLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemPost>>> All();

    }
}