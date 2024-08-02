/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/2/3 22:14:39
* 文件名: AgileAutomationController
* 描述: 自动化构建控制器
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
namespace EIP.System.Controller
{
    /// <summary>
    /// 自动化构建
    /// </summary>
    public class AgileAutomationController : BaseSystemController
    {
        #region 构造函数
        private readonly ISystemAgileAutomationLogic _systemAgileAutomationLogic;
		/// <summary>
        /// 自动化构建构造函数
        /// </summary>
        /// <param name="systemAgileAutomationLogic"></param>
        public AgileAutomationController(ISystemAgileAutomationLogic systemAgileAutomationLogic)
        {
            _systemAgileAutomationLogic = systemAgileAutomationLogic;
        }
        #endregion
		
        #region 方法

         /// <summary>
        /// 获取分页
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("自动化构建-方法-列表-获取分页", RemarkFrom.System)]
        [Route("/system/agileautomation/list")]
        [ProducesResponseType(typeof(OperateStatus<PagedResults<SystemAgileAutomationFindOutput>>), 1)]
        public async Task<ActionResult> Find(SystemAgileAutomationFindInput input)
        {
            return JsonForGridPaging(await _systemAgileAutomationLogic.Find(input));
        }

        /// <summary>
        /// 根据id获取
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("自动化构建-方法-根据id获取", RemarkFrom.System)]
        [Route("/system/agileautomation/{id}")]
        [ProducesResponseType(typeof(OperateStatus<SystemAgileAutomation>), 1)]
        public async Task<ActionResult> FindById([FromRoute]IdInput input)
        {
            return Ok(await _systemAgileAutomationLogic.FindById(input));
        }

        /// <summary>
        /// 冻结
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("界面按钮-方法-冻结", RemarkFrom.System)]
        [Route("/system/agileautomation/isfreeze")]
        public async Task<ActionResult> IsFreeze(IdInput input)
        {
            return Ok(await _systemAgileAutomationLogic.IsFreeze(input));
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("自动化构建-方法-新增/编辑-保存", RemarkFrom.System)]
        [Route("/system/agileautomation")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async Task<ActionResult> Save(SystemAgileAutomation entity)
        {
            return Ok(await _systemAgileAutomationLogic.Save(entity));
        }
        
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input">主键Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("自动化构建-方法-列表-删除", RemarkFrom.System)]
        [Route("/system/agileautomation/delete")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async Task<ActionResult> Delete( IdInput<string> input)
        {
            return Ok(await _systemAgileAutomationLogic.Delete(input));
        }

        /// <summary>
        /// 保存设计信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("自动化构建-方法-保存设计信息", RemarkFrom.System)]
        [Route("/system/agileautomation/save/json")]
        public async Task<ActionResult> SaveJson(SystemAgileAutomation input)
        {
            return Ok(await _systemAgileAutomationLogic.SaveJson(input));
        }

        /// <summary>
        /// 发布设计信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("自动化构建-方法-发布设计信息", RemarkFrom.System)]
        [Route("/system/agileautomation/public/json")]
        public async Task<ActionResult> Public(SystemAgileAutomation input)
        {
            return Ok(await _systemAgileAutomationLogic.PublicJson(input));
        }

        /// <summary>
        /// 获取工作表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("敏捷开发-方法-列表-获取所有编辑表单", RemarkFrom.System)]
        [Route("/system/agileautomation/table")]
        public async Task<ActionResult> FindTable()
        {
            return Ok(await _systemAgileAutomationLogic.FindTable());
        }

        /// <summary>
        /// 运行自动化
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("自动化构建-方法-运行自动化", RemarkFrom.System)]
        [Route("/system/agileautomation/run")]
        public async Task<ActionResult> Run(SystemAgileAautomationButtonInput input)
        {
            input.Authorization = Request.Headers["Authorization"];
            input.UserId = CurrentUser.UserId;
            input.UserCode = CurrentUser.Code;
            input.UserName = CurrentUser.Name;
            input.OrganizationName = CurrentUser.OrganizationName;
            if (CurrentUser.OrganizationId != null) input.OrganizationId = (Guid)CurrentUser.OrganizationId;
            return Ok(await _systemAgileAutomationLogic.AautomationButton(input));
        }
        #endregion
    }
}
