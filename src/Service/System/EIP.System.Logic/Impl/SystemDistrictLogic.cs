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
using EIP.System.Models.Dtos.District;

namespace EIP.System.Logic.Impl
{
    /// <summary>
    /// 行政区划
    /// </summary>
    public class SystemDistrictLogic : DapperAsyncLogic<SystemDistrict>, ISystemDistrictLogic
    {
        #region 方法

        /// <summary>
        /// 根据父级查询所有子集树形结构
        /// </summary>
        /// <param name="input">父级Id</param>
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
        /// 根据父级查询所有子集
        /// </summary>
        /// <param name="input">父级Id</param>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemDistrict>>> FindByParentId(IdInput<string> input)
        {
            return OperateStatus<IEnumerable<SystemDistrict>>.Success(await FindAllAsync(f => f.ParentId == input.Id));
        }

        /// <summary>
        /// 根据县Id获取省市县Id
        /// </summary>
        /// <param name="input">县Id</param>
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
        /// 检测代码是否已经具有重复项
        /// </summary>
        /// <param name="input">需要验证的参数</param>
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
        /// 保存省市县信息
        /// </summary>
        /// <param name="systemDistrict">省市县信息</param>
        /// <returns></returns>
        public async Task<OperateStatus> Save(SystemDistrict systemDistrict)
        {
            //判断是否具有省市区县信息
            var district = await FindAsync(f => f.DistrictId == systemDistrict.DistrictId);
            //有则更新,无则添加
            var operateStatus = district != null ? await UpdateAsync(systemDistrict) : await InsertAsync(systemDistrict);
            return operateStatus;
        }

        /// <summary>
        /// 删除省市县及下级数据
        /// </summary>
        /// <param name="input">父级id</param>
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
        /// 删除主键集合
        /// </summary>
        public IList<string> DeletIds = new List<string>();

        /// <summary>
        /// 获取删除主键信息
        /// </summary>
        /// <param name="input"></param>
        private async Task GetDeleteGuid(IdInput<string> input)
        {
            //获取下级
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