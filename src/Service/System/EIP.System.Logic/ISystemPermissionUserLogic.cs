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
    /// 权限用户业务逻辑接口
    /// </summary>
    public interface ISystemPermissionUserLogic : IAsyncLogic<SystemPermissionUser>
    {
        /// <summary>
        /// 保存各种用户:组织机构、岗位、组、人员
        /// </summary>
        /// <param name="master">类型</param>
        /// <param name="value">业务表Id：如组织机构Id、组Id、岗位Id、人员Id等</param>
        /// <param name="userIds">权限类型:组织机构、组、岗位、人员Id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemPermissionUserLogic_Cache")]
        Task<OperateStatus> SavePermissionUser(EnumPrivilegeMaster master,
            Guid value,
            IList<Guid> userIds);

        /// <summary>
        /// 保存各种用户:组织机构、岗位、组、人员【先进行删除,再进行添加】
        /// </summary>
        /// <param name="master">类型</param>
        /// <param name="value">业务表Id：如组织机构Id、组Id、岗位Id、人员Id等</param>
        /// <param name="userIds">权限类型:组织机构、组、岗位、人员Id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemPermissionUserLogic_Cache")]
        Task<OperateStatus> SavePermissionUserBeforeDelete(EnumPrivilegeMaster master,
            Guid value,
            IList<Guid> userIds);

        /// <summary>
        /// 保存用户权限类型
        /// </summary>
        /// <param name="privilegeMaster">类型</param>
        /// <param name="privilegeMasterUserId">业务表Id：如组织机构Id、组Id、岗位Id、人员Id等</param>
        /// <param name="privilegeMasterValue">权限类型:角色Id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemPermissionUserLogic_Cache")]
        Task<OperateStatus> SavePermissionMasterValueBeforeDelete(EnumPrivilegeMaster privilegeMaster,
            Guid privilegeMasterUserId,
            IList<Guid> privilegeMasterValue);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="privilegeMasterUserId">用户Id</param>
        /// <param name="privilegeMasterValue">归属类型Id:组织机构、角色、岗位、组</param>
        /// <param name="privilegeMaster">归属人员类型:组织机构、角色、岗位、组</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemPermissionUserLogic_Cache")]
        Task<OperateStatus> DeletePrivilegeMasterUser(Guid privilegeMasterUserId,
            Guid privilegeMasterValue,
            EnumPrivilegeMaster privilegeMaster);

        /// <summary>
        /// 删除用户对应权限数据
        /// </summary>
        /// <param name="privilegeMasterUserId">用户Id</param>
        /// <param name="privilegeMaster">归属人员类型:组织机构、角色、岗位、组</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemPermissionUserLogic_Cache")]
        Task<OperateStatus> DeletePrivilegeMasterUser(Guid privilegeMasterUserId,
            EnumPrivilegeMaster privilegeMaster);
        /// <summary>
        /// 删除用户对应权限数据
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemPermissionUserLogic_Cache")]
        Task<OperateStatus> DeletePrivilegeMasterUser(Guid userId);
        /// <summary>
        /// 获取模块、字段对应拥有者信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemPermissionUserLogic_Cache")]
        Task<OperateStatus<SystemPrivilegeDetailOutput>> FindSystemPrivilegeDetailOutputsByAccessAndValue(SystemPrivilegeDetailInput input);

        /// <summary>
        /// 根据特权类型及特权id获取特权用户信息
        /// </summary>
        /// <param name="privilegeMaster">特权类型</param>
        /// <param name="privilegeMasterValue">特权id</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemPermissionUserLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemPermissionUser>>> FindPermissionUsersByPrivilegeMasterAdnPrivilegeMasterValue(
            EnumPrivilegeMaster privilegeMaster,
            Guid? privilegeMasterValue = null);
    }
}