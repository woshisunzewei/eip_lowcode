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
using EIP.System.Models.Dtos.Menu;

namespace EIP.System.Controller
{
    /// <summary>
    /// 模块维护
    /// </summary>

    public class MenuController : BaseSystemController
    {
        #region 构造函数

        private readonly ISystemMenuLogic _menuLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="menuLogic"></param>
        public MenuController(ISystemMenuLogic menuLogic)
        {
            _menuLogic = menuLogic;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 获取所有模块信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("模块维护-方法-列表-获取所有模块信息", RemarkFrom.System, true)]
        [Route("/system/menu")]
        public async Task<ActionResult> Tree([FromQuery] SystemMenuTreeInput input)
        {
            var menus = (await _menuLogic.Tree()).Data.ToList();
            if (input.menuId.HasValue)
            {
                menus = menus.Where(w => w.parents.IsNotNullOrEmpty() && !w.parents.Contains(input.menuId.ToString())).ToList();
            }
            if (input.isAgileMenu.HasValue)
            {
                menus = menus.Where(w => w.extend.agileMenuType.HasValue && w.extend.agileMenuType == 1).ToList();
            }
            return JsonForTree(menus);
        }

        /// <summary>
        /// 根据父级Id获取下级模块
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("模块维护-方法-列表-根据父级Id获取下级模块", RemarkFrom.System)]
        [Route("/system/menu/list")]
        public async Task<ActionResult> Find(SystemMenuFindInput input)
        {
            return Ok(await _menuLogic.Find(input));
        }
        /// <summary>
        /// 根据id获取值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("模块维护-方法-列表-根据id获取值", RemarkFrom.System)]
        [Route("/system/menu/{id}")]
        public async Task<ActionResult> FindById([FromRoute] IdInput input)
        {
            return Ok(await _menuLogic.FindById(input));
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("模块维护-方法-新增/编辑-保存", RemarkFrom.System)]
        [Route("/system/menu")]
        public async Task<ActionResult> Save(SystemMenu input)
        {
            return Ok(await _menuLogic.Save(input));
        }
        /// <summary>
        /// 菜单移动保存
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("模块维护-方法-新增/编辑-保存", RemarkFrom.System)]
        [Route("/system/menu/move")]
        public async Task<ActionResult> SaveMove(SystemMenu input)
        {
            return Ok(await _menuLogic.SaveMove(input));
        }
        /// <summary>
        /// 是否显示菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("模块维护-方法-是否显示菜单", RemarkFrom.System)]
        [Route("/system/menu/isshowmenu")]
        public async Task<ActionResult> IsShowMenu(IdInput input)
        {
            return Ok(await _menuLogic.IsShowMenu(input));
        }

        /// <summary>
        /// 是否具有模块权限
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("模块维护-方法-是否具有模块权限", RemarkFrom.System)]
        [Route("/system/menu/havemenupermission")]
        public async Task<ActionResult> HaveMenuPermission(IdInput input)
        {
            return Ok(await _menuLogic.HaveMenuPermission(input));
        }

        /// <summary>
        /// 是否具有数据权限
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("模块维护-方法-是否具有数据权限", RemarkFrom.System)]
        [Route("/system/menu/havedatapermission")]
        public async Task<ActionResult> HaveDataPermission(IdInput input)
        {
            return Ok(await _menuLogic.HaveDataPermission(input));
        }

        /// <summary>
        /// 是否具有字段权限
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("模块维护-方法-是否具有字段权限", RemarkFrom.System)]
        [Route("/system/menu/havefieldpermission")]
        public async Task<ActionResult> HaveFieldPermission(IdInput input)
        {
            return Ok(await _menuLogic.HaveFieldPermission(input));
        }

        /// <summary>
        /// 是否具有功能项权限
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("模块维护-方法-是否具有功能项权限", RemarkFrom.System)]
        [Route("/system/menu/havebuttonpermission")]
        public async Task<ActionResult> HaveButtonPermission(IdInput input)
        {
            return Ok(await _menuLogic.HaveButtonPermission(input));
        }

        /// <summary>
        /// 冻结
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("模块维护-方法-冻结", RemarkFrom.System)]
        [Route("/system/menu/isfreeze")]
        public async Task<ActionResult> IsFreeze(IdInput input)
        {
            return Ok(await _menuLogic.IsFreeze(input));
        }

        /// <summary>
        /// 删除模块数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("模块维护-方法-列表-删除", RemarkFrom.System)]
        [Route("/system/menu/delete")]
        public async Task<ActionResult> Delete(IdInput<string> input)
        {
            return Ok(await _menuLogic.Delete(input));
        }

        /// <summary>
        /// 获取权限树菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("模块维护-方法-列表-获取权限树菜单", RemarkFrom.System)]
        [Route("/system/menu/permission/{privilegeAccess}")]
        public async Task<ActionResult> FindPermissionMenu(EnumPrivilegeAccess privilegeAccess)
        {
            return JsonForTree((await _menuLogic.FindPermissionMenu(privilegeAccess)).Data.ToList());
        }
        #endregion

        #region 应用菜单
        /// <summary>
        /// 获取所有应用菜单
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("模块维护-方法-列表-获取所有应用菜单", RemarkFrom.System)]
        [Route("/system/menu/app")]
        public async Task<ActionResult> AppTree()
        {
            return Ok((await _menuLogic.Tree(true)).Data);
        }
        #endregion
    }
}