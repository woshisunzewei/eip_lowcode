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
    /// 工作流按钮控制器
    /// </summary>
    public class ButtonController : BaseWorkflowController
    {
        #region 构造函数
        private readonly IWorkflowButtonLogic _workflowButtonLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="workflowButtonLogic"></param>
        public ButtonController(IWorkflowButtonLogic workflowButtonLogic)
        {
            _workflowButtonLogic = workflowButtonLogic;
        }
        #endregion

        #region 方法

        /// <summary>
        /// 读取所有信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程按钮-方法-列表-读取所有信息", RemarkFrom.Workflow)]
        [Route("/workflow/button/list")]
        public async Task<ActionResult> Find(WorkflowButtonFindInput input)
        {
            return JsonForGridPaging(await _workflowButtonLogic.Find(input));
        }

        /// <summary>
        /// 根据id获取
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("流程按钮-方法-根据id获取", RemarkFrom.Workflow)]
        [Route("/workflow/button/{id}")]
        public async Task<ActionResult> FindById([FromRoute]IdInput input)
        {
            return Ok(await _workflowButtonLogic.FindById(input));
        }

        /// <summary>
        /// 保存配置信息值
        /// </summary>
        /// <param name="button">配置项信息</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程按钮-方法-新增/编辑-保存", RemarkFrom.Workflow)]
        [Route("/workflow/button")]
        public async Task<ActionResult> Save( WorkflowButton button)
        {
            return Ok(await _workflowButtonLogic.Save(button));
        }

        /// <summary>
        /// 删除配置信息
        /// </summary>
        /// <param name="input">配置项主键Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程按钮-方法-列表-删除", RemarkFrom.Workflow)]
        [Route("/workflow/button/delete")]
        public async Task<ActionResult> Delete( IdInput<string> input)
        {
            return Ok(await _workflowButtonLogic.Delete(input));
        }
        #endregion
    }
}