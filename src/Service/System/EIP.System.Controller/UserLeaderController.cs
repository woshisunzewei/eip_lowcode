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
using EIP.System.Models.Dtos.UserLeader;

namespace EIP.System.Controller
{
    /// <summary>
    /// 用户领导设置
    /// </summary>

    public class UserLeaderController : BaseSystemController
    {
        #region 构造函数
        private readonly ISystemPermissionLogic _permissionLogic;
        private readonly ISystemUserLeaderLogic _userLeaderLogic;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userLeaderLogic"></param>
        /// <param name="permissionLogic"></param>        
        public UserLeaderController(ISystemUserLeaderLogic userLeaderLogic,
            ISystemPermissionLogic permissionLogic)
        {
            _userLeaderLogic = userLeaderLogic;
            _permissionLogic = permissionLogic;
        }

        #endregion

        #region 方法
        /// <summary>
        /// 获取所有用户上级
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Remark("用户上级-视图-获取所有用户上级", RemarkFrom.System)]
        [HttpGet]
        [Route("/system/userleader/{id}")]
        public async  Task<ActionResult> Find([FromRoute]IdInput input)
        {
            return Ok(await _userLeaderLogic.GetUserLeader(input));
        }

        /// <summary>
        /// 保存所有上级
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Remark("用户上级-视图-保存所有上级", RemarkFrom.System)]
        [HttpPost]
        [Route("/system/userleader")]
        public async  Task<ActionResult> Save(SystemUserLeaderSaveInput input)
        {
            return Ok(await _userLeaderLogic.Save(input));
        }

        /// <summary>
        /// 保存所有下级
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Remark("用户上级-视图-保存所有下级", RemarkFrom.System)]
        [HttpPost]
        [Route("/system/userleader/subordinate")]
        public async  Task<ActionResult> SaveSubordinate(SystemUserLeaderSaveInput input)
        {
            return Ok(await _userLeaderLogic.SaveSubordinate(input));
        }

        /// <summary>
        /// 获取所有用户下级
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [CreateBy("孙泽伟")]
        [Remark("用户上级-视图-获取所有用户下级", RemarkFrom.System)]
        [HttpGet]
        [Route("/system/userleader/subordinate/{id}")]
        public async  Task<ActionResult> FindSubordinate([FromRoute]IdInput input)
        {
            return Ok(await _userLeaderLogic.FindSubordinate(input));
        }
       
        #endregion
    }
}