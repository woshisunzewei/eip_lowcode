/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/6/1 16:54:21
* 文件名: BigScreenPageLogic
* 描述: 页面基本信息表业务逻辑接口实现
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using Dapper;
using EIP.Base.Models.Entities.DataRoom;
using EIP.Base.Repository.Fixture;
using EIP.Big.Models.Dtos.ScreenPage;
using EIP.Common.Core.Context;
using EIP.Common.Core.Resource;
using EIP.Common.Logic;
using EIP.Common.Models;
using EIP.Common.Models.Dtos;
using EIP.Common.Models.Paging;
using EIP.DataRoom.Models.Dtos;
using EIP.DataRoom.Repository;
using EIP.System.Models.Dtos.DataBase;
using EIP.System.Repository;
using Newtonsoft.Json;
using System.Dynamic;

namespace EIP.DataRoom.Logic.Impl
{
    /// <summary>
    /// 页面基本信息表业务逻辑接口实现
    /// </summary>
    public class BigScreenPageLogic : DapperAsyncLogic<BigScreenPage>, IBigScreenPageLogic
    {
        #region 构造函数
        private readonly IDsDatasourceLogic _dsDatasourceLogic;
        private readonly IDsDatasetLogic _dsDatasetLogic;
        private readonly IBigScreenPageRepository _bigScreenPageRepository;
        private readonly ISystemDataBaseRepository _dataBaseRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bigScreenPageRepository"></param>
        public BigScreenPageLogic(IBigScreenPageRepository bigScreenPageRepository,
            IDsDatasourceLogic dsDatasourceLogic, IDsDatasetLogic dsDatasetLogic, ISystemDataBaseRepository dataBaseRepository)
        {
            _dsDatasetLogic = dsDatasetLogic;
            _dsDatasourceLogic = dsDatasourceLogic;
            _bigScreenPageRepository = bigScreenPageRepository;
            _dataBaseRepository = dataBaseRepository;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<PagedResults<BigScreenPageFindOutput>>> Find(BigScreenPageFindInput input)
        {
            return OperateStatus<PagedResults<BigScreenPageFindOutput>>.Success(await _bigScreenPageRepository.Find(input));
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity">信息</param>
        /// <returns></returns>
        public async Task<OperateStatus> Save(BigScreenPageSaveInput entity)
        {
            var edit = await FindAsync(f => f.id == entity.id);
            var currentUser = EipHttpContext.CurrentUser();
            OperateStatus<string> operate = new OperateStatus<string>();
            OperateStatus operateStatus = new OperateStatus();
            if (edit != null)
            {
                edit.code = entity.code;
                edit.config = JsonConvert.SerializeObject(entity);
                edit.app_code = entity.appCode;
                edit.name = entity.name;
                edit.remark = entity.remark;
                edit.type = entity.type;
                edit.update_date = DateTime.Now;
                edit.parent_code = entity.parentCode;
                edit.cover_picture = entity.coverPicture;
                edit.order_num = entity.orderNum;
                operateStatus = await UpdateAsync(edit);
            }
            else
            {
                edit = new BigScreenPage();
                edit.code = entity.type + "_" + Guid.NewGuid();
                edit.config = JsonConvert.SerializeObject(entity);
                edit.app_code = entity.appCode;
                edit.name = entity.name;
                edit.remark = entity.remark;
                edit.type = entity.type;
                edit.order_num = entity.orderNum;
                edit.update_date = DateTime.Now;
                edit.create_date = DateTime.Now;
                operateStatus = await InsertAsync(edit);
            }
            if (operateStatus.Code != ResultCode.Success)
            {
                operate.Msg = operateStatus.Msg;
                return operate;
            }
            operate.Data = edit.code;
            operate.Code = ResultCode.Success;
            operate.Msg = Chs.Successful;
            return operate;
        }
        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<BigScreenByCodeOutput>> FindByCode(IdInput<string> input)
        {
            var data = await FindAsync(f => f.code == input.Id);
            BigScreenPageSaveInput dashboard = JsonConvert.DeserializeObject<BigScreenPageSaveInput>(data.config);
            BigScreenByCodeOutput dashboardPermissionOutput = new BigScreenByCodeOutput
            {
                id = data.id,
                name = data.name,
                appCode = data.app_code,
                code = data.code,
                coverPicture = data.cover_picture,
                orderNum = data.order_num,
                remark = data.remark,
                parentCode = data.parent_code,
                type = data.type,
                pageConfig = dashboard.pageConfig,
                chartList = dashboard.chartList,
            };
            OperateStatus<BigScreenByCodeOutput> operateStatus = new OperateStatus<BigScreenByCodeOutput>();
            operateStatus.Data = dashboardPermissionOutput;
            operateStatus.Code = ResultCode.Success;
            operateStatus.Msg = Chs.Successful;
            return operateStatus;
        }
        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<DashboardChartOutput>> Chart(DashboardChartInput input)
        {
            var page = await FindAsync(f => f.code == input.pageCode);
            var chartOption = input.chart.dataSource;
            var dataSetId = Convert.ToInt32(chartOption.businessKey);
            var dataSet = await _dsDatasetLogic.FindAsync(f => f.id == dataSetId);
            var dataSource = await _dsDatasourceLogic.FindAsync(f => f.id == dataSet.source_id);
            var dataSourceConfig = JsonConvert.DeserializeObject<DsDatasetFindByIdFieldListOutput>(dataSet.config);
            var businessData = FindBusinessData(dataSource.source_type, dataSource.url, dataSourceConfig.tableName);
            DashboardChartOutput dashboardChartOutput = new DashboardChartOutput();
            dashboardChartOutput.data = businessData;

            var columns = await _dataBaseRepository.FindDataBaseColumns(new SystemDataBaseTableDto
            {
                ConnectionType = dataSource.source_type.ToString(),
                ConnectionString = dataSource.url.ToString(),
                Name = dataSourceConfig.tableName
            });
            dynamic column = new ExpandoObject();
            foreach (var item in columns)
            {
                ((IDictionary<string, object>)column)[item.Name] = new
                {
                    aggregate = "",
                    alias = item.Name,
                    remark = item.Description,
                    originalColumn = item.Name,
                    tableName = dataSourceConfig.tableName,
                    type = item.DataType
                };
            }
            dashboardChartOutput.columnData = column;
            return OperateStatus<DashboardChartOutput>.Success(dashboardChartOutput);
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<DashboardChartOutput>> List(DashboardChartInput input)
        {
            var page = await FindAsync(f => f.code == input.pageCode);
            var config = JsonConvert.DeserializeObject<DashboardDesignSaveInput>(page.config);
            List<DashboardChartOptionInput> option = new List<DashboardChartOptionInput>();
            foreach (var item in config.chartList)
            {
                var jsonString = JsonConvert.SerializeObject(item);
                option.Add(JsonConvert.DeserializeObject<DashboardChartOptionInput>(jsonString));
            }
            var chartOption = option.FirstOrDefault(f => f.code == input.chartCode).dataSource;
            var dataSetId = Convert.ToInt32(chartOption.businessKey);
            var dataSet = await _dsDatasetLogic.FindAsync(f => f.id == dataSetId);
            var dataSource = await _dsDatasourceLogic.FindAsync(f => f.id == dataSet.source_id);
            var dataSourceConfig = JsonConvert.DeserializeObject<DsDatasetFindByIdFieldListOutput>(dataSet.config);
            var businessData = FindBusinessData(dataSource.source_type, dataSource.url, dataSourceConfig.tableName);
            DashboardChartOutput dashboardChartOutput = new DashboardChartOutput();
            dashboardChartOutput.data = businessData;
            return OperateStatus<DashboardChartOutput>.Success(dashboardChartOutput);
        }
        /// <summary>
        /// 查询业务数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public dynamic FindBusinessData(string connectionType, string connectionString, string table)
        {
            //获取模板数据
            try
            {
                var masks = new List<SystemDataBaseFindPagingBusinessDataTableColunmsInput>();
                var format = new List<SystemDataBaseFindPagingBusinessDataTableColunmsInput>();
                List<dynamic> data = new List<dynamic>();
                dynamic v = new ExpandoObject();
                SqlDatabaseFixtureInput sqlDatabaseFixtureInput = new SqlDatabaseFixtureInput();
                sqlDatabaseFixtureInput.ConnectionType = connectionType;
                sqlDatabaseFixtureInput.ConnectionString = connectionString;
                using (var fix = new SqlDatabaseFixture(sqlDatabaseFixtureInput))
                {
                    var querySql = $"select * from {table} ";
                    data = (fix.Db.Connection.Query(querySql)).ToList();
                }
                return data;
            }
            catch (Exception ex)
            {

            }
            return null;
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
                operateStatus = await DeleteAsync(f => f.code == item);
            }
            return operateStatus;
        }

        #endregion
    }
}