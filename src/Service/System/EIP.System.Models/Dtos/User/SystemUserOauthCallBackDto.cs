namespace EIP.System.Models.Dtos.User
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemUserOauthCallBackInput
    {
        /// <summary>
        /// 
        /// </summary>
        public string AuthSource { get; set; }

        /// <summary>
        /// 访问AuthorizeUrl后回调时带的参数code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 访问AuthorizeUrl后回调时带的参数auth_code，该参数目前只使用于支付宝登录
        /// </summary>
        public string AuthCode { get; set; }

        /// <summary>
        /// 访问AuthorizeUrl后回调时带的参数state，用于和请求AuthorizeUrl前的state比较，防止CSRF攻击
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// 浏览器代理
        /// </summary>
        public string UserAgent { get; set; } = string.Empty;

        /// <summary>
        /// 浏览器信息
        /// </summary>
        public string Browser { get; set; } = string.Empty;
    }

    /// <summary>
    /// 
    /// </summary>
    public class SystemUserOauthCallBackOutput
    {
    }
}
