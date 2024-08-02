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
using EIP.Workflow.Repository;

namespace EIP.Workflow.Logic.Impl
{
    /// <summary>
    /// 工作流意见数据访问接口实现
    /// </summary>
    public class WorkflowCommentLogic : DapperAsyncLogic<WorkflowComment>, IWorkflowCommentLogic
    {
        #region 构造函数

        private readonly IWorkflowCommentRepository _workflowCommentRepository;
        /// <summary>
        /// 
        /// </summary>
        public WorkflowCommentLogic(IWorkflowCommentRepository workflowCommentRepository)
        {
            _workflowCommentRepository = workflowCommentRepository;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 保存工作流意见信息
        /// </summary>
        /// <param name="comment">工作流意见信息</param>
        /// <returns></returns>
        public async Task<OperateStatus> Save(WorkflowComment comment)
        {
            var commentGet = await FindAsync(f => f.CommentId == comment.CommentId);
            var currentUser = EipHttpContext.CurrentUser();
            if (commentGet != null)
            {
                commentGet.UpdateTime = DateTime.Now;
                commentGet.UpdateUserId = comment.CreateUserId;
                commentGet.UpdateUserName = comment.CreateUserName;
                commentGet.Content = comment.Content;
                commentGet.OrderNo = comment.OrderNo;
                return await UpdateAsync(commentGet);
            }

            comment.CreateTime = DateTime.Now;
            comment.CreateUserId = currentUser.UserId;
            comment.CreateUserName = currentUser.Name;
            comment.UpdateTime = DateTime.Now;
            comment.UpdateUserId = currentUser.UserId;
            comment.UpdateUserName = currentUser.Name;

            comment.CommentId = CombUtil.NewComb();
            return await InsertAsync(comment);
        }

        /// <summary>
        /// 根据Id删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> Delete(IdInput<string> input)
        {
            OperateStatus operateStatus = new OperateStatus();
            foreach (var item in input.Id.Split(','))
            {
                operateStatus = await DeleteAsync(f => f.CommentId == Guid.Parse(item));
            }
            return operateStatus;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<OperateStatus<PagedResults<WorkflowCommentFindOutput>>> Find(WorkflowCommentFindInput input)
        {
            return OperateStatus<PagedResults<WorkflowCommentFindOutput>>.Success(await _workflowCommentRepository.Find(input));
        }

        /// <summary>
        /// 根据用户Id查询对应的意见
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<WorkflowComment>>> FindByUserId(IdInput input)
        {
            return OperateStatus<IEnumerable<WorkflowComment>>.Success((await FindAllAsync(f => f.CreateUserId == input.Id || f.Type == 0)).OrderBy(o => o.OrderNo));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<OperateStatus<WorkflowComment>> FindById(IdInput input)
        {
            return OperateStatus<WorkflowComment>.Success(await FindAsync(f => f.CommentId == input.Id));
        }
        #endregion
    }
}