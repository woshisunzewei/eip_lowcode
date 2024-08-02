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
    /// 限流日志
    /// </summary>
    public class SystemRateLimitLogRepository :  ISystemRateLimitLogRepository
    {
        /// <summary>
        /// 获取限流日志
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        public Task<PagedResults<SystemRateLimitLog>> FindPagingRateLimitLog(SystemRateLimitLogFindPagingInput paging)
        {
            var sql = new StringBuilder("SELECT RateLimitLogId,Message,RemoteIp,CreateUserCode,ControllerName,ActionName,RequestContentLength,RequestType,CreateUserName,Url,CreateTime,@rowNumber, @recordCount  FROM System_RateLimitLog @where");
            if (paging.Sidx.IsNullOrEmpty())
            {
                paging.Sidx = " CreateTime";
            }
            return new SqlMapperUtil().PagingQuerySqlAsync<SystemRateLimitLog>(sql.ToString(), paging);
        }
    }
}