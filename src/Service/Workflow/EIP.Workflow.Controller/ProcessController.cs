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


namespace EIP.Workflow.Controller
{
    /// <summary>
    /// 工作流控制器
    /// </summary>
    public class ProcessController : BaseWorkflowController
    {
        #region 构造函数
        private readonly IWorkflowProcessLogic _workflowProcessLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workflowProcessLogic"></param>
        public ProcessController(IWorkflowProcessLogic workflowProcessLogic)
        {
            _workflowProcessLogic = workflowProcessLogic;
        }
        #endregion

        #region 方法
        /// <summary>
        /// 获取所有流程
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-根据流程类型获取所有流程", RemarkFrom.Workflow)]
        [Route("/workflow/process/list")]
        public async Task<ActionResult> Find(WorkflowProcessFindInput input)
        {
            return JsonForGridPaging(await _workflowProcessLogic.Find(input));
        }

        /// <summary>
        /// 获取所有流程
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-根据流程类型获取所有历史版本流程", RemarkFrom.Workflow)]
        public async Task<ActionResult> FindVersionWorkflow(IdInput input)
        {
            return Ok(await _workflowProcessLogic.FindVersion(input));
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-保存流程信息", RemarkFrom.Workflow)]
        [Route("/workflow/process")]
        public async Task<ActionResult> Save(WorkflowProcess process)
        {
            return Ok(await _workflowProcessLogic.Save(process));
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-保存流程信息", RemarkFrom.Workflow)]
        [Route("/workflow/process/copy")]
        public async Task<ActionResult> Copy(WorkflowProcess process)
        {
            process.CreateUserId = CurrentUser.UserId;
            process.CreateUserName = CurrentUser.Name;
            return Ok(await _workflowProcessLogic.Copy(process));
        }

        /// <summary>
        /// 保存流程设计Json
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-保存流程设计Json", RemarkFrom.Workflow)]
        [Route("/workflow/process/json")]
        public async Task<ActionResult> SaveDesignJson(WorkflowProcessSaveInput process)
        {
            process.UpdateTime = DateTime.Now;
            process.UpdateUserId = CurrentUser.UserId;
            process.UpdateUserName = CurrentUser.Name;
            return Ok(await _workflowProcessLogic.SaveDesignJson(process));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-删除流程信息", RemarkFrom.Workflow)]
        [Route("/workflow/process/delete")]
        public async Task<ActionResult> Delete(IdInput<string> input)
        {
            return Ok(await _workflowProcessLogic.Delete(input));
        }

        /// <summary>
        /// 根据id获取
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-根据id获取", RemarkFrom.Workflow)]
        [Route("/workflow/process/{id}")]
        public async Task<ActionResult> FindById([FromRoute] IdInput input)
        {
            return Ok(await _workflowProcessLogic.FindById(input));
        }

        /// <summary>
        /// 根据id获取获取预览数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("流程维护-方法-根据id获取获取预览数据", RemarkFrom.Workflow)]
        [Route("/workflow/process/preview/{id}")]
        public async Task<ActionResult> FindPreviewById([FromRoute] IdInput input)
        {
            return Ok(await _workflowProcessLogic.FindById(input));
        }

        /// <summary>
        /// 保存缩略图
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("敏捷开发-方法-保存缩略图", RemarkFrom.Workflow)]
        [Route("/workflow/process/thumbnail")]
        public async Task<ActionResult> SaveThumbnail(WorkflowProcessSaveThumbnailInput input)
        {
            return Ok(await _workflowProcessLogic.SaveThumbnail(input));
        }
        #endregion
    }
}