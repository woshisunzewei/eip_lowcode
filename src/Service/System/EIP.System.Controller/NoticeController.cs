using EIP.System.Models.Dtos.Notice;

namespace EIP.System.Controller
{
    /// <summary>
    /// 公告
    /// </summary>
    public class NoticeController : BaseSystemController
    {
        #region 构造函数
        private readonly ISystemNoticeLogic _systemNoticeLogic;
        /// <summary>
        /// 公告构造函数
        /// </summary>
        /// <param name="systemNoticeLogic"></param>
        public NoticeController(ISystemNoticeLogic systemNoticeLogic)
        {
            _systemNoticeLogic = systemNoticeLogic;
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
        [Remark("公告-方法-列表-分页获取", RemarkFrom.System)]
        [Route("/system/notice/list")]
        public async  Task<ActionResult> FindPaging(SystemNoticeFindPagingInput input)
        {
            return JsonForGridPaging(await _systemNoticeLogic.FindPaging(input));
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="input">主键信息</param>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("公告-方法-编辑-根据Id获取", RemarkFrom.System)]
        [Route("/system/notice/{id}")]
        public async  Task<ActionResult> FindById([FromRoute] IdInput input)
        {
            return Ok(await _systemNoticeLogic.FindById(input));
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="input">主键信息</param>
        /// <returns></returns>
        [HttpPost]

        [CreateBy("孙泽伟")]
        [Remark("公告-方法-编辑-保存", RemarkFrom.System)]
        [Route("/system/notice")]
        public async  Task<ActionResult> Save(SystemNotice input)
        {
            return Ok(await _systemNoticeLogic.Save(input));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input">主键集合</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("公告-方法-列表-删除", RemarkFrom.System)]
        [Route("/system/notice/delete")]
        public async  Task<ActionResult> Delete(IdInput<string> input)
        {
            return Ok(await _systemNoticeLogic.Delete(input));
        }
        #endregion
    }
}
