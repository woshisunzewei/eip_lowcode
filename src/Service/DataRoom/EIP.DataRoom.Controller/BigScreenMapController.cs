/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/6/1 16:54:21
* 文件名: ScreenMapController
* 描述: 地图数据维护表控制器
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Base.Models.Entities.DataRoom;
using EIP.Big.Models.Dtos.ScreenMap;
using EIP.Common.Controller.Attribute;
using EIP.Common.Models;
using EIP.Common.Models.Dtos;
using EIP.Common.Models.Paging;
using EIP.DataRoom.Logic;
using Microsoft.AspNetCore.Mvc;
namespace EIP.DataRoom.Controller
{
    /// <summary>
    /// 地图数据维护表
    /// </summary>
    public class ScreenMapController : BaseDataRoomController
    {
        #region 构造函数
        private readonly IBigScreenMapLogic _bigScreenMapLogic;
		/// <summary>
        /// 地图数据维护表构造函数
        /// </summary>
        /// <param name="bigScreenMapLogic"></param>
        public ScreenMapController(IBigScreenMapLogic bigScreenMapLogic)
        {
            _bigScreenMapLogic = bigScreenMapLogic;
        }
        #endregion
		
        #region 方法

         /// <summary>
        /// 获取分页
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("地图数据维护表-方法-列表-获取分页", RemarkFrom.System)]
        [Route("/big/screenmap/list")]
        [ProducesResponseType(typeof(OperateStatus<PagedResults<BigScreenMapFindOutput>>), 1)]
        public async Task<ActionResult> Find(BigScreenMapFindInput input)
        {
            return JsonForGridPaging(await _bigScreenMapLogic.Find(input));
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("地图数据维护表-方法-新增/编辑-保存", RemarkFrom.System)]
        [Route("/big/screenmap")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async Task<ActionResult> Save(BigScreenMap entity)
        {
            return Ok(await _bigScreenMapLogic.Save(entity));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input">主键Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("地图数据维护表-方法-列表-删除", RemarkFrom.System)]
        [Route("/big/screenmap/delete")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async Task<ActionResult> Delete( IdInput<string> input)
        {
            return Ok(await _bigScreenMapLogic.Delete(input));
        }
        #endregion
    }
}
