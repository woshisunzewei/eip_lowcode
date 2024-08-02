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

namespace EIP.Workflow.Logic
{
    /// <summary>
    /// 工作流处理归档接口定义
    /// </summary>
    public interface IWorkflowArchivesLogic : IAsyncLogic<WorkflowArchive>
    {
        /// <summary>
        /// 保存归档信息
        /// </summary>
        /// <param name="input">归档信息</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IWorkflowArchivesLogic_Cache")]
        Task<OperateStatus> Save(WorkflowArchive input);

        /// <summary>
        /// 获取归档数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "IWorkflowArchivesLogic_Cache")]
        Task<PagedResults<WorkflowArchiveOutput>> Find(WorkflowArchiveInput input);
    }
}