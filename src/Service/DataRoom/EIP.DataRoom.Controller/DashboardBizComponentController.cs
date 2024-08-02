/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/5/29 15:57:26
* 文件名: BizComponentController
* 描述: 业务组件表控制器
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
    /// 业务组件表
    /// </summary>
    public class BizComponentController : BaseDataRoomController
    {
        #region 构造函数
        private readonly IDashboardBizComponentLogic _dashboardBizComponentLogic;
        /// <summary>
        /// 业务组件表构造函数
        /// </summary>
        /// <param name="dashboardBizComponentLogic"></param>
        public BizComponentController(IDashboardBizComponentLogic dashboardBizComponentLogic)
        {
            _dashboardBizComponentLogic = dashboardBizComponentLogic;
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
        [Remark("业务组件表-方法-列表-获取分页", RemarkFrom.System)]
        [Route("/dashboard/bizComponent/page")]
        [ProducesResponseType(typeof(OperateStatus<PagedResults<DashboardBizComponentFindOutput>>), 1)]
        public async Task<ActionResult> Find([FromQuery] DashboardBizComponentFindInput input)
        {
            return JsonForGridPagingDataRoom(await _dashboardBizComponentLogic.Find(input), input);
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
        [Route("/dashboard/bizComponent/update")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async Task<ActionResult> Update(DashboardBizComponentSaveInput entity)
        {
            return Ok(await _dashboardBizComponentLogic.Save(entity));
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
        [Route("/dashboard/bizComponent/info/{id}")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async Task<ActionResult> Info([FromRoute] IdInput<string> input)
        {
            return Ok(await _dashboardBizComponentLogic.Info(input));
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
        [Route("/dashboard/bizComponent/add")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async Task<ActionResult> Add(DashboardBizComponentSaveInput entity)
        {
            return Ok(await _dashboardBizComponentLogic.Save(entity));
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
        [Route("/dashboard/bizcomponent/delete/{id}")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async Task<ActionResult> Delete([FromRoute] IdInput<string> input)
        {
            return Ok(await _dashboardBizComponentLogic.Delete(input));
        }
        #endregion
    }
}
