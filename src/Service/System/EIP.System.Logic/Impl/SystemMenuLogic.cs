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

namespace EIP.System.Logic.Impl
{
    /// <summary>
    /// 模块
    /// </summary>
    public class SystemMenuLogic : DapperAsyncLogic<SystemMenu>, ISystemMenuLogic
    {
        #region 构造函数
        private readonly ISystemMenuButtonLogic _menuButtonLogic;
        private readonly ISystemMenuRepository _menuRepository;
        private readonly ISystemPermissionLogic _permissionLogic;
        private readonly ISystemDataLogic _dataLogic;
        /// <summary>
        /// 模块
        /// </summary>
        /// <param name="menuRepository"></param>
        /// <param name="permissionLogic"></param>
        public SystemMenuLogic(
             ISystemMenuButtonLogic menuButtonLogic,
            ISystemMenuRepository menuRepository,
            ISystemDataLogic dataLogic,
            ISystemPermissionLogic permissionLogic)
        {
            _menuButtonLogic = menuButtonLogic;
            _permissionLogic = permissionLogic;
            _menuRepository = menuRepository;
            _dataLogic = dataLogic;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 根据状态为True的模块信息
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<BaseTree>>> Tree(bool? isAppMenu = null)
        {
            var systemMenus = isAppMenu.HasValue ? (await FindAllAsync(f => f.IsAgileMenu == isAppMenu)).OrderBy(o => o.OrderNo).ToList()
                : (await FindAllAsync()).OrderBy(o => o.OrderNo).ToList();
            var baseTrees = new List<BaseTree>();
            foreach (var item in systemMenus)
            {
                baseTrees.Add(new BaseTree
                {
                    id = item.MenuId,
                    parent = item.ParentId,
                    text = item.Name,
                    icon = item.Icon,
                    theme = item.Theme,
                    parents = item.ParentIds,
                    extend = new BaseTreeExtend
                    {
                        menuId = item.MenuId,
                        name = item.Name,
                        icon = item.Icon,
                        openType = item.OpenType,
                        iframePath = item.IframePath,
                        haveDataPermission = item.HaveDataPermission,
                        agileMenuType = item.AgileMenuType,
                        isShowMenu = item.IsShowMenu,
                        image = item.Image
                    }
                });
            }
            return OperateStatus<IEnumerable<BaseTree>>.Success(baseTrees);
        }

        /// <summary>
        /// 根据状态为True的模块信息
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemMenu>>> FindMeunuByPId(IdInput input)
        {
            return OperateStatus<IEnumerable<SystemMenu>>.Success((await FindAllAsync(f => f.ParentId == input.Id)).OrderBy(o => o.OrderNo));
        }

        /// <summary>
        /// 保存模块
        /// </summary>
        /// <param name="input">模块信息</param>
        /// <returns></returns>
        public async Task<OperateStatus<Guid>> Save(SystemMenu input)
        {
            OperateStatus<Guid> result = new OperateStatus<Guid>();
            OperateStatus operate;
            input.CanbeDelete = true;
            var menu = await FindAsync(f => f.MenuId == input.MenuId);
            var currentUser = EipHttpContext.CurrentUser();
            if (menu == null)
            {
                input.CreateTime = DateTime.Now;
                input.CreateUserId = currentUser.UserId;
                input.CreateUserName = currentUser.Name;
                input.UpdateTime = DateTime.Now;
                input.UpdateUserId = currentUser.UserId;
                input.UpdateUserName = currentUser.Name;

                operate = await InsertAsync(input);
            }
            else
            {
                input.Id = menu.Id;

                input.CreateTime = menu.CreateTime;
                input.CreateUserId = menu.CreateUserId;
                input.CreateUserName = menu.CreateUserName;

                input.UpdateTime = DateTime.Now;
                input.UpdateUserId = currentUser.UserId;
                input.UpdateUserName = currentUser.Name;
                operate = await UpdateAsync(input);
            }

            //更新Router
            var agileMenus = (await FindAllAsync(f => f.IsAgileMenu && f.AgileMenuType == 1)).ToList();
            for (int i = 0; i < agileMenus.Count(); i++)
            {
                var item = agileMenus[i];
                item.Router = "app" + (i + 1);
                item.UpdateTime = DateTime.Now;
                item.UpdateUserId = currentUser.UserId;
                item.UpdateUserName = currentUser.Name;
                await UpdateAsync(item);
            }

            input = await FindAsync(f => f.MenuId == input.MenuId);

            await GeneratingParentIds(input);
            result.Data = input.MenuId;
            result.Code = operate.Code;
            result.Msg = operate.Msg;

            return result;
        }
        /// <summary>
        /// 保存模块
        /// </summary>
        /// <param name="input">模块信息</param>
        /// <returns></returns>
        public async Task<OperateStatus<Guid>> SaveMove(SystemMenu input)
        {
            OperateStatus<Guid> result = new OperateStatus<Guid>();
            OperateStatus operate;
            input.CanbeDelete = true;
            var menu = await FindAsync(f => f.MenuId == input.MenuId);
            var currentUser = EipHttpContext.CurrentUser();

            menu.ParentId = input.ParentId;

            menu.UpdateTime = DateTime.Now;
            menu.UpdateUserId = currentUser.UserId;
            menu.UpdateUserName = currentUser.Name;
            operate = await UpdateAsync(menu);

            input = await FindAsync(f => f.MenuId == input.MenuId);

            await GeneratingParentIds(input);
            result.Data = input.MenuId;
            result.Code = operate.Code;
            result.Msg = operate.Msg;

            return result;
        }

        /// <summary>
        /// 批量生成代码
        /// </summary>
        /// <returns></returns>
        private async Task<OperateStatus> GeneratingParentIds(SystemMenu menu)
        {
            OperateStatus operateStatus = new OperateStatus();
            try
            {
                var menus = (await FindAllAsync()).ToList();
                var menuFind = menus.FirstOrDefault(w => w.MenuId == menu.ParentId);
                if (menuFind != null)
                {
                    menu.ParentIds = menuFind.ParentIds.IsNullOrEmpty()
                        ? menu.MenuId.ToString()
                        : menuFind.ParentIds + "," + menu.MenuId;
                    menu.ParentIdsName = menuFind.ParentIdsName.IsNullOrEmpty()
                        ? menu.Name
                        : menuFind.ParentIdsName + "/" + menu.Name;
                }
                else
                {
                    menu.ParentIds = menu.MenuId.ToString();
                    menu.ParentIdsName = menu.Name;
                }
                await UpdateAsync(menu);
                await GeneratingParentIds(menu, menus);
            }
            catch (Exception ex)
            {
                operateStatus.Msg = ex.Message;
                return operateStatus;
            }
            operateStatus.Msg = Chs.Successful;
            operateStatus.Code = ResultCode.Success;
            return operateStatus;
        }

        /// <summary>
        /// 递归获取代码
        /// </summary>
        /// <param name="menu"></param>
        /// <param name="menus"></param>
        private async Task GeneratingParentIds(SystemMenu menu, IList<SystemMenu> menus)
        {
            string parentIds = menu.ParentIds;
            string parentIdsName = menu.ParentIdsName;
            var next = menus.Where(w => w.ParentId == menu.MenuId).ToList();
            foreach (var m in next)
            {
                m.ParentIds = parentIds + "," + m.MenuId;
                m.ParentIdsName = parentIdsName + "/" + m.Name;
                m.ParentName = menu.Name;
                await UpdateAsync(m);
                await GeneratingParentIds(m, menus);
            }
        }

        /// <summary>
        /// 删除模块及下级数据
        /// </summary>
        /// <param name="input">父级id</param>
        /// <returns></returns>
        public async Task<OperateStatus> Delete(IdInput<string> input)
        {
            var operateStatus = new OperateStatus();
            foreach (var id in input.Id.Split(','))
            {
                Guid menuId = Guid.Parse(id);
                var menu = await FindAsync(f => f.MenuId == menuId);
                if (menu != null)
                {
                    if (!menu.CanbeDelete)
                    {
                        return OperateStatus.Error(Chs.CanotDelete);
                    }
                    var menuPrivilegeAccess = EnumPrivilegeAccess.模块权限.ToShort();
                    var menuButtonPrivilegeAccess = EnumPrivilegeAccess.模块按钮.ToShort();

                    MenuDeletGuid.Add(menuId);
                    await GetMenuDeleteGuid(menuId);
                    foreach (var delete in MenuDeletGuid)
                    {
                        await _permissionLogic.DeleteAsync(f => f.PrivilegeAccess == menuPrivilegeAccess && f.PrivilegeAccessValue == delete);
                        //删除对应按钮及按钮权限
                        var menubuttons = (await _menuButtonLogic.Find(new SystemMenuButtonFindInput { Id = delete })).Data;
                        foreach (var item in menubuttons.Data)
                        {
                            await _permissionLogic.DeleteAsync(f => f.PrivilegeAccess == menuButtonPrivilegeAccess && f.PrivilegeAccessValue == item.MenuButtonId);
                            await _menuButtonLogic.DeleteAsync(d => d.MenuButtonId == item.MenuButtonId);
                        }
                        //删除数据权限
                        var datas = await _dataLogic.FindAllAsync(f => f.MenuId == delete);
                        foreach (var item in datas)
                        {
                            await _permissionLogic.DeleteAsync(f => f.PrivilegeAccess == menuButtonPrivilegeAccess && f.PrivilegeAccessValue == item.DataId);
                            await _dataLogic.DeleteAsync(d => d.DataId == item.DataId);
                        }
                        await DeleteAsync(d => d.MenuId == delete);
                    }
                }
            }
            return OperateStatus.Success();
        }

        /// <summary>
        /// 删除主键集合
        /// </summary>
        public IList<Guid> MenuDeletGuid = new List<Guid>();

        /// <summary>
        /// 获取删除主键信息
        /// </summary>
        /// <param name="guid"></param>
        private async Task GetMenuDeleteGuid(Guid guid)
        {
            //获取下级
            var menus = (await FindAllAsync(f => f.ParentId == guid)).OrderBy(o => o.OrderNo).ToList();
            if (menus.Any())
            {
                foreach (var dic in menus)
                {
                    var menuId = dic.MenuId;
                    if (!menuId.IsEmptyGuid())
                    {
                        MenuDeletGuid.Add(menuId);
                        await GetMenuDeleteGuid(menuId);
                    }
                }
            }
        }

        /// <summary>
        /// 获取权限树菜单
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<BaseTree>>> FindPermissionMenu(EnumPrivilegeAccess privilegeAccess)
        {
            return OperateStatus<IEnumerable<BaseTree>>.Success(await _menuRepository.FindPermissionMenu(privilegeAccess));
        }

        /// <summary>
        /// 获取显示在模块列表上数据
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemMenuFindOutput>>> Find(SystemMenuFindInput input)
        {
            IList<SystemMenuFindOutput> outputs = new List<SystemMenuFindOutput>();
            var data = (await _menuRepository.Find(input)).ToList();
            foreach (var item in data)
            {
                item.Number = data.Count(c => c.ParentId == item.MenuId);
            }
            //将所有有父级的排在前面
            var parents = data.Where(w => w.Number > 0).OrderBy(o => o.OrderNo).ToList();
            var lasts = data.Where(w => w.Number == 0).OrderBy(o => o.OrderNo).ToList();
            foreach (var item in parents)
            {
                outputs.Add(item);
            }
            foreach (var item in lasts)
            {
                outputs.Add(item);
            }
            return OperateStatus<IEnumerable<SystemMenuFindOutput>>.Success(outputs);
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<SystemMenu>> FindById(IdInput input)
        {
            return OperateStatus<SystemMenu>.Success(await FindAsync(f => f.MenuId == input.Id));
        }

        /// <summary>
        /// 是否显示菜单
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus> IsShowMenu(IdInput input)
        {
            var menu = await FindAsync(f => f.MenuId == input.Id);
            menu.IsShowMenu = !menu.IsShowMenu;
            var result = await UpdateAsync(menu);
            //是否在流程里面
            using (var fix = new SqlDatabaseFixture())
            {
                var workflow = await fix.Db.WorkflowProcess.FindAsync(f => f.ProcessId == menu.MenuId);
                if (workflow != null)
                {
                    workflow.IsDelete = !menu.IsShowMenu;
                }
            }
            return result;
        }

        /// <summary>
        /// 是否具有模块权限
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus> HaveMenuPermission(IdInput input)
        {
            var menu = await FindAsync(f => f.MenuId == input.Id);
            menu.HaveMenuPermission = !menu.HaveMenuPermission;
            return await UpdateAsync(menu);
        }

        /// <summary>
        /// 是否具有数据权限
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus> HaveDataPermission(IdInput input)
        {
            var menu = await FindAsync(f => f.MenuId == input.Id);
            menu.HaveDataPermission = !menu.HaveDataPermission;
            return await UpdateAsync(menu);
        }

        /// <summary>
        /// 是否具有字段权限
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<OperateStatus> HaveFieldPermission(IdInput input)
        {
            var menu = await FindAsync(f => f.MenuId == input.Id);
            menu.HaveFieldPermission = !menu.HaveFieldPermission;
            return await UpdateAsync(menu);
        }

        /// <summary>
        /// 是否具有功能项权限
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus> HaveButtonPermission(IdInput input)
        {
            var menu = await FindAsync(f => f.MenuId == input.Id);
            menu.HaveButtonPermission = !menu.HaveButtonPermission;
            return await UpdateAsync(menu);
        }

        /// <summary>
        /// 冻结
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus> IsFreeze(IdInput input)
        {
            var menu = await FindAsync(f => f.MenuId == input.Id);
            menu.IsFreeze = !menu.IsFreeze;
            return await UpdateAsync(menu);
        }
        #endregion
    }
}