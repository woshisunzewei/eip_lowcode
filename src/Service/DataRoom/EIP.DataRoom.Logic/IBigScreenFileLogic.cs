/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/6/1 16:54:21
* 文件名: IBigScreenFileLogic
* 描述: 文件表业务逻辑接口
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EasyCaching.Core.Interceptor;
using EIP.Base.Models.Entities.DataRoom;
using EIP.Big.Models.Dtos.ScreenFile;
using EIP.Common.Logic;
using EIP.Common.Models;
using EIP.Common.Models.Dtos;
using EIP.Common.Models.Paging;

namespace EIP.DataRoom.Logic
{
    /// <summary>
    /// 文件表业务逻辑接口
    /// </summary>
    public interface IBigScreenFileLogic : IAsyncLogic<BigScreenFile>
    {
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "IBigScreenFileLogic_Cache")]
        Task<OperateStatus<PagedResults<BigScreenFileFindOutput>>> Find(BigScreenFileFindInput input);
       
         /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IBigScreenFileLogic_Cache")]
        Task<OperateStatus> Save(BigScreenFile entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input">父级id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IBigScreenFileLogic_Cache")]
        Task<OperateStatus> Delete(IdInput<string> input);

    }
}