/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/5/29 15:57:28
* 文件名: CategoryTreeController
* 描述: 数据集种类树控制器
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Base.Models.Entities.DataRoom;
using EIP.Common.Controller.Attribute;
using EIP.Common.Models;
using EIP.Common.Models.Dtos;
using EIP.Common.Models.Paging;
using EIP.DataRoom.Controller;
using EIP.DataRoom.Logic;
using EIP.DataRoom.Logic.Impl;
using EIP.DataRoom.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace EIP.DataRoom.Controller
{
    /// <summary>
    /// 数据集种类树
    /// </summary>
    public class CategoryTreeController : BaseDataRoomController
    {
        #region 构造函数
        private readonly IDsCategoryTreeLogic _dsCategoryTreeLogic;
		/// <summary>
        /// 数据集种类树构造函数
        /// </summary>
        /// <param name="dsCategoryTreeLogic"></param>
        public CategoryTreeController(IDsCategoryTreeLogic dsCategoryTreeLogic)
        {
            _dsCategoryTreeLogic = dsCategoryTreeLogic;
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
        [Remark("数据集种类树-方法-列表-获取分页", RemarkFrom.System)]
        [Route("/category/queryTreeList")]
        [ProducesResponseType(typeof(OperateStatus<PagedResults<DsCategoryTreeFindOutput>>), 1)]
        public async Task<ActionResult> Find([FromQuery]DsCategoryTreeFindInput input)
        {
            return Ok(await _dsCategoryTreeLogic.Find(input));
        }
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("大屏、资源库、组件库分类-方法-列表-判断", RemarkFrom.System)]
        [Route("/category/checkRepeat")]
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
        [Route("/category/update")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async Task<ActionResult> Update(DsCategoryTreeSaveInput entity)
        {
            return Ok(await _dsCategoryTreeLogic.Save(entity));
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
        [Route("/category/add")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async Task<ActionResult> Add(DsCategoryTreeSaveInput entity)
        {
            return Ok(await _dsCategoryTreeLogic.Save(entity));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input">主键Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("数据集种类树-方法-列表-删除", RemarkFrom.System)]
        [Route("/ds/categorytree/delete")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async Task<ActionResult> Delete( IdInput<string> input)
        {
            return Ok(await _dsCategoryTreeLogic.Delete(input));
        }
        #endregion
    }
}
