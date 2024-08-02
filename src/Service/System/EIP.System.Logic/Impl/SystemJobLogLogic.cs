/**************************************************************
* Copyright (C) 2018 www.sf-info.cn 盛峰版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 3365690)
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
using EIP.System.Logic.Log.ILogic;
using EIP.System.Models.Dtos.Log;
using EIP.System.Repository.Log.IRepository;

namespace EIP.System.Logic.Log.Logic
{
    /// <summary>
    /// 作业日志
    /// </summary>
    public class SystemJobLogLogic : DapperAsyncLogic<SystemJobLog>, ISystemJobLogLogic
    {
        #region 构造函数

        private readonly ISystemJobLogRepository _jobLogRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="jobLogRepository"></param>
        public SystemJobLogLogic(ISystemJobLogRepository jobLogRepository)
        {
            _jobLogRepository = jobLogRepository;
        }

        /// <summary>
        /// 作业日志
        /// </summary>
        /// <param name="paging"></param>
        /// <param name="excelReportDto"></param>
        /// <returns></returns>
        public async Task<OperateStatus> ReportExcelJobLogQuery(QueryParam paging,
            ExcelReportDto excelReportDto)
        {
            var operateStatus = new OperateStatus();
            try
            {
                //获取总共条数
                //paging.Rows =await Count();
                //组装数据
                //IList<SystemJobLog> dtos = (await _JobLogRepository.PagingQueryProcAsync(paging)).Data.ToList();
                var tables = new Dictionary<string, DataTable>(StringComparer.OrdinalIgnoreCase);
                //组装需要导出数据
                //var dt = new DataTable("JobLog");
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
        /// 获取作业日志
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        public async Task<OperateStatus<PagedResults<SystemJobLogFindPagingOutput>>> FindSystemJobLog(SystemJobLogFindPagingInput paging)
        {
            return OperateStatus<PagedResults<SystemJobLogFindPagingOutput>>.Success(await _jobLogRepository.FindSystemJobLog(paging));
        }
        #endregion
    }
}