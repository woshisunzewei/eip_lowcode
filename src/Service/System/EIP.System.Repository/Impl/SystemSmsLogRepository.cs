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
using EIP.System.Models.Dtos.Log;

namespace EIP.System.Repository.Impl
{
    /// <summary>
    /// 短信日志
    /// </summary>
    public class SystemSmsLogRepository :  ISystemSmsLogRepository
    {
        /// <summary>
        /// 获取短信日志
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        public Task<PagedResults<SystemSmsLog>> FindPaging(SystemSmsLogFindPagingInput paging)
        {
            var sql = new StringBuilder("SELECT SmsLogId,Phone,Message,Provide,Code,CreateTime,IsSend,@rowNumber, @recordCount  FROM System_SmsLog @where");
            if (paging.Sidx.IsNullOrEmpty())
            {
                paging.Sidx = " CreateTime";
            }
            return new SqlMapperUtil().PagingQuerySqlAsync<SystemSmsLog>(sql.ToString(), paging);
        }
    }
}