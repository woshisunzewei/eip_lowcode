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
using EIP.System.Models.Dtos.Log;

namespace EIP.System.Logic
{
    /// <summary>
    /// 操作日志
    /// </summary>
    public interface ISystemOperationLogLogic : IAsyncLogic<SystemOperationLog>
    {
        /// <summary>
        /// Excel导出方式
        /// </summary>
        /// <param name="paging">查询参数</param>
        /// <param name="excelReportDto"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemOperationLogLogic_Cache")]
        Task<OperateStatus> Report(QueryParam paging, ExcelReportDto excelReportDto);

        /// <summary>
        /// 获取操作日志
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemOperationLogLogic_Cache")]
        Task<OperateStatus<PagedResults<SystemOperationLog>>> FindPaging(SystemOperationLogFindPagingInput paging);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemOperationLogLogic_Cache")]
        Task<OperateStatus<SystemOperationLog>> FindById(IdInput input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemOperationLogLogic_Cache")]
        Task<OperateStatus> Delete(IdInput<string> input);
    }
}