/**************************************************************
* Copyright (C) 2022 www.eipflow.com ����ΰ��Ȩ����(����ؾ�)
*
* ����: ����ΰ(QQ 1039318332)
* ����ʱ��: 2022/01/12 22:40:15
* �ļ���: 
* ����: 
* 
* �޸���ʷ
* �޸��ˣ�
* ʱ�䣺
* �޸�˵����
*
**************************************************************/
using EIP.System.Models.Dtos.Menu;
using EIP.System.Models.Dtos.MenuButton;

namespace EIP.System.Logic.Impl
{
    /// <summary>
    /// ģ��
    /// </summary>
    public class SystemMenuLogic : DapperAsyncLogic<SystemMenu>, ISystemMenuLogic
    {
        #region ���캯��
        private readonly ISystemMenuButtonLogic _menuButtonLogic;
        private readonly ISystemMenuRepository _menuRepository;
        private readonly ISystemPermissionLogic _permissionLogic;
        private readonly ISystemDataLogic _dataLogic;
        /// <summary>
        /// ģ��
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

        #region ����

        /// <summary>
        /// ����״̬ΪTrue��ģ����Ϣ
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
        /// ����״̬ΪTrue��ģ����Ϣ
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemMenu>>> FindMeunuByPId(IdInput input)
        {
            return OperateStatus<IEnumerable<SystemMenu>>.Success((await FindAllAsync(f => f.ParentId == input.Id)).OrderBy(o => o.OrderNo));
        }

        /// <summary>
        /// ����ģ��
        /// </summary>
        /// <param name="input">ģ����Ϣ</param>
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

            //����Router
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
        /// ����ģ��
        /// </summary>
        /// <param name="input">ģ����Ϣ</param>
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
        /// �������ɴ���
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
        /// �ݹ��ȡ����
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
        /// ɾ��ģ�鼰�¼�����
        /// </summary>
        /// <param name="input">����id</param>
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
                    var menuPrivilegeAccess = EnumPrivilegeAccess.ģ��Ȩ��.ToShort();
                    var menuButtonPrivilegeAccess = EnumPrivilegeAccess.ģ�鰴ť.ToShort();

                    MenuDeletGuid.Add(menuId);
                    await GetMenuDeleteGuid(menuId);
                    foreach (var delete in MenuDeletGuid)
                    {
                        await _permissionLogic.DeleteAsync(f => f.PrivilegeAccess == menuPrivilegeAccess && f.PrivilegeAccessValue == delete);
                        //ɾ����Ӧ��ť����ťȨ��
                        var menubuttons = (await _menuButtonLogic.Find(new SystemMenuButtonFindInput { Id = delete })).Data;
                        foreach (var item in menubuttons.Data)
                        {
                            await _permissionLogic.DeleteAsync(f => f.PrivilegeAccess == menuButtonPrivilegeAccess && f.PrivilegeAccessValue == item.MenuButtonId);
                            await _menuButtonLogic.DeleteAsync(d => d.MenuButtonId == item.MenuButtonId);
                        }
                        //ɾ������Ȩ��
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
        /// ɾ����������
        /// </summary>
        public IList<Guid> MenuDeletGuid = new List<Guid>();

        /// <summary>
        /// ��ȡɾ��������Ϣ
        /// </summary>
        /// <param name="guid"></param>
        private async Task GetMenuDeleteGuid(Guid guid)
        {
            //��ȡ�¼�
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
        /// ��ȡȨ�����˵�
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<BaseTree>>> FindPermissionMenu(EnumPrivilegeAccess privilegeAccess)
        {
            return OperateStatus<IEnumerable<BaseTree>>.Success(await _menuRepository.FindPermissionMenu(privilegeAccess));
        }

        /// <summary>
        /// ��ȡ��ʾ��ģ���б�������
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
            //�������и���������ǰ��
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
        /// ����Id��ȡ
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<SystemMenu>> FindById(IdInput input)
        {
            return OperateStatus<SystemMenu>.Success(await FindAsync(f => f.MenuId == input.Id));
        }

        /// <summary>
        /// �Ƿ���ʾ�˵�
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus> IsShowMenu(IdInput input)
        {
            var menu = await FindAsync(f => f.MenuId == input.Id);
            menu.IsShowMenu = !menu.IsShowMenu;
            var result = await UpdateAsync(menu);
            //�Ƿ�����������
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
        /// �Ƿ����ģ��Ȩ��
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus> HaveMenuPermission(IdInput input)
        {
            var menu = await FindAsync(f => f.MenuId == input.Id);
            menu.HaveMenuPermission = !menu.HaveMenuPermission;
            return await UpdateAsync(menu);
        }

        /// <summary>
        /// �Ƿ��������Ȩ��
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus> HaveDataPermission(IdInput input)
        {
            var menu = await FindAsync(f => f.MenuId == input.Id);
            menu.HaveDataPermission = !menu.HaveDataPermission;
            return await UpdateAsync(menu);
        }

        /// <summary>
        /// �Ƿ�����ֶ�Ȩ��
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
        /// �Ƿ���й�����Ȩ��
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus> HaveButtonPermission(IdInput input)
        {
            var menu = await FindAsync(f => f.MenuId == input.Id);
            menu.HaveButtonPermission = !menu.HaveButtonPermission;
            return await UpdateAsync(menu);
        }

        /// <summary>
        /// ����
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