/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/6/1 16:39:27
* 文件名: IBigScreenPagePreviewLogic
* 描述: 页面预览缓存表，每日定时删除业务逻辑接口
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EasyCaching.Core.Interceptor;
using EIP.Base.Models.Entities.DataRoom;
using EIP.Big.Models.Dtos.ScreenPagePreview;
using EIP.Common.Logic;
using EIP.Common.Models;
using EIP.Common.Models.Dtos;
using EIP.Common.Models.Paging;

namespace EIP.DataRoom.Logic
{
    /// <summary>
    /// 页面预览缓存表，每日定时删除业务逻辑接口
    /// </summary>
    public interface IBigScreenPagePreviewLogic : IAsyncLogic<BigScreenPagePreview>
    {
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "IBigScreenPagePreviewLogic_Cache")]
        Task<OperateStatus<PagedResults<BigScreenPagePreviewFindOutput>>> Find(BigScreenPagePreviewFindInput input);
       
         /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IBigScreenPagePreviewLogic_Cache")]
        Task<OperateStatus> Save(BigScreenPagePreview entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input">父级id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IBigScreenPagePreviewLogic_Cache")]
        Task<OperateStatus> Delete(IdInput<string> input);

    }
}