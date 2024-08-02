/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/5/29 15:57:28
* 文件名: DatasetLabelController
* 描述: 数据集与标签关联表控制器
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
using EIP.DataRoom.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
namespace EIP.DataRoom.Controller
{
    /// <summary>
    /// 数据集与标签关联表
    /// </summary>
    public class DatasetLabelController : BaseDataRoomController
    {
        #region 构造函数
        private readonly IDsDatasetLabelLogic _dsDatasetLabelLogic;
		/// <summary>
        /// 数据集与标签关联表构造函数
        /// </summary>
        /// <param name="dsDatasetLabelLogic"></param>
        public DatasetLabelController(IDsDatasetLabelLogic dsDatasetLabelLogic)
        {
            _dsDatasetLabelLogic = dsDatasetLabelLogic;
        }
        #endregion
		
        #region 方法

         /// <summary>
        /// 获取分页
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("数据集与标签关联表-方法-列表-获取分页", RemarkFrom.System)]
        [Route("/ds/datasetlabel/list")]
        [ProducesResponseType(typeof(OperateStatus<PagedResults<DsDatasetLabelFindOutput>>), 1)]
        public async Task<ActionResult> Find(DsDatasetLabelFindInput input)
        {
            return JsonForGridPaging(await _dsDatasetLabelLogic.Find(input));
        }

        /// <summary>
        /// 根据id获取
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("数据集与标签关联表-方法-根据id获取", RemarkFrom.System)]
        [Route("/ds/datasetlabel/{id}")]
        [ProducesResponseType(typeof(OperateStatus<DsDatasetLabel>), 1)]
        public async Task<ActionResult> FindById([FromRoute]IdInput<int> input)
        {
            return Ok(await _dsDatasetLabelLogic.FindById(input));
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("数据集与标签关联表-方法-新增/编辑-保存", RemarkFrom.System)]
        [Route("/ds/datasetlabel")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async Task<ActionResult> Save(DsDatasetLabel entity)
        {
            return Ok(await _dsDatasetLabelLogic.Save(entity));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input">主键Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("数据集与标签关联表-方法-列表-删除", RemarkFrom.System)]
        [Route("/ds/datasetlabel/delete")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async Task<ActionResult> Delete( IdInput<string> input)
        {
            return Ok(await _dsDatasetLabelLogic.Delete(input));
        }
        #endregion
    }
}
