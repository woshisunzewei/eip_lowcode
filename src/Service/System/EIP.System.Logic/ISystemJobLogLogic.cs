/**************************************************************
* Copyright (C) 2018 www.sf-info.cn ʢ���Ȩ����(����ؾ�)
*
* ����: ����ΰ(QQ 3365690)
* ����ʱ��: 2018/10/30 22:40:15
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

namespace EIP.System.Logic.Log.ILogic
{
    /// <summary>
    /// ��ҵ��־
    /// </summary>
    public interface ISystemJobLogLogic : IAsyncLogic<SystemJobLog>
    {
        /// <summary>
        /// Excel������ʽ
        /// </summary>
        /// <param name="paging">��ѯ����</param>
        /// <param name="excelReportDto"></param>
        /// <returns></returns>
        Task<OperateStatus> ReportExcelJobLogQuery(QueryParam paging, ExcelReportDto excelReportDto);

        /// <summary>
        /// ��ȡ��ҵ��־
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        Task<OperateStatus<PagedResults<SystemJobLogFindPagingOutput>>> FindSystemJobLog(SystemJobLogFindPagingInput paging);
    }
}