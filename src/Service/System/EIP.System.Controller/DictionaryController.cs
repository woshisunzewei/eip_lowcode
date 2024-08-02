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
using EIP.Common.Models.Tree;
using EIP.System.Models.Dtos.Dictionary;

namespace EIP.System.Controller
{
    /// <summary>
    /// 字典控制器
    /// </summary>
    public class DictionaryController : BaseSystemController
    {
        #region 构造函数
        private readonly ISystemDictionaryLogic _dictionaryLogic;
        /// <summary>
        /// 字典控制器
        /// </summary>
        /// <param name="dictionaryLogic"></param>
        public DictionaryController(ISystemDictionaryLogic dictionaryLogic)
        {
            _dictionaryLogic = dictionaryLogic;
        }
        #endregion

        #region 方法
        /// <summary>
        /// 所有字典树结构
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("字典信息维护-方法-列表-所有字典树结构", RemarkFrom.System)]
        [ProducesResponseType(typeof(TreeEntity), 200)]
        [Route("/system/dictionary")]
        public async Task<ActionResult> Tree()
        {
            return JsonForTree((await _dictionaryLogic.Tree()).Data);
        }

        /// <summary>
        /// 字典列表信息
        /// </summary>
        /// <param name="input">父级id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("字典信息维护-方法-列表-字典列表信息", RemarkFrom.System)]
        [ProducesResponseType(typeof(SystemDictionaryFindOutput), 200)]
        [Route("/system/dictionary/list")]
        public async Task<ActionResult> Find(SystemDictionaryFindInput input)
        {
            return Ok(await _dictionaryLogic.Find(input));
        }

        /// <summary>
        /// 根据id获取字典
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("字典信息维护-方法-根据id获取字典", RemarkFrom.System)]
        [ProducesResponseType(typeof(SystemDictionaryEditOutput), 200)]
        [Route("/system/dictionary/{id}")]
        public async Task<ActionResult> FindById([FromRoute] IdInput input)
        {
            return Ok(await _dictionaryLogic.FindById(input));
        }

        /// <summary>
        /// 保存字典
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("字典信息维护-方法-新增/编辑-保存字典", RemarkFrom.System)]
        [ProducesResponseType(typeof(OperateStatus), 200)]
        [Route("/system/dictionary")]
        public async Task<ActionResult> Save(SystemDictionary input)
        {
            return Ok(await _dictionaryLogic.Save(input));
        }

        /// <summary>
        /// 删除字典数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("字典信息维护-方法-列表-删除", RemarkFrom.System)]
        [ProducesResponseType(typeof(OperateStatus), 200)]
        [Route("/system/dictionary/delete")]
        public async Task<ActionResult> Delete(IdInput<string> input)
        {
            return Ok(await _dictionaryLogic.Delete(input));
        }

        /// <summary>
        /// 根据ParentId获取所有下级
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("字典信息维护-方法-列表-根据ParentId获取所有下级", RemarkFrom.System)]
        [ProducesResponseType(typeof(SystemDictionary), 200)]
        [Route("/system/dictionary/parentid/{id}")]
        public async Task<ActionResult> FindByParentId([FromRoute] IdInput input)
        {
            return Ok(await _dictionaryLogic.FindByParentId(input));
        }

        /// <summary>
        /// 冻结
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("字典信息维护-方法-冻结", RemarkFrom.System)]
        [Route("/system/dictionary/isfreeze")]
        public async Task<ActionResult> IsFreeze(IdInput input)
        {
            return Ok(await _dictionaryLogic.IsFreeze(input));
        }

        /// <summary>
        /// 所有字典树结构
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("字典信息维护-方法-列表-所有字典树结构", RemarkFrom.System)]
        [ProducesResponseType(typeof(TreeEntity), 200)]
        [Route("/system/dictionary/parentids")]
        public async Task<ActionResult> FindByParentIds(SystemDictionaryFindByParentIdInput input)
        {
            return JsonForTree((await _dictionaryLogic.FindByParentIds(input)).Data);
        }
        #endregion
    }
}