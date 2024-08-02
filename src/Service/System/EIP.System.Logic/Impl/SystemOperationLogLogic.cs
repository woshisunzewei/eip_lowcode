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
    /// 操作日志
    /// </summary>
    public class SystemOperationLogLogic : DapperAsyncLogic<SystemOperationLog>, ISystemOperationLogLogic
    {
        #region 构造函数
        private readonly ISystemOperationLogRepository _operationLogRepository;
        private readonly ISystemPermissionRepository _permissionRepository;
        /// <summary>
        /// 操作日志
        /// </summary>
        public SystemOperationLogLogic(ISystemOperationLogRepository operationLogRepository,
            ISystemPermissionRepository permissionRepository)
        {
            _operationLogRepository = operationLogRepository;
            _permissionRepository = permissionRepository;
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
        public async Task<OperateStatus<PagedResults<SystemOperationLog>>> FindPaging(SystemOperationLogFindPagingInput paging)
        {
            return OperateStatus<PagedResults<SystemOperationLog>>.Success(await _operationLogRepository.FindPaging(paging));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<SystemOperationLog>> FindById(IdInput input)
        {
            return OperateStatus<SystemOperationLog>.Success(await FindAsync(f => f.OperationLogId == input.Id));;
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