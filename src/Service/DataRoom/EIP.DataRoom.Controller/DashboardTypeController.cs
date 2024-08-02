/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/5/29 15:57:27
* 文件名: TypeController
* 描述: 大屏、资源库、组件库分类控制器
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
    /// 大屏、资源库、组件库分类
    /// </summary>
    public class TypeController : BaseDataRoomController
    {
        #region 构造函数
        private readonly IDashboardTypeLogic _dashboardTypeLogic;
        /// <summary>
        /// 大屏、资源库、组件库分类构造函数
        /// </summary>
        /// <param name="dashboardTypeLogic"></param>
        public TypeController(IDashboardTypeLogic dashboardTypeLogic)
        {
            _dashboardTypeLogic = dashboardTypeLogic;
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
        [Route("/dashboard/type/list/dashboardCatalog")]
        [ProducesResponseType(typeof(OperateStatus<PagedResults<DashboardTypeFindOutput>>), 1)]
        public async Task<ActionResult> FindCatalog()
        {
            return Ok(await _dashboardTypeLogic.FindType("dashboardCatalog"));
        }
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("大屏、资源库、组件库分类-方法-列表-获取分页", RemarkFrom.System)]
        [Route("/dashboard/type/list/componentCatalog")]
        [ProducesResponseType(typeof(OperateStatus<PagedResults<DashboardTypeFindOutput>>), 1)]
        public async Task<ActionResult> FindComponent()
        {
            return Ok(await _dashboardTypeLogic.FindType("componentCatalog"));
        }
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("大屏、资源库、组件库分类-方法-列表-获取分页", RemarkFrom.System)]
        [Route("/dashboard/type/list/bizComponentCatalog")]
        [ProducesResponseType(typeof(OperateStatus<PagedResults<DashboardTypeFindOutput>>), 1)]
        public async Task<ActionResult> FindBizComponent()
        {
            return Ok(await _dashboardTypeLogic.FindType("bizComponentCatalog"));
        }
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("大屏、资源库、组件库分类-方法-列表-获取分页", RemarkFrom.System)]
        [Route("/dashboard/type/list/resourceCatalog")]
        [ProducesResponseType(typeof(OperateStatus<PagedResults<DashboardTypeFindOutput>>), 1)]
        public async Task<ActionResult> FindResource()
        {
            return Ok(await _dashboardTypeLogic.FindType("resourceCatalog"));
        }
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("大屏、资源库、组件库分类-方法-列表-判断", RemarkFrom.System)]
        [Route("/dashboard/type/nameRepeat")]
        [ProducesResponseType(typeof(OperateStatus<PagedResults<DashboardTypeFindOutput>>), 1)]
        public ActionResult NameRepeat()
        {
            OperateStatus<bool> operateStatus = new OperateStatus<bool>();
            operateStatus.Code=ResultCode.Success;
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
        [Route("/dashboard/type/update")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async Task<ActionResult> Update(DashboardTypeSaveInput entity)
        {
            return Ok(await _dashboardTypeLogic.Save(entity));
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
        [Route("/dashboard/type/add")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async Task<ActionResult> Add(DashboardTypeSaveInput entity)
        {
            return Ok(await _dashboardTypeLogic.Save(entity));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input">主键Id</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("大屏、资源库、组件库分类-方法-列表-删除", RemarkFrom.System)]
        [Route("/dashboard/type/delete/{id}")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async Task<ActionResult> Delete([FromRoute] IdInput<string> input)
        {
            return Ok(await _dashboardTypeLogic.Delete(input));
        }
        #endregion
    }
}
