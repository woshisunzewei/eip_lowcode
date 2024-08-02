/**************************************************************
* Copyright (C) 2022 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2022/01/12 22:40:15
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

namespace EIP.System.Logic.Impl
{
    /// <summary>
    /// 登录日志业务逻辑
    /// </summary>
    public class SystemLoginLogLogic : DapperAsyncLogic<SystemLoginLog>, ISystemLoginLogLogic
    {
        #region 构造函数
        private readonly ISystemPermissionRepository _permissionRepository;
        private readonly ISystemLoginLogRepository _loginLogRepository;
        /// <summary>
        /// 登录日志业务逻辑
        /// </summary>
        public SystemLoginLogLogic(ISystemLoginLogRepository loginLogRepository,
            ISystemPermissionRepository permissionRepository)
        {
            _loginLogRepository = loginLogRepository;
            _permissionRepository = permissionRepository;
        }

        #endregion

        #region 方法

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
        /// 获取登录日志
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        public async Task<OperateStatus<PagedResults<SystemLoginLog>>> Find(SystemLoginLogFindPagingInput paging)
        {
            return OperateStatus<PagedResults<SystemLoginLog>>.Success(await _loginLogRepository.FindSystemLoginLog(paging));
        }

        /// <summary>
        /// 获取登录日志分析
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
        /// 退出
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
        /// 删除
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