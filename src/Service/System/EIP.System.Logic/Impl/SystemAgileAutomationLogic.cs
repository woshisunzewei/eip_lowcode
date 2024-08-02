/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/2/3 22:14:39
* 文件名: SystemAgileAutomationLogic
* 描述: 自动化构建业务逻辑接口实现
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Common.Message.Sms.Dto;
using System.Threading;
using MySqlConnection = MySqlConnector.MySqlConnection;
namespace EIP.System.Logic.Impl
{
    /// <summary>
    /// 自动化构建业务逻辑接口实现
    /// </summary>
    public class SystemAgileAutomationLogic : DapperAsyncLogic<SystemAgileAutomation>, ISystemAgileAutomationLogic
    {
        private static AsyncLocal<ScriptEngine> scriptEngine = new AsyncLocal<ScriptEngine>();

        #region 构造函数
        private readonly ISystemMessageLogic _systemMessageLogic;
        private readonly ISystemAgileAutomationRepository _systemAgileAutomationRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="systemAgileAutomationRepository"></param>
        public SystemAgileAutomationLogic(ISystemAgileAutomationRepository systemAgileAutomationRepository
            , ISystemMessageLogic systemMessageLogic)
        {
            _systemAgileAutomationRepository = systemAgileAutomationRepository;
            _systemMessageLogic = systemMessageLogic;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<PagedResults<SystemAgileAutomationFindOutput>>> Find(SystemAgileAutomationFindInput input)
        {
            return OperateStatus<PagedResults<SystemAgileAutomationFindOutput>>.Success(await _systemAgileAutomationRepository.Find(input));
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<SystemAgileAutomation>> FindById(IdInput input)
        {
            return OperateStatus<SystemAgileAutomation>.Success(await FindAsync(f => f.ConfigId == input.Id));
        }

        /// <summary>
        /// 冻结
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus> IsFreeze(IdInput input)
        {
            var data = await FindAsync(f => f.ConfigId == input.Id);
            data.IsFreeze = !data.IsFreeze;
            return await UpdateAsync(data);
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="entity">信息</param>
        /// <returns></returns>
        public async Task<OperateStatus> Save(SystemAgileAutomation entity)
        {
            var edit = await FindAsync(f => f.ConfigId == entity.ConfigId);
            var currentUser = EipHttpContext.CurrentUser();
            if (edit != null)
            {
                edit.Name = entity.Name;
                edit.IsFreeze = entity.IsFreeze;
                edit.Remark = entity.Remark;
                edit.OrderNo = entity.OrderNo;
                edit.ConfigType = entity.ConfigType;
                edit.UpdateTime = DateTime.Now;
                edit.UpdateUserId = currentUser.UserId;
                edit.UpdateUserName = currentUser.Name;

                return await UpdateAsync(edit);
            }
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
                operateStatus = await DeleteAsync(f => f.ConfigId == Guid.Parse(item));
            }
            return operateStatus;
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> SaveJson(SystemAgileAutomation input)
        {
            return await UpdateAsync(u => u.ConfigId == input.ConfigId, new { SaveJson = input.SaveJson, UpdateTime = DateTime.Now });
        }

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> PublicJson(SystemAgileAutomation input)
        {
            return await UpdateAsync(u => u.ConfigId == input.ConfigId, new { TableId = input.TableId, TableTriggerType = input.TableTriggerType, SaveJson = input.SaveJson, PublicJson = input.PublicJson, UpdateTime = DateTime.Now });
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<List<SystemAgileAutomationFindEditOutput>>> FindTable()
        {
            using (var fix = new SqlDatabaseFixture())
            {
                var data = await fix.Db.SystemAgile.SetSelect(s => new { s.ConfigId, s.ColumnJson, s.Name }).FindAllAsync(f => f.ConfigType == 2 && f.IsDelete == false);
                return OperateStatus<List<SystemAgileAutomationFindEditOutput>>.Success(data.MapToList<SystemAgile, SystemAgileAutomationFindEditOutput>());
            }
        }
        #endregion

        #region 自动化解析
        /// <summary>
        /// 处理流程自动化
        /// </summary>
        /// <param name="input"></param>
        /// <param name="contents"></param>
        public async Task<OperateStatus> AautomationTable(SystemAgileDataLog input, List<SystemAgileDataLogContent> contents = null)
        {
            OperateStatus operateStatus = new OperateStatus();
            SystemAgile agile = new SystemAgile();
            SystemDataBaseInput systemDataBaseInput = new SystemDataBaseInput();
            var configType = EnumAgileAutomationType.工作表新增或变更.ToShort();
            var triggerTypeInsertOrUpdate = EnumAgileAutomationTableTriggerType.当新增或更新记录时.ToShort();
            var triggerTypeOther = input.Type == EnumAgileDataLogType.新增.ToShort() ?
                EnumAgileAutomationTableTriggerType.仅新增记录时.ToShort() :
                EnumAgileAutomationTableTriggerType.仅更新记录时.ToShort();
            List<SystemAgileAutomation> aautomation = new List<SystemAgileAutomation>();
            using (var fix = new SqlDatabaseFixture())
            {
                var triggerTypeInsert = EnumAgileAutomationTableTriggerType.仅新增记录时.ToShort();
                aautomation = (fix.Db.SystemAgileAutomation.SetSelect(s => new
                {
                    s.TableTriggerType,
                    s.PublicJson
                }).FindAll(f =>
                    f.TableId == input.ConfigId &&
                    f.ConfigType == configType &&
                    (f.TableTriggerType == triggerTypeInsertOrUpdate || f.TableTriggerType == triggerTypeOther))).ToList();

                agile = await GetSystemAgile(fix, input.ConfigId);
                systemDataBaseInput = await GetSystemDataBaseInput(fix, agile);
            }
            try
            {
                List<SystemAgileAutomationJsonDataAllOutput> dataAllOutputs = new List<SystemAgileAutomationJsonDataAllOutput>();
                using (IDbConnection connection = GetConnectoin(systemDataBaseInput.ConnectionString, systemDataBaseInput.ConnectionType))
                {
                    foreach (var item in aautomation)
                    {
                        string where = "";
                        //是否具有条件，若有条件则筛选数据条件
                        if (item.PublicJson.IsNotNullOrEmpty())
                        {
                            var publicJson = JsonConvert.DeserializeObject<SystemAgileAutomationJsonOutput>(item.PublicJson);
                            where = SearchFilterUtil.ConvertFilters(publicJson.Data.ConditionFilters);
                            if (publicJson.Data.TriggerColumns.Any())
                            {
                                where += " and ( ";
                                foreach (var triggerColumns in publicJson.Data.TriggerColumns)
                                {
                                    where += triggerColumns + " is not null or";
                                }
                                where = " and " + where.Substring(0, where.Length - 2) + ")";
                            }
                            string sql = $"SELECT * FROM {agile.DataFromName} WHERE RelationId='{input.RelationId}'" + where;
                            //查询数据
                            var queryData = connection.Query<dynamic>(sql);
                            //若有满足数据，执行下面流程
                            if (queryData.Any())
                            {
                                dataAllOutputs.Add(new SystemAgileAutomationJsonDataAllOutput
                                {
                                    Data = queryData,
                                    Id = publicJson.Id,
                                    Json = publicJson
                                });
                                await NextChild(new SystemAgileAautomationNextChildInput
                                {
                                    Child = publicJson,
                                    DataAllOutputs = dataAllOutputs,
                                    AgileDataLog = input
                                });
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                operateStatus.Msg = ex.Message;
            }

            return operateStatus;
        }

        /// <summary>
        /// 处理流程自动化
        /// </summary>
        /// <param name="input"></param>
        /// <param name="contents"></param>
        public async Task<OperateStatus> AautomationButton(SystemAgileAautomationButtonInput input)
        {
            OperateStatus operateStatus = new OperateStatus();
            SystemAgile agile = new SystemAgile();
            SystemDataBaseInput systemDataBaseInput = new SystemDataBaseInput();
            SystemAgileAutomation aautomation = new SystemAgileAutomation();
            using (var fix = new SqlDatabaseFixture())
            {
                aautomation = (fix.Db.SystemAgileAutomation.SetSelect(s => new { s.TableTriggerType, s.PublicJson }).Find(f => f.ConfigId == input.AutomationConfigId));
                agile = await GetSystemAgile(fix, input.AgileConfigId);
                systemDataBaseInput = await GetSystemDataBaseInput(fix, agile);
            }
            try
            {
                List<SystemAgileAutomationJsonDataAllOutput> dataAllOutputs = new List<SystemAgileAutomationJsonDataAllOutput>();
                using (IDbConnection connection = GetConnectoin(systemDataBaseInput.ConnectionString, systemDataBaseInput.ConnectionType))
                {
                    if (aautomation.PublicJson.IsNotNullOrEmpty())
                    {
                        var publicJson = JsonConvert.DeserializeObject<SystemAgileAutomationJsonOutput>(aautomation.PublicJson);
                        dataAllOutputs.Add(new SystemAgileAutomationJsonDataAllOutput
                        {
                            Data = null,
                            Id = publicJson.Id
                        });
                        //如果具有下級
                        await NextChild(new SystemAgileAautomationNextChildInput
                        {
                            Child = publicJson,
                            DataAllOutputs = dataAllOutputs,
                            Authorization = input.Authorization,
                        });

                        operateStatus.Code = ResultCode.Success;
                        operateStatus.Msg = Chs.Successful;
                    }
                }
            }
            catch (Exception ex)
            {
                operateStatus.Msg = ex.Message;
            }

            return operateStatus;
        }

        /// <summary>
        /// 获取SystemAgile
        /// </summary>
        /// <param name="fix"></param>
        /// <param name="agileConfigId"></param>
        /// <returns></returns>
        private async Task<SystemAgile> GetSystemAgile(SqlDatabaseFixture fix, Guid agileConfigId)
        {
            var agile = await RedisHelper.CacheShellAsync($"ISystemAgileLogic_Cache:{agileConfigId}", DateTimeUtil.TotalSeconds(20), async () =>
            {
                return await fix.Db.SystemAgile.SetSelect(s => new { s.DataBaseId, s.ColumnJson, s.DataFrom, s.DataFromName }).FindAsync(f => f.ConfigId == agileConfigId);
            });
            return agile;
        }

        /// <summary>
        /// 获取数据库连接
        /// </summary>
        /// <param name="fix"></param>
        /// <param name="agile"></param>
        /// <returns></returns>
        private async Task<SystemDataBaseInput> GetSystemDataBaseInput(SqlDatabaseFixture fix, SystemAgile agile)
        {
            var dataBase = await RedisHelper.CacheShellAsync($"ISystemDataBaseLogic_Cache:{agile.DataBaseId}", DateTimeUtil.TotalSeconds(20), async () =>
            {
                return await fix.Db.SystemDataBase.SetSelect(s => new { s.ConnectionString, s.ConnectionType }).FindAsync(f => f.DataBaseId == agile.DataBaseId);
            });
            var systemDataBaseInput = new SystemDataBaseInput
            {
                ConnectionType = dataBase.ConnectionType,
                ConnectionString = ConfigurationUtil.GetSection(dataBase.ConnectionString)
            };
            return systemDataBaseInput;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task NextChild(SystemAgileAautomationNextChildInput input)
        {
            var childNode = input.Child.ChildNode;
            if (childNode.Type.HasValue)
            {
                switch ((EnumAgileAutomationControlType)childNode.Type)
                {
                    case EnumAgileAutomationControlType.发送站内通知:
                        {
                            var content = GetNoticeContent(childNode.Data, input.DataAllOutputs);
                            var users = await GetNoticeUser(childNode.Data, input.AgileDataLog);
                            await SendNotice(users, EnumAgileAutomationControlType.发送站内通知, content);
                        }
                        break;
                    case EnumAgileAutomationControlType.发送短信:
                        {
                            var content = GetNoticeContent(childNode.Data, input.DataAllOutputs);
                            var users = await GetNoticeUser(childNode.Data, input.AgileDataLog);
                            await SendNotice(users, EnumAgileAutomationControlType.发送短信, content);
                        }
                        break;
                    case EnumAgileAutomationControlType.发送邮件:
                        {
                            var content = GetNoticeContent(childNode.Data, input.DataAllOutputs);
                            var users = await GetNoticeUser(childNode.Data, input.AgileDataLog);
                            await SendNotice(users, EnumAgileAutomationControlType.发送邮件, content);
                        }
                        break;
                    case EnumAgileAutomationControlType.发送服务号信息:
                        {
                            var content = GetNoticeContent(childNode.Data, input.DataAllOutputs);
                            var users = await GetNoticeUser(childNode.Data, input.AgileDataLog);
                            await SendNotice(users, EnumAgileAutomationControlType.发送服务号信息, content);
                        }
                        break;
                    case EnumAgileAutomationControlType.获取多条数据:
                        {
                            GetMultipleData(input);
                        }
                        break;
                    case EnumAgileAutomationControlType.获取单条数据:
                        {

                        }
                        break;
                    case EnumAgileAutomationControlType.新增数据:
                        {
                            await AddData(input);
                        }
                        break;
                    case EnumAgileAutomationControlType.更新数据:
                        {
                            var updateData = childNode.Data.UpdateData.Where(w => w.Value.IsNotNullOrEmpty()).ToList();
                            if (childNode.Data.UpdateObj.HasValue && updateData.Any())
                            {
                                await UpdateData(input);
                            }
                        }
                        break;
                    case EnumAgileAutomationControlType.条件分支:
                        {
                            if (scriptEngine.Value == null)
                                scriptEngine.Value = new ScriptEngine();
                            foreach (var conditionNodes in childNode.ConditionNodes)
                            {
                                var conditionJs = ConvertFilters(conditionNodes.Data.ConditionFilters, input.DataAllOutputs);
                                if (conditionJs != null)
                                {
                                    conditionJs = "if(" + conditionJs + "){1}else{0}";
                                }
                                var result = scriptEngine.Value.Evaluate(conditionJs);
                                var value = result.ToString();
                                if (value == "1")
                                {
                                    //执行下一个节点
                                    await NextChild(new SystemAgileAautomationNextChildInput
                                    {
                                        AgileDataLog = input.AgileDataLog,
                                        Authorization = input.Authorization,
                                        Child = conditionNodes,
                                        DataAllOutputs = input.DataAllOutputs,
                                    });
                                }
                            }
                        }
                        break;
                    case EnumAgileAutomationControlType.并行分支:
                        {

                        }
                        break;
                    case EnumAgileAutomationControlType.删除数据:
                        {
                            await DeleteData(input);
                        }
                        break;
                    case EnumAgileAutomationControlType.发送API请求:
                        {
                            Dictionary<string, string> headers = new Dictionary<string, string>();
                            headers.Add("Authorization", input.Authorization);
                            string data = null;
                            string url = childNode.Data.Url.ReplaceEditor();
                            switch (childNode.Data.Method.ToLower())
                            {
                                case "post":
                                    data = await RequestUtil.HttpPost(url, null, headers, childNode.Data.ContentType);
                                    break;
                                default:
                                    data = await RequestUtil.HttpGet(url, null, headers, childNode.Data.ContentType);
                                    break;
                            }
                            input.DataAllOutputs.Add(new SystemAgileAutomationJsonDataAllOutput
                            {
                                Data = data.JsonStringToObject<OperateStatus<dynamic>>().Data,
                                Id = childNode.Id
                            });
                        }
                        break;
                    case EnumAgileAutomationControlType.代码块:
                        {

                        }
                        break;
                    default:
                        break;
                }

                //执行下一个节点
                await NextChild(new SystemAgileAautomationNextChildInput
                {
                    AgileDataLog = input.AgileDataLog,
                    Authorization = input.Authorization,
                    Child = childNode,
                    DataAllOutputs = input.DataAllOutputs,
                });

            }
        }
        #region 条件判断

        /// <summary>
        /// 处理SQL查询条件，含子查询
        /// </summary>
        /// <param name="filter">查询对象</param>
        /// <returns></returns>
        public static string ConvertFilters(Filter filter, List<SystemAgileAutomationJsonDataAllOutput> dataAllOutputs)
        {
            bool isFirst = true;
            StringBuilder where = new StringBuilder("");
            //处理字段查询明细
            if (filter.rules != null && filter.rules.Count > 0)
            {
                foreach (var item in filter.rules)
                {
                    if (!string.IsNullOrEmpty(item.field) && !string.IsNullOrEmpty(item.op))
                    {
                        if (isFirst != true)
                        {
                            //非首个条件添加AND或者OR
                            where.AppendFormat(" {0} ", filter.groupOp == "AND" ? "&&" : "||");
                        }
                        where.Append(CheckCondition(item, dataAllOutputs));
                        isFirst = false;
                    }
                }
            }

            //处理嵌套查询
            if (filter.groups != null && filter.groups.Count > 0)
            {
                foreach (var item in filter.groups)
                {
                    string child = ConvertFilters(item, dataAllOutputs);
                    if (!string.IsNullOrEmpty(child))
                    {
                        if (isFirst != true)
                        {
                            //非首个条件添加AND或者OR
                            where.AppendFormat(" {0} ", filter.groupOp == "AND" ? "&&" : "||");
                        }
                        where.AppendFormat(child);
                        isFirst = false;
                    }
                }
            }
            //返回
            return where.Length > 0 ? $" ({where}) " : "";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rule"></param>
        /// <param name="dataAllOutputs"></param>
        /// <returns></returns>
        private static string CheckCondition(FilterRules rule, List<SystemAgileAutomationJsonDataAllOutput> dataAllOutputs)
        {
            string content = rule.field.ReplaceEditor();
            var h1Elements = rule.field.GetNodes();
            if (h1Elements != null)
            {
                foreach (var element in h1Elements)
                {
                    //解码
                    var columnSetting = HttpUtility.UrlDecode(element.Id);
                    if (columnSetting != null)
                    {
                        var columnData = JsonConvert.DeserializeObject<SystemAgileAutomationJsonDataContentSettingOutput>(columnSetting);
                        //得到对应节点值
                        var dataOutput = dataAllOutputs.FirstOrDefault(f => f.Id == columnData.Id);
                        if (dataOutput != null)
                        {
                            foreach (var itemOutput in dataOutput.Data)
                            {
                                dynamic info = new ExpandoObject();
                                var dic = (IDictionary<string, object>)info;
                                foreach (var dd in itemOutput)
                                {
                                    string dkey = dd.Key;
                                    object dvalue = dd.Value;
                                    if (dkey == columnData.Model)
                                    {
                                        //得到值
                                        content = content.Replace(element.OuterHtml, dvalue.ToString());
                                    }
                                }
                            }
                        }
                    }
                }
            }
            string phoneKey = "Ph376A@e^270Ks_~";
            if (string.IsNullOrEmpty(rule.op) || string.IsNullOrEmpty(rule.field))
            {
                return "";
            }
            rule.field = content;
            //Sql注入替换后数据
            string data = rule.data.FilterSql();
            StringBuilder searchCase = new StringBuilder();
            switch (rule.op)
            {
                case "eq": //等于
                    searchCase.Append(GetFilter(rule.field, " =='" + data + "'"));
                    break;
                case "eqphone": //等于
                    searchCase.Append(GetFilter(rule.field, " ='" + Encrypt(data, phoneKey) + "'"));
                    break;
                case "ne": //不等于
                    searchCase.Append(GetFilter(rule.field, " !='" + data + "'"));
                    break;
                case "bw": //以...开始
                    searchCase.Append(GetFilter(rule.field, " like '" + data + "%'"));
                    break;
                case "bn": //不以...开始
                    searchCase.Append(GetFilter(rule.field, " not like '" + data + "%'"));
                    break;
                case "ew": //结束于
                    searchCase.Append(GetFilter(rule.field, " like '%" + data + "'"));
                    break;
                case "en": //不结束于
                    searchCase.Append(GetFilter(rule.field, " not like '%" + data + "'"));
                    break;
                case "lt": //小于
                    searchCase.Append(GetFilter(rule.field, " <'" + data + "'"));
                    break;
                case "le": //小于等于
                    searchCase.Append(GetFilter(rule.field, " <='" + data + "'"));
                    break;
                case "gt": //大于
                    searchCase.Append(GetFilter(rule.field, " >'" + data + "'"));
                    break;
                case "ge": //大于等于
                    searchCase.Append(GetFilter(rule.field, " >='" + data + "'"));
                    break;
                case "in": //包括
                    searchCase.Append(GetFilter(rule.field, " in ('" + data + "')"));
                    break;
                case "ni": //不包含
                    searchCase.Append(GetFilter(rule.field, " not in ('" + data + "')"));
                    break;
                case "cn"://包含
                    searchCase.Append(GetFilter(rule.field, " like '%" + data + "%' "));
                    break;
                case "nc"://不包含
                    searchCase.Append(GetFilter(rule.field, " not like '%" + data + "%'"));
                    break;
                case "nu"://空值
                    searchCase.Append(GetFilter(rule.field, " =null"));
                    break;
                case "nn"://非空值
                    searchCase.Append(GetFilter(rule.field, " != null"));
                    break;
                case "dateeq"://时间等于
                    if (data.Length == 4)//年
                    {
                        searchCase.Append(GetFilter(rule.field, " between '" + data + "-01-01 00:00:00' AND '" + data + "-12-31 23:59:59'"));
                    }
                    else if (data.Length == 7) //年月
                    {
                        string endTime = Convert.ToDateTime(data).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd");
                        searchCase.Append(GetFilter(rule.field, " between '" + data + "-01 00:00:00' AND '" + endTime + " 23:59:59'"));
                    }
                    else if (data.Length == 10) //年月日
                    {
                        searchCase.Append(GetFilter(rule.field, " between '" + data + " 00:00:00' AND '" + data + " 23:59:59'"));
                    }
                    else if (data.Length == 13) //年月日时
                    {
                        searchCase.Append(GetFilter(rule.field, " between '" + data + " :00:00' AND '" + data + " :59:59'"));
                    }
                    else if (data.Length == 16) //年月日时分
                    {
                        searchCase.Append(GetFilter(rule.field, " between '" + data + " :00' AND '" + data + " :59'"));
                    }
                    else
                    {
                        searchCase.Append(GetFilter(rule.field, " ='" + data + "'"));
                    }
                    break;
                case "datelt"://小于
                    if (data.Length == 4)//年
                    {
                        searchCase.Append(GetFilter(rule.field, " <'" + data + "-01-01 00:00:00'"));
                    }
                    else if (data.Length == 7) //年月
                    {
                        searchCase.Append(GetFilter(rule.field, " <'" + data + "-01 00:00:00' '"));
                    }
                    else if (data.Length == 10) //年月日
                    {
                        searchCase.Append(GetFilter(rule.field, " <'" + data + " 00:00:00'"));
                    }
                    else if (data.Length == 13) //年月日时
                    {
                        searchCase.Append(GetFilter(rule.field, " <'" + data + " :00:00'"));
                    }
                    else if (data.Length == 16) //年月日时分
                    {
                        searchCase.Append(GetFilter(rule.field, " <'" + data + " :00'"));
                    }
                    else
                    {
                        searchCase.Append(GetFilter(rule.field, " <'" + data + "'"));
                    }
                    break;
                case "datele"://小于等于
                    if (data.Length == 4)//年
                    {
                        searchCase.Append(GetFilter(rule.field, " <='" + data + "-01-01 00:00:00'"));
                    }
                    else if (data.Length == 7) //年月
                    {
                        searchCase.Append(GetFilter(rule.field, " <='" + data + "-01 00:00:00' '"));
                    }
                    else if (data.Length == 10) //年月日
                    {
                        searchCase.Append(GetFilter(rule.field, " <='" + data + " 00:00:00'"));
                    }
                    else if (data.Length == 13) //年月日时
                    {
                        searchCase.Append(GetFilter(rule.field, " <='" + data + " :00:00'"));
                    }
                    else if (data.Length == 16) //年月日时分
                    {
                        searchCase.Append(GetFilter(rule.field, " <='" + data + " :00'"));
                    }
                    else
                    {
                        searchCase.Append(GetFilter(rule.field, " <='" + data + "'"));
                    }
                    break;
                case "dategt"://大于
                    if (data.Length == 4)//年
                    {
                        searchCase.Append(GetFilter(rule.field, " >'" + data + "-01-01 00:00:00'"));
                    }
                    else if (data.Length == 7) //年月
                    {
                        searchCase.Append(GetFilter(rule.field, " >'" + data + "-01 00:00:00' '"));
                    }
                    else if (data.Length == 10) //年月日
                    {
                        searchCase.Append(GetFilter(rule.field, " >'" + data + " 00:00:00'"));
                    }
                    else if (data.Length == 13) //年月日时
                    {
                        searchCase.Append(GetFilter(rule.field, " >'" + data + " :00:00'"));
                    }
                    else if (data.Length == 16) //年月日时分
                    {
                        searchCase.Append(GetFilter(rule.field, " >'" + data + " :00'"));
                    }
                    else
                    {
                        searchCase.Append(GetFilter(rule.field, " >'" + data + "'"));
                    }
                    break;
                case "datege"://大于等于
                    if (data.Length == 4)//年
                    {
                        searchCase.Append(GetFilter(rule.field, " >='" + data + "-01-01 00:00:00'"));
                    }
                    else if (data.Length == 7) //年月
                    {
                        searchCase.Append(GetFilter(rule.field, " >='" + data + "-01 00:00:00' '"));
                    }
                    else if (data.Length == 10) //年月日
                    {
                        searchCase.Append(GetFilter(rule.field, " >='" + data + " 00:00:00'"));
                    }
                    else if (data.Length == 13) //年月日时
                    {
                        searchCase.Append(GetFilter(rule.field, " >='" + data + " :00:00'"));
                    }
                    else if (data.Length == 16) //年月日时分
                    {
                        searchCase.Append(GetFilter(rule.field, " >='" + data + " :00'"));
                    }
                    else
                    {
                        searchCase.Append(GetFilter(rule.field, " >='" + data + "'"));
                    }
                    break;
                case "daterange"://针对时间特别处理
                    var dateTime = data.Split('|');
                    if (dateTime.Length == 2)
                    {
                        string beginTime = dateTime[0];
                        string endTime = dateTime[1];
                        if (beginTime.Length == 4) //年
                        {
                            beginTime = beginTime + "-01-01 00:00:00";
                            endTime = endTime + "-01-01 00:00:00";
                        }
                        else if (beginTime.Length == 7) //年月
                        {
                            beginTime = beginTime + "-01 00:00:00";
                            endTime = endTime + "-01 00:00:00";
                        }
                        else if (beginTime.Length == 10) //年月日
                        {
                            beginTime = beginTime + "00:00:00";
                            endTime = endTime + "00:00:00";
                        }
                        searchCase.Append(GetFilter(rule.field, " between '" + beginTime + "' AND '" + endTime + "'"));
                    }
                    break;
            }
            return searchCase.ToString();
        }

        //默认密钥向量 
        private static byte[] Keys = { 0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F };
        /// <summary> 
        /// DES加密字符串 
        /// </summary> 
        /// <param name="encryptString">待加密的字符串</param> 
        /// <param name="encryptKey">加密密钥,要求为16位</param> 
        /// <returns>加密成功返回加密后的字符串，失败返回源串</returns> 
        public static string Encrypt(string encryptString, string encryptKey = "Eip963Ace#321Key")
        {
            try
            {
                byte[] rgbKey = Encoding.UTF8.GetBytes(encryptKey.Substring(0, 16));
                byte[] rgbIv = Keys;
                byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
                var dcsp = Aes.Create();
                MemoryStream mStream = new MemoryStream();
                CryptoStream cStream = new CryptoStream(mStream, dcsp.CreateEncryptor(rgbKey, rgbIv), CryptoStreamMode.Write);
                cStream.Write(inputByteArray, 0, inputByteArray.Length);
                cStream.FlushFinalBlock();
                return Convert.ToBase64String(mStream.ToArray());
            }
            catch (Exception ex)
            {
                return ex.Message + encryptString;
            }
        }

        /// <summary>
        /// 转换
        /// </summary>
        /// <param name="field"></param>
        /// <param name="formula"></param>
        /// <returns></returns>
        private static string GetFilter(string field, string formula)
        {
            string search = string.Empty;
            StringBuilder searchCase = new StringBuilder();
            var split = field.Split(',');
            if (split.Length > 1)
            {
                foreach (var fi in field.Split(','))
                {
                    search += fi + formula + " || " /* " like '%" + data + "%' OR "*/;
                }
                searchCase.Append($"(" + search.Substring(0, search.Length - 4) + ")");
            }
            else
            {
                searchCase.Append($"('{field}'{formula})");
            }
            return searchCase.ToString();
        }

        #endregion

        #region 新增数据
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task AddData(SystemAgileAautomationNextChildInput input)
        {
            var childNode = input.Child.ChildNode.Data;
            SystemAgile agile = new SystemAgile();
            SystemDataBaseInput systemDataBaseInput = new SystemDataBaseInput();
            using (var fix = new SqlDatabaseFixture())
            {
                agile = await RedisHelper.CacheShellAsync($"ISystemAgileLogic_Cache:{childNode.Table}", DateTimeUtil.TotalSeconds(20), async () =>
                {
                    return await fix.Db.SystemAgile.SetSelect(s => new { s.DataBaseId, s.ColumnJson, s.DataFrom, s.DataFromName }).FindAsync(f => f.ConfigId == childNode.GetTypeTable);
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
                var datas = input.DataAllOutputs.FirstOrDefault(f => f.Id == childNode.AddMultipleData);
                if (datas != null)
                {
                    if (childNode.AddType == 1)
                    {

                    }
                    else if (childNode.AddType == 2)
                    {
                        string symbol = RepositoryUtil.GetSymbol();
                        List<string> doSqls = new List<string>();
                        //参数
                        var parameters = new Dictionary<string, object>();
                        for (int i = 0; i < datas.Data.Data.Count; i++)
                        {
                            StringBuilder stringBuilder = new StringBuilder();
                            StringBuilder stringBuilderValues = new StringBuilder();
                            stringBuilder.Append($"INSERT INTO {agile.DataFromName} (");
                            for (var j = 0; j < childNode.AddData.Count; j++)
                            {
                                var item = childNode.AddData[j];

                                if (item.Value.IsNotNullOrEmpty())
                                {
                                    string content = item.Value.ReplaceEditor();
                                    var h1Elements = item.Value.GetNodes();
                                    if (h1Elements != null)
                                    {
                                        foreach (var element in h1Elements)
                                        {
                                            //解码
                                            var columnSetting = HttpUtility.UrlDecode(element.Id);
                                            if (columnSetting != null)
                                            {
                                                var columnData = JsonConvert.DeserializeObject<SystemAgileAutomationJsonDataContentSettingOutput>(columnSetting);
                                                //得到对应节点值
                                                var value = datas.Data.Data[i][columnData.Model].ToString();
                                                //得到值
                                                content = content.Replace(element.OuterHtml, value);
                                            }
                                        }
                                    }
                                    stringBuilder.Append($"{item.Model},");
                                    stringBuilderValues.Append($"{symbol}{item.Model}{i}{j},");

                                    parameters.Add($"{item.Model}{i}{j}", content.Xss());

                                }

                            }
                            parameters.Add($"RelationId{i}", CombUtil.NewComb());

                            stringBuilder.Append("RelationId,");
                            stringBuilderValues.Append($"{symbol}RelationId{i},");

                            stringBuilder.Append("CreateTime,");
                            stringBuilderValues.Append($"{symbol}CreateTime,");

                            stringBuilder.Append("CreateUserId,");
                            stringBuilderValues.Append($"{symbol}CreateUserId,");

                            stringBuilder.Append("CreateUserName,");
                            stringBuilderValues.Append($"{symbol}CreateUserName,");

                            stringBuilder.Append("CreateOrganizationId,");
                            stringBuilderValues.Append($"{symbol}CreateOrganizationId,");

                            stringBuilder.Append("CreateOrganizationName,");
                            stringBuilderValues.Append($"{symbol}CreateOrganizationName,");

                            stringBuilder.Append("UpdateUserId,");
                            stringBuilderValues.Append($"{symbol}UpdateUserId,");

                            stringBuilder.Append("UpdateUserName,");
                            stringBuilderValues.Append($"{symbol}UpdateUserName,");

                            stringBuilder.Append("UpdateOrganizationId,");
                            stringBuilderValues.Append($"{symbol}UpdateOrganizationId,");

                            stringBuilder.Append("UpdateOrganizationName,");
                            stringBuilderValues.Append($"{symbol}UpdateOrganizationName,");

                            stringBuilder.Append("IsDelete,");
                            stringBuilderValues.Append($"false,");

                            //拼接Sql
                            var sql = stringBuilder.ToString().TrimEnd(',') + " ) VALUES (" +
                                   stringBuilderValues.ToString().TrimEnd(',') + ")";

                            doSqls.Add(sql);
                        }

                        if (doSqls.Count > 0)
                        {
                            try
                            {
                                var user = EipHttpContext.CurrentUser();
                                parameters.Add($"CreateTime", DateTime.Now);
                                parameters.Add($"CreateUserId", user.UserId);
                                parameters.Add($"CreateUserName", user.Name);
                                parameters.Add($"CreateOrganizationId", user.OrganizationId);
                                parameters.Add($"CreateOrganizationName", user.OrganizationName);
                                parameters.Add($"UpdateTime", DateTime.Now);
                                parameters.Add($"UpdateUserId", user.UserId);
                                parameters.Add($"UpdateUserName", user.Name);
                                parameters.Add($"UpdateOrganizationId", user.OrganizationId);
                                parameters.Add($"UpdateOrganizationName", user.OrganizationName);

                                OperateStatus<Guid> operateStatus = new OperateStatus<Guid>();
                                var trans = connection.BeginTransaction();
                                var totalInstances = doSqls.Count();
                                int maxAllowedInstancesPerBatch = 30;
                                //总页数
                                int exceededTimes = (int)Math.Ceiling(Convert.ToDouble(totalInstances) / maxAllowedInstancesPerBatch);
                                if (exceededTimes > 1)
                                {
                                    for (int i = 0; i <= exceededTimes; i++)
                                    {
                                        var skips = i * maxAllowedInstancesPerBatch;

                                        if (skips >= totalInstances)
                                            break;

                                        var items = doSqls.Skip(skips).Take(maxAllowedInstancesPerBatch);
                                        connection.Execute(items.ExpandAndToString(";"), parameters, trans);
                                    }
                                }
                                else
                                {
                                    connection.Execute(doSqls.ExpandAndToString(";"), parameters, trans);
                                }
                                trans.Commit();
                                operateStatus.Code = ResultCode.Success;
                                operateStatus.Msg = Chs.Successful;
                            }
                            catch (Exception)
                            {
                                throw;
                            }
                        }
                    }
                }
            }
        }

        #endregion

        #region 编辑数据
        /// <summary>
        /// 新增数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task UpdateData(SystemAgileAautomationNextChildInput input)
        {
            var childNode = input.Child.ChildNode.Data;
            SystemAgile agile = new SystemAgile();
            SystemDataBaseInput systemDataBaseInput = new SystemDataBaseInput();
            using (var fix = new SqlDatabaseFixture())
            {
                //判断更新数据源类型
                Guid? configId = null;
                var updateObj = input.DataAllOutputs.FirstOrDefault(f => f.Id == childNode.UpdateObj);
                if (updateObj.Json.Type == EnumAgileAutomationControlType.数据改变.ToShort())
                {
                    configId = updateObj.Json.Data.Table;
                }
                agile = await RedisHelper.CacheShellAsync($"ISystemAgileLogic_Cache:{configId}", DateTimeUtil.TotalSeconds(20), async () =>
                {
                    return await fix.Db.SystemAgile.SetSelect(s => new { s.DataBaseId, s.ColumnJson, s.DataFrom, s.DataFromName }).FindAsync(f => f.ConfigId == configId);
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
                var datas = input.DataAllOutputs.FirstOrDefault(f => f.Id == childNode.UpdateObj);
                if (datas != null)
                {

                    string symbol = RepositoryUtil.GetSymbol();
                    List<string> doSqls = new List<string>();
                    //参数
                    var parameters = new Dictionary<string, object>();
                    for (int i = 0; i < datas.Data.Count; i++)
                    {
                        StringBuilder stringBuilder = new StringBuilder();
                        StringBuilder stringBuilderValues = new StringBuilder();
                        stringBuilder.Append($"UPDATE {agile.DataFromName} SET ");

                        var updateData = childNode.UpdateData.Where(w => w.Value.IsNotNullOrEmpty()).ToList();
                        var relationId = "";
                        for (var j = 0; j < updateData.Count; j++)
                        {
                            var item = updateData[j];

                            if (item.Value.IsNotNullOrEmpty())
                            {
                                string content = item.Value.ReplaceEditor();
                                var h1Elements = item.Value.GetNodes();
                                if (h1Elements != null)
                                {
                                    foreach (var element in h1Elements)
                                    {
                                        //解码
                                        var columnSetting = HttpUtility.UrlDecode(element.Id);
                                        if (columnSetting != null)
                                        {
                                            var columnData = JsonConvert.DeserializeObject<SystemAgileAutomationJsonDataContentSettingOutput>(columnSetting);
                                            //得到对应节点值
                                            dynamic info = new ExpandoObject();
                                            var itemData = (IDictionary<string, object>)datas.Data[i];
                                            foreach (var dd in itemData)
                                            {
                                                string dkey = dd.Key;
                                                object dvalue = dd.Value;
                                                if (dkey == columnData.Model)
                                                {
                                                    //得到值
                                                    content = content.Replace(element.OuterHtml, dvalue.ToString());
                                                }
                                                if (dkey == "RelationId")
                                                {
                                                    relationId = dvalue.ToString();
                                                }
                                            }
                                        }
                                    }
                                }
                                stringBuilder.Append($"{item.Model}={symbol}{item.Model}{i}{j},");

                                parameters.Add($"{item.Model}{i}{j}", content.Xss());
                            }
                        }
                        parameters.Add($"RelationId{i}", relationId);
                        //拼接Sql
                        var sql = stringBuilder.ToString().TrimEnd(',');
                        sql += $" WHERE RelationId={symbol}RelationId{i}";
                        doSqls.Add(sql);
                    }

                    if (doSqls.Count > 0)
                    {
                        try
                        {
                            var user = EipHttpContext.CurrentUser();
                            parameters.Add($"CreateTime", DateTime.Now);
                            parameters.Add($"CreateUserId", user.UserId);
                            parameters.Add($"CreateUserName", user.Name);
                            parameters.Add($"CreateOrganizationId", user.OrganizationId);
                            parameters.Add($"CreateOrganizationName", user.OrganizationName);
                            parameters.Add($"UpdateTime", DateTime.Now);
                            parameters.Add($"UpdateUserId", user.UserId);
                            parameters.Add($"UpdateUserName", user.Name);
                            parameters.Add($"UpdateOrganizationId", user.OrganizationId);
                            parameters.Add($"UpdateOrganizationName", user.OrganizationName);

                            OperateStatus<Guid> operateStatus = new OperateStatus<Guid>();
                            var trans = connection.BeginTransaction();
                            var totalInstances = doSqls.Count();
                            int maxAllowedInstancesPerBatch = 30;
                            //总页数
                            int exceededTimes = (int)Math.Ceiling(Convert.ToDouble(totalInstances) / maxAllowedInstancesPerBatch);
                            if (exceededTimes > 1)
                            {
                                for (int i = 0; i <= exceededTimes; i++)
                                {
                                    var skips = i * maxAllowedInstancesPerBatch;

                                    if (skips >= totalInstances)
                                        break;

                                    var items = doSqls.Skip(skips).Take(maxAllowedInstancesPerBatch);
                                    connection.Execute(items.ExpandAndToString(";"), parameters, trans);
                                }
                            }
                            else
                            {
                                connection.Execute(doSqls.ExpandAndToString(";"), parameters, trans);
                            }
                            trans.Commit();
                            operateStatus.Code = ResultCode.Success;
                            operateStatus.Msg = Chs.Successful;
                        }
                        catch (Exception)
                        {
                            throw;
                        }
                    }
                }
            }
        }

        #endregion

        #region 获取数据
        /// <summary>
        /// 获取多条数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private void GetMultipleData(SystemAgileAautomationNextChildInput input)
        {
            var childNode = input.Child.ChildNode;
            //获取方式工作表
            if (childNode.Data.GetType == 1)
            {

            }
            //获取方式自定义请求
            else if (childNode.Data.GetType == 2)
            {
                //得到自定义请求数据
                var data = input.DataAllOutputs.FirstOrDefault(f => f.Id == childNode.Data.GetCustomerRequest);
                input.DataAllOutputs.Add(new SystemAgileAutomationJsonDataAllOutput
                {
                    Data = data,
                    Id = childNode.Id
                });
            }
        }

        #endregion

        #region 删除数据
        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task DeleteData(SystemAgileAautomationNextChildInput input)
        {
            var childNode = input.Child.ChildNode;
            SystemAgile agile = new SystemAgile();
            SystemDataBaseInput systemDataBaseInput = new SystemDataBaseInput();
            using (var fix = new SqlDatabaseFixture())
            {
                agile = await RedisHelper.CacheShellAsync($"ISystemAgileLogic_Cache:{childNode.Data.GetTypeTable}", DateTimeUtil.TotalSeconds(20), async () =>
                {
                    return await fix.Db.SystemAgile.SetSelect(s => new { s.DataBaseId, s.ColumnJson, s.DataFrom, s.DataFromName }).FindAsync(f => f.ConfigId == childNode.Data.GetTypeTable);
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

                string where = "";
                //是否具有条件，若有条件则筛选数据条件
                where = SearchFilterUtil.ConvertFilters(childNode.Data.ConditionFilters);

                string sql = $"DELETE FROM {agile.DataFromName} WHERE 1=1 " + where;
                //查询数据
                var queryData = connection.Execute(sql);
                //若有满足数据，执行下面流程
                if (queryData > 0)
                {
                    input.DataAllOutputs.Add(new SystemAgileAutomationJsonDataAllOutput
                    {
                        Data = queryData,
                        Id = childNode.Id
                    });
                }
            }
        }

        #endregion

        #region 通知消息
        /// <summary>
        /// 发送通知消息
        /// </summary>
        /// <param name="noticeUsers"></param>
        /// <param name="type"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        private async Task SendNotice(IList<SystemUserInfo> noticeUsers,
             EnumAgileAutomationControlType type,
             string message = "")
        {
            if (noticeUsers.Any())
            {
                foreach (var user in noticeUsers)
                {
                    string returnUrl = "";
                    //判断通知类型
                    switch (type)
                    {
                        case EnumAgileAutomationControlType.发送站内通知:
                            await _systemMessageLogic.SendWebSite(new SendWebSiteInput
                            {
                                Message = message,
                                Title = "消息通知",
                                ReturnUrl = returnUrl,
                                MessageLogId = CombUtil.NewComb(),
                                ReceiverId = user.UserId.ToString(),
                                ReceiverName = user.Name,
                                ReceiverType = EnumReceiverType.人员
                            });
                            break;
                        case EnumAgileAutomationControlType.发送短信:
                            await _systemMessageLogic.SendSms(new SendSmsInput
                            {
                                Message = message,
                                ReturnUrl = returnUrl,
                                Phone = user.Mobile,
                                MessageLogId = CombUtil.NewComb(),
                                ReceiverId = user.UserId.ToString(),
                                ReceiverName = user.Name,
                                ReceiverType = EnumReceiverType.人员
                            });
                            break;
                        case EnumAgileAutomationControlType.发送邮件:
                            _systemMessageLogic.SendEmail(new SendEmailInput
                            {
                                Name = "邮件通知",
                                Message = message,
                                ToAddress = user.Email,
                                ToName = user.Name,
                                MessageLogId = CombUtil.NewComb(),
                                ReceiverId = user.UserId.ToString(),
                                ReceiverName = user.Name,
                                ReceiverType = EnumReceiverType.人员
                            });
                            break;
                    }

                }
            }
        }

        /// <summary>
        /// 获取通知内容
        /// </summary>
        /// <param name="config"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        private string GetNoticeContent(SystemAgileAutomationJsonDataOutput config, List<SystemAgileAutomationJsonDataAllOutput> dataAllOutputs)
        {
            string content = config.Content.ReplaceEditor();
            var h1Elements = config.Content.GetNodes();
            if (h1Elements != null)
            {
                foreach (var element in h1Elements)
                {
                    //解码
                    var columnSetting = HttpUtility.UrlDecode(element.Id);
                    if (columnSetting != null)
                    {
                        var columnData = JsonConvert.DeserializeObject<SystemAgileAutomationJsonDataContentSettingOutput>(columnSetting);
                        //得到对应节点值
                        var data = dataAllOutputs.FirstOrDefault(f => f.Id == columnData.Id);
                        if (data != null)
                        {
                            foreach (var item in data.Data)
                            {
                                dynamic info = new ExpandoObject();
                                var dic = (IDictionary<string, object>)info;
                                foreach (var dd in item)
                                {
                                    string dkey = dd.Key;
                                    object dvalue = dd.Value;
                                    if (dkey == columnData.Model)
                                    {
                                        //得到值
                                        content = content.Replace(element.OuterHtml, dvalue.ToString());
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return content;
        }

        /// <summary>
        /// 获取通知用户
        /// </summary>
        /// <param name="config"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task<IList<SystemUserInfo>> GetNoticeUser(SystemAgileAutomationJsonDataOutput config, SystemAgileDataLog input)
        {
            IList<SystemUserInfo> noticeUsers = new List<SystemUserInfo>();
            IList<string> ids = new List<string>();
            if (config.Range.Any())
            {
                ids = config.Range.Select(s => s.Id).ToList();
            }
            //得到通知人
            using (var fixture = new SqlDatabaseFixture())
            {
                switch ((EnumAgileAutomationNoticeUserType)config.UserType)
                {
                    case EnumAgileAutomationNoticeUserType.所有成员://所有人员
                        var users = await fixture.Db.SystemUserInfo.SetSelect(s => new { s.UserId, s.Name, s.Email, s.Mobile }).FindAllAsync(f => !f.IsAdmin && !f.IsFreeze);
                        foreach (var item in users)
                        {
                            noticeUsers.Add(item);
                        }
                        break;
                    case EnumAgileAutomationNoticeUserType.组织://组织
                        foreach (var id in ids)
                        {
                            var gid = Guid.Parse(id);
                            var orgs = (await fixture.Db.SystemPermissionUserFindByPrivilegeMasterAndValueOutput
                                .FindAllAsync<SystemPermissionUserFindByPrivilegeMasterAndValueUsersOutput>(
                                    f => f.PrivilegeMaster == EnumPrivilegeMaster.组织架构.ToShort() &&
                                         f.PrivilegeMasterValue == gid, o => o.Users)).ToList();
                            if (!orgs.Any()) continue;
                            foreach (var item in orgs[0].Users)
                            {
                                noticeUsers.Add(new SystemUserInfo()
                                {
                                    Name = item.Name,
                                    Email = item.Email,
                                    Mobile = item.Mobile,
                                });
                            }
                        }
                        break;
                    case EnumAgileAutomationNoticeUserType.人员://人员
                        if (ids.Any())
                        {
                            var userId = new List<Guid>();
                            foreach (var id in ids)
                            {
                                userId.Add(Guid.Parse(id));
                            }
                            var permissionUser = await fixture.Db.SystemUserInfo.SetSelect(s => new { s.UserId, s.Name, s.Email, s.Mobile }).FindAllAsync(f => userId.Contains(f.UserId));
                            if (permissionUser != null)
                            {
                                foreach (var item in permissionUser)
                                {
                                    noticeUsers.Add(item);
                                }
                            }
                        }
                        break;
                    case EnumAgileAutomationNoticeUserType.角色://角色人员
                        foreach (var id in ids)
                        {
                            var gid = Guid.Parse(id);
                            var roles = (await fixture.Db.SystemPermissionUserFindByPrivilegeMasterAndValueOutput
                                .FindAllAsync<SystemPermissionUserFindByPrivilegeMasterAndValueUsersOutput>(
                                    f => f.PrivilegeMaster == EnumPrivilegeMaster.角色.ToShort() &&
                                         f.PrivilegeMasterValue == gid, o => o.Users)).ToList();
                            if (!roles.Any()) continue;
                            foreach (var item in roles[0].Users)
                            {
                                noticeUsers.Add(new SystemUserInfo()
                                {
                                    UserId = item.UserId,
                                    Name = item.Name,
                                    Email = item.Email,
                                    Mobile = item.Mobile,
                                    //Qq = item.Qq,
                                    //WeChat = item.WeChat
                                });
                            }
                        }
                        break;
                    case EnumAgileAutomationNoticeUserType.岗位://岗位
                        break;
                    case EnumAgileAutomationNoticeUserType.组://组
                        break;
                    case EnumAgileAutomationNoticeUserType.发起者://发起人
                        var user = await fixture.Db.SystemUserInfo.SetSelect(s => new { s.UserId, s.Name, s.Email, s.Mobile }).FindAsync(f => f.UserId == input.CreateUserId);
                        noticeUsers.Add(new SystemUserInfo()
                        {
                            UserId = input.CreateUserId,
                            Name = input.CreateUserName,
                            Email = user.Email,
                            Mobile = user.Mobile,
                            //Qq = item.Qq,
                            //WeChat = item.WeChat
                        });
                        break;
                    case EnumAgileAutomationNoticeUserType.提交人直线领导: //提交人的直线领导
                        var userLeader = (await fixture.Db.SystemUserFindLeaderOutput.FindAllAsync<SystemUserLeadersOutput>(f => f.UserId == input.CreateUserId, o => o.Outputs)).ToList();
                        if (userLeader.Any())
                        {
                            foreach (var item in userLeader.First().Outputs)
                            {
                                noticeUsers.Add(new SystemUserInfo()
                                {
                                    UserId = item.UserId,
                                    Name = item.Name,
                                    Email = item.Email,
                                    Mobile = item.Mobile,
                                    //Qq = item.Qq,
                                    //WeChat = item.WeChat
                                });
                            }
                        }
                        break;
                    case EnumAgileAutomationNoticeUserType.自定义: //自定义Sql表达式
                        foreach (string ruleSql in ids)
                        {
                            string sql = string.Empty;
                            //解析Sql
                            if (!ruleSql.IsNullOrEmpty())
                            {
                                //替换Html标签
                                sql = ruleSql.ReplaceHtmlTag();
                            }
                        }
                        break;
                    case EnumAgileAutomationNoticeUserType.程序指定: //程序指定
                        break;
                }
            }
            return noticeUsers;
        }

        #endregion

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
        #endregion
    }
}