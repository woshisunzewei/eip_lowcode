/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/6/1 16:39:27
* 文件名: IBigScreenBizComponentRepository
* 描述: 业务组件表数据访问接口
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Big.Models.Dtos.ScreenBizComponent;
using EIP.Common.Models.Paging;

namespace EIP.DataRoom.Repository
{
    /// <summary>
    /// 业务组件表数据访问接口
    /// </summary>
    public interface IBigScreenBizComponentRepository 
    {
         /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResults<BigScreenBizComponentFindOutput>> Find(BigScreenBizComponentFindInput input);
    }
}