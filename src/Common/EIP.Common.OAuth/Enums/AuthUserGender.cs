using System.ComponentModel;

namespace EIP.Common.OAuth.Enums
{
    public enum AuthUserGender
    {
        [Description("男")]
        MALE=1,
        [Description("女")]
        FEMALE = 0,
        [Description("未知")]
        UNKNOWN = -1
    }
}