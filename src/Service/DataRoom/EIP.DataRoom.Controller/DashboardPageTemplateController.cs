/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/5/29 15:57:27
* 文件名: PageTemplateController
* 描述: 页面模板表控制器
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
using EIP.DataRoom.Logic;
using EIP.DataRoom.Models.Dtos;
using Microsoft.AspNetCore.Mvc;
namespace EIP.DataRoom.Controller
{
    /// <summary>
    /// 页面模板表
    /// </summary>
    public class PageTemplateController : BaseDataRoomController
    {
        #region 构造函数
        private readonly IDashboardPageTemplateLogic _dashboardPageTemplateLogic;
		/// <summary>
        /// 页面模板表构造函数
        /// </summary>
        /// <param name="dashboardPageTemplateLogic"></param>
        public PageTemplateController(IDashboardPageTemplateLogic dashboardPageTemplateLogic)
        {
            _dashboardPageTemplateLogic = dashboardPageTemplateLogic;
        }
        #endregion
		
        #region 方法

         /// <summary>
        /// 获取分页
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("页面模板表-方法-列表-获取分页", RemarkFrom.System)]
        [Route("/dashboard/pagetemplate/list")]
        [ProducesResponseType(typeof(OperateStatus<PagedResults<DashboardPageTemplateFindOutput>>), 1)]
        public async Task<ActionResult> Find(DashboardPageTemplateFindInput input)
        {
            return JsonForGridPaging(await _dashboardPageTemplateLogic.Find(input));
        }

        /// <summary>
        /// 根据id获取
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("页面模板表-方法-根据id获取", RemarkFrom.System)]
        [Route("/dashboard/pagetemplate/{id}")]
        [ProducesResponseType(typeof(OperateStatus<DashboardPageTemplate>), 1)]
        public async Task<ActionResult> FindById([FromRoute]IdInput<int> input)
        {
            return Ok(await _dashboardPageTemplateLogic.FindById(input));
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("页面模板表-方法-新增/编辑-保存", RemarkFrom.System)]
        [Route("/dashboard/pagetemplate")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async Task<ActionResult> Save(DashboardPageTemplate entity)
        {
            return Ok(await _dashboardPageTemplateLogic.Save(entity));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input">主键Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("页面模板表-方法-列表-删除", RemarkFrom.System)]
        [Route("/dashboard/pagetemplate/delete")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async Task<ActionResult> Delete( IdInput<string> input)
        {
            return Ok(await _dashboardPageTemplateLogic.Delete(input));
        }
        #endregion
    }
}
