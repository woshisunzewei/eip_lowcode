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
namespace EIP.Workflow.Logic
{
    /// <summary>
    /// 工作流权限
    /// </summary>
    public interface IWorkflowPermissionLogic : IAsyncLogic<WorkflowPermission>
    {
        /// <summary>
        /// 保存按钮信息
        /// </summary>
        /// <param name="input">按钮信息</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "IWorkflowPermissionLogic_Cache")]
        Task<OperateStatus> Save(SystemPermissionSaveInput input);

        /// <summary>
        /// 保存按钮信息
        /// </summary>
        /// <param name="input">按钮信息</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "IWorkflowPermissionLogic_Cache")]
        Task<OperateStatus<IEnumerable<WorkflowPermission>>> Find(SystemPermissionByPrivilegeMasterValueInput input);
    }
}