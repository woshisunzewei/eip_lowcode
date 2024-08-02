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

namespace EIP.System.Controller
{
    /// <summary>
    /// 主页
    /// </summary>
    public class HomeController : BaseSystemController
    {
        private readonly ISystemPermissionLogic _permissionLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="permissionLogic"></param>
        public HomeController(ISystemPermissionLogic permissionLogic)
        {
            _permissionLogic = permissionLogic;
        }

        /// <summary>
        /// 加载模块权限
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("首页-方法-获取模块权限", RemarkFrom.System)]
        [Route("/system/home/menutree")]
        public async  Task<ActionResult> MenusTree()
        {
            var permissions = (await _permissionLogic.MenusTree(new SystemPermissionMenuInput()
            {
                UserId = CurrentUser.UserId
            })).Data.ToList();
            List<BaseTree> baseTrees = new List<BaseTree>();
            foreach (var permission in permissions)
            {
                BaseTree baseTree = new BaseTree();
                baseTree.text = permission.Text;
                baseTree.parent = permission.ParentId;
                baseTree.id = permission.Id;
                baseTree.icon = permission.Icon;
                baseTrees.Add(baseTree);
            }
            return JsonForTree(baseTrees);
        }
        /// <summary>
        /// 获取当前登录人员所有菜单
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("首页-方法-获取当前登录人员所有菜单", RemarkFrom.System)]
        [Route("/system/home/menu")]
        public async  Task<ActionResult> Menus(SystemPermissionMenuInput input)
        {
            input.UserId = CurrentUser.UserId;
            return Ok((await _permissionLogic.Menus(input)).Data.ToList());
        }

        /// <summary>
        /// 获取当前登录人员所有菜单敏捷开发配置
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("首页-方法-获取当前登录人员所有菜单敏捷开发配置", RemarkFrom.System)]
        [Route("/system/home/menu/agile")]
        public async  Task<ActionResult> MenusAgile(SystemPermissionMenuInput input)
        {
            input.UserId = CurrentUser.UserId;
            return Ok(await _permissionLogic.MenusAgile(input));
        }
    }
}