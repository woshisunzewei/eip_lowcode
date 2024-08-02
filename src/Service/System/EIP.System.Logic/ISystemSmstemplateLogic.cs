/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2023/4/22 23:09:59
* 文件名: ISystemSmstemplateLogic
* 描述: 短信维护业务逻辑接口
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.System.Models.Dtos.Smstemplate;

namespace EIP.System.Logic
{
    /// <summary>
    /// 短信维护业务逻辑接口
    /// </summary>
    public interface ISystemSmstemplateLogic : IAsyncLogic<SystemSmstemplate>
    {
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemSmsTemplateLogic_Cache")]
        Task<OperateStatus<PagedResults<SystemSmstemplateFindOutput>>> Find(SystemSmstemplateFindInput input);
       
        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemSmsTemplateLogic_Cache")]
        Task<OperateStatus<SystemSmstemplate>> FindById(IdInput input);

         /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemSmsTemplateLogic_Cache")]
        Task<OperateStatus> Save(SystemSmstemplate entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input">父级id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemSmsTemplateLogic_Cache")]
        Task<OperateStatus> Delete(IdInput<string> input);

    }
}