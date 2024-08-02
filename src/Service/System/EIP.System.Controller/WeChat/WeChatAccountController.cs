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
using EIP.Base.Models.Entities.System;
using EIP.Common.Controller.Attribute;
using EIP.Common.Models.Dtos;
using EIP.WeChat.Logic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EIP.System.Controller
{
    /// <summary>
    /// 帐号模版
    /// </summary>
    public class WeChatAccountController : BaseSystemController
    {
        #region 构造函数
        private readonly IWeChatAccountLogic _weChatAccountLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="weChatAccountLogic"></param>
        public WeChatAccountController(IWeChatAccountLogic weChatAccountLogic)
        {
            _weChatAccountLogic = weChatAccountLogic;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 分页获取帐号模版
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("公众号模版-方法-列表-分页获取帐号模版", RemarkFrom.System)]
        [Route("/wechat/account/list")]
        public async  Task<ActionResult> Find()
        {
            return JsonForGridLoadOnce(await _weChatAccountLogic.FindAllAsync());
        }

        /// <summary>
        /// 根据id获取
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("公众号模版-方法-根据id获取", RemarkFrom.Workflow)]
        [Route("/wechat/account/{id}")]
        public async  Task<ActionResult> FindById([FromRoute] IdInput input)
        {
            return Ok(await _weChatAccountLogic.FindById(input));
        }

        /// <summary>
        /// 保存配置信息值
        /// </summary>
        /// <param name="button">配置项信息</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("公众号模版-方法-新增/编辑-保存", RemarkFrom.Workflow)]
        [Route("/wechat/account")]
        public async  Task<ActionResult> Save(SystemUserInfoThree button)
        {
            return Ok(await _weChatAccountLogic.Save(button));
        }

        /// <summary>
        /// 删除配置信息
        /// </summary>
        /// <param name="input">配置项主键Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("公众号模版-方法-列表-删除", RemarkFrom.Workflow)]
        [Route("/wechat/account/delete")]
        public async  Task<ActionResult> Delete(IdInput<string> input)
        {
            return Ok(await _weChatAccountLogic.Delete(input));
        }
        #endregion
    }
}