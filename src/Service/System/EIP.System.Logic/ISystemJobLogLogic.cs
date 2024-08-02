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
using EIP.System.Models.Dtos.Log;

namespace EIP.System.Logic.Log.ILogic
{
    /// <summary>
    /// 作业日志
    /// </summary>
    public interface ISystemJobLogLogic : IAsyncLogic<SystemJobLog>
    {
        /// <summary>
        /// Excel导出方式
        /// </summary>
        /// <param name="paging">查询参数</param>
        /// <param name="excelReportDto"></param>
        /// <returns></returns>
        Task<OperateStatus> ReportExcelJobLogQuery(QueryParam paging, ExcelReportDto excelReportDto);

        /// <summary>
        /// 获取作业日志
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        Task<OperateStatus<PagedResults<SystemJobLogFindPagingOutput>>> FindSystemJobLog(SystemJobLogFindPagingInput paging);
    }
}