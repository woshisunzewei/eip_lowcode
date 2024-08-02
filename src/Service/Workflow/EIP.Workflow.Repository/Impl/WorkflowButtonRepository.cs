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
using System.Text;

namespace EIP.Workflow.Repository.Impl
{
    /// <summary>
    /// 工作流按钮接口实现
    /// </summary>
    public class WorkflowButtonRepository : IWorkflowButtonRepository
    {
        /// <summary>
        /// 根据流程类型获取流程信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<PagedResults<WorkflowButtonFindOutput>> Find(WorkflowButtonFindInput input)
        {
            var sql = new StringBuilder(@"SELECT 
                                              ButtonId,
                                              CreateTime,
                                              CreateUserName,
                                              UpdateTime,
                                              UpdateUserName,
                                              Name,
                                              Icon,
                                              Script,
                                              Remark,
                                              OrderNo,
                                              MobileShow,
                                              Style,
                                              @rowNumber, @recordCount FROM Workflow_Button @where ");
            
            if (input.Sidx.IsNullOrEmpty())
            {
                input.Sidx = " OrderNo";
            }
            return new SqlMapperUtil().PagingQuerySqlAsync<WorkflowButtonFindOutput>(sql.ToString(), input);
        }
    }
}