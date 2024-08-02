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
using EIP.Common.Models;
using EIP.Common.Models.Dtos;
using EIP.System.Models.Dtos.WeChat.MpTemplateSend;
using EIP.WeChat.Logic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace EIP.System.Controller.WeChat.Mp
{
    /// <summary>
    /// 微信公众号模版发送发送
    /// </summary>
    public class WeChatMpTemplateSendController : BaseSystemController
    {
        #region 构造函数
        private readonly IWeChatMpTemplateSendLogic _weChatMpTemplateSendLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="weChatMpTemplateSendLogic"></param>
        public WeChatMpTemplateSendController(IWeChatMpTemplateSendLogic weChatMpTemplateSendLogic)
        {
            _weChatMpTemplateSendLogic = weChatMpTemplateSendLogic;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 分页获取微信公众号模版发送
        /// </summary>
        /// <param name="input">分页参数</param>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("公众号模版发送-方法-列表-获取微信公众号模版发送", RemarkFrom.System)]
        [Route("/wechat/mp/template/send/list/{id}")]
        public async Task<ActionResult> Find([FromRoute] IdInput input)
        {
            return JsonForGridLoadOnce(await _weChatMpTemplateSendLogic.FindAllAsync(f => f.TemplateId == input.Id));
        }

        /// <summary>
        /// 根据id获取
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("公众号模版发送-方法-根据id获取", RemarkFrom.Workflow)]
        [Route("/wechat/mp/template/send/{id}")]
        public async Task<ActionResult> FindById([FromRoute] IdInput input)
        {
            return Ok(OperateStatus<WeChatMpTemplateSend>.Success(await _weChatMpTemplateSendLogic.FindAsync(f => f.TemplateSendId == input.Id)));
        }

        /// <summary>
        /// 保存配置信息值
        /// </summary>
        /// <param name="input">配置项信息</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("公众号模版发送-方法-新增/编辑-保存", RemarkFrom.Workflow)]
        [Route("/wechat/mp/template/send")]
        public async Task<ActionResult> Save(WeChatMpTemplateSend input)
        {
            return Ok(await _weChatMpTemplateSendLogic.Save(input));
        }

        /// <summary>
        /// 删除配置信息
        /// </summary>
        /// <param name="input">配置项主键Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("公众号模版发送-方法-列表-删除", RemarkFrom.Workflow)]
        [Route("/wechat/mp/template/send/delete")]
        public async Task<ActionResult> Delete(IdInput<string> input)
        {
            return Ok(await _weChatMpTemplateSendLogic.Delete(input));
        }

        /// <summary>
        /// 公众号模版发送
        /// </summary>
        /// <param name="input">配置项主键Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("公众号模版发送-方法-列表-发送", RemarkFrom.Workflow)]
        [Route("/wechat/mp/template/send/save/{id}")]
        public async Task<ActionResult> Send([FromRoute] IdInput input)
        {
            return Ok(await _weChatMpTemplateSendLogic.Send(input));
        }

        /// <summary>
        /// 公众号模版发送
        /// </summary>
        /// <param name="input">配置项主键Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("公众号模版发送-方法-列表-发送", RemarkFrom.Workflow)]
        [Route("/wechat/mp/template/send/preview")]
        public async Task<ActionResult> SendPreview(WechatMpTemplateSendPreviewInput input)
        {
            return Ok(await _weChatMpTemplateSendLogic.SendPreview(input));
        }
        #endregion
    }
}