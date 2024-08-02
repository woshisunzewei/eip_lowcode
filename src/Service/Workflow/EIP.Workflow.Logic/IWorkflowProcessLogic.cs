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
namespace EIP.Workflow.Logic
{
    /// <summary>
    /// 工作流处理界面按钮接口定义
    /// </summary>
    public interface IWorkflowProcessLogic : IAsyncLogic<WorkflowProcess>
    {
        /// <summary>
        /// 根据流程类型获取流程信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "IWorkflowProcessLogic_Cache")]
        Task<OperateStatus<PagedResults<WorkflowProcessFindOutput>>> Find(WorkflowProcessFindInput input);

        /// <summary>
        /// 获取历史版本
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "IWorkflowProcessLogic_Cache")]
        Task<OperateStatus<IEnumerable<WorkflowProcess>>> FindVersion(IdInput input);

        /// <summary>
        /// 保存流程基本信息
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IWorkflowProcessLogic_Cache")]
        Task<OperateStatus> Save(WorkflowProcess process);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IWorkflowProcessLogic_Cache")]
        Task<OperateStatus> Delete(IdInput<string> input);

        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IWorkflowProcessLogic_Cache")]
        Task<OperateStatus> Copy(WorkflowProcess process);

        /// <summary>
        /// 保存流程设计器基本信息
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IWorkflowProcessLogic_Cache")]
        Task<OperateStatus> SaveDesignJson(WorkflowProcessSaveInput process);

        /// <summary>
        /// 获取所有流程
        /// </summary>
        [EasyCachingAble(CacheKeyPrefix = "IWorkflowProcessLogic_Cache")]
        Task<OperateStatus<IList<BaseTree>>> FindAll(IdInput input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "IWorkflowProcessLogic_Cache")]
        Task<OperateStatus<WorkflowProcessFindByIdOutput>> FindById(IdInput input);

        /// <summary>
        /// 保存缩略图
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IWorkflowProcessLogic_Cache")]
        Task<OperateStatus> SaveThumbnail(WorkflowProcessSaveThumbnailInput input);

    }
}