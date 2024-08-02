/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/6/1 16:39:27
* 文件名: ScreenBizComponentController
* 描述: 业务组件表控制器
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Base.Models.Entities.DataRoom;
using EIP.Big.Models.Dtos.ScreenBizComponent;
using EIP.Big.Models.Dtos.ScreenPage;
using EIP.Common.Controller.Attribute;
using EIP.Common.Models;
using EIP.Common.Models.Dtos;
using EIP.Common.Models.Paging;
using EIP.DataRoom.Logic;
using EIP.DataRoom.Logic.Impl;
using EIP.DataRoom.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace EIP.DataRoom.Controller
{
    /// <summary>
    /// 业务组件表
    /// </summary>
    public class ScreenBizComponentController : BaseDataRoomController
    {
        #region 构造函数
        private readonly IBigScreenBizComponentLogic _bigScreenBizComponentLogic;
		/// <summary>
        /// 业务组件表构造函数
        /// </summary>
        /// <param name="bigScreenBizComponentLogic"></param>
        public ScreenBizComponentController(IBigScreenBizComponentLogic bigScreenBizComponentLogic)
        {
            _bigScreenBizComponentLogic = bigScreenBizComponentLogic;
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
        [Remark("页面基本信息表-方法-列表-获取分页", RemarkFrom.System)]
        [Route("/bigScreen/bizComponent/page")]
        [ProducesResponseType(typeof(OperateStatus<PagedResults<BigScreenBizComponentFindOutput>>), 1)]
        public async Task<ActionResult> Find([FromQuery] BigScreenBizComponentFindInput input)
        {
            return JsonForGridPagingDataRoom(await _bigScreenBizComponentLogic.Find(input), input);
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
        [Route("/bigScreen/bizComponent/update")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async Task<ActionResult> Update(BigScreenBizComponentSaveInput entity)
        {
            return Ok(await _bigScreenBizComponentLogic.Save(entity));
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
        [Route("/bigScreen/bizComponent/add")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async Task<ActionResult> Add(BigScreenBizComponentSaveInput entity)
        {
            return Ok(await _bigScreenBizComponentLogic.Save(entity));
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("大屏、资源库、组件库分类-方法-新增/编辑-保存", RemarkFrom.System)]
        [Route("/bigScreen/bizComponent/info/{id}")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async Task<ActionResult> Info([FromRoute] IdInput<string> input)
        {
            return Ok(await _bigScreenBizComponentLogic.Info(input));
        }
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("大屏、资源库、组件库分类-方法-列表-判断", RemarkFrom.System)]
        [Route("/bigScreen/bizComponent/name/repeat")]
        [ProducesResponseType(typeof(OperateStatus<PagedResults<DashboardTypeFindOutput>>), 1)]
        public ActionResult NameRepeat()
        {
            OperateStatus<bool> operateStatus = new OperateStatus<bool>();
            operateStatus.Code = ResultCode.Success;
            operateStatus.Msg = null;
            operateStatus.Data = false;
            return Ok(operateStatus);
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input">主键Id</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("业务组件表-方法-列表-删除", RemarkFrom.System)]
        [Route("/bigScreen/bizcomponent/delete/{id}")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async Task<ActionResult> Delete([FromRoute] IdInput<string> input)
        {
            return Ok(await _bigScreenBizComponentLogic.Delete(input));
        }

        #endregion
    }
}
