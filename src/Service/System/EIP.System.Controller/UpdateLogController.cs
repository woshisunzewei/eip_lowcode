/**************************************************************
* Copyright (C) 2022 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2022/01/12 22:40:15
* 文件名: 
* 描述: 
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
namespace EIP.System.Controller
{
    /// <summary>
    /// 更新日志控制器
    /// </summary>
    public class UpdateLogController : BaseSystemController
    {
        #region 构造函数
        private readonly ISystemUpdateLogLogic _systemUpdateLogLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="systemUpdateLogLogic"></param>
        public UpdateLogController(ISystemUpdateLogLogic systemUpdateLogLogic)
        {
            _systemUpdateLogLogic = systemUpdateLogLogic;
        }
        #endregion

        #region 方法

        /// <summary>
        /// 读取所有信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("更新日志-方法-列表-读取所有信息", RemarkFrom.System)]
        [Route("/system/updatelog/list")]
        public async  Task<ActionResult> Find()
        {
            return Ok(OperateStatus<IEnumerable<SystemUpdateLog>>.Success((await _systemUpdateLogLogic.FindAllAsync()).ToList().OrderByDescending(f=>f.Id)));
        }

        #endregion
    }
}