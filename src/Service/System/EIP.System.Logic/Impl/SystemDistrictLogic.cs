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
using EIP.System.Models.Dtos.District;

namespace EIP.System.Logic.Impl
{
    /// <summary>
    /// ��������
    /// </summary>
    public class SystemDistrictLogic : DapperAsyncLogic<SystemDistrict>, ISystemDistrictLogic
    {
        #region ����

        /// <summary>
        /// ���ݸ�����ѯ�����Ӽ����νṹ
        /// </summary>
        /// <param name="input">����Id</param>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<TreeEntity>>> FindSync(IdInput<string> input)
        {
            var children = (await FindAllAsync(f => f.ParentId == input.Id));
            var lists = new List<TreeEntity>();
            foreach (var list in children.ToList().OrderBy(o => o.OrderNo))
            {
                lists.Add(new TreeEntity
                {
                    Key = list.DistrictId,
                    Title = list.Name,
                    IsLeaf = list.LevelType == 5,
                    LevelType=list.LevelType
                });
            }
            return OperateStatus<IEnumerable<TreeEntity>>.Success(lists);
        }

        /// <summary>
        /// ���ݸ�����ѯ�����Ӽ�
        /// </summary>
        /// <param name="input">����Id</param>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemDistrict>>> FindByParentId(IdInput<string> input)
        {
            return OperateStatus<IEnumerable<SystemDistrict>>.Success(await FindAllAsync(f => f.ParentId == input.Id));
        }

        /// <summary>
        /// ������Id��ȡʡ����Id
        /// </summary>
        /// <param name="input">��Id</param>
        /// <returns></returns>
        public async Task<OperateStatus<SystemDistrictFindDistrictByCountIdOutout>> FindByCountId(IdInput<string> input)
        {
            using (var fixture = new SqlDatabaseFixture())
            {
                var data = await fixture.Db.SystemDistrictFindDistrict.FindAsync<SystemDistrictFindDistrictByCountIdProvinceOutout>(f => f.DistrictId == input.Id, o => o.District);
                data.ProvinceId = data.District.ParentId;
                return OperateStatus<SystemDistrictFindDistrictByCountIdOutout>.Success(data);
            }
        }

        /// <summary>
        /// �������Ƿ��Ѿ������ظ���
        /// </summary>
        /// <param name="input">��Ҫ��֤�Ĳ���</param>
        /// <returns></returns>
        public async Task<OperateStatus> Check(CheckSameValueInput input)
        {
            var operateStatus = new OperateStatus();

            bool result = !input.Id.IsEmptyGuid() ? (await FindAllAsync(f => f.DistrictId == input.Param && f.DistrictId != input.Id.ToString())).Any() :
                    (await FindAllAsync(f => f.DistrictId == input.Param)).Any();

            if (result)
            {
                operateStatus.Code = ResultCode.Error;
                operateStatus.Msg = string.Format(Chs.HaveCode, input.Param);
            }
            else
            {
                operateStatus.Code = ResultCode.Success;
                operateStatus.Msg = Chs.CheckSuccessful;
            }
            return operateStatus;
        }

        /// <summary>
        /// ����ʡ������Ϣ
        /// </summary>
        /// <param name="systemDistrict">ʡ������Ϣ</param>
        /// <returns></returns>
        public async Task<OperateStatus> Save(SystemDistrict systemDistrict)
        {
            //�ж��Ƿ����ʡ��������Ϣ
            var district = await FindAsync(f => f.DistrictId == systemDistrict.DistrictId);
            //�������,�������
            var operateStatus = district != null ? await UpdateAsync(systemDistrict) : await InsertAsync(systemDistrict);
            return operateStatus;
        }

        /// <summary>
        /// ɾ��ʡ���ؼ��¼�����
        /// </summary>
        /// <param name="input">����id</param>
        /// <returns></returns>
        public async Task<OperateStatus> Delete(IdInput<string> input)
        {
            var operateStatus = new OperateStatus();
            DeletIds.Add(input.Id);
            await GetDeleteGuid(input);
            foreach (var delete in DeletIds)
            {
                operateStatus = await DeleteAsync(d => d.DistrictId == delete);
            }
            return operateStatus;
        }

        /// <summary>
        /// ɾ����������
        /// </summary>
        public IList<string> DeletIds = new List<string>();

        /// <summary>
        /// ��ȡɾ��������Ϣ
        /// </summary>
        /// <param name="input"></param>
        private async Task GetDeleteGuid(IdInput<string> input)
        {
            //��ȡ�¼�
            var dictionary = (await FindByParentId(input)).Data.ToList();
            foreach (var dic in dictionary)
            {
                DeletIds.Add(dic.DistrictId);
                await GetDeleteGuid(input);
            }
        }
        #endregion
    }
}