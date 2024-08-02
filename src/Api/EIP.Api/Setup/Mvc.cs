namespace EIP.Api.Setup
{
    /// <summary>
    /// Mvc
    /// </summary>
    public static class Mvc
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public static void AddMvcSetUp(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            services.Configure<KestrelServerOptions>(x => x.AllowSynchronousIO = true)
                .Configure<IISServerOptions>(x => x.AllowSynchronousIO = true);
            services.AddMvc(options =>
            {
                options.Filters.Add<ExceptionFilter>();
                options.Filters.Add<ActionResultFilter>();
            }).AddJsonOptions(options =>
            {

            }).AddApplicationPart(Assembly.Load(new AssemblyName("EIP.Workflow.Controller")))
              .AddApplicationPart(Assembly.Load(new AssemblyName("EIP.System.Controller")))
              .AddApplicationPart(Assembly.Load(new AssemblyName("EIP.DataRoom.Controller")));

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTokenJwtAuthorize();

            services.AddControllers().AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            });
        }
    }

}
