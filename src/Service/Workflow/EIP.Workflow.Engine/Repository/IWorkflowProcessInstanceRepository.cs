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
using EIP.Common.Models.Paging;

namespace EIP.Workflow.Engine.Repository
{
    /// <summary>
    /// ��������ʱ
    /// </summary>
    public interface IWorkflowProcessInstanceRepository 
    {
        /// <summary>
        ///��ȡ�ҵ�����
        /// </summary>
        /// <returns></returns>
        Task<PagedResults<WorkflowEngineFindHaveSendOutput>> FindHaveSeed(WorkflowEngineFindHaveSendInput input);

        /// <summary>
        ///��ȡ��ʱ����
        /// </summary>
        /// <returns></returns>
        Task<PagedResults<WorkflowEngineFindOverTimeOutput>> FindOverTime(WorkflowEngineFindHaveSendInput input);

        /// <summary>
        /// ��ȡ����������
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResults<WorkflowEngineFindNeedDoOutput>> FindNeedDo(WorkflowEngineFindNeedDoInput input);

        /// <summary>
        /// ��ȡ��������
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<WorkflowSearchNumOutput> FindNum(IdInput input);

        /// <summary>
        /// ��ȡ�Ѵ���
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResults<WorkflowEngineFindHaveDoOutput>> FindHaveDo(WorkflowEngineFindHaveDoInput input);

        /// <summary>
        ///��ȡ����б�
        /// </summary>
        /// <returns></returns>
        Task<PagedResults<WorkflowEngineFindMonitorListOuput>> FindMonitorList(WorkflowEngineFindMonitorListInput input);
        /// <summary>
        ///��ȡ����б�
        /// </summary>
        /// <returns></returns>
        Task<PagedResults<WorkflowEngineFindMonitorDeleteListOuput>> FindMonitorDeleteList(WorkflowEngineFindMonitorDeleteListInput input);
        /// <summary>
        /// ��ҳ��ѯ�ݸ���
        /// </summary>
        /// <param name="paging">��ҳ����</param>
        /// <returns></returns>
        Task<PagedResults<WorkflowEngineFindDraftOutput>> FindDraft(WorkflowEngineFindDraftInput paging);

        /// <summary>
        /// ��ҳ��ѯ������
        /// </summary>
        /// <param name="paging">��ҳ����</param>
        /// <returns></returns>
        Task<PagedResults<WorkflowEngineFindModelOutput>> FindModel(WorkflowEngineFindModelInput paging);

        /// <summary>
        ///����ʵ��Id��ȡ���Ϣ
        /// </summary>
        /// <returns></returns>
        Task<WorkflowEngineFindMonitorDetailOutput> FindMonitorDetial(IdInput input);
    }
}
