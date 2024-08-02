namespace EIP.System.Models.Dtos.User
{
    /// <summary>
    /// Oauth登录获取授权地址
    /// </summary>
    public class SystemUserOauthUrlInput
    {
        /// <summary>
        /// 来源
        /// [Description("微信公众平台")]
        /// WECHAT_MP,
        /// [Description("微信开放平台")]
        /// WECHAT_OPEN,
        /// [Description("企业微信自动授权")]
        /// WECHAT_ENTERPRISE,
        /// [Description("企业微信扫码")]
        /// WECHAT_ENTERPRISE_SCAN,
        /// [Description("钉钉扫码")]
        /// DINGTALK_SCAN,
        /// </summary>
        public string Source { get; set; }
    }

    /// <summary>
    /// Oauth登录获取授权地址
    /// </summary>
    public class SystemUserOauthUrlOutput
    {
        /// <summary>
        /// 授权地址
        /// </summary>
        public string Url { get; set; }
    }

}
