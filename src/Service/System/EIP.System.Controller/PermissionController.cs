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
    /// 权限控制器
    /// </summary>

    public class PermissionController : BaseSystemController
    {
        #region 构造函数

        private readonly ISystemPermissionLogic _permissionLogic;
        private readonly ISystemPermissionUserLogic _permissionUserLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="permissionLogic"></param>
        /// <param name="permissionUserLogic"></param>
        public PermissionController(ISystemPermissionLogic permissionLogic,
            ISystemPermissionUserLogic permissionUserLogic)
        {
            _permissionLogic = permissionLogic;
            _permissionUserLogic = permissionUserLogic;
        }

        #endregion

        /// <summary>
        /// 根据特权Id获取模块权限信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("模块权限-方法-列表-根据特权Id获取模块权限信息", RemarkFrom.System)]
        [Route("/system/permission/privilegemaster/{privilegeMasterValue}/{privilegeMaster}/{privilegeAccess}")]
        public async  Task<ActionResult> FindPermissionByPrivilegeMasterValue([FromRoute] SystemPermissionByPrivilegeMasterValueInput input)
        {
            return Ok(await _permissionLogic.FindPermissionByPrivilegeMasterValue(input));
        }

        /// <summary>
        /// 获取所有模块按钮
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("模块按钮权限-视图-获取所有模块按钮", RemarkFrom.System)]
        [Route("/system/permission/menubutton/{privilegeMasterValue}/{privilegeMaster}")]
        public async  Task<ActionResult> FindMenuButtonByPrivilegeMaster([FromRoute] SystemPermissionFindMenuButtonByPrivilegeMasterInput input)
        {
            input.UserId = CurrentUser.UserId;
            return Ok(await _permissionLogic.FindMenuButtonByPrivilegeMaster(input));
        }

        /// <summary>
        /// 获取所有模块按钮
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("模块按钮权限-视图-获取所有模块按钮", RemarkFrom.System)]
        [Route("/system/permission/menubuttonall/{privilegeMasterValue}/{privilegeMaster}")]
        public async  Task<ActionResult> FindMenuButtonByPrivilegeMasterAll([FromRoute] SystemPermissionFindMenuButtonByPrivilegeMasterInput input)
        {
            input.UserId = CurrentUser.UserId;
            return Ok(await _permissionLogic.FindMenuButtonByPrivilegeMasterAll(input));
        }
        /// <summary>
        /// 模块按钮权限
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("数据权限-视图-列表", RemarkFrom.System)]
        [Route("/system/permission/data/{privilegeMasterValue}/{privilegeMaster}")]
        public async  Task<ActionResult> FindDataByPrivilegeMaster([FromRoute] SystemPermissionFindDataByPrivilegeMasterInput input)
        {
            input.UserId = CurrentUser.UserId;
            return Ok(await _permissionLogic.FindDataByPrivilegeMaster(input));
        }

        /// <summary>
        /// 模块按钮权限
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("数据权限-视图-列表", RemarkFrom.System)]
        [Route("/system/permission/dataall/{privilegeMasterValue}/{privilegeMaster}")]
        public async  Task<ActionResult> FindDataByPrivilegeMasterAll([FromRoute] SystemPermissionFindDataByPrivilegeMasterInput input)
        {
            input.UserId = CurrentUser.UserId;
            return Ok(await _permissionLogic.FindDataByPrivilegeMasterAll(input));
        }
        #region 公用

        /// <summary>
        /// 根据角色Id,岗位Id,组Id,人员Id获取具有的模块信息,用于分派权限时读取左侧拥有的菜单
        /// </summary>
        /// <param name="input">输入参数</param>
        /// <returns>树形模块信息</returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("系统权限公用-方法-列表-根据角色Id,岗位Id,组Id,人员Id获取具有的模块信息", RemarkFrom.System)]
        [Route("/system/permission/menuhave/{privilegeMasterValue}/{privilegeMaster}/{privilegeAccess}")]
        public async  Task<ActionResult> FindMenuHavePermissionByPrivilegeMasterValue([FromRoute] SystemPermissiontMenuHaveByPrivilegeMasterValueInput input)
        {
            return JsonForTree((await _permissionLogic.FindMenuHavePermissionByPrivilegeMasterValue(input)).Data.ToList());
        }
       
        /// <summary>
        /// 保存权限
        /// </summary>
        /// <param name="input">权限类型:模块、模块按钮</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("系统权限公用-方法-保存权限", RemarkFrom.System)]
        [Route("/system/permission")]
        public async  Task<ActionResult> SavePermission(SystemPermissionSaveInput input)
        {
            input.Permissiones = JsonConvert.DeserializeObject<IList<SystemPermissionSaveConvertInput>>(input.MenuPermissions).Select(m => m.P).ToList();
            return Ok(await _permissionLogic.SavePermission(input));
        }

        /// <summary>
        /// 系统权限公用-方法-获取模块功能项信息
        /// </summary>
        /// <param name="viewRote">权限类型:模块、模块按钮</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("系统权限公用-方法-获取模块功能项信息", RemarkFrom.System)]
        [Route("/system/permission/menubutton")]
        public async  Task<ActionResult> FindMenuButton(ViewRote viewRote)
        {
            viewRote.UserId = CurrentUser.UserId;
            return Ok(await _permissionLogic.FindMenuButton(viewRote));
        }

        /// <summary>
        /// 系统权限公用-方法-获取模块功能项信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("系统权限公用-方法-获取模块、字段对应拥有者信息", RemarkFrom.System)]
        [Route("/system/permission/detail/{id}/{access}")]
        public async  Task<ActionResult> FindSystemPrivilegeDetailOutputsByAccessAndValue([FromRoute] SystemPrivilegeDetailInput input)
        {
            return Ok(await _permissionUserLogic.FindSystemPrivilegeDetailOutputsByAccessAndValue(input));
        }

        /// <summary>
        /// 系统权限公用-方法-获取模块功能项信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("系统权限公用-方法-获取模块、字段对应拥有者信息", RemarkFrom.System)]
        [Route("/system/permission/menu")]
        public async  Task<ActionResult> FindSystemPermissionMenuAllByUserId()
        {
            return JsonForTree((await _permissionLogic.FindSystemPermissionMenuAllByUserId()).Data);
        }
        #endregion
    }
}