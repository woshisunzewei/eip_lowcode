using Senparc.Weixin;
using Microsoft.AspNetCore.Mvc;
using Senparc.Weixin.Entities;
namespace EIP.System.Controller.WeChat.Mp
{
    /// <summary>
    /// 
    /// </summary>
    public class WeChatMpBaseController : ControllerBase
    {
        /// <summary>
        /// 
        /// </summary>
        protected string AppId
        {
            get
            {
                return Config.SenparcWeixinSetting.WeixinAppId;//与微信公众账号后台的AppId设置保持一致，区分大小写。
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected static ISenparcWeixinSettingForMP MpSetting
        {
            get
            {
                return Config.SenparcWeixinSetting.MpSetting;
            }
        }

    }
}
