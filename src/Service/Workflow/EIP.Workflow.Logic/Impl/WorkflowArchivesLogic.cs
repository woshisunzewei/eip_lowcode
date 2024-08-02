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
using EIP.Workflow.Repository;

namespace EIP.Workflow.Logic.Impl
{
    /// <summary>
    ///  工作流处理界面归档接口实现
    /// </summary>
    public class WorkflowArchivesLogic : DapperAsyncLogic<WorkflowArchive>, IWorkflowArchivesLogic
    {
        private readonly IWorkflowArchiveRepository _workflowArchivesRepository;

        public WorkflowArchivesLogic(IWorkflowArchiveRepository workflowArchivesRepository)
        {
            _workflowArchivesRepository = workflowArchivesRepository;
        }

        /// <summary>
        /// 保存归档信息
        /// </summary>
        /// <param name="button">归档信息</param>
        /// <returns></returns>
        public async Task<OperateStatus> Save(WorkflowArchive button)
        {
            if (!button.ArchiveId.IsEmptyGuid())
                return await UpdateAsync(button);
            button.ArchiveId = CombUtil.NewComb();
            return await InsertAsync(button);
        }

        /// <summary>
        /// 获取归档信息
        /// </summary>
        /// <param name="input">归档信息</param>
        /// <returns></returns>
        public async Task<PagedResults<WorkflowArchiveOutput>> Find(WorkflowArchiveInput input)
        {
            return await _workflowArchivesRepository.Find(input);
        }
    }
}