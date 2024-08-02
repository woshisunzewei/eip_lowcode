/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2023/4/22 23:10:00
* 文件名: ISystemSmstemplateRepository
* 描述: 短信维护数据访问接口
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.System.Models.Dtos.Smstemplate;

namespace EIP.System.Repository
{
    /// <summary>
    /// 短信维护数据访问接口
    /// </summary>
    public interface ISystemSmstemplateRepository 
    {
         /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResults<SystemSmstemplateFindOutput>> Find(SystemSmstemplateFindInput input);
    }
}