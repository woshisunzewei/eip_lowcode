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
    /// �쳣��־
    /// </summary>
    public class SystemExceptionLogLogic : DapperAsyncLogic<SystemExceptionLog>, ISystemExceptionLogLogic
    {
        #region ���캯��

        private readonly ISystemExceptionLogRepository _exceptionLogRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="exceptionLogRepository"></param>
        public SystemExceptionLogLogic(ISystemExceptionLogRepository exceptionLogRepository)
        {
            _exceptionLogRepository = exceptionLogRepository;
        }

        /// <summary>
        /// �쳣��־
        /// </summary>
        /// <param name="paging"></param>
        /// <param name="excelReportDto"></param>
        /// <returns></returns>
        public async Task<OperateStatus> ReportExcelExceptionLogQuery(QueryParam paging,
            ExcelReportDto excelReportDto)
        {
            var operateStatus = new OperateStatus();
            try
            {
                //��ȡ�ܹ�����
                //paging.Rows =await Count();
                //��װ����
                //IList<SystemExceptionLog> dtos = (await _exceptionLogRepository.PagingQueryProcAsync(paging)).Data.ToList();
                var tables = new Dictionary<string, DataTable>(StringComparer.OrdinalIgnoreCase);
                //��װ��Ҫ��������
                //var dt = new DataTable("ExceptionLog");
                //dt.Columns.Add("Num");
                //dt.Columns.Add("OperateTime");
                //dt.Columns.Add("Code");
                //dt.Columns.Add("Message");
                //dt.Columns.Add("RequestUrl");
                //dt.Columns.Add("ClientHost");

                //var num = 1;
                //if (dtos.Any())
                //{
                //    foreach (var dto in dtos)
                //    {
                //        var row = dt.NewRow();
                //        dt.Rows.Add(row);
                //        row[0] = num;
                //        row[1] = dto.CreateTime;
                //        row[2] = dto.CreateUserCode;
                //        row[3] = dto.Message;
                //        row[4] = dto.RequestUrl;
                //        row[5] = dto.ClientHost;
                //        num++;
                //    }
                //}
                //tables.Add(dt.TableName, dt);
                //OpenXmlExcel.ExportExcel(excelReportDto.TemplatePath, excelReportDto.DownPath, tables);
                operateStatus.Code = ResultCode.Success;
            }
            catch (Exception)
            {
                operateStatus.Code = ResultCode.Error;
            }
            return operateStatus;
        }

        /// <summary>
        /// ��ȡ�쳣��־
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        public async Task<OperateStatus<PagedResults<SystemExceptionLog>>> FindSystemExceptionLog(SystemExceptionLogFindPagingInput paging)
        {
            return OperateStatus<PagedResults<SystemExceptionLog>>.Success(await _exceptionLogRepository.FindSystemExceptionLog(paging));
        }
        #endregion
    }
}