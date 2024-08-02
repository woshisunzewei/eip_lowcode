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
    /// 用户流程权限
    /// </summary>
    public class PermissionController : BaseWorkflowController
    {
        #region 构造函数
        private readonly IWorkflowPermissionLogic _workflowPermissionLogic;
        private readonly IWorkflowProcessLogic _workflowProcessLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="workflowPermissionLogic"></param>
        /// <param name="workflowProcessLogic"></param>
        public PermissionController(IWorkflowPermissionLogic workflowPermissionLogic,
            IWorkflowProcessLogic workflowProcessLogic)
        {
            _workflowPermissionLogic = workflowPermissionLogic;
            _workflowProcessLogic = workflowProcessLogic;
        }
        #endregion

        /// <summary>
        /// 保存配置信息值
        /// </summary>
        /// <param name="input">配置项信息</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程按钮-方法-新增/编辑-保存", RemarkFrom.Workflow)]
        [Route("/workflow/permission")]
        public async Task<ActionResult> Save(SystemPermissionSaveInput input)
        {
            input.Permissiones = JsonConvert.DeserializeObject<IList<SystemPermissionSaveConvertInput>>(input.MenuPermissions).Select(m => m.P).ToList();
            return Ok(await _workflowPermissionLogic.Save(input));
        }

        /// <summary>
        /// 获取所有流程权限
        /// </summary>
        /// <param name="input">配置项信息</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("流程按钮-方法-新增/编辑-保存", RemarkFrom.Workflow)]
        [Route("/workflow/permission/find")]
        public async Task<ActionResult> Find(SystemPermissionByPrivilegeMasterValueInput input)
        {
            return Ok(await _workflowPermissionLogic.Find(input));
        }

        /// <summary>
        /// 获取所有流程实例
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("流程按钮-方法-获取所有流程实例", RemarkFrom.Workflow)]
        [Route("/workflow/permission")]
        public async Task<ActionResult> FindAll()
        {
            IdInput input = new IdInput()
            {
               
            };
            return JsonForTree((await _workflowProcessLogic.FindAll(input)).Data);
        }
    }
}
