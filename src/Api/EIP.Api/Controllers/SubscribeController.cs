namespace EIP.Api.Controller
{
    /// <summary>
    /// 订阅
    /// </summary>
    public class SubscribeController : ControllerBase
    {
        private readonly IWorkflowEngineLogic _workflowEngineLogic;
        private readonly ISystemAgileDataLogLogic _agileDataLogLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="agileDataLogLogic"></param>
        /// <param name="workflowEngineLogic"></param>
        public SubscribeController(ISystemAgileDataLogLogic agileDataLogLogic, IWorkflowEngineLogic workflowEngineLogic)
        {
            _workflowEngineLogic = workflowEngineLogic;
            _agileDataLogLogic = agileDataLogLogic;
        }

        /// <summary>
        /// 消息订阅
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [NonAction]
        [CapSubscribe("MessageSubscribe")]
        public async Task MessageSubscribe(WorkflowEngineRunProcessInput input)
        {
            await _workflowEngineLogic.SendWorkflowNotice(input);
        }

        /// <summary>
        /// 保存敏捷开发数据日志
        /// </summary>
        /// <param name="input">主键信息</param>
        /// <returns></returns>
        [NonAction]
        [CapSubscribe("AgileDataLogSaveSubscribe")]
        public async Task<ActionResult> Save(SystemAgileDataLog input)
        {
            return Ok(await _agileDataLogLogic.Save(input));
        }

    }
}
