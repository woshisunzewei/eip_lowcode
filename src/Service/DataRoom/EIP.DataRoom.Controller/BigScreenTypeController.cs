/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/6/1 16:54:21
* 文件名: ScreenTypeController
* 描述: 大屏、资源库、组件库分类控制器
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Base.Models.Entities.DataRoom;
using EIP.Big.Models.Dtos.ScreenType;
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
    /// 大屏、资源库、组件库分类
    /// </summary>
    public class ScreenTypeController : BaseDataRoomController
    {
        #region 构造函数
        private readonly IBigScreenTypeLogic _bigScreenTypeLogic;
		/// <summary>
        /// 大屏、资源库、组件库分类构造函数
        /// </summary>
        /// <param name="bigScreenTypeLogic"></param>
        public ScreenTypeController(IBigScreenTypeLogic bigScreenTypeLogic)
        {
            _bigScreenTypeLogic = bigScreenTypeLogic;
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
        [Remark("大屏、资源库、组件库分类-方法-列表-获取分页", RemarkFrom.System)]
        [Route("/bigScreen/type/list/bigScreenCatalog")]
        [ProducesResponseType(typeof(OperateStatus<PagedResults<DashboardTypeFindOutput>>), 1)]
        public async Task<ActionResult> FindCatalog()
        {
            return Ok(await _bigScreenTypeLogic.FindType("bigScreenCatalog"));
        }
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("大屏、资源库、组件库分类-方法-列表-获取分页", RemarkFrom.System)]
        [Route("/bigScreen/type/list/resourceCatalog")]
        [ProducesResponseType(typeof(OperateStatus<PagedResults<DashboardTypeFindOutput>>), 1)]
        public async Task<ActionResult> FindResourceCatalog()
        {
            return Ok(await _bigScreenTypeLogic.FindType("resourceCatalog"));
        }
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("大屏、资源库、组件库分类-方法-列表-获取分页", RemarkFrom.System)]
        [Route("/bigScreen/type/list/componentCatalog")]
        [ProducesResponseType(typeof(OperateStatus<PagedResults<DashboardTypeFindOutput>>), 1)]
        public async Task<ActionResult> FindComponentCatalog()
        {
            return Ok(await _bigScreenTypeLogic.FindType("componentCatalog"));
        }
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("大屏、资源库、组件库分类-方法-列表-获取分页", RemarkFrom.System)]
        [Route("/bigScreen/type/list/bizComponentCatalog")]
        [ProducesResponseType(typeof(OperateStatus<PagedResults<DashboardTypeFindOutput>>), 1)]
        public async Task<ActionResult> FindBizComponentCatalog()
        {
            return Ok(await _bigScreenTypeLogic.FindType("bizComponentCatalog"));
        }
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("大屏、资源库、组件库分类-方法-列表-判断", RemarkFrom.System)]
        [Route("/bigScreen/type/nameRepeat")]
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
        /// 保存
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("大屏、资源库、组件库分类-方法-新增/编辑-保存", RemarkFrom.System)]
        [Route("/bigScreen/type/update")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async Task<ActionResult> Update(BigScreenTypeSaveInput entity)
        {
            return Ok(await _bigScreenTypeLogic.Save(entity));
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
        [Route("/bigScreen/type/add")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async Task<ActionResult> Add(BigScreenTypeSaveInput entity)
        {
            return Ok(await _bigScreenTypeLogic.Save(entity));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input">主键Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("大屏、资源库、组件库分类-方法-列表-删除", RemarkFrom.System)]
        [Route("/big/screentype/delete")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async Task<ActionResult> Delete( IdInput<string> input)
        {
            return Ok(await _bigScreenTypeLogic.Delete(input));
        }
        #endregion
    }
}
