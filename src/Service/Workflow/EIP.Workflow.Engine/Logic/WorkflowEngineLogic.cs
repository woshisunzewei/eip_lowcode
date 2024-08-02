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
namespace EIP.Workflow.Engine.Logic
{
    /// <summary>
    /// 工作流引擎接口实现
    /// </summary>
    public class WorkflowEngineLogic : IWorkflowEngineLogic
    {
        private readonly string[] flowFiled = { "流水号", "流程名称", "流程状态", "活动名称", "当前处理人姓名", "当前处理人Id", "任务Id", "流程Id", "活动Id", "自定义数据", "发起人Id", "发起人姓名", "下一步待处理人Id", "下一步待处理人姓名", "下一步待处理任务Id" };
        private IList<WorkflowProcessInstanceActivity> _instanceActivities = new List<WorkflowProcessInstanceActivity>();
        private IList<WorkflowProcessInstanceLink> _instanceLinks = new List<WorkflowProcessInstanceLink>();
        private readonly IWorkflowProcessInstanceActivityRepository _processInstanceActivityRepository;
        private readonly IWorkflowProcessInstanceRepository _processInstanceRepository;
        private readonly IWorkflowProcessInstanceTaskRepository _processInstanceTaskRepository;
        private readonly ISystemMessageLogic _workflowMessageLogic;
        private readonly ICapPublisher _publisher;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="processInstanceActivityRepository"></param>
        /// <param name="workflowMessageLogic"></param>
        /// <param name="processInstanceRepository"></param>
        /// <param name="publisher"></param>
        /// <param name="processInstanceTaskRepository"></param>
        public WorkflowEngineLogic(IWorkflowProcessInstanceActivityRepository processInstanceActivityRepository,
            ISystemMessageLogic workflowMessageLogic,
            IWorkflowProcessInstanceRepository processInstanceRepository,
            ICapPublisher publisher,
            IWorkflowProcessInstanceTaskRepository processInstanceTaskRepository)
        {
            _publisher = publisher;
            _workflowMessageLogic = workflowMessageLogic;
            _processInstanceActivityRepository = processInstanceActivityRepository;
            _processInstanceRepository = processInstanceRepository;
            _processInstanceTaskRepository = processInstanceTaskRepository;
        }

        /// <summary>
        /// 获取当前登录人员可以使用的流程库
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<WorkflowLibraryOutput>>> FindLibrary(WorkflowLibraryInput input)
        {
            using (var fixture = new SqlDatabaseFixture())
            {
                IList<WorkflowLibraryOutput> outputs = new List<WorkflowLibraryOutput>();
                //var types = (await fixture.Db.SystemType.SetSelect(s => new { s.TypeId, s.Name }).FindAllAsync()).OrderBy(o => o.OrderNo);
                ////判断是否为超级管理员
                //var currentUser = EipHttpContext.CurrentUser();
                //var allProcess = currentUser.IsAdmin ? await _processRepository.FindAllProcess() : (await _processRepository.FindSendLibrary(new IdInput
                //{
                //    Id = input.UserId
                //})).OrderBy(o => o.OrderNo).ToList();

                //foreach (var type in types)
                //{
                //    IList<WorkflowLibraryListOutput> listOutputs = new List<WorkflowLibraryListOutput>();
                //    //获取
                //    var process = allProcess.Where(w => w.TypeId == type.TypeId);
                //    foreach (var pro in process)
                //    {
                //        var output = new WorkflowLibraryListOutput
                //        {
                //            Icon = pro.Icon,
                //            //BgColor = pro.BgColor,
                //            IconColor = pro.IconColor,
                //            Image = pro.Image,
                //            Name = pro.Name,
                //            Remark = pro.Remark,
                //            Version = pro.Version,
                //            ProcessId = pro.ProcessId
                //        };
                //        listOutputs.Add(output);
                //    }
                //    if (listOutputs.Any())
                //    {
                //        outputs.Add(new WorkflowLibraryOutput
                //        {
                //            TypeName = type.Name,
                //            List = listOutputs
                //        });
                //    }

                //}
                return OperateStatus<IEnumerable<WorkflowLibraryOutput>>.Success(outputs);
            }
        }

        /// <summary>
        /// 根据ActivityId获取该活动配置的按钮。
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<WorkflowButton>>> FindProcessButtonByActivity(IdInput input)
        {
            return OperateStatus<IEnumerable<WorkflowButton>>.Success(await _processInstanceActivityRepository.FindProcessButtonByActivity(input));
        }

        /// <summary>
        /// 根据ActivityId获取该活动配置的按钮。
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<WorkflowButton>>> FindProcessInstanceButtonByActivity(IdInput input)
        {
            return OperateStatus<IEnumerable<WorkflowButton>>.Success(await _processInstanceActivityRepository.FindProcessInstanceButtonByActivity(input));
        }

        /// <summary>
        /// 流程开始发起返回所有能够处理人员
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<WorkflowEngineTaskProcessOutput>> StartProcess(WorkflowEngineStartProcessInput input)
        {
            using (var fixture = new SqlDatabaseFixture())
            {

                WorkflowEngineProcessInstanceOutput instance = await FindWorkflowEngineProcessInstance(fixture, input);
                instance.Columns = input.Columns;
                OperateStatus<WorkflowEngineTaskProcessOutput> operateStatus = new OperateStatus<WorkflowEngineTaskProcessOutput>
                {
                    Data = new WorkflowEngineTaskProcessOutput()
                };
                try
                {
                    //写入文件
                    var path = Directory.GetCurrentDirectory() + "/Workflow/" + instance.Instance.ProcessInstanceId + "/";
                    foreach (var item in instance.InstanceActivities)
                    {
                        var activity = instance.Activities.FirstOrDefault(w => w.ActivityId == item.ActivityId);
                        FileUtil.WriteFile(path + item.ActivityId + ".json", activity.Json);
                    }

                    var startActivity = instance.InstanceActivities.ToList().FirstOrDefault(w => w.Type == EnumAcitvityType.开始.FindDescription());
                    if (startActivity == null)
                    {
                        operateStatus.Msg = ResourceWorkflowEngine.流程配置错误无开始节点;
                        return operateStatus;
                    }

                    #region 下一步所有步骤
                    var result = await FindProcessInstanceNextActivities(instance, startActivity.ActivityId, input.CreateUserId);
                    if (result.Code == ResultCode.Error)
                    {
                        operateStatus.Msg = result.Msg;
                        return operateStatus;
                    }
                    if (_instanceActivities.Count() == 0)
                    {
                        operateStatus.Msg = "未找到满足条件节点";
                        return operateStatus;
                    }
                    operateStatus.Data.Activities = _instanceActivities.Select(s => s.ActivityId).ToList();
                    operateStatus.Data.Links = _instanceLinks.Select(s => s.LinkId).ToList();
                    #endregion

                    #region 下一步流程处理人
                    var persons = await FindProcessInstancePerson(instance, input.CreateUserId);
                    if (persons.Code == ResultCode.Error)
                    {
                        operateStatus.Code = ResultCode.Error;
                        operateStatus.Msg = persons.Msg;
                        return operateStatus;
                    }
                    #endregion

                    operateStatus.Data.Person = persons.Data;
                    operateStatus.Code = ResultCode.Success;
                }
                catch (Exception e)
                {
                    operateStatus.Code = ResultCode.Error;
                    operateStatus.Msg = e.Message;
                }
                return operateStatus;
            }
        }

        /// <summary>
        /// 运行开始流程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<WorkflowEngineProcessRunOutput>> StartProcessRun(WorkflowEngineStartProcessRunInput input)
        {
            using (var fixture = new SqlDatabaseFixture())
            {
                if (input.NextTaskString.IsNotNullOrEmpty())
                {
                    input.NextTask = input.NextTaskString.JsonStringToList<WorkflowEngineProcessingPersonOutput>();
                }
                OperateStatus<WorkflowEngineProcessRunOutput> operateStatus = new OperateStatus<WorkflowEngineProcessRunOutput>
                {
                    Data = new WorkflowEngineProcessRunOutput()
                };
                WorkflowEngineStartProcessInput startProcessInput = input.MapTo<WorkflowEngineStartProcessRunInput, WorkflowEngineStartProcessInput>();
                WorkflowEngineProcessInstanceOutput instance = await FindWorkflowEngineProcessInstance(fixture, startProcessInput);
                var startActivity = instance.InstanceActivities.ToList().FirstOrDefault(w => w.Type == EnumAcitvityType.开始.FindDescription());
                if (startActivity == null)
                {
                    operateStatus.Msg = ResourceWorkflowEngine.流程配置错误无开始节点;
                    return operateStatus;
                }

                #region 流程主数据
                try
                {
                    await fixture.Db.WorkflowProcessInstance.InsertAsync(instance.Instance);
                    await fixture.Db.WorkflowProcessInstanceActivity.BulkInsertAsync(instance.InstanceActivities);
                    await fixture.Db.WorkflowProcessInstanceLink.BulkInsertAsync(instance.InstanceLinks);
                }
                catch (Exception e)
                {
                    operateStatus.Code = ResultCode.Error;
                    operateStatus.Msg = e.Message;
                    return operateStatus;
                }

                #endregion

                #region 开始任务

                var startTaskResult = await SaveInstanceStartTask(new WorkflowEngineSaveInstanceStartTaskInput()
                {
                    CreateUserId = input.CreateUserId,
                    CreateUserName = input.CreateUserName,
                    ProcessInstanceId = input.ProcessInstanceId,
                    ActivityId = startActivity.ActivityId,
                    CustomData = input.CustomData
                }, fixture);
                if (startTaskResult.Code == ResultCode.Error)
                {
                    operateStatus.Msg = startTaskResult.Msg;
                    return operateStatus;
                }
                #endregion

                #region 代办任务

                await SaveInstanceTask(new WorkflowEngineSaveInstanceTaskInput
                {
                    TaskId = startTaskResult.Data.TaskId,
                    CreateUserId = input.CreateUserId,
                    CreateUserName = input.CreateUserName,
                    CreateUserOrganizationId = input.CreateUserOrganizationId,
                    CreateUserOrganizationName = input.CreateUserOrganizationName,
                    ProcessInstanceId = input.ProcessInstanceId,
                    PrevTaskId = startTaskResult.Data.TaskId,
                    Persons = input.NextTask,
                    CustomData = input.CustomData
                }, fixture);

                #endregion

                #region 主流程
                var statusRemarkResult = await UpdateInstanceStatusRemark(new WorkflowEngineSaveInstanceTaskInput
                {
                    CreateUserId = input.CreateUserId,
                    CreateUserName = input.CreateUserName,
                    ProcessInstanceId = input.ProcessInstanceId,
                    Persons = input.NextTask
                }, fixture);
                operateStatus.Data.TaskId = startTaskResult.Data.TaskId;
                operateStatus.Code = ResultCode.Success;
                operateStatus.Msg = string.Format(ResourceWorkflowEngine.发起流程成功, instance.Instance.Sn) + statusRemarkResult.Data.TrimEnd(',');
                //当前活动是否归档
                //if (startActivity.IsArchive)
                //{
                //    operateStatus.Data.IsArchive = true;
                //    var tasks = new List<WorkflowProcessInstanceTask> { startTaskResult.Data };
                //    operateStatus.Data.DesignJson = FindDesignJson(instance.InstanceLinks, instance.InstanceActivities, tasks);
                //}
                #region 处理状态
                await UpdateIsPass(new WorkflowEngineUpdateIsPassInput
                {
                    Activities = input.Activities,
                    Links = input.Links
                }, fixture);
                await UpdateFormStatus(new WorkflowEngineUpdateFormStatusInput
                {
                    ProcessInstanceId = instance.Instance.ProcessInstanceId,
                    Status = EnumProcessInstanceStatus.处理中,
                    Sn = instance.Instance.Sn
                });
                #endregion
                if (operateStatus.Code == ResultCode.Success)
                {
                    #region 发送信息
                    await _publisher.PublishAsync("MessageSubscribe", new WorkflowEngineRunProcessInput
                    {
                        Notice = input.Notice,
                        TaskId = operateStatus.Data.TaskId,
                        DoUserId = input.CreateUserId,
                        NextTask = input.NextTask,
                        Columns = input.Columns
                    });
                    #endregion
                }
                return operateStatus;
                #endregion
            }
        }

        /// <summary>
        /// 运行开始流程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task<OperateStatus<WorkflowEngineProcessRunOutput>> StartSubProcessProcessRun(WorkflowEngineStartSubProcessRunInput input)
        {
            using (var fixture = new SqlDatabaseFixture())
            {
                if (input.NextTaskString.IsNotNullOrEmpty())
                {
                    input.NextTask = input.NextTaskString.JsonStringToList<WorkflowEngineProcessingPersonOutput>();
                }

                OperateStatus<WorkflowEngineProcessRunOutput> operateStatus = new OperateStatus<WorkflowEngineProcessRunOutput>
                {
                    Data = new WorkflowEngineProcessRunOutput()
                };
                WorkflowEngineStartProcessInput startProcessInput = input.MapTo<WorkflowEngineStartSubProcessRunInput, WorkflowEngineStartProcessInput>();

                //当前处理人第一个人作为发起人:2020-5-31
                if (input.NextTask.Count() > 0 && input.NextTask[0].Persons.Count() > 0)
                {
                    var userId = input.NextTask[0].Persons[0].UserId;
                    var userInfo = await fixture.Db.SystemUserInfo.FindAsync(f => f.UserId == userId);
                    input.CreateUserId = userInfo.UserId;
                    input.CreateUserName = userInfo.Name;
                    input.CreateUserOrganizationId = userInfo.OrganizationId;
                    input.CreateUserOrganizationName = userInfo.OrganizationName;

                    startProcessInput.CreateUserId = userInfo.UserId;
                    startProcessInput.CreateUserName = userInfo.Name;
                    startProcessInput.CreateUserOrganizationId = userInfo.OrganizationId;
                    startProcessInput.CreateUserOrganizationName = userInfo.OrganizationName;
                }
                else
                {
                    operateStatus.Code = ResultCode.Error;
                    operateStatus.Msg = "子流程处理人为空";
                }

                WorkflowEngineProcessInstanceOutput instance = await FindWorkflowEngineProcessInstance(fixture, startProcessInput);
                var startActivity = instance.InstanceActivities.ToList().FirstOrDefault(w => w.Type == EnumAcitvityType.开始.FindDescription());
                if (startActivity == null)
                {
                    operateStatus.Msg = ResourceWorkflowEngine.流程配置错误无开始节点;
                    return operateStatus;
                }

                #region 流程主数据
                var trans = fixture.Db.BeginTransaction();
                try
                {
                    await fixture.Db.WorkflowProcessInstance.InsertAsync(instance.Instance, trans);
                    await fixture.Db.WorkflowProcessInstanceActivity.BulkInsertAsync(instance.InstanceActivities, trans);
                    await fixture.Db.WorkflowProcessInstanceLink.BulkInsertAsync(instance.InstanceLinks, trans);
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    operateStatus.Code = ResultCode.Error;
                    operateStatus.Msg = e.Message;
                    return operateStatus;
                }
                trans.Commit();

                #endregion

                #region 代办任务

                await SaveInstanceTask(new WorkflowEngineSaveInstanceTaskInput
                {
                    CreateUserId = input.CreateUserId,
                    CreateUserName = input.CreateUserName,
                    CreateUserOrganizationId = input.CreateUserOrganizationId,
                    CreateUserOrganizationName = input.CreateUserOrganizationName,
                    ProcessInstanceId = input.ProcessInstanceId,
                    PrevTaskId = input.PrevTaskId,
                    Persons = input.NextTask
                }, fixture);

                #endregion

                #region 主流程
                var statusRemarkResult = await UpdateInstanceStatusRemark(new WorkflowEngineSaveInstanceTaskInput
                {
                    CreateUserId = input.CreateUserId,
                    CreateUserName = input.CreateUserName,
                    ProcessInstanceId = input.ProcessInstanceId,
                    Persons = input.NextTask
                }, fixture);
                operateStatus.Data.TaskId = input.PrevTaskId;
                operateStatus.Code = ResultCode.Success;
                operateStatus.Msg = string.Format(ResourceWorkflowEngine.发起流程成功, instance.Instance.Sn) + statusRemarkResult.Data.TrimEnd(',');
                //当前活动是否归档
                await UpdateIsPass(new WorkflowEngineUpdateIsPassInput
                {
                    Activities = input.Activities,
                    Links = input.Links
                }, fixture);
                return operateStatus;
                #endregion
            }
        }

        /// <summary>
        /// 流程运行调用此方法，将当前任务结束，并分发任务给下一步节点的办理人。
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<WorkflowEngineTaskProcessOutput>> TaskProcess(WorkflowEngineRunProcessInput input)
        {
            using (var fixture = new SqlDatabaseFixture())
            {
                if (input.NextTaskString.IsNotNullOrEmpty())
                {
                    input.NextTask = input.NextTaskString.JsonStringToList<WorkflowEngineProcessingPersonOutput>();
                }
                var instance = await FindRunInstanceByProcessInstanceId(fixture, input.ProcessInstanceId);
                instance.Columns = input.Columns;
                OperateStatus<WorkflowEngineTaskProcessOutput> operateStatus = new OperateStatus<WorkflowEngineTaskProcessOutput>
                {
                    Data = new WorkflowEngineTaskProcessOutput()
                };
                try
                {
                    var checkOperateStatus = CheckCurrentTask(instance, input.TaskId);
                    if (checkOperateStatus.Code == ResultCode.Error)
                    {
                        operateStatus.Msg = checkOperateStatus.Msg;
                        return operateStatus;
                    }
                    #region 下一步所有步骤
                    //设置当前活动为完成状态
                    var instanceTask = instance.InstanceTasks.FirstOrDefault(f => f.TaskId == input.TaskId);
                    if (instanceTask != null)
                    {
                        instanceTask.Status = EnumTaskStatus.完成.ToShort();
                        //通过策略判断
                        var approveUserPassAgree = FindProcessInstanceNextActivitiesApproveUserPassAgree(instance, input.ActivityId);
                        if (approveUserPassAgree.Code == ResultCode.Success)
                        {
                            var result = await FindProcessInstanceNextActivities(instance, input.ActivityId, input.DoUserId);
                            if (result.Code == ResultCode.Error)
                            {
                                operateStatus.Msg = result.Msg;
                                return operateStatus;
                            }
                            instanceTask.Status = EnumTaskStatus.正在处理.ToShort();
                        }
                        else
                        {
                            operateStatus.Data.NeedChosenPerson = false;
                            operateStatus.Code = ResultCode.Success;
                            operateStatus.Msg = Chs.Successful;
                            return operateStatus;
                        }

                    }
                    #endregion

                    #region 处理人
                    var operateStatusPerson = await FindProcessInstancePerson(instance, input.DoUserId);
                    if (operateStatusPerson.Code == ResultCode.Error)
                    {
                        operateStatus.Msg = operateStatusPerson.Msg;
                        return operateStatus;
                    }
                    #endregion

                    if (!_instanceActivities.Any())
                    {
                        operateStatus.Code = ResultCode.Error;
                        operateStatus.Msg = ResourceWorkflowEngine.未找到后续流程节点;
                        return operateStatus;
                    }
                    operateStatus.Data.Activities = _instanceActivities.Select(s => s.ActivityId).ToList();
                    operateStatus.Data.Links = _instanceLinks.Select(s => s.LinkId).ToList();
                    operateStatus.Data.Person = operateStatusPerson.Data;
                    operateStatus.Code = ResultCode.Success;
                    operateStatus.Msg = Chs.Successful;
                }
                catch (WorkflowEngineException e)
                {
                    operateStatus.Msg = e.Message;
                }
                return operateStatus;
            }
        }

        /// <summary>
        /// 返回无流程处理人下个流程信息
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<WorkflowEngineTaskProcessOutput>> TaskProcessNoPerson(WorkflowEngineRunProcessInput input)
        {
            using (var fixture = new SqlDatabaseFixture())
            {
                var instance = await FindRunInstanceByProcessInstanceId(fixture, input.ProcessInstanceId);
                OperateStatus<WorkflowEngineTaskProcessOutput> operateStatus = new OperateStatus<WorkflowEngineTaskProcessOutput>
                {
                    Data = new WorkflowEngineTaskProcessOutput()
                };
                try
                {
                    var checkOperateStatus = CheckCurrentTask(instance, input.TaskId);
                    if (checkOperateStatus.Code == ResultCode.Error)
                    {
                        operateStatus.Msg = checkOperateStatus.Msg;
                        return operateStatus;
                    }
                    #region 下一步所有步骤
                    //设置当前活动为完成状态
                    var instanceTask = instance.InstanceTasks.FirstOrDefault(f => f.TaskId == input.TaskId);
                    if (instanceTask != null)
                    {
                        instanceTask.Status = EnumTaskStatus.完成.ToShort();
                        var result = await FindProcessInstanceNextActivities(instance, input.ActivityId, input.DoUserId);
                        if (result.Code == ResultCode.Error)
                        {
                            return operateStatus;
                        }

                        instanceTask.Status = EnumTaskStatus.正在处理.ToShort();
                    }
                    #endregion

                    if (!_instanceActivities.Any())
                    {
                        operateStatus.Code = ResultCode.Error;
                        operateStatus.Msg = ResourceWorkflowEngine.未找到后续流程节点;
                        return operateStatus;
                    }
                    operateStatus.Data.Activities = _instanceActivities.Select(s => s.ActivityId).ToList();
                    operateStatus.Data.Links = _instanceLinks.Select(s => s.LinkId).ToList();
                    operateStatus.Code = ResultCode.Success;
                    operateStatus.Msg = Chs.Successful;
                }
                catch (WorkflowEngineException e)
                {
                    operateStatus.Msg = e.Message;
                }
                return operateStatus;
            }
        }

        /// <summary>
        /// 流程运行调用此方法，将当前任务结束，并分发任务给下一步节点的办理人。
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<WorkflowEngineProcessRunOutput>> TaskProcessRun(WorkflowEngineRunProcessInput input)
        {
            using (var fixture = new SqlDatabaseFixture())
            {
                if (input.NextTaskString.IsNotNullOrEmpty())
                {
                    input.NextTask = input.NextTaskString.JsonStringToList<WorkflowEngineProcessingPersonOutput>();
                }
                OperateStatus<WorkflowEngineProcessRunOutput> operateStatus = new OperateStatus<WorkflowEngineProcessRunOutput>
                {
                    Data = new WorkflowEngineProcessRunOutput()
                };
                //得到流程所有信息
                var instance = await FindRunInstanceByProcessInstanceId(fixture, input.ProcessInstanceId);
                //获取当前活动
                var checkOperateStatus = CheckCurrentTask(instance, input.TaskId);
                if (checkOperateStatus.Code == ResultCode.Error)
                {
                    operateStatus.Msg = checkOperateStatus.Msg;
                    return operateStatus;
                }
                var time = DateTime.Now;
                if (!input.NeedChosenPerson)
                {
                    return await TaskProcessRunNoNeedChosenPerson(input, checkOperateStatus.Data, fixture);
                }

                if (!input.NextTask.Any())
                {
                    operateStatus.Msg = ResourceWorkflowEngine.未找到后续流程节点;
                    return operateStatus;
                }

                foreach (var nextTask in input.NextTask)
                {
                    if (!nextTask.Persons.Any() && nextTask.Activity.Type != EnumAcitvityType.结束.FindDescription() && nextTask.Activity.Type != EnumAcitvityType.合并.FindDescription())
                    {
                        operateStatus.Msg = nextTask.Activity.Title + ResourceWorkflowEngine.无流程处理人;
                        return operateStatus;
                    }
                }

                try
                {
                    WorkflowProcessInstanceTask task = checkOperateStatus.Data;
                    //判断下个节点是否具有完成节点
                    var endActivity = input.NextTask.Where(w => w.Activity.Type == EnumAcitvityType.结束.FindDescription()).ToList();
                    var activity = instance.InstanceActivities.FirstOrDefault(f => f.ActivityId == input.ActivityId);
                    if (endActivity.Any())
                    {
                        operateStatus = await RunEndTask(fixture, input, instance, task);
                        if (operateStatus.Code == ResultCode.Success)
                        {
                            //if (activity != null && activity.IsArchive)
                            //{
                            //    operateStatus.Data.IsArchive = true;
                            //    instance.InstanceTasks.Add(task);
                            //    operateStatus.Data.DesignJson = FindDesignJson(instance.InstanceLinks, instance.InstanceActivities, instance.InstanceTasks);
                            //}

                            #region 发送信息
                            await _publisher.PublishAsync("MessageSubscribe", new WorkflowEngineRunProcessInput
                            {
                                Notice = input.Notice,
                                TaskId = input.TaskId,
                                DoUserId = input.DoUserId,
                                NextTask = input.NextTask,
                                Columns = input.Columns
                            });
                            #endregion
                        }
                        input.NextTask = input.NextTask.Where(w => w.Activity.Type != EnumAcitvityType.结束.FindDescription()).ToList();
                    }

                    //是否具有需要处理的活动节点
                    if (input.NextTask.Any())
                    {
                        #region 判断节点处理列表:将所有流程处理人指向为当前处理人

                        //判断当前流程节点通过策略
                        var allTasksNeedDo = instance.InstanceTasks.Where(f => f.ActivityId == task.ActivityId
                                                                               && f.Status == EnumTaskStatus.正在处理.ToShort()).ToList();
                        IList<WorkflowProcessInstanceTask> updateTasks = new List<WorkflowProcessInstanceTask>();
                        if (allTasksNeedDo.Any())
                        {
                            foreach (var taskDo in allTasksNeedDo)
                            {
                                taskDo.DoUserId = input.DoUserId;
                                taskDo.DoUserName = input.DoUserName;
                                taskDo.Comment = input.Comment;
                                taskDo.Status = EnumTaskStatus.完成.ToShort();
                                taskDo.Remark = input.DoUserName + "|" +
                                                    time.ToYyyyMMddHHmmss() + "|" +
                                                    ResourceWorkflowEngine.处理完毕;
                                taskDo.CompletedTime = time;
                                updateTasks.Add(taskDo);
                            }
                        }

                        #endregion

                        #region 插入代办任务

                        await SaveInstanceTask(new WorkflowEngineSaveInstanceTaskInput
                        {
                            TaskId = input.TaskId,
                            CreateUserId = input.DoUserId,
                            CreateUserName = input.DoUserName,
                            CreateUserOrganizationId = input.DoUserOrganizationId,
                            CreateUserOrganizationName = input.DoUserOrganizationName,
                            ProcessInstanceId = task.ProcessInstanceId,
                            PrevTaskId = input.TaskId,
                            Persons = input.NextTask,
                            CustomData = input.CustomData
                        }, fixture);

                        #endregion

                        #region 主流程

                        if (updateTasks.Any())
                        {
                            try
                            {
                                await fixture.Db.WorkflowProcessInstanceTask.BulkUpdateAsync(updateTasks);
                                var statusRemarkResult = await UpdateInstanceStatusRemark(
                                    new WorkflowEngineSaveInstanceTaskInput
                                    {
                                        CreateUserId = input.DoUserId,
                                        CreateUserName = input.DoUserName,
                                        ProcessInstanceId = task.ProcessInstanceId,
                                        Persons = input.NextTask
                                    }, fixture);
                                operateStatus.Data.TaskId = input.TaskId;
                                operateStatus.Code = ResultCode.Success;
                                operateStatus.Msg =
                                    string.Format(ResourceWorkflowEngine.发起流程成功, instance.Instance.Sn) +
                                    statusRemarkResult.Data.TrimEnd(',');
                                //当前活动是否归档
                                //if (activity != null && activity.IsArchive)
                                //{
                                //    operateStatus.Data.IsArchive = true;
                                //    instance.InstanceTasks.Add(task);
                                //    operateStatus.Data.DesignJson = FindDesignJson(instance.InstanceLinks,
                                //        instance.InstanceActivities, instance.InstanceTasks);
                                //}

                                #region 处理状态
                                await UpdateIsPass(new WorkflowEngineUpdateIsPassInput
                                {
                                    Activities = input.Activities,
                                    Links = input.Links
                                }, fixture);
                                #endregion
                            }
                            catch (Exception e)
                            {
                                operateStatus.Msg = e.Message;
                                return operateStatus;
                            }
                        }
                        if (operateStatus.Code == ResultCode.Success)
                        {
                            #region 发送信息
                            await _publisher.PublishAsync("MessageSubscribe", input);
                            #endregion
                        }
                        return operateStatus;
                        #endregion
                    }
                    //如果不包含结束
                    if (endActivity.Count == 0 && input.NextTask.Count() == 0)
                    {
                        operateStatus.Code = ResultCode.Error;
                        operateStatus.Msg = ResourceWorkflowEngine.未找到后续流程节点;
                    }
                    return operateStatus;
                }
                catch (WorkflowEngineException e)
                {
                    operateStatus.Msg = e.Message;
                    return operateStatus;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="trigger"></param>
        /// <returns></returns>
        public async Task SendWorkflowNotice(WorkflowEngineRunProcessInput input, EnumNoticeTrigger? trigger = EnumNoticeTrigger.下一步成功)
        {
            using (SqlDatabaseFixture fixture = new SqlDatabaseFixture())
            {
                var task = await fixture.Db.WorkflowProcessInstanceTask.SetSelect(s => new { s.ActivityId, s.ProcessInstanceId, s.CustomData, s.TaskId, s.DoUserId, s.DoUserName }).FindAsync(f => f.TaskId == input.TaskId);
                //得到当前活动
                var activity = await fixture.Db.WorkflowProcessInstanceActivity.SetSelect(s => new { s.ActivityId, s.ProcessInstanceId, s.FormId }).FindAsync(f => f.ActivityId == task.ActivityId && f.ProcessInstanceId == task.ProcessInstanceId);

                var activityObj = GetProcessinstanceActivity(task.ProcessInstanceId, activity.ActivityId).JsonStringToObject<WorkflowActivityDto>();
                if (activityObj != null)
                {
                    if (activityObj.Props.Notice.Any())
                    {
                        List<WorkflowActivityNotice> notices = new List<WorkflowActivityNotice>();

                        //序列化得到需要通知的内容
                        var noticesUserChosen = activityObj.Props.Notice.Where(w => w.UserChosen && w.Use && w.Trigger == trigger.ToShort()).ToList();
                        if (input.Notice.IsNotNullOrEmpty())
                        {
                            var noticeIndex = input.Notice.Split(',');
                            for (int i = 0; i < noticesUserChosen.Count; i++)
                            {
                                var notice = noticesUserChosen[i];
                                if (noticeIndex.Contains(i.ToString()))
                                {
                                    notices.Add(notice);
                                }
                            }
                        }

                        var noticesUnUserChosen = activityObj.Props.Notice.Where(w => !w.UserChosen && w.Use && w.Trigger == trigger.ToShort()).ToList();
                        foreach (var item in noticesUnUserChosen)
                        {
                            notices.Add(item);
                        }

                        foreach (var notice in notices)
                        {
                            if (notice.Config != null)
                            {
                                //序列号配置
                                var config = notice.Config;
                                IList<SystemUserInfo> noticeUsers = new List<SystemUserInfo>();
                                IList<string> ids = new List<string>();
                                if (config.Range.Any())
                                {
                                    ids = config.Range.Select(s => s.Id).ToList();
                                }
                                //得到通知人
                                switch (config.Type)
                                {
                                    case EnumNoticeDoUserType.所有成员://所有人员
                                        var users = await fixture.Db.SystemUserInfo.SetSelect(s => new { s.UserId, s.Name, s.Email, s.Mobile }).FindAllAsync(f => !f.IsAdmin && !f.IsFreeze);
                                        foreach (var item in users)
                                        {
                                            noticeUsers.Add(item);
                                        }
                                        break;
                                    case EnumNoticeDoUserType.组织://组织
                                        foreach (var id in ids)
                                        {
                                            var gid = Guid.Parse(id);
                                            var orgs = (await fixture.Db.SystemPermissionUserFindByPrivilegeMasterAndValueOutput
                                                .FindAllAsync<SystemPermissionUserFindByPrivilegeMasterAndValueUsersOutput>(
                                                    f => f.PrivilegeMaster == EnumPrivilegeMaster.组织架构.ToShort() &&
                                                         f.PrivilegeMasterValue == gid, o => o.Users)).ToList();
                                            if (!orgs.Any()) continue;
                                            foreach (var item in orgs[0].Users)
                                            {
                                                noticeUsers.Add(new SystemUserInfo()
                                                {
                                                    Name = item.Name,
                                                    Email = item.Email,
                                                    Mobile = item.Mobile,
                                                });
                                            }
                                        }
                                        break;
                                    case EnumNoticeDoUserType.人员://人员
                                        if (ids.Any())
                                        {
                                            var userId = new List<Guid>();
                                            foreach (var id in ids)
                                            {
                                                userId.Add(Guid.Parse(id));
                                            }
                                            var permissionUser = await fixture.Db.SystemUserInfo.SetSelect(s => new { s.UserId, s.Name, s.Email, s.Mobile }).FindAllAsync(f => userId.Contains(f.UserId));
                                            if (permissionUser == null) continue;
                                            foreach (var item in permissionUser)
                                            {
                                                noticeUsers.Add(item);
                                            }
                                        }
                                        break;
                                    case EnumNoticeDoUserType.角色://角色人员
                                        foreach (var id in ids)
                                        {
                                            var gid = Guid.Parse(id);
                                            var roles = (await fixture.Db.SystemPermissionUserFindByPrivilegeMasterAndValueOutput
                                                .FindAllAsync<SystemPermissionUserFindByPrivilegeMasterAndValueUsersOutput>(
                                                    f => f.PrivilegeMaster == EnumPrivilegeMaster.角色.ToShort() &&
                                                         f.PrivilegeMasterValue == gid, o => o.Users)).ToList();
                                            if (!roles.Any()) continue;
                                            foreach (var item in roles[0].Users)
                                            {
                                                noticeUsers.Add(new SystemUserInfo()
                                                {
                                                    UserId = item.UserId,
                                                    Name = item.Name,
                                                    Email = item.Email,
                                                    Mobile = item.Mobile,
                                                    //Qq = item.Qq,
                                                    //WeChat = item.WeChat
                                                });
                                            }
                                        }
                                        break;
                                    case EnumNoticeDoUserType.岗位://岗位
                                        break;
                                    case EnumNoticeDoUserType.组://组
                                        break;
                                    case EnumNoticeDoUserType.下一步处理接收人: //下一步待处理人
                                        if (input.NextTask.Any())
                                        {
                                            foreach (var item in input.NextTask)
                                            {
                                                foreach (var person in item.Persons)
                                                {
                                                    var nextTaskUser = await fixture.Db.SystemUserInfo.SetSelect(s => new { s.UserId, s.Name, s.Email, s.Mobile }).FindAsync(f => f.UserId == person.UserId);
                                                    noticeUsers.Add(nextTaskUser);
                                                }
                                            }
                                        }
                                        break;
                                    case EnumNoticeDoUserType.当前处理人: //下一步待处理人
                                        var doUser = await fixture.Db.SystemUserInfo.SetSelect(s => new { s.UserId, s.Name, s.Email, s.Mobile }).FindAsync(f => f.UserId == task.DoUserId);
                                        noticeUsers.Add(doUser);
                                        break;
                                    case EnumNoticeDoUserType.发起者://发起人
                                        var instance = await fixture.Db.WorkflowProcessInstance.SetSelect(s => new { s.CreateUserId }).FindAsync(f => f.ProcessInstanceId == activity.ProcessInstanceId);
                                        if (instance != null)
                                        {
                                            var instanceUser = await fixture.Db.SystemUserInfo.SetSelect(s => new { s.UserId, s.Name, s.Email, s.Mobile }).FindAsync(f => f.UserId == instance.CreateUserId);
                                            noticeUsers.Add(instanceUser);
                                        }
                                        break;
                                    case EnumNoticeDoUserType.提交人直线领导: //提交人的直线领导
                                        var userLeader = (await fixture.Db.SystemUserFindLeaderOutput.FindAllAsync<SystemUserLeadersOutput>(f => f.UserId == input.DoUserId, o => o.Outputs)).ToList();
                                        if (userLeader.Any())
                                        {
                                            foreach (var item in userLeader.First().Outputs)
                                            {
                                                noticeUsers.Add(new SystemUserInfo()
                                                {
                                                    UserId = item.UserId,
                                                    Name = item.Name,
                                                    Email = item.Email,
                                                    Mobile = item.Mobile,
                                                    //Qq = item.Qq,
                                                    //WeChat = item.WeChat
                                                });
                                            }
                                        }
                                        break;
                                    case EnumNoticeDoUserType.发起者直线领导: //发起者的直线领导
                                        var createUserInstance = await fixture.Db.WorkflowProcessInstance.SetSelect(s => new { s.CreateUserId }).FindAsync(f => f.ProcessInstanceId == activity.ProcessInstanceId);
                                        var createUserLeader = (await fixture.Db.SystemUserFindLeaderOutput.FindAllAsync<SystemUserLeadersOutput>(f => f.UserId == createUserInstance.CreateUserId, o => o.Outputs)).ToList();
                                        if (createUserLeader.Any())
                                        {
                                            foreach (var item in createUserLeader.First().Outputs)
                                            {
                                                noticeUsers.Add(new SystemUserInfo()
                                                {
                                                    UserId = item.UserId,
                                                    Name = item.Name,
                                                    Email = item.Email,
                                                    Mobile = item.Mobile,
                                                    //Qq = item.Qq,
                                                    //WeChat = item.WeChat
                                                });
                                            }
                                        }
                                        break;
                                    case EnumNoticeDoUserType.自定义: //自定义Sql表达式
                                        foreach (string ruleSql in ids)
                                        {
                                            string sql = string.Empty;
                                            //解析Sql
                                            if (!ruleSql.IsNullOrEmpty())
                                            {
                                                //替换Html标签
                                                sql = ruleSql.ReplaceHtmlTag();
                                            }
                                        }
                                        break;
                                    case EnumNoticeDoUserType.程序指定: //程序指定
                                        break;
                                }

                                if (noticeUsers.Any())
                                {
                                    foreach (var user in noticeUsers)
                                    {
                                        string message = "", returnUrl = "";
                                        //判断通知类型
                                        switch (notice.Type)
                                        {
                                            case EnumNoticeType.站内:
                                                message = await GetSendWorkflowNoticeMessage(config, task, activity, fixture, user);
                                                returnUrl = await GetSendNoticeUrl(config, task, activity, fixture, user);
                                                await _workflowMessageLogic.SendWebSite(new SendWebSiteInput
                                                {
                                                    CustomData = JsonConvert.SerializeObject(input),
                                                    Message = message,
                                                    Title = config.Title,
                                                    ReturnUrl = returnUrl,
                                                    MessageLogId = CombUtil.NewComb(),
                                                    ReceiverId = user.UserId.ToString(),
                                                    ReceiverName = user.Name,
                                                    ReceiverType = EnumReceiverType.人员
                                                });
                                                break;
                                            case EnumNoticeType.短信:
                                                message = await GetSendWorkflowNoticeMessage(config, task, activity, fixture, user);
                                                returnUrl = await GetSendNoticeUrl(config, task, activity, fixture, user);
                                                await _workflowMessageLogic.SendSms(new SendSmsInput
                                                {
                                                    Provide = notice.Config.Provide,
                                                    CustomData = JsonConvert.SerializeObject(input),
                                                    Message = message,
                                                    ReturnUrl = returnUrl,
                                                    Phone = user.Mobile,
                                                    MessageLogId = CombUtil.NewComb(),
                                                    ReceiverId = user.UserId.ToString(),
                                                    ReceiverName = user.Name,
                                                    ReceiverType = EnumReceiverType.人员
                                                });
                                                break;
                                            case EnumNoticeType.邮件:
                                                message = await GetSendWorkflowNoticeMessage(config, task, activity, fixture, user);
                                                returnUrl = await GetSendNoticeUrl(config, task, activity, fixture, user);
                                                _workflowMessageLogic.SendEmail(new SendEmailInput
                                                {
                                                    CustomData = JsonConvert.SerializeObject(input),
                                                    Name = "流程通知",
                                                    Message = message,
                                                    Subject = config.Title,
                                                    ToAddress = user.Email,
                                                    ToName = user.Name,
                                                    MessageLogId = CombUtil.NewComb(),
                                                    ReceiverId = user.UserId.ToString(),
                                                    ReceiverName = user.Name,
                                                    ReceiverType = EnumReceiverType.人员
                                                });
                                                break;
                                            case EnumNoticeType.App:
                                                message = await GetSendWorkflowNoticeMessage(config, task, activity, fixture, user);
                                                returnUrl = await GetSendNoticeUrl(config, task, activity, fixture, user);
                                                //_workflowMessageLogic.SendApp(new SendAppInput
                                                //{
                                                //    CustomData = JsonConvert.SerializeObject(new { input.TaskId }),
                                                //    Message = message,
                                                //    Title = config.Title,
                                                //    Url = returnUrl,
                                                //    MessageLogId = CombUtil.NewComb(),
                                                //    ReceiverId = noticeUsers.Select(s => s.UserId).ExpandAndToString(),
                                                //    ReceiverName = noticeUsers.Select(s => s.Name).ExpandAndToString(),
                                                //    ReceiverType = EnumReceiverType.人员
                                                //});
                                                break;
                                            case EnumNoticeType.微信公众号:
                                                var parameter = await GetSendWorkflowWeiXinMpNoticeMessage(config.Param, task, activity, fixture, user);
                                                returnUrl = await GetSendNoticeUrl(config, task, activity, fixture, user);
                                                _workflowMessageLogic.SendWeiXinMp(new SendWeiXinMpInput
                                                {
                                                    CustomData = JsonConvert.SerializeObject(input),
                                                    MpAppId = config.MpAppId,
                                                    MiniAppId = config.MiniAppId,
                                                    Parameter = parameter,
                                                    Code = config.Code,
                                                    ReturnUrl = returnUrl,
                                                    MessageLogId = CombUtil.NewComb(),
                                                    ReceiverId = user.UserId.ToString(),
                                                    ReceiverName = user.Name,
                                                    ReceiverType = EnumReceiverType.人员
                                                });
                                                break;
                                            case EnumNoticeType.微信小程序:
                                                break;
                                            case EnumNoticeType.企业微信:
                                                returnUrl = await GetSendNoticeUrl(config, task, activity, fixture, user);
                                                message = await GetSendWorkflowNoticeMessage(config, task, activity, fixture, user);
                                                await _workflowMessageLogic.SendWeiXinWork(new SendWeiXinWorkInput
                                                {
                                                    Title = config.Title,
                                                    Message = message,
                                                    ReturnUrl = returnUrl,
                                                    Phone = user.Mobile,
                                                    MessageLogId = CombUtil.NewComb(),
                                                    ReceiverId = user.UserId.ToString(),
                                                    ReceiverName = user.Name,
                                                    ReceiverType = EnumReceiverType.人员
                                                });
                                                break;
                                            case EnumNoticeType.Qq:
                                                break;
                                            case EnumNoticeType.钉钉:
                                                returnUrl = await GetSendNoticeUrl(config, task, activity, fixture, user);
                                                message = await GetSendWorkflowNoticeMessage(config, task, activity, fixture, user);
                                                await _workflowMessageLogic.SendDingTalk(new SendDingTalkInput
                                                {
                                                    Title = config.Title,
                                                    Message = message,
                                                    ReturnUrl = returnUrl,
                                                    Phone = user.Mobile,
                                                    MessageLogId = CombUtil.NewComb(),
                                                    ReceiverId = user.UserId.ToString(),
                                                    ReceiverName = user.Name,
                                                    ReceiverType = EnumReceiverType.人员
                                                });
                                                break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 修改表单状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task<OperateStatus> UpdateFormStatus(WorkflowEngineUpdateFormStatusInput input)
        {
            OperateStatus<Guid> operateStatus = new OperateStatus<Guid>();
            try
            {
                SystemAgile systemAgile = new SystemAgile();
                string connectionType, connectionString;
                using (var fixture = new SqlDatabaseFixture())
                {
                    var sqlSystemConfig = $@"select config.DataFromName,config.DataBaseId from System_Agile config
                                            left join workflow_process process on config.ConfigId=process.FormId
                                            left join workflow_processinstance processInstance on processInstance.ProcessId=process.ProcessId
                                            where ProcessInstanceId='{input.ProcessInstanceId}'";
                    systemAgile = await fixture.Db.Connection.QueryFirstAsync<SystemAgile>(sqlSystemConfig);
                    var dataBase = await GetSystemDataBase(fixture, new IdInput { Id = systemAgile.DataBaseId });
                    connectionType = dataBase.ConnectionType;
                    connectionString = dataBase.ConnectionString;
                }

                if (systemAgile != null)
                {
                    StringBuilder stringBuilder = new StringBuilder($"UPDATE {systemAgile.DataFromName} SET WorkflowStatus={input.Status.ToShort()}");
                    if (input.Sn.IsNotNullOrEmpty())
                    {
                        stringBuilder.Append($",WorkflowSn={input.Sn}");
                    }
                    string sql = stringBuilder.ToString().TrimEnd(',') + $" WHERE RelationId='{input.ProcessInstanceId}' ";
                    if (sql.IsNotNullOrEmpty())
                    {
                        int table = new DbHelper(connectionString, connectionType).ExecuteSql(sql);
                        if (table > 0)
                        {
                            operateStatus.Code = ResultCode.Success;
                            operateStatus.Msg = Chs.Successful;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                operateStatus.Msg = e.Message;
                operateStatus.Code = ResultCode.Success;
            }
            return operateStatus;
        }
        /// <summary>
        /// 根据数据库Id返回对应数据库连接
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task<SystemDataBaseInput> GetSystemDataBase(SqlDatabaseFixture fixture, IdInput input)
        {
            var cacheKey = $"ISystemDataBaseLogic_Cache:{input.Id}";
            var dataBase = await RedisHelper.CacheShellAsync(cacheKey, DateTimeUtil.TotalSeconds(20), async () =>
            {
                return await fixture.Db.SystemDataBase.FindAsync(f => f.DataBaseId == input.Id);
            });
            SystemDataBaseInput systemDataBaseInput = new SystemDataBaseInput();
            if (dataBase != null)
            {
                systemDataBaseInput = new SystemDataBaseInput
                {
                    ConnectionType = dataBase.ConnectionType,
                    ConnectionString = ConfigurationUtil.GetSection(dataBase.ConnectionString)
                };
            }
            return systemDataBaseInput;
        }
        /// <summary>
        /// 获取发送信息字符串
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetSendWorkflowNoticeMessage(WorkflowActivityNoticeConfig config,
            WorkflowProcessInstanceTask task,
            WorkflowProcessInstanceActivity activity,
            SqlDatabaseFixture fix,
            SystemUserInfo userInfo)
        {
            return await AnalysisContent(config.Content, config, task, activity, fix, userInfo);
        }

        /// <summary>
        /// 获取发送信息字符串
        /// </summary>
        /// <returns></returns>
        private async Task<string> GetSendNoticeUrl(WorkflowActivityNoticeConfig config,
            WorkflowProcessInstanceTask task,
            WorkflowProcessInstanceActivity activity,
            SqlDatabaseFixture fix,
            SystemUserInfo userInfo)
        {
            return await AnalysisContent(config.UrlContent, config, task, activity, fix, userInfo);
        }

        /// <summary>
        /// 解析内容
        /// </summary>
        /// <param name="content"></param>
        /// <param name="config"></param>
        /// <param name="task"></param>
        /// <param name="activity"></param>
        /// <param name="fix"></param>
        /// <param name="userInfo"></param>
        /// <returns></returns>
        private async Task<string> AnalysisContent(string content, WorkflowActivityNoticeConfig config,
            WorkflowProcessInstanceTask task,
            WorkflowProcessInstanceActivity activity,
            SqlDatabaseFixture fix,
            SystemUserInfo userInfo)
        {
            if (content.IsNullOrEmpty())
            {
                return "";
            }
            var message = content.ReplaceEditor();
            var h1Elements = content.GetNodes();
            if (h1Elements != null)
            {
                IList<WorkflowEngineFormControls> controls = new List<WorkflowEngineFormControls>();
                foreach (var element in h1Elements)
                {
                    //解码
                    var columnName = HttpUtility.UrlDecode(element.Id);
                    switch (columnName)
                    {
                        case "活动Id":
                            message = message.Replace(element.OuterHtml, task.ActivityId.ToString());
                            break;
                        case "自定义数据":
                            message = message.Replace(element.OuterHtml, task.CustomData.ToString());
                            break;
                        case "任务Id":
                            message = message.Replace(element.OuterHtml, task.TaskId.ToString());
                            break;
                        case "流程Id":
                            message = message.Replace(element.OuterHtml, task.ProcessInstanceId.ToString());
                            break;
                        case "流水号":
                            {
                                var instance = await fix.Db.WorkflowProcessInstance.FindAsync(f => f.ProcessInstanceId == task.ProcessInstanceId);
                                message = message.Replace(element.OuterHtml, instance.Sn);
                            }
                            break;
                        case "流程名称":
                            {
                                var instance = await fix.Db.WorkflowProcessInstance.FindAsync(f => f.ProcessInstanceId == task.ProcessInstanceId);
                                message = message.Replace(element.OuterHtml, instance.Title);
                            }
                            break;
                        case "流程状态":
                            {
                                var instance = await fix.Db.WorkflowProcessInstance.FindAsync(f => f.ProcessInstanceId == task.ProcessInstanceId);
                                message = message.Replace(element.OuterHtml, EnumUtil.FindEnumNameByIndex<EnumProcessInstanceStatus>((int)instance.Status));
                            }
                            break;
                        case "活动名称":
                            if (activity != null)
                            {
                                message = message.Replace(element.OuterHtml, activity.Title);
                            }
                            break;
                        case "当前处理人姓名":
                            message = message.Replace(element.OuterHtml, task.DoUserName);
                            break;
                        case "当前处理人Id":
                            message = message.Replace(element.OuterHtml, task.DoUserId.ToString());
                            break;
                        case "发起人Id":
                            {
                                var instance = await fix.Db.WorkflowProcessInstance.FindAsync(f => f.ProcessInstanceId == task.ProcessInstanceId);
                                message = message.Replace(element.OuterHtml, instance.CreateUserId.ToString());
                            }
                            break;
                        case "发起人姓名":
                            {
                                var instance = await fix.Db.WorkflowProcessInstance.FindAsync(f => f.ProcessInstanceId == task.ProcessInstanceId);
                                message = message.Replace(element.OuterHtml, instance.CreateUserName);
                            }
                            break;
                        case "下一步待处理人Id":
                            {
                                var nextTask = await fix.Db.WorkflowProcessInstanceTask.SetSelect(s => new { s.ReceiveUserId }).FindAsync(f => f.PrevTaskId == task.TaskId && f.ReceiveUserId == userInfo.UserId && f.ProcessInstanceId == task.ProcessInstanceId);
                                if (nextTask != null)
                                {
                                    message = message.Replace(element.OuterHtml, nextTask.ReceiveUserId.ToString());
                                }
                            }
                            break;
                        case "下一步待处理人姓名":
                            {
                                var nextTask = await fix.Db.WorkflowProcessInstanceTask.SetSelect(s => new { s.ReceiveUserName }).FindAsync(f => f.PrevTaskId == task.TaskId && f.ReceiveUserId == userInfo.UserId && f.ProcessInstanceId == task.ProcessInstanceId);
                                if (nextTask != null)
                                {
                                    message = message.Replace(element.OuterHtml, task.ReceiveUserName.ToString());
                                }
                            }
                            break;
                        case "下一步待处理任务Id":
                            {
                                var nextTask = await fix.Db.WorkflowProcessInstanceTask.SetSelect(s => new { s.TaskId }).FindAsync(f => f.PrevTaskId == task.TaskId && f.ReceiveUserId == userInfo.UserId && f.ProcessInstanceId == task.ProcessInstanceId);
                                if (nextTask != null)
                                {
                                    message = message.Replace(element.OuterHtml, nextTask.TaskId.ToString());
                                }
                            }
                            break;
                        default:
                            controls.Add(new WorkflowEngineFormControls
                            {
                                ColumnName = columnName
                            });
                            break;
                    }
                }
                if (controls.Any())
                {
                    var result = await InitFormData(new WorkflowEngineFormInitDataInput
                    {
                        ActivityId = task.ActivityId,
                        FormId = (Guid)activity.FormId,
                        ProcessInstanceId = task.ProcessInstanceId,
                        Controls = controls,
                    });

                    foreach (var element in h1Elements)
                    {
                        //解码
                        var columnName = HttpUtility.UrlDecode(element.Id);
                        //查询值
                        var columnNameResult = result.FirstOrDefault(f => f.Name == columnName);
                        if (columnNameResult != null)
                        {
                            message = message.Replace(element.OuterHtml, columnNameResult.Value);
                        }
                    }
                }
            }
            return message;
        }

        /// <summary>
        /// 获取微信发送信息字符串
        /// </summary>
        /// <returns></returns>
        private async Task<List<SendWeiXinMpNoticeMessageOutput>> GetSendWorkflowWeiXinMpNoticeMessage(
            IList<SendWeiXinMpNoticeMessageOutput> contentNameValues,
            WorkflowProcessInstanceTask task,
            WorkflowProcessInstanceActivity activity,
            SqlDatabaseFixture fix,
            SystemUserInfo userInfo)
        {
            List<SendWeiXinMpNoticeMessageOutput> nameValues = new List<SendWeiXinMpNoticeMessageOutput>();
            var instance = await fix.Db.WorkflowProcessInstance.SetSelect(s => new { s.Sn, s.Title, s.Status }).FindAsync(f => f.ProcessInstanceId == task.ProcessInstanceId);
            foreach (var contentNameValue in contentNameValues)
            {
                var message = contentNameValue.Value;
                var notice = message.Split('{');
                if (notice.Length > 1)
                {
                    IList<WorkflowEngineFormControls> controls = new List<WorkflowEngineFormControls>();
                    IList<string> flowFiledControls = new List<string>();
                    foreach (var item in notice)
                    {
                        if (item.Contains("}"))
                        {
                            var columnName = item.Split('}')[0];
                            if (flowFiled.Contains(columnName))
                            {
                                flowFiledControls.Add(columnName);
                            }
                            else
                            {
                                controls.Add(new WorkflowEngineFormControls
                                {
                                    ColumnName = item.Split('}')[0]
                                });
                            }
                        }
                    }
                    if (controls.Any())
                    {
                        var result = await InitFormData(new WorkflowEngineFormInitDataInput
                        {
                            ActivityId = task.ActivityId,
                            FormId = (Guid)activity.FormId,
                            ProcessInstanceId = task.ProcessInstanceId,
                            Controls = controls
                        });
                        //替换值
                        foreach (var item in result)
                        {
                            message = message.Replace(item.Name, item.Value);
                        }
                    }
                    if (flowFiledControls.Any())
                    {
                        foreach (var item in flowFiledControls)
                        {
                            switch (item)
                            {
                                case "活动Id":
                                    message = message.Replace(item, task.ActivityId.ToString());
                                    break;
                                case "自定义数据":
                                    message = message.Replace(item, task.CustomData.ToString());
                                    break;
                                case "任务Id":
                                    message = message.Replace(item, task.TaskId.ToString());
                                    break;
                                case "流程Id":
                                    message = message.Replace(item, task.ProcessInstanceId.ToString());
                                    break;
                                case "流水号":
                                    message = message.Replace(item, instance.Sn);
                                    break;
                                case "流程名称":
                                    message = message.Replace(item, instance.Title);
                                    break;
                                case "流程状态":
                                    message = message.Replace(item, EnumUtil.FindEnumNameByIndex<EnumProcessInstanceStatus>((int)instance.Status));
                                    break;
                                case "活动名称":
                                    message = message.Replace(item, activity.Title);
                                    break;
                                case "当前处理人姓名":
                                    message = message.Replace(item, task.DoUserName);
                                    break;
                                case "当前处理人Id":
                                    message = message.Replace(item, task.DoUserId.ToString());
                                    break;
                                case "下一步待处理人Id":
                                    {
                                        var nextTask = await fix.Db.WorkflowProcessInstanceTask.SetSelect(s => new { s.ReceiveUserId }).FindAsync(f => f.PrevTaskId == task.TaskId && f.ReceiveUserId == userInfo.UserId && f.ProcessInstanceId == task.ProcessInstanceId);
                                        if (nextTask != null)
                                        {
                                            message = message.Replace(item, nextTask.ReceiveUserId.ToString());
                                        }
                                    }
                                    break;
                                case "下一步待处理人姓名":
                                    {
                                        var nextTask = await fix.Db.WorkflowProcessInstanceTask.SetSelect(s => new { s.ReceiveUserName }).FindAsync(f => f.PrevTaskId == task.TaskId && f.ReceiveUserId == userInfo.UserId && f.ProcessInstanceId == task.ProcessInstanceId);
                                        if (nextTask != null)
                                        {
                                            message = message.Replace(item, task.ReceiveUserName.ToString());
                                        }
                                    }
                                    break;
                                case "下一步待处理任务Id":
                                    {
                                        var nextTask = await fix.Db.WorkflowProcessInstanceTask.SetSelect(s => new { s.TaskId }).FindAsync(f => f.PrevTaskId == task.TaskId && f.ReceiveUserId == userInfo.UserId && f.ProcessInstanceId == task.ProcessInstanceId);
                                        if (nextTask != null)
                                        {
                                            message = message.Replace(item, nextTask.TaskId.ToString());
                                        }
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                    message = message.Replace("{", "").Replace("}", "");
                }
                nameValues.Add(new SendWeiXinMpNoticeMessageOutput
                {
                    Name = contentNameValue.Name,
                    Value = message,
                    Color = contentNameValue.Color
                });
            }
            return nameValues;
        }

        /// <summary>
        /// 无流程处理人时处理策略
        /// </summary>
        /// <param name="input"></param>
        /// <param name="checkOperateStatus"></param>
        /// <param name="fixture"></param>
        /// <returns></returns>
        private async Task<OperateStatus<WorkflowEngineProcessRunOutput>> TaskProcessRunNoNeedChosenPerson(
            WorkflowEngineRunProcessInput input,
            WorkflowProcessInstanceTask checkOperateStatus,
            SqlDatabaseFixture fixture)
        {
            OperateStatus<WorkflowEngineProcessRunOutput> operateStatus = new OperateStatus<WorkflowEngineProcessRunOutput>
            {
                Data = new WorkflowEngineProcessRunOutput()
            };
            var time = DateTime.Now;
            var nowTask = checkOperateStatus;
            nowTask.DoUserId = input.DoUserId;
            nowTask.DoUserName = input.DoUserName;
            nowTask.Comment = input.Comment;
            nowTask.Status = EnumTaskStatus.完成.ToShort();
            nowTask.Remark = input.DoUserName + "|" +
                             time.ToYyyyMMddHHmmss() + "|" +
                             ResourceWorkflowEngine.处理完毕;
            nowTask.CompletedTime = time;
            //更新任务
            if (await fixture.Db.WorkflowProcessInstanceTask.UpdateAsync(nowTask))
            {
                operateStatus.Code = ResultCode.Success;
                operateStatus.Msg = nowTask.Remark;
                return operateStatus;
            }
            operateStatus.Code = ResultCode.Error;
            operateStatus.Msg = Chs.Error;
            return operateStatus;
        }

        /// <summary>
        /// 跳转到指定的任务节点，有预先指定方式，或运行时动态调用方式。
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus> JumpProcess(WorkflowEngineJumpProcessInput input)
        {
            return null;
        }

        /// <summary>
        /// 当前任务节点的上一步节点完成人发现办理有误需撤销，调用此方法，重新回到上一步节点。
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus> RevokeProcess(WorkflowEngineRevokeProcessInput input)
        {
            using (var fixture = new SqlDatabaseFixture())
            {
                OperateStatus operateStatus = new OperateStatus();
                WorkflowProcessInstanceTask task = await fixture.Db.WorkflowProcessInstanceTask.FindAsync(f => f.TaskId == input.TaskId);
                if (task == null)
                {
                    operateStatus.Msg = ResourceWorkflowEngine.未找到流程活动信息;
                    return operateStatus;
                }

                if (task.Status == EnumTaskStatus.撤销.ToShort())
                {
                    operateStatus.Msg = ResourceWorkflowEngine.撤销失败;
                    return operateStatus;
                }

                if (task.Status == EnumTaskStatus.完成.ToShort())
                {
                    operateStatus.Msg = string.Format(ResourceWorkflowEngine.任务已处理, task.DoUserName);
                    return operateStatus;
                }
                //修改当前活动状态
                task.Status = EnumTaskStatus.撤销.ToShort();
                task.DoUserId = input.DoUserId;
                task.DoUserName = input.DoUserName;
                task.CompletedTime = DateTime.Now;
                task.Remark = input.DoUserName + DateTime.Now.ToYyyyMMddHHmmss() + "|" + ResourceWorkflowEngine.已撤销;
                await fixture.Db.WorkflowProcessInstanceTask.UpdateAsync(task);

                var instance = await fixture.Db.WorkflowProcessInstance.FindAsync(f => f.ProcessInstanceId == task.ProcessInstanceId);
                instance.Status = EnumProcessInstanceStatus.撤销.ToShort();
                await fixture.Db.WorkflowProcessInstance.UpdateAsync(instance);
                await UpdateFormStatus(new WorkflowEngineUpdateFormStatusInput
                {
                    ProcessInstanceId = task.ProcessInstanceId,
                    Status = EnumProcessInstanceStatus.撤销
                });
                operateStatus.Code = ResultCode.Success;
                operateStatus.Msg = string.Format(ResourceWorkflowEngine.撤销成功, instance.Sn);
                return operateStatus;
            }
        }

        /// <summary>
        /// 当前任务节点的上一步节点完成人发现办理有误需撤销，调用此方法，重新回到上一步节点。
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus> RevokeByCreateUser(WorkflowEngineRevokeByCreateUserInput input)
        {
            using (var fixture = new SqlDatabaseFixture())
            {
                OperateStatus operateStatus = new OperateStatus();
                foreach (var item in input.ProcessInstanceId.Split(','))
                {
                    var processInstanceId = Guid.Parse(item);
                    var instance = await fixture.Db.WorkflowProcessInstance.FindAsync(f => f.ProcessInstanceId == processInstanceId);
                    List<WorkflowProcessInstanceTask> tasks = (await fixture.Db.WorkflowProcessInstanceTask.FindAllAsync(f => f.ProcessInstanceId == processInstanceId && f.Status == EnumTaskStatus.正在处理.ToShort())).ToList();
                    if (instance == null)
                    {
                        operateStatus.Msg = ResourceWorkflowEngine.未找到流程实例信息;
                        return operateStatus;
                    }

                    foreach (var task in tasks)
                    {
                        if (task.Status == EnumTaskStatus.撤销.ToShort())
                        {
                            operateStatus.Msg = ResourceWorkflowEngine.撤销失败;
                            return operateStatus;
                        }
                    }
                    foreach (var task in tasks)
                    {
                        if (task.Status == EnumTaskStatus.完成.ToShort())
                        {
                            operateStatus.Msg = string.Format(ResourceWorkflowEngine.任务已处理, task.DoUserName);
                            return operateStatus;
                        }
                    }
                    foreach (var task in tasks)
                    {
                        //修改当前活动状态
                        task.Status = EnumTaskStatus.撤销.ToShort();
                        task.DoUserId = input.DoUserId;
                        task.DoUserName = input.DoUserName;
                        task.CompletedTime = DateTime.Now;
                        task.Remark = input.DoUserName + "|" + DateTime.Now.ToYyyyMMddHHmmss() + "|" + ResourceWorkflowEngine.已撤销;
                        await fixture.Db.WorkflowProcessInstanceTask.UpdateAsync(task);
                    }

                    instance.Status = EnumProcessInstanceStatus.撤销.ToShort();
                    await fixture.Db.WorkflowProcessInstance.UpdateAsync(instance);
                    await UpdateFormStatus(new WorkflowEngineUpdateFormStatusInput
                    {
                        ProcessInstanceId = processInstanceId,
                        Status = EnumProcessInstanceStatus.撤销
                    });
                    operateStatus.Code = ResultCode.Success;
                    operateStatus.Msg = string.Format(ResourceWorkflowEngine.撤销成功, instance.Sn);
                }

                return operateStatus;
            }
        }

        /// <summary>
        /// 当前任务办理人退回任务到上一步执行节点。
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus> SendbackProcess(WorkflowEngineSendbackProcessInput input)
        {
            return null;
        }

        /// <summary>
        ///流程结束后仍需返回，由结束节点前的执行人调用此方法，状态回到结束前的节点。
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus> ReverseProcess()
        {
            return null;
        }

        /// <summary>
        /// 知会他人了解任务。向知会人员发送通知并产生一个知会待办项，知会人员查看后任务消失，系统记录知会的发起和查看过程信息。
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus> UnderstandingProcess(WorkflowEngineUnderstandingProcessInput input)
        {
            using (var fixture = new SqlDatabaseFixture())
            {
                OperateStatus operateStatus = new OperateStatus();

                #region 验证

                if (input.UserId.IsNullOrEmpty())
                {
                    operateStatus.Msg = ResourceWorkflowEngine.处理人员为空;
                    return operateStatus;
                }

                WorkflowProcessInstanceTask task = await fixture.Db.WorkflowProcessInstanceTask.FindAsync(f => f.TaskId == input.TaskId);
                if (task == null)
                {
                    operateStatus.Msg = ResourceWorkflowEngine.未找到流程活动信息;
                    return operateStatus;
                }

                //判断当前任务状态是否为完成
                if (task.Status == EnumTaskStatus.完成.ToShort() && !input.FromStart)
                {
                    operateStatus.Msg = string.Format(ResourceWorkflowEngine.任务已处理,
                         task.DoUserName);
                    return operateStatus;
                }

                #endregion

                var userName = input.UserName.Split(',');
                var userId = input.UserId.Split(',');
                var time = DateTime.Now;
                for (int i = 0; i < userId.Length; i++)
                {
                    await fixture.Db.WorkflowProcessInstanceTask.InsertAsync(new WorkflowProcessInstanceTask
                    {
                        TaskId = CombUtil.NewComb(),
                        ProcessInstanceId = task.ProcessInstanceId,
                        SendUserId = input.DoUserId,
                        SendUserName = input.DoUserName,
                        ReceiveUserId = Guid.Parse(userId[i]),
                        ReceiveUserName = userName[i],
                        ReceiveTime = time,
                        Status = EnumTaskStatus.正在处理.ToShort(),
                        ActivityId = task.ActivityId,
                        ActivityType = EnumAcitvityType.知会.ToShort(),
                        PrevTaskId = input.TaskId,
                        Remark = userName[i] + "|" + time.ToYyyyMMddHHmmss() + "|处理中",
                    });
                }

                //修改说明
                var instance = await fixture.Db.WorkflowProcessInstance.FindAsync(f => f.ProcessInstanceId == task.ProcessInstanceId);
                instance.StatusRemark += "</br>知会:" + input.UserName;
                await fixture.Db.WorkflowProcessInstance.UpdateAsync(instance);
                operateStatus.Msg = string.Format(ResourceWorkflowEngine.知会成功, input.UserName);
                operateStatus.Code = ResultCode.Success;
                return operateStatus;
            }
        }

        /// <summary>
        /// 知会已阅
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> UnderstandingReadProcess(WorkflowEngineUnderstandingReadProcessInput input)
        {
            using (var fixture = new SqlDatabaseFixture())
            {
                OperateStatus operateStatus = new OperateStatus();

                #region 验证

                WorkflowProcessInstanceTask task =
                    await fixture.Db.WorkflowProcessInstanceTask.FindAsync(f => f.TaskId == input.TaskId);
                if (task == null)
                {
                    operateStatus.Msg = ResourceWorkflowEngine.未找到流程活动信息;
                    return operateStatus;
                }

                //判断当前任务状态是否为完成
                if (task.Status == EnumTaskStatus.知会已阅.ToShort())
                {
                    operateStatus.Msg = string.Format(ResourceWorkflowEngine.任务已处理,
                         task.DoUserName);
                    return operateStatus;
                }

                #endregion

                var time = DateTime.Now;
                task.DoUserId = input.DoUserId;
                task.DoUserName = input.DoUserName;
                task.CompletedTime = time;
                task.Status = EnumTaskStatus.知会已阅.ToShort();
                task.Comment = input.Comment;
                task.Remark = input.DoUserName + "|" +
                              time.ToYyyyMMddHHmmss() + "|" + ResourceWorkflowEngine.知会已阅;
                if (await fixture.Db.WorkflowProcessInstanceTask.UpdateAsync(task))
                {
                    operateStatus.Msg = ResourceWorkflowEngine.知会已阅成功;
                    operateStatus.Code = ResultCode.Success;
                }
                //判断当前节点是否都完成
                var allTask = await fixture.Db.WorkflowProcessInstanceTask.FindAllAsync(f => f.PrevTaskId == task.PrevTaskId && f.Status == EnumTaskStatus.正在处理.ToShort());
                if (!allTask.Any())
                {
                    //进行下一步
                    //下一节点
                    var instanceLinks = await fixture.Db.WorkflowProcessInstanceLink.SetSelect(s => new { s.End }).FindAllAsync(f => f.Begin == task.ActivityId && f.ProcessInstanceId == task.ProcessInstanceId);
                    if (instanceLinks.Any())
                    {
                        var linksTo = instanceLinks.Select(s => s.End).ToList();
                        var activitys = await fixture.Db.WorkflowProcessInstanceActivity.SetSelect(s => new { s.ActivityId, s.Type }).FindAllAsync(f => linksTo.Contains(f.ActivityId) && f.ProcessInstanceId == task.ProcessInstanceId);

                        if (activitys.Any())
                        {
                            //是否具有结束
                            if (activitys.Any(a => a.Type == "end"))
                            {
                                var endAct = activitys.FirstOrDefault(w => w.Type == EnumAcitvityType.结束.FindDescription());
                                var endTask = new WorkflowProcessInstanceTask
                                {
                                    TaskId = CombUtil.NewComb(),
                                    ProcessInstanceId = task.ProcessInstanceId,
                                    ActivityId = endAct.ActivityId,
                                    ActivityType = ConvertIndexByDes(endAct.Type),
                                    SendUserId = input.DoUserId,
                                    SendUserName = input.DoUserName,
                                    DoUserId = input.DoUserId,
                                    DoUserName = input.DoUserName,
                                    ReceiveUserId = input.DoUserId,
                                    ReceiveUserName = input.DoUserName,
                                    ReceiveTime = time,
                                    CompletedTime = time,
                                    CustomData = task.CustomData,
                                    Status = EnumTaskStatus.完成.ToShort(),
                                    Remark = input.DoUserName + "|" + time.ToYyyyMMddHHmmss() + "|提交",
                                    PrevTaskId = input.TaskId
                                };
                                await fixture.Db.WorkflowProcessInstanceTask.InsertAsync(endTask);

                                await EndProcess(new WorkflowEngineEndProcessInput
                                {
                                    TaskId = task.TaskId,
                                    DoUserId = input.DoUserId,
                                    DoUserName = input.DoUserName
                                });
                            }
                            else
                            {
                                //循环执行剩余节点
                            }
                        }
                    }
                }
                return operateStatus;
            }
        }

        /// <summary>
        ///邀请他人对任务给出指导、发表意见。向邀请人员发送通知并产生一个阅示待办项，阅示人员批阅时需给出意见
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus> InvitationReadProcess(WorkflowEngineInvitationReadInput input)
        {
            using (var fixture = new SqlDatabaseFixture())
            {
                OperateStatus operateStatus = new OperateStatus();

                #region 验证

                if (input.UserId.IsNullOrEmpty())
                {
                    operateStatus.Msg = ResourceWorkflowEngine.处理人员为空;
                    return operateStatus;
                }

                WorkflowProcessInstanceTask task = await fixture.Db.WorkflowProcessInstanceTask.FindAsync(f => f.TaskId == input.TaskId);
                if (task == null)
                {
                    operateStatus.Msg = ResourceWorkflowEngine.未找到流程活动信息;
                    return operateStatus;
                }

                //判断当前任务状态是否为完成
                if (task.Status == EnumTaskStatus.完成.ToShort())
                {
                    operateStatus.Msg = string.Format(ResourceWorkflowEngine.任务已处理,
                         task.DoUserName);
                    return operateStatus;
                }

                #endregion

                var userName = input.UserName.Split(',');
                var userId = input.UserId.Split(',');
                var time = DateTime.Now;
                for (int i = 0; i < userId.Length; i++)
                {
                    await fixture.Db.WorkflowProcessInstanceTask.InsertAsync(new WorkflowProcessInstanceTask
                    {
                        TaskId = CombUtil.NewComb(),
                        ProcessInstanceId = task.ProcessInstanceId,
                        SendUserId = input.DoUserId,
                        SendUserName = input.DoUserName,
                        ReceiveUserId = Guid.Parse(userId[i]),
                        ReceiveUserName = userName[i],
                        ReceiveTime = time,
                        Status = EnumTaskStatus.正在处理.ToShort(),
                        ActivityId = task.ActivityId,
                        ActivityType = EnumAcitvityType.阅示.ToShort(),
                        PrevTaskId = input.TaskId,
                        Remark = userName[i] + "|" + time.ToYyyyMMddHHmmss() + "|处理中",
                    });
                }

                //修改说明
                var instance = await fixture.Db.WorkflowProcessInstance.FindAsync(f => f.ProcessInstanceId == task.ProcessInstanceId);
                instance.StatusRemark += "</br>阅示:" + input.UserName;
                await fixture.Db.WorkflowProcessInstance.UpdateAsync(instance);
                operateStatus.Msg = string.Format(ResourceWorkflowEngine.阅示成功, input.UserName);
                operateStatus.Code = ResultCode.Success;
                //判断当前节点是否都完成
                var allTask = await fixture.Db.WorkflowProcessInstanceTask.FindAllAsync(f => f.PrevTaskId == task.PrevTaskId && f.Status == EnumTaskStatus.正在处理.ToShort());
                if (!allTask.Any())
                {
                    //进行下一步
                    //下一节点
                    var instanceLinks = await fixture.Db.WorkflowProcessInstanceLink.SetSelect(s => new { s.End }).FindAllAsync(f => f.Begin == task.ActivityId && f.ProcessInstanceId == task.ProcessInstanceId);
                    if (instanceLinks.Any())
                    {
                        var linksTo = instanceLinks.Select(s => s.End).ToList();
                        var activitys = await fixture.Db.WorkflowProcessInstanceActivity.SetSelect(s => new { s.ActivityId, s.Type }).FindAllAsync(f => linksTo.Contains(f.ActivityId) && f.ProcessInstanceId == task.ProcessInstanceId);

                        if (activitys.Any())
                        {
                            //是否具有结束
                            if (activitys.Any(a => a.Type == "end"))
                            {
                                var endAct = activitys.FirstOrDefault(w => w.Type == EnumAcitvityType.结束.FindDescription());
                                var endTask = new WorkflowProcessInstanceTask
                                {
                                    TaskId = CombUtil.NewComb(),
                                    ProcessInstanceId = task.ProcessInstanceId,
                                    ActivityId = endAct.ActivityId,
                                    ActivityType = ConvertIndexByDes(endAct.Type),
                                    SendUserId = input.DoUserId,
                                    SendUserName = input.DoUserName,
                                    DoUserId = input.DoUserId,
                                    DoUserName = input.DoUserName,
                                    ReceiveUserId = input.DoUserId,
                                    ReceiveUserName = input.DoUserName,
                                    ReceiveTime = time,
                                    CompletedTime = time,
                                    CustomData = task.CustomData,
                                    Status = EnumTaskStatus.完成.ToShort(),
                                    Remark = input.DoUserName + "|" + time.ToYyyyMMddHHmmss() + "|提交",
                                    PrevTaskId = input.TaskId
                                };
                                await fixture.Db.WorkflowProcessInstanceTask.InsertAsync(endTask);

                                await EndProcess(new WorkflowEngineEndProcessInput
                                {
                                    TaskId = task.TaskId,
                                    DoUserId = input.DoUserId,
                                    DoUserName = input.DoUserName
                                });
                            }
                            else
                            {
                                //循环执行剩余节点
                            }
                        }
                    }
                }
                return operateStatus;
            }
        }

        /// <summary>
        ///将任务退回给发起人，发起人修改后可重新提交也可撤销申请。
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus> ReturnAndWriteProcess(WorkflowEngineReturnAndWriteProcessInput input)
        {
            using (var fixture = new SqlDatabaseFixture())
            {
                OperateStatus operateStatus = new OperateStatus();

                #region 基本检查

                WorkflowProcessInstanceTask task = await fixture.Db.WorkflowProcessInstanceTask.FindAsync(f => f.TaskId == input.TaskId);
                if (task == null)
                {
                    operateStatus.Msg = ResourceWorkflowEngine.未找到流程活动信息;
                    return operateStatus;
                }

                //判断当前任务状态是否为完成
                if (task.Status == EnumTaskStatus.完成.ToShort())
                {
                    operateStatus.Msg = string.Format(ResourceWorkflowEngine.任务已处理,
                         task.DoUserName);
                    return operateStatus;
                }

                #endregion

                #region 获取开始节点

                var startTask = ((await fixture.Db.WorkflowProcessInstanceTask.FindAllAsync(f => f.ProcessInstanceId == task.ProcessInstanceId && f.ActivityType == EnumAcitvityType.开始.ToShort())).OrderBy(o => o.CompletedTime)).FirstOrDefault();
                var time = DateTime.Now;
                if (startTask != null)
                {
                    //修改当前任务
                    task.Status = EnumTaskStatus.完成.ToShort();
                    task.Remark = input.DoUserName + "|" + time.ToYyyyMMddHHmmss() + "|" + ResourceWorkflowEngine.处理完毕;
                    task.DoUserId = input.DoUserId;
                    task.DoUserName = input.DoUserName;
                    task.Comment = input.Comment;
                    task.CompletedTime = DateTime.Now;
                    task.CustomData = task.CustomData;
                    await fixture.Db.WorkflowProcessInstanceTask.UpdateAsync(task);

                    //插入任务
                    WorkflowProcessInstanceTask instanceTask = new WorkflowProcessInstanceTask()
                    {
                        TaskId = CombUtil.NewComb(),
                        ProcessInstanceId = task.ProcessInstanceId,
                        SendUserId = input.DoUserId,
                        SendUserName = input.DoUserName,
                        ReceiveUserId = startTask.SendUserId,
                        ReceiveTime = time,
                        ReceiveUserName = startTask.SendUserName,
                        Status = EnumTaskStatus.正在处理.ToShort(),
                        Remark = startTask.SendUserName + "|" + time.ToYyyyMMddHHmmss() + "|处理中",
                        PrevTaskId = input.TaskId,
                        CustomData = task.CustomData,
                        ActivityId = startTask.ActivityId,
                        ActivityType = EnumAcitvityType.开始.ToShort()
                    };
                    await fixture.Db.WorkflowProcessInstanceTask.InsertAsync(instanceTask);

                    //处理实例信息
                    WorkflowProcessInstance instance =
                        await fixture.Db.WorkflowProcessInstance.FindAsync(f => f.ProcessInstanceId == task.ProcessInstanceId);
                    instance.Status = EnumProcessInstanceStatus.退回.ToShort();
                    instance.StatusRemark = string.Format(ResourceWorkflowEngine.退回重填, startTask.SendUserName);
                    await fixture.Db.WorkflowProcessInstance.UpdateAsync(instance);
                    await UpdateFormStatus(new WorkflowEngineUpdateFormStatusInput
                    {
                        ProcessInstanceId = task.ProcessInstanceId,
                        Status = EnumProcessInstanceStatus.退回
                    });
                    operateStatus.Msg = ResourceWorkflowEngine.退回重填成功;
                    operateStatus.Code = ResultCode.Success;
                }

                #endregion

                return operateStatus;
            }
        }

        /// <summary>
        /// 将任务退回给到指定活动
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> ReturnProcess(WrofklowEngineReturnProcessInput input)
        {
            using (var fixture = new SqlDatabaseFixture())
            {
                OperateStatus operateStatus = new OperateStatus();

                #region 基本检查

                WorkflowProcessInstanceTask task =
                    await fixture.Db.WorkflowProcessInstanceTask.FindAsync(f => f.TaskId == input.TaskId);
                if (task == null)
                {
                    operateStatus.Msg = ResourceWorkflowEngine.未找到流程活动信息;
                    return operateStatus;
                }

                //判断当前任务状态是否为完成
                if (task.Status == EnumTaskStatus.完成.ToShort())
                {
                    operateStatus.Msg = string.Format(ResourceWorkflowEngine.任务已处理,
                         task.DoUserName);
                    return operateStatus;
                }

                #endregion

                #region 根据活动Id查询

                if (!task.PrevTaskId.IsNullOrEmptyGuid())
                {
                    var prevTasks =
                        await _processInstanceTaskRepository.FindProcessInstanceTaskByPrevTaskId(
                            new IdInput((Guid)task.PrevTaskId));
                    foreach (var t in prevTasks)
                    {
                        t.Status = EnumTaskStatus.退回.ToShort();
                        t.DoUserId = input.DoUserId;
                        t.DoUserName = input.DoUserName;
                        t.Comment = input.Comment;
                        t.CompletedTime = DateTime.Now;
                        await fixture.Db.WorkflowProcessInstanceTask.UpdateAsync(t);
                    }
                }
                else
                {
                    //修改当前活动状态
                    task.Status = EnumTaskStatus.退回.ToShort();
                    task.DoUserId = input.DoUserId;
                    task.DoUserName = input.DoUserName;
                    task.Comment = input.Comment;
                    task.CompletedTime = DateTime.Now;
                    await fixture.Db.WorkflowProcessInstanceTask.UpdateAsync(task);
                }

                var activity = (await _processInstanceActivityRepository.FindReturnDoUser(
                    new WorkflowEngineFindReturnActivityInput
                    {
                        ActivityId = input.ActivityId,
                        ProcessInstanceId = task.ProcessInstanceId
                    })).ToList();
                if (!activity.Any())
                {
                    operateStatus.Msg = ResourceWorkflowEngine.未找到流程处理人;
                    return operateStatus;
                }

                StringBuilder domessage = new StringBuilder();
                StringBuilder statusRemark = new StringBuilder();
                var time = DateTime.Now;
                foreach (var item in activity)
                {
                    await fixture.Db.WorkflowProcessInstanceTask.InsertAsync(new WorkflowProcessInstanceTask
                    {
                        TaskId = CombUtil.NewComb(),
                        ProcessInstanceId = task.ProcessInstanceId,
                        ActivityId = input.ActivityId,
                        ActivityType = EnumAcitvityType.审批.ToShort(),
                        SendUserId = input.DoUserId,
                        SendUserName = input.DoUserName,
                        ReceiveUserId = item.DoUserId,
                        ReceiveUserName = item.DoUserName,
                        ReceiveTime = time,
                        Status = EnumActivityStatus.正在处理.ToShort(),
                        Remark = item.DoUserName + "|" + time.ToYyyyMMddHHmmss() + "|处理中",
                        PrevTaskId = task.TaskId
                    });
                    domessage.Append($"{item.DoUserName},");
                    statusRemark.Append($"{item.Title}:{item.DoUserName},");
                }

                WorkflowProcessInstance instance =
                    await fixture.Db.WorkflowProcessInstance.FindAsync(f => f.ProcessInstanceId == task.ProcessInstanceId);
                instance.StatusRemark = string.Format(ResourceWorkflowEngine.退回, statusRemark.ToString().TrimEnd(','));
                await fixture.Db.WorkflowProcessInstance.UpdateAsync(instance);
                operateStatus.Code = ResultCode.Success;
                operateStatus.Msg =
                    string.Format(ResourceWorkflowEngine.发起流程成功任务退回到下一步, domessage.ToString().TrimEnd(','));

                //根据人员插入

                #endregion

                return operateStatus;
            }
        }

        /// <summary>
        /// 取消拒绝
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> CancelProcess(WorkflowEngineRefuseProcessInput input)
        {
            using (var fixture = new SqlDatabaseFixture())
            {
                OperateStatus operateStatus = new OperateStatus();

                #region 基本检查

                WorkflowProcessInstanceTask task =
                    await fixture.Db.WorkflowProcessInstanceTask.FindAsync(f => f.TaskId == input.TaskId);
                if (task == null)
                {
                    operateStatus.Msg = ResourceWorkflowEngine.未找到流程活动信息;
                    return operateStatus;
                }

                //判断当前任务状态是否为完成
                if (task.Status == EnumTaskStatus.完成.ToShort())
                {
                    operateStatus.Msg = string.Format(ResourceWorkflowEngine.任务已处理,
                         task.DoUserName);
                    return operateStatus;
                }

                #endregion

                #region 根据活动Id查询

                //修改当前活动状态
                task.Status = EnumTaskStatus.取消.ToShort();
                task.DoUserId = input.DoUserId;
                task.DoUserName = input.DoUserName;
                task.Comment = input.Comment;
                task.CompletedTime = DateTime.Now;
                task.Remark = input.DoUserName +
                              DateTime.Now.ToYyyyMMddHHmmss() + "|" + ResourceWorkflowEngine.取消成功;
                await fixture.Db.WorkflowProcessInstanceTask.UpdateAsync(task);

                WorkflowProcessInstance instance =
                    await fixture.Db.WorkflowProcessInstance.FindAsync(f => f.ProcessInstanceId == task.ProcessInstanceId);
                instance.StatusRemark = ResourceWorkflowEngine.已取消;
                instance.Status = EnumProcessInstanceStatus.取消.ToShort();
                await fixture.Db.WorkflowProcessInstance.UpdateAsync(instance);
                await UpdateFormStatus(new WorkflowEngineUpdateFormStatusInput
                {
                    ProcessInstanceId = task.ProcessInstanceId,
                    Status = EnumProcessInstanceStatus.取消
                });
                operateStatus.Code = ResultCode.Success;
                operateStatus.Msg = string.Format(ResourceWorkflowEngine.取消成功, instance.Sn);

                #endregion

                return operateStatus;
            }
        }

        /// <summary>
        /// 任务拒绝
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> RefuseProcess(WorkflowEngineRefuseProcessInput input)
        {
            using (var fixture = new SqlDatabaseFixture())
            {
                OperateStatus operateStatus = new OperateStatus();

                #region 基本检查

                WorkflowProcessInstanceTask task =
                    await fixture.Db.WorkflowProcessInstanceTask.FindAsync(f => f.TaskId == input.TaskId);
                if (task == null)
                {
                    operateStatus.Msg = ResourceWorkflowEngine.未找到流程活动信息;
                    return operateStatus;
                }

                //判断当前任务状态是否为完成
                if (task.Status == EnumTaskStatus.完成.ToShort())
                {
                    operateStatus.Msg = string.Format(ResourceWorkflowEngine.任务已处理,
                         task.DoUserName);
                    return operateStatus;
                }

                #endregion

                #region 根据活动Id查询

                //修改当前活动状态
                task.Status = EnumTaskStatus.拒绝.ToShort();
                task.DoUserId = input.DoUserId;
                task.DoUserName = input.DoUserName;
                task.Comment = input.Comment;
                task.CompletedTime = DateTime.Now;
                task.Remark = input.DoUserName +
                              DateTime.Now.ToYyyyMMddHHmmss() + "|" + ResourceWorkflowEngine.已拒绝;
                await fixture.Db.WorkflowProcessInstanceTask.UpdateAsync(task);

                WorkflowProcessInstance instance =
                    await fixture.Db.WorkflowProcessInstance.FindAsync(f => f.ProcessInstanceId == task.ProcessInstanceId);
                instance.StatusRemark = ResourceWorkflowEngine.已拒绝;
                instance.Status = EnumProcessInstanceStatus.拒绝.ToShort();
                await fixture.Db.WorkflowProcessInstance.UpdateAsync(instance);
                await UpdateFormStatus(new WorkflowEngineUpdateFormStatusInput
                {
                    ProcessInstanceId = task.ProcessInstanceId,
                    Status = EnumProcessInstanceStatus.拒绝
                });
                operateStatus.Code = ResultCode.Success;
                operateStatus.Msg = string.Format(ResourceWorkflowEngine.拒绝成功, instance.Sn);

                #endregion

                return operateStatus;
            }
        }

        /// <summary>
        /// 任务召回
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> RecallProcess(WorkflowEngineRecallProcessInput input)
        {
            using (var fixture = new SqlDatabaseFixture())
            {
                OperateStatus operateStatus = new OperateStatus();

                #region 基本检查

                WorkflowProcessInstanceTask task = await fixture.Db.WorkflowProcessInstanceTask.FindAsync(f => f.TaskId == input.TaskId);
                if (task == null)
                {
                    operateStatus.Msg = ResourceWorkflowEngine.未找到流程活动信息;
                    return operateStatus;
                }

                if (task.Status == EnumTaskStatus.召回.ToShort())
                {
                    operateStatus.Msg = ResourceWorkflowEngine.召回失败;
                    return operateStatus;
                }

                if (task.Status == EnumTaskStatus.完成.ToShort())
                {
                    operateStatus.Msg = string.Format(ResourceWorkflowEngine.任务已处理, task.DoUserName);
                    return operateStatus;
                }

                //判断当前任务状态是否为完成
                var tasks = (await fixture.Db.WorkflowProcessInstanceTask.FindAllAsync(f => f.PrevTaskId == input.TaskId)).ToList();
                if (tasks.Any(a => a.Status != EnumTaskStatus.正在处理.ToShort()))
                {
                    operateStatus.Msg = ResourceWorkflowEngine.下级任务已处理;
                    return operateStatus;
                }
                #endregion

                #region 根据活动Id查询

                //修改当前活动状态
                task.Status = EnumTaskStatus.召回.ToShort();
                task.DoUserId = input.DoUserId;
                task.DoUserName = input.DoUserName;
                task.CompletedTime = DateTime.Now;
                task.Remark = input.DoUserName +
                              DateTime.Now.ToYyyyMMddHHmmss() + "|" + ResourceWorkflowEngine.已召回;
                await fixture.Db.WorkflowProcessInstanceTask.UpdateAsync(task);

                foreach (var recall in tasks)
                {
                    recall.Status = EnumTaskStatus.召回.ToShort();
                    recall.DoUserId = input.DoUserId;
                    recall.DoUserName = input.DoUserName;
                    recall.CompletedTime = DateTime.Now;
                    recall.Remark = input.DoUserName +
                                  DateTime.Now.ToYyyyMMddHHmmss() + "|" + ResourceWorkflowEngine.已召回;
                    await fixture.Db.WorkflowProcessInstanceTask.UpdateAsync(recall);
                }

                var time = DateTime.Now;
                //插入新待办
                task.TaskId = CombUtil.NewComb();
                task.Status = EnumTaskStatus.正在处理.ToShort();
                task.DoUserId = null;
                task.DoUserName = null;
                task.CompletedTime = null;
                task.ReceiveTime = time;
                task.Remark = input.DoUserName +
                              time.ToYyyyMMddHHmmss() + "|处理中";
                await fixture.Db.WorkflowProcessInstanceTask.InsertAsync(task);
                WorkflowProcessInstance instance = await fixture.Db.WorkflowProcessInstance.FindAsync(f => f.ProcessInstanceId == task.ProcessInstanceId);
                operateStatus.Code = ResultCode.Success;
                operateStatus.Msg = string.Format(ResourceWorkflowEngine.召回成功, instance.Sn);

                #endregion

                return operateStatus;
            }
        }

        /// <summary>
        /// 加签:
        /// 1,判断当前节点是否为开始节点
        ///   1.1,开始节点:插入实例及开始信息
        ///   1.2,非开始节点:修改当前状态
        /// 2,判断加签类别:并行加签,串行加签
        ///   1.1,并行加签则给每个人都发送代办
        ///   1.2,串行加签则给第一个发送代办
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> AddProcess(WorkflowEngineAddProcessInput input)
        {
            OperateStatus operateStatus = new OperateStatus();
            if (input.Extend.UserId.IsNullOrEmpty())
            {
                operateStatus.Msg = ResourceWorkflowEngine.处理人员为空;
                return operateStatus;
            }
            var addOprateStatus = input.ActivityType == "start" ? await AddStartProcess(input) : await AddTaskProcess(input);
            if (addOprateStatus.Code == ResultCode.Success)
            {
                //插入加签流程
                operateStatus = input.Extend.Type == EnumTaskAddType.并行 ? await AddProcessParallel(input, addOprateStatus.Data) : await AddProcessSerial(input, addOprateStatus.Data);
            }
            return operateStatus;
        }

        /// <summary>
        /// 加签审批
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> AddApproveProcess(WorkflowEngineAddProcessInput input)
        {
            using (var fixture = new SqlDatabaseFixture())
            {
                OperateStatus operateStatus = new OperateStatus();

                #region 基础检测

                WorkflowProcessInstanceTask task = await fixture.Db.WorkflowProcessInstanceTask.FindAsync(f => f.TaskId == input.TaskId);
                if (task == null)
                {
                    operateStatus.Msg = ResourceWorkflowEngine.未找到流程活动信息;
                    return operateStatus;
                }

                //判断当前任务状态是否为完成
                if (task.Status == EnumTaskStatus.完成.ToShort())
                {
                    operateStatus.Msg = string.Format(ResourceWorkflowEngine.任务已处理,
                         task.DoUserName);
                    return operateStatus;
                }

                #endregion

                var time = DateTime.Now;
                var addExtend = task.Extend.JsonStringToObject<WorkflowEngineAddProcessExtendInput>();
                if (addExtend.Type == EnumTaskAddType.串行)
                {
                    //所有流程处理人
                    var users = addExtend.UserId.Split(',');
                    var length = Array.IndexOf(users, input.DoUserId.ToString());
                    //是否最后一步
                    if (users.Length == length + 1)
                    {
                        //判断前一步骤和当前扩展中的第一步骤是否一致,若一致则判断是回到哪,若不一致则回到上一步骤并发送代办
                        var belongToTask = await fixture.Db.WorkflowProcessInstanceTask.FindAsync(f => f.TaskId == addExtend.BelongToTaskId);
                        if (addExtend.Approve == EnumTaskAddApprove.进入下一关卡)
                        {
                            //推送下一步骤
                            return await TaskProcessRun(new WorkflowEngineRunProcessInput
                            {
                                TaskId = belongToTask.TaskId,
                                DoUserId = input.DoUserId,
                                DoUserName = input.DoUserName,
                                Comment = input.Comment,
                                ActivityId = belongToTask.ActivityId
                            });
                        }
                        //判断所有完成后流程走向
                        else if (addExtend.Approve == EnumTaskAddApprove.回到本关卡)
                        {
                            if (addExtend.FromTaskId != addExtend.BelongToTaskId)
                            {
                                belongToTask = await fixture.Db.WorkflowProcessInstanceTask.FindAsync(f => f.TaskId == addExtend.FromTaskId);
                            }
                            //获取当前加签步骤
                            WorkflowProcessInstanceTask instancetask = new WorkflowProcessInstanceTask
                            {
                                TaskId = CombUtil.NewComb(),
                                ProcessInstanceId = task.ProcessInstanceId,
                                ActivityId = belongToTask.ActivityId,
                                ActivityType = belongToTask.ActivityType,
                                SendUserId = input.DoUserId,
                                SendUserName = input.DoUserName,
                                ReceiveUserId = (Guid)belongToTask.DoUserId,
                                ReceiveUserName = belongToTask.DoUserName,
                                ReceiveTime = time,
                                Status = EnumActivityStatus.正在处理.ToShort(),
                                Extend = belongToTask.Extend,
                                Remark = belongToTask.DoUserName + "|" + time.ToYyyyMMddHHmmss() + "|处理中",
                                PrevTaskId = task.TaskId
                            };
                            var insert = await fixture.Db.WorkflowProcessInstanceTask.InsertAsync(instancetask);
                            //更新当前流程
                            if (insert)
                            {
                                WorkflowProcessInstance instance = await fixture.Db.WorkflowProcessInstance.FindAsync(f => f.ProcessInstanceId == task.ProcessInstanceId);
                                instance.StatusRemark += "<br/>" + instancetask.Remark;
                                await fixture.Db.WorkflowProcessInstance.UpdateAsync(instance);
                            }
                            operateStatus.Code = ResultCode.Success;
                            operateStatus.Msg = string.Format(ResourceWorkflowEngine.加签回到发起关卡,
                                belongToTask.DoUserName);
                        }
                    }

                    else
                    {
                        //下一个人
                        var curruser = users[length + 1];
                        var currusername = addExtend.UserName.Split(',')[length + 1];
                        WorkflowProcessInstanceTask instancetask = new WorkflowProcessInstanceTask
                        {
                            TaskId = CombUtil.NewComb(),
                            ProcessInstanceId = task.ProcessInstanceId,
                            ActivityId = task.ActivityId,
                            ActivityType = EnumAcitvityType.加签.ToShort(),
                            SendUserId = input.DoUserId,
                            SendUserName = input.DoUserName,
                            ReceiveUserId = Guid.Parse(curruser),
                            ReceiveUserName = currusername,
                            ReceiveTime = time,
                            Status = EnumActivityStatus.正在处理.ToShort(),
                            Extend = task.Extend,
                            Remark = currusername + "|" + time.ToYyyyMMddHHmmss() + "|加签处理中",
                            PrevTaskId = task.TaskId
                        };
                        var insert = await fixture.Db.WorkflowProcessInstanceTask.InsertAsync(instancetask);
                        //更新当前流程实例新
                        if (insert)
                        {
                            WorkflowProcessInstance instance =
                                await fixture.Db.WorkflowProcessInstance.FindAsync(f => f.ProcessInstanceId == task.ProcessInstanceId);
                            instance.StatusRemark += "<br/>" + instancetask.Remark;
                            await fixture.Db.WorkflowProcessInstance.UpdateAsync(instance);
                        }

                        operateStatus.Code = ResultCode.Success;
                        operateStatus.Msg = string.Format(ResourceWorkflowEngine.加签处理成功, currusername);
                    }
                }

                #region 处理当前步骤

                task.DoUserId = input.DoUserId;
                task.DoUserName = input.DoUserName;
                task.CompletedTime = DateTime.Now;
                task.Status = EnumTaskStatus.完成.ToShort();
                task.Comment = input.Comment;
                task.Remark = input.DoUserName + "|" + time.ToYyyyMMddHHmmss() + "|" + ResourceWorkflowEngine.处理完毕;
                await fixture.Db.WorkflowProcessInstanceTask.UpdateAsync(task);

                #endregion

                if (addExtend.Type == EnumTaskAddType.并行)
                {
                    //判断同一个上一任务是否都完成
                    var prevTaks = await _processInstanceTaskRepository.FindProcessInstanceTaskByPrevTaskId(new IdInput((Guid)task.PrevTaskId));
                    //是否都完成
                    var other = prevTaks.Where(w => w.Status != EnumTaskStatus.完成.ToShort()).ToList();
                    //是否最后一步
                    if (other.Count == 0)
                    {
                        //判断前一步骤和当前扩展中的第一步骤是否一致,若一致则判断是回到哪,若不一致则回到上一步骤并发送代办

                        var belongToTask = await fixture.Db.WorkflowProcessInstanceTask.FindAsync(f => f.TaskId == addExtend.BelongToTaskId);
                        if (addExtend.Approve == EnumTaskAddApprove.进入下一关卡)
                        {
                            //推送下一步骤
                            operateStatus = await TaskProcessRun(new WorkflowEngineRunProcessInput
                            {
                                TaskId = belongToTask.TaskId,
                                DoUserId = input.DoUserId,
                                DoUserName = input.DoUserName,
                                Comment = input.Comment,
                                ActivityId = belongToTask.ActivityId
                            });
                        }
                        //判断所有完成后流程走向
                        if (addExtend.Approve == EnumTaskAddApprove.回到本关卡)
                        {
                            if (addExtend.FromTaskId != addExtend.BelongToTaskId)
                            {
                                belongToTask = await fixture.Db.WorkflowProcessInstanceTask.FindAsync(f => f.TaskId == addExtend.FromTaskId);
                            }
                            //获取当前加签步骤
                            WorkflowProcessInstanceTask instancetask = new WorkflowProcessInstanceTask
                            {
                                TaskId = CombUtil.NewComb(),
                                ProcessInstanceId = task.ProcessInstanceId,
                                ActivityId = belongToTask.ActivityId,
                                ActivityType = belongToTask.ActivityType,
                                SendUserId = input.DoUserId,
                                SendUserName = input.DoUserName,
                                ReceiveUserId = (Guid)belongToTask.DoUserId,
                                ReceiveUserName = belongToTask.DoUserName,
                                ReceiveTime = time,
                                Status = EnumActivityStatus.正在处理.ToShort(),
                                Extend = belongToTask.Extend,
                                Remark = belongToTask.DoUserName + "|" + time.ToYyyyMMddHHmmss() + "|处理中",
                                PrevTaskId = task.TaskId
                            };
                            var insert = await fixture.Db.WorkflowProcessInstanceTask.InsertAsync(instancetask);
                            //更新当前流程
                            if (insert)
                            {
                                WorkflowProcessInstance instance =
                                    await fixture.Db.WorkflowProcessInstance.FindAsync(f => f.ProcessInstanceId == task.ProcessInstanceId);
                                instance.StatusRemark += "<br/>" + instancetask.Remark;
                                await fixture.Db.WorkflowProcessInstance.UpdateAsync(instance);
                            }

                            operateStatus.Code = ResultCode.Success;
                            operateStatus.Msg = string.Format(ResourceWorkflowEngine.加签处理成功,
                                belongToTask.DoUserName);
                        }
                    }
                    else
                    {
                        //下一个人
                        operateStatus.Code = ResultCode.Success;
                        StringBuilder stringBuilder = new StringBuilder();
                        //提示信息
                        foreach (var item in other)
                        {
                            stringBuilder.Append(item.ReceiveUserName + $",");
                        }

                        operateStatus.Msg = string.Format(ResourceWorkflowEngine.加签并行处理成功,
                            stringBuilder.ToString().TrimEnd(','));
                    }
                }
                return operateStatus;
            }
        }


        /// <summary>
        /// 开始活动加签
        /// </summary>
        /// <returns></returns>
        private async Task<OperateStatus<WorkflowProcessInstanceTask>> AddStartProcess(WorkflowEngineAddProcessInput input)
        {
            using (var fixture = new SqlDatabaseFixture())
            {
                var trans = fixture.Db.BeginTransaction();
                OperateStatus<WorkflowProcessInstanceTask> operateStatus = new OperateStatus<WorkflowProcessInstanceTask>();
                WorkflowEngineProcessInstanceOutput instance = await FindWorkflowEngineProcessInstance(fixture, new WorkflowEngineStartProcessInput()
                {
                    ProcessInstanceId = input.ProcessInstanceId,
                    ProcessId = input.ProcessId,
                    Title = input.Title,
                    Urgency = (EnumProcessInstanceUrgency)input.Urgency,
                    CreateUserId = input.DoUserId,
                    CreateUserName = input.DoUserName,
                    CreateUserOrganizationId = input.DoUserOrganizationId,
                    CreateUserOrganizationName = input.DoUserOrganizationName,
                });
                var startActivity = instance.InstanceActivities.ToList().FirstOrDefault(w => w.Type == EnumAcitvityType.开始.FindDescription());
                if (startActivity == null)
                {
                    operateStatus.Msg = ResourceWorkflowEngine.流程配置错误无开始节点;
                    return operateStatus;
                }
                var time = DateTime.Now;
                WorkflowProcessInstanceTask task = new WorkflowProcessInstanceTask
                {
                    TaskId = CombUtil.NewComb(),
                    ProcessInstanceId = instance.Instance.ProcessInstanceId,
                    ActivityId = startActivity.ActivityId,
                    ActivityType = EnumAcitvityType.开始.ToShort(),
                    SendUserId = input.DoUserId,
                    SendUserName = input.DoUserName,
                    DoUserId = input.DoUserId,
                    DoUserName = input.DoUserName,
                    ReceiveUserId = input.DoUserId,
                    ReceiveUserName = input.DoUserName,
                    ReceiveTime = time,
                    CompletedTime = time,
                    Status = EnumTaskStatus.完成.ToShort(),
                    Remark = input.DoUserName + "|" + time.ToYyyyMMddHHmmss() + "|提交"
                };
                try
                {
                    //判断类型
                    if (instance.Instance.Type == EnumProcessInstanceType.范本.ToShort())
                    {
                        //全部数据复制一份

                    }
                    await fixture.Db.WorkflowProcessInstance.InsertAsync(instance.Instance, trans);
                    await fixture.Db.WorkflowProcessInstanceActivity.BulkInsertAsync(instance.InstanceActivities, trans);
                    await fixture.Db.WorkflowProcessInstanceLink.BulkInsertAsync(instance.InstanceLinks, trans);
                    await fixture.Db.WorkflowProcessInstanceTask.InsertAsync(task, trans);
                }
                catch (WorkflowEngineException e)
                {
                    trans.Rollback();
                    operateStatus.Msg = e.Message;
                    return operateStatus;
                }
                trans.Commit();
                operateStatus.Code = ResultCode.Success;
                operateStatus.Data = task;
                return operateStatus;
            }
        }

        /// <summary>
        /// 任务活动加签
        /// </summary>
        /// <returns></returns>
        private async Task<OperateStatus<WorkflowProcessInstanceTask>> AddTaskProcess(WorkflowEngineAddProcessInput input)
        {
            using (var fixture = new SqlDatabaseFixture())
            {
                OperateStatus<WorkflowProcessInstanceTask> operateStatus = new OperateStatus<WorkflowProcessInstanceTask>();

                #region 基础检测

                WorkflowProcessInstanceTask task = await fixture.Db.WorkflowProcessInstanceTask.FindAsync(f => f.TaskId == input.TaskId);
                if (task == null)
                {
                    operateStatus.Msg = ResourceWorkflowEngine.未找到流程活动信息;
                    return operateStatus;
                }

                //判断当前任务状态是否为完成
                if (task.Status == EnumTaskStatus.完成.ToShort())
                {
                    operateStatus.Msg = string.Format(ResourceWorkflowEngine.任务已处理, task.DoUserName);
                    return operateStatus;
                }
                #endregion

                var time = DateTime.Now;
                task.DoUserId = input.DoUserId;
                task.DoUserName = input.DoUserName;
                task.CompletedTime = DateTime.Now;
                task.Status = EnumTaskStatus.完成.ToShort();
                task.Comment = input.Comment;
                task.Remark = input.DoUserName + "|" + time.ToYyyyMMddHHmmss() + "|" + ResourceWorkflowEngine.处理完毕;
                await fixture.Db.WorkflowProcessInstanceTask.UpdateAsync(task);
                operateStatus.Data = task;
                operateStatus.Code = ResultCode.Success;
                return operateStatus;
            }
        }

        /// <summary>
        /// 加签串行
        /// </summary>
        /// <param name="input"></param>
        /// <param name="task"></param>
        /// <returns></returns>
        private async Task<OperateStatus> AddProcessSerial(WorkflowEngineAddProcessInput input,
            WorkflowProcessInstanceTask task)
        {
            using (var fixture = new SqlDatabaseFixture())
            {
                var trans = fixture.Db.BeginTransaction();
                OperateStatus operateStatus = new OperateStatus();
                StringBuilder domessage = new StringBuilder();
                StringBuilder statusRemark = new StringBuilder();
                var time = DateTime.Now;
                var userName = input.Extend.UserName.Split(',');
                var userId = input.Extend.UserId.Split(',');

                //判断任务是否具有归属TaskId
                if (task.Extend.IsNotNullOrEmpty())
                {
                    var extend = task.Extend.JsonStringToObject<WorkflowEngineAddProcessExtendInput>();
                    input.Extend.BelongToTaskId = extend.BelongToTaskId;
                    input.Extend.FromTaskId = task.TaskId;
                }
                else
                {
                    input.Extend.BelongToTaskId = task.TaskId;
                    input.Extend.FromTaskId = task.TaskId;
                }

                WorkflowProcessInstanceTask instancetask = new WorkflowProcessInstanceTask
                {
                    TaskId = CombUtil.NewComb(),
                    ProcessInstanceId = task.ProcessInstanceId,
                    ActivityId = input.ActivityId,
                    ActivityType = EnumAcitvityType.加签.ToShort(),
                    SendUserId = input.DoUserId,
                    SendUserName = input.DoUserName,
                    ReceiveUserId = Guid.Parse(userId[0]),
                    ReceiveUserName = userName[0],
                    ReceiveTime = time,
                    Status = EnumActivityStatus.正在处理.ToShort(),
                    Extend = input.Extend.ObjectToJsonString(),
                    Remark = userName[0] + "|" + time.ToYyyyMMddHHmmss() + "|加签处理中",
                    PrevTaskId = task.TaskId
                };
                domessage.Append($"{userName[0]},");
                statusRemark.Append($"加签:{userName[0]},");
                WorkflowProcessInstance instance = await fixture.Db.WorkflowProcessInstance.FindAsync(f => f.ProcessInstanceId == task.ProcessInstanceId);
                instance.StatusRemark += string.Format("<br/>" + ResourceWorkflowEngine.加签, statusRemark.ToString().TrimEnd(','));
                try
                {
                    await fixture.Db.WorkflowProcessInstanceTask.InsertAsync(instancetask);
                    await fixture.Db.WorkflowProcessInstance.UpdateAsync(instance);
                }
                catch (Exception e)
                {
                    operateStatus.Msg = e.Message;
                    trans.Rollback();
                    return operateStatus;
                }
                trans.Commit();
                operateStatus.Code = ResultCode.Success;
                operateStatus.Msg = string.Format(ResourceWorkflowEngine.加签成功, domessage.ToString().TrimEnd(','));
                return operateStatus;
            }
        }

        /// <summary>
        /// 加签并行
        /// </summary>
        /// <param name="input"></param>
        /// <param name="task"></param>
        /// <returns></returns>
        private async Task<OperateStatus> AddProcessParallel(WorkflowEngineAddProcessInput input, WorkflowProcessInstanceTask task)
        {
            using (var fixture = new SqlDatabaseFixture())
            {
                var trans = fixture.Db.BeginTransaction();
                OperateStatus operateStatus = new OperateStatus();
                IList<WorkflowProcessInstanceTask> tasks = new List<WorkflowProcessInstanceTask>();
                StringBuilder domessage = new StringBuilder();
                StringBuilder statusRemark = new StringBuilder();
                var time = DateTime.Now;
                var userName = input.Extend.UserName.Split(',');
                var userId = input.Extend.UserId.Split(',');
                if (task.Extend.IsNotNullOrEmpty())
                {
                    var extend = task.Extend.JsonStringToObject<WorkflowEngineAddProcessExtendInput>();
                    input.Extend.BelongToTaskId = extend.BelongToTaskId;
                    input.Extend.FromTaskId = task.TaskId;
                }
                else
                {
                    input.Extend.BelongToTaskId = task.TaskId;
                    input.Extend.FromTaskId = task.TaskId;
                }

                for (int i = 0; i < userId.Length; i++)
                {
                    tasks.Add(new WorkflowProcessInstanceTask
                    {
                        TaskId = CombUtil.NewComb(),
                        ProcessInstanceId = task.ProcessInstanceId,
                        ActivityId = input.ActivityId,
                        ActivityType = EnumAcitvityType.加签.ToShort(),
                        SendUserId = input.DoUserId,
                        SendUserName = input.DoUserName,
                        ReceiveUserId = Guid.Parse(userId[i]),
                        ReceiveUserName = userName[i],
                        ReceiveTime = time,
                        Status = EnumActivityStatus.正在处理.ToShort(),
                        Remark = userName[i] + "|" + time.ToYyyyMMddHHmmss() + "|加签处理中",
                        PrevTaskId = task.TaskId,
                        Extend = input.Extend.ObjectToJsonString()
                    });
                    domessage.Append($"{userName[i]},");
                    statusRemark.Append($"加签:{userName[i]},");
                }

                WorkflowProcessInstance instance = await fixture.Db.WorkflowProcessInstance.FindAsync(f => f.ProcessInstanceId == task.ProcessInstanceId, trans);
                instance.StatusRemark += string.Format("<br/>" + ResourceWorkflowEngine.加签, statusRemark.ToString().TrimEnd(','));
                try
                {
                    await fixture.Db.WorkflowProcessInstanceTask.BulkInsertAsync(tasks, trans);
                    await fixture.Db.WorkflowProcessInstance.UpdateAsync(instance, trans);
                }
                catch (Exception e)
                {
                    operateStatus.Msg = e.Message;
                    trans.Rollback();
                    return operateStatus;
                }
                trans.Commit();
                operateStatus.Code = ResultCode.Success;
                operateStatus.Msg = string.Format(ResourceWorkflowEngine.加签成功, domessage.ToString().TrimEnd(','));
                return operateStatus;
            }
        }

        /// <summary>
        ///删除后的任务将被放入系统回收箱，管理员可以【恢复】或【彻底删除】回收箱中的任务。
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus> DeleteByTaskId(WorkflowEngineDeleteByTaskIdInput input)
        {
            using (var fixture = new SqlDatabaseFixture())
            {
                OperateStatus operateStatus = new OperateStatus();

                #region 基本检查

                WorkflowProcessInstanceTask task = await fixture.Db.WorkflowProcessInstanceTask.FindAsync(f => f.TaskId == input.TaskId);
                if (task == null)
                {
                    operateStatus.Msg = ResourceWorkflowEngine.未找到流程活动信息;
                    return operateStatus;
                }

                if (task.Status == EnumTaskStatus.删除.ToShort())
                {
                    operateStatus.Msg = ResourceWorkflowEngine.删除失败;
                    return operateStatus;
                }

                if (task.Status == EnumTaskStatus.完成.ToShort())
                {
                    operateStatus.Msg = string.Format(ResourceWorkflowEngine.任务已处理, task.DoUserName);
                    return operateStatus;
                }
                #endregion

                var tasks = await fixture.Db.WorkflowProcessInstanceTask.FindAllAsync(f => f.ProcessInstanceId == task.ProcessInstanceId && f.TaskId != task.TaskId);
                //修改当前活动状态
                task.Status = EnumTaskStatus.删除.ToShort();
                task.DoUserId = input.DoUserId;
                task.DoUserName = input.DoUserName;
                task.CompletedTime = DateTime.Now;
                task.Remark = input.DoUserName +
                              DateTime.Now.ToYyyyMMddHHmmss() + "|" + ResourceWorkflowEngine.已删除;
                await fixture.Db.WorkflowProcessInstanceTask.UpdateAsync(task);

                foreach (var recall in tasks)
                {
                    recall.Status = EnumTaskStatus.删除.ToShort();
                    recall.DoUserId = input.DoUserId;
                    recall.DoUserName = input.DoUserName;
                    recall.CompletedTime = DateTime.Now;
                    await fixture.Db.WorkflowProcessInstanceTask.UpdateAsync(recall);
                }

                WorkflowProcessInstance instance = await fixture.Db.WorkflowProcessInstance.FindAsync(f => f.ProcessInstanceId == task.ProcessInstanceId);
                instance.Status = EnumProcessInstanceStatus.删除.ToShort();
                instance.StatusRemark = input.Comment;
                await fixture.Db.WorkflowProcessInstance.UpdateAsync(instance);
                await UpdateFormStatus(new WorkflowEngineUpdateFormStatusInput
                {
                    ProcessInstanceId = task.ProcessInstanceId,
                    Status = EnumProcessInstanceStatus.删除
                });
                operateStatus.Code = ResultCode.Success;
                operateStatus.Msg = string.Format(ResourceWorkflowEngine.删除成功, instance.Sn);
                return operateStatus;
            }
        }

        /// <summary>
        ///删除后的任务将被放入系统回收箱，管理员可以【恢复】或【彻底删除】回收箱中的任务。
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus> DeleteByProcessInstanceId(WorkflowEngineDeleteByProcessInstanceIdInput input)
        {
            using (var fixture = new SqlDatabaseFixture())
            {
                OperateStatus operateStatus = new OperateStatus();
                WorkflowProcessInstance instance = await fixture.Db.WorkflowProcessInstance.FindAsync(f => f.ProcessInstanceId == input.ProcessInstanceId);
                instance.Status = EnumProcessInstanceStatus.删除.ToShort();
                instance.StatusRemark = input.Comment;
                instance.UpdateTime = DateTime.Now;
                instance.UpdateUserId = input.DoUserId;
                instance.UpdateUserName = input.DoUserName;
                instance.UpdateUserOrganizationId = input.DoUserOrganizationId;
                instance.UpdateUserOrganizationName = input.DoUserOrganizationName;
                await fixture.Db.WorkflowProcessInstance.UpdateAsync(instance);
                await UpdateFormStatus(new WorkflowEngineUpdateFormStatusInput
                {
                    ProcessInstanceId = input.ProcessInstanceId,
                    Status = EnumProcessInstanceStatus.删除
                });
                operateStatus.Code = ResultCode.Success;
                operateStatus.Msg = string.Format(ResourceWorkflowEngine.删除成功, instance.Sn);
                return operateStatus;
            }
        }

        /// <summary>
        /// 【彻底删除】任务。
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<IList<WorkflowEngineFormDeleteDataInput>>> DeletePhysicsProcessInstanceId(IdInput<string> input)
        {
            OperateStatus<IList<WorkflowEngineFormDeleteDataInput>> operateStatus = new OperateStatus<IList<WorkflowEngineFormDeleteDataInput>>();
            using (var fixture = new SqlDatabaseFixture())
            {
                IList<WorkflowEngineFormDeleteDataInput> output = new List<WorkflowEngineFormDeleteDataInput>();
                var trans = fixture.Db.BeginTransaction();
                try
                {
                    foreach (var id in input.Id.Split(','))
                    {
                        var instance = await fixture.Db.WorkflowProcessInstance.FindAsync(f => f.ProcessInstanceId == Guid.Parse(id), trans);
                        if (instance != null)
                        {
                            var form = await fixture.Db.WorkflowProcess.FindAsync(f => f.ProcessId == instance.ProcessId, trans);
                            if (form != null)
                            {
                                var workflowEngineFormDeleteDataInput = new WorkflowEngineFormDeleteDataInput
                                {
                                    ProcessInstanceId = instance.ProcessInstanceId
                                };
                                if (form.FormId != null)
                                {
                                    workflowEngineFormDeleteDataInput.FormId = (Guid)form.FormId;
                                }
                                output.Add(workflowEngineFormDeleteDataInput);
                            }
                            await fixture.Db.WorkflowProcessInstanceTask.DeleteAsync(d => d.ProcessInstanceId == Guid.Parse(id), trans);
                            await fixture.Db.WorkflowProcessInstanceActivity.DeleteAsync(d => d.ProcessInstanceId == Guid.Parse(id), trans);
                            await fixture.Db.WorkflowProcessInstanceLink.DeleteAsync(d => d.ProcessInstanceId == Guid.Parse(id), trans);
                            await fixture.Db.WorkflowProcessInstance.DeleteAsync(d => d.ProcessInstanceId == Guid.Parse(id), trans);
                        }
                    }
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    operateStatus.Msg = e.Message;
                    return operateStatus;
                }
                trans.Commit();
                operateStatus.Code = ResultCode.Success;
                operateStatus.Msg = Chs.Successful;
                operateStatus.Data = output;

                if (operateStatus.Code == ResultCode.Success)
                {
                    await DeleteFormData(operateStatus.Data);
                }
            }
            return operateStatus;
        }

        /// <summary>
        /// 【恢复】任务。
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus> RecoveryDeleteByProcessInstanceId(IdInput<string> input)
        {
            OperateStatus operateStatus = new OperateStatus();
            using (var fixture = new SqlDatabaseFixture())
            {
                var trans = fixture.Db.BeginTransaction();
                try
                {
                    foreach (var id in input.Id.Split(','))
                    {
                        //查询流程实例
                        var instance = await fixture.Db.WorkflowProcessInstance.FindAsync(f => f.ProcessInstanceId == Guid.Parse(id), trans);
                        var status = EnumTaskStatus.删除.ToShort();
                        var tasks = await fixture.Db.WorkflowProcessInstanceTask.FindAllAsync(f => f.ProcessInstanceId == instance.ProcessInstanceId && f.Status == status, trans);
                        foreach (var item in tasks)
                        {
                            item.Status = EnumTaskStatus.正在处理.ToShort();
                        }
                        instance.Status = EnumProcessInstanceStatus.处理中.ToShort();
                        if (tasks.Any())
                        {
                            await fixture.Db.WorkflowProcessInstanceTask.BulkUpdateAsync(tasks, trans);
                        }
                        await fixture.Db.WorkflowProcessInstance.UpdateAsync(instance, trans);
                    }
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    operateStatus.Msg = e.Message;
                    return operateStatus;
                }
                trans.Commit();
                foreach (var id in input.Id.Split(','))
                {
                    await UpdateFormStatus(new WorkflowEngineUpdateFormStatusInput
                    {
                        ProcessInstanceId = Guid.Parse(id),
                        Status = EnumProcessInstanceStatus.处理中
                    });
                }
                operateStatus.Code = ResultCode.Success;
                operateStatus.Msg = Chs.Successful;
            }
            return operateStatus;
        }

        /// <summary>
        /// 直接结束流程
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus> EndProcess(WorkflowEngineEndProcessInput input)
        {
            OperateStatus operateStatus = new OperateStatus();
            using (var fixture = new SqlDatabaseFixture())
            {
                var task = await fixture.Db.WorkflowProcessInstanceTask.SetSelect(s => new { s.ProcessInstanceId, s.CustomData, s.ActivityId }).FindAsync(f => f.TaskId == input.TaskId);
                var instance = await fixture.Db.WorkflowProcessInstance.FindAsync(f => f.ProcessInstanceId == task.ProcessInstanceId);
                var time = DateTime.Now;
                var allTasks = await fixture.Db.WorkflowProcessInstanceTask.FindAllAsync(w => w.Status == EnumTaskStatus.正在处理.ToShort() && w.ProcessInstanceId == task.ProcessInstanceId);
                foreach (var item in allTasks)
                {
                    item.DoUserId = input.DoUserId;
                    item.DoUserName = input.DoUserName;
                    item.Comment = input.Comment;
                    item.Status = EnumTaskStatus.完成.ToShort();
                    item.Remark = input.DoUserName + "|" + time.ToYyyyMMddHHmmss() + "|" + ResourceWorkflowEngine.处理完毕;
                    item.CompletedTime = time;
                    await fixture.Db.WorkflowProcessInstanceTask.UpdateAsync(item);
                }

                //修改主流程
                var status = EnumProcessInstanceStatus.完成.ToShort();
                var statusRemark = ResourceWorkflowEngine.处理完毕;
                await fixture.Db.WorkflowProcessInstance.UpdateAsync(u => u.ProcessInstanceId == task.ProcessInstanceId, new { Status = status, StatusRemark = statusRemark });

                await UpdateFormStatus(new WorkflowEngineUpdateFormStatusInput
                {
                    ProcessInstanceId = task.ProcessInstanceId,
                    Status = EnumProcessInstanceStatus.完成
                });

                //插入强制结束任务
                var endAct = await fixture.Db.WorkflowProcessInstanceActivity.SetSelect(s => new { s.ActivityId, s.Type }).FindAsync(f => f.ProcessInstanceId == task.ProcessInstanceId && f.Type == "end");
                var endTask = new WorkflowProcessInstanceTask
                {
                    TaskId = CombUtil.NewComb(),
                    ProcessInstanceId = task.ProcessInstanceId,
                    ActivityId = endAct.ActivityId,
                    ActivityType = ConvertIndexByDes(endAct.Type),
                    SendUserId = input.DoUserId,
                    SendUserName = input.DoUserName,
                    DoUserId = input.DoUserId,
                    DoUserName = input.DoUserName,
                    ReceiveUserId = input.DoUserId,
                    ReceiveUserName = input.DoUserName,
                    ReceiveTime = time,
                    CompletedTime = time,
                    CustomData = task.CustomData,
                    Status = EnumTaskStatus.完成.ToShort(),
                    Remark = input.DoUserName + "|" + time.ToYyyyMMddHHmmss() + "|直接结束",
                    PrevTaskId = input.TaskId
                };

                await fixture.Db.WorkflowProcessInstanceTask.InsertAsync(endTask);
                operateStatus.Msg = string.Format(ResourceWorkflowEngine.流程已结束, instance.Sn);
                operateStatus.Code = ResultCode.Success;
                //子流程
                if (!instance.FromTaskId.IsNullOrEmptyGuid())
                {
                    var activities = await fixture.Db.WorkflowProcessInstanceActivity.SetSelect(s => new { s.ActivityId, s.FormId }).FindAllAsync(f => f.ProcessInstanceId == task.ProcessInstanceId);
                    var instanceActivities = activities.FirstOrDefault(f => f.ActivityId == task.ActivityId);
                    await RunSubProcessEndTask(fixture, new WorkflowEngineRunSubProcessEndTaskInput
                    {
                        FormId = instanceActivities.FormId,
                        ProcessInstanceId = task.ProcessInstanceId,
                        Instance = instance,
                        DoUserName = input.DoUserName,
                        DoUserId = input.DoUserId,
                    });
                }
                return operateStatus;
            }
        }

        /// <summary>
        ///任务自由调度。可以将任务向前或向后调度到已处理或未进入过的关卡，允许从多个正在执行的关卡调度到一个关卡
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus> DispatchProcess()
        {
            return null;
        }

        /// <summary>
        ///获取待处理任务
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<PagedResults<WorkflowEngineFindNeedDoOutput>>> FindNeedDo(WorkflowEngineFindNeedDoInput input)
        {
            return OperateStatus<PagedResults<WorkflowEngineFindNeedDoOutput>>.Success(await _processInstanceRepository.FindNeedDo(input));
        }

        /// <summary>
        /// 获取已办事项
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<PagedResults<WorkflowEngineFindHaveDoOutput>>> FindHaveDo(WorkflowEngineFindHaveDoInput input)
        {
            return OperateStatus<PagedResults<WorkflowEngineFindHaveDoOutput>>.Success(await _processInstanceRepository.FindHaveDo(input));
        }

        /// <summary>
        ///获取我的请求
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<PagedResults<WorkflowEngineFindHaveSendOutput>>> FindHaveSend(WorkflowEngineFindHaveSendInput input)
        {
            return OperateStatus<PagedResults<WorkflowEngineFindHaveSendOutput>>.Success(await _processInstanceRepository.FindHaveSeed(input));
        }

        /// <summary>
        ///获取超时任务
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<PagedResults<WorkflowEngineFindOverTimeOutput>>> FindOverTime(WorkflowEngineFindHaveSendInput input)
        {
            return OperateStatus<PagedResults<WorkflowEngineFindOverTimeOutput>>.Success(await _processInstanceRepository.FindOverTime(input));
        }

        /// <summary>
        ///获取流程数量
        /// </summary>
        /// <returns></returns>
        public async Task<WorkflowSearchNumOutput> FindNum(IdInput input)
        {
            return await _processInstanceRepository.FindNum(input);
        }
        /// <summary>
        /// 获取流程实例过程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<WorkflowEngineFindTaskByProcessInstanceIdOutput>> FindTaskByProcessInstanceId(WorkflowEngineFindTaskByProcessInstanceIdInput input)
        {
            using (var fix = new SqlDatabaseFixture())
            {
                var data = await fix.Db.WorkflowProcessInstanceTask.FindAsync(f => f.Status == 4 && f.ReceiveUserId == input.ReceiveUserId && f.ProcessInstanceId == input.ProcessInstanceId);
                WorkflowEngineFindTaskByProcessInstanceIdOutput output = new WorkflowEngineFindTaskByProcessInstanceIdOutput();
                if (data != null)
                {
                    output.TaskId = data.TaskId;
                    output.ActivityId = data.ActivityId;
                }
                return OperateStatus<WorkflowEngineFindTaskByProcessInstanceIdOutput>.Success(output);
            }
        }
        /// <summary>
        /// 获取流程实例过程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<WorkflowEngineFindInstanceProcessOutput>>> FindInstanceProcess(WorkflowEngineFindInstanceProcessInput input)
        {
            var process = (await _processInstanceTaskRepository.FindInstanceProcess(input)).ToList();
            foreach (var activity in process)
            {
                switch (activity.ActivityType)
                {
                    case (short)EnumAcitvityType.知会:
                        activity.Title = EnumAcitvityType.知会.ToString();
                        break;
                    case (short)EnumAcitvityType.加签:
                        activity.Title += $"({EnumAcitvityType.加签})";
                        break;
                }
            }
            return OperateStatus<IEnumerable<WorkflowEngineFindInstanceProcessOutput>>.Success(process);
        }

        /// <summary>
        /// 根据实例Id获取对应活动信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<WorkflowEngineFindActivityByProcessIdAndTypeOutput>>> FindActivityByProcessIdAndType(WorkflowEngineFindActivityByProcessIdAndTypeInput input)
        {
            return OperateStatus<IEnumerable<WorkflowEngineFindActivityByProcessIdAndTypeOutput>>.Success(await _processInstanceActivityRepository.FindActivityByProcessIdAndType(input));
        }

        /// <summary>
        /// 根据活动Id获取活动信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<WorkflowEngineFindActivityByTaskIdOutput>> FindActivityByTaskId(IdInput input)
        {
            return OperateStatus<WorkflowEngineFindActivityByTaskIdOutput>.Success(await _processInstanceActivityRepository.FindActivityByTaskId(input));
        }

        /// <summary>
        ///  根据实例Id获取表单信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<WorkflowEngineFindFormByProcessInstanceIdOutput> FindWorkflowProcessFormByProcessInstanceId(IdInput input)
        {
            return await _processInstanceActivityRepository.FindWorkflowProcessFormByProcessInstanceId(input);
        }

        /// <summary>
        /// 退回活动
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<WorkflowEngineFindReturnActivityOutput>>> FindReturnActivity(WorkflowEngineFindReturnActivityInput input)
        {
            return OperateStatus<IEnumerable<WorkflowEngineFindReturnActivityOutput>>.Success(await _processInstanceActivityRepository.FindReturnActivity(input));
        }

        #region 流程监控

        /// <summary>
        /// 流程监控
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<PagedResults<WorkflowEngineFindMonitorListOuput>>> FindMonitorList(
            WorkflowEngineFindMonitorListInput input)
        {
            return OperateStatus<PagedResults<WorkflowEngineFindMonitorListOuput>>.Success(await _processInstanceRepository.FindMonitorList(input));
        }

        /// <summary>
        /// 流程监控
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResults<WorkflowEngineFindMonitorDeleteListOuput>> FindMonitorDeleteList(
            WorkflowEngineFindMonitorDeleteListInput input)
        {
            return await _processInstanceRepository.FindMonitorDeleteList(input);
        }
        /// <summary>
        /// 获取设计流程图
        /// </summary>
        /// <param name="links"></param>
        /// <param name="activities"></param>
        /// <param name="tasks"></param>
        /// <returns></returns>
        private string FindDesignJson(
            IList<WorkflowProcessInstanceLink> links,
            IList<WorkflowProcessInstanceActivity> activities,
            IList<WorkflowProcessInstanceTask> tasks)
        {
            IList<WorkflowEngineFindMonitorFlowNodeListOutput> nodeList = new List<WorkflowEngineFindMonitorFlowNodeListOutput>();
            foreach (var activity in activities)
            {
                var state = GetProcessinstanceActivity(activity.ProcessInstanceId, activity.ActivityId).JsonStringToObject<WorkflowEngineFindMonitorFlowNodeListOutput>();

                var activityAllTask = tasks.Where(w => w.ActivityId == activity.ActivityId).ToList();
                //判断当前节点是否通过
                var task = (activityAllTask.Where(w => w.Status != EnumTaskStatus.完成.ToShort())).FirstOrDefault();
                if (task != null)
                {
                    state.ispass = true;
                    state.status = task.Status;
                    state.remark = task.Remark;
                }
                else
                {
                    task = activityAllTask.FirstOrDefault();
                    if (task != null)
                    {
                        state.ispass = true;
                        state.status = task.Status;
                        state.remark = task.Remark;
                    }
                }
                nodeList.Add(state);
            }

            IList<WorkflowEngineFindMonitorFlowLinkListOutput> linkList = new List<WorkflowEngineFindMonitorFlowLinkListOutput>();
            foreach (var li in links)
            {
                var link = li.Json.JsonStringToObject<WorkflowEngineFindMonitorFlowLinkListOutput>();
                link.cls.linkColor = li.IsPass ? "red" : "#2a2929";
                //查找是否具有已到达的节点
                var toTask = tasks.Where(w => w.ActivityId == li.End).OrderBy(o => o.ReceiveTime).FirstOrDefault();
                //判断上一步信息
                var prevTask = links.FirstOrDefault(f => f.LinkId == li.LinkId);
                if (prevTask != null)
                {
                    if (toTask != null) link.remark = toTask.Remark;
                }
                linkList.Add(link);
            }
            return JsonConvert.SerializeObject(new
            {
                nodeList = nodeList,
                linkList = linkList,
                attr = new
                {
                    id = "flow-" + CombUtil.NewComb()
                },
                remarks = new List<string>(),
                config = new
                {
                    showGrid = true,
                    showGridText = "隐藏网格",
                    showGridIcon = "eye",
                }
            });
        }

        /// <summary>
        /// 获取流程图
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<WorkflowEngineFindMonitorFlowOutput>> FindMonitorFlowChart(IdInput input)
        {
            using (var fixture = new SqlDatabaseFixture())
            {
                WorkflowEngineFindMonitorFlowOutput output = new WorkflowEngineFindMonitorFlowOutput();
                var activities = (await fixture.Db.WorkflowProcessInstanceActivity.FindAllAsync(f => f.ProcessInstanceId == input.Id)).ToList();
                var links = (await fixture.Db.WorkflowProcessInstanceLink.FindAllAsync(f => f.ProcessInstanceId == input.Id)).ToList();
                var tasks = (await fixture.Db.WorkflowProcessInstanceTask.FindAllAsync(f => f.ProcessInstanceId == input.Id)).ToList();
                output.DesignJson = FindDesignJson(links, activities, tasks);
                output.InstanceProcess = tasks;
                return OperateStatus<WorkflowEngineFindMonitorFlowOutput>.Success(output);
            }
        }

        /// <summary>
        /// 获取流程图
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<WorkflowEngineFindMonitorFlowOutput> FindMonitorArchive(IdInput input)
        {
            using (var fixture = new SqlDatabaseFixture())
            {
                WorkflowEngineFindMonitorFlowOutput output = new WorkflowEngineFindMonitorFlowOutput();
                var archive = await fixture.Db.WorkflowArchive.SetSelect(s => new { s.TaskId }).FindAsync(f => f.ArchiveId == input.Id);
                var processInstanceId = (await fixture.Db.WorkflowProcessInstanceTask.SetSelect(s => new { s.ProcessInstanceId }).FindAsync(f => f.TaskId == archive.TaskId)).ProcessInstanceId;
                var tasks = (await fixture.Db.WorkflowProcessInstanceTask.FindAllAsync(f => f.ProcessInstanceId == processInstanceId)).ToList();
                output.DesignJson = archive.DesignJson;
                output.InstanceProcess = tasks;
                return output;
            }
        }

        /// <summary>
        /// 根据流程Id获取任务信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<WorkflowEngineFindByTaskIdOutput>> FindTaskById(IdInput input)
        {
            using (var fixture = new SqlDatabaseFixture())
            {
                var task = (await fixture.Db.WorkflowProcessInstanceTask.FindAsync(f => f.TaskId == input.Id)).MapTo<WorkflowProcessInstanceTask, WorkflowEngineFindByTaskIdOutput>();
                var user = await fixture.Db.SystemUserInfo.SetSelect(s => new { s.HeadImage }).FindAsync(f => f.UserId == task.ReceiveUserId);
                return OperateStatus<WorkflowEngineFindByTaskIdOutput>.Success(task);
            }
        }

        /// <summary>
        /// 阅示已阅
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> InvitationReadProcessSure(WorkflowEngineInvitationReadSureInput input)
        {
            using (var fixture = new SqlDatabaseFixture())
            {
                OperateStatus operateStatus = new OperateStatus();

                #region 验证

                WorkflowProcessInstanceTask task = await fixture.Db.WorkflowProcessInstanceTask.FindAsync(f => f.TaskId == input.TaskId);
                if (task == null)
                {
                    operateStatus.Msg = ResourceWorkflowEngine.未找到流程活动信息;
                    return operateStatus;
                }

                //判断当前任务状态是否为完成
                if (task.Status == EnumTaskStatus.完成.ToShort())
                {
                    operateStatus.Msg = string.Format(ResourceWorkflowEngine.任务已处理,
                         task.DoUserName);
                    return operateStatus;
                }
                #endregion

                var time = DateTime.Now;
                task.DoUserId = input.DoUserId;
                task.DoUserName = input.DoUserName;
                task.CompletedTime = time;
                task.Status = EnumTaskStatus.阅示已阅.ToShort();
                task.Comment = input.Comment;
                task.Remark = input.DoUserName + time.ToYyyyMMddHHmmss() + "|" + ResourceWorkflowEngine.阅示已阅;
                if (await fixture.Db.WorkflowProcessInstanceTask.UpdateAsync(task))
                {
                    operateStatus.Msg = ResourceWorkflowEngine.阅示已阅成功;
                    operateStatus.Code = ResultCode.Success;
                }
                return operateStatus;
            }
        }

        /// <summary>
        /// 阅示已阅
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> InvitationReadProcessApprove(WorkflowEngineInvitationReadApproveInput input)
        {
            using (var fixture = new SqlDatabaseFixture())
            {
                OperateStatus operateStatus = new OperateStatus();

                #region 验证

                WorkflowProcessInstanceTask task = await fixture.Db.WorkflowProcessInstanceTask.FindAsync(f => f.TaskId == input.TaskId);
                if (task == null)
                {
                    operateStatus.Msg = ResourceWorkflowEngine.未找到流程活动信息;
                    return operateStatus;
                }

                //判断当前任务状态是否为完成
                if (task.Status == EnumTaskStatus.完成.ToShort())
                {
                    operateStatus.Msg = string.Format(ResourceWorkflowEngine.任务已处理,
                         task.DoUserName);
                    return operateStatus;
                }
                #endregion

                var time = DateTime.Now;
                task.DoUserId = input.DoUserId;
                task.DoUserName = input.DoUserName;
                task.CompletedTime = time;
                task.Status = EnumTaskStatus.阅示已阅待审核.ToShort();
                task.Comment = input.Comment;
                task.Remark = input.DoUserName + time.ToYyyyMMddHHmmss() + "|" + ResourceWorkflowEngine.阅示已阅待审核;
                if (await fixture.Db.WorkflowProcessInstanceTask.UpdateAsync(task))
                {
                    var processInstance = await fixture.Db.WorkflowProcessInstance.SetSelect(s => new { s.ProcessId }).FindAsync(f => f.ProcessInstanceId == task.ProcessInstanceId);
                    var process = await fixture.Db.WorkflowProcess.SetSelect(s => new { s.FormId }).FindAsync(f => f.ProcessId == processInstance.ProcessId);

                    await _publisher.PublishAsync("MessageSubscribe", new WorkflowEngineRunProcessInput
                    {
                        Notice = input.Notice,
                        TaskId = input.TaskId,
                        ProcessInstanceId = task.ProcessInstanceId,
                        ActivityId = task.ActivityId,
                        FormId = process.FormId,
                    });
                    operateStatus.Msg = "审核通过";
                    operateStatus.Code = ResultCode.Success;
                }
                return operateStatus;
            }
        }

        /// <summary>
        /// 批阅后审核拒绝
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> InvitationReadProcessApproveRefuse(WorkflowEngineInvitationReadApproveInput input)
        {
            using (var fixture = new SqlDatabaseFixture())
            {
                OperateStatus operateStatus = new OperateStatus();

                #region 验证

                WorkflowProcessInstanceTask task = await fixture.Db.WorkflowProcessInstanceTask.FindAsync(f => f.TaskId == input.TaskId);
                if (task == null)
                {
                    operateStatus.Msg = ResourceWorkflowEngine.未找到流程活动信息;
                    return operateStatus;
                }

                //判断当前任务状态是否为完成
                if (task.Status == EnumTaskStatus.完成.ToShort())
                {
                    operateStatus.Msg = string.Format(ResourceWorkflowEngine.任务已处理,
                         task.DoUserName);
                    return operateStatus;
                }
                #endregion

                task.ApproveUserId = input.DoUserId;
                task.ApproveUserName = input.DoUserName;
                task.ApproveTime = DateTime.Now;
                task.ApproveStatus = 1;
                task.ApproveComment = input.Comment;
                task.Status = EnumTaskStatus.完成.ToShort();
                task.Remark = input.DoUserName + "|" + DateTime.Now + "|审核完成";
                if (await fixture.Db.WorkflowProcessInstanceTask.UpdateAsync(task))
                {
                    //插入任务
                    task.TaskId = CombUtil.NewComb();
                    task.PrevTaskId = input.TaskId;
                    task.DoUserId = null;
                    task.DoUserName = null;
                    task.Comment = null;
                    task.CompletedTime = null;
                    task.ApproveUserId = null;
                    task.ApproveComment = null;
                    task.ApproveStatus = null;
                    task.ApproveTime = null;
                    task.ApproveUserName = null;
                    task.Remark = null;
                    task.Status = EnumTaskStatus.正在处理.ToShort();
                    await fixture.Db.WorkflowProcessInstanceTask.InsertAsync(task);

                    operateStatus.Msg = "拒绝成功";
                    operateStatus.Code = ResultCode.Success;
                }
                return operateStatus;
            }
        }

        /// <summary>
        /// 批阅后审核通过
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> InvitationReadProcessApprovePass(WorkflowEngineInvitationReadApproveInput input)
        {
            using (var fixture = new SqlDatabaseFixture())
            {
                OperateStatus operateStatus = new OperateStatus();

                #region 验证
                var time = DateTime.Now;
                WorkflowProcessInstanceTask task = await fixture.Db.WorkflowProcessInstanceTask.FindAsync(f => f.TaskId == input.TaskId);
                if (task == null)
                {
                    operateStatus.Msg = ResourceWorkflowEngine.未找到流程活动信息;
                    return operateStatus;
                }

                //判断当前任务状态是否为完成
                if (task.Status == EnumTaskStatus.完成.ToShort())
                {
                    operateStatus.Msg = string.Format(ResourceWorkflowEngine.任务已处理,
                         task.DoUserName);
                    return operateStatus;
                }
                #endregion

                task.ApproveUserId = input.DoUserId;
                task.ApproveUserName = input.DoUserName;
                task.ApproveTime = DateTime.Now;
                task.ApproveStatus = 0;
                task.ApproveComment = input.Comment;
                task.Status = EnumTaskStatus.完成.ToShort();
                task.Remark = input.DoUserName + "|" + DateTime.Now + "|审核完成";
                if (await fixture.Db.WorkflowProcessInstanceTask.UpdateAsync(task))
                {
                    //判断当前节点是否都完成
                    var allTask = await fixture.Db.WorkflowProcessInstanceTask.FindAllAsync(f => f.ProcessInstanceId == task.ProcessInstanceId && (f.Status == EnumTaskStatus.正在处理.ToShort() || f.Status == EnumTaskStatus.阅示已阅待审核.ToShort()));
                    if (!allTask.Any())
                    {
                        //进行下一步
                        //下一节点
                        var instanceLinks = await fixture.Db.WorkflowProcessInstanceLink.SetSelect(s => new { s.End }).FindAllAsync(f => f.Begin == task.ActivityId && f.ProcessInstanceId == task.ProcessInstanceId);
                        if (instanceLinks.Any())
                        {
                            var linksTo = instanceLinks.Select(s => s.End).ToList();
                            var activitys = await fixture.Db.WorkflowProcessInstanceActivity.SetSelect(s => new { s.ActivityId, s.Type }).FindAllAsync(f => linksTo.Contains(f.ActivityId) && f.ProcessInstanceId == task.ProcessInstanceId);

                            if (activitys.Any())
                            {
                                //是否具有结束
                                if (activitys.Any(a => a.Type == "end"))
                                {
                                    var endAct = activitys.FirstOrDefault(w => w.Type == EnumAcitvityType.结束.FindDescription());

                                    var endTask = new WorkflowProcessInstanceTask
                                    {
                                        TaskId = CombUtil.NewComb(),
                                        ProcessInstanceId = task.ProcessInstanceId,
                                        ActivityId = endAct.ActivityId,
                                        ActivityType = ConvertIndexByDes(endAct.Type),
                                        SendUserId = input.DoUserId,
                                        SendUserName = input.DoUserName,
                                        DoUserId = input.DoUserId,
                                        DoUserName = input.DoUserName,
                                        ReceiveUserId = input.DoUserId,
                                        ReceiveUserName = input.DoUserName,
                                        ReceiveTime = time,
                                        CompletedTime = time,
                                        CustomData = task.CustomData,
                                        Status = EnumTaskStatus.完成.ToShort(),
                                        Remark = input.DoUserName + "|" + time.ToYyyyMMddHHmmss() + "|提交",
                                        PrevTaskId = input.TaskId
                                    };
                                    await fixture.Db.WorkflowProcessInstanceTask.InsertAsync(endTask);

                                    await EndProcess(new WorkflowEngineEndProcessInput
                                    {
                                        TaskId = task.TaskId,
                                        DoUserId = input.DoUserId,
                                        DoUserName = input.DoUserName
                                    });
                                }
                                else
                                {
                                    //循环执行剩余节点
                                }
                            }
                        }
                    }

                    operateStatus.Msg = ResourceWorkflowEngine.阅示已阅待审核成功;
                    operateStatus.Code = ResultCode.Success;
                }
                return operateStatus;
            }
        }

        /// <summary>
        /// 根据实例Id获取活动信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<WorkflowEngineFindMonitorDetailOutput>> FindMonitorDetail(IdInput input)
        {
            return OperateStatus<WorkflowEngineFindMonitorDetailOutput>.Success(await _processInstanceRepository.FindMonitorDetial(input));
        }
        #endregion

        #region 草稿箱
        /// <summary>
        /// 保存到草稿箱
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> SaveDraft(WorkflowEngineStartProcessInput input)
        {
            OperateStatus operateStatus = new OperateStatus();
            using (var fixture = new SqlDatabaseFixture())
            {
                //判断实例是否存在
                var instanceCheck = await fixture.Db.WorkflowProcessInstance.FindAsync(f => f.ProcessInstanceId == input.ProcessInstanceId && f.ProcessId == input.ProcessId);
                if (instanceCheck == null)
                {
                    WorkflowEngineProcessInstanceOutput instance = await FindWorkflowEngineProcessInstance(fixture, input);
                    instance.Instance.Type = EnumProcessInstanceType.草稿.ToShort();
                    var startActivity = instance.InstanceActivities.ToList().FirstOrDefault(w => w.Type == EnumAcitvityType.开始.FindDescription());
                    if (startActivity == null)
                    {
                        operateStatus.Msg = ResourceWorkflowEngine.流程配置错误无开始节点;
                        return operateStatus;
                    }
                    var trans = fixture.Db.BeginTransaction();
                    try
                    {
                        await fixture.Db.WorkflowProcessInstance.InsertAsync(instance.Instance, trans);
                        operateStatus.Code = ResultCode.Success;
                        operateStatus.Msg = Chs.Successful;
                        trans.Commit();
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        operateStatus.Code = ResultCode.Error;
                        operateStatus.Msg = e.Message;
                    }
                }
                else
                {
                    instanceCheck.Title = input.Title;
                    instanceCheck.Urgency = input.Urgency.ToShort();
                    instanceCheck.UpdateTime = DateTime.Now;
                    instanceCheck.UpdateUserId = input.CreateUserId;
                    instanceCheck.UpdateUserName = input.CreateUserName;
                    instanceCheck.UpdateUserOrganizationId = input.CreateUserOrganizationId;
                    instanceCheck.UpdateUserOrganizationName = input.CreateUserOrganizationName;
                    instanceCheck.Type = EnumProcessInstanceType.草稿.ToShort();
                    var trans = fixture.Db.BeginTransaction();
                    try
                    {
                        var result = await fixture.Db.WorkflowProcessInstance.UpdateAsync(instanceCheck, trans);
                        if (result)
                        {
                            trans.Commit();
                            operateStatus.Code = ResultCode.Success;
                            operateStatus.Msg = Chs.Successful;
                        }
                        else
                        {
                            operateStatus.Msg = ResourceWorkflowEngine.保存到草稿箱失败;
                        }
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        operateStatus.Msg = e.Message;
                    }
                }
                return operateStatus;
            }
        }

        /// <summary>
        /// 分页查询草稿箱
        /// </summary>
        /// <param name="paging">分页参数</param>
        /// <returns></returns>
        public async Task<OperateStatus<PagedResults<WorkflowEngineFindDraftOutput>>> FindDraft(WorkflowEngineFindDraftInput paging)
        {
            return OperateStatus<PagedResults<WorkflowEngineFindDraftOutput>>.Success(await _processInstanceRepository.FindDraft(paging));
        }

        /// <summary>
        /// 删除草稿
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public async Task<OperateStatus<IList<WorkflowEngineFormDeleteDataInput>>> DeleteDraft(IdInput<string> inputs)
        {
            OperateStatus<IList<WorkflowEngineFormDeleteDataInput>> operateStatus = new OperateStatus<IList<WorkflowEngineFormDeleteDataInput>>();
            using (var fixture = new SqlDatabaseFixture())
            {
                IList<WorkflowEngineFormDeleteDataInput> output = new List<WorkflowEngineFormDeleteDataInput>();
                var trans = fixture.Db.BeginTransaction();
                try
                {
                    foreach (var input in inputs.Id.Split(','))
                    {
                        //查询流程实例
                        var instance = await fixture.Db.WorkflowProcessInstance.FindAsync(f => f.ProcessInstanceId == Guid.Parse(input), trans);
                        if (instance != null)
                        {
                            var form = await fixture.Db.WorkflowProcess.FindAsync(f => f.ProcessId == instance.ProcessId, trans);
                            if (form != null)
                            {
                                output.Add(new WorkflowEngineFormDeleteDataInput
                                {
                                    FormId = (Guid)form.FormId,
                                    ProcessInstanceId = instance.ProcessInstanceId
                                });
                            }
                            //删除
                            await fixture.Db.WorkflowProcessInstance.DeleteAsync(d => d.ProcessInstanceId == Guid.Parse(input), trans);
                            await fixture.Db.WorkflowProcessInstanceActivity.DeleteAsync(d => d.ProcessInstanceId == Guid.Parse(input), trans);
                            await fixture.Db.WorkflowProcessInstanceLink.DeleteAsync(d => d.ProcessInstanceId == Guid.Parse(input), trans);
                        }
                    }
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    operateStatus.Msg = e.Message;
                    return operateStatus;
                }
                trans.Commit();
                operateStatus.Code = ResultCode.Success;
                operateStatus.Msg = Chs.Successful;
                operateStatus.Data = output;

                if (operateStatus.Code == ResultCode.Success)
                {
                    await DeleteFormData(operateStatus.Data);
                }
            }
            return operateStatus;
        }
        #endregion

        #region 范本夹
        /// <summary>
        /// 保存到范本夹
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> SaveModel(WorkflowEngineStartProcessInput input)
        {
            OperateStatus operateStatus = new OperateStatus();
            using (var fixture = new SqlDatabaseFixture())
            {
                //判断实例是否存在
                var instanceCheck = (await fixture.Db.WorkflowProcessInstance.FindAllAsync(f => f.ProcessInstanceId == input.ProcessInstanceId && f.ProcessId == input.ProcessId)).ToList();
                if (instanceCheck.Count == 0)
                {
                    WorkflowEngineProcessInstanceOutput instance = await FindWorkflowEngineProcessInstance(fixture, input);
                    instance.Instance.Type = EnumProcessInstanceType.范本.ToShort();
                    var startActivity = instance.InstanceActivities.ToList().FirstOrDefault(w => w.Type == EnumAcitvityType.开始.FindDescription());
                    if (startActivity == null)
                    {
                        operateStatus.Msg = ResourceWorkflowEngine.流程配置错误无开始节点;
                        return operateStatus;
                    }
                    var trans = fixture.Db.BeginTransaction();
                    try
                    {
                        await fixture.Db.WorkflowProcessInstance.InsertAsync(instance.Instance, trans);
                        operateStatus.Code = ResultCode.Success;
                        operateStatus.Msg = Chs.Successful;
                        trans.Commit();
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        operateStatus.Code = ResultCode.Error;
                        operateStatus.Msg = e.Message;
                    }
                }
                else
                {
                    var instance = instanceCheck.FirstOrDefault();
                    if (instance != null)
                    {
                        instance.Title = input.Title;
                        instance.Urgency = input.Urgency.ToShort();
                        instance.UpdateTime = DateTime.Now;
                        instance.UpdateUserId = input.CreateUserId;
                        instance.UpdateUserName = input.CreateUserName;
                        instance.UpdateUserOrganizationId = input.CreateUserOrganizationId;
                        instance.UpdateUserOrganizationName = input.CreateUserOrganizationName;
                        instance.Type = EnumProcessInstanceType.范本.ToShort();
                        var trans = fixture.Db.BeginTransaction();
                        try
                        {
                            var result = await fixture.Db.WorkflowProcessInstance.UpdateAsync(instance, trans);
                            if (result)
                            {
                                trans.Commit();
                                operateStatus.Code = ResultCode.Success;
                                operateStatus.Msg = Chs.Successful;
                            }
                            else
                            {
                                operateStatus.Msg = ResourceWorkflowEngine.保存到范本夹失败;
                            }
                        }
                        catch (Exception e)
                        {
                            trans.Rollback();
                            operateStatus.Msg = e.Message;
                        }
                    }
                }
                return operateStatus;
            }
        }

        /// <summary>
        /// 分页查询范本夹
        /// </summary>
        /// <param name="paging">分页参数</param>
        /// <returns></returns>
        public async Task<OperateStatus<PagedResults<WorkflowEngineFindModelOutput>>> FindModel(WorkflowEngineFindModelInput paging)
        {
            return OperateStatus<PagedResults<WorkflowEngineFindModelOutput>>.Success(await _processInstanceRepository.FindModel(paging));
        }
        /// <summary>
        /// 删除范本夹
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public async Task<OperateStatus<IList<WorkflowEngineFormDeleteDataInput>>> DeleteModel(IdInput<string> inputs)
        {
            OperateStatus<IList<WorkflowEngineFormDeleteDataInput>> operateStatus = new OperateStatus<IList<WorkflowEngineFormDeleteDataInput>>();
            using (var fixture = new SqlDatabaseFixture())
            {
                IList<WorkflowEngineFormDeleteDataInput> output = new List<WorkflowEngineFormDeleteDataInput>();
                var trans = fixture.Db.BeginTransaction();
                try
                {
                    foreach (var input in inputs.Id.Split(','))
                    {
                        //查询流程实例
                        var instance = await fixture.Db.WorkflowProcessInstance.FindAsync(f => f.ProcessInstanceId == Guid.Parse(input), trans);
                        if (instance != null)
                        {
                            var form = await fixture.Db.WorkflowProcess.FindAsync(f => f.ProcessId == instance.ProcessId, trans);
                            if (form != null)
                            {
                                output.Add(new WorkflowEngineFormDeleteDataInput
                                {
                                    FormId = (Guid)form.FormId,
                                    ProcessInstanceId = instance.ProcessInstanceId
                                });
                            }
                            //删除
                            await fixture.Db.WorkflowProcessInstance.DeleteAsync(d => d.ProcessInstanceId == Guid.Parse(input), trans);
                            await fixture.Db.WorkflowProcessInstanceActivity.DeleteAsync(d => d.ProcessInstanceId == Guid.Parse(input), trans);
                            await fixture.Db.WorkflowProcessInstanceLink.DeleteAsync(d => d.ProcessInstanceId == Guid.Parse(input), trans);
                        }
                    }
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    operateStatus.Msg = e.Message;
                    return operateStatus;
                }
                trans.Commit();
                operateStatus.Code = ResultCode.Success;
                operateStatus.Msg = Chs.Successful;
                operateStatus.Data = output;

                if (operateStatus.Code == ResultCode.Success)
                {
                    await DeleteFormData(operateStatus.Data);
                }
            }
            return operateStatus;
        }
        #endregion


        #region 流程辅助推算
        /// <summary>
        /// 保存实例开始任务
        /// </summary>
        /// <returns></returns>
        private async Task<OperateStatus<WorkflowProcessInstanceTask>> SaveInstanceStartTask(WorkflowEngineSaveInstanceStartTaskInput input, SqlDatabaseFixture fixture)
        {
            OperateStatus<WorkflowProcessInstanceTask> operateStatus = new OperateStatus<WorkflowProcessInstanceTask>();
            try
            {
                WorkflowProcessInstanceTask task = new WorkflowProcessInstanceTask
                {
                    TaskId = CombUtil.NewComb(),
                    ProcessInstanceId = input.ProcessInstanceId,
                    ActivityId = input.ActivityId,
                    ActivityType = EnumAcitvityType.开始.ToShort(),
                    SendUserId = input.CreateUserId,
                    SendUserName = input.CreateUserName,
                    DoUserId = input.CreateUserId,
                    DoUserName = input.CreateUserName,
                    ReceiveUserId = input.CreateUserId,
                    ReceiveUserName = input.CreateUserName,
                    ReceiveTime = input.CreateDateTime,
                    CustomData = input.CustomData,
                    CompletedTime = input.CreateDateTime,
                    Status = EnumTaskStatus.完成.ToShort(),
                    Remark = input.CreateUserName + $"|" + input.CreateDateTime.ToYyyyMMddHHmmss() + "|提交"
                };
                await fixture.Db.WorkflowProcessInstanceTask.InsertAsync(task);
                operateStatus.Code = ResultCode.Success;
                operateStatus.Msg = Chs.Successful;
                operateStatus.Data = task;
            }
            catch (Exception ex)
            {
                operateStatus.Msg = ex.Message;
            }
            return operateStatus;
        }

        /// <summary>
        /// 更新状态备注
        /// </summary>
        /// <param name="input">审批人员</param>
        /// <returns></returns>
        private async Task<OperateStatus<string>> UpdateInstanceStatusRemark(WorkflowEngineSaveInstanceTaskInput input, SqlDatabaseFixture fixture)
        {
            OperateStatus<string> operateStatus = new OperateStatus<string>()
            {
                Data = ""
            };
            StringBuilder statusRemark = new StringBuilder();
            StringBuilder personsStringBuilder = new StringBuilder();
            if (input.Persons.Any())
            {
                foreach (var persionsActivity in input.Persons)
                {
                    foreach (var per in persionsActivity.Persons)
                    {
                        personsStringBuilder.Append($"{per.Name},");
                    }

                    var activityObj = GetProcessinstanceActivity(persionsActivity.Activity.ProcessInstanceId, persionsActivity.Activity.ActivityId).JsonStringToObject<WorkflowEngineFindMonitorFlowStatesOutput>();

                    statusRemark.Append(activityObj.text.text + ":" + personsStringBuilder.ToString().TrimEnd(',') + ",");
                }
                WorkflowEngineUpdateInstanceStatusRemarkInput statusRemarkInput = new WorkflowEngineUpdateInstanceStatusRemarkInput
                {
                    StatusRemark = statusRemark.ToString().TrimEnd(','),
                    ProcessInstanceId = input.Persons[0].Activity.ProcessInstanceId
                };

                var result = await fixture.Db.WorkflowProcessInstance.UpdateAsync(u => u.ProcessInstanceId == input.ProcessInstanceId, new { Status = 0, StatusRemark = statusRemarkInput.StatusRemark });
                if (result)
                {
                    operateStatus.Data = statusRemarkInput.StatusRemark;
                }
            }
            operateStatus.Code = ResultCode.Success;
            operateStatus.Msg = Chs.Successful;
            return operateStatus;
        }

        /// <summary>
        /// 保存实例任务
        /// 需要将处理人和基础信息传入
        /// </summary>
        /// <returns></returns>
        private async Task SaveInstanceTask(WorkflowEngineSaveInstanceTaskInput input, SqlDatabaseFixture fixture)
        {
            OperateStatus operateStatus = new OperateStatus();
            try
            {
                StringBuilder domessage = new StringBuilder();
                IList<WorkflowProcessInstanceTask> tasks = new List<WorkflowProcessInstanceTask>();
                foreach (var item in input.Persons)
                {
                    //判断下一步活动类型,是否为子流程
                    if (item.Activity.Type == EnumAcitvityType.子流程.FindDescription())
                    {
                        operateStatus = await StartSubProcess(fixture, new WorkflowEngineStatrSubProcessInput()
                        {
                            Persons = item.Persons,
                            Activity = item.Activity,
                            InstanceTask = input
                        });
                        if (operateStatus.Code == ResultCode.Error)
                        {
                            return;
                        }
                    }
                    else
                    {
                        foreach (var per in item.Persons)
                        {
                            //var overTime = GetWorkflowEngineOverTime(item.Activity);
                            var task = new WorkflowProcessInstanceTask
                            {
                                TaskId = CombUtil.NewComb(),
                                ProcessInstanceId = input.ProcessInstanceId,
                                ActivityId = item.Activity.ActivityId,
                                ActivityType = EnumAcitvityType.审批.ToShort(),
                                SendUserId = input.CreateUserId,
                                SendUserName = input.CreateUserName,
                                ReceiveUserId = per.UserId,
                                ReceiveUserName = per.Name,
                                ReceiveTime = input.CreateDateTime,
                                CustomData = input.CustomData,
                                Status = EnumActivityStatus.正在处理.ToShort(),
                                Remark = per.Name + "|" + input.CreateDateTime.ToYyyyMMddHHmmss() + "|处理中",
                                PrevTaskId = input.PrevTaskId
                            };
                            tasks.Add(task);
                            domessage.Append($"{per.Name},");
                        }
                    }
                }
                if (tasks.Any())
                {
                    await fixture.Db.WorkflowProcessInstanceTask.BulkInsertAsync(tasks);
                }

                //写入缓存
                var instanceCacheKey = string.Format(ResouceWorkflowCacheKey.流程实例任务, input.ProcessInstanceId);
                var taskAll = await fixture.Db.WorkflowProcessInstanceTask.FindAllAsync(f => f.ProcessInstanceId == input.ProcessInstanceId);
                RedisHelper.SAdd(instanceCacheKey, tasks);
                operateStatus.Code = ResultCode.Success;
                operateStatus.Msg = Chs.Successful;
            }
            catch (Exception ex)
            {
                operateStatus.Msg = ex.Message;
            }
        }

        /// <summary>
        /// 启动子流程
        /// </summary>
        /// <returns></returns>
        private async Task<OperateStatus> StartSubProcess(SqlDatabaseFixture fixture, WorkflowEngineStatrSubProcessInput input)
        {
            OperateStatus operateStatus = new OperateStatus();
            //if (input.Activity.SubProcessProcessId.IsNullOrEmptyGuid())
            //{
            //    operateStatus.Msg = ResourceWorkflowEngine.未找到子流程;
            //    return operateStatus;
            //}
            ////获取子流程相关信息
            //var subProcessActivity = (await fixture.Db.WorkflowProcessActivity.FindAllAsync(f => f.ProcessId == input.Activity.SubProcessProcessId)).FirstOrDefault(f => f.Type == EnumAcitvityType.开始.FindDescription());
            //if (subProcessActivity == null)
            //{
            //    operateStatus.Msg = ResourceWorkflowEngine.流程配置错误无开始节点;
            //    return operateStatus;
            //}
            //var subProcess = await fixture.Db.WorkflowProcess.FindAsync(input.Activity.SubProcessProcessId);
            //if (subProcess.FormId.IsNullOrEmptyGuid() && subProcessActivity.FormId.IsNullOrEmptyGuid())
            //{
            //    operateStatus.Msg = ResourceWorkflowEngine.无发起表单;
            //    return operateStatus;
            //}
            //WorkflowEngineStartProcessInput startProcessInput = new WorkflowEngineStartProcessInput();
            //if (input.Activity.SubProcessProcessId != null)
            //    startProcessInput.ProcessId = (Guid)input.Activity.SubProcessProcessId;
            //startProcessInput.ProcessInstanceId = CombUtil.NewComb();
            //startProcessInput.Title = subProcess.Name;
            //startProcessInput.Urgency = EnumProcessInstanceUrgency.正常;
            //startProcessInput.ActivityId = subProcessActivity.ActivityId;
            //startProcessInput.Type = EnumAcitvityType.开始;
            //startProcessInput.CreateUserId = input.InstanceTask.CreateUserId;
            //startProcessInput.CreateUserCode = input.InstanceTask.CreateUserCode;
            //startProcessInput.CreateUserName = input.InstanceTask.CreateUserName;
            //startProcessInput.CreateUserOrganizationId = input.InstanceTask.CreateUserOrganizationId;
            //startProcessInput.CreateUserOrganizationName = input.InstanceTask.CreateUserOrganizationName;
            ////自动结束发起步骤
            //if (input.Activity.SubProcessAutoendStart)
            //{
            //    //得到下一步流程数据
            //    var starProcessResult = await StartProcess(startProcessInput);
            //    WorkflowEngineStartProcessRunInput startProcessRunInput = new WorkflowEngineStartProcessRunInput();
            //    startProcessRunInput.ActivityId = subProcessActivity.ActivityId;
            //    startProcessRunInput.Type = EnumAcitvityType.开始;
            //    startProcessRunInput.ProcessId = startProcessInput.ProcessId;
            //    startProcessRunInput.ProcessInstanceId = startProcessInput.ProcessInstanceId;
            //    startProcessRunInput.Title = startProcessInput.Title;
            //    startProcessRunInput.Urgency = EnumProcessInstanceUrgency.正常;
            //    startProcessRunInput.CreateUserId = startProcessInput.CreateUserId;
            //    startProcessRunInput.CreateUserCode = startProcessInput.CreateUserCode;
            //    startProcessRunInput.CreateUserName = startProcessInput.CreateUserName;
            //    startProcessRunInput.CreateUserOrganizationId = startProcessInput.CreateUserOrganizationId;
            //    startProcessRunInput.CreateUserOrganizationName = startProcessInput.CreateUserOrganizationName;
            //    startProcessRunInput.Activities = starProcessResult.Data.Activities;
            //    startProcessRunInput.Links = starProcessResult.Data.Links;
            //    startProcessRunInput.NextTask = starProcessResult.Data.Person;
            //    var startProcessRunResult = await StartProcessRun(startProcessRunInput);
            //    operateStatus.Code = ResultCode.Success;
            //    operateStatus.Msg = startProcessRunResult.Msg;
            //}
            //else
            //{
            //    WorkflowEngineStartSubProcessRunInput startProcessRunInput = new WorkflowEngineStartSubProcessRunInput();
            //    startProcessRunInput.ActivityId = subProcessActivity.ActivityId;
            //    startProcessRunInput.Type = EnumAcitvityType.开始;
            //    startProcessRunInput.ProcessId = startProcessInput.ProcessId;
            //    startProcessRunInput.ProcessInstanceId = startProcessInput.ProcessInstanceId;
            //    startProcessRunInput.Title = startProcessInput.Title;
            //    startProcessRunInput.PrevTaskId = input.InstanceTask.TaskId;
            //    startProcessRunInput.Urgency = EnumProcessInstanceUrgency.正常;
            //    startProcessRunInput.CreateUserId = startProcessInput.CreateUserId;
            //    startProcessRunInput.CreateUserCode = startProcessInput.CreateUserCode;
            //    startProcessRunInput.CreateUserName = startProcessInput.CreateUserName;
            //    startProcessRunInput.CreateUserOrganizationId = startProcessInput.CreateUserOrganizationId;
            //    startProcessRunInput.CreateUserOrganizationName = startProcessInput.CreateUserOrganizationName;
            //    var activity = subProcessActivity.MapTo<WorkflowProcessInstanceActivity>();
            //    activity.ProcessInstanceId = startProcessRunInput.ProcessInstanceId;
            //    var personDetailOutputs = new List<WorkflowEngineProcessingPersonDetailOutput>();
            //    startProcessRunInput.NextTask = new List<WorkflowEngineProcessingPersonOutput>
            //    {
            //      new WorkflowEngineProcessingPersonOutput
            //      {
            //          Activity = activity,
            //          //Persons = new List<WorkflowEngineProcessingPersonDetailOutput>
            //          //{
            //          //    new WorkflowEngineProcessingPersonDetailOutput
            //          //    {
            //          //        UserId=startProcessInput.CreateUserId,
            //          //        Code=startProcessRunInput.CreateUserCode,
            //          //        Name=startProcessRunInput.CreateUserName,
            //          //        Pattern = "radio",
            //          //        PatternName = startProcessRunInput.CreateUserName,
            //          //        UserTypeSelect=4
            //          //    }
            //          //}
            //          Persons=input.Persons
            //      }
            //    };
            //    var startProcessRunResult = await StartSubProcessProcessRun(startProcessRunInput);
            //    operateStatus.Code = ResultCode.Success;
            //    operateStatus.Msg = startProcessRunResult.Msg;
            //}
            ////查看是否具有下发数据
            //if (input.Activity.SubProcessInput.IsNotNullOrEmpty())
            //{
            //    IList<WorkflowEngineFormControls> controlses = new List<WorkflowEngineFormControls>();
            //    //序列化得到值
            //    var dataDto = input.Activity.SubProcessInput.JsonStringToList<WorkflowEngineSubProcessDataDto>();
            //    dataDto.ForEach(e => controlses.Add(new WorkflowEngineFormControls { ColumnName = e.Id, Type = e.Type }));
            //    var ta = await fixture.Db.WorkflowProcessInstanceTask.FindAsync(input.InstanceTask.TaskId);
            //    var a = (await fixture.Db.WorkflowProcessInstanceActivity.FindAllAsync(f => f.ActivityId == ta.ActivityId && f.ProcessInstanceId == ta.ProcessInstanceId)).ToList();
            //    //得到实例数据
            //    Guid inFormId = Guid.Empty;
            //    var first = a.FirstOrDefault();
            //    if (first?.FormId != null)
            //        inFormId = (Guid)first.FormId;
            //    var inputResult = await InitFormData(new WorkflowEngineFormInitDataInput
            //    {
            //        ActivityId = startProcessInput.ActivityId,
            //        ActivityType = startProcessInput.Type.FindDescription(),
            //        FormId = inFormId,
            //        OrganizationId = startProcessInput.CreateUserOrganizationId,
            //        OrganizationName = startProcessInput.CreateUserOrganizationName,
            //        ProcessInstanceId = input.Activity.ProcessInstanceId,
            //        StartProcessInstanceId = startProcessInput.ProcessInstanceId,
            //        Controls = controlses
            //    });
            //    //将得到数据插入
            //    IList<WorkflowEngineFormControls> insertControlses = new List<WorkflowEngineFormControls>();
            //    foreach (var data in dataDto)
            //    {
            //        var res = inputResult.FirstOrDefault(w => w.Name == data.Id);
            //        if (res == null) continue;
            //        insertControlses.Add(new WorkflowEngineFormControls { ColumnName = data.ColumnName, Type = "input", Value = res.Value });
            //    }
            //    var formId = subProcessActivity.FormId.IsNullOrEmptyGuid() ? (Guid)subProcess.FormId : (Guid)subProcessActivity.FormId;
            //    await SaveFormData(new WorkflowEngineFormSaveDataInput()
            //    {
            //        UserId = startProcessInput.CreateUserId,
            //        UserCode = startProcessInput.CreateUserCode,
            //        UserName = startProcessInput.CreateUserName,
            //        ActivityType = startProcessInput.Type.FindDescription(),
            //        FormId = formId,
            //        OrganizationId = startProcessInput.CreateUserOrganizationId,
            //        OrganizationName = startProcessInput.CreateUserOrganizationName,
            //        ProcessInstanceId = startProcessInput.ProcessInstanceId,
            //        Controls = insertControlses
            //    });
            //}
            //var dateTime = DateTime.Now;
            //var tasks = new List<WorkflowProcessInstanceTask>();
            //foreach (var per in input.Persons)
            //{
            //    var instanceTask = new WorkflowProcessInstanceTask
            //    {
            //        TaskId = CombUtil.NewComb(),
            //        ProcessInstanceId = input.Activity.ProcessInstanceId,
            //        ActivityId = input.Activity.ActivityId,
            //        ActivityType = EnumAcitvityType.子流程.ToShort(),
            //        SendUserId = startProcessInput.CreateUserId,
            //        SendUserName = startProcessInput.CreateUserName,
            //        ReceiveUserId = per.UserId,
            //        ReceiveUserName = per.Name,
            //        ReceiveTime = dateTime,
            //        Status = EnumActivityStatus.正在处理.ToShort(),
            //        Remark = per.Name + "|" + dateTime.ToYyyyMMddHHmmss() + "|处理中",
            //        PrevTaskId = input.InstanceTask.TaskId
            //    };
            //    //若不需要该步骤等待
            //    if (!input.Activity.SubProcessAsync)
            //    {
            //        instanceTask.Status = EnumActivityStatus.完成.ToShort();
            //        instanceTask.Remark = per.Name + "|" + dateTime.ToYyyyMMddHHmmss() + "|完成";
            //        instanceTask.DoUserCode = startProcessInput.CreateUserCode;
            //        instanceTask.DoUserId = startProcessInput.CreateUserId;
            //        instanceTask.DoUserName = startProcessInput.CreateUserName;
            //        instanceTask.CompletedTime = dateTime;
            //    }
            //    tasks.Add(instanceTask);
            //}
            //if (tasks.Any())
            //{
            //    await fixture.Db.WorkflowProcessInstanceTask.BulkInsertAsync(tasks);
            //    var processInstance = await fixture.Db.WorkflowProcessInstance.FindAsync(startProcessInput.ProcessInstanceId);
            //    processInstance.FromTaskId = tasks[0].TaskId;
            //    await fixture.Db.WorkflowProcessInstance.UpdateAsync(processInstance);

            //    await UpdateFormStatus(new WorkflowEngineUpdateFormStatusInput
            //    {
            //        ProcessInstanceId = startProcessInput.ProcessInstanceId,
            //        Status = EnumProcessInstanceStatus.处理中
            //    });
            //}
            return operateStatus;
        }

        /// <summary>
        /// 更新通过状态
        /// </summary>
        /// <returns></returns>
        private async Task UpdateIsPass(WorkflowEngineUpdateIsPassInput input, SqlDatabaseFixture fixture)
        {
            if (input.Activities.Any())
            {
                var activityId = input.Activities;
                await fixture.Db.WorkflowProcessInstanceActivity.UpdateAsync(u => activityId.Contains(u.ActivityId), new { IsPass = 1 });
            }

            if (input.Links.Any())
            {
                var links = input.Links;
                await fixture.Db.WorkflowProcessInstanceLink.UpdateAsync(u => links.Contains(u.LinkId), new { IsPass = 1 });
            }
        }

        /// <summary>
        /// 获取所有开始运行实例数据
        /// </summary>
        /// <param name="fixture"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<WorkflowEngineProcessInstanceOutput> FindWorkflowEngineProcessInstance(
            SqlDatabaseFixture fixture,
            WorkflowEngineStartProcessInput input)
        {
            WorkflowEngineProcessInstanceOutput instanceReturn = new WorkflowEngineProcessInstanceOutput();
            //检测是否已存在
            await fixture.Db.WorkflowProcessInstance.DeleteAsync(d => d.ProcessInstanceId == input.ProcessInstanceId);

            //缓存获取
            var cacheKey = string.Format(ResouceWorkflowCacheKey.流程配置, input.ProcessId);
            var processCache = await RedisHelper.CacheShellAsync(cacheKey, DateTimeUtil.TotalSeconds(30), async () =>
            {
                WorkflowEngineProcessOutput workflowEngineProcessOutput = new WorkflowEngineProcessOutput();
                workflowEngineProcessOutput.Process = await fixture.Db.WorkflowProcess.FindAsync(f => f.ProcessId == input.ProcessId);
                workflowEngineProcessOutput.ProcessActivities = await fixture.Db.WorkflowProcessActivity.FindAllAsync(w => w.ProcessId == input.ProcessId);
                workflowEngineProcessOutput.ProcessLinks = await fixture.Db.WorkflowProcessLink.FindAllAsync(f => f.ProcessId == input.ProcessId);
                return workflowEngineProcessOutput;
            });
            if (input.Sn.IsNullOrEmpty())
            {
                //解析Sn规则
                if (processCache.Process.Sn.IsNotNullOrEmpty())
                {
                    //替换
                    processCache.Process.Sn = processCache.Process.Sn.ReplaceEditor();
                    var h1Elements = processCache.Process.Sn.GetNodes();
                    if (h1Elements != null)
                    {
                        foreach (var element in h1Elements)
                        {
                            //解码
                            var columnName = HttpUtility.UrlDecode(element.Id);
                            switch (columnName)
                            {
                                case "年":
                                    processCache.Process.Sn = processCache.Process.Sn.Replace(element.OuterHtml, DateTime.Now.ToString("yyyy"));
                                    break;
                                case "月":
                                    processCache.Process.Sn = processCache.Process.Sn.Replace(element.OuterHtml, DateTime.Now.ToString("MM"));
                                    break;
                                case "日":
                                    processCache.Process.Sn = processCache.Process.Sn.Replace(element.OuterHtml, DateTime.Now.ToString("dd"));
                                    break;
                                case "时":
                                    processCache.Process.Sn = processCache.Process.Sn.Replace(element.OuterHtml, DateTime.Now.ToString("HH"));
                                    break;
                                case "分":
                                    processCache.Process.Sn = processCache.Process.Sn.Replace(element.OuterHtml, DateTime.Now.ToString("mm"));
                                    break;
                                case "秒":
                                    processCache.Process.Sn = processCache.Process.Sn.Replace(element.OuterHtml, DateTime.Now.ToString("ss"));
                                    break;
                                case "毫秒":
                                    processCache.Process.Sn = processCache.Process.Sn.Replace(element.OuterHtml, DateTime.Now.ToString("fff"));
                                    break;
                                case "雪花唯一":
                                    processCache.Process.Sn = processCache.Process.Sn.Replace(element.OuterHtml, StringUitl.IdGen());
                                    break;
                                default:
                                    if (columnName.Contains("0"))
                                    {
                                        var count = await fixture.Db.WorkflowProcessInstance.CountAsync(f => f.ProcessId == input.ProcessId && f.CreateTime >= DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd")) && f.CreateTime < DateTime.Parse(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")));
                                        var length = columnName.Length;
                                        var snCount = (count + 1).ToString().PadLeft(length, '0');
                                        processCache.Process.Sn = processCache.Process.Sn.Replace(element.OuterHtml, snCount);
                                    }
                                    break;
                            }
                        }
                    }
                }
                else
                {
                    processCache.Process.Sn = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                }
                input.Sn = processCache.Process.Sn;
            }

            var instance = new WorkflowProcessInstance
            {
                ProcessInstanceId = input.ProcessInstanceId,
                ProcessId = input.ProcessId,
                Sn = input.Sn,
                Title = input.Title,
                Status = EnumProcessInstanceStatus.处理中.ToShort(),
                Type = EnumProcessInstanceType.正常.ToShort(),
                Urgency = input.Urgency.ToShort(),
                CreateTime = DateTime.Now,
                CreateUserId = input.CreateUserId,
                CreateUserName = input.CreateUserName,
                CreateUserOrganizationId = input.CreateUserOrganizationId,
                CreateUserOrganizationName = input.CreateUserOrganizationName,
                UpdateTime = DateTime.Now,
                UpdateUserId = input.CreateUserId,
                UpdateUserName = input.CreateUserName,
                UpdateUserOrganizationId = input.CreateUserOrganizationId,
                UpdateUserOrganizationName = input.CreateUserOrganizationName,
            };

            var allInstanceActivitys = processCache.ProcessActivities.MapToList<WorkflowProcessActivity, WorkflowProcessInstanceActivity>();
            foreach (var activity in allInstanceActivitys)
            {
                activity.ProcessInstanceId = input.ProcessInstanceId;
            }
            instanceReturn.InstanceActivities = allInstanceActivitys;
            var allInstanceLinks = processCache.ProcessLinks.MapToList<WorkflowProcessLink, WorkflowProcessInstanceLink>();
            foreach (var link in allInstanceLinks)
            {
                link.ProcessInstanceId = input.ProcessInstanceId;
            }
            instanceReturn.InstanceLinks = allInstanceLinks;
            instanceReturn.Instance = instance;
            instanceReturn.Activities = processCache.ProcessActivities.ToList();
            return instanceReturn;

        }

        /// <summary>
        /// 获取流程实例下面所有节点
        /// </summary>
        /// <param name="instance">当前流程实例</param>
        /// <param name="activityId">当前步骤</param>
        /// <param name="userId">当前处理人员</param>
        /// <returns></returns>
        private async Task<OperateStatus> FindProcessInstanceNextActivities(
                WorkflowEngineProcessInstanceOutput instance,
                Guid activityId,
                Guid userId)
        {
            _instanceActivities = new List<WorkflowProcessInstanceActivity>();
            _instanceLinks = new List<WorkflowProcessInstanceLink>();
            OperateStatus operateStatus = new OperateStatus();

            var instanceAcitvitys = FindNextInstanceAcitvitys(instance, activityId);
            //下一个步骤节点
            var nextActivitys = instanceAcitvitys.Activities.ToList();
            if (!nextActivitys.Any())
            {
                operateStatus.Msg = ResourceWorkflowEngine.未找到后续流程节点;
                return operateStatus;
            }
            foreach (var activity in nextActivitys)
            {
                foreach (var link in instanceAcitvitys.Links.Where(w => w.End == activity.ActivityId))
                {
                    _instanceLinks.Add(link);
                }
                //条件
                if (activity.Type == EnumAcitvityType.条件.FindDescription())
                {
                    var result = await FindProcessInstanceAcitvitysCondition(instance, activity, userId);
                    if (result.Code != ResultCode.Success)
                    {
                        operateStatus.Msg = result.Msg;
                        return operateStatus;
                    }
                }

                //合并
                else if (activity.Type == EnumAcitvityType.合并.FindDescription())
                {
                    var joinOperateStatus = await FindProcessInstanceAcitvitysJoin(instance, activity, userId);
                    //不走下步
                    if (!joinOperateStatus.Data)
                    {
                        _instanceActivities.Add(activity);
                        operateStatus.Code = ResultCode.Success;
                        operateStatus.Msg = Chs.Successful;
                        return operateStatus;
                    }
                }

                //结束
                else if (activity.Type == EnumAcitvityType.结束.FindDescription())
                {
                    _instanceActivities.Add(activity);
                    break;
                }

                //审批
                else if (activity.Type == EnumAcitvityType.审批.FindDescription())
                {
                    //判断当前审批人类型
                    _instanceActivities.Add(activity);
                }

                //子流程
                else if (activity.Type == EnumAcitvityType.子流程.FindDescription())
                {
                    //判断子流程是否需要等待返回
                    //if (!activity.SubProcessAsync)
                    //{
                    //    //查询下个步骤
                    //    await FindProcessInstanceNextActivities(instance, activity.ActivityId, userId);
                    //}
                    _instanceActivities.Add(activity);
                }
                //知会
                else if (activity.Type == EnumAcitvityType.知会.FindDescription())
                {
                    _instanceActivities.Add(activity);
                }
                //阅示
                else if (activity.Type == EnumAcitvityType.阅示.FindDescription())
                {
                    _instanceActivities.Add(activity);
                }
            }
            operateStatus.Code = ResultCode.Success;
            operateStatus.Msg = Chs.Successful;
            return operateStatus;
        }

        /// <summary>
        /// 获取流程下步活动通过策略
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="activityId"></param>
        /// <returns></returns>
        private OperateStatus FindProcessInstanceNextActivitiesApproveUserPassAgree(WorkflowEngineProcessInstanceOutput instance,
            Guid activityId)
        {
            OperateStatus operateStatus = new OperateStatus();
            //获取当前节点人员审批类型
            var nowAcitivity = instance.InstanceActivities.FirstOrDefault(f => f.ActivityId == activityId);
            if (nowAcitivity.Type == "begin")
            {
                operateStatus.Code = ResultCode.Success;
                return operateStatus;
            }
            //当前是否为开始节点
            if (nowAcitivity != null)
            {
                var taskAll = instance.InstanceTasks.Where(w => w.ActivityId == activityId && w.ActivityType == EnumAcitvityType.审批.ToShort() && w.Status <= EnumTaskStatus.完成.ToShort()).ToList();

                var nowActivityObj = GetProcessinstanceActivity(nowAcitivity.ProcessInstanceId, nowAcitivity.ActivityId).JsonStringToObject<WorkflowActivityDto>();

                switch (nowActivityObj.Props.User.Pass)
                {
                    case 0://一人同意即可通过
                        operateStatus.Code = ResultCode.Success;
                        return operateStatus;
                    case 1://所有人必须同意
                        if (!taskAll.Any(a => a.Status != EnumTaskStatus.完成.ToShort()))
                        {
                            operateStatus.Code = ResultCode.Success;
                            return operateStatus;
                        }
                        break;
                    case 2://通过百分比
                        int isPassNum = taskAll.Count(c => c.Status == EnumTaskStatus.完成.ToShort()), allNum = taskAll.Count();
                        //判断比例
                        if (isPassNum != 0)
                        {
                            var isPass = isPassNum / allNum * 100 >= nowActivityObj.Props.User.PassConfig;
                            if (isPass)
                            {
                                operateStatus.Code = ResultCode.Success;
                                return operateStatus;
                            }
                        }
                        break;
                    default:
                        operateStatus.Code = ResultCode.Success;
                        return operateStatus;
                }
            }
            return operateStatus;
        }

        /// <summary>
        /// 根据得到所有的后续所有活动得到对应活动下所有可处理人员
        /// </summary>
        /// <param name="instance">当前流程实例数据</param>
        /// <param name="userId">当前用户</param>
        /// <returns></returns>
        private async Task<OperateStatus<List<WorkflowEngineProcessingPersonOutput>>> FindProcessInstancePerson(
            WorkflowEngineProcessInstanceOutput instance,
            Guid userId)
        {
            OperateStatus<List<WorkflowEngineProcessingPersonOutput>> operateStatus = new OperateStatus<List<WorkflowEngineProcessingPersonOutput>>();
            //获取流程处理人
            var persons = await FindWorkflowEngineProcessingPersonOutput(_instanceActivities, userId, instance);
            //若无处理人
            if (!persons.Any())
            {
                //查看无处理人时处理情况:如是否跳过
                var doResult = await CheckNoProcessingPerson(instance, userId);
                if (doResult.Code == ResultCode.Error)
                {
                    operateStatus.Msg = doResult.Msg;
                    return operateStatus;
                }
            }
            operateStatus.Data = persons;
            operateStatus.Code = ResultCode.Success;
            operateStatus.Msg = Chs.Successful;
            return operateStatus;
        }
        /// <summary>
        /// 根据活动获取所有活动的流程处理人员
        /// </summary>
        /// <param name="activity">当前步骤</param>
        /// <param name="uId">当前用户</param>
        /// <param name="userJson"></param>
        /// <returns></returns>
        private async Task<List<WorkflowEngineProcessingPersonDetailOutput>> FindWorkflowEnginPersonOutput(
            WorkflowProcessInstanceActivity activity,
            Guid uId,
            string userJson)
        {
            using (var fixture = new SqlDatabaseFixture())
            {
                List<WorkflowEngineProcessingPersonOutput> processingPersonOutput = new List<WorkflowEngineProcessingPersonOutput>();

                List<WorkflowEngineProcessingPersonDetailOutput> persons = new List<WorkflowEngineProcessingPersonDetailOutput>();
                if (!userJson.IsNullOrEmpty())
                {
                    IList<WorkflowEngineActivityProcessingPersonOutput> personOutputs = userJson.JsonStringToList<WorkflowEngineActivityProcessingPersonOutput>();

                    int patternGroup = 1;
                    IList<string> approveUserName = new List<string>();
                    foreach (var output in personOutputs)
                    {
                        approveUserName.Add(output.ApproveUserName);
                        var pattern = output.ApproveUserTypeSelectPattern.ToLower();
                        var userTypeSelect = output.ApproveUserTypeSelect.ToShort();
                        var organizationRange = output.ApproveUserTypeSelectOrganizationRange;
                        var patternName = pattern == EnumApproveUserTypeSelectPattern.radio.ToString() ? patternGroup.ToString() : "";
                        IList<string> ids = new List<string>();
                        if (output.ApproveUserTypeSelectRangeId.IsNotNullOrEmpty())
                        {
                            ids = output.ApproveUserTypeSelectRangeId.JsonStringToList<ApproveUserTypeSelectRangeIdDto>().Select(s => s.Id).ToList();
                        }
                        switch (output.ApproveUserTypeSelect)
                        {
                            case EnumActivityProcessorType.所有人员://所有人员
                                var users = await fixture.Db.SystemUserInfo.SetSelect(s => new { s.UserId, s.Name }).FindAllAsync(f => !f.IsAdmin && !f.IsFreeze);
                                foreach (var item in users)
                                {
                                    persons.Add(new WorkflowEngineProcessingPersonDetailOutput
                                    {
                                        UserTypeSelect = userTypeSelect,
                                        Pattern = pattern,
                                        PatternName = patternName,
                                        UserId = item.UserId,
                                        Name = item.Name
                                    });
                                }
                                break;
                            case EnumActivityProcessorType.组织://组织
                                foreach (var id in ids)
                                {
                                    var gid = Guid.Parse(id);
                                    var orgs = (await fixture.Db.SystemPermissionUserFindByPrivilegeMasterAndValueOutput
                                        .FindAllAsync<SystemPermissionUserFindByPrivilegeMasterAndValueUsersOutput>(
                                            f => f.PrivilegeMaster == EnumPrivilegeMaster.组织架构.ToShort() &&
                                                 f.PrivilegeMasterValue == gid, o => o.Users)).ToList();
                                    if (!orgs.Any()) continue;
                                    foreach (var item in orgs[0].Users)
                                    {
                                        persons.Add(new WorkflowEngineProcessingPersonDetailOutput
                                        {
                                            UserTypeSelect = userTypeSelect,
                                            Pattern = pattern,
                                            PatternName = patternName,
                                            UserId = item.UserId,
                                            Name = item.Name
                                        });
                                    }
                                }
                                break;
                            case EnumActivityProcessorType.人员://人员
                                if (ids.Any())
                                {
                                    var userId = new List<Guid>();
                                    foreach (var id in ids)
                                    {
                                        userId.Add(Guid.Parse(id));
                                    }
                                    var permissionUser = await fixture.Db.SystemUserInfo.SetSelect(s => new { s.UserId, s.Name }).FindAllAsync(f => userId.Contains(f.UserId));
                                    if (permissionUser == null) continue;
                                    foreach (var item in permissionUser)
                                    {
                                        persons.Add(new WorkflowEngineProcessingPersonDetailOutput
                                        {
                                            UserTypeSelect = userTypeSelect,
                                            Pattern = pattern,
                                            PatternName = patternName,
                                            UserId = item.UserId,
                                            Name = item.Name
                                        });
                                    }
                                }
                                break;
                            case EnumActivityProcessorType.岗位:
                                if (ids.Any())
                                {
                                    IList<Guid> orgIds = new List<Guid>();
                                    if (organizationRange.HasValue && organizationRange != 100)
                                    {
                                        var nowUser = await fixture.Db.SystemUserInfo.SetSelect(s => new { s.OrganizationId }).FindAsync(f => f.UserId == uId);
                                        var nowOrg = await fixture.Db.SystemOrganization.SetSelect(s => new { s.ParentIds }).FindAsync(f => f.OrganizationId == nowUser.OrganizationId);
                                        //得到对应上下级组织架构
                                        var organizationId = nowOrg.ParentIds.Split(',').Select(s => Guid.Parse(s)).ToList();
                                        var orgs = await fixture.Db.SystemOrganization.SetSelect(s => new { s.OrganizationId }).FindAsync(f =>
                                            organizationId.Contains(f.OrganizationId) && f.Nature == organizationRange);
                                        var parentIds = orgs.OrganizationId.ToString();
                                        orgIds = (await fixture.Db.SystemOrganization.SetSelect(s => new { s.OrganizationId }).FindAllAsync(f => f.ParentIds.Contains(parentIds))).Select(s => s.OrganizationId).ToList();
                                    }
                                    foreach (var id in ids)
                                    {
                                        var gid = Guid.Parse(id);
                                        var posts = (await fixture.Db.SystemPermissionUserFindByPrivilegeMasterAndValueOutput
                                            .FindAllAsync<SystemPermissionUserFindByPrivilegeMasterAndValueUsersOutput>(
                                                f => f.PrivilegeMaster == EnumPrivilegeMaster.岗位.ToShort() &&
                                                     f.PrivilegeMasterValue == gid, o => o.Users)).ToList();
                                        if (!posts.Any()) continue;
                                        foreach (var item in posts[0].Users)
                                        {
                                            if (organizationRange.HasValue && organizationRange != 100)
                                            {
                                                if (orgIds.Any(a => a == item.OrganizationId))
                                                {
                                                    persons.Add(new WorkflowEngineProcessingPersonDetailOutput
                                                    {
                                                        UserTypeSelect = userTypeSelect,
                                                        Pattern = pattern,
                                                        PatternName = patternName,
                                                        UserId = item.UserId,
                                                        Name = item.Name
                                                    });
                                                }
                                            }
                                            else
                                            {
                                                persons.Add(new WorkflowEngineProcessingPersonDetailOutput
                                                {
                                                    UserTypeSelect = userTypeSelect,
                                                    Pattern = pattern,
                                                    PatternName = patternName,
                                                    UserId = item.UserId,
                                                    Name = item.Name
                                                });
                                            }
                                        }
                                    }
                                }
                                break;
                            case EnumActivityProcessorType.组:
                                if (ids.Any())
                                {
                                    IList<Guid> orgIds = new List<Guid>();
                                    if (organizationRange.HasValue && organizationRange != 100)
                                    {
                                        var nowUser = await fixture.Db.SystemUserInfo.SetSelect(s => new { s.OrganizationId }).FindAsync(f => f.UserId == uId);
                                        var nowOrg = await fixture.Db.SystemOrganization.SetSelect(s => new { s.ParentIds }).FindAsync(f => f.OrganizationId == nowUser.OrganizationId);
                                        //得到对应上下级组织架构
                                        var organizationId = nowOrg.ParentIds.Split(',').Select(s => Guid.Parse(s)).ToList();
                                        var orgs = await fixture.Db.SystemOrganization.SetSelect(s => new { s.OrganizationId }).FindAsync(f =>
                                            organizationId.Contains(f.OrganizationId) && f.Nature == organizationRange);
                                        var parentIds = orgs.OrganizationId.ToString();
                                        orgIds = (await fixture.Db.SystemOrganization.SetSelect(s => new { s.OrganizationId }).FindAllAsync(f => f.ParentIds.Contains(parentIds))).Select(s => s.OrganizationId).ToList();
                                    }
                                    foreach (var id in ids)
                                    {
                                        var gid = Guid.Parse(id);
                                        var posts = (await fixture.Db.SystemPermissionUserFindByPrivilegeMasterAndValueOutput
                                            .FindAllAsync<SystemPermissionUserFindByPrivilegeMasterAndValueUsersOutput>(
                                                f => f.PrivilegeMaster == EnumPrivilegeMaster.组.ToShort() &&
                                                     f.PrivilegeMasterValue == gid, o => o.Users)).ToList();
                                        if (!posts.Any()) continue;
                                        foreach (var item in posts[0].Users)
                                        {
                                            if (organizationRange.HasValue && organizationRange != 100)
                                            {
                                                if (orgIds.Any(a => a == item.OrganizationId))
                                                {
                                                    persons.Add(new WorkflowEngineProcessingPersonDetailOutput
                                                    {
                                                        UserTypeSelect = userTypeSelect,
                                                        Pattern = pattern,
                                                        PatternName = patternName,
                                                        UserId = item.UserId,
                                                        Name = item.Name
                                                    });
                                                }
                                            }
                                            else
                                            {
                                                persons.Add(new WorkflowEngineProcessingPersonDetailOutput
                                                {
                                                    UserTypeSelect = userTypeSelect,
                                                    Pattern = pattern,
                                                    PatternName = patternName,
                                                    UserId = item.UserId,
                                                    Name = item.Name
                                                });
                                            }
                                        }
                                    }
                                }
                                break;
                            case EnumActivityProcessorType.角色://角色人员
                                if (ids.Any())
                                {
                                    IList<Guid> orgIds = new List<Guid>();
                                    if (organizationRange.HasValue && organizationRange != 100)
                                    {
                                        var nowUser = await fixture.Db.SystemUserInfo.SetSelect(s => new { s.OrganizationId }).FindAsync(f => f.UserId == uId);
                                        var nowOrg = await fixture.Db.SystemOrganization.SetSelect(s => new { s.ParentIds }).FindAsync(f => f.OrganizationId == nowUser.OrganizationId);
                                        //得到对应上下级组织架构
                                        var organizationId = nowOrg.ParentIds.Split(',').Select(s => Guid.Parse(s)).ToList();
                                        var orgs = await fixture.Db.SystemOrganization.SetSelect(s => new { s.OrganizationId }).FindAsync(f =>
                                            organizationId.Contains(f.OrganizationId) && f.Nature == organizationRange);

                                        if (orgs != null)
                                        {
                                            var parentIds = orgs.OrganizationId.ToString();
                                            orgIds = (await fixture.Db.SystemOrganization.SetSelect(s => new { s.OrganizationId }).FindAllAsync(f => f.ParentIds.Contains(parentIds))).Select(s => s.OrganizationId).ToList();
                                        }
                                    }
                                    foreach (var id in ids)
                                    {
                                        var gid = Guid.Parse(id);
                                        var roles = (await fixture.Db.SystemPermissionUserFindByPrivilegeMasterAndValueOutput
                                            .FindAllAsync<SystemPermissionUserFindByPrivilegeMasterAndValueUsersOutput>(
                                                f => f.PrivilegeMaster == EnumPrivilegeMaster.角色.ToShort() &&
                                                     f.PrivilegeMasterValue == gid, o => o.Users)).ToList();
                                        if (!roles.Any()) continue;
                                        foreach (var item in roles[0].Users)
                                        {
                                            if (organizationRange.HasValue && organizationRange != 100)
                                            {
                                                if (orgIds.Any(a => a == item.OrganizationId))
                                                {
                                                    persons.Add(new WorkflowEngineProcessingPersonDetailOutput
                                                    {
                                                        UserTypeSelect = userTypeSelect,
                                                        Pattern = pattern,
                                                        PatternName = patternName,
                                                        UserId = item.UserId,
                                                        Name = item.Name
                                                    });
                                                }
                                            }
                                            else
                                            {
                                                persons.Add(new WorkflowEngineProcessingPersonDetailOutput
                                                {
                                                    UserTypeSelect = userTypeSelect,
                                                    Pattern = pattern,
                                                    PatternName = patternName,
                                                    UserId = item.UserId,
                                                    Name = item.Name
                                                });
                                            }
                                        }
                                    }
                                }
                                break;
                            case EnumActivityProcessorType.发起者://发起人
                                var instance = await fixture.Db.WorkflowProcessInstance.SetSelect(s => new { s.CreateUserId, s.CreateUserName }).FindAsync(f => f.ProcessInstanceId == activity.ProcessInstanceId);
                                if (instance != null)
                                {
                                    persons.Add(new WorkflowEngineProcessingPersonDetailOutput
                                    {
                                        UserTypeSelect = userTypeSelect,
                                        Pattern = pattern,
                                        PatternName = patternName,
                                        UserId = instance.CreateUserId,
                                        Name = instance.CreateUserName
                                    });
                                }
                                else
                                {
                                    var nowUser = await fixture.Db.SystemUserInfo.SetSelect(s => new { s.UserId, s.Name }).FindAsync(f => f.UserId == uId);
                                    persons.Add(new WorkflowEngineProcessingPersonDetailOutput
                                    {
                                        UserTypeSelect = userTypeSelect,
                                        Pattern = pattern,
                                        PatternName = patternName,
                                        UserId = nowUser.UserId,
                                        Name = nowUser.Name
                                    });
                                }
                                break;
                            case EnumActivityProcessorType.提交人直线领导: //提交人的直线领导
                                var userLeader = (await fixture.Db.SystemUserFindLeaderOutput.FindAllAsync<SystemUserLeadersOutput>(f => f.UserId == uId, o => o.Outputs)).ToList();
                                if (userLeader.Any())
                                {
                                    foreach (var item in userLeader.First().Outputs)
                                    {
                                        persons.Add(new WorkflowEngineProcessingPersonDetailOutput
                                        {
                                            UserTypeSelect = userTypeSelect,
                                            Pattern = pattern,
                                            PatternName = patternName,
                                            UserId = item.UserId,
                                            Name = item.Name
                                        });
                                    }
                                }
                                break;
                            case EnumActivityProcessorType.表单人员: //表单处理人
                                IList<WorkflowEngineFormControls> controls = new List<WorkflowEngineFormControls>();
                                string relationTxt = "_Txt";
                                foreach (var id in ids)
                                {
                                    controls.Add(new WorkflowEngineFormControls
                                    {
                                        ColumnName = id,
                                    });
                                    controls.Add(new WorkflowEngineFormControls
                                    {
                                        ColumnName = id + relationTxt,
                                    });
                                }
                                if (activity.FormId.HasValue)
                                {
                                    var results = await InitFormData(new WorkflowEngineFormInitDataInput
                                    {
                                        ActivityId = activity.ActivityId,
                                        FormId = (Guid)activity.FormId,
                                        ProcessInstanceId = activity.ProcessInstanceId,
                                        Controls = controls
                                    });
                                    foreach (var id in ids)
                                    {
                                        var item = results.FirstOrDefault(w => w.Name == id + relationTxt);
                                        var itemRelationId = results.FirstOrDefault(w => w.Name == id);
                                        if (item.Value.IsNotNullOrEmpty())
                                        {
                                            var userNames = item.Value.Split(',');
                                            var userIds = itemRelationId.Value.Split(',');
                                            for (int i = 0; i < userNames.Length; i++)
                                            {
                                                persons.Add(new WorkflowEngineProcessingPersonDetailOutput
                                                {
                                                    UserTypeSelect = userTypeSelect,
                                                    Pattern = pattern,
                                                    PatternName = patternName,
                                                    UserId = Guid.Parse(userIds[i]),
                                                    Name = userNames[i]
                                                });
                                            }
                                        }
                                    }
                                    //activity.ApproveUserStrategy = EnumApproveUserStrategy.发送给列表中的所有处理人.ToShort();
                                }
                                break;
                            case EnumActivityProcessorType.发起者直线领导: //发起者的直线领导
                                var createUserInstance = await fixture.Db.WorkflowProcessInstance.SetSelect(s => new { s.CreateUserId }).FindAsync(f => f.ProcessInstanceId == activity.ProcessInstanceId);
                                var createUserId = uId;
                                if (createUserInstance != null)
                                {
                                    createUserId = createUserInstance.CreateUserId;
                                }
                                var createUserLeader = (await fixture.Db.SystemUserFindLeaderOutput.FindAllAsync<SystemUserLeadersOutput>(f => f.UserId == createUserId, o => o.Outputs)).ToList();
                                if (createUserLeader.Any())
                                {
                                    foreach (var item in createUserLeader.First().Outputs)
                                    {
                                        persons.Add(new WorkflowEngineProcessingPersonDetailOutput
                                        {
                                            UserTypeSelect = userTypeSelect,
                                            Pattern = pattern,
                                            PatternName = patternName,
                                            UserId = item.UserId,
                                            Name = item.Name
                                        });
                                    }
                                }
                                break;
                            case EnumActivityProcessorType.自定义:
                                foreach (var item in ids)
                                {
                                    //序列
                                    var ruleSql = item.ReplaceHtmlTag();
                                    var rules = ruleSql.Split('{');
                                    foreach (var ruleItem in rules)
                                    {
                                        if (ruleItem.Contains("}"))
                                        {
                                            var ruleItemSplits = ruleItem.Split('}');
                                            foreach (var ruleItemSplit in ruleItemSplits)
                                            {
                                                if (ruleItemSplit.Contains("[表单]:"))
                                                {
                                                    var columnName = ruleItemSplit.Replace("[表单]:", "");
                                                    IList<WorkflowEngineFormControls> ruleControls = new List<WorkflowEngineFormControls>();
                                                    ruleControls.Add(new WorkflowEngineFormControls
                                                    {
                                                        ColumnName = columnName,
                                                    });
                                                    if (activity.FormId.HasValue)
                                                    {
                                                        var results = await InitFormData(new WorkflowEngineFormInitDataInput
                                                        {
                                                            ActivityId = activity.ActivityId,
                                                            FormId = (Guid)activity.FormId,
                                                            ProcessInstanceId = activity.ProcessInstanceId,
                                                            Controls = ruleControls
                                                        });
                                                        var value = results.FirstOrDefault(w => w.Name == columnName).Value;
                                                        ruleSql = ruleSql.Replace("{[表单]:" + columnName + "}", value);
                                                    }
                                                }
                                                if (ruleItemSplit.Contains("[角色]:"))
                                                {
                                                    var ruleItems = ruleItemSplit.Replace("[角色]:", "");
                                                    ruleSql = ruleSql.Replace("{[角色]:" + ruleItems + "}", ruleItems.InSql());
                                                }
                                            }

                                        }
                                    }
                                    //执行Sql查询人员
                                    persons = (await GetApproveUserBySql(null, ruleSql)).ToList();
                                    foreach (var approveUser in persons)
                                    {
                                        approveUser.UserTypeSelect = userTypeSelect;
                                        approveUser.Pattern = pattern;
                                        approveUser.PatternName = patternName;
                                    }
                                }
                                break;
                            case EnumActivityProcessorType.程序指定: //程序指定
                                persons.Add(new WorkflowEngineProcessingPersonDetailOutput
                                {
                                    UserTypeSelect = userTypeSelect,
                                    Pattern = pattern,
                                    PatternName = patternName
                                });
                                break;
                        }
                        patternGroup++;
                    }
                    //personOutput.Activity.Title += approveUserName.Any() ? "【" + approveUserName.ExpandAndToString() + "】" : "";
                    persons = persons.DistinctBy(d => d.UserId).ToList();
                }
                return persons;
            }
        }

        /// <summary>
        /// 查找加签人员
        /// </summary>
        /// <param name="activityId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<WorkflowEngineFindAddctivityChosenUserOutput> FindAddctivityChosenUser(Guid activityId, Guid userId)
        {
            WorkflowEngineFindAddctivityChosenUserOutput output = new WorkflowEngineFindAddctivityChosenUserOutput();
            IList<JsTreeEntity> users = new List<JsTreeEntity>();
            using (var fixture = new SqlDatabaseFixture())
            {
                //var activity = await fixture.Db.WorkflowProcessInstanceActivity.FindAsync(activityId);
                //var findUsers = await FindWorkflowEnginPersonOutput(activity, userId, activity.AddActivityChosenUser);
                //foreach (var item in findUsers)
                //{
                //    users.Add(new JsTreeEntity
                //    {
                //        id = item.UserId,
                //        text = item.Name + $"({item.Code})",
                //        parent = "#"
                //    });
                //}
                //output.Users = users;
            }
            return output;
        }

        /// <summary>
        /// 获取下面所有节点
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="activityId"></param>
        /// <returns></returns>
        private WorkflowEngineFindNextInstanceAcitvitysDto FindNextInstanceAcitvitys(
            WorkflowEngineProcessInstanceOutput instance,
            Guid activityId)
        {
            var instanceLinks = instance.InstanceLinks.Where(w => w.Begin == activityId).ToList();
            var linksTo = instanceLinks.Select(s => s.End).ToList();
            return new WorkflowEngineFindNextInstanceAcitvitysDto
            {
                Activities = instance.InstanceActivities.Where(w => linksTo.Contains(w.ActivityId)),
                Links = instanceLinks
            };
        }

        /// <summary>
        /// 根据条件获取后续满足条件的第一个活动节点
        /// </summary>
        /// <returns></returns>
        private async Task<OperateStatus<IEnumerable<WorkflowProcessInstanceActivity>>> FindProcessInstanceAcitvitysCondition(WorkflowEngineProcessInstanceOutput instance,
            WorkflowProcessInstanceActivity activity,
            Guid userId)
        {
            //查看当前节点是否配置条件表达式
            OperateStatus<IEnumerable<WorkflowProcessInstanceActivity>> operateStatus = new OperateStatus<IEnumerable<WorkflowProcessInstanceActivity>>();

            var state = GetProcessinstanceActivity(activity.ProcessInstanceId, activity.ActivityId).JsonStringToObject<WorkflowEngineFindMonitorFlowStatesOutput>();

            //判断连线
            var links = instance.InstanceLinks.Where(w => w.Begin == activity.ActivityId).ToList();
            if (!links.Any())
            {
                operateStatus.Msg = ResourceWorkflowEngine.未找到分支活动后续连线;
                return operateStatus;
            }
            //是否都具有表达式
            if (links.Any(w => w.Judge.IsNullOrEmpty()))
            {
                operateStatus.Msg = ResourceWorkflowEngine.配置判断表达式为空;
                return operateStatus;
            }
            var nextActivitys = FindNextInstanceAcitvitys(instance, activity.ActivityId).Activities.ToList();
            if (!nextActivitys.Any())
            {
                operateStatus.Msg = ResourceWorkflowEngine.未找到分支活动判断节点表达式;
                return operateStatus;
            }
            var conditionObj = new WorkflowEngineConditionInput();
            //满足条件连线
            var gotoLinks = new List<WorkflowProcessInstanceLink>();
            //所有连线条件判断
            foreach (var link in links)
            {
                var linkObject = link.Json.JsonStringToObject<WorkflowEngineFindMonitorFlowLinksOutput>();
                conditionObj.Judge = linkObject.props.@base.judge;
                conditionObj.ProcessInstanceId = link.ProcessInstanceId;
                conditionObj.Columns = instance.Columns;
                using (var fix = new SqlDatabaseFixture())
                {
                    var systemAgile = await fix.Db.SystemAgile.SetSelect(s => new { s.DataFromName, s.DataBaseId }).FindAsync(f => f.ConfigId == linkObject.props.@base.formId);
                    conditionObj.Table = systemAgile.DataFromName;

                    var dataBase = await GetSystemDataBase(fix, new IdInput { Id = systemAgile.DataBaseId });
                    conditionObj.ConnectionType = dataBase.ConnectionType;
                    conditionObj.ConnectionString = dataBase.ConnectionString;
                }

                //是否具有表达式
                bool result = await _processInstanceActivityRepository.CheckCondition(conditionObj);
                if (result)
                    gotoLinks.Add(link);
            }

            foreach (var link in gotoLinks)
            {
                _instanceLinks.Add(link);
                var nextActivity = nextActivitys.FirstOrDefault(w => w.ActivityId == link.End);
                if (nextActivity != null)
                {
                    if (nextActivity.Type == EnumAcitvityType.审批.FindDescription() || nextActivity.Type == EnumAcitvityType.结束.FindDescription() || nextActivity.Type == EnumAcitvityType.子流程.FindDescription())
                    {
                        _instanceActivities.Add(nextActivity);
                    }
                    else if (nextActivity.Type == EnumAcitvityType.条件.FindDescription())
                    {
                        var result = await FindProcessInstanceAcitvitysCondition(instance, nextActivity, userId);
                        if (result.Code != ResultCode.Success)
                        {
                            operateStatus.Msg = result.Msg;
                            return operateStatus;
                        }
                    }
                    else
                    {
                        await FindProcessInstancePerson(instance, userId);
                    }
                }
            }
            operateStatus.Code = ResultCode.Success;
            return operateStatus;
        }

        /// <summary>
        /// 根据条件获取后续满足条件的第一个活动节点
        /// </summary>
        /// <returns></returns>
        private async Task<OperateStatus<bool>> FindProcessInstanceAcitvitysJoin(
            WorkflowEngineProcessInstanceOutput instance,
            WorkflowProcessInstanceActivity activity,
            Guid userId)
        {
            //是否通过需要走下一步活动
            bool isPass = false;
            OperateStatus<bool> operateStatus = new OperateStatus<bool>();
            // 若为分支
            var join = GetProcessinstanceActivity(activity.ProcessInstanceId, activity.ActivityId).JsonStringToObject<WorkflowActivityJoinDto>();
            if (join.Props.Base.Type == EnumJoinType.分支.ToShort())
            {
                isPass = true;
            }
            //合并
            else
            {
                //到合并处查看是否走下一步活动
                var activityFroms = instance.InstanceLinks.Where(w => w.End == activity.ActivityId).ToList();
                #region 通过策略
                //全部通过
                if (join.Props.Base.Pass == EnumJoinPassType.全部通过.ToShort())
                {
                    //查看前一节点对应的活动是否都通过
                    foreach (var item in activityFroms)
                    {
                        _instanceLinks.Add(item);
                        //判断活动类型
                        var ac = instance.InstanceActivities.FirstOrDefault(f => f.ActivityId == item.Begin);
                        if (ac != null)
                        {
                            if (ac.Type == EnumAcitvityType.审批.FindDescription())
                            {
                                //是否在审批完成
                                var task = instance.InstanceTasks.Where(w => w.ActivityId == item.Begin && w.Status == EnumTaskStatus.完成.ToShort());
                                if (!task.Any())
                                {
                                    isPass = false;
                                    break;
                                }
                                isPass = true;
                            }
                        }
                    }
                }

                //1人通过就通过
                else if (join.Props.Base.Pass == EnumJoinPassType.一人通过就通过.ToShort())
                {
                    foreach (var item in activityFroms)
                    {
                        _instanceLinks.Add(item);
                        //判断活动类型
                        var ac = instance.InstanceActivities.FirstOrDefault(f => f.ActivityId == item.Begin);
                        if (ac != null)
                        {
                            if (ac.Type == EnumAcitvityType.审批.FindDescription())
                            {
                                //是否在审批完成
                                var task = instance.InstanceTasks.Where(w => w.ActivityId == item.Begin && w.Status == EnumTaskStatus.完成.ToShort());
                                if (task.Any())
                                {
                                    isPass = true;
                                    break;
                                }
                            }
                        }
                    }
                }

                //后台百分比通过
                else if (join.Props.Base.Pass == EnumJoinPassType.百分比通过.ToShort())
                {
                    int isPassNum = 0, allNum = 0;
                    foreach (var item in activityFroms)
                    {
                        _instanceLinks.Add(item);
                        //判断活动类型
                        var ac = instance.InstanceActivities.FirstOrDefault(f => f.ActivityId == item.Begin);
                        if (ac != null)
                        {
                            if (ac.Type == EnumAcitvityType.审批.FindDescription())
                            {
                                //是否在审批完成
                                var task = instance.InstanceTasks.Where(w =>
                                    w.ActivityId == item.Begin && w.Status == EnumTaskStatus.完成.ToShort());
                                if (task.Any())
                                {
                                    isPassNum++;
                                }
                                allNum++;
                            }
                        }
                    }

                    //判断比例
                    if (isPassNum != 0)
                    {
                        isPass = isPassNum / allNum * 100 >= join.Props.Base.PassPercent;
                    }
                }
                #endregion
            }
            //若通过则查找下一步活动节点
            if (isPass)
            {
                await FindProcessInstanceNextActivities(instance, activity.ActivityId, userId);
            }
            operateStatus.Data = isPass;
            operateStatus.Code = ResultCode.Success;
            return operateStatus;
        }

        /// <summary>
        /// 根据活动获取所有活动的流程处理人员
        /// </summary>
        /// <param name="activities">下一步走的所有步骤</param>
        /// <param name="uId">当前用户</param>
        /// <param name="instance"></param>
        /// <returns></returns>
        private async Task<List<WorkflowEngineProcessingPersonOutput>> FindWorkflowEngineProcessingPersonOutput(
            IList<WorkflowProcessInstanceActivity> activities,
            Guid uId,
             WorkflowEngineProcessInstanceOutput instance)
        {
            using (var fixture = new SqlDatabaseFixture())
            {
                List<WorkflowEngineProcessingPersonOutput> processingPersonOutput = new List<WorkflowEngineProcessingPersonOutput>();
                foreach (var activity in activities)
                {
                    //解析用户
                    WorkflowEngineProcessingPersonOutput personOutput = new WorkflowEngineProcessingPersonOutput
                    {
                        Activity = activity
                    };

                    var activityObj = GetProcessinstanceActivity(activity.ProcessInstanceId, activity.ActivityId).JsonStringToObject<WorkflowActivityDto>();

                    var approve = activityObj.Props.User;
                    if (approve != null && approve.Approve.Any())
                    {
                        IList<WorkflowActivityUserApprove> personOutputsSelect = new List<WorkflowActivityUserApprove>();
                        IList<WorkflowEngineProcessingPersonDetailOutput> persons = new List<WorkflowEngineProcessingPersonDetailOutput>();
                        #region 处理策略
                        if (approve.Strategy == EnumApproveUserStrategy.列表中的第一处理人.ToShort())
                        {
                            personOutputsSelect.Add(approve.Approve[0]);
                        }
                        else
                        {
                            personOutputsSelect = approve.Approve;
                        }
                        #endregion
                        int patternGroup = 1;
                        IList<string> approveUserName = new List<string>();
                        foreach (var output in personOutputsSelect)
                        {
                            approveUserName.Add(output.Name);
                            var pattern = output.Pattern.ToString();
                            var userTypeSelect = output.Type.ToShort();
                            var organizationRange = output.OrganizationRange;
                            var patternName = pattern == EnumApproveUserTypeSelectPattern.radio.ToString() ? patternGroup.ToString() : "";
                            IList<string> ids = output.Range.Select(s => s.Id).ToList();
                            switch (output.Type)
                            {
                                case EnumActivityProcessorType.所有人员://所有人员
                                    var users = await fixture.Db.SystemUserInfo.SetSelect(s => new { s.UserId, s.Name, s.OrganizationName, s.HeadImage, s.Code }).FindAllAsync(f => !f.IsAdmin && !f.IsFreeze);
                                    foreach (var item in users)
                                    {
                                        persons.Add(new WorkflowEngineProcessingPersonDetailOutput
                                        {
                                            UserTypeSelect = userTypeSelect,
                                            Pattern = pattern,
                                            PatternName = patternName,
                                            UserId = item.UserId,
                                            Name = item.Name,
                                            Code = item.Code,
                                            OrganizationName = item.OrganizationName,
                                            HeadImage = item.HeadImage,
                                        });
                                    }
                                    break;
                                case EnumActivityProcessorType.组织://组织
                                    foreach (var id in ids)
                                    {
                                        var gid = Guid.Parse(id);
                                        var orgs = (await fixture.Db.SystemPermissionUserFindByPrivilegeMasterAndValueOutput
                                            .FindAllAsync<SystemPermissionUserFindByPrivilegeMasterAndValueUsersOutput>(
                                                f => f.PrivilegeMaster == EnumPrivilegeMaster.组织架构.ToShort() &&
                                                     f.PrivilegeMasterValue == gid, o => o.Users)).ToList();
                                        if (!orgs.Any()) continue;
                                        foreach (var item in orgs[0].Users)
                                        {
                                            persons.Add(new WorkflowEngineProcessingPersonDetailOutput
                                            {
                                                UserTypeSelect = userTypeSelect,
                                                Pattern = pattern,
                                                PatternName = patternName,
                                                UserId = item.UserId,
                                                Name = item.Name,
                                                Code = item.Code,
                                                OrganizationName = item.OrganizationName,
                                                HeadImage = item.HeadImage,
                                            });
                                        }
                                    }
                                    break;
                                case EnumActivityProcessorType.人员://人员
                                    if (ids.Any())
                                    {
                                        var userId = new List<Guid>();
                                        foreach (var id in ids)
                                        {
                                            userId.Add(Guid.Parse(id));
                                        }
                                        var permissionUser = await fixture.Db.SystemUserInfo.SetSelect(s => new { s.UserId, s.Name, s.OrganizationName, s.HeadImage, s.Code }).FindAllAsync(f => userId.Contains(f.UserId));
                                        if (permissionUser == null) continue;
                                        foreach (var item in permissionUser)
                                        {
                                            persons.Add(new WorkflowEngineProcessingPersonDetailOutput
                                            {
                                                UserTypeSelect = userTypeSelect,
                                                Pattern = pattern,
                                                PatternName = patternName,
                                                UserId = item.UserId,
                                                Name = item.Name,
                                                Code = item.Code,
                                                OrganizationName = item.OrganizationName,
                                                HeadImage = item.HeadImage,
                                            });
                                        }
                                    }
                                    break;
                                case EnumActivityProcessorType.岗位:
                                    if (ids.Any())
                                    {
                                        IList<Guid> orgIds = new List<Guid>();
                                        if (organizationRange.HasValue && organizationRange != 100)
                                        {
                                            var nowUser = await fixture.Db.SystemUserInfo.SetSelect(s => new { s.OrganizationId }).FindAsync(f => f.UserId == uId);
                                            var nowOrg = await fixture.Db.SystemOrganization.SetSelect(s => new { s.ParentIds }).FindAsync(f => f.OrganizationId == nowUser.OrganizationId);
                                            //得到对应上下级组织架构
                                            var organizationId = nowOrg.ParentIds.Split(',').Select(s => Guid.Parse(s)).ToList();
                                            var orgs = await fixture.Db.SystemOrganization.SetSelect(s => new { s.OrganizationId }).FindAsync(f =>
                                                organizationId.Contains(f.OrganizationId) && f.Nature == organizationRange);
                                            var parentIds = orgs.OrganizationId.ToString();
                                            orgIds = (await fixture.Db.SystemOrganization.SetSelect(s => new { s.OrganizationId }).FindAllAsync(f => f.ParentIds.Contains(parentIds))).Select(s => s.OrganizationId).ToList();
                                        }
                                        foreach (var id in ids)
                                        {
                                            var gid = Guid.Parse(id);
                                            var posts = (await fixture.Db.SystemPermissionUserFindByPrivilegeMasterAndValueOutput
                                                .FindAllAsync<SystemPermissionUserFindByPrivilegeMasterAndValueUsersOutput>(
                                                    f => f.PrivilegeMaster == EnumPrivilegeMaster.岗位.ToShort() &&
                                                         f.PrivilegeMasterValue == gid, o => o.Users)).ToList();
                                            if (!posts.Any()) continue;
                                            foreach (var item in posts[0].Users)
                                            {
                                                if (organizationRange.HasValue && organizationRange != 100)
                                                {
                                                    if (orgIds.Any(a => a == item.OrganizationId))
                                                    {
                                                        persons.Add(new WorkflowEngineProcessingPersonDetailOutput
                                                        {
                                                            UserTypeSelect = userTypeSelect,
                                                            Pattern = pattern,
                                                            PatternName = patternName,
                                                            UserId = item.UserId,
                                                            Name = item.Name,
                                                            Code = item.Code,
                                                            OrganizationName = item.OrganizationName,
                                                            HeadImage = item.HeadImage,
                                                        });
                                                    }
                                                }
                                                else
                                                {
                                                    persons.Add(new WorkflowEngineProcessingPersonDetailOutput
                                                    {
                                                        UserTypeSelect = userTypeSelect,
                                                        Pattern = pattern,
                                                        PatternName = patternName,
                                                        UserId = item.UserId,
                                                        Name = item.Name,
                                                        Code = item.Code,
                                                        OrganizationName = item.OrganizationName,
                                                        HeadImage = item.HeadImage,
                                                    });
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case EnumActivityProcessorType.组:
                                    if (ids.Any())
                                    {
                                        IList<Guid> orgIds = new List<Guid>();
                                        if (organizationRange.HasValue && organizationRange != 100)
                                        {
                                            var nowUser = await fixture.Db.SystemUserInfo.SetSelect(s => new { s.OrganizationId }).FindAsync(f => f.UserId == uId);
                                            var nowOrg = await fixture.Db.SystemOrganization.SetSelect(s => new { s.ParentIds }).FindAsync(f => f.OrganizationId == nowUser.OrganizationId);
                                            //得到对应上下级组织架构
                                            var organizationId = nowOrg.ParentIds.Split(',').Select(s => Guid.Parse(s)).ToList();
                                            var orgs = await fixture.Db.SystemOrganization.SetSelect(s => new { s.OrganizationId }).FindAsync(f =>
                                                organizationId.Contains(f.OrganizationId) && f.Nature == organizationRange);
                                            var parentIds = orgs.OrganizationId.ToString();
                                            orgIds = (await fixture.Db.SystemOrganization.SetSelect(s => new { s.OrganizationId }).FindAllAsync(f => f.ParentIds.Contains(parentIds))).Select(s => s.OrganizationId).ToList();
                                        }
                                        foreach (var id in ids)
                                        {
                                            var gid = Guid.Parse(id);
                                            var posts = (await fixture.Db.SystemPermissionUserFindByPrivilegeMasterAndValueOutput
                                                .FindAllAsync<SystemPermissionUserFindByPrivilegeMasterAndValueUsersOutput>(
                                                    f => f.PrivilegeMaster == EnumPrivilegeMaster.组.ToShort() &&
                                                         f.PrivilegeMasterValue == gid, o => o.Users)).ToList();
                                            if (!posts.Any()) continue;
                                            foreach (var item in posts[0].Users)
                                            {
                                                if (organizationRange.HasValue && organizationRange != 100)
                                                {
                                                    if (orgIds.Any(a => a == item.OrganizationId))
                                                    {
                                                        persons.Add(new WorkflowEngineProcessingPersonDetailOutput
                                                        {
                                                            UserTypeSelect = userTypeSelect,
                                                            Pattern = pattern,
                                                            PatternName = patternName,
                                                            UserId = item.UserId,
                                                            Name = item.Name,
                                                            Code = item.Code,
                                                            OrganizationName = item.OrganizationName,
                                                            HeadImage = item.HeadImage,
                                                        });
                                                    }
                                                }
                                                else
                                                {
                                                    persons.Add(new WorkflowEngineProcessingPersonDetailOutput
                                                    {
                                                        UserTypeSelect = userTypeSelect,
                                                        Pattern = pattern,
                                                        PatternName = patternName,
                                                        UserId = item.UserId,
                                                        Name = item.Name,
                                                        Code = item.Code,
                                                        OrganizationName = item.OrganizationName,
                                                        HeadImage = item.HeadImage,
                                                    });
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case EnumActivityProcessorType.角色://角色人员
                                    if (ids.Any())
                                    {
                                        IList<Guid> orgIds = new List<Guid>();
                                        if (organizationRange.HasValue && organizationRange != 100)
                                        {
                                            var nowUser = await fixture.Db.SystemUserInfo.SetSelect(s => new { s.OrganizationId }).FindAsync(f => f.UserId == uId);
                                            var nowOrg = await fixture.Db.SystemOrganization.SetSelect(s => new { s.ParentIds }).FindAsync(f => f.OrganizationId == nowUser.OrganizationId);
                                            //得到对应上下级组织架构
                                            var organizationId = nowOrg.ParentIds.Split(',').Select(s => Guid.Parse(s)).ToList();
                                            var orgs = await fixture.Db.SystemOrganization.SetSelect(s => new { s.OrganizationId }).FindAsync(f =>
                                                organizationId.Contains(f.OrganizationId) && f.Nature == organizationRange);

                                            if (orgs != null)
                                            {
                                                var parentIds = orgs.OrganizationId.ToString();
                                                orgIds = (await fixture.Db.SystemOrganization.SetSelect(s => new { s.OrganizationId }).FindAllAsync(f => f.ParentIds.Contains(parentIds))).Select(s => s.OrganizationId).ToList();
                                            }
                                        }
                                        foreach (var id in ids)
                                        {
                                            var gid = Guid.Parse(id);
                                            var roles = (await fixture.Db.SystemPermissionUserFindByPrivilegeMasterAndValueOutput
                                                .FindAllAsync<SystemPermissionUserFindByPrivilegeMasterAndValueUsersOutput>(
                                                    f => f.PrivilegeMaster == EnumPrivilegeMaster.角色.ToShort() &&
                                                         f.PrivilegeMasterValue == gid, o => o.Users)).ToList();
                                            if (!roles.Any()) continue;
                                            foreach (var item in roles[0].Users)
                                            {
                                                if (organizationRange.HasValue && organizationRange != 100)
                                                {
                                                    if (orgIds.Any(a => a == item.OrganizationId))
                                                    {
                                                        persons.Add(new WorkflowEngineProcessingPersonDetailOutput
                                                        {
                                                            UserTypeSelect = userTypeSelect,
                                                            Pattern = pattern,
                                                            PatternName = patternName,
                                                            UserId = item.UserId,
                                                            Name = item.Name,
                                                            Code = item.Code,
                                                            OrganizationName = item.OrganizationName,
                                                            HeadImage = item.HeadImage,
                                                        });
                                                    }
                                                }
                                                else
                                                {
                                                    persons.Add(new WorkflowEngineProcessingPersonDetailOutput
                                                    {
                                                        UserTypeSelect = userTypeSelect,
                                                        Pattern = pattern,
                                                        PatternName = patternName,
                                                        UserId = item.UserId,
                                                        Name = item.Name,
                                                        Code = item.Code,
                                                        OrganizationName = item.OrganizationName,
                                                        HeadImage = item.HeadImage,
                                                    });
                                                }
                                            }
                                        }
                                    }
                                    break;
                                case EnumActivityProcessorType.发起者://发起人
                                    var instanceStart = await fixture.Db.WorkflowProcessInstance.SetSelect(s => new { s.CreateUserId }).FindAsync(f => f.ProcessInstanceId == activity.ProcessInstanceId);
                                    if (instanceStart != null)
                                    {
                                        var nowUser = await fixture.Db.SystemUserInfo.SetSelect(s => new { s.UserId, s.Name, s.OrganizationName, s.HeadImage, s.Code }).FindAsync(f => f.UserId == instanceStart.CreateUserId);
                                        persons.Add(new WorkflowEngineProcessingPersonDetailOutput
                                        {
                                            UserTypeSelect = userTypeSelect,
                                            Pattern = pattern,
                                            PatternName = patternName,
                                            UserId = nowUser.UserId,
                                            Name = nowUser.Name,
                                            Code = nowUser.Code,
                                            OrganizationName = nowUser.OrganizationName,
                                            HeadImage = nowUser.HeadImage,
                                        });
                                    }
                                    else
                                    {
                                        var nowUser = await fixture.Db.SystemUserInfo.SetSelect(s => new { s.UserId, s.Name, s.OrganizationName, s.HeadImage, s.Code }).FindAsync(f => f.UserId == uId);
                                        persons.Add(new WorkflowEngineProcessingPersonDetailOutput
                                        {
                                            UserTypeSelect = userTypeSelect,
                                            Pattern = pattern,
                                            PatternName = patternName,
                                            UserId = nowUser.UserId,
                                            Name = nowUser.Name,
                                            Code = nowUser.Code,
                                            OrganizationName = nowUser.OrganizationName,
                                            HeadImage = nowUser.HeadImage,
                                        });
                                    }
                                    break;
                                case EnumActivityProcessorType.提交人直线领导: //提交人的直线领导
                                    var userLeader = (await fixture.Db.SystemUserFindLeaderOutput.FindAllAsync<SystemUserLeadersOutput>(f => f.UserId == uId, o => o.Outputs)).ToList();
                                    if (userLeader.Any())
                                    {
                                        foreach (var item in userLeader.First().Outputs)
                                        {
                                            persons.Add(new WorkflowEngineProcessingPersonDetailOutput
                                            {
                                                UserTypeSelect = userTypeSelect,
                                                Pattern = pattern,
                                                PatternName = patternName,
                                                UserId = item.UserId,
                                                Name = item.Name,
                                                Code = item.Code,
                                                OrganizationName = item.OrganizationName,
                                                HeadImage = item.HeadImage,
                                            });
                                        }
                                    }
                                    break;
                                case EnumActivityProcessorType.表单人员: //表单处理人
                                    IList<WorkflowEngineFormControls> controls = new List<WorkflowEngineFormControls>();
                                    string relationTxt = "_Txt";
                                    foreach (var id in ids)
                                    {
                                        controls.Add(new WorkflowEngineFormControls
                                        {
                                            ColumnName = id,
                                        });
                                        controls.Add(new WorkflowEngineFormControls
                                        {
                                            ColumnName = id + relationTxt,
                                        });
                                    }
                                    if (activity.FormId.HasValue)
                                    {
                                        var results = await InitFormData(new WorkflowEngineFormInitDataInput
                                        {
                                            ActivityId = activity.ActivityId,
                                            FormId = (Guid)activity.FormId,
                                            ProcessInstanceId = activity.ProcessInstanceId,
                                            Controls = controls,
                                            Columns = instance.Columns
                                        });
                                        foreach (var id in ids)
                                        {
                                            var itemRelationId = results.FirstOrDefault(w => w.Name == id);
                                            if (itemRelationId.Value.IsNotNullOrEmpty())
                                            {
                                                var userId = itemRelationId.Value.Split(',').Select(s => Guid.Parse(s));
                                                var nowUser = await fixture.Db.SystemUserInfo.SetSelect(s => new { s.UserId, s.Name, s.OrganizationName, s.HeadImage, s.Code }).FindAllAsync(f => userId.Contains(f.UserId));
                                                foreach (var item in nowUser)
                                                {
                                                    persons.Add(new WorkflowEngineProcessingPersonDetailOutput
                                                    {
                                                        UserTypeSelect = userTypeSelect,
                                                        Pattern = pattern,
                                                        PatternName = patternName,
                                                        UserId = item.UserId,
                                                        Name = item.Name,
                                                        Code = item.Code,
                                                        OrganizationName = item.OrganizationName,
                                                        HeadImage = item.HeadImage
                                                    });
                                                }
                                            }
                                        }
                                        //personOutput.Activity.ApproveUserStrategy = EnumApproveUserStrategy.发送给列表中的所有处理人.ToShort();
                                    }
                                    break;
                                case EnumActivityProcessorType.发起者直线领导: //发起者的直线领导
                                    var createUserInstance = await fixture.Db.WorkflowProcessInstance.SetSelect(s => new { s.CreateUserId }).FindAsync(f => f.ProcessInstanceId == activity.ProcessInstanceId);
                                    var createUserId = uId;
                                    if (createUserInstance != null)
                                    {
                                        createUserId = createUserInstance.CreateUserId;
                                    }
                                    var createUserLeader = (await fixture.Db.SystemUserFindLeaderOutput.FindAllAsync<SystemUserLeadersOutput>(f => f.UserId == createUserId, o => o.Outputs)).ToList();
                                    if (createUserLeader.Any())
                                    {
                                        foreach (var item in createUserLeader.First().Outputs)
                                        {
                                            persons.Add(new WorkflowEngineProcessingPersonDetailOutput
                                            {
                                                UserTypeSelect = userTypeSelect,
                                                Pattern = pattern,
                                                PatternName = patternName,
                                                UserId = item.UserId,
                                                Name = item.Name,
                                                Code = item.Code,
                                                OrganizationName = item.OrganizationName,
                                                HeadImage = item.HeadImage,
                                            });
                                        }
                                    }
                                    break;
                                case EnumActivityProcessorType.自定义:
                                    foreach (var item in ids)
                                    {
                                        //序列
                                        var ruleSql = item.ReplaceHtmlTag();
                                        var rules = ruleSql.Split('{');
                                        foreach (var ruleItem in rules)
                                        {
                                            if (ruleItem.Contains("}"))
                                            {
                                                var ruleItemSplits = ruleItem.Split('}');
                                                foreach (var ruleItemSplit in ruleItemSplits)
                                                {
                                                    if (ruleItemSplit.Contains("[表单]:"))
                                                    {
                                                        var columnName = ruleItemSplit.Replace("[表单]:", "");
                                                        IList<WorkflowEngineFormControls> ruleControls = new List<WorkflowEngineFormControls>();
                                                        ruleControls.Add(new WorkflowEngineFormControls
                                                        {
                                                            ColumnName = columnName,
                                                        });
                                                        if (activity.FormId.HasValue)
                                                        {
                                                            var results = await InitFormData(new WorkflowEngineFormInitDataInput
                                                            {
                                                                ActivityId = activity.ActivityId,
                                                                FormId = (Guid)activity.FormId,
                                                                ProcessInstanceId = activity.ProcessInstanceId,
                                                                Controls = ruleControls,
                                                                Columns = instance.Columns
                                                            });
                                                            var value = results.FirstOrDefault(w => w.Name == columnName).Value;
                                                            ruleSql = ruleSql.Replace("{[表单]:" + columnName + "}", value);
                                                        }
                                                    }
                                                    if (ruleItemSplit.Contains("[角色]:"))
                                                    {
                                                        var ruleItems = ruleItemSplit.Replace("[角色]:", "");
                                                        ruleSql = ruleSql.Replace("{[角色]:" + ruleItems + "}", ruleItems.InSql());
                                                    }
                                                }

                                            }
                                        }
                                        //执行Sql查询人员
                                        persons = (await GetApproveUserBySql(null, ruleSql)).ToList();
                                        foreach (var approveUser in persons)
                                        {
                                            approveUser.UserTypeSelect = userTypeSelect;
                                            approveUser.Pattern = pattern;
                                            approveUser.PatternName = patternName;
                                        }
                                    }
                                    break;
                                case EnumActivityProcessorType.程序指定: //程序指定
                                    persons.Add(new WorkflowEngineProcessingPersonDetailOutput
                                    {
                                        UserTypeSelect = userTypeSelect,
                                        Pattern = pattern,
                                        PatternName = patternName
                                    });
                                    break;
                            }
                            patternGroup++;
                        }
                        //personOutput.Activity.Title += approveUserName.Any() ? "【" + approveUserName.ExpandAndToString() + "】" : "";
                        personOutput.Persons = persons.DistinctBy(d => d.UserId).ToList();
                    }
                    processingPersonOutput.Add(personOutput);
                }
                return processingPersonOutput;
            }
        }

        /// <summary>
        /// 检测无流程处理人策略
        /// </summary>
        /// <returns></returns>
        private async Task<OperateStatus> CheckNoProcessingPerson(WorkflowEngineProcessInstanceOutput instance, Guid userId)
        {
            OperateStatus operateStatus = new OperateStatus();
            foreach (var activity in _instanceActivities.Where(w => !w.IsPass))
            {
                activity.IsPass = true;
                if (activity.Type != EnumAcitvityType.结束.FindDescription())
                {
                    var config = GetProcessinstanceActivity(activity.ProcessInstanceId, activity.ActivityId).JsonStringToObject<WorkflowEngineFindMonitorFlowStatesOutput>();

                    //判断条件
                    if (config.props.user.approveUserNoDo == EnumTaskApproveUserNoDo.不能提交.GetHashCode())
                    {
                        operateStatus.Msg = ResourceWorkflowEngine.未找到流程处理人;
                        return operateStatus;
                    }
                }
                else
                {
                    operateStatus.Code = ResultCode.Success;
                    return operateStatus;
                }
            }
            //则默认所有的均为默认通过,插入通过步骤,查询下一步是否具有处理人
            foreach (var activity in _instanceActivities)
            {
                return await FindProcessInstancePerson(instance, userId);
            }
            operateStatus.Code = ResultCode.Success;
            return operateStatus;
        }

        /// <summary>
        /// 检查当前活动状态
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="taskId"></param>
        /// <returns></returns>
        private OperateStatus<WorkflowProcessInstanceTask> CheckCurrentTask(WorkflowEngineProcessInstanceOutput instance, Guid taskId)
        {
            OperateStatus<WorkflowProcessInstanceTask> operateStatus = new OperateStatus<WorkflowProcessInstanceTask>();
            WorkflowProcessInstanceTask task = instance.InstanceTasks.FirstOrDefault(f => f.TaskId == taskId);
            if (task == null)
            {
                operateStatus.Msg = ResourceWorkflowEngine.未找到流程活动信息;
                return operateStatus;
            }
            if (task.Status == EnumTaskStatus.完成.ToShort())
            {
                operateStatus.Msg = string.Format(ResourceWorkflowEngine.任务已处理, task.DoUserName);
                return operateStatus;
            }
            operateStatus.Data = task;
            operateStatus.Code = ResultCode.Success;
            return operateStatus;
        }

        /// <summary>
        /// 运行结束流程
        /// </summary>
        /// <returns></returns>
        private async Task<OperateStatus<WorkflowEngineProcessRunOutput>> RunEndTask(
            SqlDatabaseFixture fixture,
            WorkflowEngineRunProcessInput input,
            WorkflowEngineProcessInstanceOutput instance,
            WorkflowProcessInstanceTask task)
        {
            OperateStatus<WorkflowEngineProcessRunOutput> operateStatus = new OperateStatus<WorkflowEngineProcessRunOutput>
            {
                Data = new WorkflowEngineProcessRunOutput()
            };
            var time = DateTime.Now;
            var allTasks = instance.InstanceTasks.Where(w => w.PrevTaskId == task.PrevTaskId && w.Status == EnumTaskStatus.正在处理.ToShort());
            foreach (var item in allTasks)
            {
                item.DoUserId = input.DoUserId;
                item.DoUserName = input.DoUserName;
                item.Comment = input.Comment;
                item.Status = EnumTaskStatus.完成.ToShort();
                item.Remark = input.DoUserName + "|" + time.ToYyyyMMddHHmmss() + "|" + ResourceWorkflowEngine.处理完毕;
                item.CompletedTime = time;
                await fixture.Db.WorkflowProcessInstanceTask.UpdateAsync(item);
            }

            //插入结束流程
            if (task != null)
            {
                var endAct = input.NextTask.FirstOrDefault(w => w.Activity.Type == EnumAcitvityType.结束.FindDescription());
                if (endAct != null)
                {
                    var endTask = new WorkflowProcessInstanceTask
                    {
                        TaskId = CombUtil.NewComb(),
                        ProcessInstanceId = task.ProcessInstanceId,
                        ActivityId = endAct.Activity.ActivityId,
                        ActivityType = ConvertIndexByDes(endAct.Activity.Type),
                        SendUserId = input.DoUserId,
                        SendUserName = input.DoUserName,
                        DoUserId = input.DoUserId,
                        DoUserName = input.DoUserName,
                        ReceiveUserId = input.DoUserId,
                        ReceiveUserName = input.DoUserName,
                        ReceiveTime = time,
                        CompletedTime = time,
                        CustomData = task.CustomData,
                        Status = EnumTaskStatus.完成.ToShort(),
                        Remark = input.DoUserName + "|" + time.ToYyyyMMddHHmmss() + "|提交",
                        PrevTaskId = input.TaskId
                    };
                    await fixture.Db.WorkflowProcessInstanceTask.InsertAsync(endTask);
                    operateStatus.Data.TaskId = endTask.TaskId;
                }
            }
            //TODO:将所有阅示任务默认处理
            //修改主流程
            instance.Instance.Status = EnumProcessInstanceStatus.完成.ToShort();
            instance.Instance.StatusRemark = ResourceWorkflowEngine.处理完毕;
            await fixture.Db.WorkflowProcessInstance.UpdateAsync(instance.Instance);
            await UpdateIsPass(new WorkflowEngineUpdateIsPassInput
            {
                Activities = input.Activities,
                Links = input.Links
            }, fixture);
            await UpdateFormStatus(new WorkflowEngineUpdateFormStatusInput
            {
                ProcessInstanceId = instance.Instance.ProcessInstanceId,
                Status = EnumProcessInstanceStatus.完成
            });
            operateStatus.Msg = string.Format(ResourceWorkflowEngine.流程已结束, instance.Instance.Sn);
            operateStatus.Code = ResultCode.Success;
            //子流程
            if (!instance.Instance.FromTaskId.IsNullOrEmptyGuid())
            {
                var instanceActivities = instance.InstanceActivities.FirstOrDefault(f => f.ActivityId == task.ActivityId);
                await RunSubProcessEndTask(fixture, new WorkflowEngineRunSubProcessEndTaskInput
                {
                    FormId = instanceActivities.FormId,
                    ProcessInstanceId = instanceActivities.ProcessInstanceId,
                    Instance = instance.Instance,
                    DoUserName = input.DoUserName,
                    DoUserId = input.DoUserId,
                });
            }

            var cacheKey = string.Format(ResouceWorkflowCacheKey.流程实例, input.ProcessInstanceId);
            await RedisHelper.DelAsync(cacheKey);

            return operateStatus;
        }

        /// <summary>
        /// 运行子流程接收后任务
        /// </summary>
        /// <returns></returns>
        private async Task RunSubProcessEndTask(SqlDatabaseFixture fixture,
            WorkflowEngineRunSubProcessEndTaskInput input)
        {
            //查询父任务
            var task = await fixture.Db.WorkflowProcessInstanceTask.FindAsync(f => f.TaskId == input.Instance.FromTaskId);
            //由前端选择表单,需优化
            var processInstance = await fixture.Db.WorkflowProcessInstance.FindAsync(f => f.ProcessInstanceId == task.ProcessInstanceId);
            var formId = (await fixture.Db.WorkflowProcess.FindAsync(f => f.ProcessId == processInstance.ProcessId)).FormId;
            var activity = (await fixture.Db.WorkflowProcessInstanceActivity.FindAsync(f => f.ActivityId == task.ActivityId && f.ProcessInstanceId == task.ProcessInstanceId));
            OperateStatus operateStatus = new OperateStatus();
            WorkflowEngineRunProcessInput taskProcessInput = new WorkflowEngineRunProcessInput();
            taskProcessInput.TaskId = task.TaskId;
            taskProcessInput.ActivityId = task.ActivityId;
            taskProcessInput.Type = (EnumAcitvityType)task.ActivityType;
            taskProcessInput.DoUserId = input.DoUserId;
            taskProcessInput.DoUserName = input.DoUserName;
            ////等待子流程返回
            //if (activity.SubProcessAsync)
            //{
            //    //得到发起数据
            //    var taskProcessResult = await TaskProcess(taskProcessInput);
            //    WorkflowEngineRunProcessInput runProcessInput = new WorkflowEngineRunProcessInput();
            //    runProcessInput.TaskId = task.TaskId;
            //    runProcessInput.ActivityId = task.ActivityId;
            //    runProcessInput.Type = (EnumAcitvityType)task.ActivityType;
            //    runProcessInput.DoUserId = input.DoUserId;
            //    runProcessInput.DoUserCode = input.DoUserCode;
            //    runProcessInput.DoUserName = input.DoUserName;
            //    runProcessInput.Activities = taskProcessResult.Data.Activities;
            //    runProcessInput.Links = taskProcessResult.Data.Links;
            //    runProcessInput.NextTask = taskProcessResult.Data.Person;
            //    var startProcessRunResult = await TaskProcessRun(runProcessInput);
            //    operateStatus.Code = ResultCode.Success;
            //    operateStatus.Msg = startProcessRunResult.Msg;
            //}
            //if (activity.SubProcessOutput.IsNotNullOrEmpty())
            //{
            //    IList<WorkflowEngineFormControls> controlses = new List<WorkflowEngineFormControls>();
            //    //序列化得到值
            //    var dataDto = activity.SubProcessOutput.JsonStringToList<WorkflowEngineSubProcessDataDto>();
            //    dataDto.ForEach(e => controlses.Add(new WorkflowEngineFormControls { ColumnName = e.Id }));
            //    //得到实例数据
            //    var inputResult = await InitFormData(new WorkflowEngineFormInitDataInput
            //    {
            //        ActivityId = taskProcessInput.ActivityId,
            //        ActivityType = taskProcessInput.Type.FindDescription(),
            //        FormId = (Guid)input.FormId,//当前任务表单
            //        OrganizationId = taskProcessInput.DoUserOrganizationId,
            //        OrganizationName = taskProcessInput.DoUserOrganizationName,
            //        ProcessInstanceId = input.ProcessInstanceId,
            //        Controls = controlses
            //    });
            //    //将得到数据插入
            //    IList<WorkflowEngineFormControls> insertControlses = new List<WorkflowEngineFormControls>();
            //    foreach (var data in dataDto)
            //    {
            //        var res = inputResult.FirstOrDefault(w => w.Name == data.Id);
            //        if (res == null) continue;
            //        insertControlses.Add(new WorkflowEngineFormControls { ColumnName = data.ColumnName, Type = "input", Value = res.Value });
            //    }
            //    await SaveFormData(new WorkflowEngineFormSaveDataInput
            //    {
            //        UserId = taskProcessInput.DoUserId,
            //        UserCode = taskProcessInput.DoUserCode,
            //        UserName = taskProcessInput.DoUserName,
            //        ActivityType = taskProcessInput.Type.FindDescription(),
            //        FormId = (Guid)formId,
            //        ProcessInstanceId = task.ProcessInstanceId,
            //        Controls = insertControlses
            //    });
            //}
        }

        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="des"></param>
        /// <returns></returns>
        private short ConvertIndexByDes(string des)
        {
            switch (des.ToLower())
            {
                case "start":
                    return EnumAcitvityType.开始.ToShort();
                case "task":
                    return EnumAcitvityType.审批.ToShort();
                case "subprocess":
                    return EnumAcitvityType.子流程.ToShort();
                case "fork":
                    return EnumAcitvityType.条件.ToShort();
                case "join":
                    return EnumAcitvityType.合并.ToShort();
                case "understanding":
                    return EnumAcitvityType.知会.ToShort();
                case "invitationread":
                    return EnumAcitvityType.阅示.ToShort();
                case "add":
                    return EnumAcitvityType.加签.ToShort();
                case "end":
                    return EnumAcitvityType.结束.ToShort();
                default:
                    return 0;
            }
        }

        /// <summary>
        /// 根据任务Id获取流程实例信息
        /// 1,从数据库获取,
        /// 2,从缓存中获取(后期添加)
        /// </summary>
        /// <param name="fixture"></param>
        /// <param name="processInstanceId"></param>
        /// <returns></returns>
        private async Task<WorkflowEngineProcessInstanceOutput> FindRunInstanceByProcessInstanceId(
            SqlDatabaseFixture fixture,
            Guid processInstanceId)
        {
            var instanceCacheKey = string.Format(ResouceWorkflowCacheKey.流程实例, processInstanceId);
            var instanceReturn = await RedisHelper.CacheShellAsync(instanceCacheKey, DateTimeUtil.TotalSeconds(30), async () =>
            {
                WorkflowEngineProcessInstanceOutput output = new WorkflowEngineProcessInstanceOutput();
                output.Instance = await fixture.Db.WorkflowProcessInstance.FindAsync(f => f.ProcessInstanceId == processInstanceId);
                output.InstanceActivities = (await fixture.Db.WorkflowProcessInstanceActivity.FindAllAsync(f => f.ProcessInstanceId == processInstanceId)).ToList();
                output.InstanceLinks = (await fixture.Db.WorkflowProcessInstanceLink.FindAllAsync(f => f.ProcessInstanceId == processInstanceId)).ToList();
                return output;
            });
            instanceReturn.InstanceTasks = (await fixture.Db.WorkflowProcessInstanceTask.FindAllAsync(f => f.ProcessInstanceId == processInstanceId)).ToList();
            return instanceReturn;
        }

        #endregion

        #region 流程设计器

        /// <summary>
        /// 连线检测条件
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus> CheckConditionLink(WorkflowEngineConditionInput input)
        {
            OperateStatus operateStatus = new OperateStatus();
            try
            {
                await _processInstanceActivityRepository.CheckCondition(input);
                operateStatus.Code = ResultCode.Success;
                operateStatus.Msg = ResourceWorkflowEngine.判断条件有效;
            }
            catch (EipException e)
            {
                operateStatus.Msg = $"{ResourceWorkflowEngine.判断条件无效}:{e.Message}";
            }
            return operateStatus;
        }

        /// <summary>
        /// 获取超时时间信息
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        private WorkflowEngineOverTimeOutput GetWorkflowEngineOverTime(WorkflowProcessInstanceActivity activity)
        {
            //var activityObj = activity.Json.JsonStringToObject<WorkflowActivityDto>();
            WorkflowEngineOverTimeOutput output = new WorkflowEngineOverTimeOutput();
            //if (activityObj.Props.TimeOut.Any())
            //{
            //    if (output.OverTimeMinute != null)
            //    {
            //        output.OverTime = DateTime.Now.AddMinutes((double)output.OverTimeMinute);
            //    }
            //}
            return output;
        }
        #endregion

        #region 事件

        /// <summary>
        /// 根据Api获取数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<object> EventDoByApi(WorkflowEngineEventDoByApiInput input)
        {
            var globalParams = GlobalParams.GetValuesByName();
            input.Url = globalParams.FirstOrDefault(f => f.Key == EnumConfigKey.systemDomainApi.ToString()).Value + input.Url;

            switch (input.Type.ToLower())
            {
                case "post":
                    return await RequestUtil.Post(input.Url, new
                    {
                        Controls = input.Data.Controls,
                        Task = input.Data.Task
                    }, input.Header);
                default:
                    return await RequestUtil.Get(input.Url, null, input.Header);
            }
        }
        #endregion

        /// <summary>
        /// 初始化表单数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<IList<WorkflowEngineFormControlsOutput>> InitFormData(WorkflowEngineFormInitDataInput input)
        {
            IList<WorkflowEngineFormControlsOutput> outputs = new List<WorkflowEngineFormControlsOutput>();
            using (var fixture = new SqlDatabaseFixture())
            {
                var systemAgile = await fixture.Db.SystemAgile.SetSelect(s => new { s.DataFromName, s.DataBaseId }).FindAsync(f => f.ConfigId == input.FormId);
                var dataBase = await GetSystemDataBase(fixture, new IdInput { Id = systemAgile.DataBaseId });
                //拼接sql
                string sql = $"SELECT * FROM {systemAgile.DataFromName} WHERE RelationId='{input.ProcessInstanceId}'";
                if (input.Columns.Any())
                {
                    sql = _processInstanceActivityRepository.CheckConditionSql(new WorkflowEngineConditionInput
                    {
                        Columns = input.Columns,
                        Judge = $" RelationId='{input.ProcessInstanceId}' ",
                        ProcessInstanceId = input.ProcessInstanceId,
                        Table = systemAgile.DataFromName,
                        ConnectionType = dataBase.ConnectionType,
                        ConnectionString = dataBase.ConnectionString
                    });
                }

                DataTable table = new DbHelper(dataBase.ConnectionString).CreateSqlDataTable(sql);
                //判断当前节点是否为开始节点
                WorkflowProcessInstance instance = new WorkflowProcessInstance();
                bool isStart = input.ActivityType == EnumAcitvityType.开始.FindDescription();
                if (!isStart)
                {
                    //获取流程实例发起人
                    instance = await fixture.Db.WorkflowProcessInstance.FindAsync(f => f.ProcessInstanceId == input.ProcessInstanceId);
                }
                DataRow dr = null;
                IList<string> fields = new List<string>();
                if (table.Rows.Count > 0)
                {
                    dr = table.Rows[0];
                    foreach (DataColumn dc in table.Columns)
                    {
                        fields.Add(dc.ColumnName);
                    }
                }
                foreach (var control in input.Controls)
                {
                    //判断类型是否为附件
                    if (control.Type == "files")
                    {
                        //查找附件并拷贝附件插入
                        var files = await fixture.Db.SystemFile.FindAllAsync(f =>
                            f.CorrelationId == input.ProcessInstanceId + "|" + control.ColumnName);
                        foreach (var file in files)
                        {
                            //file.FileId = CombUtil.NewComb();
                            //file.CorrelationId = input.StartProcessInstanceId + "|" + control.ColumnName;
                            ////拷贝文件
                            //var uploadPath = ConfigurationUtil.GetUploadPath();
                            //var path = file.Path.Split('/');
                            //var fileName = path[path.Length - 1].Replace(file.Extension, "");
                            //var orignFile = uploadPath + file.Path;
                            //var newFilePath = file.Path.Replace(fileName, file.FileId.ToString());
                            //var newFile = uploadPath + newFilePath;
                            //File.Copy(orignFile, newFile, true);
                            //file.Path = newFilePath;
                            //await fixture.Db.SystemFile.InsertAsync(file);
                        }
                    }
                    else
                    {
                        //判断是否具有默认值
                        var output = new WorkflowEngineFormControlsOutput
                        {
                            Name = control.ColumnName,
                            Type = control.Type,
                            Value = control.Default.IsNotNullOrEmpty() ? SetDefaultValue(control.Default, input, isStart, instance) : null
                        };

                        //数据库读取数据
                        if (table.Rows.Count > 0)
                        {
                            if (fields.Any(a => a == control.ColumnName))
                                if (dr != null)
                                    output.Value = dr[control.ColumnName].ToString();
                        }
                        outputs.Add(output);
                        if (control.IsSingle == "false" || control.ColumnName.ToLower() == "radio" || control.ColumnName.ToLower() == "checkbox")
                        {
                            //判断是否具有默认值
                            var outputSingle = new WorkflowEngineFormControlsOutput
                            {
                                Name = control.ColumnName,
                                Type = control.Type,
                                Value = control.Default.IsNotNullOrEmpty() ? SetDefaultValue(control.Default, input, isStart, instance) : null
                            };

                            //数据库读取数据
                            if (table.Rows.Count > 0)
                            {
                                if (fields.Any(a => a == control.ColumnName))
                                    if (dr != null)
                                        outputSingle.Value = dr[control.ColumnName].ToString();
                            }
                            outputs.Add(outputSingle);
                        }
                    }
                }
            }
            return outputs;
        }

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        private WorkflowEngineGetConnectionOutput GetConnection()
        {
            return new WorkflowEngineGetConnectionOutput
            {
                Connection = ConfigurationUtil.GetDbConnectionString()
            };
        }

        /// <summary>
        /// 删除表单数据
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        public async Task<OperateStatus> DeleteFormData(IList<WorkflowEngineFormDeleteDataInput> inputs)
        {
            OperateStatus<Guid> operateStatus = new OperateStatus<Guid>();
            try
            {

            }
            catch (Exception e)
            {
                operateStatus.Msg = e.Message;
                operateStatus.Code = ResultCode.Error;
            }
            return operateStatus;
        }

        /// <summary>
        /// 获取审批人员
        /// </summary>
        /// <param name="dataBaseId"></param>
        /// <param name="sql"></param>
        /// <returns></returns>
        public async Task<IEnumerable<WorkflowEngineProcessingPersonDetailOutput>> GetApproveUserBySql(Guid? dataBaseId, string sql)
        {
            var connection = dataBaseId.HasValue ? (GetConnection()).Connection : ConfigurationUtil.GetDbConnectionString();
            using (var db = GetDbConnection(connection))
            {
                return await db.QueryAsync<WorkflowEngineProcessingPersonDetailOutput>(sql);
            }
        }

        /// <summary>
        /// 赋予默认值
        /// </summary>
        /// <param name="default"></param>
        /// <param name="input"></param>
        /// <param name="isStart"></param>
        /// <param name="instance"></param>
        /// <returns></returns>
        private string SetDefaultValue(string @default, WorkflowEngineFormInitDataInput input, bool isStart, WorkflowProcessInstance instance)
        {
            object value = string.Empty;
            switch (@default)
            {
                //当前处理人
                case "userid":
                    value = input.UserId;
                    break;
                case "usercode":
                    value = input.UserCode;
                    break;
                case "username":
                    value = input.UserName;
                    break;
                case "orgid":
                    value = input.OrganizationId;
                    break;
                case "orgname":
                    value = input.OrganizationName;
                    break;
                //流程发起人
                case "senduserid":
                    value = isStart ? input.UserId : instance.CreateUserId;
                    break;
                case "sendusername":
                    value = isStart ? input.UserName : instance.CreateUserName;
                    break;
                case "sendorgid":
                    value = isStart ? input.OrganizationId : instance.CreateUserOrganizationId;
                    break;
                case "sendorgname":
                    value = isStart ? input.OrganizationName : instance.CreateUserOrganizationName;
                    break;
                //发起时间
                case "sendshorttime":
                    value = isStart ? DateTime.Now.ToString("yyyy-MM-dd") : instance.CreateTime.ToString("yyyy-MM-dd");
                    break;
                case "sendlongtime":
                    value = isStart ? DateTime.Now.ToString("yyyy年MM月dd日") : instance.CreateTime.ToString("yyyy年MM月dd日");
                    break;
                case "sendshorttimemmhh":
                    value = isStart ? DateTime.Now.ToString("HH:mm") : instance.CreateTime.ToString("HH:mm");
                    break;
                case "sendlongtimemmhh":
                    value = isStart ? DateTime.Now.ToString("HH时mm分") : instance.CreateTime.ToString("HH时mm分");
                    break;
                case "sendshortdatetime":
                    value = isStart ? DateTime.Now.ToString("yyyy-MM-dd HH:mm") : instance.CreateTime.ToString("yyyy-MM-dd HH:mm");
                    break;
                case "sendlongdatetime":
                    value = isStart ? DateTime.Now.ToString("yyyy年MM月dd日 HH时mm分") : instance.CreateTime.ToString("yyyy年MM月dd日 HH时mm分");
                    break;

                //发起时间
                case "shorttime":
                    value = DateTime.Now.ToString("yyyy-MM-dd");
                    break;
                case "longtime":
                    value = DateTime.Now.ToString("yyyy年MM月dd日");
                    break;
                case "shorttimemmhh":
                    value = DateTime.Now.ToString("HH:mm");
                    break;
                case "longtimemmhh":
                    value = DateTime.Now.ToString("HH时mm分");
                    break;
                case "shortdatetime":
                    value = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
                    break;
                case "longdatetime":
                    value = DateTime.Now.ToString("yyyy年MM月dd日 HH时mm分");
                    break;
            }

            return value.ToString();
        }

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <param name="connstring"></param>
        private DbConnection GetDbConnection(string connstring)
        {
            DbConnection conn;
            //判断执行类型
            var connectionType = ConfigurationUtil.GetDbConnectionType();
            switch (connectionType)
            {
                case ResourceDataBaseType.Mysql:
                    conn = new MySqlConnection(connstring);
                    break;
                case ResourceDataBaseType.Postgresql:
                    conn = new NpgsqlConnection(connstring);
                    break;
                case ResourceDataBaseType.Dm:
                    conn = new DmConnection(connstring);
                    break;
                //case ResourceDataBaseType.Kingbase:
                //    break;
                default:
                    conn = new SqlConnection(connstring);
                    break;
            }
            return conn;
        }

        /// <summary>
        /// 获取活动实例
        /// </summary>
        /// <param name="processInstanceId"></param>
        /// <param name="activityId"></param>
        /// <returns></returns>
        private string GetProcessinstanceActivity(Guid processInstanceId, Guid activityId)
        {
            return FileUtil.ReadFile(Directory.GetCurrentDirectory() + "/Workflow/" + processInstanceId + "/" + activityId + ".json");
        }
    }
}