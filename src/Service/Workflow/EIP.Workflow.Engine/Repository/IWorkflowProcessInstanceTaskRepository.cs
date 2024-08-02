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
    public interface IWorkflowProcessInstanceTaskRepository
    {
        /// <summary>
        /// 获取流程实例过程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IEnumerable<WorkflowEngineFindInstanceProcessOutput>> FindInstanceProcess(WorkflowEngineFindInstanceProcessInput input);

        /// <summary>
        /// 获取流程实例过程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IEnumerable<WorkflowProcessInstanceTask>> FindProcessInstanceTaskByPrevTaskId(IdInput input);
    }
}
