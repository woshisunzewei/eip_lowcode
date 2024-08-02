using EIP.System.Models.Dtos.UserThree;

namespace EIP.System.Controller
{
    /// <summary>
    /// 钉钉用户
    /// </summary>
    public class SystemUserThreeController : BaseSystemController
    {
        #region 构造函数
        private readonly ISystemUserInfoThreeLogic _systemUserThreeLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dingTalkUserLogic"></param>
        public SystemUserThreeController(ISystemUserInfoThreeLogic dingTalkUserLogic)
        {
            _systemUserThreeLogic = dingTalkUserLogic;
        }

        #endregion
        /// <summary>
        /// 同步人员
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("钉钉-方法-列表-同步人员", RemarkFrom.DingTalk)]
        [Route("/dingtalk/user/async")]
        public async Task<ActionResult> Async()
        {
            return Ok(await _systemUserThreeLogic.DingTalkAsync());
        }

        /// <summary>
        /// 根据名称绑定用户(已在系统中导入用户)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("钉钉-方法-根据名称绑定用户(已在系统中导入用户)", RemarkFrom.DingTalk)]
        [Route("/dingtalk/user/bind")]
        public ActionResult Bind()
        {
            return Ok(_systemUserThreeLogic.Bind());
        }

        /// <summary>
        /// 同步用户到系统
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("钉钉-方法-列表-同步用户到系统", RemarkFrom.DingTalk)]
        [Route("/dingtalk/user/async/system")]
        public async Task<ActionResult> AsyncToSystem()
        {
            return Ok(await _systemUserThreeLogic.AsyncToSystem(4));
        }

        /// <summary>
        /// 转移到某个组织架构
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("钉钉-方法-列表-同步用户到系统", RemarkFrom.DingTalk)]
        [Route("/dingtalk/user/transfer/system")]
        public async Task<ActionResult> TransferToSystem(SystemUserThreeToSystemInput input)
        {
            return Ok(await _systemUserThreeLogic.TransferToSystem(input));
        }

        /// <summary>
        /// 绑定用户根据用户Id
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("钉钉-方法-列表-绑定用户根据用户Id", RemarkFrom.DingTalk)]
        [Route("/dingtalk/user/bind/userid")]
        public async Task<ActionResult> BindUserId(UserThreeBindInput input)
        {
            return Ok(await _systemUserThreeLogic.BindUserId(input));
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("钉钉-方法-列表-获取用户列表", RemarkFrom.DingTalk)]
        [Route("/dingtalk/user/list")]
        public async Task<ActionResult> Find(UserThreePagingInput input)
        {
            input.Type = 4;
            return JsonForGridPaging(await _systemUserThreeLogic.Find(input));
        }
    }
}
