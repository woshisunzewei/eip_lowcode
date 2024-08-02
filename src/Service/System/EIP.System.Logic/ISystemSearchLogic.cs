/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/7/9 22:28:57
* 文件名: ISystemSearchLogic
* 描述: 业务逻辑接口
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.System.Models.Dtos.Search;

namespace EIP.System.Logic
{
    /// <summary>
    /// 业务逻辑接口
    /// </summary>
    public interface ISystemSearchLogic : IAsyncLogic<SystemSearch>
    {
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemSearchLogic_Cache")]
        Task<OperateStatus<PagedResults<SystemSearchFindOutput>>> Find(SystemSearchFindInput input);
       
        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemSearchLogic_Cache")]
        Task<OperateStatus<SystemSearch>> FindById(IdInput input);

         /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemSearchLogic_Cache")]
        Task<OperateStatus> Save(SystemSearch entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input">父级id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemSearchLogic_Cache")]
        Task<OperateStatus> Delete(IdInput<string> input);

    }
}