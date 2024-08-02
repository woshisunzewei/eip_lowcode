/**************************************************************
* Copyright (C) 2022 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2018/11/9 9:21:04
* 文件名: 
* 描述: 
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.System.Models.Dtos.Material;

namespace EIP.System.Logic
{
    /// <summary>
    /// 素材业务逻辑接口
    /// </summary>
    public interface ISystemMaterialLogic : IAsyncLogic<SystemMaterial>
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="paging"></param>
        [EasyCachingAble(CacheKeyPrefix = "ISystemMaterialLogic_Cache")]
        Task<OperateStatus<PagedResults<SystemMaterial>>> Find(SystemMaterialFindInput paging);

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="paging"></param>
        [EasyCachingAble(CacheKeyPrefix = "ISystemMaterialLogic_Cache")]
        Task<OperateStatus<SystemMaterial>> FindById(IdInput input);

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemMaterialLogic_Cache")]
        Task<OperateStatus> Save(SystemMaterialSaveInput input);

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemMaterialLogic_Cache")]
        Task<OperateStatus> Delete(IdInput<string> input);
    }
}