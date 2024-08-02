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
using EIP.System.Models.Dtos.Role;

namespace EIP.System.Logic.Impl
{
    /// <summary>
    /// 角色业务逻辑
    /// </summary>
    public class SystemRoleLogic : DapperAsyncLogic<SystemRole>, ISystemRoleLogic
    {
        #region 构造函数

        private readonly ISystemPermissionUserLogic _permissionUserLogic;
        private readonly ISystemPermissionLogic _permissionLogic;
        private readonly ISystemRoleRepository _roleRepository;

        /// <summary>
        /// 角色业务逻辑
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

        #region 方法

        /// <summary>
        /// 根据组织机构Id查询角色信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<PagedResults<SystemRolesFindOutput>>> Find(SystemRolesFindInput input)
        {
            if (input.DataSql.IsNullOrEmpty()) return OperateStatus<PagedResults<SystemRolesFindOutput>>.Success(new PagedResults<SystemRolesFindOutput>());
            return OperateStatus<PagedResults<SystemRolesFindOutput>>.Success(await _roleRepository.FindPagingRole(input));
        }

        /// <summary>
        /// 获取所有未冻结角色
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemRoleSelectOutput>>> Select()
        {
            return OperateStatus<IEnumerable<SystemRoleSelectOutput>>.Success((await FindAllAsync(f => f.IsFreeze == false)).MapToList<SystemRole,SystemRoleSelectOutput>());
        }

        /// <summary>
        /// 获取所有未冻结角色
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemRole>>> All()
        {
            return OperateStatus<IEnumerable<SystemRole>>.Success(await FindAllAsync(f => f.IsFreeze == false));
        }

        /// <summary>
        /// 保存岗位信息
        /// </summary>
        /// <param name="input">岗位信息</param>
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
        /// 根据Id获取
        /// </summary>
        /// <param name="role">岗位信息</param>
        /// <returns></returns>
        public async Task<OperateStatus<SystemRole>> FindById(IdInput input)
        {
            return OperateStatus<SystemRole>.Success(await FindAsync(f => f.RoleId == input.Id));
        }
        /// <summary>
        /// 角色复制
        /// </summary>
        /// <param name="input">角色Id</param>
        /// <returns></returns>
        public async Task<OperateStatus> Copy(SystemCopyInput input)
        {
            var operateStatus = new OperateStatus();
            try
            {
                //获取角色信息
                var role = await FindAsync(f => f.RoleId == input.Id);
                role.RoleId = CombUtil.NewComb();
                role.Name = input.Name;

                //获取该角色拥有的权限及人员
                var allUser = (await _permissionUserLogic.FindPermissionUsersByPrivilegeMasterAdnPrivilegeMasterValue(EnumPrivilegeMaster.角色,
                    input.Id)).Data.ToList();
                var allPer = (await _permissionLogic.FindSystemPermissionsByPrivilegeMasterValueAndValue(EnumPrivilegeMaster.角色, input.Id)).Data.ToList();
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
        /// 删除角色信息
        /// </summary>
        /// <param name="input">角色Id</param>
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
                        operateStatus.Msg = ResourceSystem.角色不存在;
                        return operateStatus;
                    }
                    //判断是否具有人员
                    //var permissionUsers = await _permissionUserLogic.FindPermissionUsersByPrivilegeMasterAdnPrivilegeMasterValue(EnumPrivilegeMaster.角色, roleId);
                    //if (permissionUsers.Any())
                    //{
                    //    operateStatus.Code = ResultCode.Error;
                    //    operateStatus.Message = ResourceSystem.具有人员;
                    //    return operateStatus;
                    //}

                    ////判断是否具有按钮权限
                    //var functionPermissions = await _permissionLogic.FindPermissionByPrivilegeMasterValue(
                    //    new SystemPermissionByPrivilegeMasterValueInput
                    //    {
                    //        PrivilegeAccess = EnumPrivilegeAccess.模块按钮,
                    //        PrivilegeMasterValue = roleId,
                    //        PrivilegeMaster = EnumPrivilegeMaster.角色
                    //    });
                    //if (functionPermissions.Any())
                    //{
                    //    operateStatus.Code = ResultCode.Error;
                    //    operateStatus.Message = ResourceSystem.具有功能项权限;
                    //    return operateStatus;
                    //}

                    ////判断是否具有模块权限
                    //var menuPermissions = await _permissionLogic.FindPermissionByPrivilegeMasterValue(
                    //    new SystemPermissionByPrivilegeMasterValueInput
                    //    {
                    //        PrivilegeAccess = EnumPrivilegeAccess.模块权限,
                    //        PrivilegeMasterValue = roleId,
                    //        PrivilegeMaster = EnumPrivilegeMaster.角色
                    //    });

                    //if (menuPermissions.Any())
                    //{
                    //    operateStatus.Code = ResultCode.Error;
                    //    operateStatus.Message = ResourceSystem.具有模块权限;
                    //    return operateStatus;
                    //}

                    ////判断是否具有流程权限
                    //var workflowPermissions = await _permissionLogic.FindPermissionByPrivilegeMasterValue(
                    //    new SystemPermissionByPrivilegeMasterValueInput
                    //    {
                    //        PrivilegeAccess = EnumPrivilegeAccess.流程权限,
                    //        PrivilegeMasterValue = roleId,
                    //        PrivilegeMaster = EnumPrivilegeMaster.角色
                    //    });

                    //if (workflowPermissions.Any())
                    //{
                    //    operateStatus.Code = ResultCode.Error;
                    //    operateStatus.Message = ResourceSystem.具有流程权限;
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
        /// 冻结
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