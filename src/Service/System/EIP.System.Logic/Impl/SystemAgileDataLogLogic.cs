/**************************************************************
* Copyright (C) 2022 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2018/11/9 9:21:04
* 文件名: 
* 描述: 
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Common.Core.Extension;
using MySqlConnection = MySqlConnector.MySqlConnection;

namespace EIP.System.Logic.Impl
{
    /// <summary>
    /// 敏捷开发业务逻辑接口实现
    /// </summary>
    public class SystemAgileDataLogLogic : DapperAsyncLogic<SystemAgileDataLog>, ISystemAgileDataLogLogic
    {
        #region 构造函数
        /// <summary>
        /// 需要建立值和键两个字段
        /// </summary>
        private readonly string[] _relationField = { "radio", "checkbox", "organization", "dictionary", "user", "select", "district", "map", "cascader" };
        /// <summary>
        /// 关联文本
        /// </summary>
        private readonly string relationTxt = "_Txt";
        private readonly ISystemAgileLogic _agileConfigLogic;
        private readonly ISystemAgileAutomationLogic _systemAgileAutomationLogic;
        /// <summary>
        /// 
        /// </summary>
        public SystemAgileDataLogLogic(
            ISystemAgileLogic agileConfigLogic, ISystemAgileAutomationLogic systemAgileAutomationLogic)
        {
            _agileConfigLogic = agileConfigLogic;
            _systemAgileAutomationLogic = systemAgileAutomationLogic;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task<OperateStatus> Save(SystemAgileDataLog input)
        {
            OperateStatus operateStatus = new OperateStatus();
            var contents = new List<SystemAgileDataLogContent>();
            if (input.Type == EnumAgileDataLogType.新增.ToShort())
            {
                operateStatus = await InsertAsync(input);
            }
            else if (input.Type == EnumAgileDataLogType.编辑.ToShort())
            {
                SystemDataBaseInput systemDataBaseInput = new SystemDataBaseInput();
                SystemAgile agile = new SystemAgile();
                using (var fix = new SqlDatabaseFixture())
                {
                    agile = await RedisHelper.CacheShellAsync($"ISystemAgileLogic_Cache:{input.ConfigId}", DateTimeUtil.TotalSeconds(20), async () =>
                    {
                        return await fix.Db.SystemAgile.SetSelect(s => new { s.DataBaseId, s.ColumnJson, s.DataFrom, s.DataFromName }).FindAsync(f => f.ConfigId == input.ConfigId);
                    });

                    var dataBase = await RedisHelper.CacheShellAsync($"ISystemDataBaseLogic_Cache:{agile.DataBaseId}", DateTimeUtil.TotalSeconds(20), async () =>
                    {
                        return await fix.Db.SystemDataBase.SetSelect(s => new { s.ConnectionString, s.ConnectionType }).FindAsync(f => f.DataBaseId == agile.DataBaseId);
                    });

                    systemDataBaseInput = new SystemDataBaseInput
                    {
                        ConnectionType = dataBase.ConnectionType,
                        ConnectionString = ConfigurationUtil.GetSection(dataBase.ConnectionString)
                    };
                }
                using (IDbConnection connection = GetConnectoin(systemDataBaseInput.ConnectionString, systemDataBaseInput.ConnectionType))
                {
                    var before = JsonConvert.DeserializeObject<List<SystemAgileDataLogValue>>(input.Before);
                    List<string> sqls = new List<string>();
                    foreach (var item in before)
                    {
                        sqls.Add($"SELECT * FROM {item.TableName} WHERE RelationId='{input.RelationId}'");
                    }
                    var queryData = connection.QueryMultiple(sqls.ExpandAndToString(";"));
                    List<SystemAgileDataLogValue> systemAgileDataLogValues = new List<SystemAgileDataLogValue>();
                    foreach (var item in before)
                    {
                        var data = queryData.Read<object>().ToList();
                        systemAgileDataLogValues.Add(new SystemAgileDataLogValue
                        {
                            TableName = item.TableName,
                            Description = item.Description,
                            Data = data
                        });
                    }
                    input.After = JsonConvert.SerializeObject(systemAgileDataLogValues);
                    //主表
                    var columns = agile.ColumnJson.JsonStringToList<SystemAgileFindFormColumnsJsonOutput>();

                    var masterBefore = before.FirstOrDefault();
                    var masterAfter = JsonConvert.DeserializeObject<List<SystemAgileDataLogValue>>(input.After).FirstOrDefault();
                    foreach (var item in columns.Where(w => w.Type != "batch"))
                    {
                        dynamic beforeValue;
                        dynamic afterValue;
                        if (_relationField.Contains(item.Type.ToLower()))
                        {
                            beforeValue = masterBefore.Data[0][item.Model + relationTxt].Value;
                            afterValue = masterAfter.Data[0][item.Model + relationTxt].Value;
                        }
                        else
                        {
                            try
                            {
                                beforeValue = masterBefore.Data[0][item.Model].Value;
                                afterValue = masterAfter.Data[0][item.Model].Value;
                            }
                            catch (Exception ex)
                            {
                                throw;
                            }
                        }
                        afterValue = afterValue != null ? afterValue.ToString() : "";
                        beforeValue = beforeValue != null ? beforeValue.ToString() : "";
                        if (beforeValue != afterValue)
                        {
                            contents.Add(new SystemAgileDataLogContent
                            {
                                After = afterValue.ToString(),
                                Before = beforeValue.ToString(),
                                Filed = item.Model,
                                Name = item.Label,
                                Hidden = item.Options.Hidden
                            });
                        }
                    }
                    input.Content = JsonConvert.SerializeObject(contents);
                }
                if (contents.Any())
                {
                    operateStatus = await InsertAsync(input);
                }
            }
            else if (input.Type == EnumAgileDataLogType.删除.ToShort())
            {

            }
            if (operateStatus.Code == ResultCode.Success)
            {
                operateStatus.Msg = "保存成功";
                await _systemAgileAutomationLogic.AautomationTable(input, contents);
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
        /// 查询
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<OperateStatus<List<SystemAgileDataLogFindByRelationOutput>>> FindByRelationId(SystemAgileDataLogFindByRelationInput paging)
        {
            using (var fix = new SqlDatabaseFixture())
            {
                var datas = (await fix.Db.SystemAgileDataLog.SetSelect(s => new { s.Type, s.Content, s.CreateTime, s.CreateUserName, s.CreateUserId }).FindAllAsync(f => f.RelationId == paging.RelationId)).OrderByDescending(o => o.CreateTime);
                List<SystemAgileDataLogFindByRelationOutput> outputs = new List<SystemAgileDataLogFindByRelationOutput>();
                if (datas.Any())
                {
                    //查找用户头像
                    var userId = datas.Select(s => s.CreateUserId).Distinct().ToList();
                    var user = await fix.Db.SystemUserInfo.SetSelect(s => new { s.HeadImage, s.UserId }).FindAllAsync(f => userId.Contains(f.UserId));
                    foreach (var item in datas)
                    {
                        SystemAgileDataLogFindByRelationOutput systemAgileDataLogFindByRelationOutput = item.MapTo<SystemAgileDataLog, SystemAgileDataLogFindByRelationOutput>();
                        var header = user.FirstOrDefault(f => f.UserId == item.CreateUserId);
                        if (header != null)
                        {
                            systemAgileDataLogFindByRelationOutput.CreateUserHeadImage = header.HeadImage;
                        }
                    }
                }
                return OperateStatus<List<SystemAgileDataLogFindByRelationOutput>>.Success(outputs);
            }
        }
        #endregion
    }
}