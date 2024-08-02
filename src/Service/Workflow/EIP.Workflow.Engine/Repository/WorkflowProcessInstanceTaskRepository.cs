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
    public class WorkflowProcessInstanceTaskRepository :  IWorkflowProcessInstanceTaskRepository
    {
        string symbol = RepositoryUtil.GetSymbol();
        /// <summary>
        /// 获取流程实例过程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<IEnumerable<WorkflowEngineFindInstanceProcessOutput>> FindInstanceProcess(WorkflowEngineFindInstanceProcessInput input)
        {
            string sql = $@"select task.TaskId,userInfo.HeadImage,task.SendUserId,task.ReceiveUserId, task.SendUserName,task.ReceiveUserName,task.ReceiveTime,task.DoUserId, task.DoUserName,task.CompletedTime,task.Comment,task.Status,task.ActivityId,activity.Title,activity.Type,task.ActivityType,task.ApproveUserId,task.ApproveUserName,task.ApproveTime,task.ApproveStatus,task.ApproveComment from Workflow_ProcessInstance_Task task
            left join Workflow_ProcessInstance_Activity activity on task.ActivityId = activity.ActivityId and activity.ProcessInstanceId = @id
            left join System_UserInfo userInfo on userInfo.UserId = task.ReceiveUserId
            where task.ProcessInstanceId = {symbol}id order by task.Id";
            return new SqlMapperUtil().SqlWithParams<WorkflowEngineFindInstanceProcessOutput>(sql, new { id = input.ProcessInstanceId });
        }

        /// <summary>
        /// 获取上一步一样的活动
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<IEnumerable<WorkflowProcessInstanceTask>> FindProcessInstanceTaskByPrevTaskId(IdInput input)
        {
            string sql = $"SELECT * FROM Workflow_ProcessInstance_Task WHERE PrevTaskId={symbol}prevtaskid";
            return new SqlMapperUtil().SqlWithParams<WorkflowProcessInstanceTask>(sql, new { prevtaskid = input.Id });
        }
    }
}
