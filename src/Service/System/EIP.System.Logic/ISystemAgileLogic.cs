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
    public interface ISystemAgileLogic : IAsyncLogic<SystemAgile>
    {
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="paging"></param>
        [EasyCachingAble(CacheKeyPrefix = "ISystemAgileLogic_Cache")]
        Task<OperateStatus<PagedResults<SystemAgileFindOutput>>> Find(SystemAgileFindInput paging);

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="paging"></param>
        [EasyCachingAble(CacheKeyPrefix = "ISystemAgileLogic_Cache")]
        Task<OperateStatus<SystemAgile>> FindById(IdInput input);

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="paging"></param>
        [EasyCachingAble(CacheKeyPrefix = "ISystemAgileLogic_Cache")]
        Task<OperateStatus<SystemAgile>> FindPublicJsonById(IdInput input,bool searchPublicJson=false);

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="paging"></param>
        [EasyCachingAble(CacheKeyPrefix = "ISystemAgileLogic_Cache")]
        Task<OperateStatus<SystemAgile>> FindColumnJsonJsonById(IdInput input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemAgileLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemAgileFindByMenuIdOutput>>> FindByMenuId(SystemAgileFindByMenuIdInput input);

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemAgileLogic_Cache,ISystemDataBaseLogic_Cache,ISystemAgileAutomationLogic_Cache")]
        Task<OperateStatus> Save(SystemAgile input);

        /// <summary>
        /// 冻结
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemAgileLogic_Cache,ISystemDataBaseLogic_Cache,ISystemAgileAutomationLogic_Cache")]
        Task<OperateStatus> IsFreeze(IdInput input);

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemAgileLogic_Cache,ISystemDataBaseLogic_Cache,ISystemAgileAutomationLogic_Cache")]
        Task<OperateStatus> SaveType(SystemAgile input);

        /// <summary>
        /// 保存表单设计器基本信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemAgileLogic_Cache,ISystemDataBaseLogic_Cache,ISystemAgileAutomationLogic_Cache")]
        Task<OperateStatus> SaveJson(SystemAgile input);

        /// <summary>
        /// 保存缩略图
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemAgileLogic_Cache")]
        Task<OperateStatus> SaveThumbnail(SystemAgile input);

        /// <summary>
        /// 保存表单设计器基本信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemAgileLogic_Cache,ISystemDataBaseLogic_Cache,ISystemPermissionLogic_Cache,ISystemAgileAutomationLogic_Cache")]
        Task<OperateStatus> PublicJson(SystemAgile input);

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemAgileLogic_Cache,ISystemDataBaseLogic_Cache,ISystemAgileAutomationLogic_Cache")]
        Task<OperateStatus> Delete(IdInput<string> input);

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="paging"></param>
        [EasyCachingAble(CacheKeyPrefix = "ISystemAgileLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemAgileFindBaseOutput>>> FindBase(SystemAgileFindBaseInput input);

        /// <summary>
        /// 根据表名获取配置
        /// </summary>
        /// <param name="paging"></param>
        [EasyCachingAble(CacheKeyPrefix = "ISystemAgileLogic_Cache")]
        Task<OperateStatus<SystemAgileFindColumnJsonDataFromNameOutput>> FindColumnJsonByDataFromName(SystemAgileFindColumnJsonDataFromNameInput input);
        
        /// <summary>
        /// 根据表名获取配置
        /// </summary>
        /// <param name="paging"></param>
        [EasyCachingAble(CacheKeyPrefix = "ISystemAgileLogic_Cache")]
        Task<OperateStatus<SystemAgileFindColumnJsonByMenuIdOutput>> FindColumnJsonByMenuId(SystemAgileFindColumnJsonByMenuIdInput input);

        /// <summary>
        /// 根据关键词查询相关发布设计内容
        /// </summary>
        /// <param name="paging"></param>
        [EasyCachingAble(CacheKeyPrefix = "ISystemAgileLogic_Cache")]
        Task<OperateStatus<List<SystemAgileFindByKeyOutput>>> Key(SystemAgileFindByKeyInput input);

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="input"></param>
        [EasyCachingAble(CacheKeyPrefix = "ISystemAgileLogic_Cache")]
        Task<OperateStatus<List<SystemAgileFindFormColumnsOutput>>> FindFormColumns(IdInput input);
    }
}