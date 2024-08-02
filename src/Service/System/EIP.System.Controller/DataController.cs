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
using EIP.System.Models.Dtos.Data;

namespace EIP.System.Controller
{
    /// <summary>
    /// 数据权限控制器
    /// </summary>
    public class DataController : BaseSystemController
    {
        #region 构造函数

        private readonly ISystemDataLogic _dataLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dataLogic"></param>
        public DataController(ISystemDataLogic dataLogic)
        {
            _dataLogic = dataLogic;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 根据模块Id获取数据权限规则
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("数据权限配置-方法-列表-根据模块Id获取数据权限规则", RemarkFrom.System)]
        [Route("/system/data/list")]
        public async Task<ActionResult> Find(SystemDataFindInput input)
        {
            return JsonForGridPaging(await _dataLogic.Find(input));
        }
        /// <summary>
        /// 编辑/修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("数据权限配置-方法-列表-根据id获取值", RemarkFrom.System)]
        [Route("/system/data/{id}")]
        public async Task<ActionResult> FindById([FromRoute] IdInput input)
        {
            return Ok(await _dataLogic.FindById(input));
        }

        /// <summary>
        /// 保存数据权限规则
        /// </summary>
        /// <param name="input">数据权限规则</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("数据权限配置-方法-新增/编辑-保存数据权限规则", RemarkFrom.System)]
        [Route("/system/data")]
        public async Task<ActionResult> Save(SystemData input)
        {
            return Ok(await _dataLogic.Save(input));
        }

        /// <summary>
        /// 根据字段Id删除数据权限规则
        /// </summary>
        /// <param name="input">数据权限规则Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("数据权限配置-方法-列表-删除", RemarkFrom.System)]
        [Route("/system/data/delete")]
        public async Task<ActionResult> Delete(IdInput<string> input)
        {
            return Ok(await _dataLogic.Delete(input));
        }

        /// <summary>
        /// 冻结
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("数据权限配置-方法-冻结", RemarkFrom.System)]
        [Route("/system/data/isfreeze")]
        public async Task<ActionResult> IsFreeze(IdInput input)
        {
            return Ok(await _dataLogic.IsFreeze(input));
        }
        #endregion
    }
}