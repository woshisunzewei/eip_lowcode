/**************************************************************
* Copyright (C) 2022 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2022/01/12 22:40:15
* 文件名: 
* 描述: 
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
namespace EIP.Workflow.Repository
{
    /// <summary>
    /// 工作流处理界面按钮接口定义
    /// </summary>
    public interface IWorkflowProcessRepository 
    {
        /// <summary>
        /// 根据权限Id获取所有流程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IEnumerable<WorkflowProcess>> FindSendLibrary(IdInput input);

        /// <summary>
        /// 根据权限Id获取所有流程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IEnumerable<WorkflowProcess>> FindAllProcess();

        /// <summary>
        /// 根据流程类型获取流程信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResults<WorkflowProcessFindOutput>> Find(WorkflowProcessFindInput input);
    }
}