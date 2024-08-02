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
using System.Text;

namespace EIP.Workflow.Repository.Impl
{
    /// <summary>
    /// 工作流意见数据访问接口实现
    /// </summary>
    public class WorkflowCommentRepository : IWorkflowCommentRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResults<WorkflowCommentFindOutput>> Find(WorkflowCommentFindInput input)
        {
            var sql = new StringBuilder(@"SELECT 
                                              CommentId,
                                              Content,
                                              Type,
                                              OrderNo,
                                              CreateTime,
                                              CreateUserName,
                                              @rowNumber, @recordCount FROM workflow_comment @where ");
            if (input.UserId.HasValue)
            {
                sql.Append($" and CreateUserId='{input.UserId}'");
            }
            if (input.Sidx.IsNullOrEmpty())
            {
                input.Sidx = " OrderNo";
            }
            return new SqlMapperUtil().PagingQuerySqlAsync<WorkflowCommentFindOutput>(sql.ToString(), input);
        }
    }
}