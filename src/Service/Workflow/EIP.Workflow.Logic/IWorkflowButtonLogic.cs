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
using EIP.Workflow.Models.Dtos.Button;

namespace EIP.Workflow.Logic
{
    /// <summary>
    /// 按钮定义
    /// </summary>
    public interface IWorkflowButtonLogic : IAsyncLogic<WorkflowButton>
    {
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="button">按钮信息</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IWorkflowButtonLogic_Cache")]
        Task<OperateStatus> Save(WorkflowButton button);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input">父级id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IWorkflowButtonLogic_Cache")]
        Task<OperateStatus> Delete(IdInput<string> input);

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "IWorkflowButtonLogic_Cache")]
        Task<OperateStatus<WorkflowButton>> FindById(IdInput input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "IWorkflowButtonLogic_Cache")]
        Task<OperateStatus<PagedResults<WorkflowButtonFindOutput>>> Find(WorkflowButtonFindInput input);
    }
}