/**************************************************************
* Copyright (C) www.eipflow.com ����ΰ��Ȩ����(����ؾ�)
*
* ����: ����ΰ(QQ 1039318332)
* ����ʱ��: 2018/10/30 22:40:15
* �ļ���: 
* ����: 
* 
* �޸���ʷ
* �޸��ˣ�
* ʱ�䣺
* �޸�˵����
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
        /// ��ȡ����ʵ������
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
        /// ��ȡ��һ��һ���Ļ
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
