/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2023/4/22 23:09:59
* 文件名: ISystemSmsconfigLogic
* 描述: 短信配置业务逻辑接口
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.System.Models.Dtos.Smsconfig;

namespace EIP.System.Logic
{
    /// <summary>
    /// 短信配置业务逻辑接口
    /// </summary>
    public interface ISystemSmsconfigLogic : IAsyncLogic<SystemSmsconfig>
    {
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemSmsConfigLogic_Cache")]
        Task<OperateStatus<PagedResults<SystemSmsconfigFindOutput>>> Find(SystemSmsconfigFindInput input);
       
        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemSmsConfigLogic_Cache")]
        Task<OperateStatus<SystemSmsconfig>> FindById(IdInput input);

         /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemSmsConfigLogic_Cache")]
        Task<OperateStatus> Save(SystemSmsconfig entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input">父级id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemSmsConfigLogic_Cache")]
        Task<OperateStatus> Delete(IdInput<string> input);

    }
}