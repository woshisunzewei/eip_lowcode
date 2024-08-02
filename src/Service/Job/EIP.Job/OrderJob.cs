using EIP.Common.Log;
using EIP.Common.Log.Handler;
using Quartz;
using System.Text;

namespace EIP.Job
{
    /// <summary>
    /// Job
    /// </summary>
    public class OrderJob : IJob
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Execute(IJobExecutionContext context)
        {
            JobDataMap data = context.JobDetail.JobDataMap;
            string correlation = data.GetString("correlation");
            StringBuilder logBuilder = new StringBuilder("开始执行定时备份作业【" + correlation + "】【" + DateTime.Now + "】");
            JobLogHandler jobLogHandler = new JobLogHandler(logBuilder.ToString(), Guid.Parse(correlation));
            jobLogHandler.WriteLog();
            LogWriter.Debug("TFS今日分润信息开始");
        }
    }
}
