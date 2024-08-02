using EIP.System.Models.Dtos.UserThree;

namespace EIP.System.Controller.WeChat.Work
{
    /// <summary>
    /// 企业微信用户
    /// </summary>
    public class SystemUserThreeController : BaseSystemController
    {
        #region 构造函数
        private readonly ISystemUserInfoThreeLogic _systemUserThreeLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="systemUserThreeLogic"></param>
        public SystemUserThreeController(ISystemUserInfoThreeLogic systemUserThreeLogic)
        {
            _systemUserThreeLogic = systemUserThreeLogic;
        }

        #endregion

        /// <summary>
        /// 同步人员
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("企业微信-方法-列表-同步人员", RemarkFrom.DingTalk)]
        [Route("/wechat/work/user/async")]
        public ActionResult Async()
        {
            return Ok(_systemUserThreeLogic.WeChatWorkAsync());
        }

        /// <summary>
        /// 获取所有标签
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("企业微信-方法-列表-同步人员", RemarkFrom.DingTalk)]
        [Route("/wechat/work/user/tag/list")]
        public ActionResult TagList()
        {
            return Ok(_systemUserThreeLogic.WeChatWorkTagList());
        }

        /// <summary>
        /// 同步人员根据标签
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("企业微信-方法-列表-同步人员", RemarkFrom.DingTalk)]
        [Route("/wechat/work/user/async/tag")]
        public ActionResult AsyncTag(int tagId)
        {
            return Ok(_systemUserThreeLogic.WeChatWorkAsyncTag(tagId));
        }
        /// <summary>
        /// 同步用户到系统
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("企业微信-方法-列表-同步用户到系统", RemarkFrom.DingTalk)]
        [Route("/wechat/work/user/async/system")]
        public async Task<ActionResult> AsyncToSystem()
        {
            return Ok(await _systemUserThreeLogic.AsyncToSystem(3));
        }

        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("企业微信-方法-列表-获取用户列表", RemarkFrom.DingTalk)]
        [Route("/wechat/work/user/list")]
        public async Task<ActionResult> Find(UserThreePagingInput input)
        {
            input.Type = 3;
            return JsonForGridPaging(await _systemUserThreeLogic.Find(input));
        }

        /// <summary>
        /// 绑定用户根据用户Id
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("企业微信-方法-列表-绑定用户根据用户Id", RemarkFrom.DingTalk)]
        [Route("/wechat/work/user/bind/userid")]
        public async Task<ActionResult> BindUserId(UserThreeBindInput input)
        {
            return Ok(await _systemUserThreeLogic.BindUserId(input));
        }
    }
}
