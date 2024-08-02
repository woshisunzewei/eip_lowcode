using EIP.System.Controller.WeChat.Open.MessageHandlers;
using EIP.System.Controller.WeChat.Work.MessageHandlers;
using Microsoft.AspNetCore.Http.Features;
using Senparc.NeuChar.MessageHandlers;
using Senparc.Weixin.Work.Containers;
using Senparc.Weixin.Work.MessageHandlers.Middleware;
using Senparc.Weixin.WxOpen;
using Senparc.Weixin.WxOpen.MessageHandlers.Middleware;

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseUrls("http://*:5000");

//配置文件
ConfigurationUtil.Configuration = builder.Configuration;
builder.Configuration.AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true);

//启动Logo
builder.Services.AddConsoleLogoSetUp();

//压缩
builder.Services.AddCompressionSetUp();

//缓存
builder.Services.AddCachingSetUp();

//跨域
builder.Services.AddCorsSetUp();

//Swagger
builder.Services.AddSwaggerSetUp();

//验证码
builder.Configuration.AddJsonFile($"Settings/Captcha.json", optional: true, reloadOnChange: true);
builder.Services.AddCaptchaSetUp();

//日志
builder.Services.AddNLogSetUp();
builder.Logging.ClearProviders();
builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Information);
builder.Host.UseNLog();

//Jwt
builder.Services.AddApiJwtAuthorize((context) =>
{
    return true;
});

//Session
builder.Services.AddSession();

//Mvc
builder.Services.AddMvcSetUp();

//微信
builder.Services.AddSenparcGlobalServices(ConfigurationUtil.Configuration)
   .AddSenparcWeixinServices(ConfigurationUtil.Configuration);

//Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(ConfigureContainer);

//SignalR
builder.Services.AddSignalR();

//限流
builder.Configuration.AddJsonFile($"Settings/RateLimit.json", optional: true, reloadOnChange: true);
builder.Services.Configure<IpRateLimitOptions>(ConfigurationUtil.Configuration.GetSection("IpRateLimiting"));
builder.Services.Configure<IpRateLimitPolicies>(ConfigurationUtil.Configuration.GetSection("IpRateLimitPolicies"));
builder.Services.AddSingleton<IIpPolicyStore, DistributedCacheIpPolicyStore>();
builder.Services.AddSingleton<IRateLimitCounterStore, DistributedCacheRateLimitCounterStore>();
builder.Services.AddInMemoryRateLimiting();
builder.Services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = long.MaxValue;
    options.ValueLengthLimit = int.MaxValue;
    options.MemoryBufferThreshold = int.MaxValue;
});
//消息订阅
builder.Services.AddCapSetUp();

//第三方数据来源库
builder.Configuration.AddJsonFile($"Settings/DataBase.json", optional: true, reloadOnChange: true);

var app = builder.Build();
app.UseSession();
//使用静态上下文
app.UseStaticHttpContext();
//使用压缩响应中间件
app.UseResponseCompression();

#region 微信开放平台
builder.Configuration.AddJsonFile($"Settings/Senparc.json", optional: true, reloadOnChange: true);
var senparcWeixinSetting = app.Services.GetService<IOptions<SenparcWeixinSetting>>()!.Value;
//启用微信配置（必须）
var registerService = app.UseSenparcWeixin(app.Environment,
    null /* 不为 null 则覆盖 appsettings  中的 SenpacSetting 配置*/,
    null /* 不为 null 则覆盖 appsettings  中的 SenpacWeixinSetting 配置*/,
    register => { /* CO2NET 全局配置 */ },
    (register, weixinSetting) =>
    {
        //注册公众号信息（可以执行多次，注册多个公众号）
        register.RegisterMpAccount(weixinSetting, "【EIP】公众号");
        register.RegisterWorkAccount(weixinSetting, "【EIP】企业微信");
        register.RegisterWxOpenAccount(weixinSetting, "【EIP】小程序");
    });

//MessageHandler 中间件介绍：https://www.cnblogs.com/szw/p/Wechat-MessageHandler-Middleware.html
//使用公众号的 MessageHandler 中间件（不再需要创建 Controller）
app.UseMessageHandlerForMp("/WeixinAsync", CustomMessageHandler.GenerateMessageHandler, options =>
{
    //获取默认微信配置
    var weixinSetting = Senparc.Weixin.Config.SenparcWeixinSetting;

    //[必须] 设置微信配置
    options.AccountSettingFunc = context => weixinSetting;

    //[可选] 设置最大文本长度回复限制（超长后会调用客服接口分批次回复）
    options.TextResponseLimitOptions = new TextResponseLimitOptions(2048, weixinSetting.WeixinAppId);
});
//使用企业微信的 MessageHandler 中间件（不再需要创建 Controller）
app.UseMessageHandlerForWork("/WorkAsync", WorkCustomMessageHandler.GenerateMessageHandler, options =>
{
    //获取默认微信配置
    var weixinSetting = Senparc.Weixin.Config.SenparcWeixinSetting;

    //[必须] 设置微信配置
    options.AccountSettingFunc = context => weixinSetting;

    //[可选] 设置最大文本长度回复限制（超长后会调用客服接口分批次回复）
    var appKey = AccessTokenContainer.BuildingKey(weixinSetting.WorkSetting);
    options.TextResponseLimitOptions = new TextResponseLimitOptions(2048, appKey);
});

//使用小程序的 MessageHandler 中间件（不再需要创建 Controller）
app.UseMessageHandlerForWxOpen("/WxOpenAsync", CustomWxOpenMessageHandler.GenerateMessageHandler, options =>
{
    //获取默认微信配置
    var weixinSetting = Senparc.Weixin.Config.SenparcWeixinSetting;

    //[必须] 设置微信配置
    options.AccountSettingFunc = context => weixinSetting;

    //[可选] 设置最大文本长度回复限制（超长后会调用客服接口分批次回复）
    options.TextResponseLimitOptions = new TextResponseLimitOptions(2048, weixinSetting.WxOpenAppId);
});
#endregion

#region 跨域相关
app.UseCors("EIPCors");
#endregion

#region Swagger
app.UseSwaggerAuthorized();
app.UseSwagger();
app.UseKnife4UI(c =>
{
    c.RoutePrefix = string.Empty;
    c.SwaggerEndpoint("/swagger/api/swagger.json", "EIP.Api v1");
});
#endregion

#region 中间件
//错误
app.UseErrorHandlingMiddleware();

//请求
app.UseRequestProviderMiddleware();
#endregion

#region 作业
await StdSchedulerManager.Init();
StdSchedulerManager.Start();
#endregion

#region 达梦数据库
//SqlMapper.AddTypeHandler(new SqlGuidTypeHandler());
//SqlMapper.RemoveTypeMap(typeof(Guid));
//SqlMapper.RemoveTypeMap(typeof(Guid?));
#endregion

app.UseRouting();
app.UseAuthorization();

//限流
app.UseMiddleware<CustomizationLimitMiddleware>();

//开启静态文件服务默认为wwwroot路径
app.UseStaticFiles();

app.MapHub<WebSiteHub>("/eiphub");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();

#region Autofac注册
/// <summary>
/// Autofac注册
/// </summary>
/// <param name="builder"></param>
void ConfigureContainer(ContainerBuilder builder)
{
    var assemblys = new List<Assembly>();
    var libs = DependencyContext.Default.CompileLibraries.Where(w => w.Name.StartsWith("EIP"));
    foreach (var lib in libs)
    {
        try
        {
            var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(lib.Name));
            assemblys.Add(assembly);
        }
        catch (Exception)
        {

        }
    }
    builder.RegisterAssemblyTypes(assemblys.ToArray()).Where(t => t.Namespace.IsNotNullOrEmpty() && t.Namespace.StartsWith("EIP") && t.Name.EndsWith("Logic")).AsImplementedInterfaces();
    builder.RegisterAssemblyTypes(assemblys.ToArray()).Where(t => t.Namespace.IsNotNullOrEmpty() && t.Namespace.StartsWith("EIP") && t.Name.EndsWith("Repository")).AsImplementedInterfaces();
    builder.ConfigureCastleInterceptor();
}
#endregion
