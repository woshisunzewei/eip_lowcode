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
using EIP.Base.Models.Entities.WeChat;
using EIP.Common.Controller.Attribute;
using EIP.Common.Models.Dtos;
using EIP.System.Models.Dtos.WeChat.MpTemplate;
using EIP.WeChat.Logic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EIP.System.Controller.WeChat.Mp
{
    /// <summary>
    /// 微信公众号模版
    /// </summary>
    public class WeChatMpTemplateController : BaseSystemController
    {
        #region 构造函数
        private readonly IWeChatMpTemplateLogic _weChatMpTemplateLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="weChatMpTemplateLogic"></param>
        public WeChatMpTemplateController(IWeChatMpTemplateLogic weChatMpTemplateLogic)
        {
            _weChatMpTemplateLogic = weChatMpTemplateLogic;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 分页获取微信公众号模版
        /// </summary>
        /// <param name="input">分页参数</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("公众号模版-方法-列表-分页获取微信公众号模版", RemarkFrom.System)]
        [Route("/wechat/mp/template/list")]
        public async Task<ActionResult> Find(WeChatMpTemplatePagingInput input)
        {
            return JsonForGridPaging(await _weChatMpTemplateLogic.Find(input));
        }

        /// <summary>
        /// 根据id获取
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("公众号模版-方法-根据id获取", RemarkFrom.Workflow)]
        [Route("/wechat/mp/template/{id}")]
        public async Task<ActionResult> FindById([FromRoute] IdInput input)
        {
            return Ok(await _weChatMpTemplateLogic.FindById(input));
        }

        /// <summary>
        /// 保存配置信息值
        /// </summary>
        /// <param name="button">配置项信息</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("公众号模版-方法-新增/编辑-保存", RemarkFrom.Workflow)]
        [Route("/wechat/mp/template")]
        public async Task<ActionResult> Save(WeChatMpTemplate button)
        {
            return Ok(await _weChatMpTemplateLogic.Save(button));
        }

        /// <summary>
        /// 删除配置信息
        /// </summary>
        /// <param name="input">配置项主键Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("公众号模版-方法-列表-删除", RemarkFrom.Workflow)]
        [Route("/wechat/mp/template/delete")]
        public async Task<ActionResult> Delete(IdInput<string> input)
        {
            return Ok(await _weChatMpTemplateLogic.Delete(input));
        }
        #endregion
    }
}