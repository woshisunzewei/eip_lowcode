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
    /// 用户控件控制器
    /// </summary>

    public class CommonController : BaseSystemController
    {
        #region 构造函数
        private readonly ISystemDictionaryLogic _dictionaryLogic;
        private readonly ISystemUserInfoLogic _userInfoLogic;
        private readonly ISystemPermissionUserLogic _permissionUserLogic;
        private readonly ISystemOrganizationLogic _organizationLogic;
        private readonly ISystemMenuLogic _menuLogic;
        private readonly ISystemRoleLogic _roleLogic;
        private readonly ISystemDataLogic _dataLogic;
        private readonly ISystemPermissionLogic _permissionLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userInfoLogic"></param>
        /// <param name="permissionUserLogic"></param>
        /// <param name="organizationLogic"></param>
        /// <param name="menuLogic"></param>
        /// <param name="dictionaryLogic"></param>
        /// <param name="mobileMenuLogic"></param>
        /// <param name="dataBaseLogic"></param>
        /// <param name="dataLogic"></param>
        /// <param name="roleLogic"></param>
        /// <param name="permissionLogic"></param>
        public CommonController(
            ISystemUserInfoLogic userInfoLogic,
            ISystemPermissionUserLogic permissionUserLogic,
            ISystemOrganizationLogic organizationLogic,
            ISystemMenuLogic menuLogic,
            ISystemDictionaryLogic dictionaryLogic,
            ISystemDataLogic dataLogic,
            ISystemRoleLogic roleLogic,
            ISystemPermissionLogic permissionLogic)
        {
            _menuLogic = menuLogic;
            _userInfoLogic = userInfoLogic;
            _permissionUserLogic = permissionUserLogic;
            _organizationLogic = organizationLogic;
            _dictionaryLogic = dictionaryLogic;
            _roleLogic = roleLogic;
            _permissionLogic = permissionLogic;
            _dataLogic = dataLogic;
        }

        #endregion

        #region 方法
        /// <summary>
        /// 查看具有特权的人员
        /// </summary>
        /// <param name="privilegeMaster">归属人员类型:企业、角色、岗位、组</param>
        /// <param name="privilegeMasterValue">企业Id、角色Id、岗位Id、组Id</param>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Remark("用户控件-视图-查看具有特权的人员", RemarkFrom.System)]
        [HttpGet]
        [Route("/system/common/chosenuser/{privilegeMaster}/{privilegeMasterValue}")]
        public async  Task<ActionResult> FindChosenPrivilegeMasterUser([FromRoute] EnumPrivilegeMaster privilegeMaster,
            [FromRoute] Guid privilegeMasterValue)
        {
            #region 获取权限控制器信息

            string menuId = ResourceMenuId.组织架构;
            if (privilegeMaster == EnumPrivilegeMaster.角色)
            {
                menuId = ResourceMenuId.角色维护;
            }
            if (privilegeMaster == EnumPrivilegeMaster.组)
            {
                menuId = ResourceMenuId.组维护;
            }
            if (privilegeMaster == EnumPrivilegeMaster.岗位)
            {
                menuId = ResourceMenuId.岗位维护;
            }
            SystemUserFindCommonInput input = new SystemUserFindCommonInput
            {
                DataSql = (await _permissionLogic.FindDataPermissionSql(new ViewRote
                {
                    UserId = CurrentUser.UserId,
                    MenuId = Guid.Parse(menuId)
                })).Data,
                PrivilegeMaster = EnumPrivilegeMaster.组织架构
            };
            #endregion

            //获取所有人员信息
            IList<SystemUserFindCommonOutput> chosenUserDtos = (await _userInfoLogic.FindCommon(input)).Data.ToList();

            //获取所有的用户
            var permissions = (await _permissionUserLogic.FindPermissionUsersByPrivilegeMasterAdnPrivilegeMasterValue(privilegeMaster, privilegeMasterValue)).Data.ToList();
            foreach (var dto in chosenUserDtos)
            {
                dto.Exist = permissions.Where(w => w.PrivilegeMasterUserId == dto.UserId).FirstOrDefault() != null;
            }
            return Ok(OperateStatus<List<SystemUserFindCommonOutput>>.Success(chosenUserDtos.OrderByDescending(w => w.Exist).ToList()));
        }

        /// <summary>
        /// 保存用户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Remark("用户控件-方法-保存权限用户信息", RemarkFrom.System)]
        [HttpPost]
        [Route("/system/common/chosenuser")]
        public async Task<ActionResult> SavePrivilegeMasterUser(SystemPermissionSavePrivilegeMasterUserInput input)
        {
            return Ok(await _permissionUserLogic.SavePermissionUserBeforeDelete(input.PrivilegeMaster, input.PrivilegeMasterValue, JsonConvert.DeserializeObject<IList<SystemPermissionSaveUserInput>>(input.PrivilegeMasterUser).Select(m => m.U).ToList()));
        }

        #endregion

    }
}