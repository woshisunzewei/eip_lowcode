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
    /// �쳣��־
    /// </summary>
    public interface ISystemExceptionLogLogic : IAsyncLogic<SystemExceptionLog>
    {
        /// <summary>
        /// Excel������ʽ
        /// </summary>
        /// <param name="paging">��ѯ����</param>
        /// <param name="excelReportDto"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemExceptionLogLogic_Cache")]
        Task<OperateStatus> ReportExcelExceptionLogQuery(QueryParam paging, ExcelReportDto excelReportDto);

        /// <summary>
        /// ��ȡ�쳣��־
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemExceptionLogLogic_Cache")]
        Task<OperateStatus<PagedResults<SystemExceptionLog>>> FindSystemExceptionLog(SystemExceptionLogFindPagingInput paging);
    }
}