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
using EIP.System.Models.Dtos.UserLeader;

namespace EIP.System.Logic.Impl
{
    /// <summary>
    /// 用户业务逻辑实现
    /// </summary>
    public class SystemUserLeaderLogic : DapperAsyncLogic<SystemUserLeader>, ISystemUserLeaderLogic
    {
        /// <summary>
        /// 根据Id获取用户领导
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemUserLeader>>> GetUserLeader(IdInput input)
        {
            return OperateStatus<IEnumerable<SystemUserLeader>>.Success(await FindAllAsync(f => f.UserId == input.Id));
        }

        /// <summary>
        /// 查询下级人员
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<IList<SystemUserLeadersOutput>>> FindSubordinate(IdInput input)
        {
            using (var fixture = new SqlDatabaseFixture())
            {
                var data = (await fixture.Db.SystemUserFindSubordinateOutput.FindAllAsync<SystemUserLeadersOutput>(f => f.LeaderUserId == input.Id, o => o.Outputs)).ToList();
                if (data.Any())
                {
                    var subordinate = data.First().Outputs;
                    if (subordinate.Any())
                    {
                        var allOrgs = (await fixture.Db.SystemOrganization.FindAllAsync()).ToList();
                        foreach (var user in subordinate)
                        {
                            var organization = allOrgs.FirstOrDefault(w => w.OrganizationId == user.OrganizationId);
                            if (organization != null && !organization.ParentIds.IsNullOrEmpty())
                            {
                                user.OrganizationNames = organization.ParentIdsName;
                            }
                        }
                    }
                    return OperateStatus<IList<SystemUserLeadersOutput>>.Success(subordinate);
                }
                return OperateStatus<IList<SystemUserLeadersOutput>>.Success(new List<SystemUserLeadersOutput>());
            }
        }

        /// <summary>
        /// 保存用户领导信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> Save(SystemUserLeaderSaveInput input)
        {
            //先删除所有领导数据
            OperateStatus operateStatus = new OperateStatus();
            try
            {
                await DeleteAsync(d => d.UserId == input.UserId);
                //新增
                foreach (var item in input.U)
                {
                    operateStatus = await InsertAsync(new SystemUserLeader { UserId = input.UserId, LeaderUserId = item });
                }
                operateStatus.Msg = Chs.Successful;
                operateStatus.Code = ResultCode.Success;
            }
            catch (Exception ex)
            {
                operateStatus.Msg = ex.Message;
            }
            return operateStatus;
        }

        /// <summary>
        /// 保存用户下级信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userSubordinate"></param>
        /// <returns></returns>
        public async Task<OperateStatus> SaveSubordinate(SystemUserLeaderSaveInput input)
        {
            //先删除所有领导数据
            OperateStatus operateStatus = new OperateStatus();
            try
            {
                await DeleteAsync(d => d.LeaderUserId == input.UserId);
                //新增
                foreach (var item in input.U)
                {
                    operateStatus = await InsertAsync(new SystemUserLeader { UserId = item, LeaderUserId = input.UserId });
                }
                operateStatus.Msg = Chs.Successful;
                operateStatus.Code = ResultCode.Success;
            }
            catch (Exception ex)
            {
                operateStatus.Msg = ex.Message;
            }
            return operateStatus;
        }
    }
}