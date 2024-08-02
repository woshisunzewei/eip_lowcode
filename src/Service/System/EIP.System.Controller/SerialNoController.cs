/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2023/8/22 7:58:35
* 文件名: SerialnoController
* 描述: 编号规则控制器
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.System.Models.Dtos.SerialNo;
namespace EIP.System.Controller
{
    /// <summary>
    /// 编号规则
    /// </summary>
    public class SerialNoController : BaseSystemController
    {
        #region 构造函数
        private readonly ISystemSerialNoLogic _systemSerialNoLogic;
		/// <summary>
        /// 编号规则构造函数
        /// </summary>
        /// <param name="systemSerialnoLogic"></param>
        public SerialNoController(ISystemSerialNoLogic systemSerialnoLogic)
        {
            _systemSerialNoLogic = systemSerialnoLogic;
        }
        #endregion

        /// <summary>
        /// 根据编号获取值
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("编号规则-方法-根据编号获取值", RemarkFrom.System)]
        [Route("/system/serialno/create")]
        [ProducesResponseType(typeof(OperateStatus<SystemSerialNo>), 1)]
        public async  Task<ActionResult> Create(SystemSerialCreateInput input)
        {
            return Ok(await _systemSerialNoLogic.Create(input));
        }
    }
}
