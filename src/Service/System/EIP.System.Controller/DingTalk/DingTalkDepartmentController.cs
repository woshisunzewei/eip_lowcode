using EIP.System.Models.Dtos.OrganizationThree;

namespace EIP.System.Controller
{
    /// <summary>
    /// 钉钉组织架构
    /// </summary>
    public class DingTalkDepartmentController : BaseSystemController
    {
        #region 构造函数
        private readonly ISystemOrganizationThreeLogic _dingTalkDepartmentLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dingTalkDepartmentLogic"></param>
        public DingTalkDepartmentController(ISystemOrganizationThreeLogic dingTalkDepartmentLogic)
        {
            _dingTalkDepartmentLogic = dingTalkDepartmentLogic;
        }

        #endregion

        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("钉钉-方法-列表-获取部门列表", RemarkFrom.DingTalk)]
        [Route("/dingtalk/department/tree")]
        public ActionResult Tree()
        {
            return JsonForTree((_dingTalkDepartmentLogic.Tree(4)).Data);
        }
        /// <summary>
        /// 获取部门列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("钉钉-方法-获取部门列表", RemarkFrom.DingTalk)]
        [Route("/dingtalk/department/async")]
        public async Task<ActionResult> Async()
        {
            return Ok(await _dingTalkDepartmentLogic.DingTalkAsync());
        }

        /// <summary>
        /// 同步组织到系统
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("钉钉-方法-同步组织到系统", RemarkFrom.DingTalk)]
        [Route("/dingtalk/department/async/system")]
        public async Task<ActionResult> AsyncToSystem()
        {
            return Ok(await _dingTalkDepartmentLogic.AsyncToSystem(4));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("钉钉-方法-获取部门列表", RemarkFrom.DingTalk)]
        [Route("/dingtalk/department/list")]
        public async Task<ActionResult> FindFind(SystemOrganizationThreePagingInput input)
        {
            input.Type = 4;
            return JsonForGridPaging(await _dingTalkDepartmentLogic.Find(input));
        }
    }
}
