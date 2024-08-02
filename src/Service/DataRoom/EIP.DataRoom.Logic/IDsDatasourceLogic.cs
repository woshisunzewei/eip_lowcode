/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/5/29 15:57:29
* 文件名: IDsDatasourceLogic
* 描述: 数据源配置表业务逻辑接口
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
using Microsoft.AspNetCore.Mvc;

namespace EIP.DataRoom.Logic
{
    /// <summary>
    /// 数据源配置表业务逻辑接口
    /// </summary>
    public interface IDsDatasourceLogic : IAsyncLogic<DsDatasource>
    {
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "IDsDatasourceLogic_Cache")]
        Task<OperateStatus<PagedResults<DsDatasourceFindOutput>>> Find(DsDatasourceFindInput input);

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "IDsDatasourceLogic_Cache")]
        Task<OperateStatus<List<DsDatasourceFindOutput>>> FindList(DataSourceListInput input);
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "IDsDatasourceLogic_Cache")]
        Task<OperateStatus<List<DataSourceGetTableListOutput>>> FindTableList(DataSourceGetTableListInput input);
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "IDsDatasourceLogic_Cache")]
        Task<OperateStatus<List<DataSourceGetFieldListOutput>>> FindFieldList(DataSourceGetFieldListInput input);

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IDsDatasourceLogic_Cache")]
        Task<OperateStatus> Save(DataSourceSaveInput entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input">父级id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IDsDatasourceLogic_Cache")]
        Task<OperateStatus> Delete(IdInput<string> input);

    }
}