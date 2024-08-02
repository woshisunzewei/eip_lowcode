/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/6/1 16:39:27
* 文件名: IBigScreenBizComponentLogic
* 描述: 业务组件表业务逻辑接口
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EasyCaching.Core.Interceptor;
using EIP.Base.Models.Entities.DataRoom;
using EIP.Big.Models.Dtos.ScreenBizComponent;
using EIP.Common.Logic;
using EIP.Common.Models;
using EIP.Common.Models.Dtos;
using EIP.Common.Models.Paging;
using EIP.DataRoom.Models.Dtos;

namespace EIP.DataRoom.Logic
{
    /// <summary>
    /// 业务组件表业务逻辑接口
    /// </summary>
    public interface IBigScreenBizComponentLogic : IAsyncLogic<BigScreenBizComponent>
    {
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "IBigScreenBizComponentLogic_Cache")]
        Task<OperateStatus<PagedResults<BigScreenBizComponentFindOutput>>> Find(BigScreenBizComponentFindInput input);
       
         /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IBigScreenBizComponentLogic_Cache")]
        Task<OperateStatus> Save(BigScreenBizComponentSaveInput entity);

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "IBigScreenBizComponentLogic_Cache")]
        Task<OperateStatus<BigScreenBizComponentInfoOutput>> Info(IdInput<string> input);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input">父级id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IBigScreenBizComponentLogic_Cache")]
        Task<OperateStatus> Delete(IdInput<string> input);

    }
}