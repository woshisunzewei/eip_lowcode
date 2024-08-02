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

namespace EIP.System.Repository.Log.IRepository
{
    /// <summary>
    /// 作业日志
    /// </summary>
    public interface ISystemJobLogRepository
    {
        /// <summary>
        /// 获取作业日志
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        Task<PagedResults<SystemJobLogFindPagingOutput>> FindSystemJobLog(SystemJobLogFindPagingInput paging);
    }
}