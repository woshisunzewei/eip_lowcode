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
using EIP.System.Models.Dtos.Role;

namespace EIP.System.Logic.Impl
{
    /// <summary>
    /// ��ɫҵ���߼�
    /// </summary>
    public class SystemRoleLogic : DapperAsyncLogic<SystemRole>, ISystemRoleLogic
    {
        #region ���캯��

        private readonly ISystemPermissionUserLogic _permissionUserLogic;
        private readonly ISystemPermissionLogic _permissionLogic;
        private readonly ISystemRoleRepository _roleRepository;

        /// <summary>
        /// ��ɫҵ���߼�
        /// </summary>
        /// <param name="permissionUserLogic"></param>
        /// <param name="permissionLogic"></param>
        /// <param name="roleRepository"></param>
        public SystemRoleLogic(
            ISystemPermissionUserLogic permissionUserLogic,
            ISystemPermissionLogic permissionLogic,
            ISystemRoleRepository roleRepository)
        {
            _permissionUserLogic = permissionUserLogic;
            _permissionLogic = permissionLogic;
            _roleRepository = roleRepository;
        }

        #endregion

        #region ����

        /// <summary>
        /// ������֯����Id��ѯ��ɫ��Ϣ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<PagedResults<SystemRolesFindOutput>>> Find(SystemRolesFindInput input)
        {
            if (input.DataSql.IsNullOrEmpty()) return OperateStatus<PagedResults<SystemRolesFindOutput>>.Success(new PagedResults<SystemRolesFindOutput>());
            return OperateStatus<PagedResults<SystemRolesFindOutput>>.Success(await _roleRepository.FindPagingRole(input));
        }

        /// <summary>
        /// ��ȡ����δ�����ɫ
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemRoleSelectOutput>>> Select()
        {
            return OperateStatus<IEnumerable<SystemRoleSelectOutput>>.Success((await FindAllAsync(f => f.IsFreeze == false)).MapToList<SystemRole,SystemRoleSelectOutput>());
        }

        /// <summary>
        /// ��ȡ����δ�����ɫ
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemRole>>> All()
        {
            return OperateStatus<IEnumerable<SystemRole>>.Success(await FindAllAsync(f => f.IsFreeze == false));
        }

        /// <summary>
        /// �����λ��Ϣ
        /// </summary>
        /// <param name="input">��λ��Ϣ</param>
        /// <returns></returns>
        public async Task<OperateStatus> Save(SystemRole input)
        {
            OperateStatus operateStatus;
            input.CanbeDelete = true;
            var role = await FindAsync(f => f.RoleId == input.RoleId);
            var currentUser = EipHttpContext.CurrentUser();
            if (role == null)
            {
                input.CreateTime = DateTime.Now;
                input.CreateUserId = currentUser.UserId;
                input.CreateUserName = currentUser.Name;
                input.UpdateTime = DateTime.Now;
                input.UpdateUserId = currentUser.UserId;
                input.UpdateUserName = currentUser.Name;
                input.RoleId = Guid.NewGuid();
                operateStatus = await InsertAsync(input);
            }
            else
            {
                input.Id = role.Id;
                input.CreateTime = role.CreateTime;
                input.CreateUserId = role.CreateUserId;
                input.CreateUserName = role.CreateUserName;

                input.UpdateTime = DateTime.Now;
                input.UpdateUserId = currentUser.UserId;
                input.UpdateUserName = currentUser.Name;
                operateStatus = await UpdateAsync(input);
            }
            return operateStatus;
        }

        /// <summary>
        /// ����Id��ȡ
        /// </summary>
        /// <param name="role">��λ��Ϣ</param>
        /// <returns></returns>
        public async Task<OperateStatus<SystemRole>> FindById(IdInput input)
        {
            return OperateStatus<SystemRole>.Success(await FindAsync(f => f.RoleId == input.Id));
        }
        /// <summary>
        /// ��ɫ����
        /// </summary>
        /// <param name="input">��ɫId</param>
        /// <returns></returns>
        public async Task<OperateStatus> Copy(SystemCopyInput input)
        {
            var operateStatus = new OperateStatus();
            try
            {
                //��ȡ��ɫ��Ϣ
                var role = await FindAsync(f => f.RoleId == input.Id);
                role.RoleId = CombUtil.NewComb();
                role.Name = input.Name;

                //��ȡ�ý�ɫӵ�е�Ȩ�޼���Ա
                var allUser = (await _permissionUserLogic.FindPermissionUsersByPrivilegeMasterAdnPrivilegeMasterValue(EnumPrivilegeMaster.��ɫ,
                    input.Id)).Data.ToList();
                var allPer = (await _permissionLogic.FindSystemPermissionsByPrivilegeMasterValueAndValue(EnumPrivilegeMaster.��ɫ, input.Id)).Data.ToList();
                foreach (var user in allUser)
                {
                    user.PrivilegeMasterValue = role.RoleId;
                }
                foreach (var per in allPer)
                {
                    per.PrivilegeMasterValue = role.RoleId;
                }

                using (var fixture = new SqlDatabaseFixture())
                {
                    var trans = fixture.Db.BeginTransaction();
                    try
                    {
                        if (allUser.Any())
                            await fixture.Db.SystemPermissionUser.BulkInsertAsync(allUser, trans);
                        if (allPer.Any())
                            await fixture.Db.SystemPermission.BulkInsertAsync(allPer, trans);
                        await fixture.Db.SystemRole.InsertAsync(role, trans);
                    }
                    catch (Exception e)
                    {
                        trans.Rollback();
                        operateStatus.Msg = e.Message;
                        return operateStatus;
                    }
                    trans.Commit();
                    operateStatus.Msg = Chs.Successful;
                    operateStatus.Code = ResultCode.Success;
                }
            }
            catch (Exception e)
            {
                operateStatus.Msg = e.Message;
            }
            return operateStatus;
        }

        /// <summary>
        /// ɾ����ɫ��Ϣ
        /// </summary>
        /// <param name="input">��ɫId</param>
        /// <returns></returns>
        public async Task<OperateStatus> Delete(IdInput<string> input)
        {
            var operateStatus = new OperateStatus();
            if (input.Id.IsNullOrEmpty())
                return operateStatus;
            using (var fixture = new SqlDatabaseFixture())
            {
                foreach (var id in input.Id.Split(','))
                {
                    var roleId = Guid.Parse(id);
                    var roleInfo = await FindAsync(f => f.RoleId == roleId);
                    if (roleInfo == null)
                    {
                        operateStatus.Code = ResultCode.Error;
                        operateStatus.Msg = ResourceSystem.��ɫ������;
                        return operateStatus;
                    }
                    //�ж��Ƿ������Ա
                    //var permissionUsers = await _permissionUserLogic.FindPermissionUsersByPrivilegeMasterAdnPrivilegeMasterValue(EnumPrivilegeMaster.��ɫ, roleId);
                    //if (permissionUsers.Any())
                    //{
                    //    operateStatus.Code = ResultCode.Error;
                    //    operateStatus.Message = ResourceSystem.������Ա;
                    //    return operateStatus;
                    //}

                    ////�ж��Ƿ���а�ťȨ��
                    //var functionPermissions = await _permissionLogic.FindPermissionByPrivilegeMasterValue(
                    //    new SystemPermissionByPrivilegeMasterValueInput
                    //    {
                    //        PrivilegeAccess = EnumPrivilegeAccess.ģ�鰴ť,
                    //        PrivilegeMasterValue = roleId,
                    //        PrivilegeMaster = EnumPrivilegeMaster.��ɫ
                    //    });
                    //if (functionPermissions.Any())
                    //{
                    //    operateStatus.Code = ResultCode.Error;
                    //    operateStatus.Message = ResourceSystem.���й�����Ȩ��;
                    //    return operateStatus;
                    //}

                    ////�ж��Ƿ����ģ��Ȩ��
                    //var menuPermissions = await _permissionLogic.FindPermissionByPrivilegeMasterValue(
                    //    new SystemPermissionByPrivilegeMasterValueInput
                    //    {
                    //        PrivilegeAccess = EnumPrivilegeAccess.ģ��Ȩ��,
                    //        PrivilegeMasterValue = roleId,
                    //        PrivilegeMaster = EnumPrivilegeMaster.��ɫ
                    //    });

                    //if (menuPermissions.Any())
                    //{
                    //    operateStatus.Code = ResultCode.Error;
                    //    operateStatus.Message = ResourceSystem.����ģ��Ȩ��;
                    //    return operateStatus;
                    //}

                    ////�ж��Ƿ��������Ȩ��
                    //var workflowPermissions = await _permissionLogic.FindPermissionByPrivilegeMasterValue(
                    //    new SystemPermissionByPrivilegeMasterValueInput
                    //    {
                    //        PrivilegeAccess = EnumPrivilegeAccess.����Ȩ��,
                    //        PrivilegeMasterValue = roleId,
                    //        PrivilegeMaster = EnumPrivilegeMaster.��ɫ
                    //    });

                    //if (workflowPermissions.Any())
                    //{
                    //    operateStatus.Code = ResultCode.Error;
                    //    operateStatus.Message = ResourceSystem.��������Ȩ��;
                    //    return operateStatus;
                    //}
                }
                var trans = fixture.Db.BeginTransaction();
                try
                {
                    foreach (var id in input.Id.Split(','))
                    {
                        var roleId = Guid.Parse(id);
                        await fixture.Db.SystemPermission.DeleteAsync(d => d.PrivilegeMasterValue == roleId, trans);
                        await fixture.Db.SystemPermissionUser.DeleteAsync(d => d.PrivilegeMasterValue == roleId, trans);
                        await fixture.Db.SystemRole.DeleteAsync(d => d.RoleId == roleId, trans);
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
        /// ����
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus> IsFreeze(IdInput input)
        {
            var data = await FindAsync(f => f.RoleId == input.Id);
            data.IsFreeze = !data.IsFreeze;
            return await UpdateAsync(data);
        }

        #endregion
    }
}