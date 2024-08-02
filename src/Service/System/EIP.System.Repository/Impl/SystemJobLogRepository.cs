/**************************************************************
* Copyright (C) 2018 www.sf-info.cn 盛峰版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 3365690)
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
using EIP.System.Repository.Log.IRepository;

namespace EIP.System.Repository.Log.Repository
{
    /// <summary>
    /// 作业日志
    /// </summary>
    public class SystemJobLogRepository : ISystemJobLogRepository
    {
        /// <summary>
        /// 获取作业日志
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        public Task<PagedResults<SystemJobLogFindPagingOutput>> FindSystemJobLog(SystemJobLogFindPagingInput paging)
        {
            var sql = new StringBuilder("SELECT Message,CreateTime,@rowNumber, @recordCount  FROM System_JobLog @where");
            if (paging.Correlation != null)
                sql.Append($" AND Correlation='{paging.Correlation}'");
            if (paging.Sidx.IsNullOrEmpty())
            {
                paging.Sidx = " CreateTime";
            }
            return new SqlMapperUtil().PagingQuerySqlAsync<SystemJobLogFindPagingOutput>(sql.ToString(), paging);
        }
    }
}