/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/5/29 15:57:29
* 文件名: DsDatasourceLogic
* 描述: 数据源配置表业务逻辑接口实现
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Base.Models.Entities.DataRoom;
using EIP.Base.Repository.Fixture;
using EIP.Common.Core.Context;
using EIP.Common.Logic;
using EIP.Common.Models;
using EIP.Common.Models.Dtos;
using EIP.Common.Models.Paging;
using EIP.DataRoom.Models.Dtos;
using EIP.DataRoom.Repository;
using EIP.System.Models.Dtos.DataBase;
using EIP.System.Repository;

namespace EIP.DataRoom.Logic.Impl
{
    /// <summary>
    /// 数据源配置表业务逻辑接口实现
    /// </summary>
    public class DsDatasourceLogic : DapperAsyncLogic<DsDatasource>, IDsDatasourceLogic
    {
        #region 构造函数

        private readonly IDsDatasourceRepository _dsDatasourceRepository;
        private readonly ISystemDataBaseRepository _dataBaseRepository;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dsDatasourceRepository"></param>
        public DsDatasourceLogic(IDsDatasourceRepository dsDatasourceRepository, ISystemDataBaseRepository dataBaseRepository)
        {
            _dsDatasourceRepository = dsDatasourceRepository;
            _dataBaseRepository = dataBaseRepository;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<PagedResults<DsDatasourceFindOutput>>> Find(DsDatasourceFindInput input)
        {
            return OperateStatus<PagedResults<DsDatasourceFindOutput>>.Success(await _dsDatasourceRepository.Find(input));
        }
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<List<DsDatasourceFindOutput>>> FindList(DataSourceListInput input)
        {
            List<DsDatasourceFindOutput> outputs = new List<DsDatasourceFindOutput>();
            var datas = await FindAllAsync();
            foreach (var item in datas)
            {
                DsDatasourceFindOutput output = new DsDatasourceFindOutput
                {
                    id = item.id,
                    driverClassName = item.driver_class_name,
                    host = item.host,
                    moduleCode = item.module_code,
                    password = item.password,
                    port = item.port,
                    remark = item.remark,
                    sourceName = item.source_name,
                    sourceType = item.source_type,
                    tableName = item.table_name,
                    username = item.username,
                };
                outputs.Add(output);
            }
            return OperateStatus<List<DsDatasourceFindOutput>>.Success(outputs);
        }
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<List<DataSourceGetTableListOutput>>> FindTableList(DataSourceGetTableListInput input)
        {
            List<DataSourceGetTableListOutput> outputs = new List<DataSourceGetTableListOutput>();
            var datas = await FindAsync(f => f.id == input.id);
            //获取对应的表结构
            var tables = await _dataBaseRepository.FindDataBaseTable(new SystemDataBaseInput
            {
                ConnectionType = datas.source_type.ToString(),
                ConnectionString = datas.url.ToString(),
            });
            foreach (var item in tables)
            {
                outputs.Add(new DataSourceGetTableListOutput
                {
                    name = item.Name,
                    status = 0
                });
            }

            var views = await _dataBaseRepository.FindDataBaseView(new SystemDataBaseInput
            {
                ConnectionType = datas.source_type.ToString(),
                ConnectionString = datas.url.ToString(),
            });
            foreach (var item in views)
            {
                outputs.Add(new DataSourceGetTableListOutput
                {
                    name = item.Name,
                    status = 0
                });
            }
            return OperateStatus<List<DataSourceGetTableListOutput>>.Success(outputs);
        }
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<List<DataSourceGetFieldListOutput>>> FindFieldList(DataSourceGetFieldListInput input)
        {
            List<DataSourceGetFieldListOutput> outputs = new List<DataSourceGetFieldListOutput>();
            var datas = await FindAsync(f => f.id == input.id);
            //获取对应的表结构
            var columns = await _dataBaseRepository.FindDataBaseColumns(new SystemDataBaseTableDto
            {
                ConnectionType = datas.source_type.ToString(),
                ConnectionString = datas.url.ToString(),
                Name = input.tableName
            });
            foreach (var item in columns)
            {
                outputs.Add(new DataSourceGetFieldListOutput
                {
                    columnComment = item.Description,
                    columnName = item.Name,
                    columnType = item.DataType,
                    fieldDesc = item.Description
                });
            }
            return OperateStatus<List<DataSourceGetFieldListOutput>>.Success(outputs);
        }
        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<DsDatasource>> FindById(IdInput<int> input)
        {
            return OperateStatus<DsDatasource>.Success(await FindAsync(f => f.id == input.Id));
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity">信息</param>
        /// <returns></returns>
        public async Task<OperateStatus> Save(DataSourceSaveInput entity)
        {
            var edit = await FindAsync(f => f.id == entity.id);
            var currentUser = EipHttpContext.CurrentUser();
            if (edit != null)
            {
                edit.driver_class_name = entity.driverClassName;
                edit.password = entity.password;
                edit.remark = entity.remark;
                edit.source_name = entity.sourceName;
                edit.source_type = entity.sourceType;
                edit.url = entity.url;
                edit.username = entity.username;
                edit.update_date = DateTime.Now;
                return await UpdateAsync(edit);
            }
            edit = new DsDatasource();
            edit.driver_class_name = entity.driverClassName;
            edit.password = entity.password;
            edit.remark = entity.remark;
            edit.source_name = entity.sourceName;
            edit.source_type = entity.sourceType;
            edit.url = entity.url;
            edit.username = entity.username;
            edit.create_date = DateTime.Now;
            edit.update_date = DateTime.Now;
            return await InsertAsync(edit);
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
                operateStatus = await DeleteAsync(f => f.id == Int32.Parse(item));
            }
            return operateStatus;
        }

        #endregion
    }
}