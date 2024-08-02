/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
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
using EIP.Workflow.Models.Dtos.Comment;

namespace EIP.Workflow.Logic
{
    /// <summary>
    /// 工作流意见数据访问接口
    /// </summary>
    public interface IWorkflowCommentLogic : IAsyncLogic<WorkflowComment>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "IWorkflowCommentLogic_Cache")]
        Task<OperateStatus<PagedResults<WorkflowCommentFindOutput>>> Find(WorkflowCommentFindInput input);

        /// <summary>
        /// 根据用户Id查询对应的意见
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "IWorkflowCommentLogic_Cache")]
        Task<OperateStatus<IEnumerable<WorkflowComment>>> FindByUserId(IdInput input);

        /// <summary>
        /// 保存工作流意见信息
        /// </summary>
        /// <param name="comment">工作流意见信息</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IWorkflowCommentLogic_Cache")]
        Task<OperateStatus> Save(WorkflowComment comment);

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "IWorkflowCommentLogic_Cache")]
        Task<OperateStatus<WorkflowComment>> FindById(IdInput input);

        /// <summary>
        /// 删除工作流意见信息
        /// </summary>
        /// <param name="input">工作流意见信息</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IWorkflowCommentLogic_Cache")]
        Task<OperateStatus> Delete(IdInput<string> input);
    }
}