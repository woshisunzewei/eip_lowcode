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
using EIP.System.Models.Dtos.Post;
using EIP.System.Models.Dtos.Role;

namespace EIP.System.Controller
{
    /// <summary>
    /// 岗位控制器
    /// </summary>

    public class PostController : BaseSystemController
    {
        #region 构造函数
        private readonly ISystemOrganizationLogic _organizationLogic;
        private readonly ISystemPermissionLogic _permissionLogic;
        private readonly ISystemPostLogic _postLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="postLogic"></param>
        /// <param name="organizationLogic"></param>
        /// <param name="permissionLogic"></param>
        public PostController(ISystemPostLogic postLogic,
            ISystemOrganizationLogic organizationLogic,
            ISystemPermissionLogic permissionLogic)
        {
            _postLogic = postLogic;
            _organizationLogic = organizationLogic;
            _permissionLogic = permissionLogic;
        }

        #endregion

        #region 方法
        /// <summary>
        /// 根据组织机构获取岗位信息
        /// </summary>
        /// <param name="input">组织机构Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("岗位维护-方法-列表-根据组织机构获取岗位信息", RemarkFrom.System)]
        [Route("/system/post/list")]
        public async  Task<ActionResult> Find(SystemPostFindInput input)
        {
            #region 获取权限控制器信息
            input.DataSql = (await _permissionLogic.FindDataPermissionSql(new ViewRote
            {
                UserId = CurrentUser.UserId,
                MenuId = ResourceMenuId.岗位维护.ToGuid()
            })).Data;
            #endregion
            return JsonForGridPaging(await _postLogic.Find(input));
        }
       
        /// <summary>
        /// 读取组织机构树
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("组织机构维护-方法-列表-读取组织机构树", RemarkFrom.System)]
        [Route("/system/post/organization")]
        public async  Task<ActionResult> FindOrganization()
        {
            #region 获取权限控制器信息
            SystemOrganizationDataPermissionInput input = new SystemOrganizationDataPermissionInput
            {
                PrincipalUser = CurrentUser,
                DataSql = (await _permissionLogic.FindDataPermissionSql(new ViewRote { UserId = CurrentUser.UserId, MenuId = ResourceMenuId.岗位维护.ToGuid() })).Data
            };
            #endregion
            return JsonForTree((await _organizationLogic.FindDataPermission(input)).Data.ToList());
        }

        /// <summary>
        /// 编辑/修改
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("岗位维护-方法-列表-根据id获取值", RemarkFrom.System)]
        [Route("/system/post/{id}")]
        public async  Task<ActionResult> FindById([FromRoute] IdInput input)
        {
            return Ok(await _postLogic.FindById(input));
        }

        /// <summary>
        /// 保存岗位数据
        /// </summary>
        /// <param name="post">岗位信息</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("岗位维护-方法-新增/编辑-保存", RemarkFrom.System)]
        [Route("/system/post")]
        public async  Task<ActionResult> Save(SystemPost post)
        {
            return Ok(await _postLogic.Save(post));
        }

        /// <summary>
        /// 复制
        /// </summary>
        /// <param name="input">复制信息</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("岗位维护-方法-列表-复制", RemarkFrom.System)]
        [Route("/system/post/copy")]
        public async  Task<ActionResult> Copy(SystemCopyInput input)
        {
            return Ok(await _postLogic.Copy(input));
        }

        /// <summary>
        ///    删除岗位数据
        /// </summary>
        /// <param name="input">岗位Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("岗位维护-方法-列表-删除", RemarkFrom.System)]
        [Route("/system/post/delete")]
        public async  Task<ActionResult> Delete( IdInput<string> input)
        {
            return Ok(await _postLogic.Delete(input));
        }

        /// <summary>
        /// 冻结
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("岗位维护-方法-冻结", RemarkFrom.System)]
        [Route("/system/post/isfreeze")]
        public async  Task<ActionResult> IsFreeze(IdInput input)
        {
            return Ok(await _postLogic.IsFreeze(input));
        }

        /// <summary>
        /// 所有未冻结岗位
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("岗位维护-方法-冻结", RemarkFrom.System)]
        [Route("/system/post/all")]
        public async  Task<ActionResult> All()
        {
            return Ok(await _postLogic.All());
        }
        #endregion
    }
}