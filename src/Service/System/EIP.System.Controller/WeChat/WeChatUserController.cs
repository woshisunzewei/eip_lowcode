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
using EIP.Base.Models.Resx;
using EIP.Common.Controller.Attribute;
using EIP.Common.Core.Extension;
using EIP.Common.Models;
using EIP.Common.Models.Dtos;
using EIP.System.Logic;
using EIP.System.Models.Dtos.Organization;
using EIP.System.Models.Dtos.User;
using EIP.System.Models.Dtos.WeChat.User;
using EIP.WeChat.Logic;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace EIP.System.Controller
{
    /// <summary>
    /// 授权用户
    /// </summary>
    public class WeChatUserController : BaseSystemController
    {
        #region 构造函数
        private readonly ISystemPermissionLogic _permissionLogic;
        private readonly IWeChatUserLogic _weChatUserLogic;
        private readonly ISystemUserInfoLogic _userInfoLogic;
        private readonly ISystemOrganizationLogic _organizationLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="weChatUserLogic"></param>
        /// <param name="organizationLogic"></param>
        /// <param name="userInfoLogic"></param>
        /// <param name="permissionLogic"></param>
        public WeChatUserController(IWeChatUserLogic weChatUserLogic,
             ISystemOrganizationLogic organizationLogic,
            ISystemUserInfoLogic userInfoLogic,
            ISystemPermissionLogic permissionLogic)
        {
            _organizationLogic = organizationLogic;
            _weChatUserLogic = weChatUserLogic;
            _permissionLogic = permissionLogic;
            _userInfoLogic = userInfoLogic;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 分页获取所有授权用户
        /// </summary>
        /// <param name="input">用户信息分页参数</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("微信授权用户-方法-列表-分页获取所有授权用户", RemarkFrom.System)]
        [Route("/wechat/user/list")]
        public async Task<ActionResult> Find(WeChatUserPagingInput input)
        {
            return JsonForGridPaging(await _weChatUserLogic.Find(input));
        }

        /// <summary>
        /// 绑定授权用户
        /// </summary>
        /// <param name="input">绑定</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("微信授权用户-方法-列表-绑定", RemarkFrom.System)]
        [Route("/wechat/user/bind")]
        public async Task<ActionResult> BindUser(WeChatUserBindInput input)
        {
            return Ok(await _weChatUserLogic.BindUser(input));
        }

        /// <summary>
        /// 解绑授权用户
        /// </summary>
        /// <param name="input">绑定</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("微信授权用户-方法-列表-解绑", RemarkFrom.System)]
        [Route("/wechat/user/unbind")]
        public async Task<ActionResult> UnBindUser(IdInput<string> input)
        {
            return Ok(await _weChatUserLogic.UnBindUser(input));
        }

        /// <summary>
        /// 获取绑定用户列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("用户维护-方法-列表-分页获取所有用户信息", RemarkFrom.System)]
        [Route("/wechat/user/bind/finduser")]
        public async Task<ActionResult> FindUser(WeChatUserFindBindInput input)
        {
            #region 获取权限控制器信息
            SystemUserFindCommonInput commonInput = new SystemUserFindCommonInput
            {
                DataSql = (await _permissionLogic.FindDataPermissionSql(new ViewRote { UserId = CurrentUser.UserId, MenuId = ResourceMenuId.授权用户.ToGuid() })).Data,
            };
            #endregion
            var chosenUserDtos = await _userInfoLogic.FindCommon(commonInput);

            if (input.UserId.HasValue)
            {
                foreach (var dto in chosenUserDtos.Data)
                {
                    dto.Exist = dto.UserId == input.UserId;
                }
            }
            chosenUserDtos.Data = chosenUserDtos.Data.ToList().OrderByDescending(w => w.Exist).ToList();
            return Ok(chosenUserDtos);
        }

        /// <summary>
        /// 获取绑定用户组织机构树
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("组织机构维护-方法-列表-读取组织机构树", RemarkFrom.System)]
        [Route("/wechat/user/bind/findorganization")]
        public async Task<ActionResult> FindOrganization()
        {
            #region 获取权限控制器信息
            SystemOrganizationDataPermissionInput input = new SystemOrganizationDataPermissionInput
            {
                PrincipalUser = CurrentUser,
                DataSql = (await _permissionLogic.FindDataPermissionSql(new ViewRote
                {
                    UserId = CurrentUser.UserId,
                    MenuId = ResourceMenuId.授权用户.ToGuid()
                })).Data
            };
            #endregion
            return JsonForTree((await _organizationLogic.FindDataPermission(input)).Data.ToList());
        }
        #endregion
    }
}