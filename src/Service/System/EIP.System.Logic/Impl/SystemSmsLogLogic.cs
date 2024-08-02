/**************************************************************
* Copyright (C) www.eipflow.com ����ΰ��Ȩ����(����ؾ�)
*
* ����: ����ΰ(QQ 1039318332)
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
using EIP.System.Logic.Log.ILogic;
using EIP.System.Models.Dtos.Log;

namespace EIP.System.Logic.Impl
{
    /// <summary>
    /// ������־
    /// </summary>
    public class SystemSmsLogLogic : DapperAsyncLogic<SystemSmsLog>, ISystemSmsLogLogic
    {
        #region ���캯��
        private readonly ISystemSmsLogRepository _smsLogRepository;
        /// <summary>
        /// ������־
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
        /// ��ȡ������־
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