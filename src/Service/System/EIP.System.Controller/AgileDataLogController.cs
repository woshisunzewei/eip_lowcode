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
    /// 敏捷开发数据日志
    /// </summary>
    public class AgileDataLogController : BaseSystemController
    {
        #region 构造函数
        private readonly ISystemAgileDataLogLogic _agileDataLogLogic;
        /// <summary>
        /// 
        /// </summary>
        public AgileDataLogController(ISystemAgileDataLogLogic agileDataLogLogic)
        {
            _agileDataLogLogic = agileDataLogLogic;
        }

        #endregion

        /// <summary>
        /// 分页获取
        /// </summary>
        /// <param name="input">分页参数</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("敏捷开发数据日志-方法-列表-根据管理字段Id查询", RemarkFrom.System)]
        [Route("/system/agile/datalog/relationid")]
        public async Task<ActionResult> FindByRelationId(SystemAgileDataLogFindByRelationInput input)
        {
            return Ok(await _agileDataLogLogic.FindByRelationId(input));
        }
    }
}