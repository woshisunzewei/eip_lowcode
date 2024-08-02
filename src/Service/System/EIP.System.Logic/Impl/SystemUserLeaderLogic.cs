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
using EIP.System.Models.Dtos.UserLeader;

namespace EIP.System.Logic.Impl
{
    /// <summary>
    /// �û�ҵ���߼�ʵ��
    /// </summary>
    public class SystemUserLeaderLogic : DapperAsyncLogic<SystemUserLeader>, ISystemUserLeaderLogic
    {
        /// <summary>
        /// ����Id��ȡ�û��쵼
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemUserLeader>>> GetUserLeader(IdInput input)
        {
            return OperateStatus<IEnumerable<SystemUserLeader>>.Success(await FindAllAsync(f => f.UserId == input.Id));
        }

        /// <summary>
        /// ��ѯ�¼���Ա
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
        /// �����û��쵼��Ϣ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> Save(SystemUserLeaderSaveInput input)
        {
            //��ɾ�������쵼����
            OperateStatus operateStatus = new OperateStatus();
            try
            {
                await DeleteAsync(d => d.UserId == input.UserId);
                //����
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
        /// �����û��¼���Ϣ
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="userSubordinate"></param>
        /// <returns></returns>
        public async Task<OperateStatus> SaveSubordinate(SystemUserLeaderSaveInput input)
        {
            //��ɾ�������쵼����
            OperateStatus operateStatus = new OperateStatus();
            try
            {
                await DeleteAsync(d => d.LeaderUserId == input.UserId);
                //����
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