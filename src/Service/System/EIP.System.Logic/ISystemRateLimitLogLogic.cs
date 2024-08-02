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

namespace EIP.System.Logic
{
    /// <summary>
    /// ������־
    /// </summary>
    public interface ISystemRateLimitLogLogic : IAsyncLogic<SystemRateLimitLog>
    {
        /// <summary>
        /// Excel������ʽ
        /// </summary>
        /// <param name="paging">��ѯ����</param>
        /// <param name="excelReportDto"></param>
        /// <returns></returns>
        Task<OperateStatus> ReportExcelRateLimitLogQuery(QueryParam paging, ExcelReportDto excelReportDto);

        /// <summary>
        /// ��ȡ������־
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        Task<OperateStatus<PagedResults<SystemRateLimitLog>>> FindPagingRateLimitLog(SystemRateLimitLogFindPagingInput paging);
    }
}