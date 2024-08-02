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
using EIP.System.Models.Dtos.Role;
namespace EIP.System.Controller
{
    /// <summary>
    /// 角色控制器
    /// </summary>
    public class RoleController : BaseSystemController
    {
        #region 构造函数

        private readonly ISystemRoleLogic _roleLogic;
        private readonly ISystemPermissionUserLogic _permissionUserLogic;
        private readonly ISystemOrganizationLogic _organizationLogic;
        private readonly ISystemPermissionLogic _permissionLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="roleLogic"></param>
        /// <param name="permissionUserLogic"></param>
        /// <param name="organizationLogic"></param>
        /// <param name="permissionLogic"></param>
        public RoleController(ISystemRoleLogic roleLogic,
            ISystemPermissionUserLogic permissionUserLogic,
            ISystemOrganizationLogic organizationLogic,
            ISystemPermissionLogic permissionLogic)
        {
            _roleLogic = roleLogic;
            _permissionUserLogic = permissionUserLogic;
            _organizationLogic = organizationLogic;
            _permissionLogic = permissionLogic;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 保存用户角色信息
        /// </summary>
        /// <param name="userRole">角色json字符串</param>
        /// <param name="userId">用户信息</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("角色维护-方法-新增/编辑-保存用户角色信息", RemarkFrom.System)]
        [Route("/system/role/user")]
        public async  Task<ActionResult> SaveUserRole(string userRole,
            Guid userId)
        {
            IList<SystemRoleUserSaveInput> models =
                JsonConvert.DeserializeObject<IList<SystemRoleUserSaveInput>>(userRole);
            IList<Guid> roles = models.Select(m => m.R).ToList();
            return Ok(
                await _permissionUserLogic.SavePermissionMasterValueBeforeDelete(EnumPrivilegeMaster.角色, userId,
                    roles));
        }

        /// <summary>
        /// 根据组织机构获取角色信息
        /// </summary>
        /// <param name="input">组织机构主键Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("角色维护-方法-列表-根据组织机构获取角色信息", RemarkFrom.System)]
        [Route("/system/role/list")]
        public async  Task<ActionResult> Find(SystemRolesFindInput input)
        {
            #region 获取权限控制器信息
            input.DataSql = (await _permissionLogic.FindDataPermissionSql(new ViewRote { UserId = CurrentUser.UserId, MenuId = ResourceMenuId.角色维护.ToGuid() })).Data;
            #endregion
            return JsonForGridPaging(await _roleLogic.Find(input));
        }

        /// <summary>
        /// 根据组织机构获取角色信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("角色维护-方法-列表-获取所有未冻结的角色", RemarkFrom.System)]
        [Route("/system/role/select")]
        public async  Task<ActionResult> Select()
        {
            return Ok(await _roleLogic.Select());
        }

        /// <summary>
        /// 读取组织机构树
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("组织机构维护-方法-列表-读取组织机构树", RemarkFrom.System)]
        [Route("/system/role/organization")]
        public async  Task<ActionResult> FindOrganization()
        {
            #region 获取权限控制器信息
            SystemOrganizationDataPermissionInput input = new SystemOrganizationDataPermissionInput
            {
                PrincipalUser = CurrentUser,
                DataSql = (await _permissionLogic.FindDataPermissionSql(new ViewRote { UserId = CurrentUser.UserId, MenuId = ResourceMenuId.角色维护.ToGuid() })).Data
            };
            #endregion
            return JsonForTree((await _organizationLogic.FindDataPermission(input)).Data.ToList());
        }

        /// <summary>
        /// 获取所有未冻结的角色
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("角色维护-方法-列表-获取所有未冻结的角色", RemarkFrom.System)]
        [Route("/system/role/all")]
        public async  Task<ActionResult> All()
        {
            return Ok(await _roleLogic.All());
        }

        /// <summary>
        /// 保存角色数据
        /// </summary>
        /// <param name="role">角色信息</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("角色维护-方法-新增/编辑-保存", RemarkFrom.System)]
        [Route("/system/role")]
        public async  Task<ActionResult> Save(SystemRole role)
        {
            return Ok(await _roleLogic.Save(role));
        }

        /// <summary>
        /// 删除角色数据
        /// </summary>
        /// <param name="input">角色Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("角色维护-方法-列表-删除", RemarkFrom.System)]
        [Route("/system/role/delete")]
        public async  Task<ActionResult> Delete( IdInput<string> input)
        {
            return Ok(await _roleLogic.Delete(input));
        }

        /// <summary>
        /// 角色复制
        /// </summary>
        /// <param name="input">角色Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("角色维护-方法-列表-角色复制", RemarkFrom.System)]
        [Route("/system/role/copy")]
        public async  Task<ActionResult> Copy( SystemCopyInput input)
        {
            return Ok(await _roleLogic.Copy(input));
        }

        /// <summary>
        /// 编辑/修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("角色维护-方法-列表-根据id获取值", RemarkFrom.System)]
        [Route("/system/role/{id}")]
        public async  Task<ActionResult> FindById([FromRoute] IdInput input)
        {
            return Ok(await _roleLogic.FindById(input));
        }

        /// <summary>
        /// 冻结
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("角色维护-方法-冻结", RemarkFrom.System)]
        [Route("/system/role/isfreeze")]
        public async  Task<ActionResult> IsFreeze(IdInput input)
        {
            return Ok(await _roleLogic.IsFreeze(input));
        }
        #endregion
    }
}

