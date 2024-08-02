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
using EIP.System.Logic.Log.ILogic;
using EIP.System.Models.Dtos.Log;

namespace EIP.System.Logic.Impl
{
    /// <summary>
    /// 短信日志
    /// </summary>
    public class SystemSmsLogLogic : DapperAsyncLogic<SystemSmsLog>, ISystemSmsLogLogic
    {
        #region 构造函数
        private readonly ISystemSmsLogRepository _smsLogRepository;
        /// <summary>
        /// 短信日志
        /// </summary>
        public SystemSmsLogLogic(ISystemSmsLogRepository smsLogRepository)
        {
            _smsLogRepository = smsLogRepository;
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paging"></param>
        /// <param name="excelReportDto"></param>
        /// <returns></returns>
        public Task<OperateStatus> Report(QueryParam paging, ExcelReportDto excelReportDto)
        {
            throw new global::System.NotImplementedException();
        }

        /// <summary>
        /// 获取操作日志
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        public async Task<OperateStatus<PagedResults<SystemSmsLog>>> FindPaging(SystemSmsLogFindPagingInput paging)
        {
            return OperateStatus<PagedResults<SystemSmsLog>>.Success(await _smsLogRepository.FindPaging(paging));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<SystemSmsLog>> FindById(IdInput input)
        {
            return OperateStatus<SystemSmsLog>.Success(await FindAsync(f => f.SmsLogId == input.Id)); ;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> Delete(IdInput<string> input)
        {
            return await DeleteByIdsAsync(input.Id);
        }
    }
}