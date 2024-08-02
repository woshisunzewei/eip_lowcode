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

namespace EIP.Workflow.Repository
{
    /// <summary>
    /// 工作流处理界面按钮接口定义
    /// </summary>
    public interface IWorkflowButtonRepository 
    {
        /// <summary>
        /// 根据流程类型获取流程信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResults<WorkflowButtonFindOutput>> Find(WorkflowButtonFindInput input);
    }
}