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
using EIP.System.Models.Dtos.Data;

namespace EIP.System.Logic.Impl
{
    /// <summary>
    /// 数据权限规则业务逻辑
    /// </summary>
    public class SystemDataLogic : DapperAsyncLogic<SystemData>, ISystemDataLogic
    {
        #region 构造函数

        private readonly ISystemDataRepository _dataRepository;
        /// <summary>
        /// 数据权限业务逻辑
        /// </summary>
        /// <param name="dataRepository"></param>
        public SystemDataLogic(ISystemDataRepository dataRepository)
        {
            _dataRepository = dataRepository;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 根据模块Id获取数据权限规则
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<PagedResults<SystemDataFindOutput>>> Find(SystemDataFindInput input)
        {
            return OperateStatus<PagedResults<SystemDataFindOutput>>.Success(await _dataRepository.Find(input));
        }

        /// <summary>
        /// 保存数据权限规则
        /// </summary>
        /// <param name="input">数据权限规则</param>
        /// <returns></returns>
        public async Task<OperateStatus> Save(SystemData input)
        {
            OperateStatus operateStatus;
            var data = await FindAsync(f => f.DataId == input.DataId);
            var currentUser = EipHttpContext.CurrentUser();
            if (data == null)
            {
                input.DataId = Guid.NewGuid();
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
                input.Id = data.Id;
                input.CreateTime = data.CreateTime;
                input.CreateUserId = data.CreateUserId;
                input.CreateUserName = data.CreateUserName;

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
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<SystemData>> FindById(IdInput input)
        {
            return OperateStatus<SystemData>.Success((await FindAsync(f => f.DataId == input.Id)));
        }

        /// <summary>
        /// 删除数据权限规则信息
        /// </summary>
        /// <param name="input">数据权限规则Id</param>
        /// <returns></returns>
        public async Task<OperateStatus> Delete(IdInput<string> input)
        {
            var operateStatus = new OperateStatus();
            using (var fixture = new SqlDatabaseFixture())
            {
                var trans = fixture.Db.BeginTransaction();
                try
                {
                    var ids = input.Id.Trim().Split(',').ToList();
                    IList<Guid> privilegeAccessValue = new List<Guid>();
                    foreach (var id in ids)
                    {
                        privilegeAccessValue.Add(Guid.Parse(id));
                    }
                    var privilegeAccess = EnumPrivilegeAccess.数据权限.ToShort();
                    await fixture.Db.SystemPermission.DeleteAsync(f => f.PrivilegeAccess == privilegeAccess && privilegeAccessValue.Contains(f.PrivilegeAccessValue), trans);
                    var dataId = privilegeAccessValue;
                    await fixture.Db.SystemData.DeleteAsync(f => dataId.Contains(f.DataId), trans);
                    operateStatus.Msg = Chs.Successful;
                    operateStatus.Code = ResultCode.Success;
                    trans.Commit();
                }
                catch (Exception ex)
                {
                    trans.Rollback();
                    operateStatus.Msg = ex.Message;
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
            var data = await FindAsync(f => f.DataId == input.Id);
            data.IsFreeze = !data.IsFreeze;
            return await UpdateAsync(data);
        }

        #endregion
    }
}