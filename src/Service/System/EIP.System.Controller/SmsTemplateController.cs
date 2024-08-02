/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2023/4/22 23:10:00
* 文件名: SmstemplateController
* 描述: 短信维护控制器
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.System.Models.Dtos.Smstemplate;

namespace EIP.System.Controller
{
    /// <summary>
    /// 短信维护
    /// </summary>
    public class SmsTemplateController : BaseSystemController
    {
        #region 构造函数
        private readonly ISystemSmstemplateLogic _systemSmstemplateLogic;
		/// <summary>
        /// 短信维护构造函数
        /// </summary>
        /// <param name="systemSmstemplateLogic"></param>
        public SmsTemplateController(ISystemSmstemplateLogic systemSmstemplateLogic)
        {
            _systemSmstemplateLogic = systemSmstemplateLogic;
        }
        #endregion
		
        #region 方法

         /// <summary>
        /// 获取分页
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("短信维护-方法-列表-获取分页", RemarkFrom.System)]
        [Route("/system/smstemplate/list")]
        [ProducesResponseType(typeof(OperateStatus<PagedResults<SystemSmstemplateFindOutput>>), 1)]
        public async  Task<ActionResult> Find(SystemSmstemplateFindInput input)
        {
            return JsonForGridPaging(await _systemSmstemplateLogic.Find(input));
        }

        /// <summary>
        /// 根据id获取
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("短信维护-方法-根据id获取", RemarkFrom.System)]
        [Route("/system/smstemplate/{id}")]
        [ProducesResponseType(typeof(OperateStatus<SystemSmstemplate>), 1)]
        public async  Task<ActionResult> FindById([FromRoute]IdInput input)
        {
            return Ok(await _systemSmstemplateLogic.FindById(input));
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("短信维护-方法-新增/编辑-保存", RemarkFrom.System)]
        [Route("/system/smstemplate")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async  Task<ActionResult> Save(SystemSmstemplate entity)
        {
            return Ok(await _systemSmstemplateLogic.Save(entity));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input">主键Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("短信维护-方法-列表-删除", RemarkFrom.System)]
        [Route("/system/smstemplate/delete")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async  Task<ActionResult> Delete( IdInput<string> input)
        {
            return Ok(await _systemSmstemplateLogic.Delete(input));
        }
        #endregion
    }
}
