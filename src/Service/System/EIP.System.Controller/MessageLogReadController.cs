using EIP.System.Models.Dtos.Message;
using System.ComponentModel;

namespace EIP.System.Controller
{
    /// <summary>
    /// 消息阅读记录表,记录那些人员什么时候已经查看
    /// </summary>
    public class MessageLogReadController : BaseSystemController
    {
        #region 构造函数
        private readonly ISystemMessageLogReadLogic _systemMessageLogReadLogic;

        /// <summary>
        /// 消息阅读记录表,记录那些人员什么时候已经查看构造函数
        /// </summary>
        /// <param name="systemMessageReadLogic"></param>
        public MessageLogReadController(ISystemMessageLogReadLogic systemMessageReadLogic)
        {
            _systemMessageLogReadLogic = systemMessageReadLogic;
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
        [Description("消息阅读记录表,记录那些人员什么时候已经查看-方法-列表-分页获取")]
        [Route("/system/messagelogread/list")]
        public async  Task<ActionResult> FindPaging(SystemMessageLogReadFindPagingInput input)
        {
            return JsonForGridPaging(await _systemMessageLogReadLogic.FindPaging(input));
        }

        /// <summary>
        /// 保存消息阅读状态
        /// </summary>
        /// <param name="input">主键信息</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("消息阅读记录表,记录那些人员什么时候已经查看-方法-编辑-保存消息阅读状态")]
        [Route("/system/messagelogread")]
        public async  Task<ActionResult> SaveRead(SystemMessageLogReadSaveInput input)
        {
            input.UserId = CurrentUser.UserId;
            input.UserName = CurrentUser.Name;
            return Ok(await _systemMessageLogReadLogic.SaveRead(input));
        }

        /// <summary>
        /// 删除已阅读信息
        /// </summary>
        /// <param name="input">主键集合</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("消息阅读记录表,记录那些人员什么时候已经查看-方法-列表-删除")]
        [Route("/system/messagelogread/delete")]
        public async  Task<ActionResult> DeleteHaveRead(SystemMessageLogDeleteHaveReadInput input)
        {
            input.UserId = CurrentUser.UserId;
            return Ok(await _systemMessageLogReadLogic.DeleteHaveRead(input));
        }
        #endregion
    }
}
