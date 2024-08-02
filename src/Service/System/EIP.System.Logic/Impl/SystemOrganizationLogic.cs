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
namespace EIP.System.Logic.Impl
{
    /// <summary>
    /// ��֯����
    /// </summary>
    public class SystemOrganizationLogic : DapperAsyncLogic<SystemOrganization>, ISystemOrganizationLogic
    {
        #region ���캯��

        private readonly ISystemOrganizationRepository _organizationRepository;
        private readonly ISystemRoleLogic _roleLogic;
        private readonly ISystemGroupLogic _groupLogic;
        private readonly ISystemPostLogic _postLogic;

        /// <summary>
        /// ��֯����
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

        #region ����

        /// <summary>
        /// ͬ����ȡ����������
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
        /// ��������Ȩ����֯����
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<BaseTree>>> FindDataPermissionTree(SystemOrganizationDataPermissionInput input)
        {
            if (input.DataSql.IsNullOrEmpty())
                return OperateStatus<IEnumerable<BaseTree>>.Success(new List<BaseTree>());
            var datas = (await _organizationRepository.FindDataPermissionTree(input)).ToList();
            //�жϷ�Χ����
            string sql = "";
            switch (input.Range)
            {
                //��ǰ��˾
                case 3:
                    var now = await FindByIdAsync(input.PrincipalUser.OrganizationId);
                    var allOrgs = now.ParentIds.Split(',');
                    datas = datas.Where(w => allOrgs.Contains(w.id.ToString())).ToList();
                    break;
                //��ǰ��˾���¼�����
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
                //��ǰ��˾����¼�˲���
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
                        //ƴ��
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
            //��ģ��
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
            //������ģ��
            foreach (var permission in outputs)
            {
                //�ж��ж��ٸ�ģ��
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
        /// ���ݵ�ǰ�ڵ㣬�����ӽڵ�
        /// </summary>
        /// <param name="allOrgs">TreeEntity�ļ���</param>
        /// <param name="currTreeEntity">��ǰ�ڵ�</param>
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
        /// ���ݸ�����ѯ�¼�
        /// </summary>
        /// <param name="input">����id</param>
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
            return OperateStatus<IEnumerable<SystemOrganizationOutput>>.Success(outputs);
        }
        /// <summary>
        /// ��������Ȩ����֯����
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<BaseTree>>> FindDataPermission(SystemOrganizationDataPermissionInput input)
        {
            if (input.DataSql.IsNullOrEmpty())
                return OperateStatus<IEnumerable<BaseTree>>.Success(new List<BaseTree>());
            return OperateStatus<IEnumerable<BaseTree>>.Success((await _organizationRepository.FindDataPermissionTree(input)).ToList());
        }
        /// <summary>
        /// ������֯����
        /// </summary>
        /// <param name="input">��֯����</param>
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
        /// ����Id��ȡ
        /// </summary>
        /// <param name="role">��λ��Ϣ</param>
        /// <returns></returns>
        public async Task<OperateStatus<SystemOrganization>> FindById(IdInput input)
        {
            return OperateStatus<SystemOrganization>.Success(await FindAsync(f => f.OrganizationId == input.Id));
        }

        /// <summary>
        /// ɾ����֯�����¼�����
        /// ɾ������:
        /// 1:û���¼�ģ��
        /// 2:û��Ȩ������
        /// 3:û����Ա
        /// </summary>
        /// <param name="input">��֯����id</param>
        /// <returns></returns>
        public async Task<OperateStatus> Delete(IdInput<string> input)
        {
            var operateStatus = new OperateStatus();
            using (var fixture = new SqlDatabaseFixture())
            {
                //�ж��¼�ģ��
                foreach (var id in input.Id.Split(','))
                {
                    var organizationId = Guid.Parse(id);
                    var orgs = (await CountAsync(f => f.ParentId == organizationId));
                    if (orgs > 0)
                    {
                        operateStatus.Code = ResultCode.Error;
                        operateStatus.Msg = ResourceSystem.�����¼���;
                        return operateStatus;
                    }

                    //�ж��Ƿ������Ա
                    var permissionUsers = await fixture.Db.SystemUserInfo.CountAsync(c => c.OrganizationId == organizationId);
                    if (permissionUsers > 0)
                    {
                        operateStatus.Code = ResultCode.Error;
                        operateStatus.Msg = ResourceSystem.������Ա;
                        return operateStatus;
                    }

                    ////�ж��Ƿ��н�ɫ
                    var orgRole = await _roleLogic.CountAsync(c => c.OrganizationId == organizationId);
                    if (orgRole > 0)
                    {
                        operateStatus.Code = ResultCode.Error;
                        operateStatus.Msg = ResourceSystem.���н�ɫ;
                        return operateStatus;
                    }

                    //�ж��Ƿ�����
                    var orgGroup = await _groupLogic.CountAsync(c => c.OrganizationId == organizationId);
                    if (orgGroup > 0)
                    {
                        operateStatus.Code = ResultCode.Error;
                        operateStatus.Msg = ResourceSystem.������;
                        return operateStatus;
                    }

                    //�ж��Ƿ��и�λ
                    var orgPost = await _postLogic.CountAsync(c => c.OrganizationId == organizationId);
                    if (orgPost > 0)
                    {
                        operateStatus.Code = ResultCode.Error;
                        operateStatus.Msg = ResourceSystem.���и�λ;
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
        /// �������ɴ���
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
        /// �ݹ��ȡ����
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
        /// ����
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus> IsFreeze(IdInput input)
        {
            var data = await FindAsync(f => f.OrganizationId == input.Id);
            data.IsFreeze = !data.IsFreeze;
            return await UpdateAsync(data);
        }

        /// <summary>
        /// �����û�
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        public async Task<OperateStatus<List<string>>> Import(IList<SystemOrganizationImportDto> input)
        {
            OperateStatus<List<string>> operateStatus = new OperateStatus<List<string>>();
            if (input.Count == 0)
            {
                operateStatus.Msg = "��������Ϊ��";
                return operateStatus;
            }

            var nameNull = input.Count(c => c.Name.IsNullOrEmpty());
            if (nameNull > 0)
            {
                operateStatus.Msg = "��ȷ���û���ʵ����������д";
                return operateStatus;
            }

            //���ݼ��
            var currentUser = EipHttpContext.CurrentUser();
            List<SystemOrganization> systemOrganizationSaveInputs = new List<SystemOrganization>();
            List<string> errors = new List<string>();
            using (var fix = new SqlDatabaseFixture())
            {
                foreach (var organization in input)
                {
                    //��֯�ܹ��Ƿ����
                    SystemOrganization org = new SystemOrganization();
                    org.Name = organization.Name.Trim();
                    org.CreateTime = DateTime.Now;
                    org.CreateUserId = currentUser.UserId;
                    org.CreateUserName = currentUser.Name;
                    org.UpdateTime = DateTime.Now;
                    org.UpdateUserId = currentUser.UserId;
                    org.UpdateUserName = currentUser.Name;
                    org.Nature = EnumOrgNature.����.ToShort();
                    org.OrganizationId = CombUtil.NewComb();
                    if (organization.ParentName.IsNullOrEmpty())
                    {
                        org.ParentId = null;
                        systemOrganizationSaveInputs.Add(org);
                    }
                    else
                    {
                        //�ж��Ƿ���и�����Ϣ
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