/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/5/29 15:57:27
* 文件名: IDashboardPageRepository
* 描述: 页面基本信息表数据访问接口
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Common.Models.Paging;
using EIP.DataRoom.Models.Dtos;

namespace EIP.DataRoom.Repository
{
    /// <summary>
    /// 页面基本信息表数据访问接口
    /// </summary>
    public interface IDashboardPageRepository 
    {
         /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResults<DashboardPageFindOutput>> Find(DashboardPageFindInput input);
    }
}