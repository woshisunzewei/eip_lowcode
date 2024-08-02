/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2023/7/1 16:35:32
* 文件名: SystemDatasourceLogic
* 描述: 数据源管理业务逻辑接口实现
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.System.Models.Dtos.Datasource;

namespace EIP.System.Logic.Impl
{
    /// <summary>
    /// 数据源管理业务逻辑接口实现
    /// </summary>
    public class SystemDatasourceLogic : DapperAsyncLogic<SystemDataSource>, ISystemDatasourceLogic
    {
        #region 构造函数

        private readonly ISystemDatasourceRepository _systemDatasourceRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="systemDatasourceRepository"></param>
        public SystemDatasourceLogic(ISystemDatasourceRepository systemDatasourceRepository)
        {
            _systemDatasourceRepository = systemDatasourceRepository;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<PagedResults<SystemDatasourceFindOutput>>> Find(SystemDatasourceFindInput input)
        {
            return OperateStatus<PagedResults<SystemDatasourceFindOutput>>.Success(await _systemDatasourceRepository.Find(input));
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<SystemDataSource>> FindById(IdInput input)
        {
            return OperateStatus<SystemDataSource>.Success(await FindAsync(f => f.DataSourceId == input.Id));
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity">信息</param>
        /// <returns></returns>
        public async Task<OperateStatus> Save(SystemDataSource entity)
        {
            var edit = await FindAsync(f => f.DataSourceId == entity.DataSourceId);
            var currentUser = EipHttpContext.CurrentUser();
            if (edit != null)
            {
                entity.Id = edit.Id;
                entity.UpdateTime = DateTime.Now;
                entity.UpdateUserId = currentUser.UserId;
                entity.UpdateUserName = currentUser.Name;
                return await UpdateAsync(entity);
            }
            entity.DataSourceId = CombUtil.NewComb();
            entity.CreateTime = DateTime.Now;
            entity.CreateUserId = currentUser.UserId;
            entity.CreateUserName = currentUser.Name;

            entity.UpdateTime = DateTime.Now;
            entity.UpdateUserId = currentUser.UserId;
            entity.UpdateUserName = currentUser.Name;
            return await InsertAsync(entity);
        }

        /// <summary>
        /// 根据Id删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> Delete(IdInput<string> input)
        {
            OperateStatus operateStatus = new OperateStatus();
            foreach (var item in input.Id.Split(','))
            {
                //判断是否具有占用
                using (var fix = new SqlDatabaseFixture())
                {
                    var check = await fix.Db.SystemAgile.SetSelect(s => new { s.Name, s.ConfigType, s.ConfigId }).FindAllAsync(f => f.PublicJson.Contains(item));
                    if (check.Any())
                    {
                        var list = check.Where(w => w.ConfigType == EnumAgileConfigType.列表配置.ToShort()).Select(s => s.Name).GroupBy(g => g).Select(s => s.Key).ExpandAndToString();
                        var edit = check.Where(w => w.ConfigType == EnumAgileConfigType.表单配置.ToShort()).Select(s => s.Name).GroupBy(g => g).Select(s => s.Key).ExpandAndToString();
                        operateStatus.Msg = $"数据源被占用：" + (list.IsNotNullOrEmpty() ? $"列表【{list}】" : "") + (edit.IsNotNullOrEmpty() ? $"编辑【{edit}】" : "");
                        return operateStatus;
                    }
                }
                operateStatus = await DeleteAsync(f => f.DataSourceId == Guid.Parse(item));
            }
            return operateStatus;
        }

        #endregion
    }
}