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
    /// ��¼��־ҵ���߼�
    /// </summary>
    public class SystemLoginLogLogic : DapperAsyncLogic<SystemLoginLog>, ISystemLoginLogLogic
    {
        #region ���캯��
        private readonly ISystemPermissionRepository _permissionRepository;
        private readonly ISystemLoginLogRepository _loginLogRepository;
        /// <summary>
        /// ��¼��־ҵ���߼�
        /// </summary>
        public SystemLoginLogLogic(ISystemLoginLogRepository loginLogRepository,
            ISystemPermissionRepository permissionRepository)
        {
            _loginLogRepository = loginLogRepository;
            _permissionRepository = permissionRepository;
        }

        #endregion

        #region ����

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
        /// ��ȡ��¼��־
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        public async Task<OperateStatus<PagedResults<SystemLoginLog>>> Find(SystemLoginLogFindPagingInput paging)
        {
            return OperateStatus<PagedResults<SystemLoginLog>>.Success(await _loginLogRepository.FindSystemLoginLog(paging));
        }

        /// <summary>
        /// ��ȡ��¼��־����
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemLoginLog>>> FindAnalysis(SystemLoginLogFindPagingInput paging)
        {
            paging.Size = 99999;
            paging.Sidx = "CreateTime";
            var data = (await _loginLogRepository.FindSystemLoginLog(paging)).Data;
            return OperateStatus<IEnumerable<SystemLoginLog>>.Success(data);
        }

        /// <summary>
        /// �˳�
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> LoginOut(IdInput input)
        {
            OperateStatus operateStatus = new OperateStatus();
            var loginLog = await FindAsync(f => f.LoginLogId == input.Id);
            if (loginLog != null)
            {
                loginLog.LoginOutTime = DateTime.Now;
                var timeSpan = (TimeSpan)(loginLog.LoginOutTime - loginLog.LoginTime.ToServerLocalTime());
                loginLog.StandingTime = timeSpan.TotalHours;
                return await UpdateAsync(loginLog);
            }
            return operateStatus;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<SystemLoginLog> FindById(IdInput input)
        {
            return await FindByIdAsync(input.Id);
        }

        /// <summary>
        /// ɾ��
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> Delete(IdInput<string> input)
        {
            return await DeleteByIdsAsync(input.Id);
        }
        #endregion
    }
}