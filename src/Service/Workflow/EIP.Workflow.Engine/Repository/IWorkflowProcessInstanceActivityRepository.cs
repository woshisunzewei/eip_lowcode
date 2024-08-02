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
    public interface IWorkflowProcessInstanceActivityRepository 
    {

        /// <summary>
        /// 根据实例Id获取对应活动信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IEnumerable<WorkflowEngineFindActivityByProcessIdAndTypeOutput>> FindActivityByProcessIdAndType(WorkflowEngineFindActivityByProcessIdAndTypeInput input);

        /// <summary>
        ///    根据ActivityId获取该活动配置的按钮。
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IEnumerable<WorkflowButton>> FindProcessButtonByActivity(IdInput input);

        /// <summary>
        /// 获取流程实例按钮
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IEnumerable<WorkflowButton>> FindProcessInstanceButtonByActivity(IdInput input);

        /// <summary>
        /// 判断条件是否成立
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
        /// 连线检测条件
        /// </summary>
        /// <returns></returns>
        Task<bool> CheckConditionLink(WorkflowEngineConditionInput input);

        /// <summary>
        /// 根据活动Id获取活动信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<WorkflowEngineFindActivityByTaskIdOutput> FindActivityByTaskId(IdInput input);

        /// <summary>
        /// 根据实例Id获取表单信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<WorkflowEngineFindFormByProcessInstanceIdOutput> FindWorkflowProcessFormByProcessInstanceId(IdInput input);

        /// <summary>
        /// 获取退回活动
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IEnumerable<WorkflowEngineFindReturnActivityOutput>> FindReturnActivity(WorkflowEngineFindReturnActivityInput input);

        /// <summary>
        /// 获取流程实例活动
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IEnumerable<WorkflowEngineFindReturnDoUserOutput>> FindReturnDoUser(WorkflowEngineFindReturnActivityInput input);

    }
}
