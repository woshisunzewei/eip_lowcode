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
namespace EIP.System.Repository
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public interface ISystemUserInfoRepository
    {

        /// <summary>
        /// 复杂查询分页方式
        /// </summary>
        /// <param name="input">查询参数</param>
        /// <returns>分页</returns>
        Task<PagedResults<SystemUserOutput>> Find(SystemUserPagingInput input);

        /// <summary>
        /// 用户公共信息
        /// </summary>
        /// <param name="input">查询参数</param>
        /// <returns>分页</returns>
        Task<IEnumerable<SystemUserFindCommonOutput>> FindCommon(SystemUserFindCommonInput input);

        /// <summary>
        /// 复杂查询分页方式
        /// </summary>
        /// <param name="dataSql">查询参数</param>
        /// <returns>分页</returns>
        Task<IEnumerable<SystemUserChosenOutput>> FindAllUser(string dataSql);
    }
}