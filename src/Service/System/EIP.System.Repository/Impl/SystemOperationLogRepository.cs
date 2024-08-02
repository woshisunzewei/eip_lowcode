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

namespace EIP.System.Repository.Impl
{
    /// <summary>
    /// 操作日志
    /// </summary>
    public class SystemOperationLogRepository :  ISystemOperationLogRepository
    {
        /// <summary>
        /// 获取操作日志
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