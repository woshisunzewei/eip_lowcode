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
namespace EIP.System.Repository
{
    /// <summary>
    /// 组织机构
    /// </summary>
    public interface ISystemOrganizationRepository 
    {
        /// <summary>
        /// 获取具有数据权限的组织机构数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IEnumerable<BaseTree>> FindDataPermissionTree(SystemOrganizationDataPermissionInput input);

        /// <summary>
        /// 根据父级查询下级
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IEnumerable<SystemOrganizationOutput>> FindByParentId(SystemOrganizationDataPermissionInput input);
    }
}