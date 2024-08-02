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

namespace EIP.System.Repository
{
    /// <summary>
    /// 异常日志
    /// </summary>
    public interface ISystemExceptionLogRepository
    {
        /// <summary>
        /// 获取异常日志
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        Task<PagedResults<SystemExceptionLog>> FindSystemExceptionLog(SystemExceptionLogFindPagingInput paging);
    }
}