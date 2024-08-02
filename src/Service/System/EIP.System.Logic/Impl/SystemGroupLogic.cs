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
using EIP.System.Models.Dtos.Group;
using EIP.System.Models.Dtos.Role;

namespace EIP.System.Logic.Impl
{
    /// <summary>
    /// 分组查询逻辑
    /// </summary>
    public class SystemGroupLogic : DapperAsyncLogic<SystemGroup>, ISystemGroupLogic
    {
        #region 构造函数

        private readonly ISystemGroupRepository _groupRepository;
        private readonly ISystemPermissionUserLogic _permissionUserLogic;
        private readonly ISystemPermissionLogic _permissionLogic;

        /// <summary>
        /// 组
        /// </summary>
        /// <param name="permissionUserLogic"></param>
        /// <param name="permissionLogic"></param>
        /// <param name="groupRepository"></param>
        public SystemGroupLogic(
            ISystemPermissionUserLogic permissionUserLogic,
            ISystemPermissionLogic permissionLogic,
            ISystemGroupRepository groupRepository)
        {
            _permissionUserLogic = permissionUserLogic;
            _permissionLogic = permissionLogic;
            _groupRepository = groupRepository;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 根据组织机构获取组信息
        /// </summary>
        /// <param name="input">组织机构</param>
        /// <returns></returns>
        public async Task<OperateStatus<PagedResults<SystemGroupFindOutput>>> Find(SystemGroupFindInput input)
        {
            if (input.DataSql.IsNullOrEmpty()) return OperateStatus<PagedResults<SystemGroupFindOutput>>.Success(new PagedResults<SystemGroupFindOutput>());
            return OperateStatus<PagedResults<SystemGroupFindOutput>>.Success(await _groupRepository.FindGroupByOrganizationId(input));
        }

        /// <summary>
        /// 保存组信息
        /// </summary>
        /// <param name="group">岗位信息</param>
        /// <param name="belongTo">归属</param>
        /// <returns></returns>
        public async Task<OperateStatus> Save(SystemGroup group,
            EnumGroupBelongTo belongTo)
        {
            group.BelongTo = (byte)belongTo;
            var groupById = await FindAsync(f => f.GroupId == group.GroupId);
            var currentUser = EipHttpContext.CurrentUser();
            if(groupById == null)
            {
                group.GroupId = CombUtil.NewComb();
                group.CreateTime = DateTime.Now;
                group.CreateUserId = currentUser.UserId;
                group.CreateUserName = currentUser.Name;
                group.UpdateTime = DateTime.Now;
                group.UpdateUserId = currentUser.UserId;
                group.UpdateUserName = currentUser.Name;
                return await InsertAsync(group);
            }

            group.Id = groupById.Id;

            group.CreateTime = groupById.CreateTime;
            group.CreateUserId = groupById.CreateUserId;
            group.CreateUserName = groupById.CreateUserName;

            group.UpdateTime = DateTime.Now;
            group.UpdateUserId = currentUser.UserId;
            group.UpdateUserName = currentUser.Name;
            return await UpdateAsync(group);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<SystemGroup>> FindById(IdInput input)
        {
            return OperateStatus<SystemGroup>.Success(await FindAsync(f => f.GroupId == input.Id));
        }

        /// <summary>
        /// 删除组信息
        /// </summary>
        /// <param name="input">组Id</param>
        /// <returns></returns>
        public async Task<OperateStatus> Delete(IdInput<string> input)
        {
            var operateStatus = new OperateStatus();
            using (var fixture = new SqlDatabaseFixture())
            {
                var trans = fixture.Db.BeginTransaction();
                try
                {
                    foreach (var id in input.Id.Split(','))
                    {
                        var groupId = Guid.Parse(id);
                        await fixture.Db.SystemPermission.DeleteAsync(d => d.PrivilegeMasterValue == groupId, trans);
                        await fixture.Db.SystemPermissionUser.DeleteAsync(d => d.PrivilegeMasterValue == groupId, trans);
                        await fixture.Db.SystemGroup.DeleteAsync(d => d.GroupId == groupId, trans);
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
        /// 复制
        /// </summary>
        /// <param name="input">复制信息</param>
        /// <returns></returns>
        public async Task<OperateStatus> Copy(SystemCopyInput input)
        {
            var operateStatus = new OperateStatus();
            try
            {
                //获取信息
                var role = await FindAsync(f => f.GroupId == input.Id);
                role.GroupId = CombUtil.NewComb();
                role.Name = input.Name;

                //获取拥有的权限及人员
                var allUser = (await _permissionUserLogic.FindPermissionUsersByPrivilegeMasterAdnPrivilegeMasterValue(EnumPrivilegeMaster.组,
                    input.Id)).Data.ToList();
                var allPer = (await _permissionLogic.FindSystemPermissionsByPrivilegeMasterValueAndValue(EnumPrivilegeMaster.组, input.Id)).Data.ToList();
                foreach (var user in allUser)
                {
                    user.PrivilegeMasterValue = role.GroupId;
                }
                foreach (var per in allPer)
                {
                    per.PrivilegeMasterValue = role.GroupId;
                }
                using (var fixture = new SqlDatabaseFixture())
                {
                    var trans = fixture.Db.BeginTransaction();
                    try
                    {
                        await fixture.Db.SystemPermissionUser.BulkInsertAsync(allUser, trans);
                        await fixture.Db.SystemPermission.BulkInsertAsync(allPer, trans);
                        await fixture.Db.SystemGroup.InsertAsync(role, trans);
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
        /// 冻结
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus> IsFreeze(IdInput input)
        {
            var data = await FindAsync(f => f.GroupId == input.Id);
            data.IsFreeze = !data.IsFreeze;
            return await UpdateAsync(data);
        }

        /// <summary>
        /// 获取所有未冻结角色
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemGroup>>> All()
        {
            return OperateStatus<IEnumerable<SystemGroup>>.Success(await FindAllAsync(f => f.IsFreeze == false));
        }

        #endregion
    }
}