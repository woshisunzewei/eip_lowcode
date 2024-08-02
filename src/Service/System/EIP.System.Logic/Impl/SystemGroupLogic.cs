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
using EIP.System.Models.Dtos.Group;
using EIP.System.Models.Dtos.Role;

namespace EIP.System.Logic.Impl
{
    /// <summary>
    /// �����ѯ�߼�
    /// </summary>
    public class SystemGroupLogic : DapperAsyncLogic<SystemGroup>, ISystemGroupLogic
    {
        #region ���캯��

        private readonly ISystemGroupRepository _groupRepository;
        private readonly ISystemPermissionUserLogic _permissionUserLogic;
        private readonly ISystemPermissionLogic _permissionLogic;

        /// <summary>
        /// ��
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

        #region ����

        /// <summary>
        /// ������֯������ȡ����Ϣ
        /// </summary>
        /// <param name="input">��֯����</param>
        /// <returns></returns>
        public async Task<OperateStatus<PagedResults<SystemGroupFindOutput>>> Find(SystemGroupFindInput input)
        {
            if (input.DataSql.IsNullOrEmpty()) return OperateStatus<PagedResults<SystemGroupFindOutput>>.Success(new PagedResults<SystemGroupFindOutput>());
            return OperateStatus<PagedResults<SystemGroupFindOutput>>.Success(await _groupRepository.FindGroupByOrganizationId(input));
        }

        /// <summary>
        /// ��������Ϣ
        /// </summary>
        /// <param name="group">��λ��Ϣ</param>
        /// <param name="belongTo">����</param>
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
        /// ɾ������Ϣ
        /// </summary>
        /// <param name="input">��Id</param>
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
        /// ����
        /// </summary>
        /// <param name="input">������Ϣ</param>
        /// <returns></returns>
        public async Task<OperateStatus> Copy(SystemCopyInput input)
        {
            var operateStatus = new OperateStatus();
            try
            {
                //��ȡ��Ϣ
                var role = await FindAsync(f => f.GroupId == input.Id);
                role.GroupId = CombUtil.NewComb();
                role.Name = input.Name;

                //��ȡӵ�е�Ȩ�޼���Ա
                var allUser = (await _permissionUserLogic.FindPermissionUsersByPrivilegeMasterAdnPrivilegeMasterValue(EnumPrivilegeMaster.��,
                    input.Id)).Data.ToList();
                var allPer = (await _permissionLogic.FindSystemPermissionsByPrivilegeMasterValueAndValue(EnumPrivilegeMaster.��, input.Id)).Data.ToList();
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
        /// ����
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus> IsFreeze(IdInput input)
        {
            var data = await FindAsync(f => f.GroupId == input.Id);
            data.IsFreeze = !data.IsFreeze;
            return await UpdateAsync(data);
        }

        /// <summary>
        /// ��ȡ����δ�����ɫ
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemGroup>>> All()
        {
            return OperateStatus<IEnumerable<SystemGroup>>.Success(await FindAllAsync(f => f.IsFreeze == false));
        }

        #endregion
    }
}