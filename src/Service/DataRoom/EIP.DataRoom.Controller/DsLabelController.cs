/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/5/29 15:57:29
* 文件名: LabelController
* 描述: 标签控制器
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Common.Controller.Attribute;
using EIP.Common.Models;
using EIP.Common.Models.Dtos;
using EIP.Common.Models.Paging;
using EIP.DataRoom.Logic;
using EIP.DataRoom.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace EIP.DataRoom.Controller
{
    /// <summary>
    /// 标签
    /// </summary>
    public class LabelController : BaseDataRoomController
    {
        #region 构造函数
        private readonly IDsLabelLogic _dsLabelLogic;
        /// <summary>
        /// 标签构造函数
        /// </summary>
        /// <param name="dsLabelLogic"></param>
        public LabelController(IDsLabelLogic dsLabelLogic)
        {
            _dsLabelLogic = dsLabelLogic;
        }
        #endregion

        #region 方法
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("标签-方法-列表-获取分页", RemarkFrom.System)]
        [Route("/label/list")]
        [ProducesResponseType(typeof(OperateStatus<PagedResults<DsLabelFindOutput>>), 1)]
        public async Task<ActionResult> Find([FromQuery]DsLabelFindInput input)
        {
            return JsonForGridPagingDataRoom(await _dsLabelLogic.Find(input), input);
        }
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("标签-方法-列表-获取分页", RemarkFrom.System)]
        [Route("/label/getLabelList")]
        [ProducesResponseType(typeof(OperateStatus<List<DsLabelFindOutput>>), 1)]
        public async Task<ActionResult> FindLabelList()
        {
            return Ok(await _dsLabelLogic.FindLabelList());
        }
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("标签-方法-列表-获取分页", RemarkFrom.System)]
        [Route("/label/getLabelType")]
        [ProducesResponseType(typeof(OperateStatus<List<string>>), 1)]
        public async Task<ActionResult> FindLabelType()
        {
            return Ok(await _dsLabelLogic.FindLabelType());
        }
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("大屏、资源库、组件库分类-方法-列表-判断", RemarkFrom.System)]
        [Route("/label/checkRepeat")]
        [ProducesResponseType(typeof(OperateStatus<bool>), 1)]
        public ActionResult NameRepeat()
        {
            OperateStatus<bool> operateStatus = new OperateStatus<bool>();
            operateStatus.Code = ResultCode.Success;
            operateStatus.Msg = null;
            operateStatus.Data = false;
            return Ok(operateStatus);
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("大屏、资源库、组件库分类-方法-新增/编辑-保存", RemarkFrom.System)]
        [Route("/label/addOrUpdateLabel")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async Task<ActionResult> Save(DsLabelSaveInput entity)
        {
            return Ok(await _dsLabelLogic.Save(entity));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input">主键Id</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("标签-方法-列表-删除", RemarkFrom.System)]
        [Route("/label/removeLabel/{id}")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async Task<ActionResult> Delete([FromRoute]IdInput<string> input)
        {
            return Ok(await _dsLabelLogic.Delete(input));
        }
        #endregion
    }
}
