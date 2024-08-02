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
using EasyCaching.Core.Interceptor;
using EIP.Base.Repository.Fixture;
using EIP.Workflow.Engine.Models.Dtos.Form;

namespace EIP.Workflow.Engine.Logic
{
    /// <summary>
    /// 工作流引擎接口
    /// </summary>
    public interface IWorkflowEngineLogic
    {
        #region 流程推算
        /// <summary>
        /// 根据实例Id获取对应活动信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<OperateStatus<IEnumerable<WorkflowEngineFindActivityByProcessIdAndTypeOutput>>> FindActivityByProcessIdAndType(WorkflowEngineFindActivityByProcessIdAndTypeInput input);

        /// <summary>
        /// 根据活动Id获取活动信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<OperateStatus<WorkflowEngineFindActivityByTaskIdOutput>> FindActivityByTaskId(IdInput input);

        /// <summary>
        /// 根据实例Id获取表单信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<WorkflowEngineFindFormByProcessInstanceIdOutput> FindWorkflowProcessFormByProcessInstanceId(IdInput input);

        /// <summary>
        ///    根据ActivityId获取该活动配置的按钮。
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<OperateStatus<IEnumerable<WorkflowButton>>> FindProcessButtonByActivity(IdInput input);

        /// <summary>
        /// 获取流程实例按钮
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<OperateStatus<IEnumerable<WorkflowButton>>> FindProcessInstanceButtonByActivity(IdInput input);

        /// <summary>
        /// 获取当前登录人员可以使用的流程库
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<OperateStatus<IEnumerable<WorkflowLibraryOutput>>> FindLibrary(WorkflowLibraryInput input);
        #endregion

        #region 流程走向
        /// <summary>
        /// 查找开始流程走向
        /// </summary>
        /// <param name="fixture"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<WorkflowEngineProcessInstanceOutput> FindWorkflowEngineProcessInstance(
            SqlDatabaseFixture fixture,
            WorkflowEngineStartProcessInput input);
        /// <summary>
        /// 开始流程并返回下一步处理人员
        /// </summary>
        /// <returns></returns>
        Task<OperateStatus<WorkflowEngineTaskProcessOutput>> StartProcess(WorkflowEngineStartProcessInput input);

        /// <summary>
        /// 运行开始流程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IAgileDataBaseLogic_Cache,ISystemDataBaseLogic_Cache")]
        Task<OperateStatus<WorkflowEngineProcessRunOutput>> StartProcessRun(WorkflowEngineStartProcessRunInput input);

        /// <summary>
        /// 保存到草稿箱
        /// </summary>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IAgileDataBaseLogic_Cache,ISystemDataBaseLogic_Cache")]
        Task<OperateStatus> SaveDraft(WorkflowEngineStartProcessInput input);

        /// <summary>
        /// 保存到范本夹
        /// </summary>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IAgileDataBaseLogic_Cache,ISystemDataBaseLogic_Cache")]
        Task<OperateStatus> SaveModel(WorkflowEngineStartProcessInput input);

        /// <summary>
        /// 得到下一步处理人员
        /// </summary>
        /// <returns></returns>
        Task<OperateStatus<WorkflowEngineProcessRunOutput>> TaskProcessRun(WorkflowEngineRunProcessInput input);

        /// <summary>
        /// 任务扭转返回下一步处理人员
        /// </summary>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IAgileDataBaseLogic_Cache,ISystemDataBaseLogic_Cache")]
        Task<OperateStatus<WorkflowEngineTaskProcessOutput>> TaskProcess(WorkflowEngineRunProcessInput input);

        /// <summary>
        /// 跳转到指定的任务节点，有预先指定方式，或运行时动态调用方式。
        /// </summary>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IAgileDataBaseLogic_Cache,ISystemDataBaseLogic_Cache")]
        Task<OperateStatus> JumpProcess(WorkflowEngineJumpProcessInput input);

        /// <summary>
        /// 当前任务节点的上一步节点完成人发现办理有误需撤销，调用此方法，重新回到上一步节点。
        /// </summary>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IAgileDataBaseLogic_Cache,ISystemDataBaseLogic_Cache")]
        Task<OperateStatus> RevokeProcess(WorkflowEngineRevokeProcessInput input);

        /// <summary>
        /// 发起人撤销流程
        /// </summary>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IAgileDataBaseLogic_Cache,ISystemDataBaseLogic_Cache")]
        Task<OperateStatus> RevokeByCreateUser(WorkflowEngineRevokeByCreateUserInput input);

        /// <summary>
        /// 当前任务办理人退回任务到上一步执行节点。
        /// </summary>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IAgileDataBaseLogic_Cache,ISystemDataBaseLogic_Cache")]
        Task<OperateStatus> SendbackProcess(WorkflowEngineSendbackProcessInput input);

        /// <summary>
        ///流程结束后仍需返回，由结束节点前的执行人调用此方法，状态回到结束前的节点。
        /// </summary>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IAgileDataBaseLogic_Cache,ISystemDataBaseLogic_Cache")]
        Task<OperateStatus> ReverseProcess();

        /// <summary>
        ///邀请他人对任务给出指导、发表意见。向邀请人员发送通知并产生一个阅示待办项，阅示人员批阅时需给出意见
        /// </summary>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IAgileDataBaseLogic_Cache,ISystemDataBaseLogic_Cache")]
        Task<OperateStatus> InvitationReadProcess(WorkflowEngineInvitationReadInput input);

        /// <summary>
        ///邀请他人对任务给出指导、发表意见。向邀请人员发送通知并产生一个阅示待办项，阅示人员批阅时需给出意见
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IAgileDataBaseLogic_Cache,ISystemDataBaseLogic_Cache")]
        Task<OperateStatus> InvitationReadProcessSure(WorkflowEngineInvitationReadSureInput input);

        /// <summary>
        ///邀请他人对任务给出指导、发表意见。向邀请人员发送通知并产生一个阅示待办项，阅示人员批阅时需给出意见
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IAgileDataBaseLogic_Cache,ISystemDataBaseLogic_Cache")]
        Task<OperateStatus> InvitationReadProcessApprove(WorkflowEngineInvitationReadApproveInput input);

        /// <summary>
        /// 批阅后审核通过
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IAgileDataBaseLogic_Cache,ISystemDataBaseLogic_Cache")]
        Task<OperateStatus> InvitationReadProcessApprovePass(WorkflowEngineInvitationReadApproveInput input);

        /// <summary>
        /// 批阅后审核拒绝
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IAgileDataBaseLogic_Cache,ISystemDataBaseLogic_Cache")]
        Task<OperateStatus> InvitationReadProcessApproveRefuse(WorkflowEngineInvitationReadApproveInput input);

        /// <summary>
        ///将任务退回给发起人，发起人修改后可重新提交也可撤销申请。
        /// </summary>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IAgileDataBaseLogic_Cache,ISystemDataBaseLogic_Cache")]
        Task<OperateStatus> ReturnAndWriteProcess(WorkflowEngineReturnAndWriteProcessInput input);

        /// <summary>
        /// 知会他人了解任务。向知会人员发送通知并产生一个知会待办项，知会人员查看后任务消失，系统记录知会的发起和查看过程信息。
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IAgileDataBaseLogic_Cache,ISystemDataBaseLogic_Cache")]
        Task<OperateStatus> UnderstandingProcess(WorkflowEngineUnderstandingProcessInput input);

        /// <summary>
        /// 知会已阅
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IAgileDataBaseLogic_Cache,ISystemDataBaseLogic_Cache")]
        Task<OperateStatus> UnderstandingReadProcess(WorkflowEngineUnderstandingReadProcessInput input);

        /// <summary>
        /// 将任务退回给到指定活动
        /// </summary>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IAgileDataBaseLogic_Cache,ISystemDataBaseLogic_Cache")]
        Task<OperateStatus> ReturnProcess(WrofklowEngineReturnProcessInput input);

        /// <summary>
        /// 将任务取消
        /// </summary>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IAgileDataBaseLogic_Cache,ISystemDataBaseLogic_Cache")]
        Task<OperateStatus> CancelProcess(WorkflowEngineRefuseProcessInput input);
        /// <summary>
        /// 任务拒绝
        /// </summary>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IAgileDataBaseLogic_Cache,ISystemDataBaseLogic_Cache")]
        Task<OperateStatus> RefuseProcess(WorkflowEngineRefuseProcessInput input);

        /// <summary>
        /// 任务召回
        /// </summary>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IAgileDataBaseLogic_Cache,ISystemDataBaseLogic_Cache")]
        Task<OperateStatus> RecallProcess(WorkflowEngineRecallProcessInput input);

        /// <summary>
        /// 处理者可以在审批时临时增加签核人员
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IAgileDataBaseLogic_Cache,ISystemDataBaseLogic_Cache")]
        Task<OperateStatus> AddProcess(WorkflowEngineAddProcessInput input);

        /// <summary>
        /// 加签审批
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IAgileDataBaseLogic_Cache,ISystemDataBaseLogic_Cache")]
        Task<OperateStatus> AddApproveProcess(WorkflowEngineAddProcessInput input);

        ///// <summary>
        /////使用撤销功能放弃申请。申请撤销后在系统中可见但所有相关审批将被回收，撤销者或管理员可以【恢复】被撤销的申请。
        ///// </summary>
        ///// <returns></returns>
        //Task<OperateStatus> RevokeProcess();

        /// <summary>
        ///删除后的任务将被放入系统回收箱，管理员可以【恢复】或【彻底删除】回收箱中的任务。
        /// </summary>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IAgileDataBaseLogic_Cache,ISystemDataBaseLogic_Cache")]
        Task<OperateStatus> DeleteByTaskId(WorkflowEngineDeleteByTaskIdInput input);

        /// <summary>
        ///删除后的任务将被放入系统回收箱，管理员可以【恢复】或【彻底删除】回收箱中的任务。
        /// </summary>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IAgileDataBaseLogic_Cache,ISystemDataBaseLogic_Cache")]
        Task<OperateStatus> DeleteByProcessInstanceId(WorkflowEngineDeleteByProcessInstanceIdInput input);

        /// <summary>
        /// 【彻底删除】任务。
        /// </summary>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IAgileDataBaseLogic_Cache,ISystemDataBaseLogic_Cache")]
        Task<OperateStatus<IList<WorkflowEngineFormDeleteDataInput>>> DeletePhysicsProcessInstanceId(IdInput<string> inputs);

        /// <summary>
        /// 【恢复】任务。
        /// </summary>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IAgileDataBaseLogic_Cache,ISystemDataBaseLogic_Cache")]
        Task<OperateStatus> RecoveryDeleteByProcessInstanceId(IdInput<string> input);

        /// <summary>
        ///任务自由调度。可以将任务向前或向后调度到已处理或未进入过的关卡，允许从多个正在执行的关卡调度到一个关卡
        /// </summary>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IAgileDataBaseLogic_Cache,ISystemDataBaseLogic_Cache")]
        Task<OperateStatus> DispatchProcess();

        /// <summary>
        /// 直接结束流程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IAgileDataBaseLogic_Cache,ISystemDataBaseLogic_Cache")]
        Task<OperateStatus> EndProcess(WorkflowEngineEndProcessInput input);

        /// <summary>
        /// 发送通知消息
        /// </summary>
        /// <param name="input"></param>
        /// <param name="trigger"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemMessageLogLogic_Cache,ISystemMessageLogReadLogic_Cache,IAgileDataBaseLogic_Cache")]
        Task SendWorkflowNotice(WorkflowEngineRunProcessInput input, EnumNoticeTrigger? trigger = EnumNoticeTrigger.下一步成功);
        #endregion

        #region 获取数据

        #region 待办事项
        /// <summary>
        ///获取待办事项
        /// </summary>
        /// <returns></returns>
        Task<OperateStatus<PagedResults<WorkflowEngineFindNeedDoOutput>>> FindNeedDo(WorkflowEngineFindNeedDoInput input);
        #endregion

        #region 已办事项
        /// <summary>
        /// 获取已办事项
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<OperateStatus<PagedResults<WorkflowEngineFindHaveDoOutput>>> FindHaveDo(WorkflowEngineFindHaveDoInput input);
        #endregion

        #region 流程督办


        #endregion

        #region 我的请求
        /// <summary>
        ///获取我的请求
        /// </summary>
        /// <returns></returns>
        Task<OperateStatus<PagedResults<WorkflowEngineFindHaveSendOutput>>> FindHaveSend(WorkflowEngineFindHaveSendInput input);

        #endregion

        #region 超时任务
        /// <summary>
        ///获取超时任务
        /// </summary>
        /// <returns></returns>
        Task<OperateStatus<PagedResults<WorkflowEngineFindOverTimeOutput>>> FindOverTime(WorkflowEngineFindHaveSendInput input);

        #endregion

        #region 获取流程数量
        /// <summary>
        ///获取流程数量
        /// </summary>
        /// <returns></returns>
        Task<WorkflowSearchNumOutput> FindNum(IdInput input);

        #endregion

        #region 流程代理

        #endregion

        #region 退回活动
        /// <summary>
        /// 退回活动
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<OperateStatus<IEnumerable<WorkflowEngineFindReturnActivityOutput>>> FindReturnActivity(WorkflowEngineFindReturnActivityInput input);
        #endregion

        #region 草稿箱
        /// <summary>
        /// 分页查询草稿箱
        /// </summary>
        /// <param name="paging">分页参数</param>
        /// <returns></returns>
        Task<OperateStatus<PagedResults<WorkflowEngineFindDraftOutput>>> FindDraft(WorkflowEngineFindDraftInput paging);

        /// <summary>
        /// 删除草稿
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<OperateStatus<IList<WorkflowEngineFormDeleteDataInput>>> DeleteDraft(IdInput<string> input);
        #endregion

        #region 范本夹
        /// <summary>
        /// 分页查询范本夹
        /// </summary>
        /// <param name="paging">分页参数</param>
        /// <returns></returns>
        Task<OperateStatus<PagedResults<WorkflowEngineFindModelOutput>>> FindModel(WorkflowEngineFindModelInput paging);

        /// <summary>
        /// 删除范本夹
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<OperateStatus<IList<WorkflowEngineFormDeleteDataInput>>> DeleteModel(IdInput<string> input);
        #endregion

        #endregion

        #region 流程监控

        /// <summary>
        ///根据实例Id和用户Id查询流程任务信息
        /// </summary>
        /// <returns></returns>
        Task<OperateStatus<WorkflowEngineFindTaskByProcessInstanceIdOutput>> FindTaskByProcessInstanceId(WorkflowEngineFindTaskByProcessInstanceIdInput input);

        /// <summary>
        /// 获取流程实例过程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<OperateStatus<IEnumerable<WorkflowEngineFindInstanceProcessOutput>>> FindInstanceProcess(WorkflowEngineFindInstanceProcessInput input);

        /// <summary>
        ///获取监控列表
        /// </summary>
        /// <returns></returns>
        Task<OperateStatus<PagedResults<WorkflowEngineFindMonitorListOuput>>> FindMonitorList(WorkflowEngineFindMonitorListInput input);
        /// <summary>
        ///获取监控列表
        /// </summary>
        /// <returns></returns>
        Task<PagedResults<WorkflowEngineFindMonitorDeleteListOuput>> FindMonitorDeleteList(WorkflowEngineFindMonitorDeleteListInput input);
        /// <summary>
        /// 获取流程图
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<OperateStatus<WorkflowEngineFindMonitorFlowOutput>> FindMonitorFlowChart(IdInput input);

        /// <summary>
        /// 获取流程图
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<WorkflowEngineFindMonitorFlowOutput> FindMonitorArchive(IdInput input);

        /// <summary>
        /// 根据流程Id获取任务信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<OperateStatus<WorkflowEngineFindByTaskIdOutput>> FindTaskById(IdInput input);

        /// <summary>
        /// 根据活动实例Id获取流程详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<OperateStatus<WorkflowEngineFindMonitorDetailOutput>> FindMonitorDetail(IdInput input);
        #endregion

        #region 处理人
        /// <summary>
        /// 获取加签处理人
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<WorkflowEngineFindAddctivityChosenUserOutput> FindAddctivityChosenUser(Guid activityId, Guid userId);
        #endregion

        #region 判断

        /// <summary>
        /// 连线检测条件
        /// </summary>
        /// <returns></returns>
        Task<OperateStatus> CheckConditionLink(WorkflowEngineConditionInput input);

        #endregion

        #region 事件
        /// <summary>
        /// 根据Api查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<object> EventDoByApi(WorkflowEngineEventDoByApiInput input);
        #endregion
    }
}
