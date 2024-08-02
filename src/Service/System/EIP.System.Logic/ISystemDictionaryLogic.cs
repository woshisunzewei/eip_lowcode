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
using EIP.System.Models.Dtos.Dictionary;

namespace EIP.System.Logic
{
    /// <summary>
    /// 系统字典业务逻辑接口
    /// </summary>
    public interface ISystemDictionaryLogic : IAsyncLogic<SystemDictionary>
    {
        /// <summary>
        /// 查询所有字典信息
        /// </summary>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemDictionaryLogic_Cache")]
        Task<OperateStatus<IEnumerable<BaseTree>>> Tree();

        /// <summary>
        /// 根据父级查询下级(所有下级)
        /// </summary>
        /// <param name="input">父级id</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemDictionaryLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemDictionaryFindOutput>>> Find(SystemDictionaryFindInput input);

        /// <summary>
        /// 保存字典信息
        /// </summary>
        /// <param name="dictionary">字典信息</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemDictionaryLogic_Cache,ISystemDataBaseLogic_Cache")]
        Task<OperateStatus> Save(SystemDictionary input);

        /// <summary>
        /// 删除字典及下级数据
        /// </summary>
        /// <param name="input">父级id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemDictionaryLogic_Cache,ISystemDataBaseLogic_Cache")]
        Task<OperateStatus> Delete(IdInput<string> input);

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemDictionaryLogic_Cache")]
        Task<OperateStatus<SystemDictionaryEditOutput>> FindById(IdInput input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemDictionaryLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemDictionary>>> FindByParentId(IdInput input);

        /// <summary>
        /// 冻结
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemDictionaryLogic_Cache")]
        Task<OperateStatus> IsFreeze(IdInput input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemDictionaryLogic_Cache")]
        Task<OperateStatus<IEnumerable<BaseTree>>> FindByParentIds(SystemDictionaryFindByParentIdInput input);
    }
}