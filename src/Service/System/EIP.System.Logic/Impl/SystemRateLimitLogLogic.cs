/**************************************************************
* Copyright (C) 2022 www.eipflow.com ����ΰ��Ȩ����(����ؾ�)
*
* ����: ����ΰ(QQ 1039318332)
* ����ʱ��: 2022/01/12 22:40:15
* �ļ���: 
* ����: 
* 
* �޸���ʷ
* �޸��ˣ�
* ʱ�䣺
* �޸�˵����
*
**************************************************************/
using EIP.System.Models.Dtos.Log;

namespace EIP.System.Logic.Impl
{
    /// <summary>
    /// ������־
    /// </summary>
    public class SystemRateLimitLogLogic : DapperAsyncLogic<SystemRateLimitLog>, ISystemRateLimitLogLogic
    {
        #region ���캯��
        private readonly ISystemRateLimitLogRepository _rateLimitLogRepository;
        /// <summary>
        /// ������־
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
        /// ��ȡ������־
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        public async Task<OperateStatus<PagedResults<SystemRateLimitLog>>> FindPagingRateLimitLog(SystemRateLimitLogFindPagingInput paging)
        {
            return OperateStatus<PagedResults<SystemRateLimitLog>>.Success(await _rateLimitLogRepository.FindPagingRateLimitLog(paging));
        }
    }
}