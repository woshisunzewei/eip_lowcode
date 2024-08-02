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
using EIP.Workflow.Models.Dtos.Archives;

namespace EIP.Workflow.Repository.Impl
{
    /// <summary>
    /// 工作流意见数据访问接口实现
    /// </summary>
    public class WorkflowArchiveRepository : IWorkflowArchiveRepository
    {
        /// <summary>
        /// 获取归档数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResults<WorkflowArchiveOutput>> Find(WorkflowArchiveInput input)
        {
            string sql = $@"select
                                ArchiveId,
                                DoUserName,
                                pro.Name ProcessName,
                                ins.Title ProcessInstanceTitle, 
                                act.Title ActivityTitle,ac.CreateTime,@rowNumber, @recordCount
                                from Workflow_Archive ac
                                left join Workflow_ProcessInstance_Task task on ac.TaskId=task.TaskId
								left join Workflow_ProcessInstance ins on task.ProcessInstanceId=ins.ProcessInstanceId
                                left join Workflow_ProcessInstance_Activity act on task.ActivityId=act.ActivityId and act.ProcessInstanceId=ins.ProcessInstanceId
                                left join Workflow_Process pro on ins.ProcessId=pro.ProcessId
                                 @where";
            return new SqlMapperUtil().PagingQuerySqlAsync<WorkflowArchiveOutput>(sql, input);
        }
    }
}