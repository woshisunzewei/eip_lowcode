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
using EIP.Workflow.Repository;

namespace EIP.Workflow.Logic.Impl
{
    /// <summary>
    /// 工作流处理界面按钮接口实现
    /// </summary>
    public class WorkflowButtonLogic : DapperAsyncLogic<WorkflowButton>, IWorkflowButtonLogic
    {
        #region 构造函数

        private readonly IWorkflowButtonRepository _workflowButtonRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workflowButtonRepository"></param>
        public WorkflowButtonLogic(IWorkflowButtonRepository workflowButtonRepository)
        {
            _workflowButtonRepository = workflowButtonRepository;
        }

        #endregion

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
                operateStatus = await DeleteAsync(f => f.ButtonId == Guid.Parse(item));
            }
            return operateStatus;
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<WorkflowButton>> FindById(IdInput input)
        {
            return OperateStatus<WorkflowButton>.Success(await FindAsync(f => f.ButtonId == input.Id));
        }

        /// <summary>
        /// 获取分页按钮
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<PagedResults<WorkflowButtonFindOutput>>> Find(WorkflowButtonFindInput input)
        {
            return OperateStatus<PagedResults<WorkflowButtonFindOutput>>.Success(await _workflowButtonRepository.Find(input));
        }

        /// <summary>
        /// 保存按钮信息
        /// </summary>
        /// <param name="button">按钮信息</param>
        /// <returns></returns>
        public async Task<OperateStatus> Save(WorkflowButton button)
        {
            var editButton = await FindAsync(f => f.ButtonId == button.ButtonId);
            var currentUser = EipHttpContext.CurrentUser();
            if (editButton != null)
            {
                button.Id = editButton.Id;
                button.UpdateTime = DateTime.Now;
                button.UpdateUserId = currentUser.UserId;
                button.UpdateUserName = currentUser.Name;
                return await UpdateAsync(button);
            }
            button.ButtonId = CombUtil.NewComb();
            button.CreateTime = DateTime.Now;
            button.CreateUserId = currentUser.UserId;
            button.CreateUserName = currentUser.Name;

            button.UpdateTime = DateTime.Now;
            button.UpdateUserId = currentUser.UserId;
            button.UpdateUserName = currentUser.Name;
            return await InsertAsync(button);
        }
    }
}