/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/2/3 22:14:39
* 文件名: ISystemAgileAutomationLogic
* 描述: 自动化构建业务逻辑接口
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
    /// 自动化构建业务逻辑接口
    /// </summary>
    public interface ISystemAgileAutomationLogic : IAsyncLogic<SystemAgileAutomation>
    {
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemAgileAutomationLogic_Cache")]
        Task<OperateStatus<PagedResults<SystemAgileAutomationFindOutput>>> Find(SystemAgileAutomationFindInput input);
       
        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemAgileAutomationLogic_Cache")]
        Task<OperateStatus<SystemAgileAutomation>> FindById(IdInput input);

        /// <summary>
        /// 冻结
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemAgileAutomationLogic_Cache")]
        Task<OperateStatus> IsFreeze(IdInput input);

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemAgileAutomationLogic_Cache")]
        Task<OperateStatus> Save(SystemAgileAutomation entity);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input">父级id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemAgileAutomationLogic_Cache")]
        Task<OperateStatus> Delete(IdInput<string> input);

        /// <summary>
        /// 保存表单设计器基本信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemAgileAutomationLogic_Cache")]
        Task<OperateStatus> SaveJson(SystemAgileAutomation input);

        /// <summary>
        /// 保存表单设计器基本信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemAgileAutomationLogic_Cache")]
        Task<OperateStatus> PublicJson(SystemAgileAutomation input);

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="paging"></param>
        [EasyCachingAble(CacheKeyPrefix = "ISystemAgileAutomationLogic_Cache")]
        Task<OperateStatus<List<SystemAgileAutomationFindEditOutput>>> FindTable();

        /// <summary>
        /// 表单触发自动化（通过日志触发）
        /// </summary>
        /// <param name="input"></param>
        /// <param name="contents"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemAgileAutomationLogic_Cache,ISystemDataBaseLogic_Cache,ISystemConfigLogic_Cache")]
        Task<OperateStatus> AautomationTable(SystemAgileDataLog input, List<SystemAgileDataLogContent> contents = null);

        /// <summary>
        /// 表单触发自动化（通过按钮触发）
        /// </summary>
        /// <param name="input"></param>
        /// <param name="contents"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemAgileAutomationLogic_Cache,ISystemDataBaseLogic_Cache,ISystemConfigLogic_Cache")]
        Task<OperateStatus> AautomationButton(SystemAgileAautomationButtonInput input);
    }
}