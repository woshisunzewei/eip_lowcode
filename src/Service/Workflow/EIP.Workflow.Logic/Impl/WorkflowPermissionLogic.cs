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
namespace EIP.Workflow.Logic.Impl
{
    /// <summary>
    ///    工作流权限
    /// </summary>
    public class WorkflowPermissionLogic : DapperAsyncLogic<WorkflowPermission>, IWorkflowPermissionLogic
    {
        /// <summary>
        /// 保存工作流权限
        /// </summary>
        /// <param name="input">工作流权限</param>
        /// <returns></returns>
        public async Task<OperateStatus> Save(SystemPermissionSaveInput input)
        {
            //删除
            await DeleteAsync(d => d.PrivilegeMasterValue == input.PrivilegeMasterValue && d.PrivilegeMaster == input.PrivilegeMaster.GetHashCode());
            //批量插入
            IList<WorkflowPermission> workflowPermissions = new List<WorkflowPermission>();
            foreach (var item in input.Permissiones)
            {
                workflowPermissions.Add(new WorkflowPermission
                {
                    PrivilegeMasterValue = input.PrivilegeMasterValue,
                    PrivilegeMaster = (byte)input.PrivilegeMaster,
                    ProcessId = item
                });
            }
            return await BulkInsertAsync(workflowPermissions);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<WorkflowPermission>>> Find(SystemPermissionByPrivilegeMasterValueInput input)
        {
            return OperateStatus<IEnumerable<WorkflowPermission>>.Success(await FindAllAsync(f => f.PrivilegeMasterValue == input.PrivilegeMasterValue && f.PrivilegeMaster == input.PrivilegeMaster.GetHashCode()));
        }
    }
}