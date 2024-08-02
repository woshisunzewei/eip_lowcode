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
using EIP.System.Models.Dtos.Data;

namespace EIP.System.Repository
{
    /// <summary>
    /// 数据日志
    /// </summary>
    public interface ISystemDataRepository 
    {
        /// <summary>
        /// 根据模块Id获取数据权限规则
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResults<SystemDataFindOutput>> Find(SystemDataFindInput input);

        /// <summary>
        /// 根据模块获取功能项信息
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SystemDataOutput>> FindHaveDataPermission(IdInput input);

        /// <summary>
        /// 根据模块Id获取数据权限规则
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SystemDataOutput>> FindDataByMenuId(IList<Guid> menuId = null);
    }
}