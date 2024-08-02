/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/6/1 16:54:21
* 文件名: IBigScreenPageRepository
* 描述: 页面基本信息表数据访问接口
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Common.Models.Paging;
using EIP.Big.Models.Dtos.ScreenPage;
using System.Threading.Tasks;

namespace EIP.DataRoom.Repository
{
	/// <summary>
    /// 页面基本信息表数据访问接口
    /// </summary>
    public interface IBigScreenPageRepository 
    {
         /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResults<BigScreenPageFindOutput>> Find(BigScreenPageFindInput input);
    }
}