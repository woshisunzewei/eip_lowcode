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
    /// Ȩ���û�ҵ���߼�
    /// </summary>
    public class SystemPermissionUserLogic : DapperAsyncLogic<SystemPermissionUser>, ISystemPermissionUserLogic
    {
        /// <summary>
        /// ��������û�:��֯��������λ���顢��Ա
        /// </summary>
        /// <param name="master">����</param>
        /// <param name="value">ҵ���Id������֯����Id����Id����λId����ԱId��</param>
        /// <param name="userIds">Ȩ������:��֯�������顢��λ����ԱId</param>
        /// <returns></returns>
        public async Task<OperateStatus> SavePermissionUser(EnumPrivilegeMaster master,
            Guid value,
            IList<Guid> userIds)
        {
            IList<SystemPermissionUser> systemPermissionUsers = userIds.Select(userId => new SystemPermissionUser
            {
                PrivilegeMaster = (byte)master,
                PrivilegeMasterUserId = userId,
                PrivilegeMasterValue = value
            }).ToList();
            //��������
            return await BulkInsertAsync(systemPermissionUsers);
        }

        /// <summary>
        /// ��������û�:��֯��������λ���顢��Ա���Ƚ���ɾ��,�ٽ�����ӡ�
        /// </summary>
        /// <param name="master">����</param>
        /// <param name="value">ҵ���Id������֯����Id����Id����λId����ԱId��</param>
        /// <param name="userIds">Ȩ������:��֯�������顢��λ����ԱId</param>
        /// <returns></returns>
        public async Task<OperateStatus> SavePermissionUserBeforeDelete(EnumPrivilegeMaster master,
            Guid value,
            IList<Guid> userIds)
        {
            var operateStatus = new OperateStatus();
            //ɾ��
            await DeleteAsync(d => d.PrivilegeMaster == master.ToShort() && d.PrivilegeMasterValue == value);
            IList<SystemPermissionUser> systemPermissionUsers = userIds.Select(userId => new SystemPermissionUser
            {
                PrivilegeMaster = (byte)master,
                PrivilegeMasterUserId = userId,
                PrivilegeMasterValue = value
            }).ToList();
            if (systemPermissionUsers.Any())
            {
                //��������
                operateStatus = await BulkInsertAsync(systemPermissionUsers);
            }
            else
            {
                operateStatus.Code = ResultCode.Success;
                operateStatus.Msg = Chs.Successful;
            }
            return operateStatus;
        }

        /// <summary>
        /// �����û�Ȩ������
        /// </summary>
        /// <param name="privilegeMaster">����</param>
        /// <param name="privilegeMasterUserId">ҵ���Id������֯����Id����Id����λId����ԱId��</param>
        /// <param name="privilegeMasterValue">Ȩ������:��ɫId</param>
        /// <returns></returns>
        public async Task<OperateStatus> SavePermissionMasterValueBeforeDelete(EnumPrivilegeMaster privilegeMaster,
            Guid privilegeMasterUserId,
            IList<Guid> privilegeMasterValue)
        {
            var operateStatus = new OperateStatus();
            //ɾ��
            await DeleteAsync(d => d.PrivilegeMaster == privilegeMaster.ToShort() && d.PrivilegeMasterUserId == privilegeMasterUserId);
            IList<SystemPermissionUser> systemPermissionUsers =
                privilegeMasterValue.Select(privilegeMasterValue => new SystemPermissionUser
                {
                    PrivilegeMaster = (byte)privilegeMaster,
                    PrivilegeMasterUserId = privilegeMasterUserId,
                    PrivilegeMasterValue = privilegeMasterValue,
                    IsRelationOrganization = false
                }).ToList();
            if (systemPermissionUsers.Any())
            {
                //��������
                operateStatus = await BulkInsertAsync(systemPermissionUsers);
            }
            else
            {
                operateStatus.Code = ResultCode.Success;
                operateStatus.Msg = Chs.Successful;
            }
            return operateStatus;
        }

        /// <summary>
        /// ɾ���û�
        /// </summary>
        /// <param name="input">�û�Id</param>
        /// <returns></returns>
        public async Task<OperateStatus> DeletePermissionUser(IdInput input)
        {
            return await DeleteAsync(d => d.PrivilegeMasterUserId == input.Id);
        }

        /// <summary>
        /// ɾ���û�
        /// </summary>
        /// <param name="privilegeMasterUserId">�û�Id</param>
        /// <param name="privilegeMasterValue">��������Id:��֯��������ɫ����λ����</param>
        /// <param name="privilegeMaster">������Ա����:��֯��������ɫ����λ����</param>
        /// <returns></returns>
        public async Task<OperateStatus> DeletePrivilegeMasterUser(Guid privilegeMasterUserId,
            Guid privilegeMasterValue,
            EnumPrivilegeMaster privilegeMaster)
        {
            return await DeleteAsync(d => d.PrivilegeMasterUserId == privilegeMasterUserId && d.PrivilegeMaster == privilegeMaster.ToShort() && d.PrivilegeMasterValue == privilegeMasterValue);
        }

        /// <summary>
        /// ������Ȩ���ͼ���Ȩid��ȡ��Ȩ�û���Ϣ
        /// </summary>
        /// <param name="privilegeMaster">��Ȩ����</param>
        /// <param name="privilegeMasterValue">��Ȩid</param>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemPermissionUser>>> FindPermissionUsersByPrivilegeMasterAdnPrivilegeMasterValue(
            EnumPrivilegeMaster privilegeMaster,
            Guid? privilegeMasterValue = null)
        {
            return OperateStatus<IEnumerable<SystemPermissionUser>>.Success(!privilegeMasterValue.IsNullOrEmptyGuid() ?
            await FindAllAsync(f => f.PrivilegeMaster == privilegeMaster.ToShort() && f.PrivilegeMasterValue == privilegeMasterValue) :
            await FindAllAsync(f => f.PrivilegeMaster == privilegeMaster.ToShort()));
        }

        /// <summary>
        /// ��ȡģ�顢�ֶζ�Ӧӵ������Ϣ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<SystemPrivilegeDetailOutput>> FindSystemPrivilegeDetailOutputsByAccessAndValue(SystemPrivilegeDetailInput input)
        {
            using (var fixture = new SqlDatabaseFixture())
            {
                var output = new SystemPrivilegeDetailOutput();
                //��ȡ��ɫ���顢��λ����
                var privilegeMaster = new List<short>
                {
                    EnumPrivilegeMaster.��ɫ.ToShort(),
                    EnumPrivilegeMaster.��.ToShort(),
                    EnumPrivilegeMaster.��λ.ToShort(),
                    EnumPrivilegeMaster.��֯�ܹ�.ToShort(),
                    EnumPrivilegeMaster.��Ա.ToShort()
                };
                var access = (byte)input.Access;
                var systemPermissionUsers = (await fixture.Db.SystemPermission.FindAllAsync(f => f.PrivilegeAccessValue == input.Id
                                                                                                 && f.PrivilegeAccess == access
                                                                                                 && privilegeMaster.Contains(f.PrivilegeMaster))).ToList().
                    DistinctByExtension(p => new { p.PrivilegeMasterValue, p.PrivilegeMaster }).ToList();
                //��ɫ����
                var roleId = systemPermissionUsers.Where(w => w.PrivilegeMaster == EnumPrivilegeMaster.��ɫ.ToShort()).Select(s => s.PrivilegeMasterValue).ToList();
                var roles = (await fixture.Db.SystemRole.FindAllAsync(f => roleId.Contains(f.RoleId))).ToList();

                //��
                var groupId = systemPermissionUsers.Where(w => w.PrivilegeMaster == EnumPrivilegeMaster.��.ToShort()).Select(s => s.PrivilegeMasterValue).ToList();
                var groups = (await fixture.Db.SystemGroup.FindAllAsync(f => groupId.Contains(f.GroupId))).ToList();

                //��λ
                var postId = systemPermissionUsers.Where(w => w.PrivilegeMaster == EnumPrivilegeMaster.��λ.ToShort()).Select(s => s.PrivilegeMasterValue).ToList();
                var posts = (await fixture.Db.SystemPost.FindAllAsync(f => postId.Contains(f.PostId))).ToList();

                //��Ա
                var userId = systemPermissionUsers.Where(w => w.PrivilegeMaster == EnumPrivilegeMaster.��Ա.ToShort()).Select(s => s.PrivilegeMasterValue).ToList();
                var users = (await fixture.Db.SystemUserInfo.FindAllAsync(f => userId.Contains(f.UserId))).ToList();

                //��֯�ܹ�
                var organizationId = systemPermissionUsers.Where(w => w.PrivilegeMaster == EnumPrivilegeMaster.��֯�ܹ�.ToShort()).Select(s => s.PrivilegeMasterValue).ToList();
                var organizations = (await fixture.Db.SystemOrganization.FindAllAsync(f => organizationId.Contains(f.OrganizationId))).ToList();

                IList<SystemPrivilegeDetailListOutput> privilegeDetailDtos = new List<SystemPrivilegeDetailListOutput>();
                foreach (var permissionUser in systemPermissionUsers)
                {
                    if (permissionUser.PrivilegeMaster == EnumPrivilegeMaster.��ɫ.ToShort())
                    {
                        //��ѯ��ɫ
                        var role = roles.FirstOrDefault(w => w.RoleId == permissionUser.PrivilegeMasterValue);
                        if (role == null) continue;
                        privilegeDetailDtos.Add(new SystemPrivilegeDetailListOutput
                        {
                            Name = role.Name,
                            OrganizationId = role.OrganizationId,
                            PrivilegeMaster = EnumPrivilegeMaster.��ɫ,
                            PrivilegeMasterValue = permissionUser.PrivilegeMasterValue
                        });
                    }
                    else if (permissionUser.PrivilegeMaster == EnumPrivilegeMaster.��.ToShort())
                    {
                        var group = groups.FirstOrDefault(w => w.GroupId == permissionUser.PrivilegeMasterValue);
                        if (group == null) continue;
                        privilegeDetailDtos.Add(new SystemPrivilegeDetailListOutput
                        {
                            Name = group.Name,
                            OrganizationId = group.OrganizationId,
                            PrivilegeMaster = EnumPrivilegeMaster.��,
                            PrivilegeMasterValue = permissionUser.PrivilegeMasterValue
                        });
                    }
                    else if (permissionUser.PrivilegeMaster == EnumPrivilegeMaster.��λ.ToShort())
                    {
                        var post = posts.FirstOrDefault(w => w.PostId == permissionUser.PrivilegeMasterValue);
                        if (post == null) continue;
                        privilegeDetailDtos.Add(new SystemPrivilegeDetailListOutput
                        {
                            Name = post.Name,
                            OrganizationId = post.OrganizationId,
                            PrivilegeMaster = EnumPrivilegeMaster.��λ,
                            PrivilegeMasterValue = permissionUser.PrivilegeMasterValue
                        });
                    }

                    else if (permissionUser.PrivilegeMaster == EnumPrivilegeMaster.��Ա.ToShort())
                    {
                        var user = users.FirstOrDefault(w => w.UserId == permissionUser.PrivilegeMasterValue);
                        if (user == null) continue;
                        privilegeDetailDtos.Add(new SystemPrivilegeDetailListOutput
                        {
                            Name = user.Name,
                            OrganizationId = user.OrganizationId,
                            PrivilegeMaster = EnumPrivilegeMaster.��Ա,
                            PrivilegeMasterValue = permissionUser.PrivilegeMasterValue
                        });
                    }

                    else if (permissionUser.PrivilegeMaster == EnumPrivilegeMaster.��֯�ܹ�.ToShort())
                    {
                        var organization = organizations.FirstOrDefault(w => w.OrganizationId == permissionUser.PrivilegeMasterValue);
                        if (organization == null) continue;
                        privilegeDetailDtos.Add(new SystemPrivilegeDetailListOutput
                        {
                            Name = organization.Name,
                            OrganizationId = organization.OrganizationId,
                            PrivilegeMaster = EnumPrivilegeMaster.��֯�ܹ�,
                            PrivilegeMasterValue = permissionUser.PrivilegeMasterValue
                        });
                    }
                }

                var allOrgs = (await fixture.Db.SystemOrganization.FindAllAsync()).ToList();
                foreach (var dto in privilegeDetailDtos)
                {
                    string description = string.Empty;
                    var organization = allOrgs.FirstOrDefault(w => w.OrganizationId == dto.OrganizationId);
                    if (organization != null && !organization.ParentIds.IsNullOrEmpty())
                    {
                        foreach (var parent in organization.ParentIds.Split(','))
                        {
                            //�����ϼ�
                            var dicinfo = allOrgs.FirstOrDefault(w => w.OrganizationId.ToString() == parent);
                            if (dicinfo != null) description += dicinfo.Name + "/";
                        }

                        if (!description.IsNullOrEmpty())
                            description = description.TrimEnd('/');
                    }

                    dto.Organization = description;
                }
                //��ɫ
                output.Role = privilegeDetailDtos.Where(w => w.PrivilegeMaster == EnumPrivilegeMaster.��ɫ).ToList();
                //��
                output.Group = privilegeDetailDtos.Where(w => w.PrivilegeMaster == EnumPrivilegeMaster.��).ToList();
                //��λ
                output.Post = privilegeDetailDtos.Where(w => w.PrivilegeMaster == EnumPrivilegeMaster.��λ).ToList();
                //��֯����
                output.Organization = privilegeDetailDtos.Where(w => w.PrivilegeMaster == EnumPrivilegeMaster.��֯�ܹ�).ToList();
                //�û�
                output.User = privilegeDetailDtos.Where(w => w.PrivilegeMaster == EnumPrivilegeMaster.��Ա).ToList();
                return OperateStatus<SystemPrivilegeDetailOutput>.Success(output);
            }
        }

        /// <summary>
        /// ɾ���û���ӦȨ������
        /// </summary>
        /// <param name="privilegeMasterUserId">�û�Id</param>
        /// <param name="privilegeMaster">������Ա����:��֯��������ɫ����λ����</param>
        /// <returns></returns>
        public async Task<OperateStatus> DeletePrivilegeMasterUser(Guid privilegeMasterUserId, EnumPrivilegeMaster privilegeMaster)
        {
            var privilegeMasterShort = privilegeMaster.ToShort();
            return await DeleteAsync(d =>
                d.PrivilegeMaster == privilegeMasterShort && d.PrivilegeMasterUserId == privilegeMasterUserId);
        }


        /// <summary>
        /// ɾ���û���ӦȨ������
        /// </summary>
        /// <param name="userId">�û�Id</param>
        /// <returns></returns>
        public async Task<OperateStatus> DeletePrivilegeMasterUser(Guid userId)
        {
            return await DeleteAsync(d => d.PrivilegeMasterUserId == userId);
        }
    }
}