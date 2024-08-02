namespace EIP.Api.Setup
{
    /// <summary>
    /// 跨域
    /// </summary>
    public static class Cors
    {
        /// <summary>
        /// 跨域
        /// </summary>
        /// <param name="services"></param>
        public static void AddCorsSetUp(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.AddCors(options =>
            {
                options.AddPolicy("EIPCors", policy => policy.WithOrigins(ConfigurationUtil.GetSection("EIP:Cors").Split(',')).AllowAnyHeader().AllowAnyMethod().AllowCredentials());
            });
        }
    }
}
