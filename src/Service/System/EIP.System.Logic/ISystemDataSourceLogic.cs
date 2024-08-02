/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2023/7/1 16:35:32
* 文件名: ISystemDatasourceLogic
* 描述: 数据源管理业务逻辑接口
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.System.Models.Dtos.Datasource;

namespace EIP.System.Logic
{
    /// <summary>
    /// 数据源管理业务逻辑接口
    /// </summary>
    public interface ISystemDatasourceLogic : IAsyncLogic<SystemDataSource>
    {
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemDataSourceLogic_Cache")]
        Task<OperateStatus<PagedResults<SystemDatasourceFindOutput>>> Find(SystemDatasourceFindInput input);
       
        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemDataSourceLogic_Cache")]
        Task<OperateStatus<SystemDataSource>> FindById(IdInput input);

         /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemDataSourceLogic_Cache,ISystemDataBaseLogic_Cache")]
        Task<OperateStatus> Save(SystemDataSource entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input">父级id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemDataSourceLogic_Cache")]
        Task<OperateStatus> Delete(IdInput<string> input);

    }
}