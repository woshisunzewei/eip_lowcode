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
using EIP.System.Models.Dtos.Data;
using EIP.System.Models.Dtos.Menu;
using EIP.System.Models.Dtos.MenuButton;

namespace EIP.System.Logic.Impl
{
    /// <summary>
    /// 权限业务逻辑
    /// </summary>
    public class SystemPermissionLogic : DapperAsyncLogic<SystemPermission>, ISystemPermissionLogic
    {
        #region 依赖注入
        private readonly ISystemMenuButtonRepository _menuButtonRepository;
        private readonly ISystemPermissionRepository _permissionRepository;
        private readonly ISystemDataRepository _dataRepository;
        /// <summary>
        /// 权限逻辑
        /// </summary>
        /// <param name="menuButtonRepository"></param>
        /// <param name="permissionRepository"></param>
        /// <param name="dataRepository"></param>
        public SystemPermissionLogic(
            ISystemMenuButtonRepository menuButtonRepository,
            ISystemPermissionRepository permissionRepository,
            ISystemDataRepository dataRepository)
        {
            _menuButtonRepository = menuButtonRepository;
            _permissionRepository = permissionRepository;
            _dataRepository = dataRepository;
        }
        #endregion

        #region 方法

        /// <summary>
        /// 根据状态为True的模块信息
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemPermission>>> FindPermissionByPrivilegeMasterValue(SystemPermissionByPrivilegeMasterValueInput input)
        {
            return OperateStatus<IEnumerable<SystemPermission>>.Success((await _permissionRepository.FindPermissionByPrivilegeMasterValue(input)).ToList());
        }

        /// <summary>
        /// 保存权限信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> SavePermission(SystemPermissionSaveInput input)
        {
            using (var fixture = new SqlDatabaseFixture())
            {
                var operateStatus = new OperateStatus();
                try
                {
                    IList<SystemPermission> systemPermissions = input.Permissiones.Select(per => new SystemPermission
                    {
                        PrivilegeAccess = input.PrivilegeAccess.ToShort(),
                        PrivilegeAccessValue = per,
                        PrivilegeMasterValue = input.PrivilegeMasterValue,
                        PrivilegeMaster = input.PrivilegeMaster.ToShort(),
                        PrivilegeMenuId = input.PrivilegeMenuId
                    }).ToList();

                    if (!input.PrivilegeMenuId.IsNullOrEmptyGuid())
                    {
                        await fixture.Db.SystemPermission.DeleteAsync(f => f.PrivilegeMasterValue == input.PrivilegeMasterValue && f.PrivilegeAccess == input.PrivilegeAccess.ToShort() && f.PrivilegeMenuId == input.PrivilegeMenuId);
                    }
                    else
                    {
                        await fixture.Db.SystemPermission.DeleteAsync(f => f.PrivilegeMasterValue == input.PrivilegeMasterValue && f.PrivilegeAccess == input.PrivilegeAccess.ToShort());
                    }

                    if (input.PrivilegeMaster == EnumPrivilegeMaster.人员)
                    {
                        var master = input.PrivilegeMaster.ToShort();
                        //删除对应人员数据
                        await fixture.Db.SystemPermissionUser.DeleteAsync(d => d.PrivilegeMaster == master && d.PrivilegeMasterValue == input.PrivilegeMasterValue);
                        //判断是否具有权限
                        if (!systemPermissions.Any())
                        {
                            operateStatus.Code = ResultCode.Success;
                            operateStatus.Msg = Chs.Successful;
                            return operateStatus;
                        }

                        //插入权限人员数据
                        var permissionUser = new SystemPermissionUser
                        {
                            PrivilegeMaster = (byte)input.PrivilegeMaster,
                            PrivilegeMasterUserId = input.PrivilegeMasterValue,
                            PrivilegeMasterValue = input.PrivilegeMasterValue
                        };
                        await fixture.Db.SystemPermissionUser.InsertAsync(permissionUser);
                    }

                    //是否具有权限数据
                    if (!systemPermissions.Any())
                    {
                        operateStatus.Code = ResultCode.Success;
                        operateStatus.Msg = Chs.Successful;
                        return operateStatus;
                    }

                    //插入数据库
                    await fixture.Db.SystemPermission.BulkInsertAsync(systemPermissions);
                    operateStatus.Code = ResultCode.Success;
                    operateStatus.Msg = Chs.Successful;
                    return operateStatus;
                }
                catch (Exception ex)
                {
                    operateStatus.Msg = ex.Message;
                    return operateStatus;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemPermissionFindMenuByUserIdOutput>>> MenusTree(SystemPermissionMenuInput input)
        {
            IList<SystemPermissionFindMenuByUserIdOutput> treeEntities = new List<SystemPermissionFindMenuByUserIdOutput>();
            //判断该用户是否为超级管理员:若是超级管理员则显示所有模块
            var userInfo = EipHttpContext.CurrentUser();
            //如果是超级管理员
            if (userInfo.IsAdmin)
            {
                treeEntities = (await _permissionRepository.FindSystemPermissionMenuByAdmin(input)).ToList();
            }
            else
            {
                treeEntities = (await _permissionRepository.FindSystemPermissionMenuByUserId(input)).ToList();
            }
            return OperateStatus<IEnumerable<SystemPermissionFindMenuByUserIdOutput>>.Success(treeEntities);
        }
        /// <summary>
        /// 根据用户Id获取用户具有的模块权限
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemPermissionFindMenuByUserIdRouterOutput>>> Menus(SystemPermissionMenuInput input)
        {
            try
            {
                List<SystemPermissionFindMenuByUserIdRouterOutput> outputs = new List<SystemPermissionFindMenuByUserIdRouterOutput>();
                IList<SystemPermissionFindMenuByUserIdOutput> treeEntities = new List<SystemPermissionFindMenuByUserIdOutput>();
                //判断该用户是否为超级管理员:若是超级管理员则显示所有模块
                var userInfo = EipHttpContext.CurrentUser();
                //如果是超级管理员
                if (userInfo.IsAdmin)
                {
                    treeEntities = (await _permissionRepository.FindSystemPermissionMenuByAdmin(input)).ToList();
                }
                else
                {
                    treeEntities = (await _permissionRepository.FindSystemPermissionMenuByUserId(input)).ToList();
                }
                //解析路由方式
                if (treeEntities.Any())
                {
                    RSACryptoService loginRsa = new RSACryptoService(ResourceKey.加密私钥, ResourceKey.加密公钥);
                    var decryptUserId = loginRsa.Encrypt(userInfo.UserId.ToString()).Replace("+", "%2B");
                    foreach (var data in treeEntities.Where(w => w.ParentId == null || w.ParentId.ToString().IsNullOrEmpty()))
                    {
                        var path = data.Path.IsNullOrEmpty() ? "" : data.Path.Replace("{userId}", decryptUserId);
                        SystemPermissionFindMenuByUserIdRouterOutput output = new SystemPermissionFindMenuByUserIdRouterOutput
                        {
                            router = data.Router,
                            name = data.Text,
                            path = path,
                            meta = new SystemPermissionFindMenuByUserIdRouterMetaOutput
                            {
                                menuId = data.Id,
                                icon = data.Icon,
                                theme = data.Theme,
                                @params = data.Params.IsNotNullOrEmpty() ? JsonConvert.DeserializeObject(data.Params) : null,
                                link = data.OpenType == 2 ? path : "",
                                image = data.Image,
                                count = 0
                            }
                        };
                        output.children = FindChildren(treeEntities.ToList(), data, decryptUserId);
                        outputs.Add(output);
                    }
                }
                return OperateStatus<IEnumerable<SystemPermissionFindMenuByUserIdRouterOutput>>.Success(outputs);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// 根据用户Id获取用户具有的模块权限
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemMenuFindAgileOutput>>> MenusAgile(SystemPermissionMenuInput input)
        { //判断该用户是否为超级管理员:若是超级管理员则显示所有模块
            var userInfo = EipHttpContext.CurrentUser();
            //如果是超级管理员
            if (userInfo.IsAdmin)
            {
                using (var fix = new SqlDatabaseFixture())
                {
                    var agile = new List<SystemMenuFindAgileOutput>();

                    if (input.ConfigId.HasValue)
                    {
                        agile = (await fix.Db.SystemAgile.SetSelect(s => new { s.ConfigId, s.PublicJson, s.ColumnJson, s.ConfigType, s.Name, s.Remark }).FindAllAsync(f => f.ConfigId == input.ConfigId)).MapToList<SystemAgile, SystemMenuFindAgileOutput>();
                    }
                    else
                    {
                        agile = (await fix.Db.SystemAgile.SetSelect(s => new { s.ConfigId, s.PublicJson, s.ColumnJson, s.ConfigType, s.Name, s.Remark }).FindAllAsync()).MapToList<SystemAgile, SystemMenuFindAgileOutput>();
                    }
                    return OperateStatus<IEnumerable<SystemMenuFindAgileOutput>>.Success(agile);
                }
            }
            else
            {
                var menuAgile = new List<SystemMenuFindAgileOutput>();
                if (input.ConfigId.HasValue)
                {
                    using (var fix = new SqlDatabaseFixture())
                    {
                        var agile = (await fix.Db.SystemAgile.SetSelect(s => new { s.ConfigId, s.PublicJson, s.ColumnJson, s.ConfigType, s.Name, s.Remark }).FindAllAsync(f => f.ConfigId == input.ConfigId)).MapToList<SystemAgile, SystemMenuFindAgileOutput>();
                        foreach (var item in agile)
                        {
                            menuAgile.Add(new SystemMenuFindAgileOutput
                            {
                                ConfigId = item.ConfigId,
                                ColumnJson = item.ColumnJson,
                                ConfigType = item.ConfigType,
                                Name = item.Name,
                                PublicJson = item.PublicJson,
                                Remark = item.Remark,
                                MenuId = item.MenuId,
                            });
                        }
                    }
                }
                else
                {
                    menuAgile = (await _permissionRepository.FindSystemPermissionMenuAgileByUserId(input)).ToList();
                }
                return OperateStatus<IEnumerable<SystemMenuFindAgileOutput>>.Success(menuAgile);
            }
        }

        /// <summary>
        /// 递归获取树形结构
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="treeEntity"></param>
        private List<SystemPermissionFindMenuByUserIdRouterOutput> FindChildren(List<SystemPermissionFindMenuByUserIdOutput> datas, SystemPermissionFindMenuByUserIdOutput treeEntity, string userId)
        {
            var userInfo = EipHttpContext.CurrentUser();
            List<SystemPermissionFindMenuByUserIdRouterOutput> treeEntities = new List<SystemPermissionFindMenuByUserIdRouterOutput>();
            var children = datas.Where(w => w.ParentId != null && w.ParentId.ToString() == treeEntity.Id.ToString());
            foreach (var item in children)
            {
                var path = item.Path.IsNullOrEmpty() ? "" : item.Path.Replace("{userId}", userId);
                SystemPermissionFindMenuByUserIdRouterOutput treeEntityChidren = new SystemPermissionFindMenuByUserIdRouterOutput
                {
                    router = item.Router,
                    name = item.Text,
                    path = path,
                    meta = new SystemPermissionFindMenuByUserIdRouterMetaOutput
                    {
                        menuId = item.Id,
                        icon = item.Icon,
                        theme = item.Theme,
                        @params = item.Params.IsNotNullOrEmpty() ? JsonConvert.DeserializeObject(item.Params) : null,
                        link = item.OpenType == 2 ? path : "",
                        image = item.Image,
                        count = 0
                    }
                };
                treeEntityChidren.children = FindChildren(datas, item, userId);
                treeEntities.Add(treeEntityChidren);
            }
            return treeEntities;
        }

        /// <summary>
        /// 根据用户Id获取用户具有的模块权限
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<BaseTree>>> FindSystemPermissionMenuAllByUserId()
        {
            var userInfo = EipHttpContext.CurrentUser();
            using (var fixture = new SqlDatabaseFixture())
            {
                try
                {
                    //如果是超级管理员
                    if (userInfo.IsAdmin)
                    {
                        var config = new MapperConfigurationExpression();
                        config.CreateMap<SystemMenu, BaseTree>()
                       .ForMember(o => o.id, opt => opt.MapFrom(src => src.MenuId))
                       .ForMember(o => o.parent, opt => opt.MapFrom(src => src.ParentId))
                       .ForMember(o => o.text, opt => opt.MapFrom(src => src.Name));
                        return OperateStatus<IEnumerable<BaseTree>>.Success((await fixture.Db.SystemMenu.FindAllAsync(f => !f.IsFreeze && f.HaveMenuPermission)).OrderBy(o => o.OrderNo).MapToList<SystemMenu, BaseTree>(config));
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            return OperateStatus<IEnumerable<BaseTree>>.Success((await _permissionRepository.FindSystemPermissionMenuAllByUserId(userInfo.UserId)).ToList());
        }

        /// <summary>
        /// 根据角色Id,岗位Id,组Id,人员Id获取具有的模块信息
        /// </summary>
        /// <param name="input">输入参数</param>
        /// <returns>树形模块信息</returns>
        public async Task<OperateStatus<IEnumerable<BaseTree>>> FindMenuHavePermissionByPrivilegeMasterValue(SystemPermissiontMenuHaveByPrivilegeMasterValueInput input)
        {
            return OperateStatus<IEnumerable<BaseTree>>.Success((await _permissionRepository.FindMenuHavePermissionByPrivilegeMasterValue(input)).ToList());
        }

        /// <summary>
        /// 查询对应拥有的功能项模块信息
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemPermissionFindMenuButtonByPrivilegeMasterOutput>>> FindMenuButtonByPrivilegeMaster(SystemPermissionFindMenuButtonByPrivilegeMasterInput input)
        {
            IList<SystemPermissionFindMenuButtonByPrivilegeMasterOutput> systemMenuButtons = new List<SystemPermissionFindMenuButtonByPrivilegeMasterOutput>();

            //当前传递的人员,角色,组织等拥有的菜单
            var menus = (await _permissionRepository.FindMenuHavePermissionByPrivilegeMasterValue(new SystemPermissiontMenuHaveByPrivilegeMasterValueInput()
            {
                PrivilegeMaster = input.PrivilegeMaster,
                PrivilegeMasterValue = input.PrivilegeMasterValue,
                PrivilegeAccess = EnumPrivilegeAccess.模块按钮
            })).ToList();

            if (menus.Any())
            {
                //当前传递的人员,角色,组织等拥有的按钮
                IList<SystemPermission> haveFunctions = (await FindPermissionByPrivilegeMasterValue(new SystemPermissionByPrivilegeMasterValueInput
                {
                    PrivilegeAccess = EnumPrivilegeAccess.模块按钮,
                    PrivilegeMasterValue = input.PrivilegeMasterValue,
                    PrivilegeMaster = input.PrivilegeMaster
                })).Data.ToList();
                IList<SystemMenuButtonOutput> menuButtons = new List<SystemMenuButtonOutput>();
                //判断该用户是否为超级管理员:若是超级管理员则显示所有按钮
                var userInfo = EipHttpContext.CurrentUser();
                //如果是超级管理员
                if (userInfo.IsAdmin)
                {
                    var menuIds = menus.Select(s => Guid.Parse(s.id.ToString())).ToList();
                    menuButtons = (await _menuButtonRepository.FindMenuButtonByMenuId(menuIds)).ToList();
                }
                else
                {
                    menuButtons = (await _menuButtonRepository.FindHaveMenuButtonPermission(new IdInput(input.UserId))).ToList();
                }
                //当前人员拥有的按钮
                foreach (var menu in menus)
                {
                    var menuButton = menuButtons.Where(w => w.MenuId == Guid.Parse(menu.id.ToString())).OrderBy(o => o.OrderNo);
                    foreach (var button in menuButton)
                    {
                        var selectFunction = haveFunctions.Where(w => w.PrivilegeAccessValue == button.MenuButtonId);
                        systemMenuButtons.Add(new SystemPermissionFindMenuButtonByPrivilegeMasterOutput
                        {
                            Exist = selectFunction.Any(),
                            MenuId = button.MenuId,
                            Name = button.Name,
                            Icon = button.Icon,
                            MenuNames = button.MenuNames,
                            MenuButtonId = button.MenuButtonId,
                        });
                    }
                }
            }
            return OperateStatus<IEnumerable<SystemPermissionFindMenuButtonByPrivilegeMasterOutput>>.Success(systemMenuButtons);
        }

        /// <summary>
        /// 查询对应拥有的功能项模块信息
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemPermissionFindDataByPrivilegeMasterOutput>>> FindDataByPrivilegeMaster(SystemPermissionFindDataByPrivilegeMasterInput input)
        {
            IList<SystemPermissionFindDataByPrivilegeMasterOutput> datas = new List<SystemPermissionFindDataByPrivilegeMasterOutput>();

            //获取拥有的模块信息
            var menus = (await FindMenuHavePermissionByPrivilegeMasterValue(new SystemPermissiontMenuHaveByPrivilegeMasterValueInput
            {
                PrivilegeMaster = input.PrivilegeMaster,
                PrivilegeMasterValue = input.PrivilegeMasterValue,
                PrivilegeAccess = EnumPrivilegeAccess.数据权限
            })).Data.ToList();

            if (menus.Any())
            {
                //获取拥有的功能项信息
                IList<SystemPermission> haveDatas = (await FindPermissionByPrivilegeMasterValue(new SystemPermissionByPrivilegeMasterValueInput
                {
                    PrivilegeAccess = EnumPrivilegeAccess.数据权限,
                    PrivilegeMaster = input.PrivilegeMaster,
                    PrivilegeMasterValue = input.PrivilegeMasterValue
                })).Data.ToList();

                IList<SystemDataOutput> dataOutputs = new List<SystemDataOutput>();
                //当前人员是否为超级管理员

                //判断该用户是否为超级管理员:若是超级管理员则显示所有按钮
                var userInfo = EipHttpContext.CurrentUser();
                //如果是超级管理员
                if (userInfo.IsAdmin)
                {
                    dataOutputs = (await _dataRepository.FindDataByMenuId(menus.Select(s => Guid.Parse(s.id.ToString())).ToList())).ToList();
                }
                else
                {
                    dataOutputs = (await _dataRepository.FindHaveDataPermission(new IdInput(input.UserId))).ToList();
                }
                foreach (var menu in menus)
                {
                    var data = dataOutputs.Where(w => w.MenuId == Guid.Parse(menu.id.ToString())).OrderBy(o => o.OrderNo);
                    foreach (var d in data)
                    {
                        var selectFunction = haveDatas.Where(w => w.PrivilegeAccessValue == d.DataId);
                        datas.Add(new SystemPermissionFindDataByPrivilegeMasterOutput
                        {
                            DataId = d.DataId,
                            Name = d.Name,
                            MenuNames = d.MenuNames,
                            MenuId = d.MenuId,
                            Exist = selectFunction.Any()
                        });
                    }
                }
            }
            return OperateStatus<IEnumerable<SystemPermissionFindDataByPrivilegeMasterOutput>>.Success(datas);
        }

        /// <summary>
        /// 查询对应拥有的功能项模块信息
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemMenuButton>>> FindMenuButtonByPrivilegeMasterAll(SystemPermissionFindMenuButtonByPrivilegeMasterInput input)
        {
            IList<SystemMenuButton> systemMenuButtons = new List<SystemMenuButton>();
            List<BaseTree> menus;
            IList<SystemPermission> haveFunctions = (await FindPermissionByPrivilegeMasterValue(new SystemPermissionByPrivilegeMasterValueInput
            {
                PrivilegeAccess = EnumPrivilegeAccess.模块按钮,
                PrivilegeMasterValue = input.PrivilegeMasterValue,
                PrivilegeMaster = input.PrivilegeMaster
            })).Data.ToList();
            IList<SystemMenuButtonOutput> menuButtons;
            var userInfo = EipHttpContext.CurrentUser();
            //如果是超级管理员
            if (userInfo.IsAdmin)
            {
                using (var fix = new SqlDatabaseFixture())
                {
                    var config = new MapperConfigurationExpression();
                    config.CreateMap<SystemMenu, BaseTree>()
                   .ForMember(o => o.id, opt => opt.MapFrom(src => src.MenuId))
                   .ForMember(o => o.parent, opt => opt.MapFrom(src => src.ParentId))
                   .ForMember(o => o.text, opt => opt.MapFrom(src => src.Name));
                    menus = (await fix.Db.SystemMenu.FindAllAsync(f => !f.IsFreeze && f.HaveMenuPermission)).OrderBy(o => o.OrderNo).MapToList<SystemMenu, BaseTree>(config);
                }
                menuButtons = (await _menuButtonRepository.FindMenuButtonByMenuId()).ToList();
            }
            else
            {
                menus = (await FindSystemPermissionMenuAllByUserId()).Data.ToList();
                menuButtons = (await _menuButtonRepository.FindHaveMenuButtonPermission(new IdInput(input.UserId))).ToList();
            }
            //当前人员拥有的按钮
            foreach (var menu in menus)
            {
                var menuButton = menuButtons.Where(w => w.MenuId == Guid.Parse(menu.id.ToString())).OrderBy(o => o.OrderNo);
                foreach (var button in menuButton)
                {
                    var selectFunction = haveFunctions.Where(w => w.PrivilegeAccessValue == button.MenuButtonId);
                    button.Remark = selectFunction.Any() ? "selected" : "";
                    systemMenuButtons.Add(button);
                }
            }
            return OperateStatus<IEnumerable<SystemMenuButton>>.Success(systemMenuButtons);
        }

        /// <summary>
        /// 查询对应拥有的功能项模块信息
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemData>>> FindDataByPrivilegeMasterAll(SystemPermissionFindDataByPrivilegeMasterInput input)
        {
            IList<SystemData> datas = new List<SystemData>();

            //获取拥有的功能项信息
            IList<SystemPermission> haveDatas = (await FindPermissionByPrivilegeMasterValue(new SystemPermissionByPrivilegeMasterValueInput
            {
                PrivilegeAccess = EnumPrivilegeAccess.数据权限,
                PrivilegeMaster = input.PrivilegeMaster,
                PrivilegeMasterValue = input.PrivilegeMasterValue
            })).Data.ToList();
            List<BaseTree> menus;
            IList<SystemDataOutput> dataOutputs;

            var userInfo = EipHttpContext.CurrentUser();
            //如果是超级管理员
            if (userInfo.IsAdmin)
            {
                using (var fix = new SqlDatabaseFixture())
                {
                    var config = new MapperConfigurationExpression();
                    config.CreateMap<SystemMenu, BaseTree>()
                   .ForMember(o => o.id, opt => opt.MapFrom(src => src.MenuId))
                   .ForMember(o => o.parent, opt => opt.MapFrom(src => src.ParentId))
                   .ForMember(o => o.text, opt => opt.MapFrom(src => src.Name));
                    menus = (await fix.Db.SystemMenu.FindAllAsync(f => !f.IsFreeze && f.HaveMenuPermission)).OrderBy(o => o.OrderNo).MapToList<SystemMenu, BaseTree>(config);
                }
                dataOutputs = (await _dataRepository.FindDataByMenuId()).ToList();
            }
            else
            {
                menus = (await FindSystemPermissionMenuAllByUserId()).Data.ToList();
                dataOutputs = (await _dataRepository.FindHaveDataPermission(new IdInput(input.UserId))).ToList();
            }
            foreach (var menu in menus)
            {
                var data = dataOutputs.Where(w => w.MenuId == Guid.Parse(menu.id.ToString())).OrderBy(o => o.OrderNo);
                foreach (var d in data)
                {
                    var selectFunction = haveDatas.Where(w => w.PrivilegeAccessValue == d.DataId);
                    d.Remark = selectFunction.Any() ? "selected" : "";
                    datas.Add(d);
                }
            }
            return OperateStatus<IEnumerable<SystemData>>.Success(datas);
        }

        /// <summary>
        /// 获取登录人员对应模块下的功能项
        /// </summary>
        /// <param name="viewRote">路由信息</param>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemMenuButtonByViewRote>>> FindMenuButton(ViewRote viewRote)
        {
            using (var fixture = new SqlDatabaseFixture())
            {
                //判断当前人员是否为超级管理员若是超级管理员则具有最大权限
                IList<SystemMenuButtonByViewRote> menuButton = new List<SystemMenuButtonByViewRote>();
                //判断该用户是否为超级管理员:若是超级管理员则显示所有模块
                var userInfo = EipHttpContext.CurrentUser();
                //如果是超级管理员
                if (userInfo.IsAdmin)
                {
                    var data = (await fixture.Db.SystemMenuButtons.FindAllAsync<SystemMenuButtonByViewRote>(f =>
                      f.MenuId == viewRote.MenuId,
                    q => q.Buttons)).ToList();
                    if (data.Any())
                    {
                        return OperateStatus<IEnumerable<SystemMenuButtonByViewRote>>.Success(data.First().Buttons.Where(f => !f.IsFreeze).OrderBy(o => o.OrderNo));
                    }
                    return OperateStatus<IEnumerable<SystemMenuButtonByViewRote>>.Success(menuButton);
                }
                return OperateStatus<IEnumerable<SystemMenuButtonByViewRote>>.Success((await _menuButtonRepository.FindMenuButton(viewRote)).ToList());
            }
        }

        /// <summary>
        /// 获取角色，组等具有的权限
        /// </summary>
        /// <param name="privilegeMaster">类型:角色，人员，组等</param>
        /// <param name="privilegeMasterValue">对应值</param>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemPermission>>> FindSystemPermissionsByPrivilegeMasterValueAndValue(EnumPrivilegeMaster privilegeMaster,
            Guid? privilegeMasterValue = null)
        {
            return OperateStatus<IEnumerable<SystemPermission>>.Success(privilegeMasterValue != null ? await FindAllAsync(f => f.PrivilegeMasterValue == privilegeMasterValue && f.PrivilegeMaster == privilegeMaster.ToShort()) : await FindAllAsync(f => f.PrivilegeMaster == privilegeMaster.ToShort()));
        }

        #region 数据权限
        /// <summary>
        /// 获取数据权限Sql
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<string>> FindDataPermissionSql(ViewRote input)
        {
            StringBuilder stringBuilder = new StringBuilder();
            //获取当前用户是否为超级管理员
            var userInfo = EipHttpContext.CurrentUser();
            if (userInfo.IsAdmin)
            {
                return OperateStatus<string>.Success("  1=1");
            }
            IList<SystemData> datas = (await _permissionRepository.FindDataPermission(input)).ToList();
            if (datas.Any())
            {
                foreach (var data in datas)
                {
                    if (!data.RuleHtml.IsNullOrEmpty())
                    {
                        //替换Html标签
                        data.RuleHtml = data.RuleHtml.ReplaceEditor();
                        //是否具有规则数据
                        if (!data.RuleJson.IsNullOrEmpty())
                        {
                            IList<SystemDataRuleJsonDoubleWay> ruleJsons = JsonConvert.DeserializeObject<IList<SystemDataRuleJsonDoubleWay>>(data.RuleJson).ToList();
                            foreach (var ruleJson in ruleJsons)
                            {
                                //替换Sql
                                data.RuleHtml = data.RuleHtml.Replace(ruleJson.Field, ruleJson.Value.InSql());
                            }
                        }
                        //替换固定信息
                        data.RuleHtml = (await GetRuleSql(data.RuleHtml, input.UserId)).Data;
                        //追加替换后的Sql
                        stringBuilder.Append(data.RuleHtml + " OR ");
                    }
                }
            }
            //去除最后一个OR
            string sql = stringBuilder.ToString();
            sql = sql.Contains("OR") ? sql.Substring(0, sql.Length - 3) : sql;
            sql = sql.Trim().IsNullOrEmpty() ? "1<>1" : "(" + sql + ")";
            return OperateStatus<string>.Success(sql);
        }

        /// <summary>
        /// 替换规则Sql
        /// </summary>
        /// <param name="ruleSql"></param>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        private async Task<OperateStatus<string>> GetRuleSql(string ruleSql, Guid userId)
        {
            using (var fixture = new SqlDatabaseFixture())
            {
                var h1Elements = ruleSql.GetNodes();
                try
                {
                    //获取角色、组、岗位数据
                    var privilegeMaster = new List<short>
                {
                    EnumPrivilegeMaster.角色.ToShort(),
                    EnumPrivilegeMaster.组.ToShort(),
                    EnumPrivilegeMaster.岗位.ToShort(),
                    EnumPrivilegeMaster.组织架构.ToShort()
                };
                    var privilegeDetailDtos = (await fixture.Db.SystemPermissionUser.SetSelect(s => new { s.PrivilegeMaster, s.PrivilegeMasterValue }).FindAllAsync(f => f.PrivilegeMasterUserId == userId && privilegeMaster.Contains(f.PrivilegeMaster))).ToList();
                    if (h1Elements != null)
                    {
                        foreach (var element in h1Elements)
                        {
                            //解码
                            var columnSetting = HttpUtility.UrlDecode(element.Id);
                            if (columnSetting != null)
                            {
                                switch (columnSetting)
                                {
                                    case "所有":
                                        ruleSql = ruleSql.Replace(element.OuterHtml, "1=1");
                                        break;
                                    case "当前用户":
                                        ruleSql = ruleSql.Replace(element.OuterHtml, userId.ToString());
                                        break;
                                    case "所在组织":
                                        ruleSql = ruleSql.Replace(element.OuterHtml, privilegeDetailDtos.Where(w => w.PrivilegeMaster == EnumPrivilegeMaster.组织架构.GetHashCode()).Select(d => d.PrivilegeMasterValue).ToList().ExpandAndToString().InSql());
                                        break;
                                    case "所在组织及下级组织":
                                        //查找机构
                                        var orgId = privilegeDetailDtos.Where(w => w.PrivilegeMaster == EnumPrivilegeMaster.组织架构.GetHashCode()).Select(d => d.PrivilegeMasterValue);
                                        var organizationId = new List<Guid>();
                                        //获取当前人员所在组织及下级组织
                                        foreach (var item in orgId)
                                        {
                                            var orgs = await fixture.Db.SystemOrganization.SetSelect(s => new { s.OrganizationId }).FindAllAsync(f => f.ParentIds.Contains(item.ToString()));
                                            foreach (var o in orgs.ToList())
                                            {
                                                organizationId.Add(o.OrganizationId);
                                            }
                                        }
                                        ruleSql = ruleSql.Replace(element.OuterHtml, organizationId.Distinct().ExpandAndToString().InSql());
                                        break;
                                    case "所在组织代码":
                                        ruleSql = ruleSql.Replace(element.OuterHtml, privilegeDetailDtos.Where(w => w.PrivilegeMaster == EnumPrivilegeMaster.组织架构.GetHashCode()).Select(d => d.PrivilegeMasterValue).ToList().ExpandAndToString().InSql());
                                        break;
                                    case "所在岗位":
                                        ruleSql = ruleSql.Replace(element.OuterHtml, privilegeDetailDtos.Where(w => w.PrivilegeMaster == EnumPrivilegeMaster.岗位.GetHashCode()).Select(d => d.PrivilegeMasterValue).ToList().ExpandAndToString().InSql());
                                        break;
                                    case "所在工作组":
                                        ruleSql = ruleSql.Replace(element.OuterHtml, privilegeDetailDtos.Where(w => w.PrivilegeMaster == EnumPrivilegeMaster.组.GetHashCode()).Select(d => d.PrivilegeMasterValue).ToList().ExpandAndToString().InSql());
                                        break;
                                    default:
                                        //其他可能是选择对应的权限
                                        break;
                                }
                            }
                        }
                    }

                }
                catch (Exception ex)
                {

                }
                return OperateStatus<string>.Success(ruleSql);
            }
        }
        #endregion

        #endregion
    }
}