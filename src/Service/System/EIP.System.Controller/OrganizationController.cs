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
    /// 组织机构
    /// </summary>

    public class OrganizationController : BaseSystemController
    {
        #region 构造函数
        private readonly ISystemOrganizationLogic _organizationLogic;
        private readonly ISystemPermissionLogic _permissionLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationLogic"></param>
        /// <param name="permissionLogic"></param>
        public OrganizationController(ISystemOrganizationLogic organizationLogic,
            ISystemPermissionLogic permissionLogic)
        {
            _organizationLogic = organizationLogic;
            _permissionLogic = permissionLogic;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 读取组织机构树上下级关系
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("组织机构维护-方法-列表-读取组织机构树上下级关系", RemarkFrom.System)]
        [Route("/system/organization/range/{range}")]
        public async Task<ActionResult> FindOrganizationRange([FromRoute] int range = 0)
        {
            #region 获取权限控制器信息
            string sql = " 1=1 ";
            if (!CurrentUser.IsAdmin)
            {
                switch (range)
                {
                    //当前部门及下级部门
                    case 1:
                        sql = $" org.ParentIds like '%{CurrentUser.OrganizationId}%'";
                        break;
                    //下级部门
                    case 2:
                        sql = $" org.ParentIds like '%{CurrentUser.OrganizationId},%'";
                        break;
                    //当前公司
                    case 3:
                    case 4:
                    case 5:
                        sql = $" org.Nature=1 ";
                        break;
                }
            }
            SystemOrganizationDataPermissionInput input = new SystemOrganizationDataPermissionInput
            {
                PrincipalUser = CurrentUser,
                DataSql = sql,
                Range = range
            };
            #endregion
            return JsonForTree((await _organizationLogic.FindDataPermissionTree(input)).Data);
        }

        /// <summary>
        /// 读取树结构:排除下级
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("用户控件-方法-读取组织机构树:排除下级", RemarkFrom.System)]
        [Route("/system/organization/toporg")]
        public async Task<ActionResult> FindTreeAll(SystemOrganizationTreeInput input)
        {
            return JsonForTree((await _organizationLogic.FindOrganizationTree(input)).Data);
        }

        /// <summary>
        /// 读取树结构:排除下级
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("用户控件-方法-读取组织机构树:排除下级", RemarkFrom.System)]
        [Route("/system/organization")]
        public async Task<ActionResult> Tree()
        {
            #region 获取权限控制器信息
            SystemOrganizationDataPermissionInput input = new SystemOrganizationDataPermissionInput
            {
                PrincipalUser = CurrentUser,
                DataSql = (await _permissionLogic.FindDataPermissionSql(new ViewRote
                {
                    UserId = CurrentUser.UserId,
                    MenuId = ResourceMenuId.组织架构.ToGuid()
                })).Data
            };
            #endregion
            return JsonForTree((await _organizationLogic.FindDataPermission(input)).Data);
        }

        /// <summary>
        /// 读取组织机构树
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("组织机构维护-方法-新增/编辑-读取组织机构树", RemarkFrom.System)]
        [Route("/system/organization/list")]
        public async Task<ActionResult> Find(SystemOrganizationDataPermissionInput input)
        {
            #region 获取权限控制器信息
            input.DataSql = (await _permissionLogic.FindDataPermissionSql(new ViewRote
            {
                UserId = CurrentUser.UserId,
                MenuId = ResourceMenuId.组织架构.ToGuid()
            })).Data;
            #endregion
            return Ok(await _organizationLogic.Find(input));
        }
        /// <summary>
        /// 读取树结构:排除下级
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("用户控件-方法-读取组织机构树:排除下级", RemarkFrom.System)]
        [Route("/system/organization/removechildren/{id}")]
        public async Task<ActionResult> FindRemoveChildren([FromRoute] Guid id)
        {
            SystemOrganizationDataPermissionInput input = new SystemOrganizationDataPermissionInput
            {
                PrincipalUser = CurrentUser,
                DataSql = (await _permissionLogic.FindDataPermissionSql(new ViewRote
                {
                    UserId = CurrentUser.UserId,
                    MenuId = ResourceMenuId.组织架构.ToGuid()
                })).Data
            };
            var orgs = (await _organizationLogic.FindDataPermission(input)).Data.ToList();
            if (!id.IsEmptyGuid())
            {
                orgs = orgs.Where(w => w.parents != null && !w.parents.Contains(id.ToString())).ToList();
            }
            return JsonForTree(orgs);
        }

        /// <summary>
        /// 保存组织机构数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("组织机构维护-方法-新增/编辑-保存", RemarkFrom.System)]
        [Route("/system/organization")]
        public async Task<ActionResult> Save(SystemOrganization input)
        {
            return Ok(await _organizationLogic.Save(input));
        }

        /// <summary>
        /// 删除组织机构
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("组织机构维护-方法-列表-删除", RemarkFrom.System)]
        [Route("/system/organization/delete")]
        public async Task<ActionResult> Delete(IdInput<string> input)
        {
            return Ok(await _organizationLogic.Delete(input));
        }
        /// <summary>
        /// 编辑/修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("组织机构维护-方法-列表-根据id获取值", RemarkFrom.System)]
        [Route("/system/organization/{id}")]
        public async Task<ActionResult> FindById([FromRoute] IdInput input)
        {
            return Ok(await _organizationLogic.FindById(input));
        }

        /// <summary>
        /// 冻结
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("组织机构维护-方法-冻结", RemarkFrom.System)]
        [Route("/system/organization/isfreeze")]
        public async Task<ActionResult> IsFreeze(IdInput input)
        {
            return Ok(await _organizationLogic.IsFreeze(input));
        }
        #endregion

        #region 导入导出

        /// <summary>
        /// 下载导入模版
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("组织机构维护-方法-下载导入模版", RemarkFrom.System)]
        [Route("/system/organization/import/template")]
        public FileResult DownImportTemplate()
        {
            return GenerateTemplate<SystemOrganizationImportDto>();
        }

        /// <summary>
        /// 导入
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [RequestSizeLimit(100_000_000)] //最大100m左右
        [CreateBy("孙泽伟")]
        [Remark("组织机构维护-方法-导入", RemarkFrom.System)]
        [Route("/system/organization/import")]
        public async Task<ActionResult> Import()
        {
            var result = Import<SystemOrganizationImportDto>();
            if (result.Code == ResultCode.Success)
            {
                return Ok(await _organizationLogic.Import(result.Data));
            }
            else
            {
                return Ok(result);
            }
        }
        #endregion
    }
}