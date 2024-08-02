/**************************************************************
* Copyright (C) www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2018/10/30 22:40:15
* 文件名: 
* 描述: 
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Common.Quartz.Dtos;
using EIP.System.Controller;
using Quartz;
using Quartz.Impl.Calendar;
using Quartz.Impl.Matchers;
using Quartz.Impl.Triggers;

namespace EIP.Job.Controller
{
    /// <summary>
    /// 作业
    /// </summary>
    public class JobController : BaseSystemController
    {
        /// <summary>
        /// 获取所有作业
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("任务调度-方法-列表-获取所有作业", RemarkFrom.Job)]
        [Route("/system/job/list")]
        public async Task<ActionResult> FindAllJobs()
        {
            IList<QuartzOutput> models = new List<QuartzOutput>();
            var jobGroups = await StdSchedulerManager.GetJobGroupNames();
            foreach (string group in jobGroups)
            {
                var groupMatcher = GroupMatcher<JobKey>.GroupContains(group);
                var jobKeys = await StdSchedulerManager.GetJobKeys(groupMatcher);
                foreach (var jobKey in jobKeys)
                {
                    var detail = await StdSchedulerManager.GetJobDetail(jobKey);
                    var triggers = await StdSchedulerManager.GetTriggersOfJob(jobKey);
                    foreach (ITrigger trigger in triggers)
                    {
                        var model = new QuartzOutput
                        {
                            JobGroup = group,
                            JobName = jobKey.Name,
                            JobDescription = detail.Description,
                            TriggerState = "Complete",
                            NamespaceName = detail.JobType.Namespace,
                            ClassName = detail.JobType.FullName,
                            TriggerName = trigger.Key.Name,
                            TriggerGroup = trigger.Key.Group,
                            TriggerType = trigger.GetType().Name,
                            ParametersJson = JsonConvert.SerializeObject(detail.JobDataMap.ToList())
                        };
                        model.TriggerState = (await StdSchedulerManager.GetTriggerState(trigger.Key)).ToString();
                        var nextFireTime = trigger.GetNextFireTimeUtc();
                        if (nextFireTime.HasValue)
                        {
                            model.NextFireTime = nextFireTime.Value.DateTime.ToServerLocalTime().AddHours(8);
                        }
                        var previousFireTime = trigger.GetPreviousFireTimeUtc();
                        if (previousFireTime.HasValue)
                        {
                            model.PreviousFireTime = previousFireTime.Value.DateTime.ToServerLocalTime().AddHours(8);
                        }
                        models.Add(model);
                    }
                }
            }
            return JsonForGridLoadOnce(models.OrderBy(o => o.NextFireTime));
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("任务调度-视图-编辑", RemarkFrom.Job)]
        [Route("/system/job/edit")]
        public async Task<ActionResult> JobEdit([FromBody] JobDetailInput input)
        {
            var output = new QuartzOutput();
            if (!input.JobName.IsNullOrEmpty() && !input.JobGroup.IsNullOrEmpty())
            {
                var key = new JobKey(input.JobName, input.JobGroup);
                //作业详情
                var detail = await StdSchedulerManager.GetJobDetail(key);
                output.NamespaceName = detail.JobType.Namespace;
                output.ClassName = detail.JobType.Name;
                //触发器
                var triggerKey = new TriggerKey(input.TriggerName, input.TriggerGroup);
                var trigger = await StdSchedulerManager.GetTrigger(triggerKey);

                output.JobType = detail.JobType.FullName;
                output.JobGroup = detail.Key.Group;
                output.JobName = detail.Key.Name;
                output.JobDescription = detail.Description;

                output.TriggerType = trigger.GetType().Name;
                output.TriggerName = trigger.Key.Name;
                output.TriggerGroup = trigger.Key.Group;
                output.TriggerDescription = trigger.Description;
                output.ParametersJson = JsonConvert.SerializeObject(detail.JobDataMap.ToList());
                //获取trigger类型
                switch (trigger.GetType().Name)
                {
                    case "SimpleTriggerImpl":
                        var simpleTriggerImpl = (SimpleTriggerImpl)trigger;
                        output.Interval = simpleTriggerImpl.RepeatInterval;
                        break;
                    case "CronTriggerImpl":
                        //获取表达式
                        var cronTriggerImpl = (CronTriggerImpl)trigger;
                        output.Expression = cronTriggerImpl.CronExpressionString;
                        break;
                }
                output.ReplaceExists = true;
            }

            return Ok(output);
        }

        /// <summary>
        /// 通过名称分组删除作业
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("任务调度-方法-列表-通过名称分组删除作业", RemarkFrom.Job)]
        [Route("/system/job/delete")]
        public async Task<ActionResult> DeleteJob([FromBody] JobDetailInput input)
        {
            var status = new OperateStatus();
            if (await StdSchedulerManager.DeleteJob(input.JobName, input.JobGroup))
            {
                status.Code = ResultCode.Success;
                status.Msg = Chs.Successful;
            }
            return Ok(status);
        }

        /// <summary>
        /// 通过作业名称及组名称获取作业参数
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("任务调度-方法-新增/编辑-通过名称分组删除作业", RemarkFrom.Job)]
        [Route("/system/job/detail")]
        public async Task<ActionResult> FindDetailJobDataMap([FromBody] JobDetailInput input)
        {
            if (!input.JobGroup.IsNullOrEmpty() && !input.JobName.IsNullOrEmpty())
            {
                var detail = await StdSchedulerManager.GetJobDetail(new JobKey(input.JobName, input.JobGroup));
                var maps = detail.JobDataMap;
                return Ok(maps.Select(map => new Parameters { Key = map.Key, Value = map.Value }).ToList());
            }
            return null;
        }

        /// <summary>
        /// 通过名称分组暂停作业
        /// </summary>
        /// <param name="input"></param>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("任务调度-方法-列表-通过名称分组暂停作业", RemarkFrom.Job)]
        [Route("/system/job/pause")]
        public async Task<ActionResult> PauseJob([FromBody] JobDetailInput input)
        {
            var status = new OperateStatus();
            try
            {
                await StdSchedulerManager.PauseJob(input.JobName, input.JobGroup);
                status.Code = ResultCode.Success;
                status.Msg = "暂停作业成功";
            }
            catch (Exception ex)
            {
                status.Msg = ex.Message;
            }
            return Ok(status);
        }

        /// <summary>
        /// 通过名称分组暂停所有作业
        /// </summary>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("任务调度-方法-列表-暂停所有作业", RemarkFrom.Job)]
        [Route("/system/job/pauseall")]
        public async Task<ActionResult> PauseAll()
        {
            var status = new OperateStatus();
            try
            {
                await StdSchedulerManager.PauseAll();
                status.Code = ResultCode.Success;
                status.Msg = "暂停所有作业成功";
            }
            catch (Exception ex)
            {
                status.Msg = ex.Message;
            }
            return Ok(status);
        }

        /// <summary>
        /// 通过名称分组恢复作业
        /// </summary>
        /// <param name="input"></param>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("任务调度-方法-列表-通过名称分组启动作业", RemarkFrom.Job)]
        [Route("/system/job/resume")]
        public async Task<ActionResult> ResumeJob([FromBody] JobDetailInput input)
        {
            var status = new OperateStatus();
            try
            {
                await StdSchedulerManager.ResumeJob(input.JobName, input.JobGroup);
                status.Code = ResultCode.Success;
                status.Msg = "恢复作业成功";
            }
            catch (Exception ex)
            {
                status.Msg = ex.Message;
            }
            return Ok(status);
        }

        /// <summary>
        ///    启动所有作业
        /// </summary>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("任务调度-方法-列表-启动所有作业", RemarkFrom.Job)]
        [Route("/system/job/resumeall")]
        public ActionResult ResumeAll()
        {
            var status = new OperateStatus();
            try
            {
                StdSchedulerManager.ResumeAll();
                status.Code = ResultCode.Success;
                status.Msg = "恢复作业成功";
            }
            catch (Exception ex)
            {
                status.Msg = ex.Message;
            }
            return Ok(status);
        }

        /// <summary>
        /// 保存调度作业
        /// </summary>
        /// <param name="input">调度作业实体</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("任务调度-方法-列表-保存调度作业", RemarkFrom.Job)]
        [Route("/system/job/schedule")]
        public async Task<ActionResult> ScheduleJob([FromBody] ScheduleJobInput input)
        {
            var status = new OperateStatus();
            try
            {
                if (!input.ReplaceExists)
                {
                    if (await StdSchedulerManager.CheckExists(new TriggerKey(input.TriggerName, input.TriggerGroup)))
                    {
                        status.Msg = "指定的触发器已经存在，请重新指定名称";
                        return Ok(status);
                    }
                    if (await StdSchedulerManager.CheckExists(new JobKey(input.JobName, input.JobGroup)))
                    {
                        status.Msg = "指定的作业名已经存在，请重新指定名称";
                        return Ok(status);
                    }
                }
                input.IsSave = true;
                await StdSchedulerManager.ScheduleJob(input);
                //判断之前状态
                if (input.TriggerState.IsNotNullOrEmpty())
                {
                    if (input.TriggerState == "Paused")
                    {
                        await StdSchedulerManager.PauseJob(input.JobName, input.JobGroup);
                    }
                    if (input.TriggerState == "Normal")
                    {
                        await StdSchedulerManager.ResumeJob(input.JobName, input.JobGroup);
                    }
                }
                status.Code = ResultCode.Success;
                status.Msg = "保存调度作业成功";
            }
            catch (Exception ex)
            {
                status.Msg = ex.Message;
            }
            return Ok(status);
        }

        /// <summary>
        /// 获得Corn表达式
        /// </summary>
        /// <param name="cronExpression"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("任务调度-方法-Cron-获得Corn表达式", RemarkFrom.Job)]
        [Route("/system/job/calcruntime")]
        public ActionResult CalcRunTime(string cronExpression)
        {
            try
            {
                return Ok(StdSchedulerManager.GetTaskeFireTime(cronExpression, 5));
            }
            catch
            {
                return null;
            }
        }

        #region 日历

        /// <summary>
        /// 日历编辑
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("任务调度-视图-日历编辑", RemarkFrom.Job)]
        [Route("/system/job/calendar/edit")]
        public async Task<ActionResult> CalendarEdit(string calendarName)
        {
            var model = new CalendarInput();
            if (!calendarName.IsNullOrEmpty())
            {
                model.ReplaceExists = true;
                var calendar = await StdSchedulerManager.GetCalendar(calendarName);
                var cornca = ((CronCalendar)calendar);
                model.Expression = cornca.CronExpression != null ? cornca.CronExpression.ToString() : string.Empty;
                model.Description = calendar.Description;
            }
            return Ok(model);
        }

        /// <summary>
        /// 获取所有日历
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("任务调度-方法-日历-获得所有日历", RemarkFrom.Job)]
        [Route("/system/job/calendar")]
        public async Task<ActionResult> FindCalendar()
        {
            var getCalendarNames = (await StdSchedulerManager.GetCalendarNames()).ToList();
            var calendars = getCalendarNames.Select(n => new
            {
                Name = n,
                Calendar = StdSchedulerManager.GetCalendar(n)
            }).ToDictionary(n => n.Name, n => n.Calendar);
            IList<CalendarInput> calendarModels = calendars.Select(cal => new CalendarInput
            {
                Description = cal.Value.Result.Description,
                CalendarName = cal.Key
            }).ToList();
            return JsonForGridLoadOnce(calendarModels);
        }

        /// <summary>
        /// 删除日历
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("任务调度-方法-日历-根据日历名称删除日历", RemarkFrom.Job)]
        [Route("/system/job/calendar/delete")]
        public async Task<ActionResult> DeleteCalendar(IdInput<string> input)
        {
            var status = new OperateStatus();
            try
            {
                foreach (var id in input.Id.Split(','))
                {
                    if (await StdSchedulerManager.DeleteCalendar(id))
                    {
                        status.Code = ResultCode.Success;
                        status.Msg = "删除日历成功";
                    }
                }
            }
            catch (Exception ex)
            {
                status.Msg = ex.Message;
            }
            return Ok(status);
        }

        /// <summary>
        /// 保存日历
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("任务调度-方法-日历-保存日历", RemarkFrom.Job)]
        [Route("/system/job/calendar/save")]
        public async Task<ActionResult> SaveCalendar(CalendarInput input)
        {
            var status = new OperateStatus();
            try
            {
                if (!input.ReplaceExists && await StdSchedulerManager.GetCalendar(input.CalendarName) != null)
                {
                    status.Msg = "日历已存在，请换个其它名称或选择替换现有日历";
                    return Ok(status);
                }
                await StdSchedulerManager.AddCalendar(input);
                status.Code = ResultCode.Success;
                status.Msg = "保存日历成功";
            }
            catch (Exception ex)
            {
                status.Msg = ex.Message;
            }
            return Ok(status);
        }
        #endregion
    }
}