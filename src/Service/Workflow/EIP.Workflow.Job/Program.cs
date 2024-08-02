using EIP.Workflow.Job.Jobs;
using Microsoft.Extensions.Configuration;
using NLog;
using Quartz;
using Quartz.Impl;
using Topshelf;

namespace EIP.Workflow.Job
{
    class Program
    {
        static void Main(string[] args)
        {
            var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json");
            var config = builder.Build();
            Config.ConnectionString = config.GetSection("EIP:ConnectionString").Value;

            HostFactory.Run(x =>
            {
                x.Service<TownCrier>(s =>
                {
                    s.ConstructUsing(name => new TownCrier()); //配置一个完全定制的服务,对Topshelf没有依赖关系。常用的方式。
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });
                x.RunAsLocalSystem(); //7服务使用NETWORK_SERVICE内置帐户运行。身份标识，有好几种方式，如：x.RunAs("username", "password");  x.RunAsPrompt(); x.RunAsNetworkService(); 等
                x.SetDescription("EIP权限工作流平台定时执行处理"); //8安装服务后，服务的描述
                x.SetDisplayName("EIP权限工作流平台"); //9显示名称
                x.SetServiceName("EIP权限工作流平台"); //服务名称                  
            });
        }
    }

    public class TownCrier
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

        public TownCrier()
        {
        }

        public void Start()
        {
            ISchedulerFactory factory = new StdSchedulerFactory();
            IScheduler scheduler = factory.GetScheduler().GetAwaiter().GetResult();
            scheduler.Start();

            //2、创建一个任务
            IJobDetail userRoyaltyOrderJob = JobBuilder.Create<MonitorJob>().WithIdentity("eipJobJob", "eipJobJobGroup").Build();

            //3、创建一个触发器:一分钟执行一次
            ITrigger eipJobTrigger = TriggerBuilder.Create().WithIdentity("eipJobTrigger", "eipJobJobGroup").WithCronSchedule("0 * * * * ? ").Build();

            //4、将任务与触发器添加到调度器中
            scheduler.ScheduleJob(userRoyaltyOrderJob, eipJobTrigger);

            //5、开始执行
            scheduler.Start();
            Logger.Debug("启动定时服务");
        }

        public void Stop()
        {
            Logger.Debug("停止定时服务");
        }
    }
}