/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2023/8/22 7:58:35
* 文件名: ISystemSerialNoRepository
* 描述: 编号规则数据访问接口
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.System.Models.Dtos.Serialno;

namespace EIP.System.Repository
{
    /// <summary>
    /// 编号规则数据访问接口
    /// </summary>
    public interface ISystemSerialNoRepository 
    {
         /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResults<SystemSerialNoFindOutput>> Find(SystemSerialNoFindInput input);
    }
}