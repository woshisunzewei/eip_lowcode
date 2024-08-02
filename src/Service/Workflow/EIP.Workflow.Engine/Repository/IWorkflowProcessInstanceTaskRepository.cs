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
    public interface IWorkflowProcessInstanceTaskRepository
    {
        /// <summary>
        /// ��ȡ����ʵ������
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IEnumerable<WorkflowEngineFindInstanceProcessOutput>> FindInstanceProcess(WorkflowEngineFindInstanceProcessInput input);

        /// <summary>
        /// ��ȡ����ʵ������
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IEnumerable<WorkflowProcessInstanceTask>> FindProcessInstanceTaskByPrevTaskId(IdInput input);
    }
}
