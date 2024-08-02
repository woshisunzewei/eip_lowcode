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
namespace EIP.System.Logic.Impl
{
    /// <summary>
    /// 组织机构
    /// </summary>
    public class SystemOrganizationLogic : DapperAsyncLogic<SystemOrganization>, ISystemOrganizationLogic
    {
        #region 构造函数

        private readonly ISystemOrganizationRepository _organizationRepository;
        private readonly ISystemRoleLogic _roleLogic;
        private readonly ISystemGroupLogic _groupLogic;
        private readonly ISystemPostLogic _postLogic;

        /// <summary>
        /// 组织机构
        /// </summary>
        /// <param name="organizationRepository"></param>
        /// <param name="roleLogic"></param>
        /// <param name="groupLogic"></param>
        /// <param name="postLogic"></param>
        public SystemOrganizationLogic(ISystemOrganizationRepository organizationRepository,
            ISystemRoleLogic roleLogic,
            ISystemGroupLogic groupLogic,
            ISystemPostLogic postLogic)
        {
            _roleLogic = roleLogic;
            _groupLogic = groupLogic;
            _postLogic = postLogic;
            _organizationRepository = organizationRepository;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 同步读取所有树数据
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<BaseTree>>> FindOrganizationTree(SystemOrganizationTreeInput input)
        {
            if (input.TopOrg.HasValue||input.SpecificOrg.IsNotNullOrEmpty())
            {
                var sql = "";
                if (input.TopOrg.HasValue)
                {
                    sql += $" org.ParentIds like '%{input.TopOrg}%'";
                }
                if (input.SpecificOrg.IsNotNullOrEmpty())
                {
                    sql += $" org.OrganizationId in ({input.SpecificOrg.InSql()})";
                }
                var datas = (await _organizationRepository.FindDataPermissionTree(new SystemOrganizationDataPermissionInput()
                {
                    DataSql = sql
                })).ToList();
                return OperateStatus<IEnumerable<BaseTree>>.Success(datas);
            }

            var config = new MapperConfigurationExpression();
            config.CreateMap<SystemOrganization, BaseTree>()
           .ForMember(o => o.id, opt => opt.MapFrom(src => src.OrganizationId))
           .ForMember(o => o.parent, opt => opt.MapFrom(src => src.ParentId))
           .ForMember(o => o.text, opt => opt.MapFrom(src => src.Name));
            return OperateStatus<IEnumerable<BaseTree>>.Success((await FindAllAsync()).OrderBy(o => o.OrderNo).MapToList<SystemOrganization, BaseTree>(config));
        }

        /// <summary>
        /// 所有数据权限组织机构
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<BaseTree>>> FindDataPermissionTree(SystemOrganizationDataPermissionInput input)
        {
            if (input.DataSql.IsNullOrEmpty())
                return OperateStatus<IEnumerable<BaseTree>>.Success(new List<BaseTree>());
            var datas = (await _organizationRepository.FindDataPermissionTree(input)).ToList();
            //判断范围类型
            string sql = "";
            switch (input.Range)
            {
                //当前公司
                case 3:
                    var now = await FindByIdAsync(input.PrincipalUser.OrganizationId);
                    var allOrgs = now.ParentIds.Split(',');
                    datas = datas.Where(w => allOrgs.Contains(w.id.ToString())).ToList();
                    break;
                //当前公司及下级部门
                case 4:
                    var nowAndNext = await FindByIdAsync(input.PrincipalUser.OrganizationId);
                    var nowAndNextOrgs = nowAndNext.ParentIds.Split(',');
                    datas = datas.Where(w => nowAndNextOrgs.Contains(w.id.ToString())).ToList();
                    if (datas.Any())
                    {
                        List<string> sqls = new List<string>();
                        foreach (var d in datas)
                        {
                            sqls.Add($" org.ParentIds like '%{d.id}%' ");
                        }
                        sql = "(" + StringUitl.GetArrayStr(sqls, "or") + ")";
                    }
                    else
                    {
                        sql = $" org.ParentIds like '%{input.PrincipalUser.OrganizationId}%'";
                    }
                    input.DataSql = sql;
                    datas = (await _organizationRepository.FindDataPermissionTree(input)).ToList();
                    break;
                //当前公司至登录人部门
                case 5:
                    var nowAndCurrUser = await FindByIdAsync(input.PrincipalUser.OrganizationId);
                    var nowAndCurrUserOrgs = nowAndCurrUser.ParentIds.Split(',');
                    datas = datas.Where(w => nowAndCurrUserOrgs.Contains(w.id.ToString())).ToList();
                    if (datas.Any())
                    {
                        var nextOrgs = nowAndCurrUser.ParentIds.Split(',');
                        var nextAllOrgs = new List<string>();
                        var nowNa = false;
                        foreach (var d in nextOrgs)
                        {
                            if (nowNa)
                            {
                                nextAllOrgs.Add(d);
                            }

                            if (!nowNa && datas.Any(a => a.id.ToString() == d))
                            {
                                nextAllOrgs.Add(d);
                                nowNa = true;
                            }
                        }
                        //拼接
                        sql = $" OrganizationId in ({nextAllOrgs.ExpandAndToString().InSql()}) ";
                    }
                    else
                    {
                        sql = $" org.ParentIds like '%{input.PrincipalUser.OrganizationId}%'";
                    }

                    input.DataSql = sql;
                    datas = (await _organizationRepository.FindDataPermissionTree(input)).ToList();
                    break;
            }

            //else
            //{
            //    if (!input.DataSql.IsNullOrEmpty())
            //    {
            //        foreach (var data in datas)
            //        {
            //            data.state = new JsTreeStateEntity();
            //            if (data.id.ToString() == input.PrincipalUser.OrganizationId.ToString())
            //            {
            //                data.parent = "#";
            //            }
            //        }
            //    }
            //}
            return OperateStatus<IEnumerable<BaseTree>>.Success(datas);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemOrganizationChartOutput>>> FindOrganizationAllTree()
        {
            var allOrgs = (await FindAllAsync()).ToList();
            var topOrgs = allOrgs.Where(w => w.ParentId.ToString() == "645b72ec-77e6-40d0-80aa-17d085091e97").ToList();
            //总模块
            IList<SystemOrganizationChartOutput> outputs = new List<SystemOrganizationChartOutput>(topOrgs.Count);
            foreach (var root in topOrgs)
            {
                outputs.Add(new SystemOrganizationChartOutput
                {
                    id = root.OrganizationId.ToString(),
                    name = root.Name,
                    title = root.Name
                });
            }
            //便利子模块
            foreach (var permission in outputs)
            {
                //判断有多少个模块
                IList<SystemOrganization> perRoots = allOrgs.Where(f => f.ParentId.ToString() == permission.id.ToString()).ToList();
                IList<SystemOrganizationChartOutput> trees = new List<SystemOrganizationChartOutput>();
                SystemOrganizationChartOutput tree = null;
                foreach (var treeEntity in perRoots)
                {
                    tree = new SystemOrganizationChartOutput
                    {
                        id = treeEntity.OrganizationId.ToString(),
                        name = treeEntity.Name,
                        title = treeEntity.Name,
                        children = GetWdChildNodesAllTree(ref allOrgs, treeEntity)
                    };
                    trees.Add(tree);
                }
                permission.children = trees;
                tree = null;
            }
            return OperateStatus<IEnumerable<SystemOrganizationChartOutput>>.Success(outputs);
        }

        /// <summary>
        /// 根据当前节点，加载子节点
        /// </summary>
        /// <param name="allOrgs">TreeEntity的集合</param>
        /// <param name="currTreeEntity">当前节点</param>
        private IList<SystemOrganizationChartOutput> GetWdChildNodesAllTree(ref List<SystemOrganization> allOrgs,
            SystemOrganization currTreeEntity)
        {
            IList<SystemOrganization> childNodes =
                allOrgs.Where(f => f.ParentId.ToString() == currTreeEntity.OrganizationId.ToString()).ToList();
            if (childNodes.Count <= 0)
            {
                return null;
            }
            IList<SystemOrganizationChartOutput> childTrees = new List<SystemOrganizationChartOutput>(childNodes.Count);
            SystemOrganizationChartOutput tree = null;
            foreach (var treeEntity in childNodes)
            {
                tree = new SystemOrganizationChartOutput
                {
                    id = treeEntity.OrganizationId.ToString(),
                    name = treeEntity.Name,
                    title = treeEntity.Name,
                    children = GetWdChildNodesAllTree(ref allOrgs, treeEntity)
                };
                childTrees.Add(tree);
            }
            return childTrees;
        }

        /// <summary>
        /// 根据父级查询下级
        /// </summary>
        /// <param name="input">父级id</param>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemOrganizationOutput>>> Find(SystemOrganizationDataPermissionInput input)
        {
            if (input.DataSql.IsNullOrEmpty())
                return OperateStatus<IEnumerable<SystemOrganizationOutput>>.Success(new List<SystemOrganizationOutput>());
            IList<SystemOrganizationOutput> outputs = new List<SystemOrganizationOutput>();
            var data = (await _organizationRepository.FindByParentId(input)).ToList();
            foreach (var item in data)
            {
                item.Number = data.Count(c => c.ParentId == item.OrganizationId);
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
            return OperateStatus<IEnumerable<SystemOrganizationOutput>>.Success(outputs);
        }
        /// <summary>
        /// 所有数据权限组织机构
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<BaseTree>>> FindDataPermission(SystemOrganizationDataPermissionInput input)
        {
            if (input.DataSql.IsNullOrEmpty())
                return OperateStatus<IEnumerable<BaseTree>>.Success(new List<BaseTree>());
            return OperateStatus<IEnumerable<BaseTree>>.Success((await _organizationRepository.FindDataPermissionTree(input)).ToList());
        }
        /// <summary>
        /// 保存组织机构
        /// </summary>
        /// <param name="input">组织机构</param>
        /// <returns></returns>
        public async Task<OperateStatus> Save(SystemOrganization input)
        {
            OperateStatus operateStatus;
            var organization = await FindAsync(f => f.OrganizationId == input.OrganizationId);
            var currentUser = EipHttpContext.CurrentUser();
            if (organization == null)
            {
                input.CreateTime = DateTime.Now;
                input.CreateUserId = currentUser.UserId;
                input.CreateUserName = currentUser.Name;
                input.UpdateTime = DateTime.Now;
                input.UpdateUserId = currentUser.UserId;
                input.UpdateUserName = currentUser.Name;
                operateStatus = await InsertAsync(input);
            }
            else
            {
                input.Id = organization.Id;

                input.CreateTime = organization.CreateTime;
                input.CreateUserId = organization.CreateUserId;
                input.CreateUserName = organization.CreateUserName;

                input.UpdateTime = DateTime.Now;
                input.UpdateUserId = currentUser.UserId;
                input.UpdateUserName = currentUser.Name;
                operateStatus = await UpdateAsync(input);
            }
            await GeneratingParentIds(input);
            return operateStatus;
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="role">岗位信息</param>
        /// <returns></returns>
        public async Task<OperateStatus<SystemOrganization>> FindById(IdInput input)
        {
            return OperateStatus<SystemOrganization>.Success(await FindAsync(f => f.OrganizationId == input.Id));
        }

        /// <summary>
        /// 删除组织机构下级数据
        /// 删除条件:
        /// 1:没有下级模块
        /// 2:没有权限数据
        /// 3:没有人员
        /// </summary>
        /// <param name="input">组织机构id</param>
        /// <returns></returns>
        public async Task<OperateStatus> Delete(IdInput<string> input)
        {
            var operateStatus = new OperateStatus();
            using (var fixture = new SqlDatabaseFixture())
            {
                //判断下级模块
                foreach (var id in input.Id.Split(','))
                {
                    var organizationId = Guid.Parse(id);
                    var orgs = (await CountAsync(f => f.ParentId == organizationId));
                    if (orgs > 0)
                    {
                        operateStatus.Code = ResultCode.Error;
                        operateStatus.Msg = ResourceSystem.具有下级项;
                        return operateStatus;
                    }

                    //判断是否具有人员
                    var permissionUsers = await fixture.Db.SystemUserInfo.CountAsync(c => c.OrganizationId == organizationId);
                    if (permissionUsers > 0)
                    {
                        operateStatus.Code = ResultCode.Error;
                        operateStatus.Msg = ResourceSystem.具有人员;
                        return operateStatus;
                    }

                    ////判断是否有角色
                    var orgRole = await _roleLogic.CountAsync(c => c.OrganizationId == organizationId);
                    if (orgRole > 0)
                    {
                        operateStatus.Code = ResultCode.Error;
                        operateStatus.Msg = ResourceSystem.具有角色;
                        return operateStatus;
                    }

                    //判断是否有组
                    var orgGroup = await _groupLogic.CountAsync(c => c.OrganizationId == organizationId);
                    if (orgGroup > 0)
                    {
                        operateStatus.Code = ResultCode.Error;
                        operateStatus.Msg = ResourceSystem.具有组;
                        return operateStatus;
                    }

                    //判断是否有岗位
                    var orgPost = await _postLogic.CountAsync(c => c.OrganizationId == organizationId);
                    if (orgPost > 0)
                    {
                        operateStatus.Code = ResultCode.Error;
                        operateStatus.Msg = ResourceSystem.具有岗位;
                        return operateStatus;
                    }
                }
                var trans = fixture.Db.BeginTransaction();
                try
                {
                    foreach (var id in input.Id.Split(','))
                    {
                        var organizationId = Guid.Parse(id);
                        await fixture.Db.SystemPermission.DeleteAsync(d => d.PrivilegeMasterValue == organizationId, trans);
                        await fixture.Db.SystemPermissionUser.DeleteAsync(d => d.PrivilegeMasterValue == organizationId, trans);
                        await fixture.Db.SystemOrganization.DeleteAsync(d => d.OrganizationId == organizationId, trans);
                    }
                    trans.Commit();
                    operateStatus.Msg = Chs.Successful;
                    operateStatus.Code = ResultCode.Success;
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    operateStatus.Msg = e.Message;
                    operateStatus.Code = ResultCode.Error;
                }
            }
            return operateStatus;
        }
        /// <summary>
        /// 批量生成代码
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus> GeneratingParentIds(SystemOrganization organization)
        {
            OperateStatus operateStatus = new OperateStatus();
            try
            {
                var organizations = (await FindAllAsync()).ToList();
                var organizationFind = organizations.FirstOrDefault(w => w.OrganizationId == organization.ParentId);
                if (organizationFind != null)
                {
                    organization.ParentIds = organizationFind.ParentIds.IsNullOrEmpty()
                        ? organization.OrganizationId.ToString()
                        : organizationFind.ParentIds + "," + organization.OrganizationId;
                    organization.ParentIdsName = organizationFind.ParentIdsName.IsNullOrEmpty()
                        ? organization.Name
                        : organizationFind.ParentIdsName + "/" + organization.Name;
                }
                else
                {
                    organization.ParentIds = organization.OrganizationId.ToString();
                    organization.ParentIdsName = organization.Name;
                }
                await UpdateAsync(organization);
                await GeneratingParentIds(organization, organizations);
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
        /// <param name="organization"></param>
        /// <param name="organizations"></param>
        private async Task GeneratingParentIds(SystemOrganization organization, IList<SystemOrganization> organizations)
        {
            string parentIds = organization.ParentIds;
            string parentIdsName = organization.ParentIdsName;
            var nextOrgs = organizations.Where(w => w.ParentId == organization.OrganizationId).ToList();
            foreach (var or in nextOrgs)
            {
                or.ParentIds = parentIds + "," + or.OrganizationId;
                or.ParentIdsName = parentIdsName + "/" + or.Name;
                or.ParentName = organization.Name;
                await UpdateAsync(or);
                await GeneratingParentIds(or, organizations);
            }
        }

        /// <summary>
        /// 冻结
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus> IsFreeze(IdInput input)
        {
            var data = await FindAsync(f => f.OrganizationId == input.Id);
            data.IsFreeze = !data.IsFreeze;
            return await UpdateAsync(data);
        }

        /// <summary>
        /// 导入用户
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        public async Task<OperateStatus<List<string>>> Import(IList<SystemOrganizationImportDto> input)
        {
            OperateStatus<List<string>> operateStatus = new OperateStatus<List<string>>();
            if (input.Count == 0)
            {
                operateStatus.Msg = "导入数据为空";
                return operateStatus;
            }

            var nameNull = input.Count(c => c.Name.IsNullOrEmpty());
            if (nameNull > 0)
            {
                operateStatus.Msg = "请确认用户真实姓名均已填写";
                return operateStatus;
            }

            //数据检查
            var currentUser = EipHttpContext.CurrentUser();
            List<SystemOrganization> systemOrganizationSaveInputs = new List<SystemOrganization>();
            List<string> errors = new List<string>();
            using (var fix = new SqlDatabaseFixture())
            {
                foreach (var organization in input)
                {
                    //组织架构是否存在
                    SystemOrganization org = new SystemOrganization();
                    org.Name = organization.Name.Trim();
                    org.CreateTime = DateTime.Now;
                    org.CreateUserId = currentUser.UserId;
                    org.CreateUserName = currentUser.Name;
                    org.UpdateTime = DateTime.Now;
                    org.UpdateUserId = currentUser.UserId;
                    org.UpdateUserName = currentUser.Name;
                    org.Nature = EnumOrgNature.部门.ToShort();
                    org.OrganizationId = CombUtil.NewComb();
                    if (organization.ParentName.IsNullOrEmpty())
                    {
                        org.ParentId = null;
                        systemOrganizationSaveInputs.Add(org);
                    }
                    else
                    {
                        //判断是否具有父级信息
                        var parent = systemOrganizationSaveInputs.FirstOrDefault(f => f.Name == organization.ParentName);
                        if (parent != null)
                        {
                            org.ParentId = parent.OrganizationId;
                            org.ParentName = organization.ParentName;
                        }
                        systemOrganizationSaveInputs.Add(org);
                    }
                }
            }
            if (errors.Any())
            {
                operateStatus.Data = errors;
                return operateStatus;
            }
            var operateStatusResult = new OperateStatus();
            foreach (var orgInfoMap in systemOrganizationSaveInputs)
            {
                var result = await Save(orgInfoMap);
                operateStatus.Code = result.Code;
                operateStatus.Msg = result.Msg;
            }
            return operateStatus;
        }
        #endregion
    }
}