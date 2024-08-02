/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2023/7/1 16:35:32
* 文件名: DatasourceController
* 描述: 数据源管理控制器
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.System.Models.Dtos.Datasource;
namespace EIP.System.Controller
{
    /// <summary>
    /// 数据源管理
    /// </summary>
    public class DatasourceController : BaseSystemController
    {
        #region 构造函数
        private readonly ISystemDatasourceLogic _systemDatasourceLogic;
		/// <summary>
        /// 数据源管理构造函数
        /// </summary>
        /// <param name="systemDatasourceLogic"></param>
        public DatasourceController(ISystemDatasourceLogic systemDatasourceLogic)
        {
            _systemDatasourceLogic = systemDatasourceLogic;
        }
        #endregion
		
        #region 方法

         /// <summary>
        /// 获取分页
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("数据源管理-方法-列表-获取分页", RemarkFrom.System)]
        [Route("/system/datasource/list")]
        [ProducesResponseType(typeof(OperateStatus<PagedResults<SystemDatasourceFindOutput>>), 1)]
        public async  Task<ActionResult> Find(SystemDatasourceFindInput input)
        {
            return JsonForGridPaging(await _systemDatasourceLogic.Find(input));
        }

        /// <summary>
        /// 根据id获取
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("数据源管理-方法-根据id获取", RemarkFrom.System)]
        [Route("/system/datasource/{id}")]
        [ProducesResponseType(typeof(OperateStatus<SystemDataSource>), 1)]
        public async  Task<ActionResult> FindById([FromRoute]IdInput input)
        {
            return Ok(await _systemDatasourceLogic.FindById(input));
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("数据源管理-方法-新增/编辑-保存", RemarkFrom.System)]
        [Route("/system/datasource")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async  Task<ActionResult> Save(SystemDataSource entity)
        {
            return Ok(await _systemDatasourceLogic.Save(entity));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input">主键Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("数据源管理-方法-列表-删除", RemarkFrom.System)]
        [Route("/system/datasource/delete")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async  Task<ActionResult> Delete( IdInput<string> input)
        {
            return Ok(await _systemDatasourceLogic.Delete(input));
        }
        #endregion
    }
}
