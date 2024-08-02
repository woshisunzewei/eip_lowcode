/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
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
    /// 工作流意见控制器
    /// </summary>
    public class CommentController : BaseWorkflowController
    {
        #region 构造函数
        private readonly IWorkflowCommentLogic _workflowCommentLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="workflowCommentLogic"></param>
        public CommentController(IWorkflowCommentLogic workflowCommentLogic)
        {
            _workflowCommentLogic = workflowCommentLogic;
        }
        #endregion

        #region 方法
        /// <summary>
        /// 读取所有信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程意见-方法-列表-读取所有信息", RemarkFrom.Workflow)]
        [Route("/workflow/comment/list")]
        public async Task<ActionResult> Find(WorkflowCommentFindInput input)
        {
            if (input.Type.HasValue)
            {
                input.UserId = CurrentUser.UserId;
            }
            return JsonForGridPaging((await _workflowCommentLogic.Find(input)));
        }

        /// <summary>
        /// 读取所有信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程意见-方法-列表-读取所有信息", RemarkFrom.Workflow)]
        [Route("/workflow/comment/my")]
        public async Task<ActionResult> FindByUserId()
        {
            return Ok((await _workflowCommentLogic.FindByUserId(new IdInput { Id= CurrentUser.UserId })));
        }

        /// <summary>
        /// 根据id获取
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("流程意见-方法-根据id获取", RemarkFrom.Workflow)]
        [Route("/workflow/comment/{id}")]
        public async Task<ActionResult> FindById([FromRoute]IdInput input)
        {
            return Ok(await _workflowCommentLogic.FindById(input));
        }

        /// <summary>
        /// 保存配置信息值
        /// </summary>
        /// <param name="comment">配置项信息</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程意见-方法-新增/编辑-保存", RemarkFrom.Workflow)]
        [Route("/workflow/comment")]
        public async Task<ActionResult> Save(WorkflowComment comment)
        {
            return Ok(await _workflowCommentLogic.Save(comment));
        }

        /// <summary>
        /// 删除配置信息
        /// </summary>
        /// <param name="input">配置项主键Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程意见-方法-列表-删除", RemarkFrom.Workflow)]
        [Route("/workflow/comment/delete")]
        public async Task<ActionResult> Delete(IdInput<string> input)
        {
            return Ok(await _workflowCommentLogic.Delete(input));
        }
        #endregion
    }
}