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
namespace EIP.System.Logic
{
    /// <summary>
    /// 敏捷开发业务逻辑接口
    /// </summary>
    public interface ISystemAgileDataLogLogic : IAsyncLogic<SystemAgileDataLog>
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="paging"></param>
        [EasyCachingAble(CacheKeyPrefix = "ISystemAgileDataLogLogic_Cache")]
        Task<OperateStatus<List<SystemAgileDataLogFindByRelationOutput>>> FindByRelationId(SystemAgileDataLogFindByRelationInput paging);

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemAgileDataLogLogic_Cache,ISystemMessageLogLogic_Cache")]
        Task<OperateStatus> Save(SystemAgileDataLog input);
    }
}