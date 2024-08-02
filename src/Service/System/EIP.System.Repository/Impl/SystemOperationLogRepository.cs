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

namespace EIP.System.Repository.Impl
{
    /// <summary>
    /// ������־
    /// </summary>
    public class SystemOperationLogRepository :  ISystemOperationLogRepository
    {
        /// <summary>
        /// ��ȡ������־
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        public Task<PagedResults<SystemOperationLog>> FindPaging(SystemOperationLogFindPagingInput paging)
        {
            var sql = new StringBuilder("SELECT OperationLogId,RemoteIp,RemoteIpAddress,UserAgent,OsInfo,Browser,RequestContentLength,RequestType,Url,ControllerName,ActionName,ActionExecutionTime,ResultExecutionTime,ResponseStatus,Remark,RemarkFrom,CreateUserId,CreateUserCode,CreateUserName,CreateTime,@rowNumber, @recordCount  FROM System_OperationLog @where");
            if (paging.Sidx.IsNullOrEmpty())
            {
                paging.Sidx = " CreateTime";
            }
            return new SqlMapperUtil().PagingQuerySqlAsync<SystemOperationLog>(sql.ToString(), paging);
        }
    }
}