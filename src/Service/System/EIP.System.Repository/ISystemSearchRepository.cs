/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/7/9 22:28:57
* 文件名: ISystemSearchRepository
* 描述: 数据访问接口
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.System.Models.Dtos.Search;

namespace EIP.System.Repository
{
    /// <summary>
    /// 数据访问接口
    /// </summary>
    public interface ISystemSearchRepository 
    {
         /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResults<SystemSearchFindOutput>> Find(SystemSearchFindInput input);
    }
}