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
    /// 限流日志
    /// </summary>
    public interface ISystemRateLimitLogLogic : IAsyncLogic<SystemRateLimitLog>
    {
        /// <summary>
        /// Excel导出方式
        /// </summary>
        /// <param name="paging">查询参数</param>
        /// <param name="excelReportDto"></param>
        /// <returns></returns>
        Task<OperateStatus> ReportExcelRateLimitLogQuery(QueryParam paging, ExcelReportDto excelReportDto);

        /// <summary>
        /// 获取限流日志
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        Task<OperateStatus<PagedResults<SystemRateLimitLog>>> FindPagingRateLimitLog(SystemRateLimitLogFindPagingInput paging);
    }
}