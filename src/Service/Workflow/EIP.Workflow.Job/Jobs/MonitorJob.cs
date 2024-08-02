using EIP.Workflow.Job.Fixture;
using NLog;
using Quartz;
using System;
using System.Threading.Tasks;

namespace EIP.Workflow.Job.Jobs
{
    /// <summary>
    /// 监控Job
    /// </summary>
    public class MonitorJob : IJob
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        public Task Execute(IJobExecutionContext context)
        {
            Logger.Debug("周期计算:开始每天计算下个阶段周期");
            try
            {
                using (var fix = new MsSqlDatabaseFixture())
                {

                }
            }
            catch (Exception e)
            {
                Logger.Error("自动计算订单提成发生异常:" + e.Message);
            }
            Logger.Debug("自动计算订单提成:当操作多少天后则自动算入结束");
            return Task.CompletedTask;
        }

        /// <summary>
        /// 超时发送信息
        /// </summary>
        /// <returns></returns>
        private /*async Task*/ void OverTime()
        {

        }
    }
}