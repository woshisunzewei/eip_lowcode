/**************************************************************
* Copyright (C) www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2018/10/30 22:40:15
* 文件名: 
* 描述: 
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Common.Models.Paging;

namespace EIP.Workflow.Engine.Repository
{
    /// <summary>
    /// 流程运行时
    /// </summary>
    public interface IWorkflowProcessInstanceRepository 
    {
        /// <summary>
        ///获取我的请求
        /// </summary>
        /// <returns></returns>
        Task<PagedResults<WorkflowEngineFindHaveSendOutput>> FindHaveSeed(WorkflowEngineFindHaveSendInput input);

        /// <summary>
        ///获取超时任务
        /// </summary>
        /// <returns></returns>
        Task<PagedResults<WorkflowEngineFindOverTimeOutput>> FindOverTime(WorkflowEngineFindHaveSendInput input);

        /// <summary>
        /// 获取待处理任务
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResults<WorkflowEngineFindNeedDoOutput>> FindNeedDo(WorkflowEngineFindNeedDoInput input);

        /// <summary>
        /// 获取流程数量
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<WorkflowSearchNumOutput> FindNum(IdInput input);

        /// <summary>
        /// 获取已处理
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResults<WorkflowEngineFindHaveDoOutput>> FindHaveDo(WorkflowEngineFindHaveDoInput input);

        /// <summary>
        ///获取监控列表
        /// </summary>
        /// <returns></returns>
        Task<PagedResults<WorkflowEngineFindMonitorListOuput>> FindMonitorList(WorkflowEngineFindMonitorListInput input);
        /// <summary>
        ///获取监控列表
        /// </summary>
        /// <returns></returns>
        Task<PagedResults<WorkflowEngineFindMonitorDeleteListOuput>> FindMonitorDeleteList(WorkflowEngineFindMonitorDeleteListInput input);
        /// <summary>
        /// 分页查询草稿箱
        /// </summary>
        /// <param name="paging">分页参数</param>
        /// <returns></returns>
        Task<PagedResults<WorkflowEngineFindDraftOutput>> FindDraft(WorkflowEngineFindDraftInput paging);

        /// <summary>
        /// 分页查询范本夹
        /// </summary>
        /// <param name="paging">分页参数</param>
        /// <returns></returns>
        Task<PagedResults<WorkflowEngineFindModelOutput>> FindModel(WorkflowEngineFindModelInput paging);

        /// <summary>
        ///根据实例Id获取活动信息
        /// </summary>
        /// <returns></returns>
        Task<WorkflowEngineFindMonitorDetailOutput> FindMonitorDetial(IdInput input);
    }
}
