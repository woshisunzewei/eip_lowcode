/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/5/29 15:57:29
* 文件名: DatasourceController
* 描述: 数据源配置表控制器
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
    /// 数据源配置表
    /// </summary>
    public class DatasourceController : BaseDataRoomController
    {
        #region 构造函数
        private readonly IDsDatasourceLogic _dsDatasourceLogic;
        /// <summary>
        /// 数据源配置表构造函数
        /// </summary>
        /// <param name="dsDatasourceLogic"></param>
        public DatasourceController(IDsDatasourceLogic dsDatasourceLogic)
        {
            _dsDatasourceLogic = dsDatasourceLogic;
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
        [Remark("数据源配置表-方法-列表-获取分页", RemarkFrom.System)]
        [Route("/datasource/page")]
        [ProducesResponseType(typeof(OperateStatus<PagedResults<DsDatasourceFindOutput>>), 1)]
        public async Task<ActionResult> Find([FromQuery] DsDatasourceFindInput input)
        {
            return JsonForGridPagingDataRoom(await _dsDatasourceLogic.Find(input), input);
        }

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("数据源配置表-方法-列表-获取分页", RemarkFrom.System)]
        [Route("/datasource/list")]
        [ProducesResponseType(typeof(OperateStatus<PagedResults<DsDatasourceFindOutput>>), 1)]
        public async Task<ActionResult> FindList([FromQuery] DataSourceListInput input)
        {
            return Ok(await _dsDatasourceLogic.FindList(input));
        }

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("数据源配置表-方法-列表-获取分页", RemarkFrom.System)]
        [Route("/datasource/getTableList/{id}")]
        [ProducesResponseType(typeof(OperateStatus<PagedResults<DsDatasourceFindOutput>>), 1)]
        public async Task<ActionResult> FindTableList([FromRoute] DataSourceGetTableListInput input)
        {
            return Ok(await _dsDatasourceLogic.FindTableList(input));
        }

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("数据源配置表-方法-列表-获取分页", RemarkFrom.System)]
        [Route("/datasource/getFieldList/table/{id}/{tableName}")]
        [ProducesResponseType(typeof(OperateStatus<PagedResults<DataSourceGetFieldListOutput>>), 1)]
        public async Task<ActionResult> FindFieldList([FromRoute] DataSourceGetFieldListInput input)
        {
            return Ok(await _dsDatasourceLogic.FindFieldList(input));
        }

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("数据源配置表-方法-列表-判断", RemarkFrom.System)]
        [Route("/datasource/checkRepeat")]
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
        /// 获取分页
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("数据源配置表-方法-列表-判断", RemarkFrom.System)]
        [Route("/datasource/testConnect")]
        [ProducesResponseType(typeof(OperateStatus<bool>), 1)]
        public ActionResult TestConnect()
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
        [Remark("数据源配置表-方法-列表-判断", RemarkFrom.System)]
        [Route("/datasource/getViewList/{id}")]
        [ProducesResponseType(typeof(OperateStatus<List<string>>), 1)]
        public ActionResult GetViewList()
        {
            OperateStatus<List<string>> operateStatus = new OperateStatus<List<string>>();
            operateStatus.Code = ResultCode.Success;
            operateStatus.Msg = null;
            operateStatus.Data = new List<string>();
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
        [Route("/datasource/update")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async Task<ActionResult> Update(DataSourceSaveInput entity)
        {
            return Ok(await _dsDatasourceLogic.Save(entity));
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
        [Route("/datasource/add")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async Task<ActionResult> Add(DataSourceSaveInput entity)
        {
            return Ok(await _dsDatasourceLogic.Save(entity));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input">主键Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("数据源配置表-方法-列表-删除", RemarkFrom.System)]
        [Route("/ds/datasource/delete")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async Task<ActionResult> Delete(IdInput<string> input)
        {
            return Ok(await _dsDatasourceLogic.Delete(input));
        }
        #endregion
    }
}
