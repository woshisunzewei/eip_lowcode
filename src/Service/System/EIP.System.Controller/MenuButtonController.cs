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
using EIP.System.Models.Dtos.MenuButton;

namespace EIP.System.Controller
{
    /// <summary>
    /// 界面按钮控制器
    /// </summary>
    public class MenuButtonController : BaseSystemController
    {
        #region 构造函数
        private readonly ISystemMenuButtonLogic _menuButtonLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="menuButtonLogic"></param>
        public MenuButtonController(ISystemMenuButtonLogic menuButtonLogic)
        {
            _menuButtonLogic = menuButtonLogic;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 根据模块Id获取模块按钮信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("界面按钮-方法-列表-根据模块Id获取按钮信息", RemarkFrom.System)]
        [Route("/system/menubutton/list")]
        public async  Task<ActionResult> Find(SystemMenuButtonFindInput input)
        {
            return JsonForGridPaging(await _menuButtonLogic.Find(input)); 
        }

        /// <summary>
        /// 保存按钮信息
        /// </summary>
        /// <param name="menuButton">模块按钮信息</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("界面按钮-方法-保存", RemarkFrom.System)]
        [Route("/system/menubutton")]
        public async  Task<ActionResult> Save(SystemMenuButton menuButton)
        {
            return Ok(await _menuButtonLogic.Save(menuButton));
        }
        /// <summary>
        /// 保存按钮信息
        /// </summary>
        /// <param name="input">模块按钮信息</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("界面按钮-方法-保存", RemarkFrom.System)]
        [Route("/system/menubutton/all")]
        public async  Task<ActionResult> SaveAll(SystemMenuButtonSaveAllInput input)
        {
            return Ok(await _menuButtonLogic.SaveAll(input));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("界面按钮-方法-删除", RemarkFrom.System)]
        [Route("/system/menubutton/delete")]
        public async  Task<ActionResult> Delete(IdInput<string> input)
        {
            return Ok(await _menuButtonLogic.Delete(input));
        }

        /// <summary>
        /// 根据id查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("界面按钮-编辑-根据id查询", RemarkFrom.System)]
        [Route("/system/menubutton/{id}")]
        public async  Task<ActionResult> FindById([FromRoute] IdInput input)
        {
            return Ok(await _menuButtonLogic.FindById(input));
        }

        /// <summary>
        /// 冻结
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("界面按钮-方法-冻结", RemarkFrom.System)]
        [Route("/system/menubutton/isfreeze")]
        public async  Task<ActionResult> IsFreeze(IdInput input)
        {
            return Ok(await _menuButtonLogic.IsFreeze(input));
        }

        /// <summary>
        /// 是否显示到列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("界面按钮-方法-冻结", RemarkFrom.System)]
        [Route("/system/menubutton/isShowTable")]
        public async  Task<ActionResult> IsShowTable(IdInput input)
        {
            return Ok(await _menuButtonLogic.IsShowTable(input));
        }
        #endregion
    }
}