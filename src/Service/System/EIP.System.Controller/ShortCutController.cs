/**************************************************************
* Copyright (C) www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2018/10/30 22:40:15
* 文件名: 
* 描述: 
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.System.Models.Dtos.ShortCut;

namespace EIP.System.Controller
{
    /// <summary>
    /// 系统快捷方式
    /// </summary>
    public class ShortCutController : BaseSystemController
    {
        #region 构造函数
        private readonly ISystemShortCutLogic _systemShortCutLogic;

        /// <summary>
        /// 系统快捷方式构造函数
        /// </summary>
        /// <param name="systemShortCutLogic"></param>
        public ShortCutController(ISystemShortCutLogic systemShortCutLogic)
        {
            _systemShortCutLogic = systemShortCutLogic;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 一次性获取
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("系统快捷方式-方法-列表-一次性获取", RemarkFrom.System)]
        [Route("/system/shortcut/user")]
        public async  Task<ActionResult> FindByUserId(SystemShortCutFindByUserIdInput input)
        {
            input.UserId = CurrentUser.UserId;
            return Ok(await _systemShortCutLogic.FindByUserId(input));
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="input">主键信息</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("系统快捷方式-方法-编辑-保存", RemarkFrom.System)]
        [Route("/system/shortcut")]
        public async  Task<ActionResult> Save(SystemShortCutSaveInput input)
        {
            input.UserId = CurrentUser.UserId;
            return Ok(await _systemShortCutLogic.Save(input));
        }
        
        /// <summary>
        /// 保存排序号
        /// </summary>
        /// <param name="input">主键信息</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("系统快捷方式-方法-编辑-保存", RemarkFrom.System)]
        [Route("/system/shortcut/order")]
        public async  Task<ActionResult> SaveOrderNo(IdInput<string> input)
        {
            IList<SystemShortCut> shortCuts = new List<SystemShortCut>();
            var menuids = input.Id.Split(',');
            for (int i = 0; i < menuids.Length; i++)
            {
                shortCuts.Add(new SystemShortCut
                {
                    UserId = CurrentUser.UserId,
                    OrderNo = i,
                    MenuId = Guid.Parse(menuids[i])
                });
            }
            return Ok(await _systemShortCutLogic.SaveOrderNo(shortCuts));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("系统快捷方式-方法-列表-删除所有", RemarkFrom.System)]
        [Route("/system/shortcut/delall")]
        public async  Task<ActionResult> DeleteAll(SystemShortCutDeleteAllInput input)
        {
            input.UserId = CurrentUser.UserId;
            return Ok(await _systemShortCutLogic.DeleteAll(input));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("系统快捷方式-方法-列表-删除所有", RemarkFrom.System)]
        [Route("/system/shortcut/del")]
        public async  Task<ActionResult> Delete(IdInput input)
        {
            return Ok(await _systemShortCutLogic.Delete(new SystemShortCut()
            {
                MenuId = input.Id,
                UserId = CurrentUser.UserId
            }));
        }
        #endregion
    }
}
