using EIP.System.Models.Dtos.Message;
using System.ComponentModel;

namespace EIP.System.Controller
{
    /// <summary>
    /// 消息记录表
    /// </summary>
    public class MessageLogController : BaseSystemController
    {
        #region 构造函数
        private readonly ISystemMessageLogLogic _systemMessageLogic;
        /// <summary>
        /// 消息记录表构造函数
        /// </summary>
        /// <param name="systemMessageLogic"></param>
        public MessageLogController(ISystemMessageLogLogic systemMessageLogic)
        {
            _systemMessageLogic = systemMessageLogic;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="input">分页参数</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("消息记录表-方法-列表-分页获取")]
        [Route("/system/messagelog/list")]
        public async  Task<ActionResult> FindPaging(SystemMessageLogFindPagingInput input)
        {
            input.UserId =CurrentUser.UserId;
            return JsonForGridPaging(await _systemMessageLogic.FindPaging(input));
        }

        /// <summary>
        /// 获取我的未读列表
        /// </summary>
        /// <param name="input">分页参数</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("消息记录表-方法-列表-分页获取")]
        [Route("/system/messagelog/list/my")]
        public async  Task<ActionResult> FindMyAllPaging(SystemMessageLogFindPagingInput input)
        {
            input.UserId = CurrentUser.UserId;
            return JsonForGridPaging(await _systemMessageLogic.FindMyAllPaging(input));
        }

        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="input">分页参数</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("消息记录表-方法-列表-分页获取")]
        public async  Task<ActionResult> FindMonitorPaging(SystemMessageLogFindMonitorPagingInput input)
        {
            return JsonForGridPaging(await _systemMessageLogic.FindMonitorPaging(input));
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="input">主键信息</param>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Description("消息记录表-方法-编辑-根据Id获取")]
        [Route("/system/messagelog/{id}")]
        public async  Task<ActionResult> FindById([FromRoute] IdInput input)
        {
            return Ok(await _systemMessageLogic.FindById(input));
        }

        #endregion
    }
}
