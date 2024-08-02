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
namespace EIP.System.Logic
{
    /// <summary>
    /// 组织机构
    /// </summary>
    public interface ISystemOrganizationLogic : IAsyncLogic<SystemOrganization>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<OperateStatus<IEnumerable<BaseTree>>> FindDataPermissionTree(SystemOrganizationDataPermissionInput input);

        /// <summary>
        /// 同步读取所有树数据
        /// </summary>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemOrganizationLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemOrganizationChartOutput>>> FindOrganizationAllTree();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="topOrg"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemOrganizationLogic_Cache")]
        Task<OperateStatus<IEnumerable<BaseTree>>> FindOrganizationTree(SystemOrganizationTreeInput input);

        /// <summary>
        /// 根据父级查询下级
        /// </summary>
        /// <param name="input">父级id</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemOrganizationLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemOrganizationOutput>>> Find(SystemOrganizationDataPermissionInput input);

        /// <summary>
        /// 获取具有数据权限的组织机构数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemOrganizationLogic_Cache")]
        Task<OperateStatus<IEnumerable<BaseTree>>> FindDataPermission(
            SystemOrganizationDataPermissionInput input);

        /// <summary>
        /// 保存组织机构
        /// </summary>
        /// <param name="input">组织机构</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemOrganizationLogic_Cache")]
        Task<OperateStatus> Save(SystemOrganization input);

        /// <summary>
        /// 删除组织机构下级数据
        /// </summary>
        /// <param name="input">父级id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemOrganizationLogic_Cache")]
        Task<OperateStatus> Delete(IdInput<string> input);

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="input">Id</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemOrganizationLogic_Cache")]
        Task<OperateStatus<SystemOrganization>> FindById(IdInput input);

        /// <summary>
        /// 冻结
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemOrganizationLogic_Cache")]
        Task<OperateStatus> IsFreeze(IdInput input);

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemOrganizationLogic_Cache")]
        Task<OperateStatus<List<string>>> Import(IList<SystemOrganizationImportDto> input);
    }
}