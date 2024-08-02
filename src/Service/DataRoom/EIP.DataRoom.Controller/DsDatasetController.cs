/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/5/29 15:57:28
* 文件名: DatasetController
* 描述: 数据集表控制器
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using AngleSharp.Dom;
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
    /// 数据集表
    /// </summary>
    public class DatasetController : BaseDataRoomController
    {
        #region 构造函数
        private readonly IDsDatasetLogic _dsDatasetLogic;
		/// <summary>
        /// 数据集表构造函数
        /// </summary>
        /// <param name="dsDatasetLogic"></param>
        public DatasetController(IDsDatasetLogic dsDatasetLogic)
        {
            _dsDatasetLogic = dsDatasetLogic;
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
        [Remark("数据集表-方法-列表-获取分页", RemarkFrom.System)]
        [Route("/dataset/page")]
        [ProducesResponseType(typeof(OperateStatus<PagedResults<DsDatasetFindOutput>>), 1)]
        public async Task<ActionResult> Find([FromRoute]DsDatasetFindInput input)
        {
            return JsonForGridPagingDataRoom(await _dsDatasetLogic.Find(input),input);
        }
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("数据集表-方法-列表-获取分页", RemarkFrom.System)]
        [Route("/dataset/execute/test")]
        [ProducesResponseType(typeof(OperateStatus<PagedResults<DsDatasetFindOutput>>), 1)]
        public async Task<ActionResult> Test(DsDataSetTestInput input)
        {
            return Ok(await _dsDatasetLogic.Test(input));
        }
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("大屏、资源库、组件库分类-方法-列表-判断", RemarkFrom.System)]
        [Route("/dataset/checkRepeat")]
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
        /// 获取分页
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("大屏、资源库、组件库分类-方法-列表-判断", RemarkFrom.System)]
        [Route("/dataset/datasetInfo/{id}")]
        [ProducesResponseType(typeof(OperateStatus<PagedResults<DashboardTypeFindOutput>>), 1)]
        public async Task<ActionResult> FindById([FromRoute]IdInput<int> input)
        {
            return Ok(await _dsDatasetLogic.FindById(input));
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
        [Route("/dataset/update")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async Task<ActionResult> Update(DsDatasetSaveInput entity)
        {
            return Ok(await _dsDatasetLogic.Save(entity));
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
        [Route("/dataset/add")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async Task<ActionResult> Add(DsDatasetSaveInput entity)
        {
            return Ok(await _dsDatasetLogic.Save(entity));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input">主键Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("数据集表-方法-列表-删除", RemarkFrom.System)]
        [Route("/ds/dataset/delete")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async Task<ActionResult> Delete( IdInput<string> input)
        {
            return Ok(await _dsDatasetLogic.Delete(input));
        }
        #endregion
    }
}
