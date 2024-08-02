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

namespace EIP.Workflow.Repository
{
    /// <summary>
    /// 工作流归档数据访问接口
    /// </summary>
    public interface IWorkflowArchiveRepository 
    {
        /// <summary>
        /// 获取归档数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResults<WorkflowArchiveOutput>> Find(WorkflowArchiveInput input);
    }
}