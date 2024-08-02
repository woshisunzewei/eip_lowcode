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
using EIP.System.Models.Dtos.MenuButton;

namespace EIP.System.Repository
{
    /// <summary>
    /// 模块按钮
    /// </summary>
    public interface ISystemMenuButtonRepository
    {
        /// <summary>
        /// 根据模块获取功能项信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResults<SystemMenuButtonFindOutput>> Find(SystemMenuButtonFindInput input);

        /// <summary>
        /// 根据模块获取功能项信息
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SystemMenuButtonOutput>> FindHaveMenuButtonPermission(IdInput input);

        /// <summary>
        /// 根据模块获取功能项信息
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        Task<IEnumerable<SystemMenuButtonOutput>> FindMenuButtonByMenuId(IList<Guid> menuId = null);

        /// <summary>
        /// 根据模块Id和用户Id获取按钮权限数据
        /// </summary>
        /// <param name="viewRote"></param>
        /// <returns></returns>
        Task<IEnumerable<SystemMenuButtonByViewRote>> FindMenuButton(ViewRote viewRote);

    }
}