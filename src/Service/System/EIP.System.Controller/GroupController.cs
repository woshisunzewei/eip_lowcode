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
using EIP.System.Models.Dtos.Group;
using EIP.System.Models.Dtos.Role;

namespace EIP.System.Controller
{
    /// <summary>
    /// 组管理控制器
    /// </summary>

    public class GroupController : BaseSystemController
    {
        #region 构造函数
        private readonly ISystemOrganizationLogic _organizationLogic;
        private readonly ISystemPermissionLogic _permissionLogic;
        private readonly ISystemGroupLogic _groupLogic;
        private readonly ISystemUserInfoLogic _userInfoLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="groupLogic"></param>
        /// <param name="userInfoLogic"></param>
        /// <param name="organizationLogic"></param>
        /// <param name="permissionLogic"></param>
        public GroupController(ISystemGroupLogic groupLogic,
            ISystemUserInfoLogic userInfoLogic,
            ISystemOrganizationLogic organizationLogic,
            ISystemPermissionLogic permissionLogic)
        {
            _groupLogic = groupLogic;
            _userInfoLogic = userInfoLogic;
            _organizationLogic = organizationLogic;
            _permissionLogic = permissionLogic;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 根据组织机构Id获取对应下的组信息:id为空查询所有
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("组维护-方法-列表-根据组织机构Id获取对应下的组信息:id为空查询所有", RemarkFrom.System)]
        [Route("/system/group/list")]
        public async  Task<ActionResult> Find(SystemGroupFindInput input)
        {
            #region 获取权限控制器信息
            input.DataSql = (await _permissionLogic.FindDataPermissionSql(new ViewRote
            {
                UserId = CurrentUser.UserId,
                MenuId = ResourceMenuId.组维护.ToGuid()
            })).Data;
            #endregion
            return JsonForGridPaging(await _groupLogic.Find(input));
        }

        /// <summary>
        /// 读取组织机构树
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("组织机构维护-方法-列表-读取组织机构树", RemarkFrom.System)]
        [Route("/system/group/organization")]
        public async  Task<ActionResult> FindOrganization()
        {
            #region 获取权限控制器信息
            SystemOrganizationDataPermissionInput input = new SystemOrganizationDataPermissionInput
            {
                PrincipalUser = CurrentUser,
                DataSql = (await _permissionLogic.FindDataPermissionSql(new ViewRote
                {
                    UserId = CurrentUser.UserId,
                    MenuId = ResourceMenuId.组维护.ToGuid()
                })).Data
            };
            #endregion
            return JsonForTree((await _organizationLogic.FindDataPermission(input)).Data.ToList());
        }

        /// <summary>
        /// 保存组数据
        /// </summary>
        /// <param name="group">组信息</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("组维护-方法-新增/编辑-保存", RemarkFrom.System)]
        [Route("/system/group")]
        public async  Task<ActionResult> Save(SystemGroup group)
        {
            return Ok(await _groupLogic.Save(group, EnumGroupBelongTo.系统));
        }

        /// <summary>
        /// 根据id获取值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("组维护-方法-列表-根据id获取值", RemarkFrom.System)]
        [Route("/system/group/{id}")]
        public async  Task<ActionResult> FindById([FromRoute] IdInput input)
        {
            return Ok(await _groupLogic.FindById(input));
        }

        /// <summary>
        /// 删除组数据
        /// </summary>
        /// <param name="input">组Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("组维护-方法-新增/编辑-删除", RemarkFrom.System)]
        [Route("/system/group/delete")]
        public async  Task<ActionResult> Delete( IdInput<string> input)
        {
            return Ok(await _groupLogic.Delete(input));
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="input">复制信息</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("组维护-方法-列表-复制", RemarkFrom.System)]
        [Route("/system/group/copy")]
        public async  Task<ActionResult> Copy(SystemCopyInput input)
        {
            return Ok(await _groupLogic.Copy(input));
        }

        /// <summary>
        /// 冻结
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("组维护-方法-冻结", RemarkFrom.System)]
        [Route("/system/group/isfreeze")]
        public async  Task<ActionResult> IsFreeze(IdInput input)
        {
            return Ok(await _groupLogic.IsFreeze(input));
        }

        /// <summary>
        /// 所有未冻结组
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("组维护-方法-冻结", RemarkFrom.System)]
        [Route("/system/group/all")]
        public async  Task<ActionResult> All()
        {
            return Ok(await _groupLogic.All());
        }
        #endregion
    }
}