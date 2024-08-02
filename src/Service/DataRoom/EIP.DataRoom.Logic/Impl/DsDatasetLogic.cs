/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/5/29 15:57:28
* 文件名: DsDatasetLogic
* 描述: 数据集表业务逻辑接口实现
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using Dapper;
using Dm;
using EIP.Base.Models.Entities.DataRoom;
using EIP.Base.Repository.Fixture;
using EIP.Common.Core.Context;
using EIP.Common.Core.Resource;
using EIP.Common.Logic;
using EIP.Common.Models;
using EIP.Common.Models.Dtos;
using EIP.Common.Models.Paging;
using EIP.Common.Models.Resx;
using EIP.DataRoom.Models.Dtos;
using EIP.DataRoom.Repository;
using EIP.System.Models.Dtos.DataBase;
using EIP.System.Repository;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;
using Npgsql;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Dynamic;
using System.Text;

namespace EIP.DataRoom.Logic.Impl
{
    /// <summary>
    /// 数据集表业务逻辑接口实现
    /// </summary>
    public class DsDatasetLogic : DapperAsyncLogic<DsDataset>, IDsDatasetLogic
    {
        #region 构造函数
        private readonly IDsDatasetRepository _dsDatasetRepository;
        private readonly IDsDatasourceLogic _dsDatasourceLogic;
        private readonly ISystemDataBaseRepository _dataBaseRepository;
        private readonly IDsDatasetLabelLogic _dsDatasetLabelLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dsDatasetRepository"></param>
        public DsDatasetLogic(IDsDatasetLabelLogic dsDatasetLabelLogic, IDsDatasetRepository dsDatasetRepository, IDsDatasourceLogic dsDatasourceLogic, ISystemDataBaseRepository systemDataBaseRepository)
        {
            _dsDatasetRepository = dsDatasetRepository;
            _dsDatasourceLogic = dsDatasourceLogic;
            _dataBaseRepository = systemDataBaseRepository;
            _dsDatasetLabelLogic = dsDatasetLabelLogic;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<PagedResults<DsDatasetFindOutput>>> Find(DsDatasetFindInput input)
        {
            return OperateStatus<PagedResults<DsDatasetFindOutput>>.Success(await _dsDatasetRepository.Find(input));
        }
        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<DsDataSetTestOutput>> Test(DsDataSetTestInput input)
        {
            OperateStatus<DsDataSetTestOutput> operateStatus = new OperateStatus<DsDataSetTestOutput>();
            DsDataSetTestOutput dsDataSetTestOutput = new DsDataSetTestOutput();
            var datasource = await _dsDatasourceLogic.FindAsync(f => f.id == input.dataSourceId);
            if (input.dataSetType == "custom")
            {

            }
            else
            {
                var script = JsonConvert.DeserializeObject<scriptOutput>(input.script);
                var dataOutput = await FindBusinessData(datasource.source_type, datasource.url, script, input);
                dsDataSetTestOutput.data = new
                {
                    current = input.Current,
                    list = dataOutput.Data.data,
                    size = input.Size,
                    totalCount = dataOutput.Data.count
                };
                var columns = await _dataBaseRepository.FindDataBaseColumns(new SystemDataBaseTableDto
                {
                    ConnectionType = datasource.source_type.ToString(),
                    ConnectionString = datasource.url.ToString(),
                    Name = script.tableName
                });
                foreach (var item in columns)
                {
                    dsDataSetTestOutput.structure.Add(new DsDataSetTestStructureOutput
                    {
                        fieldName = item.Name,
                        fieldType = item.DataType,
                        fieldDesc = item.Description
                    });
                }
                dsDataSetTestOutput.tableNameList.Add(script.tableName);
            }
            operateStatus.Data = dsDataSetTestOutput;
            return OperateStatus<DsDataSetTestOutput>.Success(dsDataSetTestOutput);
        }

        /// <summary>
        /// 查询业务数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<dynamic>> FindBusinessData(string ConnectionType, string ConnectionString, scriptOutput scriptOutput, QueryParam input)
        {
            //获取模板数据
            var operateStatus = new OperateStatus<dynamic>();
            try
            {
                var masks = new List<SystemDataBaseFindPagingBusinessDataTableColunmsInput>();
                var format = new List<SystemDataBaseFindPagingBusinessDataTableColunmsInput>();
                List<dynamic> data = new List<dynamic>();
                dynamic v = new ExpandoObject();
                SqlDatabaseFixtureInput sqlDatabaseFixtureInput = new SqlDatabaseFixtureInput();
                sqlDatabaseFixtureInput.ConnectionType = ConnectionType;
                sqlDatabaseFixtureInput.ConnectionString = ConnectionString;
                using (var fix = new SqlDatabaseFixture(sqlDatabaseFixtureInput))
                {
                    StringBuilder orderBy = new StringBuilder();
                    orderBy.Append(scriptOutput.fieldInfo.FirstOrDefault() + " desc");

                    var currentPage = input.Current; //当前页号
                    var pageSize = input.Size; //每页记录数
                    var lower = ((currentPage - 1) * pageSize) + 1; //记录起点
                    var upper = currentPage * pageSize; //记录终点

                    var querySql = $"select *,@rowNumber, @recordCount from {scriptOutput.tableName} @where   ";
                    var querySqlQueryMultiple =
                                       $@"select * from ({querySql}) seq where seq.row_num between {lower} and {upper}";

                    var orderString = $" {orderBy.ToString().TrimEnd(',')}";
                    var recordCount = querySqlQueryMultiple.Split("@recordCount");
                    var selectSql = recordCount[0].Trim().TrimEnd(',');
                    querySqlQueryMultiple = selectSql + " " + recordCount[1];
                    querySqlQueryMultiple = querySqlQueryMultiple.Replace("@rowNumber", " row_number() over (order by @orderBy) as row_num ")
                        .Replace("@orderBy", orderString)
                        .Replace("@where", " WHERE 1=1 ");

                    var querySqlRecordCount = querySql.Split("@recordCount");
                    var recordCountSql = querySql.Replace(querySqlRecordCount[0], ";select count(*) ").Replace("@recordCount", "  ").Replace("@where", " WHERE 1=1 ");
                    //总数量
                    querySqlQueryMultiple += recordCountSql;
                    var queryMulti = fix.Db.Connection.QueryMultiple(querySqlQueryMultiple);
                    data = queryMulti.Read<dynamic>().ToList();
                    v.count = queryMulti.Read<long>().Sum();
                }

                v.data = data;
                return OperateStatus<dynamic>.Success(v);
            }
            catch (Exception ex)
            {
                operateStatus.Msg = ex.Message;
            }
            return operateStatus;
        }
        /// <summary>
        /// 数据库链接
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        private IDbConnection GetConnectoin(string connectionString, string connectionType)
        {
            DbConnection connection;
            switch (connectionType)
            {
                case ResourceDataBaseType.Mysql:
                    connection = new MySqlConnection(connectionString);
                    break;
                case ResourceDataBaseType.Postgresql:
                    connection = new NpgsqlConnection(connectionString);
                    break;
                case ResourceDataBaseType.Dm:
                    connection = new DmConnection(connectionString);
                    break;
                default:
                    connection = new SqlConnection(connectionString);
                    break;
            }
            if (connection.State != ConnectionState.Open)
                connection.Open();
            return connection;
        }
        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<DsDatasetFindByIdOutput>> FindById(IdInput<int> input)
        {
            var data = await FindAsync(f => f.id == input.Id);
            var config = JsonConvert.DeserializeObject<DsDatasetFindByIdFieldListOutput>(data.config);
            OperateStatus<DsDatasetFindByIdOutput> operateStatus = new OperateStatus<DsDatasetFindByIdOutput>();
            DsDatasetFindByIdOutput dsDatasetFindByIdOutput = new DsDatasetFindByIdOutput()
            {
                id = data.id.ToString(),
                code = data.code,
                datasetType = data.dataset_type,
                name = data.name,
                remark = data.remark,
                sourceId = data.source_id,
                typeId = data.type_id,
                fields = config.fieldList
            };
            operateStatus.Data = dsDatasetFindByIdOutput;
            operateStatus.Code = ResultCode.Success;
            operateStatus.Msg = Chs.Successful;
            return operateStatus;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity">信息</param>
        /// <returns></returns>
        public async Task<OperateStatus> Save(DsDatasetSaveInput entity)
        {
            var edit = await FindAsync(f => f.id == entity.id); var currentUser = EipHttpContext.CurrentUser();

            OperateStatus operateStatus = new OperateStatus();

            if (edit != null)
            {
                edit.cache = entity.cache;
                edit.config = JsonConvert.SerializeObject(entity.config);
                edit.dataset_type = entity.datasetType;
                edit.editable = entity.editable;
                edit.module_code = entity.moduleCode;
                edit.name = entity.name;
                edit.remark = entity.remark;
                edit.source_id = entity.sourceId;
                edit.type_id = entity.typeId;
                edit.update_date = DateTime.Now;
                operateStatus = await UpdateAsync(edit);
            }
            else
            {
                edit = new DsDataset();
                edit.cache = entity.cache;
                edit.config = JsonConvert.SerializeObject(entity.config);
                edit.dataset_type = entity.datasetType;
                edit.editable = entity.editable;
                edit.module_code = entity.moduleCode;
                edit.name = entity.name;
                edit.remark = entity.remark;
                edit.source_id = entity.sourceId;
                edit.type_id = entity.typeId;
                edit.create_date = DateTime.Now;
                edit.update_date = DateTime.Now;
                operateStatus = await InsertAsync(edit);
            }
            if (operateStatus.Code == ResultCode.Success)
            {
                //刪除
                await _dsDatasetLabelLogic.DeleteAsync(f => f.dataset_id == edit.id);
                //新增
                foreach (var item in entity.labelIds)
                {
                    await _dsDatasetLabelLogic.InsertAsync(new DsDatasetLabel
                    {
                        dataset_id = edit.id,
                        label_id = item
                    });
                }
            }
            return operateStatus;
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