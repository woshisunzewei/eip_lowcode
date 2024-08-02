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
    /// 权限用户业务逻辑
    /// </summary>
    public class SystemPermissionUserLogic : DapperAsyncLogic<SystemPermissionUser>, ISystemPermissionUserLogic
    {
        /// <summary>
        /// 保存各种用户:组织机构、岗位、组、人员
        /// </summary>
        /// <param name="master">类型</param>
        /// <param name="value">业务表Id：如组织机构Id、组Id、岗位Id、人员Id等</param>
        /// <param name="userIds">权限类型:组织机构、组、岗位、人员Id</param>
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
            //批量保存
            return await BulkInsertAsync(systemPermissionUsers);
        }

        /// <summary>
        /// 保存各种用户:组织机构、岗位、组、人员【先进行删除,再进行添加】
        /// </summary>
        /// <param name="master">类型</param>
        /// <param name="value">业务表Id：如组织机构Id、组Id、岗位Id、人员Id等</param>
        /// <param name="userIds">权限类型:组织机构、组、岗位、人员Id</param>
        /// <returns></returns>
        public async Task<OperateStatus> SavePermissionUserBeforeDelete(EnumPrivilegeMaster master,
            Guid value,
            IList<Guid> userIds)
        {
            var operateStatus = new OperateStatus();
            //删除
            await DeleteAsync(d => d.PrivilegeMaster == master.ToShort() && d.PrivilegeMasterValue == value);
            IList<SystemPermissionUser> systemPermissionUsers = userIds.Select(userId => new SystemPermissionUser
            {
                PrivilegeMaster = (byte)master,
                PrivilegeMasterUserId = userId,
                PrivilegeMasterValue = value
            }).ToList();
            if (systemPermissionUsers.Any())
            {
                //批量保存
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
        /// 保存用户权限类型
        /// </summary>
        /// <param name="privilegeMaster">类型</param>
        /// <param name="privilegeMasterUserId">业务表Id：如组织机构Id、组Id、岗位Id、人员Id等</param>
        /// <param name="privilegeMasterValue">权限类型:角色Id</param>
        /// <returns></returns>
        public async Task<OperateStatus> SavePermissionMasterValueBeforeDelete(EnumPrivilegeMaster privilegeMaster,
            Guid privilegeMasterUserId,
            IList<Guid> privilegeMasterValue)
        {
            var operateStatus = new OperateStatus();
            //删除
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
                //批量保存
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
        /// 删除用户
        /// </summary>
        /// <param name="input">用户Id</param>
        /// <returns></returns>
        public async Task<OperateStatus> DeletePermissionUser(IdInput input)
        {
            return await DeleteAsync(d => d.PrivilegeMasterUserId == input.Id);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="privilegeMasterUserId">用户Id</param>
        /// <param name="privilegeMasterValue">归属类型Id:组织机构、角色、岗位、组</param>
        /// <param name="privilegeMaster">归属人员类型:组织机构、角色、岗位、组</param>
        /// <returns></returns>
        public async Task<OperateStatus> DeletePrivilegeMasterUser(Guid privilegeMasterUserId,
            Guid privilegeMasterValue,
            EnumPrivilegeMaster privilegeMaster)
        {
            return await DeleteAsync(d => d.PrivilegeMasterUserId == privilegeMasterUserId && d.PrivilegeMaster == privilegeMaster.ToShort() && d.PrivilegeMasterValue == privilegeMasterValue);
        }

        /// <summary>
        /// 根据特权类型及特权id获取特权用户信息
        /// </summary>
        /// <param name="privilegeMaster">特权类型</param>
        /// <param name="privilegeMasterValue">特权id</param>
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
        /// 获取模块、字段对应拥有者信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<SystemPrivilegeDetailOutput>> FindSystemPrivilegeDetailOutputsByAccessAndValue(SystemPrivilegeDetailInput input)
        {
            using (var fixture = new SqlDatabaseFixture())
            {
                var output = new SystemPrivilegeDetailOutput();
                //获取角色、组、岗位数据
                var privilegeMaster = new List<short>
                {
                    EnumPrivilegeMaster.角色.ToShort(),
                    EnumPrivilegeMaster.组.ToShort(),
                    EnumPrivilegeMaster.岗位.ToShort(),
                    EnumPrivilegeMaster.组织架构.ToShort(),
                    EnumPrivilegeMaster.人员.ToShort()
                };
                var access = (byte)input.Access;
                var systemPermissionUsers = (await fixture.Db.SystemPermission.FindAllAsync(f => f.PrivilegeAccessValue == input.Id
                                                                                                 && f.PrivilegeAccess == access
                                                                                                 && privilegeMaster.Contains(f.PrivilegeMaster))).ToList().
                    DistinctByExtension(p => new { p.PrivilegeMasterValue, p.PrivilegeMaster }).ToList();
                //角色集合
                var roleId = systemPermissionUsers.Where(w => w.PrivilegeMaster == EnumPrivilegeMaster.角色.ToShort()).Select(s => s.PrivilegeMasterValue).ToList();
                var roles = (await fixture.Db.SystemRole.FindAllAsync(f => roleId.Contains(f.RoleId))).ToList();

                //组
                var groupId = systemPermissionUsers.Where(w => w.PrivilegeMaster == EnumPrivilegeMaster.组.ToShort()).Select(s => s.PrivilegeMasterValue).ToList();
                var groups = (await fixture.Db.SystemGroup.FindAllAsync(f => groupId.Contains(f.GroupId))).ToList();

                //岗位
                var postId = systemPermissionUsers.Where(w => w.PrivilegeMaster == EnumPrivilegeMaster.岗位.ToShort()).Select(s => s.PrivilegeMasterValue).ToList();
                var posts = (await fixture.Db.SystemPost.FindAllAsync(f => postId.Contains(f.PostId))).ToList();

                //人员
                var userId = systemPermissionUsers.Where(w => w.PrivilegeMaster == EnumPrivilegeMaster.人员.ToShort()).Select(s => s.PrivilegeMasterValue).ToList();
                var users = (await fixture.Db.SystemUserInfo.FindAllAsync(f => userId.Contains(f.UserId))).ToList();

                //组织架构
                var organizationId = systemPermissionUsers.Where(w => w.PrivilegeMaster == EnumPrivilegeMaster.组织架构.ToShort()).Select(s => s.PrivilegeMasterValue).ToList();
                var organizations = (await fixture.Db.SystemOrganization.FindAllAsync(f => organizationId.Contains(f.OrganizationId))).ToList();

                IList<SystemPrivilegeDetailListOutput> privilegeDetailDtos = new List<SystemPrivilegeDetailListOutput>();
                foreach (var permissionUser in systemPermissionUsers)
                {
                    if (permissionUser.PrivilegeMaster == EnumPrivilegeMaster.角色.ToShort())
                    {
                        //查询角色
                        var role = roles.FirstOrDefault(w => w.RoleId == permissionUser.PrivilegeMasterValue);
                        if (role == null) continue;
                        privilegeDetailDtos.Add(new SystemPrivilegeDetailListOutput
                        {
                            Name = role.Name,
                            OrganizationId = role.OrganizationId,
                            PrivilegeMaster = EnumPrivilegeMaster.角色,
                            PrivilegeMasterValue = permissionUser.PrivilegeMasterValue
                        });
                    }
                    else if (permissionUser.PrivilegeMaster == EnumPrivilegeMaster.组.ToShort())
                    {
                        var group = groups.FirstOrDefault(w => w.GroupId == permissionUser.PrivilegeMasterValue);
                        if (group == null) continue;
                        privilegeDetailDtos.Add(new SystemPrivilegeDetailListOutput
                        {
                            Name = group.Name,
                            OrganizationId = group.OrganizationId,
                            PrivilegeMaster = EnumPrivilegeMaster.组,
                            PrivilegeMasterValue = permissionUser.PrivilegeMasterValue
                        });
                    }
                    else if (permissionUser.PrivilegeMaster == EnumPrivilegeMaster.岗位.ToShort())
                    {
                        var post = posts.FirstOrDefault(w => w.PostId == permissionUser.PrivilegeMasterValue);
                        if (post == null) continue;
                        privilegeDetailDtos.Add(new SystemPrivilegeDetailListOutput
                        {
                            Name = post.Name,
                            OrganizationId = post.OrganizationId,
                            PrivilegeMaster = EnumPrivilegeMaster.岗位,
                            PrivilegeMasterValue = permissionUser.PrivilegeMasterValue
                        });
                    }

                    else if (permissionUser.PrivilegeMaster == EnumPrivilegeMaster.人员.ToShort())
                    {
                        var user = users.FirstOrDefault(w => w.UserId == permissionUser.PrivilegeMasterValue);
                        if (user == null) continue;
                        privilegeDetailDtos.Add(new SystemPrivilegeDetailListOutput
                        {
                            Name = user.Name,
                            OrganizationId = user.OrganizationId,
                            PrivilegeMaster = EnumPrivilegeMaster.人员,
                            PrivilegeMasterValue = permissionUser.PrivilegeMasterValue
                        });
                    }

                    else if (permissionUser.PrivilegeMaster == EnumPrivilegeMaster.组织架构.ToShort())
                    {
                        var organization = organizations.FirstOrDefault(w => w.OrganizationId == permissionUser.PrivilegeMasterValue);
                        if (organization == null) continue;
                        privilegeDetailDtos.Add(new SystemPrivilegeDetailListOutput
                        {
                            Name = organization.Name,
                            OrganizationId = organization.OrganizationId,
                            PrivilegeMaster = EnumPrivilegeMaster.组织架构,
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
                            //查找上级
                            var dicinfo = allOrgs.FirstOrDefault(w => w.OrganizationId.ToString() == parent);
                            if (dicinfo != null) description += dicinfo.Name + "/";
                        }

                        if (!description.IsNullOrEmpty())
                            description = description.TrimEnd('/');
                    }

                    dto.Organization = description;
                }
                //角色
                output.Role = privilegeDetailDtos.Where(w => w.PrivilegeMaster == EnumPrivilegeMaster.角色).ToList();
                //组
                output.Group = privilegeDetailDtos.Where(w => w.PrivilegeMaster == EnumPrivilegeMaster.组).ToList();
                //岗位
                output.Post = privilegeDetailDtos.Where(w => w.PrivilegeMaster == EnumPrivilegeMaster.岗位).ToList();
                //组织机构
                output.Organization = privilegeDetailDtos.Where(w => w.PrivilegeMaster == EnumPrivilegeMaster.组织架构).ToList();
                //用户
                output.User = privilegeDetailDtos.Where(w => w.PrivilegeMaster == EnumPrivilegeMaster.人员).ToList();
                return OperateStatus<SystemPrivilegeDetailOutput>.Success(output);
            }
        }

        /// <summary>
        /// 删除用户对应权限数据
        /// </summary>
        /// <param name="privilegeMasterUserId">用户Id</param>
        /// <param name="privilegeMaster">归属人员类型:组织机构、角色、岗位、组</param>
        /// <returns></returns>
        public async Task<OperateStatus> DeletePrivilegeMasterUser(Guid privilegeMasterUserId, EnumPrivilegeMaster privilegeMaster)
        {
            var privilegeMasterShort = privilegeMaster.ToShort();
            return await DeleteAsync(d =>
                d.PrivilegeMaster == privilegeMasterShort && d.PrivilegeMasterUserId == privilegeMasterUserId);
        }


        /// <summary>
        /// 删除用户对应权限数据
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <returns></returns>
        public async Task<OperateStatus> DeletePrivilegeMasterUser(Guid userId)
        {
            return await DeleteAsync(d => d.PrivilegeMasterUserId == userId);
        }
    }
}