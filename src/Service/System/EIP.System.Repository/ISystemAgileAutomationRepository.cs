/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/2/3 22:14:39
* 文件名: ISystemAgileAutomationRepository
* 描述: 自动化构建数据访问接口
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
    /// 自动化构建数据访问接口
    /// </summary>
    public interface ISystemAgileAutomationRepository 
    {
         /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResults<SystemAgileAutomationFindOutput>> Find(SystemAgileAutomationFindInput input);
    }
}