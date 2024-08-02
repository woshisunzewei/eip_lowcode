/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/6/1 16:54:21
* 文件名: ScreenPageController
* 描述: 页面基本信息表控制器
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Big.Models.Dtos.ScreenPage;
using EIP.Common.Controller.Attribute;
using EIP.Common.Core.Extension;
using EIP.Common.Core.Util;
using EIP.Common.Models;
using EIP.Common.Models.Dtos;
using EIP.Common.Models.Paging;
using EIP.DataRoom.Logic;
using EIP.DataRoom.Models.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
namespace EIP.DataRoom.Controller
{
    /// <summary>
    /// 页面基本信息表
    /// </summary>
    public class ScreenPageController : BaseDataRoomController
    {
        #region 构造函数
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IBigScreenPageLogic _bigScreenPageLogic;
		/// <summary>
        /// 页面基本信息表构造函数
        /// </summary>
        /// <param name="bigScreenPageLogic"></param>
        public ScreenPageController(IBigScreenPageLogic bigScreenPageLogic,
            IHostingEnvironment hostingEnvironment)
        {
            _bigScreenPageLogic = bigScreenPageLogic;
            _hostingEnvironment = hostingEnvironment;
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
        [Route("/bigScreen/design/page")]
        [ProducesResponseType(typeof(OperateStatus<PagedResults<BigScreenPageFindOutput>>), 1)]
        public async Task<ActionResult> Find([FromQuery] BigScreenPageFindInput input)
        {
            return JsonForGridPagingDataRoom(await _bigScreenPageLogic.Find(input), input);
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
        [Route("/bigScreen/design/update")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async Task<ActionResult> Update(BigScreenPageSaveInput entity)
        {
            return Ok(await _bigScreenPageLogic.Save(entity));
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
        [Route("/bigScreen/design/add")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async Task<ActionResult> Add(BigScreenPageSaveInput entity)
        {
            return Ok(await _bigScreenPageLogic.Save(entity));
        }
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("大屏、资源库、组件库分类-方法-列表-判断", RemarkFrom.System)]
        [Route("/bigScreen/permission/check/{code}")]
        [ProducesResponseType(typeof(OperateStatus<PagedResults<DashboardTypeFindOutput>>), 1)]
        public ActionResult Check()
        {
            OperateStatus<bool> operateStatus = new OperateStatus<bool>();
            operateStatus.Code = ResultCode.Success;
            operateStatus.Msg = null;
            operateStatus.Data = true;
            return Ok(operateStatus);
        }
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("大屏、资源库、组件库分类-方法-列表-判断", RemarkFrom.System)]
        [Route("/bigScreen/design/name/repeat")]
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
        [Remark("页面基本信息表-方法-列表-获取分页", RemarkFrom.System)]
        [Route("/bigScreen/design/info/code/{id}")]
        [ProducesResponseType(typeof(OperateStatus<PagedResults<DashboardPageFindOutput>>), 1)]
        public async Task<ActionResult> FindByCode([FromRoute] IdInput<string> input)
        {
            return Ok(await _bigScreenPageLogic.FindByCode(input));
        }

        /// <summary>
        /// 获取指定图表的数据(通过配置)
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Route("/bigScreen/chart/data/chart")]
        [ProducesResponseType(typeof(OperateStatus<PagedResults<DashboardPageFindOutput>>), 1)]
        public async Task<ActionResult> Chart(DashboardChartInput input)
        {
            var data = new DashboardChartOutput();
            if (input.chart != null && input.chart.dataSource.businessKey.IsNotNullOrEmpty())
            {
                data = (await _bigScreenPageLogic.Chart(input)).Data;
            }
            else
            {
                var returnContent = FileUtil.ReadFile(_hostingEnvironment.ContentRootPath + $"/DataRoom/{input.type}.json");
                data = JsonConvert.DeserializeObject<DashboardChartOutput>(returnContent);
            }
            return Ok(OperateStatus<DashboardChartOutput>.Success(data));
        }
        /// <summary>
        /// 获取指定图表的数据(通过唯一编码)
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Route("/bigScreen/chart/data/list")]
        [ProducesResponseType(typeof(OperateStatus<PagedResults<DashboardPageFindOutput>>), 1)]
        public async Task<ActionResult> List(DashboardChartInput input)
        {
            var page = await _bigScreenPageLogic.FindAsync(f => f.code == input.pageCode);
            if (page == null)
            {
                return Ok(OperateStatus<DashboardChartOutput>.Error("页面不存在"));
            }
            var config = JsonConvert.DeserializeObject<DashboardDesignSaveInput>(page.config);
            var data = new DashboardChartOutput();
            if (config == null || config.chartList == null)
            {
                return DefaultMockData(input);
            }
            List<DashboardChartOptionInput> option = new List<DashboardChartOptionInput>();
            foreach (var item in config.chartList)
            {
                var jsonString = JsonConvert.SerializeObject(item);
                option.Add(JsonConvert.DeserializeObject<DashboardChartOptionInput>(jsonString));
            }
            var chartOption = option.FirstOrDefault(f => f.code == input.chartCode);
            if (chartOption == null)
            {
                return DefaultMockData(input);
            }

            if (chartOption.dataSource.businessKey.IsNullOrEmpty())
            {
                return DefaultMockData(input);
            }

            data = (await _bigScreenPageLogic.List(input)).Data;
            return Ok(OperateStatus<DashboardChartOutput>.Success(data));
        }

        /// <summary>
        /// 默认数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private ActionResult DefaultMockData(DashboardChartInput input)
        {
            var data = new DashboardChartOutput();
            var returnContent = FileUtil.ReadFile(_hostingEnvironment.ContentRootPath + $"/DataRoom/{input.type}.json");
            data = JsonConvert.DeserializeObject<DashboardChartOutput>(returnContent);
            return Ok(OperateStatus<DashboardChartOutput>.Success(data));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input">主键Id</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("页面基本信息表-方法-列表-删除", RemarkFrom.System)]
        [Route("/bigScreen/design/delete/{id}")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async Task<ActionResult> Delete([FromRoute] IdInput<string> input)
        {
            return Ok(await _bigScreenPageLogic.Delete(input));
        }
        #endregion
    }
}
