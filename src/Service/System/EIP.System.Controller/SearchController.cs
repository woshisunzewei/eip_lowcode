/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/7/9 22:28:57
* 文件名: SearchController
* 描述: 控制器
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.System.Models.Dtos.Search;
namespace EIP.System.Controller
{
    /// <summary>
    /// 
    /// </summary>
    public class SearchController : BaseSystemController
    {
        #region 构造函数
        private readonly ISystemSearchLogic _systemSearchLogic;
		/// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="systemSearchLogic"></param>
        public SearchController(ISystemSearchLogic systemSearchLogic)
        {
            _systemSearchLogic = systemSearchLogic;
        }
        #endregion
		
        #region 方法

         /// <summary>
        /// 获取分页
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("-方法-列表-获取分页", RemarkFrom.System)]
        [Route("/system/search/list")]
        [ProducesResponseType(typeof(OperateStatus<PagedResults<SystemSearchFindOutput>>), 1)]
        public async Task<ActionResult> Find(SystemSearchFindInput input)
        {
            input.UserId = CurrentUser.UserId;
            return JsonForGridPaging(await _systemSearchLogic.Find(input));
        }

        /// <summary>
        /// 根据id获取
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("-方法-根据id获取", RemarkFrom.System)]
        [Route("/system/search/{id}")]
        [ProducesResponseType(typeof(OperateStatus<SystemSearch>), 1)]
        public async Task<ActionResult> FindById([FromRoute]IdInput input)
        {
            return Ok(await _systemSearchLogic.FindById(input));
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("-方法-新增/编辑-保存", RemarkFrom.System)]
        [Route("/system/search")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async Task<ActionResult> Save(SystemSearch entity)
        {
            return Ok(await _systemSearchLogic.Save(entity));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input">主键Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("-方法-列表-删除", RemarkFrom.System)]
        [Route("/system/search/delete")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async Task<ActionResult> Delete( IdInput<string> input)
        {
            return Ok(await _systemSearchLogic.Delete(input));
        }
        #endregion
    }
}
