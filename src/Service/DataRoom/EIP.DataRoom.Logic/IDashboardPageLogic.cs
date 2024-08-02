/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/5/29 15:57:27
* 文件名: IDashboardPageLogic
* 描述: 页面基本信息表业务逻辑接口
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EasyCaching.Core.Interceptor;
using EIP.Base.Models.Entities.DataRoom;
using EIP.Common.Logic;
using EIP.Common.Models;
using EIP.Common.Models.Dtos;
using EIP.Common.Models.Paging;
using EIP.DataRoom.Models.Dtos;

namespace EIP.DataRoom.Logic
{
    /// <summary>
    /// 页面基本信息表业务逻辑接口
    /// </summary>
    public interface IDashboardPageLogic : IAsyncLogic<DashboardPage>
    {
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "IDashboardPageLogic_Cache")]
        Task<OperateStatus<PagedResults<DashboardPageFindOutput>>> Find(DashboardPageFindInput input);
       
        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "IDashboardPageLogic_Cache")]
        Task<OperateStatus<DashboardPage>> FindById(IdInput<int> input);

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "IDashboardPageLogic_Cache")]
        Task<OperateStatus<DashboardPermissionOutput>> Check(IdInput<string> input);

        Task<OperateStatus<DashboardChartOutput>> Chart(DashboardChartInput input);

        Task<OperateStatus<DashboardChartOutput>> List(DashboardChartInput input);

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IDashboardPageLogic_Cache")]
        Task<OperateStatus<string>> Save(DashboardDesignSaveInput entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input">父级id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IDashboardPageLogic_Cache")]
        Task<OperateStatus> Delete(IdInput<string> input);

    }
}