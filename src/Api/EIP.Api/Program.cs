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

//�����ļ�
ConfigurationUtil.Configuration = builder.Configuration;
builder.Configuration.AddJsonFile($"appsettings.json", optional: true, reloadOnChange: true);

//����Logo
builder.Services.AddConsoleLogoSetUp();

//ѹ��
builder.Services.AddCompressionSetUp();

//����
builder.Services.AddCachingSetUp();

//����
builder.Services.AddCorsSetUp();

//Swagger
builder.Services.AddSwaggerSetUp();

//��֤��
builder.Configuration.AddJsonFile($"Settings/Captcha.json", optional: true, reloadOnChange: true);
builder.Services.AddCaptchaSetUp();

//��־
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

//΢��
builder.Services.AddSenparcGlobalServices(ConfigurationUtil.Configuration)
   .AddSenparcWeixinServices(ConfigurationUtil.Configuration);

//Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(ConfigureContainer);

//SignalR
builder.Services.AddSignalR();

//����
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
//��Ϣ����
builder.Services.AddCapSetUp();

//������������Դ��
builder.Configuration.AddJsonFile($"Settings/DataBase.json", optional: true, reloadOnChange: true);

var app = builder.Build();
app.UseSession();
//ʹ�þ�̬������
app.UseStaticHttpContext();
//ʹ��ѹ����Ӧ�м��
app.UseResponseCompression();

#region ΢�ſ���ƽ̨
builder.Configuration.AddJsonFile($"Settings/Senparc.json", optional: true, reloadOnChange: true);
var senparcWeixinSetting = app.Services.GetService<IOptions<SenparcWeixinSetting>>()!.Value;
//����΢�����ã����룩
var registerService = app.UseSenparcWeixin(app.Environment,
    null /* ��Ϊ null �򸲸� appsettings  �е� SenpacSetting ����*/,
    null /* ��Ϊ null �򸲸� appsettings  �е� SenpacWeixinSetting ����*/,
    register => { /* CO2NET ȫ������ */ },
    (register, weixinSetting) =>
    {
        //ע�ṫ�ں���Ϣ������ִ�ж�Σ�ע�������ںţ�
        register.RegisterMpAccount(weixinSetting, "��EIP�����ں�");
        register.RegisterWorkAccount(weixinSetting, "��EIP����ҵ΢��");
        register.RegisterWxOpenAccount(weixinSetting, "��EIP��С����");
    });

//MessageHandler �м�����ܣ�https://www.cnblogs.com/szw/p/Wechat-MessageHandler-Middleware.html
//ʹ�ù��ںŵ� MessageHandler �м����������Ҫ���� Controller��
app.UseMessageHandlerForMp("/WeixinAsync", CustomMessageHandler.GenerateMessageHandler, options =>
{
    //��ȡĬ��΢������
    var weixinSetting = Senparc.Weixin.Config.SenparcWeixinSetting;

    //[����] ����΢������
    options.AccountSettingFunc = context => weixinSetting;

    //[��ѡ] ��������ı����Ȼظ����ƣ����������ÿͷ��ӿڷ����λظ���
    options.TextResponseLimitOptions = new TextResponseLimitOptions(2048, weixinSetting.WeixinAppId);
});
//ʹ����ҵ΢�ŵ� MessageHandler �м����������Ҫ���� Controller��
app.UseMessageHandlerForWork("/WorkAsync", WorkCustomMessageHandler.GenerateMessageHandler, options =>
{
    //��ȡĬ��΢������
    var weixinSetting = Senparc.Weixin.Config.SenparcWeixinSetting;

    //[����] ����΢������
    options.AccountSettingFunc = context => weixinSetting;

    //[��ѡ] ��������ı����Ȼظ����ƣ����������ÿͷ��ӿڷ����λظ���
    var appKey = AccessTokenContainer.BuildingKey(weixinSetting.WorkSetting);
    options.TextResponseLimitOptions = new TextResponseLimitOptions(2048, appKey);
});

//ʹ��С����� MessageHandler �м����������Ҫ���� Controller��
app.UseMessageHandlerForWxOpen("/WxOpenAsync", CustomWxOpenMessageHandler.GenerateMessageHandler, options =>
{
    //��ȡĬ��΢������
    var weixinSetting = Senparc.Weixin.Config.SenparcWeixinSetting;

    //[����] ����΢������
    options.AccountSettingFunc = context => weixinSetting;

    //[��ѡ] ��������ı����Ȼظ����ƣ����������ÿͷ��ӿڷ����λظ���
    options.TextResponseLimitOptions = new TextResponseLimitOptions(2048, weixinSetting.WxOpenAppId);
});
#endregion

#region �������
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

#region �м��
//����
app.UseErrorHandlingMiddleware();

//����
app.UseRequestProviderMiddleware();
#endregion

#region ��ҵ
await StdSchedulerManager.Init();
StdSchedulerManager.Start();
#endregion

#region �������ݿ�
//SqlMapper.AddTypeHandler(new SqlGuidTypeHandler());
//SqlMapper.RemoveTypeMap(typeof(Guid));
//SqlMapper.RemoveTypeMap(typeof(Guid?));
#endregion

app.UseRouting();
app.UseAuthorization();

//����
app.UseMiddleware<CustomizationLimitMiddleware>();

//������̬�ļ�����Ĭ��Ϊwwwroot·��
app.UseStaticFiles();

app.MapHub<WebSiteHub>("/eiphub");
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.Run();

#region Autofacע��
/// <summary>
/// Autofacע��
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
