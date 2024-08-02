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
using EIP.System.Models.Dtos.Data;
using EIP.System.Models.Dtos.Menu;
using EIP.System.Models.Dtos.MenuButton;

namespace EIP.System.Logic.Impl
{
    /// <summary>
    /// Ȩ��ҵ���߼�
    /// </summary>
    public class SystemPermissionLogic : DapperAsyncLogic<SystemPermission>, ISystemPermissionLogic
    {
        #region ����ע��
        private readonly ISystemMenuButtonRepository _menuButtonRepository;
        private readonly ISystemPermissionRepository _permissionRepository;
        private readonly ISystemDataRepository _dataRepository;
        /// <summary>
        /// Ȩ���߼�
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

        #region ����

        /// <summary>
        /// ����״̬ΪTrue��ģ����Ϣ
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemPermission>>> FindPermissionByPrivilegeMasterValue(SystemPermissionByPrivilegeMasterValueInput input)
        {
            return OperateStatus<IEnumerable<SystemPermission>>.Success((await _permissionRepository.FindPermissionByPrivilegeMasterValue(input)).ToList());
        }

        /// <summary>
        /// ����Ȩ����Ϣ
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

                    if (input.PrivilegeMaster == EnumPrivilegeMaster.��Ա)
                    {
                        var master = input.PrivilegeMaster.ToShort();
                        //ɾ����Ӧ��Ա����
                        await fixture.Db.SystemPermissionUser.DeleteAsync(d => d.PrivilegeMaster == master && d.PrivilegeMasterValue == input.PrivilegeMasterValue);
                        //�ж��Ƿ����Ȩ��
                        if (!systemPermissions.Any())
                        {
                            operateStatus.Code = ResultCode.Success;
                            operateStatus.Msg = Chs.Successful;
                            return operateStatus;
                        }

                        //����Ȩ����Ա����
                        var permissionUser = new SystemPermissionUser
                        {
                            PrivilegeMaster = (byte)input.PrivilegeMaster,
                            PrivilegeMasterUserId = input.PrivilegeMasterValue,
                            PrivilegeMasterValue = input.PrivilegeMasterValue
                        };
                        await fixture.Db.SystemPermissionUser.InsertAsync(permissionUser);
                    }

                    //�Ƿ����Ȩ������
                    if (!systemPermissions.Any())
                    {
                        operateStatus.Code = ResultCode.Success;
                        operateStatus.Msg = Chs.Successful;
                        return operateStatus;
                    }

                    //�������ݿ�
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
            //�жϸ��û��Ƿ�Ϊ��������Ա:���ǳ�������Ա����ʾ����ģ��
            var userInfo = EipHttpContext.CurrentUser();
            //����ǳ�������Ա
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
        /// �����û�Id��ȡ�û����е�ģ��Ȩ��
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemPermissionFindMenuByUserIdRouterOutput>>> Menus(SystemPermissionMenuInput input)
        {
            try
            {
                List<SystemPermissionFindMenuByUserIdRouterOutput> outputs = new List<SystemPermissionFindMenuByUserIdRouterOutput>();
                IList<SystemPermissionFindMenuByUserIdOutput> treeEntities = new List<SystemPermissionFindMenuByUserIdOutput>();
                //�жϸ��û��Ƿ�Ϊ��������Ա:���ǳ�������Ա����ʾ����ģ��
                var userInfo = EipHttpContext.CurrentUser();
                //����ǳ�������Ա
                if (userInfo.IsAdmin)
                {
                    treeEntities = (await _permissionRepository.FindSystemPermissionMenuByAdmin(input)).ToList();
                }
                else
                {
                    treeEntities = (await _permissionRepository.FindSystemPermissionMenuByUserId(input)).ToList();
                }
                //����·�ɷ�ʽ
                if (treeEntities.Any())
                {
                    RSACryptoService loginRsa = new RSACryptoService(ResourceKey.����˽Կ, ResourceKey.���ܹ�Կ);
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
        /// �����û�Id��ȡ�û����е�ģ��Ȩ��
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemMenuFindAgileOutput>>> MenusAgile(SystemPermissionMenuInput input)
        { //�жϸ��û��Ƿ�Ϊ��������Ա:���ǳ�������Ա����ʾ����ģ��
            var userInfo = EipHttpContext.CurrentUser();
            //����ǳ�������Ա
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
        /// �ݹ��ȡ���νṹ
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
        /// �����û�Id��ȡ�û����е�ģ��Ȩ��
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<BaseTree>>> FindSystemPermissionMenuAllByUserId()
        {
            var userInfo = EipHttpContext.CurrentUser();
            using (var fixture = new SqlDatabaseFixture())
            {
                try
                {
                    //����ǳ�������Ա
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
        /// ���ݽ�ɫId,��λId,��Id,��ԱId��ȡ���е�ģ����Ϣ
        /// </summary>
        /// <param name="input">�������</param>
        /// <returns>����ģ����Ϣ</returns>
        public async Task<OperateStatus<IEnumerable<BaseTree>>> FindMenuHavePermissionByPrivilegeMasterValue(SystemPermissiontMenuHaveByPrivilegeMasterValueInput input)
        {
            return OperateStatus<IEnumerable<BaseTree>>.Success((await _permissionRepository.FindMenuHavePermissionByPrivilegeMasterValue(input)).ToList());
        }

        /// <summary>
        /// ��ѯ��Ӧӵ�еĹ�����ģ����Ϣ
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemPermissionFindMenuButtonByPrivilegeMasterOutput>>> FindMenuButtonByPrivilegeMaster(SystemPermissionFindMenuButtonByPrivilegeMasterInput input)
        {
            IList<SystemPermissionFindMenuButtonByPrivilegeMasterOutput> systemMenuButtons = new List<SystemPermissionFindMenuButtonByPrivilegeMasterOutput>();

            //��ǰ���ݵ���Ա,��ɫ,��֯��ӵ�еĲ˵�
            var menus = (await _permissionRepository.FindMenuHavePermissionByPrivilegeMasterValue(new SystemPermissiontMenuHaveByPrivilegeMasterValueInput()
            {
                PrivilegeMaster = input.PrivilegeMaster,
                PrivilegeMasterValue = input.PrivilegeMasterValue,
                PrivilegeAccess = EnumPrivilegeAccess.ģ�鰴ť
            })).ToList();

            if (menus.Any())
            {
                //��ǰ���ݵ���Ա,��ɫ,��֯��ӵ�еİ�ť
                IList<SystemPermission> haveFunctions = (await FindPermissionByPrivilegeMasterValue(new SystemPermissionByPrivilegeMasterValueInput
                {
                    PrivilegeAccess = EnumPrivilegeAccess.ģ�鰴ť,
                    PrivilegeMasterValue = input.PrivilegeMasterValue,
                    PrivilegeMaster = input.PrivilegeMaster
                })).Data.ToList();
                IList<SystemMenuButtonOutput> menuButtons = new List<SystemMenuButtonOutput>();
                //�жϸ��û��Ƿ�Ϊ��������Ա:���ǳ�������Ա����ʾ���а�ť
                var userInfo = EipHttpContext.CurrentUser();
                //����ǳ�������Ա
                if (userInfo.IsAdmin)
                {
                    var menuIds = menus.Select(s => Guid.Parse(s.id.ToString())).ToList();
                    menuButtons = (await _menuButtonRepository.FindMenuButtonByMenuId(menuIds)).ToList();
                }
                else
                {
                    menuButtons = (await _menuButtonRepository.FindHaveMenuButtonPermission(new IdInput(input.UserId))).ToList();
                }
                //��ǰ��Աӵ�еİ�ť
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
        /// ��ѯ��Ӧӵ�еĹ�����ģ����Ϣ
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemPermissionFindDataByPrivilegeMasterOutput>>> FindDataByPrivilegeMaster(SystemPermissionFindDataByPrivilegeMasterInput input)
        {
            IList<SystemPermissionFindDataByPrivilegeMasterOutput> datas = new List<SystemPermissionFindDataByPrivilegeMasterOutput>();

            //��ȡӵ�е�ģ����Ϣ
            var menus = (await FindMenuHavePermissionByPrivilegeMasterValue(new SystemPermissiontMenuHaveByPrivilegeMasterValueInput
            {
                PrivilegeMaster = input.PrivilegeMaster,
                PrivilegeMasterValue = input.PrivilegeMasterValue,
                PrivilegeAccess = EnumPrivilegeAccess.����Ȩ��
            })).Data.ToList();

            if (menus.Any())
            {
                //��ȡӵ�еĹ�������Ϣ
                IList<SystemPermission> haveDatas = (await FindPermissionByPrivilegeMasterValue(new SystemPermissionByPrivilegeMasterValueInput
                {
                    PrivilegeAccess = EnumPrivilegeAccess.����Ȩ��,
                    PrivilegeMaster = input.PrivilegeMaster,
                    PrivilegeMasterValue = input.PrivilegeMasterValue
                })).Data.ToList();

                IList<SystemDataOutput> dataOutputs = new List<SystemDataOutput>();
                //��ǰ��Ա�Ƿ�Ϊ��������Ա

                //�жϸ��û��Ƿ�Ϊ��������Ա:���ǳ�������Ա����ʾ���а�ť
                var userInfo = EipHttpContext.CurrentUser();
                //����ǳ�������Ա
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
        /// ��ѯ��Ӧӵ�еĹ�����ģ����Ϣ
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemMenuButton>>> FindMenuButtonByPrivilegeMasterAll(SystemPermissionFindMenuButtonByPrivilegeMasterInput input)
        {
            IList<SystemMenuButton> systemMenuButtons = new List<SystemMenuButton>();
            List<BaseTree> menus;
            IList<SystemPermission> haveFunctions = (await FindPermissionByPrivilegeMasterValue(new SystemPermissionByPrivilegeMasterValueInput
            {
                PrivilegeAccess = EnumPrivilegeAccess.ģ�鰴ť,
                PrivilegeMasterValue = input.PrivilegeMasterValue,
                PrivilegeMaster = input.PrivilegeMaster
            })).Data.ToList();
            IList<SystemMenuButtonOutput> menuButtons;
            var userInfo = EipHttpContext.CurrentUser();
            //����ǳ�������Ա
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
            //��ǰ��Աӵ�еİ�ť
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
        /// ��ѯ��Ӧӵ�еĹ�����ģ����Ϣ
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemData>>> FindDataByPrivilegeMasterAll(SystemPermissionFindDataByPrivilegeMasterInput input)
        {
            IList<SystemData> datas = new List<SystemData>();

            //��ȡӵ�еĹ�������Ϣ
            IList<SystemPermission> haveDatas = (await FindPermissionByPrivilegeMasterValue(new SystemPermissionByPrivilegeMasterValueInput
            {
                PrivilegeAccess = EnumPrivilegeAccess.����Ȩ��,
                PrivilegeMaster = input.PrivilegeMaster,
                PrivilegeMasterValue = input.PrivilegeMasterValue
            })).Data.ToList();
            List<BaseTree> menus;
            IList<SystemDataOutput> dataOutputs;

            var userInfo = EipHttpContext.CurrentUser();
            //����ǳ�������Ա
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
        /// ��ȡ��¼��Ա��Ӧģ���µĹ�����
        /// </summary>
        /// <param name="viewRote">·����Ϣ</param>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemMenuButtonByViewRote>>> FindMenuButton(ViewRote viewRote)
        {
            using (var fixture = new SqlDatabaseFixture())
            {
                //�жϵ�ǰ��Ա�Ƿ�Ϊ��������Ա���ǳ�������Ա��������Ȩ��
                IList<SystemMenuButtonByViewRote> menuButton = new List<SystemMenuButtonByViewRote>();
                //�жϸ��û��Ƿ�Ϊ��������Ա:���ǳ�������Ա����ʾ����ģ��
                var userInfo = EipHttpContext.CurrentUser();
                //����ǳ�������Ա
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
        /// ��ȡ��ɫ����Ⱦ��е�Ȩ��
        /// </summary>
        /// <param name="privilegeMaster">����:��ɫ����Ա�����</param>
        /// <param name="privilegeMasterValue">��Ӧֵ</param>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemPermission>>> FindSystemPermissionsByPrivilegeMasterValueAndValue(EnumPrivilegeMaster privilegeMaster,
            Guid? privilegeMasterValue = null)
        {
            return OperateStatus<IEnumerable<SystemPermission>>.Success(privilegeMasterValue != null ? await FindAllAsync(f => f.PrivilegeMasterValue == privilegeMasterValue && f.PrivilegeMaster == privilegeMaster.ToShort()) : await FindAllAsync(f => f.PrivilegeMaster == privilegeMaster.ToShort()));
        }

        #region ����Ȩ��
        /// <summary>
        /// ��ȡ����Ȩ��Sql
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<string>> FindDataPermissionSql(ViewRote input)
        {
            StringBuilder stringBuilder = new StringBuilder();
            //��ȡ��ǰ�û��Ƿ�Ϊ��������Ա
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
                        //�滻Html��ǩ
                        data.RuleHtml = data.RuleHtml.ReplaceEditor();
                        //�Ƿ���й�������
                        if (!data.RuleJson.IsNullOrEmpty())
                        {
                            IList<SystemDataRuleJsonDoubleWay> ruleJsons = JsonConvert.DeserializeObject<IList<SystemDataRuleJsonDoubleWay>>(data.RuleJson).ToList();
                            foreach (var ruleJson in ruleJsons)
                            {
                                //�滻Sql
                                data.RuleHtml = data.RuleHtml.Replace(ruleJson.Field, ruleJson.Value.InSql());
                            }
                        }
                        //�滻�̶���Ϣ
                        data.RuleHtml = (await GetRuleSql(data.RuleHtml, input.UserId)).Data;
                        //׷���滻���Sql
                        stringBuilder.Append(data.RuleHtml + " OR ");
                    }
                }
            }
            //ȥ�����һ��OR
            string sql = stringBuilder.ToString();
            sql = sql.Contains("OR") ? sql.Substring(0, sql.Length - 3) : sql;
            sql = sql.Trim().IsNullOrEmpty() ? "1<>1" : "(" + sql + ")";
            return OperateStatus<string>.Success(sql);
        }

        /// <summary>
        /// �滻����Sql
        /// </summary>
        /// <param name="ruleSql"></param>
        /// <param name="userId">�û�Id</param>
        /// <returns></returns>
        private async Task<OperateStatus<string>> GetRuleSql(string ruleSql, Guid userId)
        {
            using (var fixture = new SqlDatabaseFixture())
            {
                var h1Elements = ruleSql.GetNodes();
                try
                {
                    //��ȡ��ɫ���顢��λ����
                    var privilegeMaster = new List<short>
                {
                    EnumPrivilegeMaster.��ɫ.ToShort(),
                    EnumPrivilegeMaster.��.ToShort(),
                    EnumPrivilegeMaster.��λ.ToShort(),
                    EnumPrivilegeMaster.��֯�ܹ�.ToShort()
                };
                    var privilegeDetailDtos = (await fixture.Db.SystemPermissionUser.SetSelect(s => new { s.PrivilegeMaster, s.PrivilegeMasterValue }).FindAllAsync(f => f.PrivilegeMasterUserId == userId && privilegeMaster.Contains(f.PrivilegeMaster))).ToList();
                    if (h1Elements != null)
                    {
                        foreach (var element in h1Elements)
                        {
                            //����
                            var columnSetting = HttpUtility.UrlDecode(element.Id);
                            if (columnSetting != null)
                            {
                                switch (columnSetting)
                                {
                                    case "����":
                                        ruleSql = ruleSql.Replace(element.OuterHtml, "1=1");
                                        break;
                                    case "��ǰ�û�":
                                        ruleSql = ruleSql.Replace(element.OuterHtml, userId.ToString());
                                        break;
                                    case "������֯":
                                        ruleSql = ruleSql.Replace(element.OuterHtml, privilegeDetailDtos.Where(w => w.PrivilegeMaster == EnumPrivilegeMaster.��֯�ܹ�.GetHashCode()).Select(d => d.PrivilegeMasterValue).ToList().ExpandAndToString().InSql());
                                        break;
                                    case "������֯���¼���֯":
                                        //���һ���
                                        var orgId = privilegeDetailDtos.Where(w => w.PrivilegeMaster == EnumPrivilegeMaster.��֯�ܹ�.GetHashCode()).Select(d => d.PrivilegeMasterValue);
                                        var organizationId = new List<Guid>();
                                        //��ȡ��ǰ��Ա������֯���¼���֯
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
                                    case "������֯����":
                                        ruleSql = ruleSql.Replace(element.OuterHtml, privilegeDetailDtos.Where(w => w.PrivilegeMaster == EnumPrivilegeMaster.��֯�ܹ�.GetHashCode()).Select(d => d.PrivilegeMasterValue).ToList().ExpandAndToString().InSql());
                                        break;
                                    case "���ڸ�λ":
                                        ruleSql = ruleSql.Replace(element.OuterHtml, privilegeDetailDtos.Where(w => w.PrivilegeMaster == EnumPrivilegeMaster.��λ.GetHashCode()).Select(d => d.PrivilegeMasterValue).ToList().ExpandAndToString().InSql());
                                        break;
                                    case "���ڹ�����":
                                        ruleSql = ruleSql.Replace(element.OuterHtml, privilegeDetailDtos.Where(w => w.PrivilegeMaster == EnumPrivilegeMaster.��.GetHashCode()).Select(d => d.PrivilegeMasterValue).ToList().ExpandAndToString().InSql());
                                        break;
                                    default:
                                        //����������ѡ���Ӧ��Ȩ��
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