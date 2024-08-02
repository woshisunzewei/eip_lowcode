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
    public interface IWorkflowProcessInstanceActivityRepository 
    {

        /// <summary>
        /// ����ʵ��Id��ȡ��Ӧ���Ϣ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IEnumerable<WorkflowEngineFindActivityByProcessIdAndTypeOutput>> FindActivityByProcessIdAndType(WorkflowEngineFindActivityByProcessIdAndTypeInput input);

        /// <summary>
        ///    ����ActivityId��ȡ�û���õİ�ť��
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IEnumerable<WorkflowButton>> FindProcessButtonByActivity(IdInput input);

        /// <summary>
        /// ��ȡ����ʵ����ť
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IEnumerable<WorkflowButton>> FindProcessInstanceButtonByActivity(IdInput input);

        /// <summary>
        /// �ж������Ƿ����
        /// </summary>
        /// <returns></returns>
        Task<bool> CheckCondition(WorkflowEngineConditionInput input);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        string CheckConditionSql(WorkflowEngineConditionInput input);
        /// <summary>
        /// ���߼������
        /// </summary>
        /// <returns></returns>
        Task<bool> CheckConditionLink(WorkflowEngineConditionInput input);

        /// <summary>
        /// ���ݻId��ȡ���Ϣ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<WorkflowEngineFindActivityByTaskIdOutput> FindActivityByTaskId(IdInput input);

        /// <summary>
        /// ����ʵ��Id��ȡ����Ϣ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<WorkflowEngineFindFormByProcessInstanceIdOutput> FindWorkflowProcessFormByProcessInstanceId(IdInput input);

        /// <summary>
        /// ��ȡ�˻ػ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IEnumerable<WorkflowEngineFindReturnActivityOutput>> FindReturnActivity(WorkflowEngineFindReturnActivityInput input);

        /// <summary>
        /// ��ȡ����ʵ���
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IEnumerable<WorkflowEngineFindReturnDoUserOutput>> FindReturnDoUser(WorkflowEngineFindReturnActivityInput input);

    }
}
