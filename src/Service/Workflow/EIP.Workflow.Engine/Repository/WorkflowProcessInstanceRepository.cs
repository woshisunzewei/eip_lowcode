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
namespace EIP.Workflow.Engine.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public class WorkflowProcessInstanceRepository : IWorkflowProcessInstanceRepository
    {
        /// <summary>
        ///获取我的请求
        /// </summary>
        /// <returns></returns>
        public Task<PagedResults<WorkflowEngineFindHaveSendOutput>> FindHaveSeed(WorkflowEngineFindHaveSendInput input)
        {
            string sql = $@"select process.FormId,process.ShortName,processInstance.ProcessInstanceId,processInstance.Sn,processInstance.Title, processInstance.CreateTime,processInstance.Urgency,processInstance.Status,processInstance.StatusRemark,@rowNumber, @recordCount from Workflow_ProcessInstance processInstance
                            left join Workflow_Process process on processInstance.ProcessId = process.ProcessId
                            @where and processInstance.CreateUserId = '{input.UserId}' and processInstance.Type={EnumProcessInstanceType.正常.ToShort()}";
            return new SqlMapperUtil().PagingQuerySqlAsync<WorkflowEngineFindHaveSendOutput>(sql, input);
        }

        /// <summary>
        ///获取我的请求
        /// </summary>
        /// <returns></returns>
        public Task<PagedResults<WorkflowEngineFindOverTimeOutput>> FindOverTime(WorkflowEngineFindHaveSendInput input)
        {
            string sql = $@"SELECT task.CustomData,task.Remark,process.ShortName,process.Icon,task.TaskId,task.ProcessInstanceId,task.ActivityId,instance.Sn,instance.Title,instance.CreateUserName,task.SendUserName,task.ReceiveTime,activity.Title _activityName,activity.Type,task.ActivityType,instance.Urgency,task.PrevTaskId,@rowNumber, @recordCount 
                            FROM Workflow_ProcessInstance_Task task
                            left join Workflow_ProcessInstance instance on instance.ProcessInstanceId=task.ProcessInstanceId
                            left join Workflow_Process process on instance.ProcessId = process.ProcessId
                            left join Workflow_ProcessInstance_Activity activity on task.ActivityId=activity.ActivityId and activity.ProcessInstanceId=instance.ProcessInstanceId
                            @where and task.ReceiveUserId='{input.UserId}' and task.Status in ({EnumTaskStatus.超时.GetHashCode()})
                            and task.ActivityType not in ({EnumAcitvityType.子流程.ToShort()})";
            if (input.Sidx.IsNullOrEmpty())
            {
                input.Sidx = " task.Id ";
            }
            return new SqlMapperUtil().PagingQuerySqlAsync<WorkflowEngineFindOverTimeOutput>(sql, input);
        }

        /// <summary>
        ///获取待处理任务
        /// </summary>
        /// <returns></returns>
        public Task<PagedResults<WorkflowEngineFindNeedDoOutput>> FindNeedDo(WorkflowEngineFindNeedDoInput input)
        {
            //(select wu.Avatar from system_userinfo_three wu where wu.SystemUserId=instance.CreateUserId LIMIT 1)HeadImgurl,
            string sql = $@"SELECT activity.FormId,process.ProcessId,task.CustomData,task.Remark,process.ShortName,process.Icon,task.TaskId,task.ProcessInstanceId,task.ActivityId,instance.Sn,instance.Title,instance.CreateUserName,task.SendUserName,task.ReceiveTime,activity.Title _activityName,activity.Type,task.ActivityType,instance.Urgency,task.PrevTaskId,@rowNumber, @recordCount 
                            FROM Workflow_ProcessInstance_Task task
                            left join Workflow_ProcessInstance instance on instance.ProcessInstanceId=task.ProcessInstanceId
                            left join Workflow_Process process on instance.ProcessId = process.ProcessId
                            left join Workflow_ProcessInstance_Activity activity on task.ActivityId=activity.ActivityId and activity.ProcessInstanceId=instance.ProcessInstanceId
                            
                            @where and task.ReceiveUserId='{input.UserId}' and task.Status in ({EnumTaskStatus.正在处理.GetHashCode()})
                            and task.ActivityType not in ({EnumAcitvityType.子流程.ToShort()})";

            if (input.Sidx.IsNullOrEmpty())
            {
                input.Sidx = " task.Id ";
            }

            return new SqlMapperUtil().PagingQuerySqlAsync<WorkflowEngineFindNeedDoOutput>(sql, input);
        }

        /// <summary>
        ///获取待处理任务
        /// </summary>
        /// <returns></returns>
        public async Task<WorkflowSearchNumOutput> FindNum(IdInput input)
        {
            string haveNeedSql = $@"select  COUNT(*) HaveNeed from Workflow_ProcessInstance processInstance 
where  processInstance.CreateUserId = '{input.Id}' and processInstance.Type=0";

            string needDoSql = $@"SELECT count(*) NeedDo FROM Workflow_ProcessInstance_Task task
                            where  task.ReceiveUserId= '{input.Id}' and task.Status  =4
                            and task.ActivityType!=6";

            string haveDoSql = $@"select count(*)HaveDo FROM Workflow_ProcessInstance_Task task
where  task.DoUserId= '{input.Id}' and task.PrevTaskId is not null
group by task.CustomData,task.Remark,task.TaskId,task.Status";

            WorkflowSearchNumOutput workflowSearchNumOutput = new WorkflowSearchNumOutput();
            workflowSearchNumOutput.HaveSend = await new SqlMapperUtil().SqlWithParamsSingle<int>(haveNeedSql);
            workflowSearchNumOutput.NeedDo = await new SqlMapperUtil().SqlWithParamsSingle<int>(needDoSql);
            workflowSearchNumOutput.HaveDo = await new SqlMapperUtil().SqlWithParamsSingle<int>(haveDoSql);
            return workflowSearchNumOutput;
        }

        /// <summary>
        /// 获取已办事项
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResults<WorkflowEngineFindHaveDoOutput>> FindHaveDo(WorkflowEngineFindHaveDoInput input)
        {
            string sql = $@"select *,@rowNumber, @recordCount from (select task.TaskId,instance.Status InstanceStatus,process.Name ProcessName,task.CompletedTime, instance.CreateUserName,instance.ProcessInstanceId,instance.Sn,instance.Title,instance.CreateTime
                            FROM Workflow_ProcessInstance_Task task
                            left join Workflow_ProcessInstance instance on instance.ProcessInstanceId=task.ProcessInstanceId
                            left join Workflow_Process process on instance.ProcessId = process.ProcessId
                             where task.DoUserId='{input.UserId}' and task.PrevTaskId is not null and task.ActivityType!=100
                            group by  task.TaskId,processinstanceActivity.Title ,instance.Status ,process.Name ,task.CompletedTime, instance.CreateUserName,instance.ProcessInstanceId,instance.Sn,instance.Title,instance.CreateTime
                            ) a @where";

            if (input.Sidx.IsNullOrEmpty())
            {
                input.Sidx = " CompletedTime ";
            }

            return new SqlMapperUtil().PagingQuerySqlAsync<WorkflowEngineFindHaveDoOutput>(sql, input);
        }

        /// <summary>
        /// 获取监控列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResults<WorkflowEngineFindMonitorListOuput>> FindMonitorList(WorkflowEngineFindMonitorListInput input)
        {
            string sql = $@"select processInstance.CreateUserName,processInstance.ProcessInstanceId,processInstance.Sn,processInstance.Title,processInstance.CreateTime,processInstance.Urgency,processInstance.Status,processInstance.StatusRemark,@rowNumber, @recordCount from Workflow_ProcessInstance processInstance
                            left join Workflow_Process process on processInstance.ProcessId = process.ProcessId
                             @where and processInstance.Status!={EnumProcessInstanceStatus.删除.ToShort()}";
            return new SqlMapperUtil().PagingQuerySqlAsync<WorkflowEngineFindMonitorListOuput>(sql, input);
        }

        /// <summary>
        /// 获取监控列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResults<WorkflowEngineFindMonitorDeleteListOuput>> FindMonitorDeleteList(WorkflowEngineFindMonitorDeleteListInput input)
        {
            string sql = $@"select processInstance.UpdateUserName,processInstance.UpdateTime,processInstance.ProcessInstanceId,processInstance.Sn,processInstance.Title,processInstance.CreateTime,processInstance.Urgency,processInstance.StatusRemark,@rowNumber, @recordCount from Workflow_ProcessInstance processInstance
                            left join Workflow_Process process on processInstance.ProcessId = process.ProcessId @where and processInstance.Status={EnumProcessInstanceStatus.删除.ToShort()}";
            return new SqlMapperUtil().PagingQuerySqlAsync<WorkflowEngineFindMonitorDeleteListOuput>(sql, input);
        }
        /// <summary>
        /// 分页查询草稿箱
        /// </summary>
        /// <param name="paging">分页参数</param>
        /// <returns></returns>
        public Task<PagedResults<WorkflowEngineFindDraftOutput>> FindDraft(WorkflowEngineFindDraftInput paging)
        {
            StringBuilder sql = new StringBuilder("select ProcessInstanceId,ProcessId,Urgency,Title,CreateTime,CreateUserName,UpdateTime,@rowNumber, @recordCount from Workflow_ProcessInstance");
            sql.Append($@" @where and Workflow_ProcessInstance.CreateUserId='{paging.UserId}'");
            sql.Append($@" and Workflow_ProcessInstance.Type={EnumProcessInstanceType.草稿.ToShort()}");
            return new SqlMapperUtil().PagingQuerySqlAsync<WorkflowEngineFindDraftOutput>(sql.ToString(), paging);
        }

        /// <summary>
        /// 分页查询范本夹
        /// </summary>
        /// <param name="paging">分页参数</param>
        /// <returns></returns>
        public Task<PagedResults<WorkflowEngineFindModelOutput>> FindModel(WorkflowEngineFindModelInput paging)
        {
            StringBuilder sql = new StringBuilder("select ProcessInstanceId,ProcessId,Urgency,Title,CreateTime,CreateUserName,UpdateTime,@rowNumber, @recordCount from Workflow_ProcessInstance");
            sql.Append($@" @where and Workflow_ProcessInstance.CreateUserId='{paging.UserId}'");
            sql.Append($@" and Workflow_ProcessInstance.Type={EnumProcessInstanceType.范本.ToShort()}");
            return new SqlMapperUtil().PagingQuerySqlAsync<WorkflowEngineFindModelOutput>(sql.ToString(), paging);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<WorkflowEngineFindMonitorDetailOutput> FindMonitorDetial(IdInput input)
        {
            string symbol = RepositoryUtil.GetSymbol();
            string sql = $@"
            select top 1 activity.FormId,activity.Json,form.FormPcUrl,activity.ActivityId from Workflow_ProcessInstance_Activity activity
            left join System_Agile form on activity.FormId = form.ConfigId
			where 
			activity.ProcessInstanceId={symbol}processInstanceId
			and activity.FormId is not null";
            var connectionType = ConfigurationUtil.GetDbConnectionType();
            switch (connectionType)
            {
                case ResourceDataBaseType.Mysql:
                case ResourceDataBaseType.Dm:
                case ResourceDataBaseType.Kingbase:
                    sql = sql.Replace("top 1", "");
                    sql += " LIMIT 1 ";
                    break;
                case ResourceDataBaseType.Postgresql:
                    break;
            }
            return new SqlMapperUtil().SqlWithParamsSingle<WorkflowEngineFindMonitorDetailOutput>(sql, new
            {
                processInstanceId = input.Id
            });
        }
    }
}