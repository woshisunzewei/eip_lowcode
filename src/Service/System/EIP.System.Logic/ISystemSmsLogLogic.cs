/**************************************************************
* Copyright (C) www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2018/10/30 22:40:15
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

namespace EIP.System.Logic.Log.ILogic
{
    /// <summary>
    /// 短信日志
    /// </summary>
    public interface ISystemSmsLogLogic : IAsyncLogic<SystemSmsLog>
    {
        /// <summary>
        /// Excel导出方式
        /// </summary>
        /// <param name="paging">查询参数</param>
        /// <param name="excelReportDto"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemSmsLogLogic_Cache")]
        Task<OperateStatus> Report(QueryParam paging, ExcelReportDto excelReportDto);

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemSmsLogLogic_Cache")]
        Task<OperateStatus<PagedResults<SystemSmsLog>>> FindPaging(SystemSmsLogFindPagingInput paging);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemSmsLogLogic_Cache")]
        Task<OperateStatus<SystemSmsLog>> FindById(IdInput input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemSmsLogLogic_Cache")]
        Task<OperateStatus> Delete(IdInput<string> input);
    }
}