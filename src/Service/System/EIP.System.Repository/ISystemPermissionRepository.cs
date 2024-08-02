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
using EIP.System.Models.Dtos.Menu;
using EIP.System.Models.Dtos.MenuButton;

namespace EIP.System.Repository
{
    /// <summary>
    /// 权限
    /// </summary>
    public interface ISystemPermissionRepository
    {
        /// <summary>
        /// 根据权限归属Id查询模块权限信息
        /// </summary>
        /// <param name="input">权限类型:模块、功能项、数据、字段、文件</param>
        /// <returns></returns>
        Task<IEnumerable<SystemPermission>> FindPermissionByPrivilegeMasterValue(SystemPermissionByPrivilegeMasterValueInput input);

        /// <summary>
        /// 根据用户Id获取用户具有的模块权限
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IEnumerable<SystemPermissionFindMenuByUserIdOutput>> FindSystemPermissionMenuByUserId(SystemPermissionMenuInput input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IEnumerable<SystemMenuFindAgileOutput>> FindSystemPermissionMenuAgileByUserId(SystemPermissionMenuInput input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="menuId"></param>
        /// <returns></returns>
        Task<IEnumerable<SystemMenuButtonByViewRote>> FindMenuButton(Guid userId, string menuId);

        /// <summary>
        /// 根据用户Id获取用户具有的模块权限
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SystemPermissionFindMenuByUserIdOutput>> FindSystemPermissionMenuByAdmin(SystemPermissionMenuInput input);

        /// <summary>
        /// 根据用户Id获取用户具有的模块权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<BaseTree>> FindSystemPermissionMenuAllByUserId(Guid userId);
        /// <summary>
        /// 根据用户Id获取用户具有的模块权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<BaseTree>> FindSystemPermissionMobileMenuAllByUserId(Guid userId);

        /// <summary>
        /// 根据角色Id,岗位Id,组Id,人员Id获取具有的模块信息
        /// </summary>
        /// <param name="input">输入参数</param>
        /// <returns>树形模块信息</returns>
        Task<IEnumerable<BaseTree>> FindMenuHavePermissionByPrivilegeMasterValue(SystemPermissiontMenuHaveByPrivilegeMasterValueInput input);
        /// <summary>
        /// 根据角色Id,岗位Id,组Id,人员Id获取具有的模块信息
        /// </summary>
        /// <param name="input">输入参数</param>
        /// <returns>树形模块信息</returns>
        Task<IEnumerable<BaseTree>> FindMobileMenuHavePermissionByPrivilegeMasterValue(SystemPermissiontMenuHaveByPrivilegeMasterValueInput input);

        /// <summary>
        /// 获取数据权限Sql
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IEnumerable<SystemData>> FindDataPermission(ViewRote input);
    }
}