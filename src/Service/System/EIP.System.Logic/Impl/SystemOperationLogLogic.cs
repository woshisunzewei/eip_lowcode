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
    /// ������־
    /// </summary>
    public class SystemOperationLogLogic : DapperAsyncLogic<SystemOperationLog>, ISystemOperationLogLogic
    {
        #region ���캯��
        private readonly ISystemOperationLogRepository _operationLogRepository;
        private readonly ISystemPermissionRepository _permissionRepository;
        /// <summary>
        /// ������־
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
        /// ��ȡ������־
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