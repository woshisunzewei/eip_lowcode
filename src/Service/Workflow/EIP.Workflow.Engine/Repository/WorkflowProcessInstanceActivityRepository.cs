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
    public class WorkflowProcessInstanceActivityRepository : IWorkflowProcessInstanceActivityRepository
    {
        string symbol = RepositoryUtil.GetSymbol();
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<IEnumerable<WorkflowEngineFindActivityByProcessIdAndTypeOutput>> FindActivityByProcessIdAndType(WorkflowEngineFindActivityByProcessIdAndTypeInput input)
        {
            string sql = $@"SELECT
	                            activity.ActivityId,
	                            activity.ProcessId,
	                            activity.FormId,
	                            activity.Type,
	                            activity.Json,
	                            config.PublicJson,
                                config.ColumnJson 
                            FROM
	                            Workflow_ProcessActivity activity
	                            LEFT JOIN Workflow_Process process ON process.ProcessId = activity.ProcessId
	                            LEFT JOIN System_Agile config ON config.ConfigId = activity.FormId
                           where activity.ProcessId = {symbol}processId and activity.Type = {symbol}type";
            return new SqlMapperUtil().SqlWithParams<WorkflowEngineFindActivityByProcessIdAndTypeOutput>(sql, new
            {
                processId = input.ProcessId,
                type = input.Type.FindDescription()
            });
        }

        /// <summary>
        /// 根据任务Id获取表单信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<WorkflowEngineFindActivityByTaskIdOutput> FindActivityByTaskId(IdInput input)
        {
            string sql = $@"select activity.FormId,activity.Json,activity.ActivityId,agile.ColumnJson,task.processInstanceId,task.Status from Workflow_ProcessInstance_Activity activity
            left join Workflow_ProcessInstance_Task task on task.ActivityId=activity.ActivityId 
            left join System_Agile agile on activity.FormId = agile.ConfigId
			where task.TaskId={symbol}taskId";
            return new SqlMapperUtil().SqlWithParamsSingle<WorkflowEngineFindActivityByTaskIdOutput>(sql, new
            {
                taskId = input.Id
            });
        }

        /// <summary>
        /// 根据实例Id获取表单信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<WorkflowEngineFindFormByProcessInstanceIdOutput> FindWorkflowProcessFormByProcessInstanceId(IdInput input)
        {
            string sql = $@"select form.Url,form.PublicHtml from  Workflow_Process process 
                            left join Workflow_ProcessInstance pinstance on process.ProcessId=pinstance.ProcessId
                            left join Workflow_Form form on process.FormId = form.FormId
                            where pinstance.ProcessInstanceId={symbol}processInstanceId";
            return new SqlMapperUtil().SqlWithParamsSingle<WorkflowEngineFindFormByProcessInstanceIdOutput>(sql, new
            {
                processInstanceId = input.Id
            });
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<IEnumerable<WorkflowButton>> FindProcessButtonByActivity(IdInput input)
        {
            string sql = $@" declare @str varchar(1024)  --定义变量
                            select  @str=Button from Workflow_ProcessActivity where ActivityId={symbol}activityId
                            select * from Workflow_Button where ButtonId in(select  str2table  from strtotable(@str)) order by OrderNo";
            return new SqlMapperUtil().SqlWithParams<WorkflowButton>(sql, new
            {
                activityId = input.Id
            });
        }

        /// <summary>
        /// 获取流程实例按钮
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<IEnumerable<WorkflowButton>> FindProcessInstanceButtonByActivity(IdInput input)
        {
            string sql = $@" declare @str varchar(1024)
                            select  @str=Button from Workflow_ProcessInstance_Activity where ActivityId={symbol}activityId
                            select * from Workflow_Button where ButtonId in(select  str2table  from strtotable(@str)) order by OrderNo";
            return new SqlMapperUtil().SqlWithParams<WorkflowButton>(sql, new
            {
                activityId = input.Id
            });
        }

        /// <summary>
        /// 获取流程实例活动
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<IEnumerable<WorkflowEngineFindReturnDoUserOutput>> FindReturnDoUser(WorkflowEngineFindReturnActivityInput input)
        {
            string sql = $@" select DoUserId,DoUserCode,DoUserName,activity.Title from Workflow_ProcessInstance_Task task
                            left join Workflow_ProcessInstance_Activity activity on task.ProcessInstanceId=activity.ProcessInstanceId and task.ActivityId=activity.ActivityId
                            where task.ActivityId={symbol}activityId and task.ProcessInstanceId={symbol}processInstanceId  and DoUserId is not null group by DoUserId,DoUserCode,DoUserName,activity.Title";
            return new SqlMapperUtil().SqlWithParams<WorkflowEngineFindReturnDoUserOutput>(sql, new
            {
                activityId = input.ActivityId,
                processInstanceId = input.ProcessInstanceId
            });
        }
        private string connectionString = ConfigurationUtil.GetDbConnectionString();
        private string connectionType = ConfigurationUtil.GetDbConnectionType();
        /// <summary>
        /// 检测条件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> CheckCondition(WorkflowEngineConditionInput input)
        {
            var tempSql = CheckConditionSql(input);
            //StringBuilder stringBuilder = new StringBuilder();
            //stringBuilder.Append("SELECT * FROM ");
            //stringBuilder.Append(input.Table);
            //stringBuilder.Append(" WHERE ");
            //stringBuilder.Append(input.Judge.UnEscape().Xss());
            //stringBuilder.Append((input.Judge.UnEscape().Xss().IsNotNullOrEmpty() ? " And " : "") + $" RelationId='{input.ProcessInstanceId}'");
            return await new SqlMapperUtil(string.Empty, input.ConnectionType, input.ConnectionString).SqlWithParamsBool<object>(tempSql, new { });
        }
        /// <summary>
        /// 检测条件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public string CheckConditionSql(WorkflowEngineConditionInput input)
        {
            string[] noCreateField = { "batch", "uploadimg", "uploadfile", "wps", "alert" };
            //filterSql字段，是否过滤数据字段
            string[] filterSqlField = { "switch", "sign" };
            string connectionType = ConfigurationUtil.GetDbConnectionType();
            string tempSql = "";
            var currentUser = EipHttpContext.CurrentUser();
            switch (input.ConnectionType)
            {
                case ResourceDataBaseType.Mysql:
                    {
                        tempSql = $"drop TABLE if exists " + input.Table + "_CheckConditionTemp;CREATE TEMPORARY TABLE " + input.Table + "_CheckConditionTemp" + " SELECT * FROM " + input.Table + " LIMIT 0;";
                        StringBuilder stringBuilder = new StringBuilder();
                        StringBuilder stringBuilderValues = new StringBuilder();
                        stringBuilder.Append($"INSERT INTO {input.Table + "_CheckConditionTemp"} (");
                        //主表
                        var insertControls = input.Columns.Where(item =>  !noCreateField.Contains(item.Type.ToLower()) 
                                                                                                 && item.Name.ToUpper() != "RELATIONID"
                                                                                                 && !item.Value.IsNullOrEmpty()
                                                                                                );
                        foreach (var item in insertControls)
                        {
                            stringBuilder.Append($"{item.Name},");
                            item.Value = filterSqlField.Any(f => f == item.Type) ? item.Value : item.Value.FilterSql().Xss();
                            stringBuilderValues.Append(item.Type == "switch" ? $"{item.Value}," : $"N'{item.Value}',");
                        }
                        stringBuilder.Append("RelationId,");
                        stringBuilderValues.Append($"'{input.ProcessInstanceId}',");

                        stringBuilder.Append("CreateUserId,");
                        stringBuilderValues.Append($"'{currentUser.UserId}',");

                        stringBuilder.Append("CreateUserName,");
                        stringBuilderValues.Append($"'{currentUser.Name}',");

                        stringBuilder.Append("CreateTime,");
                        stringBuilderValues.Append($"'{DateTime.Now.ToYyyyMMddHHmmss()}',");

                        stringBuilder.Append("CreateOrganizationId,");
                        stringBuilderValues.Append($"'{currentUser.OrganizationId}',");

                        stringBuilder.Append("CreateOrganizationName,");
                        stringBuilderValues.Append($"'{currentUser.OrganizationName}',");

                        stringBuilder.Append("UpdateUserId,");
                        stringBuilderValues.Append($"'{currentUser.UserId}',");

                        stringBuilder.Append("UpdateUserName,");
                        stringBuilderValues.Append($"'{currentUser.Name}',");

                        stringBuilder.Append("UpdateOrganizationId,");
                        stringBuilderValues.Append($"'{currentUser.OrganizationId}',");

                        stringBuilder.Append("UpdateOrganizationName,");
                        stringBuilderValues.Append($"'{currentUser.OrganizationName}',");

                        stringBuilder.Append("IsDelete,");
                        stringBuilderValues.Append($"0,");

                        var sql = stringBuilder.ToString().TrimEnd(',') + " ) VALUES (" +
                               stringBuilderValues.ToString().TrimEnd(',') + ")";
                        tempSql += sql + ";";
                        input.Judge = input.Judge.UnEscape();
                        tempSql += $"SELECT * FROM {input.Table}_CheckConditionTemp WHERE {input.Judge};";
                    }
                    break;
                case ResourceDataBaseType.Postgresql:
                    break;
                default:
                    {
                        tempSql = $"IF OBJECT_ID(N'tempdb..#{input.Table}_CheckConditionTemp', N'U') IS  NOT  NULL drop TABLE #" + input.Table + "_CheckConditionTemp;select top 0 * into #" + input.Table + "_CheckConditionTemp" + "  FROM " + input.Table + ";";
                        StringBuilder stringBuilder = new StringBuilder();
                        StringBuilder stringBuilderValues = new StringBuilder();
                        stringBuilder.Append($"INSERT INTO #{input.Table + "_CheckConditionTemp"} (");
                        //主表
                        var insertControls = input.Columns.Where(item => item.Name.ToUpper() != "RELATIONID"
                                                                                                 && !item.Value.IsNullOrEmpty()
                                                                                                 && !noCreateField.Contains(item.Type.ToLower()));
                        foreach (var item in insertControls)
                        {
                            stringBuilder.Append($"{item.Name},");
                            item.Value = filterSqlField.Any(f => f == item.Type) ? item.Value : item.Value.FilterSql().Xss();
                            stringBuilderValues.Append(item.Type == "switch" ? $"{item.Value}," : $"N'{item.Value}',");
                        }
                        stringBuilder.Append("RelationId,");
                        stringBuilderValues.Append($"'{input.ProcessInstanceId}',");

                        stringBuilder.Append("CreateUserId,");
                        stringBuilderValues.Append($"'{currentUser.UserId}',");

                        stringBuilder.Append("CreateUserName,");
                        stringBuilderValues.Append($"'{currentUser.Name}',");

                        stringBuilder.Append("CreateTime,");
                        stringBuilderValues.Append($"'{DateTime.Now.ToYyyyMMddHHmmss()}',");

                        stringBuilder.Append("CreateOrganizationId,");
                        stringBuilderValues.Append($"'{currentUser.OrganizationId}',");

                        stringBuilder.Append("CreateOrganizationName,");
                        stringBuilderValues.Append($"'{currentUser.OrganizationName}',");

                        stringBuilder.Append("UpdateUserId,");
                        stringBuilderValues.Append($"'{currentUser.UserId}',");

                        stringBuilder.Append("UpdateUserName,");
                        stringBuilderValues.Append($"'{currentUser.Name}',");

                        stringBuilder.Append("UpdateOrganizationId,");
                        stringBuilderValues.Append($"'{currentUser.OrganizationId}',");

                        stringBuilder.Append("UpdateOrganizationName,");
                        stringBuilderValues.Append($"'{currentUser.OrganizationName}',");

                        stringBuilder.Append("IsDelete,");
                        stringBuilderValues.Append($"0,");

                        var sql = stringBuilder.ToString().TrimEnd(',') + " ) VALUES (" +
                               stringBuilderValues.ToString().TrimEnd(',') + ")";
                        tempSql += sql + ";";
                        input.Judge = input.Judge.UnEscape();
                        tempSql += $"SELECT * FROM #{input.Table}_CheckConditionTemp WHERE {input.Judge};";
                    }
                    break;
            }
            return tempSql;
           
        }

        /// <summary>
        /// 连线检测条件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> CheckConditionLink(WorkflowEngineConditionInput input)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("SELECT * FROM ");
            stringBuilder.Append(input.Table);
            stringBuilder.Append(" WHERE ");
            stringBuilder.Append(input.Judge.UnEscape().FilterSql());
            return await new SqlMapperUtil(string.Empty, connectionType, connectionString).SqlWithParamsBool<object>(stringBuilder.ToString(), new { });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<IEnumerable<WorkflowEngineFindReturnActivityOutput>> FindReturnActivity(WorkflowEngineFindReturnActivityInput input)
        {
            string sql = $@" select activity.ActivityId,activity.Title from Workflow_ProcessInstance_Activity activity
                            where activity.ActivityId in (select ActivityId from Workflow_ProcessInstance_Task task where task.ProcessInstanceId={symbol}processInstanceId and task.TaskId!={symbol}taskId and task.DoUserId is not null) and activity.ProcessInstanceId={symbol}processInstanceId";
            return new SqlMapperUtil().SqlWithParams<WorkflowEngineFindReturnActivityOutput>(sql, new
            {
                processInstanceId = input.ProcessInstanceId,
                taskId = input.TaskId
            });
        }

    }
}