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

namespace EIP.System.Logic.Impl
{
    /// <summary>
    /// 操作日志
    /// </summary>
    public class SystemRateLimitLogLogic : DapperAsyncLogic<SystemRateLimitLog>, ISystemRateLimitLogLogic
    {
        #region 构造函数
        private readonly ISystemRateLimitLogRepository _rateLimitLogRepository;
        /// <summary>
        /// 操作日志
        /// </summary>
        public SystemRateLimitLogLogic(ISystemRateLimitLogRepository rateLimitLogRepository)
        {
            _rateLimitLogRepository = rateLimitLogRepository;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paging"></param>
        /// <param name="excelReportDto"></param>
        /// <returns></returns>
        public Task<OperateStatus> ReportExcelRateLimitLogQuery(QueryParam paging, ExcelReportDto excelReportDto)
        {
            throw new global::System.NotImplementedException();
        }

        /// <summary>
        /// 获取操作日志
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        public async Task<OperateStatus<PagedResults<SystemRateLimitLog>>> FindPagingRateLimitLog(SystemRateLimitLogFindPagingInput paging)
        {
            return OperateStatus<PagedResults<SystemRateLimitLog>>.Success(await _rateLimitLogRepository.FindPagingRateLimitLog(paging));
        }
    }
}