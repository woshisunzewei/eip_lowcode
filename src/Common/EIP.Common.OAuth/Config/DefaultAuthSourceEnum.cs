using System.ComponentModel;

namespace EIP.Common.OAuth.Enums
{
    public enum DefaultAuthSourceEnum
    {
        [Description("微信公众平台")]
        WECHAT_MP,

        [Description("微信开放平台")]
        WECHAT_OPEN,

        [Description("企业微信自动授权")]
        WECHAT_ENTERPRISE,

        [Description("企业微信扫码")]
        WECHAT_ENTERPRISE_SCAN,

        [Description("钉钉扫码")]
        DINGTALK_SCAN,
    }
}