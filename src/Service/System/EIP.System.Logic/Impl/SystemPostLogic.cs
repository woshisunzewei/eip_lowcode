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
using EIP.System.Models.Dtos.Post;
using EIP.System.Models.Dtos.Role;

namespace EIP.System.Logic.Impl
{
    /// <summary>
    /// ��λ
    /// </summary>
    public class SystemPostLogic : DapperAsyncLogic<SystemPost>, ISystemPostLogic
    {
        #region ���캯��

        private readonly ISystemPermissionUserLogic _permissionUserLogic;
        private readonly ISystemPermissionLogic _permissionLogic;
        private readonly ISystemPostRepository _postRepository;

        /// <summary>
        /// ��λ
        /// </summary>
        /// <param name="permissionUserLogic"></param>
        /// <param name="permissionLogic"></param>
        /// <param name="postRepository"></param>
        public SystemPostLogic(
            ISystemPermissionUserLogic permissionUserLogic,
            ISystemPermissionLogic permissionLogic,
            ISystemPostRepository postRepository)
        {
            _permissionUserLogic = permissionUserLogic;
            _permissionLogic = permissionLogic;
            _postRepository = postRepository;
        }

        #endregion

        #region ����

        /// <summary>
        /// ������֯������ȡ��λ��Ϣ
        /// </summary>
        /// <param name="input">��֯����Id</param>
        /// <returns></returns>
        public async Task<OperateStatus<PagedResults<SystemPostFindOutput>>> Find(SystemPostFindInput input)
        {
            if (input.DataSql.IsNullOrEmpty()) return OperateStatus<PagedResults<SystemPostFindOutput>>.Success(new PagedResults<SystemPostFindOutput>());
            return OperateStatus<PagedResults<SystemPostFindOutput>>.Success(await _postRepository.FindPostByOrganizationId(input));
        }
        /// <summary>
        /// ����Id��ȡ
        /// </summary>
        /// <param name="role">��λ��Ϣ</param>
        /// <returns></returns>
        public async Task<OperateStatus<SystemPost>> FindById(IdInput input)
        {
            return OperateStatus<SystemPost>.Success(await FindAsync(f => f.PostId == input.Id));
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
                var role = await FindAsync(f => f.PostId == input.Id);
                role.PostId = CombUtil.NewComb();
                role.Name = input.Name;

                //��ȡӵ�е�Ȩ�޼���Ա
                var allUser = (await _permissionUserLogic.FindPermissionUsersByPrivilegeMasterAdnPrivilegeMasterValue(EnumPrivilegeMaster.��λ,
                    input.Id)).Data.ToList();
                var allPer = (await _permissionLogic.FindSystemPermissionsByPrivilegeMasterValueAndValue(EnumPrivilegeMaster.��λ, input.Id)).Data.ToList();
                foreach (var user in allUser)
                {
                    user.PrivilegeMasterValue = role.PostId;
                }
                foreach (var per in allPer)
                {
                    per.PrivilegeMasterValue = role.PostId;
                }

                using (var fixture = new SqlDatabaseFixture())
                {
                    var trans = fixture.Db.BeginTransaction();
                    try
                    {
                        await fixture.Db.SystemPermissionUser.BulkInsertAsync(allUser, trans);
                        await fixture.Db.SystemPermission.BulkInsertAsync(allPer, trans);
                        await fixture.Db.SystemPost.InsertAsync(role, trans);
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
        /// �����λ��Ϣ
        /// </summary>
        /// <param name="input">��λ��Ϣ</param>
        /// <returns></returns>
        public async Task<OperateStatus> Save(SystemPost input)
        {
            OperateStatus operateStatus;
            var post = await FindAsync(f => f.PostId == input.PostId);
            var currentUser = EipHttpContext.CurrentUser();
            if (post == null)
            {
                input.CreateTime = DateTime.Now;
                input.CreateUserId = currentUser.UserId;
                input.CreateUserName = currentUser.Name;
                input.UpdateTime = DateTime.Now;
                input.UpdateUserId = currentUser.UserId;
                input.UpdateUserName = currentUser.Name;
                input.PostId = Guid.NewGuid();
                operateStatus = await InsertAsync(input);
            }
            else
            {
                input.Id = post.Id;
                input.CreateTime = post.CreateTime;
                input.CreateUserId = post.CreateUserId;
                input.CreateUserName = post.CreateUserName;

                input.UpdateTime = DateTime.Now;
                input.UpdateUserId = currentUser.UserId;
                input.UpdateUserName = currentUser.Name;
                operateStatus = await UpdateAsync(input);
            }
            return operateStatus;
        }

        /// <summary>
        /// ɾ����λ��Ϣ
        /// </summary>
        /// <param name="input">��λ��ϢId</param>
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
                        var postId = Guid.Parse(id);
                        await fixture.Db.SystemPermission.DeleteAsync(d => d.PrivilegeMasterValue == postId, trans);
                        await fixture.Db.SystemPermissionUser.DeleteAsync(d => d.PrivilegeMasterValue == postId, trans);
                        await fixture.Db.SystemPost.DeleteAsync(d => d.PostId == postId, trans);
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
            var data = await FindAsync(f => f.PostId == input.Id);
            data.IsFreeze = !data.IsFreeze;
            return await UpdateAsync(data);
        }

        /// <summary>
        /// ��ȡ����δ�����λ
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemPost>>> All()
        {
            return OperateStatus<IEnumerable<SystemPost>>.Success(await FindAllAsync(f => f.IsFreeze == false));
        }

        #endregion
    }
}