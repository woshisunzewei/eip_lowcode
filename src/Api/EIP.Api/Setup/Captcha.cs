namespace EIP.Api.Setup
{
    /// <summary>
    /// 验证码
    /// </summary>
    public static class Captcha
    {
        /// <summary>
        /// 验证码
        /// </summary>
        /// <param name="services"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddCaptchaSetUp(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            services.AddCaptcha(ConfigurationUtil.Configuration);
        }
    }
}
