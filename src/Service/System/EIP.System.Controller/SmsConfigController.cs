/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2023/4/22 23:09:59
* 文件名: SmsconfigController
* 描述: 短信配置控制器
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.System.Models.Dtos.Smsconfig;

namespace EIP.System.Controller
{
    /// <summary>
    /// 短信配置
    /// </summary>
    public class SmsConfigController : BaseSystemController
    {
        #region 构造函数
        private readonly ISystemSmsconfigLogic _systemSmsconfigLogic;
		/// <summary>
        /// 短信配置构造函数
        /// </summary>
        /// <param name="systemSmsconfigLogic"></param>
        public SmsConfigController(ISystemSmsconfigLogic systemSmsconfigLogic)
        {
            _systemSmsconfigLogic = systemSmsconfigLogic;
        }
        #endregion
		
        #region 方法

         /// <summary>
        /// 获取分页
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("短信配置-方法-列表-获取分页", RemarkFrom.System)]
        [Route("/system/smsconfig/list")]
        [ProducesResponseType(typeof(OperateStatus<PagedResults<SystemSmsconfigFindOutput>>), 1)]
        public async  Task<ActionResult> Find(SystemSmsconfigFindInput input)
        {
            return JsonForGridPaging(await _systemSmsconfigLogic.Find(input));
        }

        /// <summary>
        /// 根据id获取
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("短信配置-方法-根据id获取", RemarkFrom.System)]
        [Route("/system/smsconfig/{id}")]
        [ProducesResponseType(typeof(OperateStatus<SystemSmsconfig>), 1)]
        public async  Task<ActionResult> FindById([FromRoute]IdInput input)
        {
            return Ok(await _systemSmsconfigLogic.FindById(input));
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("短信配置-方法-新增/编辑-保存", RemarkFrom.System)]
        [Route("/system/smsconfig")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async  Task<ActionResult> Save(SystemSmsconfig entity)
        {
            return Ok(await _systemSmsconfigLogic.Save(entity));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input">主键Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("短信配置-方法-列表-删除", RemarkFrom.System)]
        [Route("/system/smsconfig/delete")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async  Task<ActionResult> Delete( IdInput<string> input)
        {
            return Ok(await _systemSmsconfigLogic.Delete(input));
        }
        #endregion
    }
}
