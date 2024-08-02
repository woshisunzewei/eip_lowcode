/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/5/29 15:57:27
* 文件名: PageController
* 描述: 页面基本信息表控制器
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Common.Controller.Attribute;
using EIP.Common.Core.Extension;
using EIP.Common.Core.Resource;
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
    public class PageController : BaseDataRoomController
    {
        #region 构造函数

        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly IDashboardPageLogic _dashboardPageLogic;
        /// <summary>
        /// 页面基本信息表构造函数
        /// </summary>
        /// <param name="dashboardPageLogic"></param>
        public PageController(IDashboardPageLogic dashboardPageLogic,
            IHostingEnvironment hostingEnvironment)
        {
            _dashboardPageLogic = dashboardPageLogic;
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
        [Route("/dashboard/design/page")]
        [ProducesResponseType(typeof(OperateStatus<PagedResults<DashboardPageFindOutput>>), 1)]
        public async Task<ActionResult> Find([FromQuery] DashboardPageFindInput input)
        {
            return JsonForGridPagingDataRoom(await _dashboardPageLogic.Find(input), input);
        }

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("页面基本信息表-方法-列表-获取分页", RemarkFrom.System)]
        [Route("/dashboard/design/info/code/{id}")]
        [ProducesResponseType(typeof(OperateStatus<PagedResults<DashboardPageFindOutput>>), 1)]
        public async Task<ActionResult> Check([FromRoute] IdInput<string> input)
        {
            return Ok(await _dashboardPageLogic.Check(input));
        }

        /// <summary>
        /// 获取指定图表的数据(通过配置)
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Route("/dashboard/chart/data/chart")]
        [ProducesResponseType(typeof(OperateStatus<PagedResults<DashboardPageFindOutput>>), 1)]
        public async Task<ActionResult> Chart(DashboardChartInput input)
        {
            var data = new DashboardChartOutput();
            if (input.chart != null && input.chart.dataSource.businessKey.IsNotNullOrEmpty())
            {
                data = (await _dashboardPageLogic.Chart(input)).Data;
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
        [Route("/dashboard/chart/data/list")]
        [ProducesResponseType(typeof(OperateStatus<PagedResults<DashboardPageFindOutput>>), 1)]
        public async Task<ActionResult> List(DashboardChartInput input)
        {
            var page = await _dashboardPageLogic.FindAsync(f => f.code == input.pageCode);
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

            data = (await _dashboardPageLogic.List(input)).Data;
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
        /// 获取分页
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("大屏、资源库、组件库分类-方法-列表-判断", RemarkFrom.System)]
        [Route("/dashboard/permission/check/{code}")]
        [ProducesResponseType(typeof(OperateStatus<PagedResults<DashboardTypeFindOutput>>), 1)]
        public ActionResult NameRepeat()
        {
            OperateStatus<bool> operateStatus = new OperateStatus<bool>();
            operateStatus.Code = ResultCode.Success;
            operateStatus.Msg = null;
            operateStatus.Data = true;
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
        [Route("/dashboard/design/update")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async Task<ActionResult> Update(DashboardDesignSaveInput entity)
        {
            return Ok(await _dashboardPageLogic.Save(entity));
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
        [Route("/dashboard/design/add")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async Task<ActionResult> Add(DashboardDesignSaveInput entity)
        {
            return Ok(await _dashboardPageLogic.Save(entity));
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("大屏、资源库、组件库分类-方法-新增/编辑-保存", RemarkFrom.System)]
        [Route("/dashboard/design/copy/{id}")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async Task<ActionResult> Copy([FromRoute] IdInput<string> input)
        {
            var data = await _dashboardPageLogic.FindAsync(f => f.code == input.Id);
            data.id = 0;
            data.code = data.type + "_" + Guid.NewGuid();
            data.name = data.name + "_复制";
            var insert = await _dashboardPageLogic.InsertAsync(data);
            OperateStatus<string> operate = new OperateStatus<string>();
            operate.Data = data.code;
            operate.Code = ResultCode.Success;
            operate.Msg = Chs.Successful;
            return Ok(operate);
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
        [Route("/dashboard/design/delete/{id}")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async Task<ActionResult> Delete([FromRoute] IdInput<string> input)
        {
            return Ok(await _dashboardPageLogic.Delete(input));
        }
        #endregion
    }
}
