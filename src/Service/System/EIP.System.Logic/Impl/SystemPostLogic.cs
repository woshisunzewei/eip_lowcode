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
using EIP.System.Models.Dtos.Post;
using EIP.System.Models.Dtos.Role;

namespace EIP.System.Logic.Impl
{
    /// <summary>
    /// 岗位
    /// </summary>
    public class SystemPostLogic : DapperAsyncLogic<SystemPost>, ISystemPostLogic
    {
        #region 构造函数

        private readonly ISystemPermissionUserLogic _permissionUserLogic;
        private readonly ISystemPermissionLogic _permissionLogic;
        private readonly ISystemPostRepository _postRepository;

        /// <summary>
        /// 岗位
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

        #region 方法

        /// <summary>
        /// 根据组织机构获取岗位信息
        /// </summary>
        /// <param name="input">组织机构Id</param>
        /// <returns></returns>
        public async Task<OperateStatus<PagedResults<SystemPostFindOutput>>> Find(SystemPostFindInput input)
        {
            if (input.DataSql.IsNullOrEmpty()) return OperateStatus<PagedResults<SystemPostFindOutput>>.Success(new PagedResults<SystemPostFindOutput>());
            return OperateStatus<PagedResults<SystemPostFindOutput>>.Success(await _postRepository.FindPostByOrganizationId(input));
        }
        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="role">岗位信息</param>
        /// <returns></returns>
        public async Task<OperateStatus<SystemPost>> FindById(IdInput input)
        {
            return OperateStatus<SystemPost>.Success(await FindAsync(f => f.PostId == input.Id));
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
                var role = await FindAsync(f => f.PostId == input.Id);
                role.PostId = CombUtil.NewComb();
                role.Name = input.Name;

                //获取拥有的权限及人员
                var allUser = (await _permissionUserLogic.FindPermissionUsersByPrivilegeMasterAdnPrivilegeMasterValue(EnumPrivilegeMaster.岗位,
                    input.Id)).Data.ToList();
                var allPer = (await _permissionLogic.FindSystemPermissionsByPrivilegeMasterValueAndValue(EnumPrivilegeMaster.岗位, input.Id)).Data.ToList();
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
        /// 保存岗位信息
        /// </summary>
        /// <param name="input">岗位信息</param>
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
        /// 删除岗位信息
        /// </summary>
        /// <param name="input">岗位信息Id</param>
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
        /// 冻结
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus> IsFreeze(IdInput input)
        {
            var data = await FindAsync(f => f.PostId == input.Id);
            data.IsFreeze = !data.IsFreeze;
            return await UpdateAsync(data);
        }

        /// <summary>
        /// 获取所有未冻结岗位
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemPost>>> All()
        {
            return OperateStatus<IEnumerable<SystemPost>>.Success(await FindAllAsync(f => f.IsFreeze == false));
        }

        #endregion
    }
}