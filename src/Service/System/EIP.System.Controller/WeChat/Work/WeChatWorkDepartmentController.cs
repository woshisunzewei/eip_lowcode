using EIP.System.Models.Dtos.OrganizationThree;

namespace EIP.System.Controller.WeChat.Work
{
    /// <summary>
    /// 企业微信部门
    /// </summary>
    public class WeChatWorkDepartmentController : BaseSystemController
    {
        #region 构造函数
        private readonly ISystemOrganizationThreeLogic _weChatWorkDepartmentLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="weChatWorkDepartmentLogic"></param>
        public WeChatWorkDepartmentController(ISystemOrganizationThreeLogic weChatWorkDepartmentLogic)
        {
            _weChatWorkDepartmentLogic = weChatWorkDepartmentLogic;
        }

        #endregion

        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("企业微信-方法-列表-获取部门列表", RemarkFrom.DingTalk)]
        [Route("/wechat/work/department/tree")]
        public ActionResult Tree()
        {
            return JsonForTree(_weChatWorkDepartmentLogic.Tree(3).Data);
        }
        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("企业微信-方法-获取部门列表", RemarkFrom.DingTalk)]
        [Route("/wechat/work/department/async")]
        public async Task<ActionResult> Async()
        {
            return Ok(await _weChatWorkDepartmentLogic.WeChatWorkAsync());
        }

        /// <summary>
        /// 同步组织到系统
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("企业微信-方法-同步组织到系统", RemarkFrom.DingTalk)]
        [Route("/wechat/work/department/async/system")]
        public async Task<ActionResult> AsyncToSystem()
        {
            return Ok(await _weChatWorkDepartmentLogic.AsyncToSystem(3));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("企业微信-方法-获取部门列表", RemarkFrom.DingTalk)]
        [Route("/wechat/work/department/list")]
        public async Task<ActionResult> FindFind(SystemOrganizationThreePagingInput input)
        {
            input.Type = 3;
            return JsonForGridPaging(await _weChatWorkDepartmentLogic.Find(input));
        }
    }
}
