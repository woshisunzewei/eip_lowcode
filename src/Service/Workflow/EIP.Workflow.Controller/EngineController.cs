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

namespace EIP.Workflow.Controller
{
    /// <summary>
    /// 工作流引擎控制器
    /// </summary>
    public class EngineController : BaseWorkflowController
    {
        #region 构造函数        
        private readonly IWorkflowEngineLogic _workflowEngineLogic;
        private readonly IWorkflowArchivesLogic _workflowArchivesLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workflowEngineLogic"></param>
        /// <param name="workflowArchivesLogic"></param>
        public EngineController(
            IWorkflowEngineLogic workflowEngineLogic,
            IWorkflowArchivesLogic workflowArchivesLogic)
        {
            _workflowEngineLogic = workflowEngineLogic;
            _workflowArchivesLogic = workflowArchivesLogic;
        }
        #endregion

        #region 数据获取

        /// <summary>
        /// 获取所有流程
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-根据流程类型获取所有流程", RemarkFrom.Workflow)]
        [ProducesResponseType(typeof(WorkflowLibraryOutput), 1)]
        [Route("/workflow/engine/library")]
        public async Task<ActionResult> FindLibrary()
        {
            WorkflowLibraryInput input = new WorkflowLibraryInput();
            input.UserId = CurrentUser.UserId;
            return Ok(await _workflowEngineLogic.FindLibrary(input));
        }

        /// <summary>
        /// 启动流程调用此方法，生成流程实例，并置状态到开始节点之后的任务节点。
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-启动流程", RemarkFrom.Workflow)]
        [ProducesResponseType(typeof(WorkflowEngineFindActivityByProcessIdAndTypeOutput), 1)]
        [Route("/workflow/engine/activity/start/{id}")]
        public async Task<ActionResult> FindActivityByProcessIdAndType([FromRoute] IdInput input)
        {
            return Ok(await _workflowEngineLogic.FindActivityByProcessIdAndType(new WorkflowEngineFindActivityByProcessIdAndTypeInput()
            {
                ProcessId = input.Id,
                Type = EnumAcitvityType.开始
            }));
        }

        /// <summary>
        /// 根据活动Id获取活动信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-根据活动Id获取活动信息", RemarkFrom.Workflow)]
        [ProducesResponseType(typeof(WorkflowEngineFindActivityByTaskIdOutput), 1)]
        [Route("/workflow/activity/task/{id}")]
        public async Task<ActionResult> FindActivityByTaskId([FromRoute] IdInput input)
        {
            return Ok(await _workflowEngineLogic.FindActivityByTaskId(input));
        }
        /// <summary>
        /// 根据实例Id获取表单信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-根据实例Id获取表单信息", RemarkFrom.Workflow)]
        [ProducesResponseType(typeof(WorkflowEngineFindFormByProcessInstanceIdOutput), 1)]
        public async Task<ActionResult> FindWorkflowProcessFormByProcessInstanceId(IdInput input)
        {
            return Ok(await _workflowEngineLogic.FindWorkflowProcessFormByProcessInstanceId(input));
        }
        /// <summary>
        /// 根据ActivityId获取该活动配置的按钮。
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-根据ActivityId获取该活动配置的按钮", RemarkFrom.Workflow)]
        [ProducesResponseType(typeof(WorkflowButton), 1)]
        public async Task<ActionResult> FindProcessButtonByActivity(IdInput input)
        {
            return Ok(await _workflowEngineLogic.FindProcessButtonByActivity(input));
        }

        /// <summary>
        /// 获取流程实例按钮
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-根据ActivityId获取该活动配置的按钮", RemarkFrom.Workflow)]
        [ProducesResponseType(typeof(WorkflowButton), 1)]
        public async Task<ActionResult> FindProcessInstanceButtonByActivity(IdInput input)
        {
            return Ok(await _workflowEngineLogic.FindProcessInstanceButtonByActivity(input));
        }

        /// <summary>
        ///获取已处理任务
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-获取已处理任务", RemarkFrom.Workflow)]
        [ProducesResponseType(typeof(WorkflowEngineFindHaveDoOutput), 1)]
        [Route("/workflow/engine/havedo")]
        public async Task<ActionResult> FindHaveDo(WorkflowEngineFindHaveDoInput input)
        {
            input.UserId = CurrentUser.UserId;
            return Ok(await _workflowEngineLogic.FindHaveDo(input));
        }

        /// <summary>
        ///获取待处理任务
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-获取待处理任务", RemarkFrom.Workflow)]
        [ProducesResponseType(typeof(WorkflowEngineFindNeedDoOutput), 1)]
        [Route("/workflow/engine/needdo")]
        public async Task<ActionResult> FindNeedDo(WorkflowEngineFindNeedDoInput input)
        {
            input.UserId = CurrentUser.UserId;
            return Ok(await _workflowEngineLogic.FindNeedDo(input));
        }

        /// <summary>
        ///获取已发送流程
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-获取已发送流程", RemarkFrom.Workflow)]
        [ProducesResponseType(typeof(WorkflowEngineFindHaveSendOutput), 1)]
        [Route("/workflow/engine/havesend")]
        public async Task<ActionResult> FindHaveSend(WorkflowEngineFindHaveSendInput input)
        {
            input.UserId = CurrentUser.UserId;
            return Ok(await _workflowEngineLogic.FindHaveSend(input));
        }

        /// <summary>
        ///获取已超时流程
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-获取已超时流程", RemarkFrom.Workflow)]
        [ProducesResponseType(typeof(WorkflowEngineFindOverTimeOutput), 1)]
        [Route("/workflow/engine/overtime")]
        public async Task<ActionResult> FindOverTime(WorkflowEngineFindHaveSendInput input)
        {
            input.UserId = CurrentUser.UserId;
            return Ok(await _workflowEngineLogic.FindOverTime(input));
        }

        /// <summary>
        /// 获取流程数量
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-获取流程数量", RemarkFrom.Workflow)]
        [ProducesResponseType(typeof(WorkflowSearchNumOutput), 1)]
        public async Task<ActionResult> FindNum()
        {
            return Ok(await _workflowEngineLogic.FindNum(new IdInput(CurrentUser.UserId)));
        }

        /// <summary>
        ///获取流程督办
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-获取流程督办", RemarkFrom.Workflow)]
        [ProducesResponseType(typeof(WorkflowEngineFindHaveSendOutput), 1)]
        public async Task<ActionResult> FindSupervise(WorkflowEngineFindHaveSendInput input)
        {
            input.UserId = CurrentUser.UserId;
            return Ok(await _workflowEngineLogic.FindHaveSend(input));
        }

        /// <summary>
        ///获取流程督办
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-获取流程督办", RemarkFrom.Workflow)]
        public async Task<ActionResult> FindFlowChart()
        {
            return Ok(await _workflowEngineLogic.FindHaveSend(new WorkflowEngineFindHaveSendInput
            {
                UserId = CurrentUser.UserId
            }));
        }

        /// <summary>
        ///获取退回活动
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-获取退回活动", RemarkFrom.Workflow)]
        [ProducesResponseType(typeof(WorkflowEngineFindReturnActivityOutput), 1)]
        [Route("/workflow/engine/returnactivity")]
        public async Task<ActionResult> FindReturnActivity(WorkflowEngineFindReturnActivityInput input)
        {
            return Ok(await _workflowEngineLogic.FindReturnActivity(input));
        }

        /// <summary>
        ///获取流程实例过程
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-获取流程实例过程", RemarkFrom.Workflow)]
        [ProducesResponseType(typeof(WorkflowEngineFindInstanceProcessOutput), 1)]
        [Route("/workflow/engine/instanceprocess")]
        public async Task<ActionResult> FindInstanceProcess(IdInput input)
        {
            return Ok(await _workflowEngineLogic.FindInstanceProcess(new WorkflowEngineFindInstanceProcessInput
            {
                ProcessInstanceId = input.Id
            }));
        }

        #endregion

        #region 流程走向
        /// <summary>
        /// 保存到草稿箱 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-保存到草稿箱", RemarkFrom.Workflow)]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        [Route("/workflow/engine/draft")]
        public async Task<ActionResult> SaveDraft(WorkflowEngineStartProcessInput input)
        {
            input.CreateUserId = CurrentUser.UserId;
            input.CreateUserName = CurrentUser.Name;
            input.CreateUserOrganizationName = CurrentUser.OrganizationName;
            if (CurrentUser.OrganizationId != null)
                input.CreateUserOrganizationId = (Guid)CurrentUser.OrganizationId;
            return Ok(await _workflowEngineLogic.SaveDraft(input));
        }

        /// <summary>
        /// 保存到范本夹 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-保存到范本夹", RemarkFrom.Workflow)]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        [Route("/workflow/engine/model")]
        public async Task<ActionResult> SaveModel(WorkflowEngineStartProcessInput input)
        {
            input.CreateUserId = CurrentUser.UserId;
            input.CreateUserName = CurrentUser.Name;
            input.CreateUserOrganizationName = CurrentUser.OrganizationName;
            if (CurrentUser.OrganizationId != null)
                input.CreateUserOrganizationId = (Guid)CurrentUser.OrganizationId;
            return Ok(await _workflowEngineLogic.SaveModel(input));
        }

        /// <summary>
        /// 开始流程并返回下一步处理人员
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-启动流程", RemarkFrom.Workflow)]
        [ProducesResponseType(typeof(WorkflowEngineTaskProcessOutput), 1)]
        [Route("/workflow/engine/start")]
        public async Task<ActionResult> StartProcess(WorkflowEngineStartProcessInput input)
        {
            input.CreateUserId = CurrentUser.UserId;
            input.CreateUserName = CurrentUser.Name;
            input.CreateUserOrganizationName = CurrentUser.OrganizationName;
            if (CurrentUser.OrganizationId != null)
                input.CreateUserOrganizationId = (Guid)CurrentUser.OrganizationId;
            return Ok(await _workflowEngineLogic.StartProcess(input));
        }

        /// <summary>
        /// 运行开始流程
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-启动流程", RemarkFrom.Workflow)]
        [ProducesResponseType(typeof(WorkflowEngineProcessRunOutput), 1)]
        [Route("/workflow/engine/start/run")]
        public async Task<ActionResult> StartProcessRun(WorkflowEngineStartProcessRunInput input)
        {
            input.CreateUserId = CurrentUser.UserId;
            input.CreateUserName = CurrentUser.Name;
            input.CreateUserOrganizationName = CurrentUser.OrganizationName;
            if (CurrentUser.OrganizationId != null)
                input.CreateUserOrganizationId = (Guid)CurrentUser.OrganizationId;
            return Ok(await _workflowEngineLogic.StartProcessRun(input));
        }

        /// <summary>
        /// 任务扭转返回下一步处理人员
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-启动流程", RemarkFrom.Workflow)]
        [ProducesResponseType(typeof(WorkflowEngineTaskProcessOutput), 1)]
        [Route("/workflow/engine/taskprocess")]
        public async Task<ActionResult> TaskProcess(WorkflowEngineRunProcessInput input)
        {
            input.DoUserId = CurrentUser.UserId;
            input.DoUserName = CurrentUser.Name;
            input.DoUserOrganizationName = CurrentUser.OrganizationName;
            if (CurrentUser.OrganizationId != null)
                input.DoUserOrganizationId = (Guid)CurrentUser.OrganizationId;
            return Ok(await _workflowEngineLogic.TaskProcess(input));
        }

        /// <summary>
        /// 流程运行调用此方法，将当前任务结束，并分发任务给下一步节点的办理人。
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-流程运行", RemarkFrom.Workflow)]
        [ProducesResponseType(typeof(WorkflowEngineProcessRunOutput), 1)]
        [Route("/workflow/engine/taskprocess/run")]
        public async Task<ActionResult> TaskProcessRun(WorkflowEngineRunProcessInput input)
        {
            input.DoUserId = CurrentUser.UserId;
            input.DoUserName = CurrentUser.Name;
            input.DoUserOrganizationName = CurrentUser.OrganizationName;
            if (CurrentUser.OrganizationId != null)
                input.DoUserOrganizationId = (Guid)CurrentUser.OrganizationId;
            return Ok(await _workflowEngineLogic.TaskProcessRun(input));
        }

        /// <summary>
        /// 跳转到指定的任务节点，有预先指定方式，或运行时动态调用方式。
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-跳转到指定的任务节点", RemarkFrom.Workflow)]
        public /*async Task<>*/ ActionResult JumpProcess()
        {
            return null;
        }

        /// <summary>
        /// 当前任务节点的上一步节点完成人发现办理有误需撤销，调用此方法，重新回到上一步节点。
        /// 为发起人撤销已发出的申请提供了方法。
        /// 比如，张三提交了请假申请后又不想请假了。
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-当前任务节点的上一步节点完成人发现办理有误需撤销", RemarkFrom.Workflow)]
        [Route("/workflow/engine/revoke")]
        public async Task<ActionResult> RevokeProcess(WorkflowEngineRevokeProcessInput input)
        {
            input.DoUserId = CurrentUser.UserId;
            input.DoUserName = CurrentUser.Name;
            input.DoUserOrganizationName = CurrentUser.OrganizationName;
            if (CurrentUser.OrganizationId != null)
                input.DoUserOrganizationId = (Guid)CurrentUser.OrganizationId;
            return Ok(await _workflowEngineLogic.RevokeProcess(input));
        }

        /// <summary>
        /// 发起人撤销流程
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-发起人撤销流程", RemarkFrom.Workflow)]
        [Route("/workflow/engine/revoke/createuser")]
        public async Task<ActionResult> RevokeByCreateUser(WorkflowEngineRevokeByCreateUserInput input)
        {
            input.DoUserId = CurrentUser.UserId;
            input.DoUserName = CurrentUser.Name;
            input.DoUserOrganizationName = CurrentUser.OrganizationName;
            if (CurrentUser.OrganizationId != null)
                input.DoUserOrganizationId = (Guid)CurrentUser.OrganizationId;
            return Ok(await _workflowEngineLogic.RevokeByCreateUser(input));
        }
        /// <summary>
        /// 当前任务办理人退回任务到上一步执行节点。
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-当前任务办理人退回任务到上一步执行节点", RemarkFrom.Workflow)]
        public /*async Task<>*/ ActionResult SendbackProcess()
        {
            return null;
        }

        /// <summary>
        ///流程结束后仍需返回，由结束节点前的执行人调用此方法，状态回到结束前的节点。
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-流程结束后仍需返回", RemarkFrom.Workflow)]
        public /*async Task<>*/ ActionResult ReverseProcess()
        {
            return null;
        }

        /// <summary>
        ///知会他人了解任务。向知会人员发送通知并产生一个知会待办项，知会人员查看后任务消失，系统记录知会的发起和查看过程信息。
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-知会", RemarkFrom.Workflow)]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        [Route("/workflow/engine/understanding")]
        public async Task<ActionResult> UnderstandingProcess(WorkflowEngineUnderstandingProcessInput input)
        {
            input.DoUserId = CurrentUser.UserId;
            input.DoUserCode = CurrentUser.Code;
            input.DoUserName = CurrentUser.Name;
            return Ok(await _workflowEngineLogic.UnderstandingProcess(input));
        }

        /// <summary>
        ///知会已阅
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-知会已阅", RemarkFrom.Workflow)]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        [Route("/workflow/engine/understanding/read")]
        public async Task<ActionResult> UnderstandingReadProcess(WorkflowEngineUnderstandingReadProcessInput input)
        {
            input.DoUserId = CurrentUser.UserId;
            input.DoUserName = CurrentUser.Name;
            return Ok(await _workflowEngineLogic.UnderstandingReadProcess(input));
        }

        /// <summary>
        ///邀请他人对任务给出指导、发表意见。向邀请人员发送通知并产生一个阅示待办项，阅示人员批阅时需给出意见
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-邀请阅示", RemarkFrom.Workflow)]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        [Route("/workflow/engine/invitationread")]
        public async Task<ActionResult> InvitationReadProcess(WorkflowEngineInvitationReadInput input)
        {
            input.DoUserId = CurrentUser.UserId;
            input.DoUserName = CurrentUser.Name;
            return Ok(await _workflowEngineLogic.InvitationReadProcess(input));
        }

        /// <summary>
        ///邀请他人对任务给出指导、发表意见。向邀请人员发送通知并产生一个阅示待办项，阅示人员批阅时需给出意见
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-阅示已阅", RemarkFrom.Workflow)]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        [Route("/workflow/engine/invitationread/sure")]
        public async Task<ActionResult> InvitationReadProcessSure(WorkflowEngineInvitationReadSureInput input)
        {
            input.DoUserId = CurrentUser.UserId;
            input.DoUserName = CurrentUser.Name;
            return Ok(await _workflowEngineLogic.InvitationReadProcessSure(input));
        }

        /// <summary>
        ///邀请他人对任务给出指导、发表意见。向邀请人员发送通知并产生一个阅示待办项，阅示人员批阅时需给出意见
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-阅示已阅", RemarkFrom.Workflow)]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        [Route("/workflow/engine/invitationread/approve")]
        public async Task<ActionResult> InvitationReadProcessApprove(WorkflowEngineInvitationReadApproveInput input)
        {
            input.DoUserId = CurrentUser.UserId;
            input.DoUserName = CurrentUser.Name;
            return Ok(await _workflowEngineLogic.InvitationReadProcessApprove(input));
        }
        /// <summary>
        /// 批阅后审核通过
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-批阅后审核通过", RemarkFrom.Workflow)]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        [Route("/workflow/engine/invitationread/approve/pass")]
        public async Task<ActionResult> InvitationReadProcessApprovePass(WorkflowEngineInvitationReadApproveInput input)
        {
            input.DoUserId = CurrentUser.UserId;
            input.DoUserName = CurrentUser.Name;
            return Ok(await _workflowEngineLogic.InvitationReadProcessApprovePass(input));
        }

        /// <summary>
        /// 批阅后审核拒绝
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-批阅后审核通过", RemarkFrom.Workflow)]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        [Route("/workflow/engine/invitationread/approve/refuse")]
        public async Task<ActionResult> InvitationReadProcessApproveRefuse(WorkflowEngineInvitationReadApproveInput input)
        {
            input.DoUserId = CurrentUser.UserId;
            input.DoUserName = CurrentUser.Name;
            return Ok(await _workflowEngineLogic.InvitationReadProcessApproveRefuse(input));
        }

        /// <summary>
        ///将任务退回给发起人，发起人修改后可重新提交也可撤销申请。
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-退回重填", RemarkFrom.Workflow)]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        [Route("/workflow/engine/returnandwrite")]
        public async Task<ActionResult> ReturnAndWriteProcess(WorkflowEngineReturnAndWriteProcessInput input)
        {
            input.DoUserId = CurrentUser.UserId;
            input.DoUserName = CurrentUser.Name;
            return Ok(await (_workflowEngineLogic.ReturnAndWriteProcess(input)));
        }

        /// <summary>
        ///将任务退回给到指定活动
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-将任务退回给到指定活动", RemarkFrom.Workflow)]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async Task<ActionResult> ReturnProcess(WrofklowEngineReturnProcessInput input)
        {
            input.DoUserId = CurrentUser.UserId;
            input.DoUserName = CurrentUser.Name;
            return Ok(await (_workflowEngineLogic.ReturnProcess(input)));
        }
        /// <summary>
        /// 任务取消
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-任务取消", RemarkFrom.Workflow)]
        [ProducesResponseType(typeof(OperateStatus), 1)]

        public async Task<ActionResult> CancelProcess(WorkflowEngineRefuseProcessInput input)
        {
            input.DoUserId = CurrentUser.UserId;
            input.DoUserName = CurrentUser.Name;
            return Ok(await (_workflowEngineLogic.CancelProcess(input)));
        }
        /// <summary>
        ///拒绝任务
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-拒绝任务", RemarkFrom.Workflow)]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        [Route("/workflow/engine/refuse")]
        public async Task<ActionResult> RefuseProcess(WorkflowEngineRefuseProcessInput input)
        {
            input.DoUserId = CurrentUser.UserId;
            input.DoUserName = CurrentUser.Name;
            return Ok(await _workflowEngineLogic.RefuseProcess(input));
        }

        /// <summary>
        /// 召回,下级步骤所有都未处理时可进行召回操作,召回后可重新发起流程
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-召回", RemarkFrom.Workflow)]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async Task<ActionResult> RecallProcess(WorkflowEngineRecallProcessInput input)
        {
            input.DoUserId = CurrentUser.UserId;
            input.DoUserName = CurrentUser.Name;
            return Ok(await _workflowEngineLogic.RecallProcess(input));
        }

        /// <summary>
        /// 删除后的任务将被放入系统回收箱，管理员可以【恢复】或【彻底删除】回收箱中的任务。
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-删除", RemarkFrom.Workflow)]
        [Route("/workflow/engine/delete/task")]
        public async Task<ActionResult> DeleteByTaskId(WorkflowEngineDeleteByTaskIdInput input)
        {
            input.DoUserId = CurrentUser.UserId;
            input.DoUserName = CurrentUser.Name;
            input.DoUserOrganizationName = CurrentUser.OrganizationName;
            if (CurrentUser.OrganizationId != null)
                input.DoUserOrganizationId = (Guid)CurrentUser.OrganizationId;
            return Ok(await _workflowEngineLogic.DeleteByTaskId(input));
        }

        /// <summary>
        /// 删除后的任务将被放入系统回收箱，管理员可以【恢复】或【彻底删除】回收箱中的任务。
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-删除", RemarkFrom.Workflow)]
        public async Task<ActionResult> DeleteByProcessInstanceId(WorkflowEngineDeleteByProcessInstanceIdInput input)
        {
            input.DoUserId = CurrentUser.UserId;
            input.DoUserName = CurrentUser.Name;
            input.DoUserOrganizationName = CurrentUser.OrganizationName;
            if (CurrentUser.OrganizationId != null)
                input.DoUserOrganizationId = (Guid)CurrentUser.OrganizationId;
            return Ok(await _workflowEngineLogic.DeleteByProcessInstanceId(input));
        }

        /// <summary>
        ///【恢复】任务。
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-【恢复】任务。", RemarkFrom.Workflow)]
        public async Task<ActionResult> RecoveryDeleteByProcessInstanceId(IdInput<string> input)
        {
            return Ok(await _workflowEngineLogic.RecoveryDeleteByProcessInstanceId(input));
        }

        /// <summary>
        /// 直接结束流程
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("loader")]
        [Remark("流程维护-方法-直接结束流程", RemarkFrom.Workflow)]
        [Route("/workflow/engine/end")]
        public async Task<ActionResult> EndProcess(WorkflowEngineEndProcessInput input)
        {
            input.DoUserId = CurrentUser.UserId;
            input.DoUserName = CurrentUser.Name;
            return Ok(await _workflowEngineLogic.EndProcess(input));
        }

        /// <summary>
        ///【彻底删除】任务。
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-【彻底删除】任务。", RemarkFrom.Workflow)]
        public async Task<ActionResult> DeletePhysicsProcessInstanceId(IdInput<string> input)
        {
            var result = await _workflowEngineLogic.DeletePhysicsProcessInstanceId(input);

            return Ok(result);
        }

        /// <summary>
        ///任务自由调度。可以将任务向前或向后调度到已处理或未进入过的关卡，允许从多个正在执行的关卡调度到一个关卡
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-调度", RemarkFrom.Workflow)]
        public /*async Task<>*/ ActionResult DispatchProcess()
        {
            return null;
        }

        /// <summary>
        ///处理者可以在审批时临时增加签核人员
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-加签", RemarkFrom.Workflow)]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        [Route("/workflow/engine/add")]
        public async Task<ActionResult> AddProcess(WorkflowEngineAddProcessInput input)
        {
            input.DoUserId = CurrentUser.UserId;
            input.DoUserName = CurrentUser.Name;
            input.DoUserOrganizationName = CurrentUser.OrganizationName;
            if (CurrentUser.OrganizationId != null) input.DoUserOrganizationId = (Guid)CurrentUser.OrganizationId;
            return Ok(await _workflowEngineLogic.AddProcess(input));
        }

        /// <summary>
        ///处理者可以在审批时临时增加签核人员
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-加签", RemarkFrom.Workflow)]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        [Route("/workflow/engine/add/approve")]
        public async Task<ActionResult> AddApproveProcess(WorkflowEngineAddProcessInput input)
        {
            input.DoUserId = CurrentUser.UserId;
            input.DoUserName = CurrentUser.Name;
            input.DoUserOrganizationName = CurrentUser.OrganizationName;
            if (CurrentUser.OrganizationId != null) input.DoUserOrganizationId = (Guid)CurrentUser.OrganizationId;
            return Ok(await _workflowEngineLogic.AddApproveProcess(input));
        }
        #endregion

        #region 流程监控

        /// <summary>
        ///获取流程监控列表数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-获取待处理任务", RemarkFrom.Workflow)]
        [ProducesResponseType(typeof(WorkflowEngineFindMonitorListOuput), 1)]
        [Route("/workflow/engine/monitor")]
        public async Task<ActionResult> FindMonitorList(WorkflowEngineFindMonitorListInput input)
        {
            return JsonForGridPaging(await _workflowEngineLogic.FindMonitorList(input));
        }
        /// <summary>
        ///获取流程监控列表数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-获取待处理任务", RemarkFrom.Workflow)]
        [ProducesResponseType(typeof(WorkflowEngineFindMonitorListOuput), 1)]
        public async Task<ActionResult> FindMonitorDeleteList(WorkflowEngineFindMonitorDeleteListInput input)
        {
            return Ok(await _workflowEngineLogic.FindMonitorDeleteList(input));
        }

        /// <summary>
        ///获取流程监控列表数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-获取流程监控列表数据", RemarkFrom.Workflow)]
        [ProducesResponseType(typeof(WorkflowEngineFindMonitorFlowOutput), 1)]
        [Route("/workflow/engine/flowchart")]
        public async Task<ActionResult> FindMonitorFlowChart(IdInput input)
        {
            return Ok(await _workflowEngineLogic.FindMonitorFlowChart(input));
        }

        /// <summary>
        /// 根据活动实例Id获取流程详情
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-根据活动实例Id获取流程详情", RemarkFrom.Workflow)]
        [ProducesResponseType(typeof(WorkflowEngineFindMonitorDetailOutput), 1)]
        [Route("/workflow/engine/detail")]
        public async Task<ActionResult> FindMonitorDetail(IdInput input)
        {
            return Ok(await _workflowEngineLogic.FindMonitorDetail(input));
        }

        #endregion

        #region 草稿箱

        /// <summary>
        ///获取草稿箱信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-获取草稿箱信息", RemarkFrom.Workflow)]
        [ProducesResponseType(typeof(WorkflowEngineFindDraftOutput), 1)]
        [Route("/workflow/engine/draft/query")]
        public async Task<ActionResult> FindDraft(WorkflowEngineFindDraftInput input)
        {
            input.UserId = CurrentUser.UserId;
            return JsonForGridPaging(await _workflowEngineLogic.FindDraft(input));
        }

        /// <summary>
        ///删除草稿箱信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-删除草稿箱信息", RemarkFrom.Workflow)]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        [Route("/workflow/engine/draft/del")]
        public async Task<ActionResult> DeleteDraft(IdInput<string> input)
        {
            var result = await _workflowEngineLogic.DeleteDraft(input);
            return Ok(result);
        }

        #endregion

        #region 范本夹

        /// <summary>
        ///获取范本夹信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-获取待处理任务", RemarkFrom.Workflow)]
        [ProducesResponseType(typeof(WorkflowEngineFindModelOutput), 1)]
        [Route("/workflow/engine/model/query")]
        public async Task<ActionResult> FindModel(WorkflowEngineFindModelInput input)
        {
            input.UserId = CurrentUser.UserId;
            return JsonForGridPaging(await _workflowEngineLogic.FindModel(input));
        }

        /// <summary>
        ///删除范本夹信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-删除范本夹信息", RemarkFrom.Workflow)]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        [Route("/workflow/engine/model/del")]
        public async Task<ActionResult> DeleteModel(IdInput<string> input)
        {
            var result = await _workflowEngineLogic.DeleteModel(input);
            return Ok(result);
        }
        #endregion

        #region 归档信息
        /// <summary>
        ///保存归档信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-保存归档信息", RemarkFrom.Workflow)]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async Task<ActionResult> SaveArchive(WorkflowArchive input)
        {
            return Ok(await _workflowArchivesLogic.Save(input));
        }

        /// <summary>
        ///获取归档数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-获取归档数据", RemarkFrom.Workflow)]
        [ProducesResponseType(typeof(WorkflowArchiveOutput), 1)]
        public async Task<ActionResult> FindArchive(WorkflowArchiveInput input)
        {
            return Ok(await _workflowArchivesLogic.Find(input));
        }

        /// <summary>
        /// 获取Id获取归档数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-获取Id获取归档数据", RemarkFrom.Workflow)]
        public async Task<ActionResult> FindArchiveById(IdInput input)
        {
            return Ok(await _workflowArchivesLogic.FindByIdAsync(input.Id));
        }

        /// <summary>
        /// 获取Id获取归档流程图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-获取Id获取归档流程图", RemarkFrom.Workflow)]
        [ProducesResponseType(typeof(WorkflowEngineFindMonitorFlowOutput), 1)]
        [Route("/workflow/engine/archive")]
        public async Task<ActionResult> FindMonitorArchive(IdInput input)
        {
            return Ok(await _workflowEngineLogic.FindMonitorArchive(input));
        }

        /// <summary>
        /// 根据任务Id获取任务详情
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-根据任务Id获取任务详情", RemarkFrom.Workflow)]
        [Route("/workflow/engine/task/{id}")]
        public async Task<ActionResult> FindTaskById([FromRoute] IdInput input)
        {
            return Ok(await _workflowEngineLogic.FindTaskById(input));
        }
        #endregion

        #region 判断

        /// <summary>
        /// 连线检测条件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-连线检测条件", RemarkFrom.Workflow)]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async Task<ActionResult> CheckConditionLink(WorkflowEngineConditionInput input)
        {
            return Ok(await _workflowEngineLogic.CheckConditionLink(input));
        }
        #endregion

        #region 人员读取
        /// <summary>
        /// 根据ActivityId获取该活动加签审批人员。
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("loader")]
        [Remark("流程维护-方法-根据ActivityId获取该活动加签审批人员", RemarkFrom.Workflow)]
        [ProducesResponseType(typeof(WorkflowButton), 1)]
        public async Task<ActionResult> FindAddctivityChosenUser(Guid activityId)
        {
            return Ok(await _workflowEngineLogic.FindAddctivityChosenUser(activityId, CurrentUser.UserId));
        }
        #endregion
    }
}