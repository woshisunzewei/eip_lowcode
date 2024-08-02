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
using MySqlConnection = MySqlConnector.MySqlConnection;

namespace EIP.System.Logic.Impl
{
    /// <summary>
    /// 系统配置文件接口实现
    /// </summary>
    public class SystemDataBaseLogic : DapperAsyncLogic<SystemDataBase>, ISystemDataBaseLogic
    {
        #region 构造函数
        /// <summary>
        /// 关联文本
        /// </summary>
        private readonly string relationTxt = "_Txt";
        /// <summary>
        /// 需要建立值和键两个字段
        /// </summary>
        private readonly string[] _relationField = { "correlationrecord", "radio", "checkbox", "organization", "dictionary", "user", "select", "district", "map", "cascader", "treeselect" };
        /// <summary>
        /// 多选,建立附表字段
        /// </summary>
        private readonly string[] _multipleField = { "organization", "dictionary", "user", "select", "checkbox", "district", "treeselect" };

        /// <summary>
        /// filterSql字段，是否过滤数据字段
        /// </summary>
        private readonly string[] _filterSqlField = { "switch", "sign", "editor" };

        /// <summary>
        /// 无需创建字段
        /// </summary>
        private readonly string[] _noCreateField = { "batch", "alert" };
        private readonly ISystemDataBaseRepository _dataBaseRepository;
        private readonly ISystemAgileLogic _agileConfigLogic;
        private readonly ISystemPermissionLogic _permissionLogic;
        private readonly ICapPublisher _publisher;
        private readonly ISystemSerialNoLogic _systemSerialNoLogic;
        /// <summary>
        /// 
        /// </summary>
        public SystemDataBaseLogic(ISystemSerialNoLogic systemSerialnoLogic, ICapPublisher publisher, ISystemDataBaseRepository dataBaseRepository,
            ISystemAgileLogic agileConfigLogic, ISystemPermissionLogic permissionLogic)
        {
            _systemSerialNoLogic = systemSerialnoLogic;
            _publisher = publisher;
            _dataBaseRepository = dataBaseRepository;
            _agileConfigLogic = agileConfigLogic;
            _permissionLogic = permissionLogic;
        }

        #endregion

        #region 方法
        public async Task<OperateStatus<IEnumerable<BaseTree>>> FindTableTree()
        {
            List<BaseTree> jsTree = new List<BaseTree>();

            //循环表
            var tables = (await FindDataBaseTable(new IdInput { })).Data.ToList();
            foreach (var table in tables)
            {
                jsTree.Add(new BaseTree
                {
                    parent = Guid.Empty,
                    text = table.Name + (table.Description.IsNotNullOrEmpty() ? "-[" + table.Description + "]" : ""),
                    id = table.Name,
                    icon = "table"
                });
            }

            //获取字段
            var columns = (await FindDataBaseColumns(new SystemDataBaseTableDto()
            {
                Name = tables.Select(s => s.Name).ExpandAndToString()
            })).Data.ToList();
            foreach (var table in tables)
            {
                foreach (var column in columns.Where(w => w.TableName == table.Name))
                {
                    jsTree.Add(new BaseTree
                    {
                        parent = table.Name,
                        text = column.Name + "(" + column.DataType + (column.DataType.Contains("char") ? "(" + column.MaxLength + ")" : "") + "," + (column.IsNullable.ToLower() == "yes" ? "null" : "not null") + ")"/* +(column.ColumnDescription.IsNullOrEmpty()?"": "-[" + column.ColumnDescription + "]")*/ ,
                        id = table.Name + "|" + column.Name,
                        icon = "bars"
                    });
                }
            }
            return OperateStatus<IEnumerable<BaseTree>>.Success(jsTree);
        }

        /// <summary>
        /// 查看对应数据库空间占用情况
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemDataBaseSpaceOutput>>> FindDataBaseSpaceused()
        {
            return OperateStatus<IEnumerable<SystemDataBaseSpaceOutput>>.Success(await _dataBaseRepository.FindDataBaseSpaceused());
        }

        /// <summary>
        /// 获取对应数据库表信息
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemDataBaseTableDto>>> FindDataBaseTable(IdInput input)
        {
            var tables = await _dataBaseRepository.FindDataBaseTable(await GetSystemDataBase(input));
            using (var fix = new SqlDatabaseFixture())
            {
                var agile = await fix.Db.SystemAgile.SetSelect(s => new { s.DataFromName }).FindAllAsync(f => f.ConfigType == 2);
                foreach (var item in agile.Where(w => w.DataFromName.IsNotNullOrEmpty()))
                {
                    var agileData = tables.Where(a => a.Name.ToLower().Contains(item.DataFromName.ToLower()));
                    foreach (var agileConfig in agileData)
                    {
                        agileConfig.IsFromAgile = true;
                    }
                }
            }
            return OperateStatus<IEnumerable<SystemDataBaseTableDto>>.Success(tables.OrderByDescending(o => o.IsFromAgile));
        }

        /// <summary>
        /// 获取对应数据库表信息
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemDataBaseTableDto>>> FindDataBaseView(IdInput input)
        {
            return OperateStatus<IEnumerable<SystemDataBaseTableDto>>.Success(await _dataBaseRepository.FindDataBaseView(await GetSystemDataBase(input)));
        }

        /// <summary>
        /// 获取对应数据库表信息
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemDataBaseTableDto>>> FindDataBaseProc(IdInput input)
        {
            return OperateStatus<IEnumerable<SystemDataBaseTableDto>>.Success(await _dataBaseRepository.FindDataBaseProc(await GetSystemDataBase(input)));
        }
        /// <summary>
        /// 获取对应表列信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemDataBaseColumnDto>>> FindDataBaseColumns(SystemDataBaseTableDto input)
        {
            var dataBase = await GetSystemDataBase(new IdInput
            {
                Id = input.DataBaseId
            });
            input.ConnectionString = dataBase.ConnectionString;
            input.ConnectionType = dataBase.ConnectionType;
            return OperateStatus<IEnumerable<SystemDataBaseColumnDto>>.Success(await _dataBaseRepository.FindDataBaseColumns(input));
        }
        /// <summary>
        /// 获取对应数据库表信息
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemDataBaseTableDto>>> FindDataBaseWorkflowTables(IdInput input)
        {
            return OperateStatus<IEnumerable<SystemDataBaseTableDto>>.Success((await _dataBaseRepository.FindDataBaseTable(await GetSystemDataBase(input))).ToList().Where(w => w.Name.Contains("Form_")));
        }

        /// <summary>
        /// 获取对应表列信息
        /// </summary>
        /// <param name="doubleWayDto"></param>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemDataBaseColumnDto>>> FindWorkflowDataBaseColumnsList(SystemDataBaseTableDto doubleWayDto)
        {
            if (doubleWayDto.Name.IsNullOrEmpty())
            {
                return OperateStatus<IEnumerable<SystemDataBaseColumnDto>>.Success(new List<SystemDataBaseColumnDto>());
            }
            var output = (await _dataBaseRepository.FindDataBaseColumns(doubleWayDto)).ToList();
            //排除
            string[] fields = { "Id", "WorkflowSn", "WorkflowTitle", "WorkflowStatus", "ProcessInstanceId", "CreateUserId", "CreateUserName", "CreateOrganizationId", "CreateTime", "UpdateTime", "CreateOrganizationName", "UpdateUserId", "UpdateUserName", "UpdateOrganizationId", "UpdateOrganizationName" };
            for (int i = output.Count - 1; i >= 0; i--)
            {
                if (fields.Any(w => w.ToLower() == output[i].Name.ToLower()))
                    output.Remove(output[i]);
            }
            return OperateStatus<IEnumerable<SystemDataBaseColumnDto>>.Success(output);
        }

        /// <summary>
        /// 获取外键信息
        /// </summary>
        /// <param name="doubleWayDto"></param>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemDataBaseFkColumnOutput>>> FindDataBasefFkColumn(SystemDataBaseTableDto doubleWayDto)
        {
            return OperateStatus<IEnumerable<SystemDataBaseFkColumnOutput>>.Success(await _dataBaseRepository.FinddatabsefFkColumn(doubleWayDto));
        }

        /// <summary>
        /// 表是否存在
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<bool>> IsTableExist(SystemDataBaseIsTableExistInput input)
        {
            using (var fix = new SqlDatabaseFixture())
            {
                if (input.MenuId.HasValue)
                {
                    var config = await fix.Db.SystemAgile.SetSelect(s => new { s.ConfigId }).FindAsync(f => f.MenuId == input.MenuId && f.ConfigType == 2);
                    if (config != null)
                    {
                        input.ConfigId = config.ConfigId;
                    }
                }

                if (!input.ConfigId.IsEmptyGuid())
                {
                    var form = await fix.Db.SystemAgile.SetSelect(s => new { s.DataFromName }).FindAsync(f => f.ConfigId == input.ConfigId);
                    if (form != null)
                    {
                        input.DataFromName = form.DataFromName;
                    }
                }
            }
            var dataBase = await GetSystemDataBase(new IdInput
            {
                Id = input.DataBaseId
            });
            input.ConnectionType = dataBase.ConnectionType;
            input.ConnectionString = dataBase.ConnectionString;
            return OperateStatus<bool>.Success(await _dataBaseRepository.IsTableExist(input));
        }

        /// <summary>
        /// 创建表单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public OperateStatus SaveFormTable(SystemDataBaseSaveFormTableInput input)
        {
            OperateStatus operateStatus = new OperateStatus();
            try
            {
                using (var fix = new SqlDatabaseFixture())
                {
                    var config = fix.Db.SystemAgile.SetSelect(s => new { s.DataBaseId, s.DataFromName }).Find(f => f.ConfigId == input.ConfigId);
                    var dataBase = fix.Db.SystemDataBase.SetSelect(s => new { s.DataBaseId, s.ConnectionType, s.ConnectionString }).Find(f => f.DataBaseId == config.DataBaseId);
                    input.ConnectionType = dataBase.ConnectionType;
                    input.ConnectionString = ConfigurationUtil.GetSection(dataBase.ConnectionString);
                }
                switch (input.ConnectionType)
                {
                    case ResourceDataBaseType.Mysql:
                        operateStatus = SaveFormTableMySql(input);
                        break;
                    case ResourceDataBaseType.Dm://达梦
                        operateStatus = SaveFormTableDm(input);
                        break;
                    case ResourceDataBaseType.Kingbase://达梦
                        operateStatus = SaveFormTableDm(input);
                        break;
                    case ResourceDataBaseType.Postgresql:
                        break;
                    default:
                        operateStatus = SaveFormTableSqlServer(input);
                        break;
                }
            }
            catch (Exception e)
            {
                operateStatus.Msg = e.Message;
                return operateStatus;
            }
            return operateStatus;
        }

        /// <summary>
        /// 创建表Mysql
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private OperateStatus SaveFormTableSqlServer(SystemDataBaseSaveFormTableInput input)
        {
            OperateStatus operateStatus = new OperateStatus();
            try
            {
                string connectionString = input.ConnectionString;
                string connectionType = input.ConnectionType;
                string sql;
                using (var fix = new SqlDatabaseFixture())
                {
                    var config = fix.Db.SystemAgile.SetSelect(s => new { s.DataFromName, s.MenuId }).Find(f => f.ConfigId == input.ConfigId);
                    //之前表是否存在
                    var isHaveTableName = IsTableExist(input.ConnectionString, input.ConnectionType, config.DataFromName);
                    if (!isHaveTableName.Data)
                    {
                        sql = $@"
                        CREATE TABLE [dbo].[@table](
                            [Id] int Identity(1,1) primary key NOT NULL,
                            [RelationId] [uniqueidentifier] NOT NULL,
                            [IsDelete] [bit] NOT NULL DEFAULT 0,
                            [CreateTime] [datetime] not null default getdate(),
	                        [CreateUserName] [nvarchar](64) NULL,
                            [UpdateTime] [datetime]  null default getdate(),
	                        [UpdateUserName] [nvarchar](64) NULL,

                            [CreateUserId] [uniqueidentifier] NULL,
	                        [CreateOrganizationId] [uniqueidentifier] NULL,
	                        [CreateOrganizationName] [nvarchar](256) NULL,

                            [UpdateUserId] [uniqueidentifier] NULL,
	                        [UpdateOrganizationId] [uniqueidentifier] NULL,
	                        [UpdateOrganizationName] [nvarchar](256) NULL
                        )
                        EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'@table', @level2type=N'COLUMN',@level2name=N'Id'
                        EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'关联Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'@table', @level2type=N'COLUMN',@level2name=N'RelationId'
                        EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'是否删除' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'@table', @level2type=N'COLUMN',@level2name=N'IsDelete'
                        EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'@table', @level2type=N'COLUMN',@level2name=N'CreateTime'
                        EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'@table', @level2type=N'COLUMN',@level2name=N'CreateUserId'
                        EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'@table', @level2type=N'COLUMN',@level2name=N'CreateUserName'
                        EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人组织机构Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'@table', @level2type=N'COLUMN',@level2name=N'CreateOrganizationId'
                        EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建人组织机构名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'@table', @level2type=N'COLUMN',@level2name=N'CreateOrganizationName'

                        EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'@table', @level2type=N'COLUMN',@level2name=N'UpdateTime'
                        EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'@table', @level2type=N'COLUMN',@level2name=N'UpdateUserId'
                        EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'@table', @level2type=N'COLUMN',@level2name=N'UpdateUserName'
                        EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人组织机构Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'@table', @level2type=N'COLUMN',@level2name=N'UpdateOrganizationId'
                        EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'修改人组织机构名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'@table', @level2type=N'COLUMN',@level2name=N'UpdateOrganizationName'

                        EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'{input.Remark}' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'{input.DataFromName}'";
                        sql = sql.Replace("@table", input.DataFromName.FilterSql());
                        new DbHelper(connectionString, connectionType).ExecuteSql(sql);
                    }
                    else
                    {
                        if (input.DataFromName != config.DataFromName)
                        {
                            isHaveTableName = IsTableExist(connectionString, connectionType, input.DataFromName);
                            if (isHaveTableName.Data)
                            {
                                operateStatus.Msg = "已存在相同名称表";
                                return operateStatus;
                            }
                            else
                            {
                                //重命名表名
                                sql = $"EXEC sp_rename '{config.DataFromName.FilterSql()}', '{input.DataFromName.FilterSql()}'";
                                new DbHelper(connectionString, connectionType).ExecuteSql(sql);

                                //更新配置表
                                sql = $" UPDATE System_Agile SET DataFromName='{input.DataFromName}' WHERE MenuId='{config.MenuId}' ";
                                new DbHelper(ConfigurationUtil.GetDbConnectionString(), ConfigurationUtil.GetDbConnectionType()).ExecuteSql(sql);
                            }
                        }
                        string updateDescriptionStr = $" EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=N'{input.Remark}' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'{input.DataFromName}'";
                        new DbHelper(connectionString, connectionType).ExecuteSql(updateDescriptionStr);
                    }
                }
            }
            catch (Exception e)
            {
                operateStatus.Msg = e.Message;
                return operateStatus;
            }
            operateStatus.Msg = Chs.Successful;
            operateStatus.Code = ResultCode.Success;
            return operateStatus;
        }

        /// <summary>
        /// 创建表Mysql
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private OperateStatus SaveFormTableMySql(SystemDataBaseSaveFormTableInput input)
        {
            OperateStatus operateStatus = new OperateStatus();
            try
            {
                string sql;
                using (var fix = new SqlDatabaseFixture())
                {
                    var config = fix.Db.SystemAgile.SetSelect(s => new { s.DataFromName, s.MenuId }).Find(f => f.ConfigId == input.ConfigId);
                    //之前表是否存在
                    var isHaveTableName = IsTableExist(input.ConnectionString, input.ConnectionType, config.DataFromName);
                    if (!isHaveTableName.Data)
                    {
                        sql = $@"
                        CREATE TABLE {input.DataFromName}(
                            Id INT(50) primary key NOT NULL AUTO_INCREMENT COMMENT '主键',
	                        RelationId char(36)  NOT NULL COMMENT '关联主键',
                            IsDelete bit  NOT NULL DEFAULT 1 COMMENT '是否删除',
                            CreateTime datetime not null DEFAULT CURRENT_TIMESTAMP COMMENT '创建时间',
	                        CreateUserName varchar(64) NULL COMMENT '创建人',
                            UpdateTime datetime not null DEFAULT CURRENT_TIMESTAMP COMMENT '修改时间',
	                        UpdateUserName varchar(64) NULL COMMENT '修改人',
                            CreateUserId char(36) NULL COMMENT '创建人Id',
                            CreateOrganizationId char(36) NULL COMMENT '创建人组织机构Id',
	                        CreateOrganizationName varchar(256) NULL COMMENT '创建人组织机构名称',
                            UpdateUserId char(36) NULL COMMENT '修改人Id',
	                        UpdateOrganizationId char(36) NULL COMMENT '修改人组织机构Id',
	                        UpdateOrganizationName varchar(256) NULL COMMENT '修改人组织机构名称'
                        )ENGINE=INNODB DEFAULT CHARSET=utf8mb4 COMMENT='{input.Remark}';
                        alter table {input.DataFromName} convert to character set utf8mb4 collate utf8mb4_unicode_ci;
                      ";

                        new DbHelper(input.ConnectionString, input.ConnectionType).ExecuteSql(sql);

                        operateStatus.Msg = Chs.Successful;
                        operateStatus.Code = ResultCode.Success;
                    }
                    else
                    {
                        if (input.DataFromName != config.DataFromName)
                        {
                            isHaveTableName = IsTableExist(input.ConnectionString, input.ConnectionType, input.DataFromName);
                            if (isHaveTableName.Data)
                            {
                                operateStatus.Msg = "已存在相同名称表";
                                return operateStatus;
                            }
                            else
                            {
                                //重命名表名
                                sql = $"ALTER TABLE {config.DataFromName} RENAME TO  {input.DataFromName}";
                                new DbHelper(input.ConnectionString, input.ConnectionType).ExecuteSql(sql);

                                //更新配置表
                                sql = $" UPDATE System_Agile SET DataFromName='{input.DataFromName}' WHERE MenuId='{config.MenuId}' ";
                                new DbHelper(ConfigurationUtil.GetDbConnectionString(), ConfigurationUtil.GetDbConnectionType()).ExecuteSql(sql);
                            }

                        }
                        string updateDescriptionStr = $" alter table {input.DataFromName} comment '{input.Remark}';";
                        new DbHelper(input.ConnectionString, input.ConnectionType).ExecuteSql(updateDescriptionStr);
                        operateStatus.Msg = Chs.Successful;
                        operateStatus.Code = ResultCode.Success;
                    }
                }
            }
            catch (Exception e)
            {
                operateStatus.Msg = e.Message;
                return operateStatus;
            }
            return operateStatus;
        }

        /// <summary>
        /// 创建表达梦
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private OperateStatus SaveFormTableDm(SystemDataBaseSaveFormTableInput input)
        {
            OperateStatus operateStatus = new OperateStatus();
            try
            {
                string connectionString = input.ConnectionString;
                string connectionType = input.ConnectionType;
                string sql;
                var isHaveTableName = IsTableExist(connectionString, connectionType, input.DataFromName);
                if (isHaveTableName.Data)
                {
                    using (var fix = new SqlDatabaseFixture())
                    {
                        var config = fix.Db.SystemAgile.SetSelect(s => new { s.DataFromName }).Find(f => f.ConfigId == input.ConfigId);
                        if (config == null)
                        {
                            operateStatus.Msg = "已存在相同名称表";
                            return operateStatus;
                        }
                        sql = $"EXEC sp_rename '{config.DataFromName.FilterSql()}', '{input.DataFromName.FilterSql()}'";
                    }
                    var renameTableStrResult = new DbHelper(connectionString, connectionType).ExecuteSql(sql);
                    string updateDescriptionStr = $" EXEC sys.sp_updateextendedproperty @name=N'MS_Description', @value=N'{input.Remark}' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'{input.DataFromName}'";
                    new DbHelper(connectionString, connectionType).ExecuteSql(updateDescriptionStr);
                }
                else
                {
                    sql = $@"
                        CREATE TABLE {input.DataFromName}(
                            Id int Identity(1,1) primary key NOT NULL,
                            RelationId varchar(36) NOT NULL,
                            IsDelete bit  NOT NULL DEFAULT 1 ,
                            CreateTime datetime not null default getdate(),
	                        CreateUserName nvarchar(64) NULL,
                            UpdateTime datetime  null default getdate(),
	                        UpdateUserName nvarchar(64) NULL,

                            CreateUserId  varchar(36) NULL,
	                        CreateOrganizationId  varchar(36) NULL,
	                        CreateOrganizationName nvarchar(256) NULL,

                            UpdateUserId  varchar(36) NULL,
	                        UpdateOrganizationId  varchar(36) NULL,
	                        UpdateOrganizationName nvarchar(256) NULL
                        )";
                    new DbHelper(connectionString, connectionType).ExecuteSql(sql);
                    new DbHelper(connectionString, connectionType).ExecuteSql($"COMMENT ON TABLE \"{input.DataFromName}\" IS '{input.Remark}'");
                    new DbHelper(connectionString, connectionType).ExecuteSql($"COMMENT ON COLUMN \"{input.DataFromName}\".\"Id\" IS '主键'");
                    new DbHelper(connectionString, connectionType).ExecuteSql($"COMMENT ON COLUMN \"{input.DataFromName}\".\"RelationId\" IS '关联Id'");
                    new DbHelper(connectionString, connectionType).ExecuteSql($"COMMENT ON COLUMN \"{input.DataFromName}\".\"IsDelete\" IS '是否删除'");
                    new DbHelper(connectionString, connectionType).ExecuteSql($"COMMENT ON COLUMN \"{input.DataFromName}\".\"CreateTime\" IS '创建时间'");
                    new DbHelper(connectionString, connectionType).ExecuteSql($"COMMENT ON COLUMN \"{input.DataFromName}\".\"CreateUserId\" IS '创建人Id'");
                    new DbHelper(connectionString, connectionType).ExecuteSql($"COMMENT ON COLUMN \"{input.DataFromName}\".\"CreateUserName\" IS '创建人'");
                    new DbHelper(connectionString, connectionType).ExecuteSql($"COMMENT ON COLUMN \"{input.DataFromName}\".\"CreateOrganizationId\" IS '创建人组织机构Id'");
                    new DbHelper(connectionString, connectionType).ExecuteSql($"COMMENT ON COLUMN \"{input.DataFromName}\".\"CreateOrganizationName\" IS '创建人组织机构名称'");
                    new DbHelper(connectionString, connectionType).ExecuteSql($"COMMENT ON COLUMN \"{input.DataFromName}\".\"UpdateTime\" IS '修改时间'");
                    new DbHelper(connectionString, connectionType).ExecuteSql($"COMMENT ON COLUMN \"{input.DataFromName}\".\"UpdateUserId\" IS '修改人Id'");
                    new DbHelper(connectionString, connectionType).ExecuteSql($"COMMENT ON COLUMN \"{input.DataFromName}\".\"UpdateUserId\" IS '修改人'");
                    new DbHelper(connectionString, connectionType).ExecuteSql($"COMMENT ON COLUMN \"{input.DataFromName}\".\"UpdateOrganizationId\" IS '修改人组织机构Id'");
                    new DbHelper(connectionString, connectionType).ExecuteSql($"COMMENT ON COLUMN \"{input.DataFromName}\".\"UpdateOrganizationName\" IS '修改人组织机构名称'");
                }
            }
            catch (Exception e)
            {
                operateStatus.Msg = e.Message;
                return operateStatus;
            }
            operateStatus.Code = ResultCode.Success;
            return operateStatus;
        }

        /// <summary>
        /// 保存表字段
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> SaveFormTableField(SystemDataBaseSaveFormTableFieldInput input)
        {
            OperateStatus operateStatus = new OperateStatus();
            //得到原有表字段
            try
            {
                var form = await _agileConfigLogic.FindAsync(f => f.ConfigId == input.ConfigId);
                if (form.FormCategory == EnumFormCategory.设计器.ToShort())
                {
                    var dataBase = await GetSystemDataBase(new IdInput { Id = form.DataBaseId });
                    string connectionType = dataBase.ConnectionType;
                    string connectionString = dataBase.ConnectionString;
                    var columns = (await _dataBaseRepository.FindDataBaseColumns(new SystemDataBaseTableDto
                    {
                        ConnectionString = dataBase.ConnectionString,
                        ConnectionType = dataBase.ConnectionType,
                        Name = form.DataFromName
                    })).ToList();

                    #region 排重
                    var fields = input.Columns.JsonStringToList<SystemDataBaseSaveFormTableFieldDetailInput>();
                    var fieldsRepeats = fields.GroupBy(x => x.Name).Where(x => x.Count() > 1).ToList();
                    if (fieldsRepeats.Any())
                    {
                        string groupByStr = string.Empty;
                        foreach (var fieldsRepeat in fieldsRepeats)
                        {
                            groupByStr += fieldsRepeat.Key + ",";
                        }
                        operateStatus.Msg = "字段：" + groupByStr.TrimEnd(',') + " 重复,请修改后重试";
                        return operateStatus;
                    }
                    #endregion

                    #region 获取新增字段
                    IList<SystemDataBaseSaveFormTableFieldDetailInput> insert = new List<SystemDataBaseSaveFormTableFieldDetailInput>();
                    IList<SystemDataBaseSaveFormTableFieldDetailInput> delete = new List<SystemDataBaseSaveFormTableFieldDetailInput>();
                    IList<SystemDataBaseSaveFormTableFieldDetailInput> modify = new List<SystemDataBaseSaveFormTableFieldDetailInput>();
                    columns = (columns.Where(w => w.Name != "Id" &&
                                                  w.Name != "WorkflowStatus" &&
                                                  w.Name != "WorkflowSn" &&
                                                  w.Name != "WorkflowTitle" &&
                                                  w.Name != "RelationId" &&
                                                  w.Name != "IsDelete" &&
                                                  w.Name != "CreateTime" &&
                                                  w.Name != "CreateUserId" &&
                                                  w.Name != "CreateUserName" &&
                                                  w.Name != "CreateOrganizationId" &&
                                                  w.Name != "CreateOrganizationName" &&
                                                  w.Name != "UpdateTime" &&
                                                  w.Name != "UpdateUserId" &&
                                                  w.Name != "UpdateUserName" &&
                                                  w.Name != "UpdateOrganizationId" &&
                                                  w.Name != "UpdateOrganizationName")).ToList();

                    //获取删除字段
                    foreach (var column in columns)
                    {
                        //当前字段是否在传入中
                        var fi = fields.Where(w => w.Name == column.Name || w.Name + relationTxt == column.Name);
                        //不在则删除
                        if (!fi.Any())
                        {
                            delete.Add(new SystemDataBaseSaveFormTableFieldDetailInput { Name = column.Name });
                        }
                        else
                        {
                            //长度是否变化
                            var field = fi.FirstOrDefault();
                            string type = column.DataType.ToLower();
                            switch (type)
                            {
                                case "varchar":
                                case "nvarchar":
                                    if (connectionType == ResourceDataBaseType.Mysql
                                        || connectionType == ResourceDataBaseType.Dm)
                                    {
                                        type = $"nvarchar({column.MaxLength})";
                                    }
                                    else
                                    {
                                        type += $"({column.MaxLength})";
                                    }
                                    break;

                                default:
                                    break;
                            }
                            if (field.DataType != type || (field.Null && column.IsNullable.IsNullOrEmpty()) || (!field.Null && column.IsNullable.IsNotNullOrEmpty()))
                            {
                                modify.Add(field);
                            }
                        }
                    }

                    //获取新增字段
                    foreach (var field in fields)
                    {
                        if (!_noCreateField.Contains(field.ControlType.ToLower()))
                        {
                            //不在则加入新增
                            if (columns.All(w => w.Name != field.Name))
                            {
                                insert.Add(field);
                            }
                            else
                            {
                                //判断是否在需要加关联id里面
                                if (_relationField.Contains(field.ControlType.ToLower()))
                                {
                                    //是否存在
                                    if (columns.All(w => w.Name != field.Name + relationTxt))
                                    {
                                        insert.Add(new SystemDataBaseSaveFormTableFieldDetailInput
                                        {
                                            Name = field.Name + relationTxt
                                        });
                                    }
                                }
                            }
                        }
                    }
                    #endregion

                    switch (connectionType)
                    {
                        case ResourceDataBaseType.Mysql:
                            operateStatus = await SaveFormTableFieldMySql(connectionString, connectionType, form, fields, insert, delete, modify);
                            break;
                        case ResourceDataBaseType.Postgresql:
                            break;
                        case ResourceDataBaseType.Dm:
                            operateStatus = await SaveFormTableFieldDm(connectionString, connectionType, form, fields, insert, delete, modify);
                            break;
                        default:
                            operateStatus = await SaveFormTableFieldSqlServerSql(connectionString, connectionType, form, fields, insert, delete, modify);
                            break;
                    }
                }
                else
                {
                    operateStatus.Code = ResultCode.Success;
                    operateStatus.Msg = "操作成功";
                }
            }
            catch (Exception e)
            {
                operateStatus.Msg = e.Message;
            }
            return operateStatus;
        }

        /// <summary>
        /// 执行Mysql保存字段
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="form"></param>
        /// <param name="fields"></param>
        /// <param name="insert"></param>
        /// <returns></returns>
        private async Task<OperateStatus> SaveFormTableFieldSqlServerSql(
            string connectionString,
            string connectionType,
            SystemAgile form,
            IList<SystemDataBaseSaveFormTableFieldDetailInput> fields,
            IList<SystemDataBaseSaveFormTableFieldDetailInput> insert,
            IList<SystemDataBaseSaveFormTableFieldDetailInput> delete,
            IList<SystemDataBaseSaveFormTableFieldDetailInput> modify)
        {
            OperateStatus operateStatus = new OperateStatus();
            //得到原有表字段
            try
            {
                #region 主表
                //删除字段
                if (delete.Any())
                {
                    string createTableStr = $"ALTER TABLE {form.DataFromName} DROP COLUMN {string.Join(",", delete.Select(s => "[" + s.Name + "]"))}";
                    new DbHelper(connectionString, connectionType).ExecuteSql(createTableStr);
                }
                //修改字段长度或类型
                if (modify.Any())
                {
                    List<string> modifySql = new List<string>();
                    foreach (var ins in modify)
                    {
                        var sql = ($"ALTER TABLE {form.DataFromName} ALTER COLUMN {ins.Name} {((ins.DataType == "nvarchar(36)" || ins.DataType == "varchar(36)") ? "uniqueidentifier" : ins.DataType)} ");
                        modifySql.Insert(0, sql);
                        if (_relationField.Contains(ins.ControlType.ToLower()))
                        {
                            sql = ($"ALTER TABLE {form.DataFromName} ALTER COLUMN {ins.Name + relationTxt} {((ins.DataType == "nvarchar(36)" || ins.DataType == "varchar(36)") ? $"varchar({ins.MaxLength})" : ins.DataType)} {(ins.Null ? "NULL" : "NOT NULL")}  COMMENT '{ins.Description}{ins.Help}' ");
                            modifySql.Insert(0, sql);
                        }
                    }
                    var modifyStr = modifySql.ExpandAndToString(";");
                    new DbHelper(connectionString, connectionType).ExecuteSql(modifyStr);
                }
                //新增字段
                if (insert.Any())
                {
                    StringBuilder createTableStr = new StringBuilder();
                    StringBuilder descriptionStr = new StringBuilder();
                    foreach (var ins in insert)
                    {
                        createTableStr.Append($",[{ins.Name}] {((ins.DataType == "nvarchar(36)" || ins.DataType == "varchar(36)") ? "uniqueidentifier" : ins.DataType)} {(ins.Null ? "NULL" : "NOT NULL")}");
                        if (_relationField.Contains(ins.ControlType.ToLower()))
                        {
                            createTableStr.Append($",[{ins.Name + relationTxt}] {((ins.DataType == "nvarchar(36)" || ins.DataType == "varchar(36)") ? $"nvarchar({ins.MaxLength})" : ins.DataType)} {(ins.Null ? "NULL" : "NOT NULL")} ");
                            descriptionStr.Append($" execute sp_addextendedproperty 'MS_Description','{ins.Description}{ins.Help}',   'user', 'dbo', 'table', '{form.DataFromName}', 'column', '{ins.Name + relationTxt}'");
                        }
                        descriptionStr.Append($" execute sp_addextendedproperty 'MS_Description','{ins.Description}{ins.Help}',   'user', 'dbo', 'table', '{form.DataFromName}', 'column', '{ins.Name}'");
                    }
                    new DbHelper(connectionString, connectionType).ExecuteSql($"ALTER TABLE {form.DataFromName} ADD {createTableStr.ToString().TrimStart(',')}");
                    new DbHelper(connectionString, connectionType).ExecuteSql(descriptionStr.ToString());
                }

                //处理附表
                foreach (var ins in fields)
                {
                    if (_multipleField.Contains(ins.ControlType.ToLower()))
                    {
                        //判断是否多选
                        var tableName = form.DataFromName + "_" + ins.Name;
                        var isHaveTableName = IsTableExist(connectionString, connectionType, tableName);
                        //判断是否为多选
                        if (!ins.IsSingle)
                        {
                            if (!isHaveTableName.Data)
                            {
                                string sql = $@"CREATE TABLE [dbo].[@table](
	                                        [Id] int identity(1,1) primary key,
                                            [RelationId] [uniqueidentifier] NOT NULL,
                                            [CorrelationId] NVARCHAR(256) NOT NULL,
                                            [Value] NVARCHAR(256) NULL)
                                            EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'{ins.Description}{ins.Help}' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'@table'
                                            EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'外键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'@table', @level2type=N'COLUMN',@level2name=N'RelationId'
                                            EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'业务数据Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'@table', @level2type=N'COLUMN',@level2name=N'CorrelationId'
                                            EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'业务数据值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'@table', @level2type=N'COLUMN',@level2name=N'Value'";
                                sql = sql.Replace("@table", tableName);
                                new DbHelper(connectionString, connectionType).ExecuteSql(sql);
                            }
                        }
                        //单选
                        else
                        {
                            if (isHaveTableName.Data)
                            {
                                new DbHelper(connectionString, connectionType).ExecuteSql($"DROP TABLE {tableName}");
                            }
                        }
                    }
                }
                #endregion

                #region 子表 
                //判断是否具有子表
                var batchs = fields.Where(w => w.ControlType.ToLower().Contains("batch")).ToList();
                if (batchs.Any())
                {
                    foreach (var batch in batchs)
                    {
                        batch.Name = form.DataFromName + "_" + batch.Name;
                        #region 排重
                        var subfields = batch.Children;
                        var subfieldsRepeats = subfields.GroupBy(x => x.Name).Where(x => x.Count() > 1).ToList();
                        if (subfieldsRepeats.Any())
                        {
                            string subGroupByStr = string.Empty;
                            foreach (var fieldsRepeat in subfieldsRepeats)
                            {
                                subGroupByStr += fieldsRepeat.Key + ",";
                            }
                            operateStatus.Msg = $"子表【{batch.Description}{batch.Name}】字段：" + subGroupByStr.TrimEnd(',') + " 重复,请修改后重试";
                            return operateStatus;
                        }
                        #endregion

                        #region 创建附主表
                        //子表主表
                        if (!IsTableExist(connectionString, connectionType, batch.Name).Data)
                        {
                            //新建表
                            string sql = $@"CREATE TABLE [dbo].[@table](
	                                        [Id] int Identity(1,1) primary key NOT NULL,
                                            [BatchId] [uniqueidentifier] NOT NULL
                                            [RelationId] [uniqueidentifier] NOT NULL)
                                            EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'主键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'@table', @level2type=N'COLUMN',@level2name=N'Id'
                                            EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'子表Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'@table', @level2type=N'COLUMN',@level2name=N'BatchId'
                                            EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'关联Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'@table', @level2type=N'COLUMN',@level2name=N'RelationId'
                                            EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'{batch.Description}' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'@table'";
                            sql = sql.Replace("@table", batch.Name);
                            new DbHelper(connectionString, connectionType).ExecuteSql(sql);
                        }
                        #endregion

                        var subcolumns = (await _dataBaseRepository.FindDataBaseColumns(new SystemDataBaseTableDto
                        {
                            ConnectionString = connectionString,
                            ConnectionType = connectionType,
                            Name = batch.Name
                        })).ToList();

                        IList<SystemDataBaseSaveFormTableFieldDetailInput> batchFieldDelete = new List<SystemDataBaseSaveFormTableFieldDetailInput>();
                        IList<SystemDataBaseSaveFormTableFieldDetailInput> batchFieldInsert = new List<SystemDataBaseSaveFormTableFieldDetailInput>();
                        IList<SystemDataBaseSaveFormTableFieldDetailInput> batchFieldModify = new List<SystemDataBaseSaveFormTableFieldDetailInput>();
                        subcolumns = (subcolumns.Where(w => w.Name != "Id" &&
                                                      w.Name != "BatchId" &&
                                                      w.Name != "RelationId")).ToList();

                        #region 删除字段 
                        foreach (var column in subcolumns)
                        {
                            var fi = subfields.Where(w => w.Name == column.Name || w.Name + relationTxt == column.Name);
                            //不在则删除
                            if (!fi.Any())
                            {
                                batchFieldDelete.Add(new SystemDataBaseSaveFormTableFieldDetailInput { Name = column.Name });
                            }
                            else
                            {
                                //长度是否变化
                                var field = fi.FirstOrDefault();
                                string type = column.DataType;
                                switch (column.DataType)
                                {
                                    case "varchar":
                                    case "nvarchar":
                                        type += $"({column.MaxLength})";
                                        break;
                                    default:
                                        break;
                                }
                                if (field.DataType != type)
                                {
                                    batchFieldModify.Add(field);
                                }
                            }
                        }
                        //执行删除字段语句
                        if (batchFieldDelete.Any())
                        {
                            string createTableStr = $"ALTER TABLE {batch.Name} DROP COLUMN {string.Join(",", batchFieldDelete.Select(s => "[" + s.Name + "]"))}";
                            new DbHelper(connectionString, connectionType).ExecuteSql(createTableStr);
                        }
                        #endregion

                        #region 修改字段长度
                        if (batchFieldModify.Any())
                        {
                            List<string> modifySql = new List<string>();
                            foreach (var ins in batchFieldModify)
                            {
                                var sql = ($"ALTER TABLE {batch.Name} ALTER COLUMN {ins.Name} {ins.DataType} ");
                                modifySql.Insert(0, sql);
                                if (_relationField.Contains(ins.ControlType.ToLower()))
                                {
                                    sql = ($"ALTER TABLE {batch.Name} modify column {ins.Name + relationTxt} {((ins.DataType == "nvarchar(36)" || ins.DataType == "varchar(36)") ? $"varchar({ins.MaxLength})" : ins.DataType)} {(ins.Null ? "NULL" : "NOT NULL")}  COMMENT '{ins.Description}{ins.Help}' ");
                                    modifySql.Insert(0, sql);
                                }

                            }
                            var modifyStr = modifySql.ExpandAndToString(";");
                            new DbHelper(connectionString, connectionType).ExecuteSql(modifyStr);
                        }
                        #endregion

                        #region 新增字段
                        foreach (var field in subfields)
                        {
                            if (!_noCreateField.Contains(field.ControlType.ToLower()))
                            {
                                //不在则新增
                                if (subcolumns.Where(w => w.Name == field.Name).Count() == 0)
                                {
                                    batchFieldInsert.Add(field);
                                }
                                else
                                {
                                    //判断是否在需要加关联id里面
                                    if (_relationField.Contains(field.ControlType))
                                    {
                                        //是否存在
                                        if (subcolumns.Where(w => w.Name == field.Name + relationTxt).Count() == 0)
                                        {
                                            field.Name = field.Name + relationTxt;
                                            batchFieldInsert.Add(field);
                                        }
                                    }
                                }
                            }
                        }
                        if (batchFieldInsert.Any())
                        {
                            StringBuilder createTableStr = new StringBuilder();
                            StringBuilder descriptionStr = new StringBuilder();
                            foreach (var ins in batchFieldInsert)
                            {
                                if (ins.DataType.IsNotNullOrEmpty())
                                {
                                    createTableStr.Append($",[{ins.Name}] {((ins.DataType == "nvarchar(36)" || ins.DataType == "varchar(36)") ? "uniqueidentifier" : ins.DataType)} {(ins.Null ? "NULL" : " NOT NULL ")}");
                                    if (_relationField.Contains(ins.DataType))
                                    {
                                        createTableStr.Append($",[{ins.Name + relationTxt}] {((ins.DataType == "nvarchar(36)" || ins.DataType == "varchar(36)") ? $"nvarchar({ins.MaxLength})" : ins.DataType)} {(ins.Null ? "NULL" : " NOT NULL ")} ");
                                        descriptionStr.Append($" execute sp_addextendedproperty 'MS_Description','{ins.Description}{ins.Help}',   'user', 'dbo', 'table', '{batch.Name}', 'column', '{ins.Name + relationTxt}'");
                                    }
                                    descriptionStr.Append($" execute sp_addextendedproperty 'MS_Description','{ins.Description}{ins.Help}',   'user', 'dbo', 'table', '{batch.Name}', 'column', '{ins.Name}'");
                                }
                            }
                            if (createTableStr.ToString().IsNotNullOrEmpty())
                            {
                                new DbHelper(connectionString, connectionType).ExecuteSql($"ALTER TABLE {batch.Name} ADD {createTableStr.ToString().TrimStart(',')}");
                                new DbHelper(connectionString, connectionType).ExecuteSql(descriptionStr.ToString());
                            }
                        }
                        //处理子附表
                        foreach (var ins in subfields)
                        {
                            if (_multipleField.Contains(ins.DataType))
                            {
                                //判断是否多选
                                var tableName = batch.Name + "_" + ins.Name;
                                var isHaveTableName = IsTableExist(connectionString, connectionType, tableName);
                                //判断是否为多选
                                if (!ins.IsSingle)
                                {
                                    if (!isHaveTableName.Data)
                                    {
                                        string sql = $@"CREATE TABLE [dbo].[@table](
	                                        [Id] int identity(1,1) primary key,
                                            [RelationId] [uniqueidentifier] NOT NULL,
                                            [CorrelationId] NVARCHAR(256) NOT NULL,
                                            [Value] NVARCHAR(256) NULL)
                                            EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'{ins.Description}{ins.Help}' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'@table'
                                            EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'外键' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'@table', @level2type=N'COLUMN',@level2name=N'RelationId'
                                            EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'业务数据Id' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'@table', @level2type=N'COLUMN',@level2name=N'CorrelationId'
                                            EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'业务数据值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'@table', @level2type=N'COLUMN',@level2name=N'Value'";
                                        sql = sql.Replace("@table", tableName);
                                        new DbHelper(connectionString, connectionType).ExecuteSql(sql);
                                    }
                                }
                                //单选
                                else
                                {
                                    //是否存在表
                                    if (isHaveTableName.Data)
                                    {
                                        //删除该表
                                        new DbHelper(connectionString, connectionType).ExecuteSql($"DROP TABLE {tableName}");
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }
                #endregion

                operateStatus.Code = ResultCode.Success;
            }
            catch (Exception e)
            {
                operateStatus.Msg = e.Message;
            }
            return operateStatus;
        }

        /// <summary>
        /// 执行Mysql保存字段
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="form"></param>
        /// <param name="fields"></param>
        /// <param name="insert"></param>
        /// <returns></returns>
        private async Task<OperateStatus> SaveFormTableFieldMySql(
            string connectionString,
            string connectionType,
            SystemAgile form,
            IList<SystemDataBaseSaveFormTableFieldDetailInput> fields,
            IList<SystemDataBaseSaveFormTableFieldDetailInput> insert,
            IList<SystemDataBaseSaveFormTableFieldDetailInput> delete,
            IList<SystemDataBaseSaveFormTableFieldDetailInput> modify)
        {
            OperateStatus operateStatus = new OperateStatus();
            //得到原有表字段
            try
            {

                #region 主表

                //删除字段
                if (delete.Any())
                {
                    string createTableStr = $"ALTER TABLE {form.DataFromName} {string.Join(",", delete.Select(s => "DROP " + s.Name + ""))}";
                    new DbHelper(connectionString, connectionType).ExecuteSql(createTableStr);
                }

                //修改字段长度或类型
                if (modify.Any())
                {
                    List<string> modifySql = new List<string>();
                    foreach (var ins in modify)
                    {
                        var sql = ($"ALTER TABLE {form.DataFromName} modify column {ins.Name} {ins.DataType} {(ins.Null ? "NULL" : "NOT NULL ")} COMMENT '{ins.Description}{ins.Help}' ");
                        modifySql.Insert(0, sql);
                        if (_relationField.Contains(ins.ControlType.ToLower()))
                        {
                            sql = ($"ALTER TABLE {form.DataFromName} modify column {ins.Name + relationTxt} {((ins.DataType == "nvarchar(36)" || ins.DataType == "varchar(36)") ? $"varchar({ins.MaxLength})" : ins.DataType)} {(ins.Null ? "NULL" : "NOT NULL")}  COMMENT '{ins.Description}{ins.Help}' ");
                            modifySql.Insert(0, sql);
                        }
                    }
                    var modifyStr = modifySql.ExpandAndToString(";");
                    new DbHelper(connectionString, connectionType).ExecuteSql(modifyStr);
                }

                //新增字段
                if (insert.Any())
                {
                    List<string> insertSql = new List<string>();
                    foreach (var ins in insert)
                    {
                        var sql = ($"ALTER TABLE {form.DataFromName} ADD {ins.Name} {ins.DataType} {(ins.Null ? "NULL" : "NOT NULL ")} COMMENT '{ins.Description}{ins.Help}' FIRST ");
                        insertSql.Insert(0, sql);
                        if (_relationField.Contains(ins.ControlType.ToLower()))
                        {
                            sql = ($"ALTER TABLE {form.DataFromName} ADD {ins.Name + relationTxt} {((ins.DataType == "nvarchar(36)" || ins.DataType == "varchar(36)") ? $"varchar({ins.MaxLength})" : ins.DataType)} {(ins.Null ? "NULL" : "NOT NULL")}  COMMENT '{ins.Description}{ins.Help}' FIRST ");
                            insertSql.Insert(0, sql);
                        }
                    }
                    var createTableStr = insertSql.ExpandAndToString(";");
                    new DbHelper(connectionString, connectionType).ExecuteSql(createTableStr);
                }

                //处理附表
                foreach (var ins in fields)
                {
                    if (_multipleField.Contains(ins.ControlType.ToLower()))
                    {
                        //判断是否多选
                        var tableName = form.DataFromName + "_" + ins.Name;
                        var isHaveTableName = IsTableExist(connectionString, connectionType, tableName);
                        //判断是否为多选
                        if (!ins.IsSingle)
                        {
                            if (!isHaveTableName.Data)
                            {
                                string sql = $@"CREATE TABLE {tableName}(
	                                        Id int primary key auto_increment COMMENT '{ins.Description}{ins.Help}',
                                            RelationId char (36) NOT NULL  COMMENT '外键',
                                            CorrelationId varchar(256) NOT NULL  COMMENT '业务数据Id',
                                            Value varchar(256) NULL  COMMENT '业务数据值')
                                            ENGINE=INNODB DEFAULT CHARSET=utf8mb4 COMMENT='{form.Name}';
                                            alter table {tableName} convert to character set utf8mb4 collate utf8mb4_unicode_ci;";
                                new DbHelper(connectionString, connectionType).ExecuteSql(sql);
                            }
                        }
                        //单选
                        else
                        {
                            if (isHaveTableName.Data)
                            {
                                new DbHelper(connectionString, connectionType).ExecuteSql($"DROP TABLE {tableName}");
                            }
                        }
                    }
                }
                #endregion

                #region 子表 
                //判断是否具有子表
                var batchs = fields.Where(w => w.ControlType.ToLower().Contains("batch")).ToList();
                if (batchs.Any())
                {
                    foreach (var batch in batchs)
                    {
                        batch.Name = form.DataFromName + "_" + batch.Name;
                        #region 排重
                        var batchFields = batch.Children;
                        var batchFieldsRepeats = batchFields.GroupBy(x => x.Name).Where(x => x.Count() > 1).ToList();
                        if (batchFieldsRepeats.Any())
                        {
                            string subGroupByStr = string.Empty;
                            foreach (var fieldsRepeat in batchFieldsRepeats)
                            {
                                subGroupByStr += fieldsRepeat.Key + ",";
                            }
                            operateStatus.Msg = $"子表【{batch.Description}{batch.Name}】字段：" + subGroupByStr.TrimEnd(',') + " 重复,请修改后重试";
                            return operateStatus;
                        }
                        #endregion

                        #region 创建附主表
                        //子表主表
                        if (!IsTableExist(connectionString, connectionType, batch.Name).Data)
                        {
                            //新建表
                            string sql = $@"CREATE TABLE {batch.Name}(
	                                        Id int primary key auto_increment COMMENT '主键',
                                            BatchId char(36) NOT NULL COMMENT '附表主键',
                                            RelationId char(36) NOT NULL COMMENT '外键')
                                            ENGINE=INNODB DEFAULT CHARSET=utf8mb4 COMMENT='{batch.Description}';
                                            alter table {batch.Name} convert to character set utf8mb4 collate utf8mb4_unicode_ci
                                          ";
                            sql = sql.Replace("@table", batch.Name);
                            new DbHelper(connectionString, connectionType).ExecuteSql(sql);
                        }
                        #endregion

                        var batchColumns = (await _dataBaseRepository.FindDataBaseColumns(new SystemDataBaseTableDto
                        {
                            ConnectionType = connectionType,
                            ConnectionString = connectionString,
                            Name = batch.Name
                        })).ToList();

                        IList<SystemDataBaseSaveFormTableFieldDetailInput> batchFieldDelete = new List<SystemDataBaseSaveFormTableFieldDetailInput>();
                        IList<SystemDataBaseSaveFormTableFieldDetailInput> batchFieldInsert = new List<SystemDataBaseSaveFormTableFieldDetailInput>();
                        IList<SystemDataBaseSaveFormTableFieldDetailInput> batchFieldModify = new List<SystemDataBaseSaveFormTableFieldDetailInput>();
                        batchColumns = (batchColumns.Where(w => w.Name != "Id" &&
                                                      w.Name != "BatchId" &&
                                                      w.Name != "RelationId")).ToList();

                        #region 删除字段 
                        foreach (var column in batchColumns)
                        {
                            var fi = batchFields.Where(w => w.Name == column.Name || w.Name + relationTxt == column.Name);
                            //不在则删除
                            if (!fi.Any())
                            {
                                batchFieldDelete.Add(new SystemDataBaseSaveFormTableFieldDetailInput { Name = column.Name });
                            }
                            else
                            {
                                //长度是否变化
                                var field = fi.FirstOrDefault();
                                string type = column.DataType;
                                switch (column.DataType)
                                {
                                    case "varchar":
                                    case "nvarchar":
                                        type = $"nvarchar({column.MaxLength})";
                                        break;
                                    default:
                                        break;
                                }
                                if (field.DataType != type)
                                {
                                    batchFieldModify.Add(field);
                                }
                            }
                        }
                        //执行删除字段语句
                        if (batchFieldDelete.Any())
                        {
                            string createTableStr = $"ALTER TABLE {batch.Name} {string.Join(",", batchFieldDelete.Select(s => "DROP " + s.Name + ""))}";
                            new DbHelper(connectionString, connectionType).ExecuteSql(createTableStr);
                        }
                        #endregion

                        #region 修改字段长度
                        if (batchFieldModify.Any())
                        {
                            List<string> modifySql = new List<string>();
                            foreach (var ins in batchFieldModify)
                            {
                                var sql = ($"ALTER TABLE {batch.Name} modify column {ins.Name} {ins.DataType} {(ins.Null ? "NULL" : "NOT NULL ")} COMMENT '{ins.Description}{ins.Help}' ");
                                modifySql.Insert(0, sql);
                                if (_relationField.Contains(ins.ControlType.ToLower()))
                                {
                                    sql = ($"ALTER TABLE {batch.Name} modify column {ins.Name + relationTxt} {((ins.DataType == "nvarchar(36)" || ins.DataType == "varchar(36)") ? $"varchar({ins.MaxLength})" : ins.DataType)} {(ins.Null ? "NULL" : "NOT NULL")}  COMMENT '{ins.Description}{ins.Help}' ");
                                    modifySql.Insert(0, sql);
                                }
                            }
                            var modifyStr = modifySql.ExpandAndToString(";");
                            new DbHelper(connectionString, connectionType).ExecuteSql(modifyStr);
                        }
                        #endregion

                        #region 新增字段
                        foreach (var field in batchFields)
                        {
                            if (!_noCreateField.Contains(field.ControlType.ToLower()))
                            {
                                //不在则新增
                                if (batchColumns.Where(w => w.Name == field.Name).Count() == 0)
                                {
                                    batchFieldInsert.Add(field);
                                }
                                else
                                {
                                    //判断是否在需要加关联id里面
                                    if (_relationField.Contains(field.ControlType))
                                    {
                                        //是否存在
                                        if (batchColumns.Where(w => w.Name == field.Name + relationTxt).Count() == 0)
                                        {
                                            field.Name = field.Name + relationTxt;
                                            batchFieldInsert.Add(field);
                                        }
                                    }
                                }
                            }
                        }

                        //新增
                        if (batchFieldInsert.Any())
                        {
                            List<string> insertSql = new List<string>();
                            foreach (var ins in batchFieldInsert)
                            {
                                var sql = ($"ALTER TABLE {batch.Name} ADD {ins.Name} {ins.DataType} {(ins.Null ? "NULL" : " NOT NULL ")} COMMENT '{ins.Description}{ins.Help}' FIRST ");
                                insertSql.Insert(0, sql);
                                if (_relationField.Contains(ins.ControlType.ToLower()))
                                {
                                    sql = ($"ALTER TABLE {batch.Name} ADD {ins.Name + relationTxt}  {((ins.DataType == "nvarchar(36)" || ins.DataType == "varchar(36)") ? $"varchar({ins.MaxLength})" : ins.DataType)} {(ins.Null ? "NULL" : " NOT NULL ")} COMMENT '{ins.Description}{ins.Help}' FIRST ");
                                    insertSql.Insert(0, sql);
                                }
                            }
                            if (insertSql.Any())
                            {
                                var createTableStr = insertSql.ExpandAndToString(";");
                                new DbHelper(connectionString, connectionType).ExecuteSql(createTableStr);
                            }
                        }

                        //处理子附表
                        foreach (var ins in batchFields)
                        {
                            if (_multipleField.Contains(ins.ControlType.ToLower()))
                            {
                                //判断是否多选
                                var tableName = batch.Name + "_" + ins.Name;
                                var isHaveTableName = IsTableExist(connectionString, connectionType, tableName);
                                //判断是否为多选
                                if (!ins.IsSingle)
                                {
                                    if (!isHaveTableName.Data)
                                    {
                                        string sql = $@"CREATE TABLE {tableName}(
	                                        Id int primary key auto_increment COMMENT '主键',
                                            BatchId char(36) NOT NULL COMMENT '附表主键',
                                            RelationId char(36) NOT NULL COMMENT '主表主键',
                                            CorrelationId varchar(256) NOT NULL COMMENT '关联数据',
                                            Value varchar(256) NULL COMMENT '值') ENGINE=INNODB DEFAULT CHARSET=utf8mb4 COMMENT='{batch.Description}';
                                            alter table {tableName} convert to character set utf8mb4 collate utf8mb4_unicode_ci; ";
                                        new DbHelper(connectionString, connectionType).ExecuteSql(sql);
                                    }
                                }
                                //单选
                                else
                                {
                                    //是否存在表
                                    if (isHaveTableName.Data)
                                    {
                                        //删除该表
                                        new DbHelper(connectionString, connectionType).ExecuteSql($"DROP TABLE {tableName}");
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }
                #endregion

                operateStatus.Code = ResultCode.Success;
            }
            catch (Exception e)
            {
                operateStatus.Msg = e.Message;
            }
            return operateStatus;
        }

        /// <summary>
        /// 执行达梦保存字段
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="form"></param>
        /// <param name="fields"></param>
        /// <param name="insert"></param>
        /// <returns></returns>
        private async Task<OperateStatus> SaveFormTableFieldDm(
           string connectionString,
           string connectionType,
            SystemAgile form,
            IList<SystemDataBaseSaveFormTableFieldDetailInput> fields,
            IList<SystemDataBaseSaveFormTableFieldDetailInput> insert,
            IList<SystemDataBaseSaveFormTableFieldDetailInput> delete,
            IList<SystemDataBaseSaveFormTableFieldDetailInput> modify)
        {
            OperateStatus operateStatus = new OperateStatus();
            //得到原有表字段
            try
            {
                #region 主表
                //删除字段
                if (delete.Any())
                {
                    foreach (var field in delete)
                    {
                        string createTableStr = $"ALTER TABLE {form.DataFromName} DROP COLUMN {field.Name}";
                        new DbHelper(connectionString, connectionType).ExecuteSql(createTableStr);
                    }
                }
                //修改字段长度或类型
                if (modify.Any())
                {
                    List<string> modifySql = new List<string>();
                    foreach (var ins in modify)
                    {
                        ins.DataType = ins.DataType.Replace("nvarchar", "varchar");
                        var sql = ($"ALTER TABLE {form.DataFromName} MODIFY {ins.Name} {ins.DataType} ");
                        new DbHelper(connectionString, connectionType).ExecuteSql(sql);
                    }
                }
                //新增字段
                if (insert.Any())
                {
                    foreach (var ins in insert)
                    {
                        ins.DataType = ins.DataType.Replace("nvarchar", "varchar");
                        new DbHelper(connectionString, connectionType).ExecuteSql($"ALTER TABLE {form.DataFromName} ADD {ins.Name} {ins.DataType} {(ins.Null ? "NULL" : "NOT NULL")}");
                        if (_relationField.Contains(ins.ControlType.ToLower()))
                        {
                            new DbHelper(connectionString, connectionType).ExecuteSql($"ALTER TABLE {form.DataFromName} ADD {ins.Name + relationTxt} {((ins.DataType == "nvarchar(36)" || ins.DataType == "varchar(36)") ? $"nvarchar({ins.MaxLength})" : ins.DataType)} {(ins.Null ? "NULL" : "NOT NULL")}");
                            new DbHelper(connectionString, connectionType).ExecuteSql($"COMMENT ON COLUMN \"{form.DataFromName}\".\"{ins.Name + relationTxt}\" IS '{ins.Description}{ins.Help}'");
                        }
                        new DbHelper(connectionString, connectionType).ExecuteSql($"COMMENT ON COLUMN \"{form.DataFromName}\".\"{ins.Name}\" IS '{ins.Description}{ins.Help}'");
                    }
                }

                //处理附表
                foreach (var ins in fields)
                {
                    if (_multipleField.Contains(ins.ControlType.ToLower()))
                    {
                        //判断是否多选
                        var tableName = form.DataFromName + "_" + ins.Name;
                        var isHaveTableName = IsTableExist(connectionString, connectionType, tableName);
                        //判断是否为多选
                        if (!ins.IsSingle)
                        {
                            if (!isHaveTableName.Data)
                            {
                                string sql = $@"CREATE TABLE {tableName}(
	                                        Id int identity(1,1) primary key,
                                            RelationId varchar(36) NOT NULL,
                                            CorrelationId NVARCHAR(256) NOT NULL,
                                            Value NVARCHAR(256) NULL)
                                           ";
                                new DbHelper(connectionString, connectionType).ExecuteSql(sql);

                                new DbHelper(connectionString, connectionType).ExecuteSql($"COMMENT ON TABLE \"{tableName}\" IS '{ins.Description}{ins.Help}'");
                                new DbHelper(connectionString, connectionType).ExecuteSql($"COMMENT ON COLUMN \"{tableName}\".\"RelationId\" IS '外键'");
                                new DbHelper(connectionString, connectionType).ExecuteSql($"COMMENT ON COLUMN \"{tableName}\".\"CorrelationId\" IS '业务数据Id'");
                                new DbHelper(connectionString, connectionType).ExecuteSql($"COMMENT ON COLUMN \"{tableName}\".\"Value\" IS '业务数据值'");
                            }
                        }
                        //单选
                        else
                        {
                            if (isHaveTableName.Data)
                            {
                                new DbHelper(connectionString, connectionType).ExecuteSql($"DROP TABLE {tableName}");
                            }
                        }
                    }
                }
                #endregion

                #region 子表 
                //判断是否具有子表
                var batchs = fields.Where(w => w.ControlType.ToLower().Contains("batch")).ToList();
                if (batchs.Any())
                {
                    foreach (var batch in batchs)
                    {
                        batch.Name = form.DataFromName + "_" + batch.Name;
                        #region 排重
                        var subfields = batch.Children;
                        var subfieldsRepeats = subfields.GroupBy(x => x.Name).Where(x => x.Count() > 1).ToList();
                        if (subfieldsRepeats.Any())
                        {
                            string subGroupByStr = string.Empty;
                            foreach (var fieldsRepeat in subfieldsRepeats)
                            {
                                subGroupByStr += fieldsRepeat.Key + ",";
                            }
                            operateStatus.Msg = $"子表【{batch.Description}{batch.Name}】字段：" + subGroupByStr.TrimEnd(',') + " 重复,请修改后重试";
                            return operateStatus;
                        }
                        #endregion

                        #region 创建附主表
                        //子表主表
                        if (!IsTableExist(connectionString, connectionType, batch.Name).Data)
                        {
                            //新建表
                            string sql = $@"CREATE TABLE {batch.Name}(
	                                        Id int Identity(1,1) primary key NOT NULL,
                                            RelationId varchar()36 NOT NULL)
                                           ";
                            new DbHelper(connectionString, connectionType).ExecuteSql(sql);
                            new DbHelper(connectionString, connectionType).ExecuteSql($"COMMENT ON TABLE \"{batch.Name}\" IS '{batch.Description}'");
                            new DbHelper(connectionString, connectionType).ExecuteSql($"COMMENT ON COLUMN \"{batch.Name}\".\"Id\" IS '主键'");
                            new DbHelper(connectionString, connectionType).ExecuteSql($"COMMENT ON COLUMN \"{batch.Name}\".\"RelationId\" IS '关联Id'");
                        }
                        #endregion

                        var subcolumns = (await _dataBaseRepository.FindDataBaseColumns(new SystemDataBaseTableDto
                        {
                            ConnectionString = connectionString,
                            ConnectionType = connectionType,
                            Name = batch.Name
                        })).ToList();

                        IList<SystemDataBaseSaveFormTableFieldDetailInput> batchFieldDelete = new List<SystemDataBaseSaveFormTableFieldDetailInput>();
                        IList<SystemDataBaseSaveFormTableFieldDetailInput> batchFieldInsert = new List<SystemDataBaseSaveFormTableFieldDetailInput>();
                        IList<SystemDataBaseSaveFormTableFieldDetailInput> batchFieldModify = new List<SystemDataBaseSaveFormTableFieldDetailInput>();
                        subcolumns = (subcolumns.Where(w => w.Name != "Id" &&
                                                      w.Name != "BatchId" &&
                                                      w.Name != "RelationId")).ToList();

                        #region 删除字段 
                        foreach (var column in subcolumns)
                        {
                            var fi = subfields.Where(w => w.Name == column.Name || w.Name + relationTxt == column.Name);
                            //不在则删除
                            if (!fi.Any())
                            {
                                batchFieldDelete.Add(new SystemDataBaseSaveFormTableFieldDetailInput { Name = column.Name });
                            }
                            else
                            {
                                //长度是否变化
                                var field = fi.FirstOrDefault();
                                string type = column.DataType;
                                switch (column.DataType)
                                {
                                    case "varchar":
                                    case "nvarchar":
                                        type += $"({column.MaxLength})";
                                        break;
                                    default:
                                        break;
                                }
                                if (field.DataType != type)
                                {
                                    batchFieldModify.Add(field);
                                }
                            }
                        }
                        //执行删除字段语句
                        if (batchFieldDelete.Any())
                        {
                            foreach (var field in batchFieldDelete)
                            {
                                string createTableStr = $"ALTER TABLE {batch.Name} DROP COLUMN {field.Name}";
                                new DbHelper(connectionString, connectionType).ExecuteSql(createTableStr);
                            }
                        }
                        #endregion

                        #region 修改字段长度
                        if (batchFieldModify.Any())
                        {
                            foreach (var ins in batchFieldModify)
                            {
                                var sql = ($"ALTER TABLE {batch.Name} MODIFY {ins.Name} {ins.DataType} ");
                                new DbHelper(connectionString, connectionType).ExecuteSql(sql);
                            }
                        }
                        #endregion

                        #region 新增字段
                        foreach (var field in subfields)
                        {
                            if (!_noCreateField.Contains(field.ControlType.ToLower()))
                            {
                                //不在则新增
                                if (subcolumns.Where(w => w.Name == field.Name).Count() == 0)
                                {
                                    batchFieldInsert.Add(field);
                                }
                                else
                                {
                                    //判断是否在需要加关联id里面
                                    if (_relationField.Contains(field.ControlType))
                                    {
                                        //是否存在
                                        if (subcolumns.Where(w => w.Name == field.Name + relationTxt).Count() == 0)
                                        {
                                            field.Name = field.Name + relationTxt;
                                            batchFieldInsert.Add(field);
                                        }
                                    }
                                }
                            }
                        }
                        if (batchFieldInsert.Any())
                        {

                            foreach (var ins in batchFieldInsert)
                            {
                                if (ins.DataType.IsNotNullOrEmpty())
                                {
                                    new DbHelper(connectionString, connectionType).ExecuteSql($"ALTER TABLE {batch.Name} ADD {ins.Name} {ins.DataType} {(ins.Null ? "NULL" : " NOT NULL ")}");
                                    if (_relationField.Contains(ins.DataType))
                                    {
                                        new DbHelper(connectionString, connectionType).ExecuteSql($"ALTER TABLE {batch.Name} ADD {ins.Name} {ins.DataType} {ins.Name + relationTxt} {((ins.DataType == "nvarchar(36)" || ins.DataType == "varchar(36)") ? $"nvarchar({ins.MaxLength})" : ins.DataType)} {(ins.Null ? "NULL" : " NOT NULL ")}");
                                        new DbHelper(connectionString, connectionType).ExecuteSql($"COMMENT ON COLUMN \"{batch.Name}\".\"{ins.Name + relationTxt}\" IS '{ins.Description}{ins.Help}'");
                                    }
                                    new DbHelper(connectionString, connectionType).ExecuteSql($"COMMENT ON COLUMN \"{batch.Name}\".\"{ins.Name}\" IS '{ins.Description}{ins.Help}'");
                                }
                            }
                        }
                        //处理子附表
                        foreach (var ins in subfields)
                        {
                            if (_multipleField.Contains(ins.DataType))
                            {
                                //判断是否多选
                                var tableName = batch.Name + "_" + ins.Name;
                                var isHaveTableName = IsTableExist(connectionString, connectionType, tableName);
                                //判断是否为多选
                                if (!ins.IsSingle)
                                {
                                    if (!isHaveTableName.Data)
                                    {
                                        string sql = $@"CREATE TABLE {tableName}(
	                                        Id int identity(1,1) primary key,
                                            RelationId varchar(36) NOT NULL,
                                            CorrelationId NVARCHAR(256) NOT NULL,
                                            Value NVARCHAR(256) NULL)
                                          ";
                                        new DbHelper(connectionString, connectionType).ExecuteSql(sql);

                                        new DbHelper(connectionString, connectionType).ExecuteSql($"COMMENT ON TABLE \"{tableName}\" IS '{ins.Description}{ins.Help}'");
                                        new DbHelper(connectionString, connectionType).ExecuteSql($"COMMENT ON COLUMN \"{tableName}\".\"RelationId\" IS '外键'");
                                        new DbHelper(connectionString, connectionType).ExecuteSql($"COMMENT ON COLUMN \"{tableName}\".\"CorrelationId\" IS '业务数据Id'");
                                        new DbHelper(connectionString, connectionType).ExecuteSql($"COMMENT ON COLUMN \"{tableName}\".\"Value\" IS '业务数据值'");

                                    }
                                }
                                //单选
                                else
                                {
                                    //是否存在表
                                    if (isHaveTableName.Data)
                                    {
                                        //删除该表
                                        new DbHelper(connectionString, connectionType).ExecuteSql($"DROP TABLE {tableName}");
                                    }
                                }
                            }
                        }
                        #endregion
                    }
                }
                #endregion

                operateStatus.Code = ResultCode.Success;
            }
            catch (Exception e)
            {
                operateStatus.Msg = e.Message;
            }
            return operateStatus;
        }

        #region 判断数据库中，指定表是否存在
        /// <summary>
        /// 判断数据库表是否存在
        /// </summary>
        /// <param name="conn">数据库连接字符串</param>
        /// <param name="tb">数据表名</param>
        /// <returns>true:表示数据表已经存在；false，表示数据表不存在</returns>
        public OperateStatus<bool> IsTableExist(string connectionString, string connectionType, string tb)
        {
            string createDbStr = string.Empty;
            switch (connectionType)
            {
                case ResourceDataBaseType.Mysql:
                    string schema = connectionString.Split(";")[0].ToString().Split("=")[1];
                    createDbStr = $"select 1 from information_schema.TABLES t where t.TABLE_NAME ='{tb}' and table_schema='{schema}'";
                    break;
                case ResourceDataBaseType.Dm:
                    createDbStr = $"select 1 from  sysobjects where NAME='{tb}'";
                    break;
                case ResourceDataBaseType.Postgresql:
                    break;
                default:
                    createDbStr = $"select 1 from  sysobjects where  id = object_id('{tb}') and type = 'U'";
                    break;
            }
            //在指定的数据库中  查找 该表是否存在
            DataTable dt = new DbHelper(connectionString, connectionType).CreateSqlDataTable(createDbStr);
            return OperateStatus<bool>.Success(dt.Rows.Count != 0);
        }

        #endregion

        /// <summary>
        /// 根据主键获取信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<object>> FindBusinessDataById(SystemDataBaseFindBusinessDataByIdInput input)
        {
            var agileConfig = (await _agileConfigLogic.FindColumnJsonJsonById(new IdInput { Id = input.ConfigId })).Data;
            var columns = agileConfig.ColumnJson.JsonStringToList<SystemAgileFindFormColumnsJsonOutput>();
            var format = new List<SystemDataBaseFindPagingBusinessDataTableColunmsInput>();
            foreach (var item in columns.Where(w => w.Type == "user"))
            {
                format.Add(new SystemDataBaseFindPagingBusinessDataTableColunmsInput
                {
                    Format = "User",
                    Name = item.Model
                });
            }
            var dataBase = await GetSystemDataBase(new IdInput { Id = agileConfig.DataBaseId });
            string connectionType = dataBase.ConnectionType;
            string connectionString = dataBase.ConnectionString;
            //值对应的用户Id集合
            List<string> userIds = new List<string>();
            List<dynamic> data = new List<dynamic>();
            List<SystemUserHeaderOutput> user = new List<SystemUserHeaderOutput>();
            using (var db = GetConnectoin(connectionString, connectionType))
            {
                string sql = $"SELECT * FROM {agileConfig.DataFromName} WHERE RelationId='{input.Id}'";
                data = (await db.QueryAsync(sql)).ToList();

                if (format.Any())
                {
                    foreach (var item in data)
                    {
                        foreach (var dd in item)
                        {
                            string dkey = dd.Key;
                            var formatUsers = format.Any(a => a.Name == dkey);
                            if (formatUsers)
                            {
                                object dvalue = dd.Value;
                                if (dvalue != null)
                                {
                                    var ids = dvalue.ToString().Split(',');
                                    foreach (var id in ids)
                                    {
                                        if (!userIds.Any(a => a == id))
                                        {
                                            userIds.Add(id);
                                        }
                                    }
                                }
                            }
                        }
                    }


                }
            }
            if (userIds.Any())
            {
                //查询用户头像
                var sqlUser = $"select HeadImage,UserId from system_userinfo where UserId in ({userIds.ExpandAndToString().InSql()})";
                using (var fix = new SqlDatabaseFixture())
                {
                    user = (await fix.Db.Connection.QueryAsync<SystemUserHeaderOutput>(sqlUser)).ToList();
                }
            }
            //是否具有需要替换的Id,或者需要掩码
            if (userIds.Any())
            {
                List<dynamic> tmp = new List<dynamic>();
                foreach (var item in data)
                {
                    dynamic info = new ExpandoObject();
                    var dic = (IDictionary<string, object>)info;
                    foreach (var dd in item)
                    {
                        string dkey = dd.Key;
                        object dvalue = dd.Value;

                        if (dvalue != null)
                        {
                            //头像处理
                            var formatUsers = format.Any(a => a.Name == dkey);
                            if (formatUsers)
                            {
                                var userIdSplit = dvalue.ToString().Split(',');
                                List<string> userIdHeader = new List<string>();
                                foreach (var userId in userIdSplit)
                                {
                                    userIdHeader.Add(user.FirstOrDefault(f => f.UserId.ToString() == userId)?.HeadImage);
                                }
                                dic.Add(dkey + "_TxtHeader", userIdHeader.ExpandAndToString());
                            }
                        }
                        dic.Add(dkey, dvalue);
                    }
                    tmp.Add(info);
                }
                data = tmp;
            }

            return OperateStatus<object>.Success(data.FirstOrDefault());
        }

        /// <summary>
        /// 根据主键获取信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<Dictionary<string, object>>> FindBusinessDataById(SystemDataBaseExportTemplateInput input)
        {
            var agileConfig = (await _agileConfigLogic.FindColumnJsonJsonById(new IdInput { Id = input.ConfigId })).Data;
            var columns = agileConfig.ColumnJson.JsonStringToList<SystemAgileFindFormColumnsJsonOutput>();
            var format = new List<SystemDataBaseFindPagingBusinessDataTableColunmsInput>();

            var dataBase = await GetSystemDataBase(new IdInput { Id = agileConfig.DataBaseId });
            string connectionType = dataBase.ConnectionType;
            string connectionString = dataBase.ConnectionString;
            //值对应的用户Id集合
            List<SystemUserHeaderOutput> user = new List<SystemUserHeaderOutput>();
            using (var db = GetConnectoin(connectionString, connectionType))
            {
                string sql = $"SELECT * FROM {agileConfig.DataFromName} WHERE RelationId='{input.RelationId}'";
                var data = (await db.QueryAsync(sql)).ToList();
                //查询关联表
                var batchs = columns.Where(w => w.Type.ToUpper() == "BATCH");
                List<dynamic> tmp = new List<dynamic>();
                var dic = new Dictionary<string, object>();
                foreach (KeyValuePair<string, object> col in data.FirstOrDefault())
                {
                    dic.Add(col.Key, col.Value);
                }
                foreach (var item in batchs)
                {
                    var tableBatchName = agileConfig.DataFromName + "_" + item.Model;
                    string sqlBatch = $"SELECT * FROM {tableBatchName} WHERE RelationId='{input.RelationId}'";
                    var dataBatch = (await db.QueryAsync(sqlBatch)).ToList();

                    if (dataBatch.Any())
                    {
                        var datas = new List<Dictionary<string, object>>();
                        foreach (var dBatch in dataBatch)
                        {
                            var dics = new Dictionary<string, object>();
                            foreach (KeyValuePair<string, object> col in dBatch)
                            {
                                dics.Add(col.Key, col.Value);
                            }
                            datas.Add(dics);
                        }
                        dic.Add(item.Model, datas);
                    }
                }
                return OperateStatus<Dictionary<string, object>>.Success(dic);
            }
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> SaveBusinessData(SystemDataBaseSaveBusinessDataInput input)
        {
            OperateStatus<Guid> operateStatus = new OperateStatus<Guid>();
            string symbol = RepositoryUtil.GetSymbol();
            //所有涉及到的表名称
            List<string> tables = new List<string>();

            //是否具有数据
            bool haveData = false;
            //若是从流程中来则将流程实例Id赋予
            if (input.ProcessInstanceId.HasValue)
            {
                input.RelationId = (Guid)input.ProcessInstanceId;
            }
            if (input.ControlsString.IsNotNullOrEmpty())
            {
                input.Columns = input.ControlsString.JsonStringToList<SystemDataBaseSaveBusinessDataColumns>();
            }
            var form = (await _agileConfigLogic.FindPublicJsonById(new IdInput { Id = input.ConfigId })).Data;
            try
            {
                //主表所有字段
                //得到配置数据库连接信息
                var dataBase = await GetSystemDataBase(new IdInput { Id = form.DataBaseId });
                string connectionType = dataBase.ConnectionType;
                string connectionString = dataBase.ConnectionString;
                var allColumns = (await _dataBaseRepository.FindDataBaseColumns(new SystemDataBaseTableDto
                {
                    ConnectionString = dataBase.ConnectionString,
                    ConnectionType = dataBase.ConnectionType,
                    Name = form.DataFromName
                })).ToList();
                IList<SystemDataBaseSaveBusinessDataColumns> columns = new List<SystemDataBaseSaveBusinessDataColumns>();
                //插入
                if (input.Columns != null)
                {
                    foreach (var column in input.Columns)
                    {
                        if (column.Name.IsNotNullOrEmpty())
                        {
                            //字表
                            if (column.Type.ToUpper() == "BATCH")
                            {
                                columns.Add(column);
                            }
                            else
                            {
                                var colums = allColumns.ToList().Any(w => w.Name.ToUpper() == column.Name.ToUpper());
                                if (colums)
                                    columns.Add(column);
                            }
                        }
                    }
                }

                //参数
                var parameters = new Dictionary<string, object>();
                //执行Sql
                IList<string> doSqls = new List<string>();
                IList<Guid> detailIdList = new List<Guid>();
                IList<DataBaseBatchIdsOutput> batchIds = new List<DataBaseBatchIdsOutput>();
                var selectSql = $"SELECT * FROM {form.DataFromName} WHERE RelationId='{input.RelationId}'";
                tables.Add(form.DataFromName);
                DataTable createDataTable = new DbHelper(connectionString, connectionType).CreateSqlDataTable(selectSql);
                //表数据
                haveData = createDataTable.Rows.Count > 0;

                //如果没有值并且不是从工作流来
                if (!haveData && input.ProcessInstanceId.IsNullOrEmpty() && input.RelationId.IsNullOrEmpty())
                {
                    input.RelationId = CombUtil.NewComb();
                }
                parameters.Add("RelationId", input.RelationId);
                //新增
                if (!haveData)
                {
                    StringBuilder stringBuilder = new StringBuilder();
                    StringBuilder stringBuilderValues = new StringBuilder();
                    stringBuilder.Append($"INSERT INTO {form.DataFromName} (");
                    if (input.Columns != null)
                    {
                        #region 主表

                        //主表
                        var insertControls = columns.Where(item => item.Name.ToUpper() != "RELATIONID"
                                                                                                 && !item.Value.IsNullOrEmpty()
                                                                                                 && !_noCreateField.Contains(item.Type.ToLower()));

                        //插入字段
                        foreach (var item in insertControls)
                        {
                            stringBuilder.Append($"{item.Name},");
                            //过滤Xss攻击
                            item.Value = _filterSqlField.Any(f => f == item.Type) ? item.Value.Xss() : item.Value;
                            if (item.Type == "switch")
                            {
                                parameters.Add($"{item.Name}", Convert.ToInt32(item.Value));
                            }
                            else
                            {
                                parameters.Add($"{item.Name}", item.Value);
                            }

                            stringBuilderValues.Append($"{symbol}{item.Name},");
                        }

                        //主表附表,如下拉框等
                        var parTable = columns.Where(item => item.Name.ToUpper() != "RELATIONID"
                                                             && !item.Value.IsNullOrEmpty()
                                                             && !_noCreateField.Contains(item.Type.ToLower())
                                                             && _multipleField.Contains(item.Type.ToLower())
                                                             && !item.IsSingle).ToList();
                        foreach (var item in parTable)
                        {
                            var tableName = form.DataFromName + "_" + item.Name;
                            tables.Add(form.DataFromName + item.Name);
                            if (IsTableExist(connectionString, connectionType, tableName).Data)
                            {
                                //得到Value和Id集合
                                var ids = item.Value.Split(",");
                                var values = columns
                                    .FirstOrDefault(f => f.Name == item.Name + relationTxt)
                                    ?.Value.Split(",");
                                for (int i = 0; i < values.Length; i++)
                                {
                                    string collSql =
                                        ($@"INSERT INTO {tableName}
                                                        (RelationId
                                                        ,CorrelationId
                                                        ,Value)
                                                        VALUES
                                                        ({symbol}RelationId
                                                        ,{symbol}{tableName}{i}CorrelationId
                                                        ,{symbol}{tableName}{i}Value)");
                                    //参数
                                    parameters.Add($"{tableName}{i}CorrelationId", ids[i]);
                                    parameters.Add($"{tableName}{i}Value", ids[i]);
                                    doSqls.Add(collSql);
                                }
                            }
                        }
                        #endregion

                        #region 子表

                        var batchControls = columns.Where(w => w.Type.ToUpper() == "BATCH");
                        foreach (var item in batchControls)
                        {
                            if (item.Value.IsNotNullOrEmpty())
                            {
                                //序列化得到子表参数
                                var subtable = HttpUtility.UrlDecode(item.Value).JsonStringToList<DataBaseBatchControlDetailOutput>();
                                var batchTableName = form.DataFromName + "_" + item.Name;
                                tables.Add(form.DataFromName + item.Name);
                                for (int i = 0; i < subtable.Count; i++)
                                {
                                    var value = subtable[i];
                                    var subtableBuilder = new StringBuilder();
                                    var subtableBuilderValues = new StringBuilder();
                                    var detailId = CombUtil.NewComb();
                                    subtableBuilder.Append($"INSERT INTO {batchTableName} (");
                                    subtableBuilder.Append("RelationId,");//外键
                                    subtableBuilderValues.Append($"{symbol}RelationId,");

                                    foreach (var detail in value.Detail)
                                    {
                                        if (detail.Name.IsNotNullOrEmpty() && detail.Value.IsNotNullOrEmpty() && detail.Name != "BatchId")
                                        {
                                            subtableBuilder.Append("" + detail.Name + ",");
                                            subtableBuilderValues.Append($"{symbol}{batchTableName}{i}{detail.Name},");
                                            if (detail.Type == "switch")
                                            {
                                                parameters.Add($"{batchTableName}{i}{detail.Name}", Convert.ToInt32(detail.Value));
                                            }
                                            else
                                            {
                                                parameters.Add($"{batchTableName}{i}{detail.Name}", detail.Value);
                                            }
                                        }
                                        if (detail.Name == "BatchId")
                                        {
                                            subtableBuilder.Append("BatchId,");

                                            var detailValue = detail.Value.IsNotNullOrEmpty() ? detail.Value : detailId.ToString();
                                            var key = $"{batchTableName}{i}BatchId";
                                            subtableBuilderValues.Append($"{symbol}{key},");
                                            parameters.Add($"{key}", detailValue);

                                            detailIdList.Add(detail.Value.IsNotNullOrEmpty() ? Guid.Parse(detail.Value) : detailId);
                                        }
                                    }
                                    string batchsql = subtableBuilder.ToString().TrimEnd(',') + " ) VALUES (" + subtableBuilderValues.ToString().TrimEnd(',') + ")";
                                    if (batchsql.IsNotNullOrEmpty())
                                    {
                                        doSqls.Add(batchsql);
                                    }
                                }
                            }
                        }

                        var batchParTable = (columns.Where(item => item.Type.ToUpper() == "BATCH" && !item.Value.IsNullOrEmpty())).ToList();
                        foreach (var item in batchParTable)
                        {
                            //序列化得到子表参数
                            var subtable = HttpUtility.UrlDecode(item.Value).JsonStringToList<DataBaseBatchControlDetailOutput>();
                            for (int i = 0; i < subtable.Count; i++)
                            {
                                var keyvalue = subtable[i];
                                foreach (var detail in keyvalue.Detail)
                                {
                                    if (!detail.Value.IsNullOrEmpty()
                                        && !_noCreateField.Contains(detail.Type.ToLower())
                                        && _multipleField.Contains(detail.Type.ToLower())
                                        && !detail.IsSingle)
                                    {
                                        var tableName = form.DataFromName + "_" + item.Name + "_" + detail.Name;
                                        tables.Add(form.DataFromName + item.Name + detail.Name);
                                        if (IsTableExist(connectionString, connectionType, tableName).Data)
                                        {
                                            //得到Value和Id集合
                                            var ids = detail.Value.Split(",");
                                            var values = keyvalue.Detail.FirstOrDefault(f => f.Name == detail.Name + relationTxt)?.Value.Split(",");
                                            for (int j = 0; j < values.Length; j++)
                                            {
                                                string batchsql = ($@"INSERT INTO {tableName}
                                                                                    (BatchId
                                                                                    ,RelationId
                                                                                    ,CorrelationId
                                                                                    ,Value)
                                                                                    VALUES
                                                                                    ({symbol}{tableName}{j}BatchId
                                                                                    ,{symbol}RelationId
                                                                                    ,{symbol}{tableName}{j}CorrelationId
                                                                                    ,{symbol}{tableName}{j}Value)");

                                                parameters.Add($"{tableName}{j}BatchId", detailIdList[i]);
                                                parameters.Add($"{tableName}{j}CorrelationId", ids[j]);
                                                parameters.Add($"{tableName}{j}Value", values[j]);

                                                doSqls.Add(batchsql);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        #endregion
                    }

                    //主键
                    stringBuilder.Append("RelationId,");
                    stringBuilderValues.Append($"{symbol}RelationId,");

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
                    stringBuilderValues.Append($"0,");
                    //拼接Sql
                    var sql = stringBuilder.ToString().TrimEnd(',') + " ) VALUES (" +
                           stringBuilderValues.ToString().TrimEnd(',') + ")";

                    doSqls.Add(sql);
                }
                //修改
                else
                {
                    List<SystemAgileDataLogContent> systemAgileDataLogContents = new List<SystemAgileDataLogContent>();
                    StringBuilder stringBuilder = new StringBuilder();
                    stringBuilder.Append($"UPDATE {form.DataFromName} SET ");
                    if (input.Columns != null)
                    {
                        #region 主表
                        var updateControls = columns.Where(item => item.Name.ToUpper() != "RELATIONID"
                                                                    && item.Name.ToUpper() != "STATUS"
                                                                    && item.Type.ToUpper() != "IMAGESHOW"
                                                                    && !_noCreateField.Contains(item.Type.ToLower()));
                        foreach (var item in updateControls)
                        {
                            item.Value = _filterSqlField.Any(f => f == item.Type) ? item.Value.Xss() : item.Value;
                            stringBuilder.Append($"{item.Name}={symbol}{item.Name},");
                            if (item.Type == "switch")
                            {
                                parameters.Add($"{item.Name}", Convert.ToInt32(item.Value));
                            }
                            else
                            {
                                parameters.Add($"{item.Name}", item.Value);
                            }
                        }
                        //主表附表
                        var parTable = columns.Where(item =>
                              item.IsDelete
                           && item.Name.ToUpper() != "RELATIONID"
                           && !item.Value.IsNullOrEmpty()
                           && item.Name.ToUpper() != "STATUS"
                           && !_noCreateField.Contains(item.Type.ToLower())
                           && _multipleField.Contains(item.Type.ToLower())).ToList();
                        foreach (var item in parTable)
                        {
                            var tableName = form.DataFromName + "_" + item.Name;
                            tables.Add(form.DataFromName + item.Name);
                            if (IsTableExist(connectionString, connectionType, tableName).Data)
                            {
                                //删除
                                string delete = $"DELETE FROM {tableName} WHERE RelationId ={symbol}RelationId";
                                doSqls.Add(delete);
                                //删除附表值,再进行新增
                                if (!item.IsSingle)
                                {
                                    //得到Value和Id集合
                                    var ids = item.Value.Split(",");
                                    var values = columns.FirstOrDefault(f => f.Name == item.Name + relationTxt)?.Value.Split(",");
                                    for (int i = 0; i < values.Length; i++)
                                    {
                                        string collSql = ($@"INSERT INTO {tableName}
                                                        (RelationId
                                                        ,CorrelationId
                                                        ,Value )
                                                        VALUES
                                                        ({symbol}RelationId
                                                        ,{symbol}{tableName}{i}CorrelationId
                                                        ,{symbol}{tableName}{i}Value)");

                                        parameters.Add($"{tableName}{i}CorrelationId", ids[i]);
                                        parameters.Add($"{tableName}{i}Value", values[i]);

                                        doSqls.Add(collSql);
                                    }
                                }
                            }
                        }
                        #endregion

                        #region 子表

                        var updateBatchControls = columns.Where(item => item.IsDelete && item.Type.ToUpper() == "BATCH" && !item.Value.IsNullOrEmpty());

                        foreach (var item in updateBatchControls)
                        {
                            //需要新增控件
                            var insertDataBaseBatchControl = new List<DataBaseBatchControlDetailOutput>();

                            //序列化得到子表参数
                            var subtable = HttpUtility.UrlDecode(item.Value).JsonStringToList<DataBaseBatchControlDetailOutput>();
                            var batchTableName = form.DataFromName + "_" + item.Name;
                            tables.Add(form.DataFromName + item.Name);
                            //查询
                            string subselect = $"SELECT BatchId FROM {batchTableName} WHERE RelationId ='{input.RelationId}' ";
                            var subdt = new DbHelper(connectionString, connectionType).CreateSqlDataTable(subselect);
                            if (subdt.Rows.Count > 0)
                            {
                                foreach (DataRow row in subdt.Rows)
                                {
                                    batchIds.Add(new DataBaseBatchIdsOutput
                                    {
                                        BatchId = (Guid)row["BatchId"],
                                        RelationId = input.RelationId,
                                    });
                                }
                            }

                            doSqls.Add($"DELETE FROM {batchTableName} WHERE RelationId ='{input.RelationId}' ");

                            //新增数据
                            for (int i = 0; i < subtable.Count(); i++)
                            {
                                var keyvalue = subtable[i];

                                var subtableBuilder = new StringBuilder();
                                var subtableBuilderValues = new StringBuilder();
                                var detailId = CombUtil.NewComb();
                                subtableBuilder.Append($"INSERT INTO {batchTableName} (");
                                subtableBuilder.Append("RelationId,");
                                subtableBuilderValues.Append($"{symbol}RelationId,");
                                foreach (var detail in keyvalue.Detail)
                                {
                                    if (detail.Name.IsNotNullOrEmpty() && detail.Value.IsNotNullOrEmpty() && detail.Name != "BatchId")
                                    {
                                        subtableBuilder.Append("" + detail.Name + ",");
                                        subtableBuilderValues.Append($"{symbol}{batchTableName}{i}{detail.Name},");

                                        if (detail.Type == "switch")
                                        {
                                            parameters.Add($"{batchTableName}{i}{detail.Name}", Convert.ToInt32(detail.Value));
                                        }
                                        else
                                        {
                                            parameters.Add($"{batchTableName}{i}{detail.Name}", detail.Value);
                                        }
                                    }
                                    if (detail.Name == "BatchId")
                                    {
                                        subtableBuilder.Append("BatchId,");
                                        subtableBuilderValues.Append($"{symbol}{batchTableName}{i}BatchId,");
                                        parameters.Add($"{batchTableName}{i}BatchId", detail.Value.IsNotNullOrEmpty() ? detail.Value : detailId.ToString());

                                        detailIdList.Add(detail.Value.IsNotNullOrEmpty() ? Guid.Parse(detail.Value) : detailId);
                                    }
                                }

                                string batchsql =
                                    subtableBuilder.ToString().TrimEnd(',') + " ) VALUES (" +
                                    subtableBuilderValues.ToString().TrimEnd(',') + ")";
                                if (batchsql.IsNotNullOrEmpty())
                                {
                                    doSqls.Add(batchsql);
                                }
                            }
                        }

                        //子表附表
                        var subParTable = columns.Where(item => item.IsDelete
                            && item.Type.ToUpper() == "BATCH" && !item.Value.IsNullOrEmpty()).ToList();
                        foreach (var item in subParTable)
                        {
                            //序列化得到子表参数
                            var subtable = HttpUtility.UrlDecode(item.Value).JsonStringToList<DataBaseBatchControlDetailOutput>();
                            for (int i = 0; i < subtable.Count; i++)
                            {
                                var keyvalue = subtable[i];
                                for (int j = 0; j < keyvalue.Detail.Count; j++)
                                {
                                    var detail = keyvalue.Detail[j];
                                    if (!detail.Value.IsNullOrEmpty()
                                        && !_noCreateField.Contains(detail.Type.ToLower())
                                        && _multipleField.Contains(detail.Type.ToLower())
                                        && !detail.IsSingle)
                                    {
                                        var tableName = form.DataFromName + "_" + item.Name + "_" + detail.Name;
                                        tables.Add(form.DataFromName + item.Name + detail.Name);
                                        if (IsTableExist(connectionString, connectionType, tableName).Data)
                                        {
                                            //删除
                                            string delete = $"DELETE FROM {tableName} WHERE RelationId='{batchIds[i].RelationId}' and BatchId='{batchIds[i].BatchId}'";
                                            doSqls.Add(delete);

                                            //得到Value和Id集合
                                            var values = detail.Value.Split(",");
                                            var ids = keyvalue.Detail.FirstOrDefault(f => f.Name == detail.Name + relationTxt)?.Value.Split(",");
                                            for (int k = 0; k < values.Length; k++)
                                            {
                                                string batchsql = ($@"INSERT INTO {tableName}
                                                                                    (BatchId
                                                                                    ,RelationId
                                                                                    ,CorrelationId
                                                                                    ,Value)
                                                                                    VALUES
                                                                                    ({symbol}{tableName}{k}BatchId
                                                                                    ,{symbol}RelationId
                                                                                    ,{symbol}{tableName}{k}CorrelationId
                                                                                    ,{symbol}{tableName}{k}RelationId)");

                                                parameters.Add($"{tableName}{k}BatchId", detailIdList[i]);
                                                parameters.Add($"{tableName}{k}CorrelationId", ids[k]);
                                                parameters.Add($"{tableName}{k}RelationId", values[k]);
                                                doSqls.Add(batchsql);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        #endregion
                    }

                    stringBuilder.Append($"UpdateTime={symbol}UpdateTime,");
                    stringBuilder.Append($"UpdateUserId={symbol}UpdateUserId,");
                    stringBuilder.Append($"UpdateUserName={symbol}UpdateUserName,");
                    stringBuilder.Append($"UpdateOrganizationId={symbol}UpdateOrganizationId,");
                    stringBuilder.Append($"UpdateOrganizationName={symbol}UpdateOrganizationName,");

                    var sql = stringBuilder.ToString().TrimEnd(',') + $" WHERE RelationId={symbol}RelationId ";
                    doSqls.Add(sql);
                }
                parameters.Add($"CreateTime", DateTime.Now);
                parameters.Add($"CreateUserId", input.UserId);
                parameters.Add($"CreateUserName", input.UserName);
                parameters.Add($"CreateOrganizationId", input.OrganizationId);
                parameters.Add($"CreateOrganizationName", input.OrganizationName);
                parameters.Add($"UpdateTime", DateTime.Now);
                parameters.Add($"UpdateUserId", input.UserId);
                parameters.Add($"UpdateUserName", input.UserName);
                parameters.Add($"UpdateOrganizationId", input.OrganizationId);
                parameters.Add($"UpdateOrganizationName", input.OrganizationName);

                if (doSqls.Any())
                {
                    SystemAgileDataLog systemAgileDataLog = new SystemAgileDataLog();
                    using (IDbConnection connection = GetConnectoin(connectionString, connectionType))
                    {
                        systemAgileDataLog = new SystemAgileDataLog()
                        {
                            Type = EnumAgileDataLogType.新增.ToShort(),
                            AgileDataLogId = CombUtil.NewComb(),
                            CreateUserId = input.UserId,
                            CreateUserName = input.UserName,
                            CreateTime = DateTime.Now,
                            RelationId = input.RelationId,
                            ConfigId = input.ConfigId
                        };

                        if (haveData)
                        {
                            //查询对应表数据
                            List<string> sqls = new List<string>();
                            Dictionary<string, string> logTables = new Dictionary<string, string>();
                            logTables.Add(form.DataFromName, form.Remark);
                            foreach (var item in columns.Where(w => w.Type.ToUpper() == "BATCH"))
                            {
                                var batchTableName = form.DataFromName + "_" + item.Name;
                                logTables.Add(batchTableName, item.Description);
                            }
                            foreach (var item in logTables)
                            {
                                sqls.Add($"SELECT * FROM {item.Key} WHERE RelationId='{input.RelationId}'");
                            }
                            var queryData = connection.QueryMultiple(sqls.ExpandAndToString(";"));
                            List<SystemAgileDataLogValue> systemAgileDataLogValues = new List<SystemAgileDataLogValue>();
                            foreach (var item in logTables)
                            {
                                var data = queryData.Read<object>().ToList();
                                systemAgileDataLogValues.Add(new SystemAgileDataLogValue
                                {
                                    TableName = item.Key,
                                    Description = item.Value,
                                    Data = data
                                });
                            }
                            systemAgileDataLog.Type = EnumAgileDataLogType.编辑.ToShort();
                            systemAgileDataLog.Before = JsonConvert.SerializeObject(systemAgileDataLogValues);
                        }

                        var trans = connection.BeginTransaction();
                        try
                        {
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
                            operateStatus.Data = input.RelationId;

                            #region 编码
                            //对单号进行清除操作
                            var rows = input.Columns.Where(a => a.Type == "serialNo");
                            if (rows != null && rows.Any() && !haveData)
                            {
                                foreach (var row in rows)
                                {
                                    if (!row.LoadDisplay)
                                    {
                                        var noCreate = await _systemSerialNoLogic.Create(new SystemSerialCreateInput
                                        {
                                            Columns = input.Columns,
                                            Rule = row.Rule,
                                            ConfigId = input.ConfigId,
                                            LoadDisplay = row.LoadDisplay,
                                            Model = row.Name,
                                            UserOccupation = row.UserOccupation
                                        });
                                        //更新值
                                        if (noCreate.Code == ResultCode.Success)
                                        {
                                            var column = columns.FirstOrDefault(f => f.Name == row.Name);
                                            column.Value = noCreate.Data;
                                            //更新
                                            var noUpdate = new StringBuilder();
                                            noUpdate.Append($"UPDATE {form.DataFromName} SET {row.Name}='{noCreate.Data}'  WHERE RelationId={symbol}RelationId");
                                            connection.Execute(noUpdate.ToString(), parameters);
                                        }
                                    }
                                    if (row.UserOccupation)
                                    {
                                        //更新用户编码
                                        _systemSerialNoLogic.Clear(input.ConfigId, row.Name);
                                    }
                                }
                            }
                            #endregion

                            #region 条码,二维码
                            rows = input.Columns.Where(a => a.Type == "dataShow" && a.DefaultValue.IsNotNullOrEmpty()).ToList();
                            if (rows != null && rows.Any() && !haveData)
                            {
                                foreach (var row in rows)
                                {
                                    string content = row.DefaultValue.ReplaceEditor();
                                    var h1Elements = row.DefaultValue.GetNodes();
                                    if (h1Elements != null)
                                    {
                                        foreach (var element in h1Elements)
                                        {
                                            //解码
                                            var columnSetting = HttpUtility.UrlDecode(element.Id);
                                            if (columnSetting != null)
                                            {
                                                string value = "";
                                                //得到对应节点值
                                                switch (columnSetting)
                                                {
                                                    case "RelationId":
                                                        value = input.RelationId.ToString();
                                                        break;
                                                    case "CreateTime":
                                                        value = DateTime.Now.ToString();
                                                        break;
                                                    case "CreateUserId":
                                                        value = input.UserId.ToString();
                                                        break;
                                                    case "CreateUserName":
                                                        value = input.UserName.ToString();
                                                        break;
                                                    case "CreateOrganizationId":
                                                        value = input.OrganizationId.ToString();
                                                        break;
                                                    case "CreateOrganizationName":
                                                        value = input.OrganizationName.ToString();
                                                        break;
                                                    default:
                                                        var column = columns.FirstOrDefault(f => f.Name == columnSetting);
                                                        value = column?.Value;
                                                        break;
                                                }
                                                //得到值
                                                content = content.Replace(element.OuterHtml, value);
                                            }
                                        }
                                    }

                                    //更新
                                    var noUpdate = new StringBuilder();
                                    noUpdate.Append($"UPDATE {form.DataFromName} SET {row.Name}='{content}'  WHERE RelationId={symbol}RelationId");
                                    connection.Execute(noUpdate.ToString(), parameters);
                                }
                            }
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            operateStatus.Msg = "操作失败:" + ex.Message;
                            operateStatus.Code = ResultCode.Error;
                            return operateStatus;
                        }
                    }
                    await _publisher.PublishAsync("AgileDataLogSaveSubscribe", systemAgileDataLog);
                }
                else
                {
                    operateStatus.Code = ResultCode.Success;
                }
            }
            catch (Exception e)
            {
                operateStatus.Msg = "操作失败:" + e.Message;
                operateStatus.Code = ResultCode.Error;
                return operateStatus;
            }

            //删除缓存
            if (tables.Any())
            {
                var caches = new List<string>();
                tables = tables.DistinctByExtension(d => d).ToList();
                foreach (var item in tables)
                {
                    StringBuilder cacheString = new StringBuilder();
                    foreach (var name in item.Split('_').Where(w => w != "_"))
                    {
                        cacheString.Append(char.ToUpper(name[0]) + name.Substring(1));
                    }
                    var cache = $"I" + cacheString.ToString() + "Logic_Cache:*";
                    var keys = SearchRedisKeys(cache);
                    if (keys.Any())
                    {
                        RedisHelper.Del(keys.ToArray());
                    }
                }
            }

            return operateStatus;
        }
        /// <summary>
        /// Searchs the redis keys.
        /// </summary>
        /// <returns>The redis keys.</returns>
        /// <param name="pattern">Pattern.</param>
        private string[] SearchRedisKeys(string pattern)
        {
            var keys = new List<string>();

            long nextCursor = 0;
            do
            {
                var scanResult = RedisHelper.Scan(nextCursor, pattern, 500);
                nextCursor = scanResult.Cursor;
                var items = scanResult.Items;
                keys.AddRange(items);
            }
            while (nextCursor != 0);

            var prefix = RedisHelper.Nodes?.Values?.FirstOrDefault()?.Prefix;

            if (!string.IsNullOrWhiteSpace(prefix))
                keys = keys.Select(x => x.Remove(0, prefix.Length)).ToList();

            return keys.Distinct().ToArray();
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
        /// 查询业务数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<dynamic>> FindBusinessData(SystemDataBaseFindPagingBusinessDataInput input)
        {
            //获取模板数据
            var operateStatus = new OperateStatus<dynamic>();
            try
            {
                var codeGeneration = (await _agileConfigLogic.FindPublicJsonById(new IdInput { Id = input.ConfigId }, true)).Data;
                if (codeGeneration.DataFromName.IsNullOrEmpty())
                {
                    operateStatus.Msg = "请配置数据源";
                    return operateStatus;
                }
                var masks = new List<SystemDataBaseFindPagingBusinessDataTableColunmsInput>();
                var format = new List<SystemDataBaseFindPagingBusinessDataTableColunmsInput>();
                List<dynamic> data = new List<dynamic>();
                dynamic v = new ExpandoObject();
                if (codeGeneration.DataFrom == EnumDataFrom.Api.ToShort())
                {
                    Dictionary<string, string> headers = new Dictionary<string, string>();
                    headers.Add("Authorization", input.Header);
                    var apiObj = JsonConvert.DeserializeObject<SystemDataBaseTableApiDto>(codeGeneration.DataFromName);
                    var result = await RequestUtil.HttpPost(apiObj.Url.Trim(), JsonConvert.SerializeObject(input), headers, "application/x-www-form-urlencoded");
                    if (apiObj.Paging)
                    {
                        var resultPaging = JsonConvert.DeserializeObject<OperateStatus<PagedResults<dynamic>>>(result);
                        if (resultPaging.Code != ResultCode.Success)
                        {
                            return OperateStatus<dynamic>.Error(resultPaging.Msg);
                        }
                        data = resultPaging.Data.Data.ToList();
                        v.count = 0;
                        if (resultPaging.Data.PagerInfo.RecordCount > 0)
                        {
                            v.count = resultPaging.Data.PagerInfo.RecordCount;
                        }
                        else if (resultPaging.Data.PagerInfo.Total > 0)
                        {
                            v.count = resultPaging.Data.PagerInfo.RecordCount;
                        }
                    }
                    else
                    {
                        var resultNoPaging = JsonConvert.DeserializeObject<OperateStatus<List<dynamic>>>(result);
                        if (resultNoPaging.Code != ResultCode.Success)
                        {
                            return OperateStatus<dynamic>.Error(resultNoPaging.Msg);
                        }
                        data = resultNoPaging.Data.ToList();
                        v.count = resultNoPaging.Data.ToList();
                    }
                    v.data = data;
                }
                else
                {
                    SqlDatabaseFixtureInput sqlDatabaseFixtureInput = new SqlDatabaseFixtureInput();
                    var dataBase = await GetSystemDataBase(new IdInput
                    {
                        Id = codeGeneration.DataBaseId
                    });
                    sqlDatabaseFixtureInput.ConnectionType = dataBase.ConnectionType;
                    sqlDatabaseFixtureInput.ConnectionString = dataBase.ConnectionString;

                    if (input.HaveDataPermission)
                    {
                        var currentUser = EipHttpContext.CurrentUser();
                        input.Rote.UserId = currentUser.UserId;
                        input.DataSql = (await _permissionLogic.FindDataPermissionSql(input.Rote)).Data;
                    }
                    using (var fix = new SqlDatabaseFixture(sqlDatabaseFixtureInput))
                    {
                        //得到需要查询的字段
                        var tables = codeGeneration.PublicJson.JsonStringToObject<SystemDataBaseFindPagingBusinessDataTableInput>();
                        //掩码字段
                        masks = tables.Columns.Where(w => w.Mask.IsNotNullOrEmpty() && w.IsSearch).ToList();
                        format = tables.Columns.Where(w => w.Format.IsNotNullOrEmpty() && w.IsSearch).ToList();
                        if (codeGeneration.DataFrom == EnumDataFrom.存储过程.ToShort())
                        {
                            var parms = new DynamicParameters();
                            string sql = input.Sql +
                                            (input.Where.IsNotNullOrEmpty() ? " and " + input.Where : "") +
                                            (input.DataSql.IsNotNullOrEmpty() ? " and " + input.DataSql : "");
                            if (input.IsPaging)
                            {
                                StringBuilder sidx = new StringBuilder();
                                StringBuilder orderBy = new StringBuilder();
                                if (input.Sidx.IsNotNullOrEmpty())
                                {
                                    for (int i = 0; i < input.Sidx.Split(',').Length; i++)
                                    {
                                        sidx.Append($"{codeGeneration.DataFromName + "." + input.Sidx.Split(',')[i]},");
                                        orderBy.Append($"{codeGeneration.DataFromName + "." + input.Sidx.Split(',')[i]} {input.Sord.Split(',')[i]},");
                                    }
                                }
                                parms.Add("Sidx", sidx.ToString().TrimEnd(','));//排序字段
                                parms.Add("Field", input.Fields);
                                parms.Add("Filters", sql);
                                parms.Add("PageIndex", input.Current);
                                parms.Add("PageSize", input.Size);
                                parms.Add("GroupBy", input.Group.IsNullOrEmpty() ? "" : input.Group);
                                parms.Add("Sort", orderBy.ToString().TrimEnd(','));
                                parms.Add("RecordCount", dbType: DbType.Int32, direction: ParameterDirection.Output);
                                data = (await fix.Db.Connection.QueryAsync<dynamic>(codeGeneration.DataFromName, parms, commandType: CommandType.StoredProcedure)).ToList();
                                v.count = data.Any() ? parms.Get<int>("RecordCount") : 0;
                            }
                            else
                            {
                                data = (await fix.Db.Connection.QueryAsync<dynamic>(codeGeneration.DataFromName, parms, commandType: CommandType.StoredProcedure)).ToList();
                                v.count = data.Count;
                                return OperateStatus<dynamic>.Success(v);
                            }
                            v.data = data;
                        }
                        else
                        {
                            //判断是否分页
                            List<string> fields = new List<string>();

                            //得到需要查询的字段
                            var columns = tables.Columns.Where(w => w.IsSearch).ToList();
                            foreach (var item in columns)
                            {
                                if (item.Type != "batch")
                                {
                                    fields.Add(codeGeneration.DataFromName + "." + item.Name);
                                    if (item.Name.EndsWith(relationTxt) && !fields.Any(a => a == codeGeneration.DataFromName + "." + item.Name.Replace(relationTxt, "")))
                                    {
                                        fields.Add(codeGeneration.DataFromName + "." + item.Name.Replace(relationTxt, ""));
                                    }
                                }
                                else
                                {
                                    var batchName = codeGeneration.DataFromName + "_" + item.Name;
                                    fields.Add(($" (SELECT COUNT(1) from {batchName} where {batchName}.RelationId= {codeGeneration.DataFromName}.RelationId)") + $"{item.Name} ");
                                }
                            }

                            //是否为删除
                            if (input.IsDelete == 1)
                            {
                                if (!fields.Contains("UpdateTime"))
                                {
                                    fields.Add("UpdateTime");
                                }

                                if (!fields.Contains("UpdateUserName"))
                                {
                                    fields.Add("UpdateUserName");
                                    fields.Add("UpdateUserId");
                                }
                            }

                            input.Fields = !fields.Any() ? "*" : fields.ExpandAndToString();
                            string sql = input.Sql +
                                          (input.Where.IsNotNullOrEmpty() ? " and " + input.Where : "") +
                                          (input.DataSql.IsNotNullOrEmpty() ? " and " + input.DataSql : "");
                            StringBuilder orderBy = new StringBuilder();
                            if (input.Sidx.IsNotNullOrEmpty())
                            {
                                for (int i = 0; i < input.Sidx.Split(',').Length; i++)
                                {
                                    orderBy.Append($"{codeGeneration.DataFromName + "." + input.Sidx.Split(',')[i]} {input.Sord.Split(',')[i]},");
                                }
                            }
                            else
                            {
                                //若排序字段为空则取最后一个字段作为排序字段
                                if (columns.Any())
                                {
                                    orderBy.Append(columns[columns.Count() - 1].Name + " " + input.Sord);
                                }
                                else
                                {
                                    orderBy.Append(" Id desc ");
                                }
                            }

                            if (input.IsPaging)
                            {
                                var currentPage = input.Current; //当前页号
                                var pageSize = input.Size; //每页记录数
                                var lower = ((currentPage - 1) * pageSize) + 1; //记录起点
                                var upper = currentPage * pageSize; //记录终点

                                var querySql = $"select {input.Fields},@rowNumber, @recordCount from {codeGeneration.DataFromName} @where {(codeGeneration.EditConfigId.HasValue ? $"and isDelete={input.IsDelete}" : "")}  ";
                                var querySqlQueryMultiple = input.IsReport ?
                                                  $@"select * from ({querySql}) seq "
                                                  : $@"select * from ({querySql}) seq where seq.row_num between {lower} and {upper}";

                                var orderString = $" {orderBy.ToString().TrimEnd(',')}";
                                var recordCount = querySqlQueryMultiple.Split("@recordCount");
                                var selectSql = recordCount[0].Trim().TrimEnd(',');
                                querySqlQueryMultiple = selectSql + " " + recordCount[1];
                                querySqlQueryMultiple = querySqlQueryMultiple.Replace("@rowNumber", " row_number() over (order by @orderBy) as row_num ")
                                    .Replace("@orderBy", orderString)
                                    .Replace("@where", " WHERE 1=1 " + (string.IsNullOrWhiteSpace(sql) ? string.Empty : sql));

                                var querySqlRecordCount = querySql.Split("@recordCount");
                                var recordCountSql = querySql.Replace(querySqlRecordCount[0], ";select count(*) ").Replace("@recordCount", "  ").Replace("@where", " WHERE 1=1 " + (string.IsNullOrWhiteSpace(sql) ? string.Empty : sql));
                                //总数量
                                querySqlQueryMultiple += recordCountSql;
                                var queryMulti = fix.Db.Connection.QueryMultiple(querySqlQueryMultiple);
                                data = queryMulti.Read<dynamic>().ToList();
                                v.count = queryMulti.Read<long>().Sum();
                            }
                            else
                            {
                                var orderBySql = orderBy.ToString().TrimEnd(',');
                                if (orderBySql.Length > 0)
                                {
                                    orderBySql = "ORDER BY " + orderBySql;
                                }
                                string selectSql = $"SELECT {input.Fields} FROM {codeGeneration.DataFromName} where 1=1 {(codeGeneration.EditConfigId.HasValue ? $"and isDelete={input.IsDelete}" : "")} {sql} {orderBySql}";
                                data = (await fix.Db.Connection.QueryAsync<dynamic>(selectSql)).ToList();
                                v.count = data.ToList().Count;
                            }
                        }
                    }
                }

                //数据再次处理
                if (data.Any())
                {
                    if (input.Fields.Contains("CreateUserName"))
                    {
                        format.Add(new SystemDataBaseFindPagingBusinessDataTableColunmsInput
                        {
                            Format = "User",
                            Name = "CreateUserId"
                        });
                    }
                    if (input.Fields.Contains("UpdateUserName"))
                    {
                        format.Add(new SystemDataBaseFindPagingBusinessDataTableColunmsInput
                        {
                            Format = "User",
                            Name = "UpdateUserId"
                        });
                    }

                    //值对应的用户Id集合
                    List<string> userIds = new List<string>();
                    List<SystemUserHeaderOutput> user = new List<SystemUserHeaderOutput>();
                    if (format.Any(a => a.Format == "User"))
                    {
                        foreach (var item in data)
                        {
                            foreach (var dd in item)
                            {
                                string dkey = dd.Key;
                                var formatUsers = format.Any(a => a.Format == "User" && a.Name == ((dkey == "CreateUserId" || dkey == "UpdateUserId") ? dkey : dkey + "_Txt"));
                                if (formatUsers)
                                {
                                    object dvalue = dd.Value;
                                    if (dvalue != null)
                                    {
                                        var ids = dvalue.ToString().Split(',');
                                        foreach (var id in ids)
                                        {
                                            if (!userIds.Any(a => a == id))
                                            {
                                                userIds.Add(id);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        if (userIds.Any())
                        {
                            //查询用户头像
                            var sql = $"select HeadImage,UserId from system_userinfo where UserId in ({userIds.ExpandAndToString().InSql()})";
                            using (var fix = new SqlDatabaseFixture())
                            {
                                user = (await fix.Db.Connection.QueryAsync<SystemUserHeaderOutput>(sql)).ToList();
                            }
                        }
                    }

                    //是否具有需要替换的Id,或者需要掩码
                    if (userIds.Any() || masks.Any())
                    {
                        List<dynamic> tmp = new List<dynamic>();
                        foreach (var item in data)
                        {
                            dynamic info = new ExpandoObject();
                            var dic = (IDictionary<string, object>)info;
                            foreach (var dd in item)
                            {
                                string dkey = dd.Key;
                                object dvalue = dd.Value;

                                if (dvalue != null)
                                {
                                    //头像处理
                                    var formatUsers = format.Any(a => a.Format == "User" && a.Name == ((dkey == "CreateUserId" || dkey == "UpdateUserId") ? dkey : dkey + "_Txt"));
                                    if (formatUsers)
                                    {
                                        var userIdSplit = dvalue.ToString().Split(',');
                                        List<string> userIdHeader = new List<string>();
                                        foreach (var userId in userIdSplit)
                                        {
                                            userIdHeader.Add(user.FirstOrDefault(f => f.UserId.ToString() == userId)?.HeadImage);
                                        }
                                        dic.Add(((dkey == "CreateUserId" || dkey == "UpdateUserId") ? dkey : dkey + "_Txt") + "Header", userIdHeader.ExpandAndToString());
                                    }

                                    //掩码
                                    var haveMask = masks.FirstOrDefault(f => f.Name == dkey);
                                    if (dvalue != null && haveMask != null)
                                    {
                                        if (!string.IsNullOrWhiteSpace(dvalue.ToString()))
                                        {
                                            dvalue = Convert.ToString(dvalue).ConvertMask(haveMask.Mask);
                                        }
                                    }
                                }
                                dic.Add(dkey, dvalue);
                            }
                            tmp.Add(info);
                        }
                        data = tmp;
                    }
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
        /// 查询业务数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<List<SystemDataBaseFindBusinessDataFilterSearchOutput>>> FindBusinessDataFilterSearch(SystemDataBaseFindBusinessDataFilterSearchInput input)
        {
            var operateStatus = new OperateStatus<List<SystemDataBaseFindBusinessDataFilterSearchOutput>>();
            try
            {
                input.Field = input.Field.Replace("_Txt", "");
                var codeGeneration = (await _agileConfigLogic.FindPublicJsonById(new IdInput { Id = input.ConfigId }, true)).Data;
                if (codeGeneration.DataFromName.IsNullOrEmpty())
                {
                    operateStatus.Msg = "请配置数据源";
                    return operateStatus;
                }
                var masks = new List<SystemDataBaseFindPagingBusinessDataTableColunmsInput>();
                var format = new List<SystemDataBaseFindPagingBusinessDataTableColunmsInput>();
                List<SystemDataBaseFindBusinessDataFilterSearchOutput> data = new List<SystemDataBaseFindBusinessDataFilterSearchOutput>();
                SqlDatabaseFixtureInput sqlDatabaseFixtureInput = new SqlDatabaseFixtureInput();
                var dataBase = await GetSystemDataBase(new IdInput
                {
                    Id = codeGeneration.DataBaseId
                });
                sqlDatabaseFixtureInput.ConnectionType = dataBase.ConnectionType;
                sqlDatabaseFixtureInput.ConnectionString = dataBase.ConnectionString;

                if (input.HaveDataPermission)
                {
                    var currentUser = EipHttpContext.CurrentUser();
                    input.Rote.UserId = currentUser.UserId;
                    input.DataSql = (await _permissionLogic.FindDataPermissionSql(input.Rote)).Data;
                }
                using (var fix = new SqlDatabaseFixture(sqlDatabaseFixtureInput))
                {
                    var tables = codeGeneration.PublicJson.JsonStringToObject<SystemDataBaseFindPagingBusinessDataTableInput>();
                    string selectSql = $"SELECT {input.Field} Field,{input.Field + "_Txt"} Field_Txt,count({input.Field}) AS count FROM {codeGeneration.DataFromName} where 1=1 and isDelete=0 and {input.Field} is not null {input.DataSql} group by {input.Field},{input.Field + "_Txt"}";
                    selectSql += " union ";
                    selectSql += $" select '-1' type,'为空' type_txt,count(*) AS Count from {codeGeneration.DataFromName} where {input.Field} is  null and isDelete=0 ";
                    data = (await fix.Db.Connection.QueryAsync<SystemDataBaseFindBusinessDataFilterSearchOutput>(selectSql)).ToList();
                    operateStatus.Data = data;
                    operateStatus.Code = ResultCode.Success;
                    operateStatus.Msg = Chs.QuerySuccessful;
                }
                return operateStatus;
            }
            catch (Exception ex)
            {
                operateStatus.Msg = ex.Message;
            }
            return operateStatus;
        }

        /// <summary>
        /// 查询业务数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<dynamic>> FindBusinessDataPage(SystemDataBaseFindPagingBusinessDataInput input)
        {
            //获取模板数据
            var operateStatus = new OperateStatus<dynamic>();
            try
            {
                var codeGeneration = (await _agileConfigLogic.FindPublicJsonById(new IdInput { Id = input.ConfigId }, true)).Data;
                if (codeGeneration.DataFromName.IsNullOrEmpty())
                {
                    operateStatus.Msg = "请配置数据源";
                    return operateStatus;
                }
                var masks = new List<SystemDataBaseFindPagingBusinessDataTableColunmsInput>();
                var format = new List<SystemDataBaseFindPagingBusinessDataTableColunmsInput>();
                List<dynamic> data = new List<dynamic>();
                SqlDatabaseFixtureInput sqlDatabaseFixtureInput = new SqlDatabaseFixtureInput();
                var dataBase = await GetSystemDataBase(new IdInput
                {
                    Id = codeGeneration.DataBaseId
                });
                sqlDatabaseFixtureInput.ConnectionType = dataBase.ConnectionType;
                sqlDatabaseFixtureInput.ConnectionString = dataBase.ConnectionString;
                using (var fix = new SqlDatabaseFixture(sqlDatabaseFixtureInput))
                {
                    //得到需要查询的字段
                    string selectSql = $"SELECT * FROM {codeGeneration.DataFromName} ";
                    data = (await fix.Db.Connection.QueryAsync<dynamic>(selectSql)).ToList();
                }
                return OperateStatus<dynamic>.Success(data);
            }
            catch (Exception ex)
            {
                operateStatus.Msg = ex.Message;
            }
            return operateStatus;
        }

        /// <summary>
        /// 查询业务数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<dynamic>> FindBusinessDataFooter(SystemDataBaseFindPagingBusinessDataInput input)
        {
            //获取模板数据
            var operateStatus = new OperateStatus<dynamic>();
            try
            {
                if (input.HaveDataPermission)
                {
                    var currentUser = EipHttpContext.CurrentUser();
                    input.Rote.UserId = currentUser.UserId;
                    input.DataSql = (await _permissionLogic.FindDataPermissionSql(input.Rote)).Data;
                }
                using (var fix = new SqlDatabaseFixture())
                {
                    var codeGeneration = await fix.Db.SystemAgile.SetSelect(s => new { s.DataFromName, s.PublicJson }).FindAsync(f => f.ConfigId == input.ConfigId);
                    //得到需要查询的字段
                    var tables = codeGeneration.PublicJson.JsonStringToObject<SystemDataBaseFindPagingBusinessDataTableInput>();
                    //判断是否分页
                    StringBuilder fields = new StringBuilder();
                    //得到需要查询的字段
                    foreach (var item in tables.Columns.Where(w => w.IsSearch && w.IsTotal))
                    {
                        fields.Append("sum(" + codeGeneration.DataFromName + "." + item.Name + $"){item.Name},");
                    }
                    input.Fields = fields.ToString().TrimEnd(',');
                    string sql = input.Sql +
                                  (input.Where.IsNotNullOrEmpty() ? " and " + input.Where : "") +
                                  (input.DataSql.IsNotNullOrEmpty() ? " and " + input.DataSql : "");
                    string selectSql = $"SELECT {input.Fields} FROM {codeGeneration.DataFromName} where 1=1 {sql} ";
                    var data = (await fix.Db.Connection.QueryAsync<dynamic>(selectSql)).ToList();
                    return OperateStatus<dynamic>.Success(data);
                }
            }
            catch (Exception ex)
            {
                operateStatus.Msg = ex.Message;
            }
            return operateStatus;
        }

        /// <summary>
        /// 获取子表数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<dynamic>> FindBatchData(DataBaseSubTableDto input)
        {
            string connectionString = "", connectionType = "";
            List<SystemDataBaseColumnJsonInput> columnJsonInputs = new List<SystemDataBaseColumnJsonInput>();
            var tableName = "";
            using (var fix = new SqlDatabaseFixture())
            {
                var agileConfig = await fix.Db.SystemAgile.SetSelect(s => new { s.ColumnJson, s.DataFromName, s.DataBaseId }).FindAsync(f => f.ConfigId == input.ConfigId);
                if (agileConfig != null)
                {
                    tableName = agileConfig.DataFromName + "_" + input.Table;
                    var dataBase = fix.Db.SystemDataBase.SetSelect(s => new { s.ConnectionType, s.ConnectionString }).Find(f => f.DataBaseId == agileConfig.DataBaseId);
                    connectionType = dataBase.ConnectionType;
                    connectionString = ConfigurationUtil.GetSection(dataBase.ConnectionString);
                    columnJsonInputs = agileConfig.ColumnJson.JsonStringToList<SystemDataBaseColumnJsonInput>();
                }
            }
            List<string> userIds = new List<string>();
            List<SystemUserHeaderOutput> user = new List<SystemUserHeaderOutput>();
            using (var db = GetConnectoin(connectionString, connectionType))
            {
                string sql = $"SELECT * FROM {tableName} WHERE RelationId='{input.Id}' ";
                var data = await db.QueryAsync(sql);
                //判断子表属性
                var column = columnJsonInputs.FirstOrDefault(f => f.Model == input.Table && f.Type == "batch");

                if (column != null)
                {
                    var users = column.List.Where(w => w.Type == "user");
                    if (users.Any())
                    {
                        foreach (var item in data)
                        {
                            foreach (var dd in item)
                            {
                                string dkey = dd.Key;
                                var formatUsers = users.FirstOrDefault(f => f.Model == dkey);
                                if (formatUsers != null)
                                {
                                    object dvalue = dd.Value;
                                    if (dvalue != null)
                                    {
                                        var ids = dvalue.ToString().Split(',');
                                        foreach (var id in ids)
                                        {
                                            if (!userIds.Any(a => a == id))
                                            {
                                                userIds.Add(id);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                }
                if (userIds.Any())
                {
                    //查询用户头像
                    var sqlUsers = $"select HeadImage,UserId from system_userinfo where UserId in ({userIds.ExpandAndToString().InSql()})";
                    using (var fix = new SqlDatabaseFixture())
                    {
                        user = (await fix.Db.Connection.QueryAsync<SystemUserHeaderOutput>(sqlUsers)).ToList();
                    }
                    List<dynamic> tmp = new List<dynamic>();
                    foreach (var item in data)
                    {
                        dynamic info = new ExpandoObject();
                        var dic = (IDictionary<string, object>)info;
                        foreach (var dd in item)
                        {
                            string dkey = dd.Key;
                            object dvalue = dd.Value;

                            if (dvalue != null)
                            {
                                var formatUsers = column.List.FirstOrDefault(f => f.Model == dkey && f.Type == "user");
                                if (formatUsers != null)
                                {
                                    var userIdSplit = dvalue.ToString().Split(',');
                                    List<string> userIdHeader = new List<string>();
                                    foreach (var userId in userIdSplit)
                                    {
                                        userIdHeader.Add(user.FirstOrDefault(f => f.UserId.ToString() == userId)?.HeadImage);
                                    }
                                    dic.Add(((dkey == "CreateUserId" || dkey == "UpdateUserId") ? dkey : dkey + "_Txt") + "Header", userIdHeader.ExpandAndToString());
                                }
                            }
                            dic.Add(dkey, dvalue);
                        }
                        tmp.Add(info);
                    }
                    data = tmp;
                }

                return OperateStatus<object>.Success(data);
            }
        }

        /// <summary>
        /// 根据Sql查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<dynamic>> FindFormSourceData(SystemDataBaseFindFormSourceDataInput input)
        {
            OperateStatus<dynamic> operateStatus = new OperateStatus<dynamic>();
            try
            {
                var parameters = new Dictionary<string, object>();
                SystemDataSource systemDataSource = await GetSystemDataSource(new IdInput(input.DataSourceId));
                if (systemDataSource != null)
                {
                    var currentUser = EipHttpContext.CurrentUser();
                    var inParams = input.InParams.JsonStringToList<SystemDataBaseFindFormSourceDataInParamsInput>();
                    //赋予值：系统信息
                    JObject policyOrderResult = JObject.Parse(input.FormValue);
                    foreach (var item in inParams)
                    {
                        if (!item.Condition.Contains("'") && input.FormValue != null)
                        {
                            switch (item.Condition)
                            {
                                case "CreateUserId":
                                    item.Condition = currentUser.UserId.ToString();
                                    break;
                                case "CreateUserName":
                                    item.Condition = currentUser.Name.ToString();
                                    break;
                                case "CreateOrganizationId":
                                    item.Condition = currentUser.OrganizationId.ToString();
                                    break;
                                case "CreateOrganizationName":
                                    item.Condition = currentUser.OrganizationName.ToString();
                                    break;
                                default:
                                    var condition = policyOrderResult[item.Condition];
                                    if (!condition.IsNullOrEmpty())
                                    {
                                        item.Condition = condition.ToString();
                                    }
                                    else
                                    {
                                        item.Condition = "";
                                    }
                                    break;
                            }
                        }
                        parameters.Add(item.Field, item.Condition.Trim('\''));
                    }
                    var output = systemDataSource.Config.JsonStringToObject<SystemDataSourceConfigOutput>();
                    var sqlFilters = ConvertFilters(output.Filters);
                    if (output.DataMenuId.HasValue)
                    {
                        input.DataSql = (await _permissionLogic.FindDataPermissionSql(new ViewRote
                        {
                            UserId = currentUser.UserId,
                            MenuId = output.DataMenuId
                        })).Data;
                    }
                    if (output.Type == "table" || output.Type == "view")
                    {
                        var name = output.Type == "table" ? output.Table.Name : output.View.Name;
                        var outputParams = new List<string>();
                        foreach (var item in output.OutParams)
                        {
                            outputParams.Add($" {item.Field} as {(item.Alias.IsNotNullOrEmpty() ? item.Alias : item.Field)} ");
                        }
                        string sql = $"SELECT {outputParams.ExpandAndToString().ToString().TrimEnd(',')} FROM {name} WHERE 1=1 ";
                        if (sqlFilters.IsNotNullOrEmpty())
                        {
                            sql += " and " + sqlFilters;
                        }
                        if (input.DataSql.IsNotNullOrEmpty())
                        {
                            sql += " and " + input.DataSql;
                        }
                        if (input.Sidx.IsNotNullOrEmpty())
                        {
                            sql += " order by " + input.Sidx + " " + input.Sord;
                        }

                        var dataBase = await GetSystemDataBase(new IdInput
                        {
                            Id = output.DataBaseId
                        });
                        using (var db = GetConnectoin(dataBase.ConnectionString, dataBase.ConnectionType))
                        {
                            operateStatus.Data = await db.QueryAsync<object>(sql, parameters);
                            operateStatus.Code = ResultCode.Success;
                            operateStatus.Msg = Chs.QuerySuccessful;
                        }
                    }
                    else if (output.Type == "api")
                    {

                    }
                    else
                    {

                    }
                }
                else
                {

                }
                return operateStatus;
            }
            catch (Exception ex)
            {
                operateStatus.Msg = "读取失败:" + ex.Message;
                return operateStatus;
            }

        }

        /// <summary>
        /// 根据Sql查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<dynamic>> FindFormSourceDataPaging(SystemDataBaseFindFormSourceDataInput input)
        {
            OperateStatus<dynamic> operateStatus = new OperateStatus<dynamic>();
            try
            {
                SystemDataSource dataSource = await GetSystemDataSource(new IdInput(input.DataSourceId));
                if (dataSource != null)
                {
                    var currentUser = EipHttpContext.CurrentUser();
                    var parameters = new Dictionary<string, object>();
                    if (input.InParams.IsNotNullOrEmpty())
                    {
                        var inParams = input.InParams.JsonStringToList<SystemDataBaseFindFormSourceDataInParamsInput>();
                        foreach (var item in inParams)
                        {
                            if (!item.Condition.Contains("'") && input.FormValue != null)
                            {
                                //赋予值：系统信息
                                JObject policyOrderResult = JObject.Parse(input.FormValue);
                                switch (item.Condition)
                                {
                                    case "CreateUserId":
                                        item.Condition = currentUser.UserId.ToString();
                                        break;
                                    case "CreateUserName":
                                        item.Condition = currentUser.Name.ToString();
                                        break;
                                    case "CreateOrganizationId":
                                        item.Condition = currentUser.OrganizationId.ToString();
                                        break;
                                    case "CreateOrganizationName":
                                        item.Condition = currentUser.OrganizationName.ToString();
                                        break;
                                    default:
                                        var condition = policyOrderResult[item.Condition];
                                        if (!condition.IsNullOrEmpty())
                                        {
                                            item.Condition = condition.ToString();
                                        }
                                        break;
                                }
                            }
                            parameters.Add(item.Field, item.Condition.Trim('\''));
                        }
                    }
                    var output = dataSource.Config.JsonStringToObject<SystemDataSourceConfigOutput>();
                    if (output.DataMenuId.HasValue)
                    {
                        input.DataSql = (await _permissionLogic.FindDataPermissionSql(new ViewRote
                        {
                            UserId = currentUser.UserId,
                            MenuId = output.DataMenuId
                        })).Data;
                    }
                    var sqlFilters = ConvertFilters(output.Filters);
                    if (output.Type == "table" || output.Type == "view")
                    {
                        List<dynamic> data = new List<dynamic>();
                        var name = output.Type == "table" ? output.Table.Name : output.View.Name;
                        var outputParams = new List<string>();
                        foreach (var item in output.OutParams)
                        {
                            outputParams.Add($" {item.Field} as {(item.Alias.IsNotNullOrEmpty() ? item.Alias : item.Field)} ");
                        }
                        dynamic v = new ExpandoObject();

                        var dataBase = await GetSystemDataBase(new IdInput { Id = output.DataBaseId });
                        if (dataBase.ConnectionType == ResourceDataBaseType.Mysql)
                        {
                            var currentPage = input.Current; //当前页号
                            var pageSize = input.Size; //每页记录数
                            var lower = ((currentPage - 1) * pageSize) + 1; //记录起点
                            var upper = currentPage * pageSize; //记录终点
                            var querySql = $"SELECT {outputParams.ExpandAndToString().ToString().TrimEnd(',')},@rowNumber, @recordCount FROM {name} @where ";
                            if (sqlFilters.IsNotNullOrEmpty())
                            {
                                querySql += " and " + sqlFilters;
                            }
                            if (input.DataSql.IsNotNullOrEmpty())
                            {
                                querySql += " and " + input.DataSql;
                            }
                            //若为空则并且有查询字段则查询最后一个排序
                            if (input.Sidx.IsNullOrEmpty() && output.OutParams.Count > 0)
                            {
                                var item = output.OutParams[output.OutParams.Count() - 1];
                                var key = (item.Alias.IsNotNullOrEmpty() ? item.Alias : item.Field);
                                input.Sidx = key;
                            }
                            var sql = $@"select * from ({querySql} order by {input.Sidx} ) seq  limit {lower - 1},{upper}";
                            var recordCount = sql.Split("@recordCount");
                            var selectSql = recordCount[0].Trim().TrimEnd(',');
                            sql = selectSql + " " + recordCount[1];
                            sql = sql.Replace("@rowNumber", " 1 ")
                                .Replace("@orderBy", " 2 ")
                                .Replace("@where", " WHERE 1=1 " + (string.IsNullOrWhiteSpace(input.Sql) ? string.Empty : input.Sql));

                            var querySqlRecordCount = querySql.Split("@recordCount");
                            var recordCountSql = querySql.Replace(querySqlRecordCount[0], ";select count(*) ").Replace("@recordCount", "  ").Replace("@where", " WHERE 1=1 " + (string.IsNullOrWhiteSpace(input.Sql) ? string.Empty : input.Sql));
                            //总数量
                            sql += recordCountSql;

                            using (var db = GetConnectoin(dataBase.ConnectionString, dataBase.ConnectionType))
                            {
                                var queryMulti = await db.QueryMultipleAsync(sql, parameters);
                                data = queryMulti.Read<dynamic>().ToList();

                                v.page = input.Current;
                                v.count = queryMulti.Read<long>().Sum();
                                v.data = data;
                            }
                        }
                        else
                        {
                            var querySql = $"SELECT {outputParams.ExpandAndToString().ToString().TrimEnd(',')},@rowNumber, @recordCount  FROM {name} @where ";
                            if (input.Sql.IsNotNullOrEmpty())
                            {
                                querySql += input.Sql;
                            }
                            if (input.DataSql.IsNotNullOrEmpty())
                            {
                                querySql += " and " + input.DataSql;
                            }
                            var sql = input.IsReport ?
                                $@"select * from ({querySql}) seq "
                                : $@"select * from ({querySql}) seq where seq.rownum between @x and @y";
                            var currentPage = input.Current; //当前页号
                            var pageSize = input.Size; //每页记录数
                            var lower = ((currentPage - 1) * pageSize) + 1; //记录起点
                            var upper = currentPage * pageSize; //记录终点
                            parameters.Add("x", lower);
                            parameters.Add("y", upper);
                            //排序字段

                            //若为空则并且有查询字段则查询最后一个排序
                            if (input.Sidx.IsNullOrEmpty() && output.OutParams.Count > 0)
                            {
                                var item = output.OutParams[output.OutParams.Count() - 1];
                                var key = (item.Alias.IsNotNullOrEmpty() ? item.Alias : item.Field);
                                input.Sidx = key;
                            }

                            var orderString = $"{input.Sidx}";
                            sql = sql.Replace("@recordCount", " count(*) over() as RecordCount ")
                                .Replace("@rowNumber", " row_number() over (order by @orderBy) as rownum ")
                                .Replace("@orderBy", orderString)
                                 .Replace("@where", " WHERE 1=1 ");

                            var querySqlRecordCount = querySql.Split("@recordCount");
                            var recordCountSql = querySql.Replace(querySqlRecordCount[0], ";select count(*) ").Replace("@recordCount", "  ").Replace("@where", " WHERE 1=1 ");
                            //总数量
                            sql += recordCountSql;
                            using (var db = GetConnectoin(dataBase.ConnectionString, dataBase.ConnectionType))
                            {
                                var queryMulti = await db.QueryMultipleAsync(sql, parameters);
                                data = queryMulti.Read<dynamic>().ToList();
                                v.page = input.Current;
                                v.count = queryMulti.Read<long>().Sum();
                                v.data = data;
                            }

                        }
                        var masks = input.Columns.JsonStringToList<SystemDataBaseFindFormSourceDataColumnsInput>().Where(w => w.Mask.IsNotNullOrEmpty()).ToList();
                        //是否具有掩码
                        if (masks.Any() && data.Any())
                        {
                            List<dynamic> tmp = new List<dynamic>();
                            foreach (var item in data)
                            {
                                dynamic info = new ExpandoObject();
                                var dic = (IDictionary<string, object>)info;
                                foreach (var dd in item)
                                {
                                    string dkey = dd.Key;
                                    object dvalue = dd.Value;
                                    var haveMask = masks.FirstOrDefault(f => f.Field == dkey);
                                    if (dvalue != null && haveMask != null)
                                    {
                                        if (!string.IsNullOrWhiteSpace(dvalue.ToString()))
                                        {
                                            dvalue = Convert.ToString(dvalue).ConvertMask(haveMask.Mask);
                                        }
                                    }
                                    dic.Add(dkey, dvalue);
                                }
                                tmp.Add(info);
                            }
                            data = tmp;

                            v.data = data;
                        }
                        return OperateStatus<dynamic>.Success(v);
                    }
                    else if (output.Type == "api")
                    {
                        return operateStatus;
                    }
                    else
                    {
                        return operateStatus;
                    }
                }

                else
                {
                    return operateStatus;
                }
            }
            catch (Exception ex)
            {
                operateStatus.Msg = "读取异常";
                return operateStatus;
            }
        }

        /// <summary>
        /// 根据Sql查询
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<dynamic>> FindFormSourceDataPagingTable(SystemDataBaseFindTableInput input)
        {
            OperateStatus<dynamic> operateStatus = new OperateStatus<dynamic>();
            try
            {
                var agileConfig = (await _agileConfigLogic.FindPublicJsonById(new IdInput { Id = input.Table })).Data;
                var dataBase = await GetSystemDataBase(new IdInput { Id = agileConfig.DataBaseId });
                string connectionType = dataBase.ConnectionType;
                string connectionString = dataBase.ConnectionString;

                var currentUser = EipHttpContext.CurrentUser();
                var parameters = new Dictionary<string, object>();

                List<dynamic> data = new List<dynamic>();
                var name = agileConfig.DataFromName;
                var outputParams = new List<string>();
                outputParams.Add("RelationId");
                var outParams = input.Columns.JsonStringToList<SystemDataBaseFindTableDataColumnsInput>();
                foreach (var item in outParams)
                {
                    outputParams.Add($"{item.Field}");
                }
                dynamic v = new ExpandoObject();

                if (dataBase.ConnectionType == ResourceDataBaseType.Mysql)
                {
                    var currentPage = input.Current; //当前页号
                    var pageSize = input.Size; //每页记录数
                    var lower = ((currentPage - 1) * pageSize) + 1; //记录起点
                    var upper = currentPage * pageSize; //记录终点
                    var querySql = $"SELECT {outputParams.Distinct().ExpandAndToString().ToString().TrimEnd(',')},@rowNumber, @recordCount FROM {name} @where ";
                    if (input.Sql.IsNotNullOrEmpty())
                    {
                        querySql += input.Sql;
                    }
                    if (input.DataSql.IsNotNullOrEmpty())
                    {
                        querySql += " and " + input.DataSql;
                    }
                    //若为空则并且有查询字段则查询最后一个排序
                    if (input.Sidx.IsNullOrEmpty() && outParams.Count > 0)
                    {
                        var item = outParams[outParams.Count() - 1];
                        var key = item.Field;
                        input.Sidx = key;
                    }
                    else
                    {
                        input.Sidx = "CreateTime";
                    }
                    var sql = $@"select * from ({querySql} order by {input.Sidx} ) seq  limit {lower - 1},{upper}";
                    var recordCount = sql.Split("@recordCount");
                    var selectSql = recordCount[0].Trim().TrimEnd(',');
                    sql = selectSql + " " + recordCount[1];
                    sql = sql.Replace("@rowNumber", " 1 ")
                        .Replace("@orderBy", " 2 ")
                        .Replace("@where", " WHERE 1=1 AND IsDelete=0 " + (string.IsNullOrWhiteSpace(input.Sql) ? string.Empty : input.Sql));

                    var querySqlRecordCount = querySql.Split("@recordCount");
                    var recordCountSql = querySql.Replace(querySqlRecordCount[0], ";select count(*) ").Replace("@recordCount", "  ").Replace("@where", " WHERE 1=1 AND IsDelete=0 " + (string.IsNullOrWhiteSpace(input.Sql) ? string.Empty : input.Sql));
                    //总数量
                    sql += recordCountSql;

                    using (var db = GetConnectoin(dataBase.ConnectionString, dataBase.ConnectionType))
                    {
                        var queryMulti = await db.QueryMultipleAsync(sql, parameters);
                        data = queryMulti.Read<dynamic>().ToList();

                        v.page = input.Current;
                        v.count = queryMulti.Read<long>().Sum();
                        v.data = data;
                    }
                }
                else
                {
                    var querySql = $"SELECT {outputParams.Distinct().ExpandAndToString().ToString().TrimEnd(',')},@rowNumber, @recordCount  FROM {name} @where  ";
                    if (input.Sql.IsNotNullOrEmpty())
                    {
                        querySql += input.Sql;
                    }
                    if (input.DataSql.IsNotNullOrEmpty())
                    {
                        querySql += " and " + input.DataSql;
                    }
                    var sql = input.IsReport ?
                        $@"select * from ({querySql}) seq "
                        : $@"select * from ({querySql}) seq where seq.rownum between @x and @y";
                    var currentPage = input.Current; //当前页号
                    var pageSize = input.Size; //每页记录数
                    var lower = ((currentPage - 1) * pageSize) + 1; //记录起点
                    var upper = currentPage * pageSize; //记录终点
                    parameters.Add("x", lower);
                    parameters.Add("y", upper);
                    //排序字段

                    //若为空则并且有查询字段则查询最后一个排序
                    if (input.Sidx.IsNullOrEmpty() && outParams.Count > 0)
                    {
                        var item = outParams[outParams.Count - 1];
                        var key = item.Field;
                        input.Sidx = key;
                    }

                    var orderString = $"{input.Sidx}";
                    sql = sql.Replace("@recordCount", " count(*) over() as RecordCount ")
                        .Replace("@rowNumber", " row_number() over (order by @orderBy) as rownum ")
                        .Replace("@orderBy", orderString)
                         .Replace("@where", " WHERE 1=1 AND IsDelete=0 ");

                    var querySqlRecordCount = querySql.Split("@recordCount");
                    var recordCountSql = querySql.Replace(querySqlRecordCount[0], ";select count(*) ").Replace("@recordCount", "  ").Replace("@where", " WHERE 1=1 AND IsDelete=0 ");
                    //总数量
                    sql += recordCountSql;
                    using (var db = GetConnectoin(dataBase.ConnectionString, dataBase.ConnectionType))
                    {
                        var queryMulti = await db.QueryMultipleAsync(sql, parameters);
                        data = queryMulti.Read<dynamic>().ToList();
                        v.page = input.Current;
                        v.count = queryMulti.Read<long>().Sum();
                        v.data = data;
                    }

                }

                var masks = outParams.Where(w => w.Mask.IsNotNullOrEmpty()).ToList();
                //是否具有掩码
                if (masks.Any() && data.Any())
                {
                    List<dynamic> tmp = new List<dynamic>();
                    foreach (var item in data)
                    {
                        dynamic info = new ExpandoObject();
                        var dic = (IDictionary<string, object>)info;
                        foreach (var dd in item)
                        {
                            string dkey = dd.Key;
                            object dvalue = dd.Value;
                            var haveMask = masks.FirstOrDefault(f => f.Field == dkey);
                            if (dvalue != null && haveMask != null)
                            {
                                if (!string.IsNullOrWhiteSpace(dvalue.ToString()))
                                {
                                    dvalue = Convert.ToString(dvalue).ConvertMask(haveMask.Mask);
                                }
                            }
                            dic.Add(dkey, dvalue);
                        }
                        tmp.Add(info);
                    }
                    data = tmp;

                    v.data = data;
                }
                return OperateStatus<dynamic>.Success(v);

            }
            catch (Exception ex)
            {
                operateStatus.Msg = "读取异常";
                return operateStatus;
            }
        }

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> DeleteBusinessData(SystemDataBaseDeleteBusinessDataInput input)
        {
            OperateStatus operateStatus = new OperateStatus();
            //获取模板数据
            var currentUser = EipHttpContext.CurrentUser();
            var agileConfig = (await _agileConfigLogic.FindPublicJsonById(new IdInput { Id = input.ConfigId })).Data;
            var dataBase = await GetSystemDataBase(new IdInput { Id = agileConfig.DataBaseId });
            string connectionType = dataBase.ConnectionType;
            string connectionString = dataBase.ConnectionString;
            using (var fix = new SqlDatabaseFixture(new SqlDatabaseFixtureInput
            {
                ConnectionString = connectionString,
                ConnectionType = connectionType,
            }))
            {
                StringBuilder stringBuilder = new StringBuilder();
                //逻辑删除
                stringBuilder.Append($" UPDATE {agileConfig.DataFromName} SET " +
                    $"IsDelete=1," +
                    $"UpdateTime='{DateTime.Now}'," +
                    $"UpdateUserName='{currentUser.Name}'," +
                    $"UpdateUserId='{currentUser.UserId}'," +
                    $"UpdateOrganizationId='{currentUser.OrganizationId}'," +
                    $"UpdateOrganizationName='{currentUser.OrganizationName}'" +
                    $" WHERE RelationId in ({input.Id.InSql()}) ");
                if (await fix.Db.Connection.ExecuteAsync(stringBuilder.ToString()) > 0)
                {
                    operateStatus.Code = ResultCode.Success;
                    operateStatus.Msg = Chs.Successful;
                }
            }
            return operateStatus;
        }

        /// <summary>
        /// 物理删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> DeleteBusinessDataPhysics(SystemDataBaseDeleteBusinessDataInput input)
        {
            OperateStatus operateStatus = new OperateStatus();
            //获取模板数据
            var agileConfig = (await _agileConfigLogic.FindPublicJsonById(new IdInput { Id = input.ConfigId })).Data;
            var dataBase = await GetSystemDataBase(new IdInput { Id = agileConfig.DataBaseId });
            string connectionType = dataBase.ConnectionType;
            string connectionString = dataBase.ConnectionString;
            using (var fix = new SqlDatabaseFixture(new SqlDatabaseFixtureInput
            {
                ConnectionString = connectionString,
                ConnectionType = connectionType,
            }))
            {
                StringBuilder stringBuilder = new StringBuilder();
                //获取信息(物理删除)
                if (agileConfig.PublicJson.IsNotNullOrEmpty())
                {
                    var names = agileConfig.PublicJson.JsonStringToObject<SystemDataBaseFindPagingBusinessDataTableInput>().Columns.Where(w => w.Type.IsNotNullOrEmpty() && w.Type.ToUpper() == "BATCH");
                    if (names.Any())
                    {
                        foreach (var item in names)
                        {
                            foreach (var id in input.Id.Split(","))
                            {
                                stringBuilder.Append($" DELETE FROM {agileConfig.DataFromName}_{item.Name} WHERE RelationId='{id}' ;");
                                stringBuilder.Append($" DELETE FROM System_File WHERE CorrelationId like'%{id}|%' ;");
                            }
                        }
                    }
                }
                stringBuilder.Append($" DELETE FROM {agileConfig.DataFromName} WHERE RelationId in ({input.Id.InSql()}) ");
                if (await fix.Db.Connection.ExecuteAsync(stringBuilder.ToString()) > 0)
                {
                    operateStatus.Code = ResultCode.Success;
                    operateStatus.Msg = Chs.Successful;
                }
            }
            return operateStatus;
        }

        /// <summary>
        /// 物理删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> DeleteBusinessDataPhysicsAll(IdInput input)
        {
            OperateStatus operateStatus = new OperateStatus();
            //获取模板数据
            var agileConfig = (await _agileConfigLogic.FindPublicJsonById(new IdInput { Id = input.Id })).Data;
            var dataBase = await GetSystemDataBase(new IdInput { Id = agileConfig.DataBaseId });
            string connectionType = dataBase.ConnectionType;
            string connectionString = dataBase.ConnectionString;
            using (var fix = new SqlDatabaseFixture(new SqlDatabaseFixtureInput
            {
                ConnectionString = connectionString,
                ConnectionType = connectionType,
            }))
            {
                StringBuilder stringBuilder = new StringBuilder();
                //获取信息(物理删除)
                var selectAll = $" SELECT RelationId FROM {agileConfig.DataFromName} WHERE isDelete=1 ";
                var datas = await fix.Db.Connection.QueryAsync<Guid>(selectAll);
                foreach (var id in datas)
                {
                    if (agileConfig.PublicJson.IsNotNullOrEmpty())
                    {
                        var names = agileConfig.PublicJson.JsonStringToObject<SystemDataBaseFindPagingBusinessDataTableInput>().Columns.Where(w => w.Type.IsNotNullOrEmpty() && w.Type.ToUpper() == "BATCH");
                        if (names.Any())
                        {
                            foreach (var item in names)
                            {
                                stringBuilder.Append($" DELETE FROM {agileConfig.DataFromName}_{item.Name} WHERE RelationId='{id}' ;");
                                stringBuilder.Append($" DELETE FROM System_File WHERE CorrelationId like'%{id}|%' ;");
                            }
                        }
                    }
                }
                if (datas.Any())
                {
                    stringBuilder.Append($" DELETE FROM {agileConfig.DataFromName} WHERE isDelete=1; ");
                    if (await fix.Db.Connection.ExecuteAsync(stringBuilder.ToString()) > 0)
                    {
                        operateStatus.Code = ResultCode.Success;
                        operateStatus.Msg = Chs.Successful;
                    }
                }
                else
                {
                    operateStatus.Code = ResultCode.Success;
                    operateStatus.Msg = Chs.Successful;
                }
            }
            return operateStatus;
        }

        /// <summary>
        /// 逻辑删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> RecoveryDeleteBusinessDataPhysics(SystemDataBaseDeleteBusinessDataInput input)
        {
            OperateStatus operateStatus = new OperateStatus();
            //获取模板数据
            var currentUser = EipHttpContext.CurrentUser();
            var agileConfig = (await _agileConfigLogic.FindPublicJsonById(new IdInput { Id = input.ConfigId })).Data;
            var dataBase = await GetSystemDataBase(new IdInput { Id = agileConfig.DataBaseId });
            string connectionType = dataBase.ConnectionType;
            string connectionString = dataBase.ConnectionString;
            using (var fix = new SqlDatabaseFixture(new SqlDatabaseFixtureInput
            {
                ConnectionString = connectionString,
                ConnectionType = connectionType,
            }))
            {
                StringBuilder stringBuilder = new StringBuilder();
                //逻辑删除
                stringBuilder.Append($" UPDATE {agileConfig.DataFromName} SET " +
                    $"IsDelete=0," +
                    $"UpdateTime='{DateTime.Now}'," +
                    $"UpdateUserName='{currentUser.Name}'," +
                    $"UpdateUserId='{currentUser.UserId}'," +
                    $"UpdateOrganizationId='{currentUser.OrganizationId}'," +
                    $"UpdateOrganizationName='{currentUser.OrganizationName}'" +
                    $" WHERE RelationId in ({input.Id.InSql()}) ");
                if (await fix.Db.Connection.ExecuteAsync(stringBuilder.ToString()) > 0)
                {
                    operateStatus.Code = ResultCode.Success;
                    operateStatus.Msg = Chs.Successful;
                }
            }
            return operateStatus;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> ImportBusinessData(SystemDataBaseImportInput input)
        {
            OperateStatus<Guid> operateStatus = new OperateStatus<Guid>();
            try
            {
                IList<string> doSqls = new List<string>();
                //参数
                var parameters = new Dictionary<string, object>();
                using (var fix = new SqlDatabaseFixture())
                {
                    var agileConfig = await fix.Db.SystemAgile.SetSelect(s => new { s.PublicJson, s.DataFromName, s.ColumnJson, s.EditConfigId }).FindAsync(f => f.ConfigId == input.ConfigId);
                    var clos = new List<SystemDataBaseImportColsInput>();
                    clos = input.Clos.Where(a => a.Field != null).ToList();
                    var columns = new List<SystemDataBaseColumnJsonInput>();
                    if (!agileConfig.EditConfigId.IsNullOrEmptyGuid())
                    {
                        var agileEditConfig = await fix.Db.SystemAgile.SetSelect(s => new { s.ColumnJson }).FindAsync(f => f.ConfigId == agileConfig.EditConfigId);
                        columns = agileEditConfig.ColumnJson.JsonStringToList<SystemDataBaseColumnJsonInput>();
                    }

                    string wField = "", wValue = "";
                    //导入是否做删除操作
                    if (input.Type == 0)
                    {
                        string where = string.IsNullOrEmpty(input.WhereField) ? "" : " WHERE " + input.WhereValue;
                        doSqls.Add($"DELETE FROM {agileConfig.DataFromName}{where}");
                        foreach (var item in clos)
                        {
                            item.Title = item.Title.Trim();
                            var column = columns.FirstOrDefault(f => f.Model == item.Field || f.Model + "_Txt" == item.Field);
                            if (column != null)
                            {
                                switch (column.Type)
                                {
                                    case "serialNo":
                                        fix.Db.Connection.Execute($"DELETE FROM system_serialno WHERE ConfigId='{input.ConfigId}' and Model='{item.Field}'");
                                        break;
                                    default:

                                        break;
                                }
                            }
                        }
                    }
                    //设置条件
                    if (!string.IsNullOrEmpty(input.WhereField) && !input.WhereField.Equals("null"))
                    {
                        var arr = input.WhereField.Split("=");
                        wField = arr[0];
                        wValue = arr[1];
                    }
                    var tableClos = new List<string>();
                    if (input.Data.Any())
                    {
                        var rows = input.Data.FirstOrDefault();
                        foreach (var row in rows)
                        {
                            var key = row.Key;
                            var value = row.Value;
                            tableClos.Add(key);
                        }

                        string symbol = RepositoryUtil.GetSymbol();

                        var datas = input.Data.ToList();
                        for (int i = 0; i < datas.Count(); i++)
                        {
                            try
                            {
                                var data = datas[i];
                                StringBuilder stringBuilder = new StringBuilder();
                                StringBuilder stringBuilderValues = new StringBuilder();
                                stringBuilder.Append($"INSERT INTO {agileConfig.DataFromName} (");
                                string sql;
                                Guid id = CombUtil.NewComb();
                                IList<SystemDataBaseSaveBusinessDataColumns> columnsSaveBusinessData = new List<SystemDataBaseSaveBusinessDataColumns>();
                                foreach (var item in clos)
                                {
                                    item.Title = item.Title.Trim();
                                    if (tableClos.Any(t => t == item.Title))
                                    {
                                        string value = data[item.Title].ToString().Trim();
                                        columnsSaveBusinessData.Add(new SystemDataBaseSaveBusinessDataColumns
                                        {
                                            Name = item.Field,
                                            Value = value
                                        });
                                    }
                                }
                                foreach (var item in clos)
                                {
                                    item.Title = item.Title.Trim();
                                    if (tableClos.Any(t => t == item.Title))
                                    {
                                        string value = data[item.Title].ToString().Trim();
                                        var column = columns.FirstOrDefault(f => f.Model == item.Field || f.Model + "_Txt" == item.Field);
                                        if (column != null && value.IsNotNullOrEmpty())
                                        {
                                            switch (column.Type)
                                            {
                                                case "select":
                                                    {
                                                        SystemDataBaseColumnOptionSelect optionSelect = JsonConvert.DeserializeObject<SystemDataBaseColumnOptionSelect>(column.Options.ToString());
                                                        if (optionSelect.dynamic)
                                                        {

                                                        }
                                                        else
                                                        {
                                                            var optionValue = optionSelect.options.FirstOrDefault(f => f.label.Trim() == value.Trim());
                                                            if (optionValue != null)
                                                            {
                                                                var field = item.Field.Replace("_Txt", "");
                                                                stringBuilder.Append($"{field},");
                                                                stringBuilderValues.Append($"{symbol}{field}{i},");
                                                                parameters.Add($"{field}{i}", optionValue.value);
                                                            }
                                                        }
                                                    }
                                                    break;
                                                case "correlationRecord":
                                                    {
                                                        SystemDataBaseColumnOptionCorrelationRecord optionCorrelationRecord = JsonConvert.DeserializeObject<SystemDataBaseColumnOptionCorrelationRecord>(column.Options.ToString());
                                                        if (optionCorrelationRecord.dynamicConfig.table.HasValue)
                                                        {
                                                            //获取工作表
                                                            var agile = await fix.Db.SystemAgile.SetSelect(s => new { s.DataBaseId, s.DataFromName }).FindAsync(f => f.ConfigId == (Guid)optionCorrelationRecord.dynamicConfig.table);
                                                            if (agile != null)
                                                            {
                                                                var dataBase = await GetSystemDataBase(new IdInput { Id = agile.DataBaseId });
                                                                string connectionType = dataBase.ConnectionType;
                                                                string connectionString = dataBase.ConnectionString;
                                                                using (var db = GetConnectoin(dataBase.ConnectionString, dataBase.ConnectionType))
                                                                {
                                                                    var field = optionCorrelationRecord.columns.Where(w => w.isShow).FirstOrDefault().name;
                                                                    var sqlCorrelationRecord = "select RelationId from " + agile.DataFromName + $" where {field}='{value.Trim()}'";
                                                                    try
                                                                    {
                                                                        var correlationRecordDatas = await db.QueryAsync<SystemDataBaseColumnOptionCorrelationRecordDataOutput>(sqlCorrelationRecord);
                                                                        if (correlationRecordDatas != null && correlationRecordDatas.Any())
                                                                        {
                                                                            var fieldR = item.Field.Replace("_Txt", "");
                                                                            stringBuilder.Append($"{fieldR},");
                                                                            stringBuilderValues.Append($"{symbol}{fieldR}{i},");
                                                                            parameters.Add($"{fieldR}{i}", correlationRecordDatas.FirstOrDefault().RelationId);
                                                                        }
                                                                    }
                                                                    catch (Exception ex)
                                                                    {
                                                                        throw;
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                    break;
                                                case "radio":
                                                    {
                                                        SystemDataBaseColumnOptionSelect optionSelect = JsonConvert.DeserializeObject<SystemDataBaseColumnOptionSelect>(column.Options.ToString());
                                                        if (optionSelect.dynamic)
                                                        {

                                                        }
                                                        else
                                                        {
                                                            var optionValue = optionSelect.options.FirstOrDefault(f => f.label.Trim() == value.Trim());
                                                            if (optionValue != null)
                                                            {
                                                                var field = item.Field.Replace("_Txt", "");
                                                                stringBuilder.Append($"{field},");
                                                                stringBuilderValues.Append($"{symbol}{field}{i},");
                                                                parameters.Add($"{field}{i}", optionValue.value);
                                                            }
                                                        }
                                                    }
                                                    break;
                                                case "user":
                                                    {
                                                        var org = await fix.Db.SystemUserInfo.SetSelect(s => new { s.UserId }).FindAsync(f => f.Name == value);
                                                        if (org != null)
                                                        {
                                                            var field = item.Field.Replace("_Txt", "");
                                                            stringBuilder.Append($"{field},");
                                                            stringBuilderValues.Append($"{symbol}{field}{i},");
                                                            parameters.Add($"{field}{i}", org.UserId);
                                                        }
                                                    }
                                                    break;
                                                case "organization":
                                                    {
                                                        var org = await fix.Db.SystemOrganization.SetSelect(s => new { s.OrganizationId }).FindAsync(f => f.Name == value);
                                                        if (org != null)
                                                        {
                                                            var field = item.Field.Replace("_Txt", "");
                                                            stringBuilder.Append($"{field},");
                                                            stringBuilderValues.Append($"{symbol}{field}{i},");
                                                            parameters.Add($"{field}{i}", org.OrganizationId);
                                                        }
                                                    }
                                                    break;
                                                default:
                                                    break;
                                            }
                                            stringBuilder.Append($"{item.Field},");
                                            stringBuilderValues.Append($"{symbol}{item.Field}{i},");
                                            parameters.Add($"{item.Field}{i}", value.FilterSql());
                                        }
                                        else
                                        {
                                            if (column != null)
                                            {
                                                switch (column.Type)
                                                {
                                                    case "serialNo"://自增编号
                                                        {
                                                            SystemDataBaseSaveBusinessDataColumns row = JsonConvert.DeserializeObject<SystemDataBaseSaveBusinessDataColumns>(column.Options.ToString());

                                                            var noCreate = await _systemSerialNoLogic.Create(new SystemSerialCreateInput
                                                            {
                                                                Columns = columnsSaveBusinessData,
                                                                Rule = row.Rule,
                                                                ConfigId = input.ConfigId,
                                                                LoadDisplay = row.LoadDisplay,
                                                                Model = item.Field,
                                                                UserOccupation = row.UserOccupation
                                                            });
                                                            //更新值
                                                            if (noCreate.Code == ResultCode.Success)
                                                            {
                                                                var field = item.Field.Replace("_Txt", "");
                                                                stringBuilder.Append($"{field},");
                                                                stringBuilderValues.Append($"{symbol}{field}{i},");
                                                                parameters.Add($"{field}{i}", noCreate.Data);
                                                            }

                                                            if (row.UserOccupation)
                                                            {
                                                                //更新用户编码
                                                                _systemSerialNoLogic.Clear(input.ConfigId, row.Name);
                                                            }
                                                        }
                                                        break;
                                                    default:

                                                        break;
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //序号处理
                                        var column = columns.FirstOrDefault(f => f.Model == item.Field || f.Model + "_Txt" == item.Field);
                                        if (column != null)
                                        {
                                            switch (column.Type)
                                            {
                                                case "serialNo"://自增编号
                                                    {
                                                        SystemDataBaseSaveBusinessDataColumns row = JsonConvert.DeserializeObject<SystemDataBaseSaveBusinessDataColumns>(column.Options.ToString());

                                                        var noCreate = await _systemSerialNoLogic.Create(new SystemSerialCreateInput
                                                        {
                                                            Columns = columnsSaveBusinessData,
                                                            Rule = row.Rule,
                                                            ConfigId = input.ConfigId,
                                                            LoadDisplay = row.LoadDisplay,
                                                            Model = item.Field,
                                                            UserOccupation = row.UserOccupation
                                                        });
                                                        //更新值
                                                        if (noCreate.Code == ResultCode.Success)
                                                        {
                                                            var field = item.Field.Replace("_Txt", "");
                                                            stringBuilder.Append($"{field},");
                                                            stringBuilderValues.Append($"{symbol}{field}{i},");
                                                            parameters.Add($"{field}{i}", noCreate.Data);
                                                        }

                                                        if (row.UserOccupation)
                                                        {
                                                            //更新用户编码
                                                            _systemSerialNoLogic.Clear(input.ConfigId, row.Name);
                                                        }
                                                    }
                                                    break;
                                                default:

                                                    break;
                                            }
                                        }
                                    }
                                }
                                //主键
                                stringBuilder.Append("RelationId,");
                                stringBuilderValues.Append($"'{id}',");

                                stringBuilder.Append("CreateUserId,");
                                stringBuilderValues.Append($"'{input.UserId}',");

                                stringBuilder.Append("CreateUserName,");
                                stringBuilderValues.Append($"'{input.UserName}',");

                                stringBuilder.Append("CreateOrganizationId,");
                                stringBuilderValues.Append($"'{input.OrganizationId}',");

                                stringBuilder.Append("CreateOrganizationName,");
                                stringBuilderValues.Append($"'{input.OrganizationName}',");

                                stringBuilder.Append("IsDelete,");
                                stringBuilderValues.Append($"0,");

                                //参数带过来的
                                if (!string.IsNullOrEmpty(input.WhereField) && !input.WhereField.Equals("null"))
                                {
                                    stringBuilder.Append(wField + ",");
                                    stringBuilderValues.Append($"'{wValue}',");
                                }

                                //拼接Sql
                                sql = stringBuilder.ToString().TrimEnd(',') + " ) VALUES (" +
                                      stringBuilderValues.ToString().TrimEnd(',') + ")";
                                if (stringBuilder.ToString().IsNotNullOrEmpty())
                                {
                                    doSqls.Add(sql);
                                }
                            }
                            catch (Exception e)
                            {
                                operateStatus.Msg = "操作失败:" + e.Message;
                                operateStatus.Code = ResultCode.Error;
                            }
                        }
                    }
                }
                if (doSqls.Any())
                {
                    var agileConfig = (await _agileConfigLogic.FindPublicJsonById(new IdInput { Id = input.ConfigId })).Data;
                    var dataBase = await GetSystemDataBase(new IdInput { Id = agileConfig.DataBaseId });
                    string connectionType = dataBase.ConnectionType;
                    string connectionString = dataBase.ConnectionString;
                    using (var fix = new SqlDatabaseFixture(new SqlDatabaseFixtureInput
                    {
                        ConnectionString = connectionString,
                        ConnectionType = connectionType,
                    }))
                    {
                        var trans = fix.Db.Connection.BeginTransaction();
                        try
                        {
                            var totalInstances = doSqls.Count();
                            int maxAllowedInstancesPerBatch = 50;
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
                                    fix.Db.Connection.Execute(items.ExpandAndToString(";"), parameters, trans);
                                }
                            }
                            else
                            {
                                fix.Db.Connection.Execute(doSqls.ExpandAndToString(";"), parameters, trans);
                            }
                            trans.Commit();
                            operateStatus.Code = ResultCode.Success;
                            operateStatus.Msg = Chs.Successful;
                        }
                        catch (Exception ex)
                        {
                            trans.Rollback();
                            operateStatus.Msg = ex.Message;
                            operateStatus.Code = ResultCode.Error;
                            return operateStatus;
                        }
                    }
                }
                else
                {
                    operateStatus.Code = ResultCode.Success;
                }

            }
            catch (Exception e)
            {
                operateStatus.Msg = "操作失败:" + e.Message;
                operateStatus.Code = ResultCode.Error;
            }
            return operateStatus;
        }

        #endregion


        /// <summary>
        /// 处理SQL查询条件，含子查询
        /// </summary>
        /// <param name="filter">查询对象</param>
        /// <returns></returns>
        public static string ConvertFilters(Filter filter)
        {
            if (filter == null)
                return "";

            //标识是否为首次加载有效字段条件，作为添加AND或OR关键字
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
                            where.AppendFormat(" {0} ", filter.groupOp);
                        }
                        where.Append(ConvertFilters(item));
                        isFirst = false;
                    }
                }
            }

            //处理嵌套查询
            if (filter.groups != null && filter.groups.Count > 0)
            {
                foreach (var item in filter.groups)
                {
                    string child = ConvertFilters(item);
                    if (!string.IsNullOrEmpty(child))
                    {
                        where.AppendFormat(" {0} {1}", (filter.rules != null && filter.rules.Count > 0) ? filter.groupOp : "", child);
                    }
                }
            }

            //返回
            return where.Length > 0 ? $" ({where}) " : "";
        }

        /// <summary>
        /// 处理单个字段查询，匹配数据类型及查询方式
        /// </summary>
        /// <param name="rule">查询字段对象</param>
        /// <returns></returns>
        private static string ConvertFilters(FilterRules rule)
        {
            string phoneKey = "Ph376A@e^270Ks_~";
            if (string.IsNullOrEmpty(rule.op) || string.IsNullOrEmpty(rule.field))
            {
                return "";
            }
            string symbol = RepositoryUtil.GetSymbol();
            //Sql注入替换后数据
            StringBuilder searchCase = new StringBuilder();
            switch (rule.op)
            {
                case "eq": //等于
                    searchCase.Append(GetFilter(rule.field, " =" + symbol + rule.field));
                    break;
                case "ne": //不等于
                    searchCase.Append(GetFilter(rule.field, " !=" + symbol + rule.field));
                    break;
                case "bw": //以...开始
                    searchCase.Append(GetFilter(rule.field, " like concat(''," + symbol + rule.field + ",'%')"));
                    break;
                case "bn": //不以...开始
                    searchCase.Append(GetFilter(rule.field, " not like concat(''," + symbol + rule.field + ",'%')"));
                    break;
                case "ew": //结束于
                    searchCase.Append(GetFilter(rule.field, " like concat('%'," + symbol + rule.field + ",'')"));
                    break;
                case "en": //不结束于
                    searchCase.Append(GetFilter(rule.field, " not like concat('%'," + symbol + rule.field + ",'')"));
                    break;
                case "lt": //小于
                    searchCase.Append(GetFilter(rule.field, " <" + symbol + rule.field + ""));
                    break;
                case "le": //小于等于
                    searchCase.Append(GetFilter(rule.field, " <=" + symbol + rule.field + ""));
                    break;
                case "gt": //大于
                    searchCase.Append(GetFilter(rule.field, " >" + symbol + rule.field + ""));
                    break;
                case "ge": //大于等于
                    searchCase.Append(GetFilter(rule.field, " >=" + symbol + rule.field + ""));
                    break;
                case "in": //包括
                    searchCase.Append(GetFilter(rule.field, " in " + symbol + rule.field + ""));
                    break;
                case "ni": //不包含
                    searchCase.Append(GetFilter(rule.field, " not in " + symbol + rule.field + ""));
                    break;
                case "cn"://包含
                    searchCase.Append(GetFilter(rule.field, " like concat('%'," + symbol + rule.field + ",'%')"));
                    break;
                case "nc"://不包含
                    searchCase.Append(GetFilter(rule.field, " not like concat('%'," + symbol + rule.field + ",'%')"));
                    break;
                case "nu"://空值
                    searchCase.Append(GetFilter(rule.field, " is null"));
                    break;
                case "nn"://非空值
                    searchCase.Append(GetFilter(rule.field, " is not null"));
                    break;
                case "date"://针对时间特别处理
                    searchCase.Append(GetFilter(rule.field, " between " + symbol + rule.field + " AND " + symbol + rule.field + "'"));
                    break;
                case "daterange"://针对时间特别处理
                                 //var dateTime = data.Split('&');
                                 //searchCase.Append(GetFilter(rule.field, " between '" + dateTime[0].Trim() + "' AND '" + dateTime[1].Trim() + "'"));
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
                    search += fi + formula + " OR " /* " like '%" + data + "%' OR "*/;
                }
                searchCase.Append("(" + search.Substring(0, search.Length - 4) + ")");
            }
            else
            {
                searchCase.Append(field + formula);
            }
            return searchCase.ToString();
        }

        /// <summary>
        /// 根据数据库Id返回对应数据库连接
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task<SystemDataBaseInput> GetSystemDataBase(IdInput input)
        {
            var cacheKey = $"ISystemDataBaseLogic_Cache:{input.Id}";
            var dataBase = await RedisHelper.CacheShellAsync(cacheKey, DateTimeUtil.TotalSeconds(20), async () =>
            {
                return await FindAsync(f => f.DataBaseId == input.Id);
            });
            SystemDataBaseInput systemDataBaseInput = new SystemDataBaseInput();
            if (dataBase != null)
            {
                systemDataBaseInput = new SystemDataBaseInput
                {
                    ConnectionType = dataBase.ConnectionType,
                    ConnectionString = ConfigurationUtil.GetSection(dataBase.ConnectionString)
                };
            }
            return systemDataBaseInput;
        }
        /// <summary>
        /// 根据数据库Id返回对应数据库连接
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task<SystemDataSource> GetSystemDataSource(IdInput input)
        {
            var cacheKey = $"ISystemDataSourceLogic_Cache:{input.Id}";
            var systemDataSource = await RedisHelper.CacheShellAsync(cacheKey, DateTimeUtil.TotalSeconds(20), async () =>
            {
                using (var fix = new SqlDatabaseFixture())
                {
                    return await fix.Db.SystemDataSource.SetSelect(s => new { s.Config }).FindAsync(f => f.DataSourceId == input.Id);
                }
            });
            return systemDataSource;
        }
        #region 数据库维护
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemDataBase>>> FindAll()
        {
            return OperateStatus<IEnumerable<SystemDataBase>>.Success((await FindAllAsync()).OrderBy(o => o.OrderNo));
        }

        /// <summary>
        /// 保存数据库配置
        /// </summary>
        /// <param name="input">数据库配置</param>
        /// <returns></returns>
        public async Task<OperateStatus> Save(SystemDataBase input)
        {
            OperateStatus operateStatus = new OperateStatus();
            //检测是否能够连接成功
            if (input.ConnectionString.IsNullOrEmpty())
            {
                operateStatus.Msg = ResourceSystem.请先配置数据库连接字符串;
                return operateStatus;
            }
            //判断是否能够连接成功
            try
            {
                new DbHelper(ConfigurationUtil.GetSection(input.ConnectionString)).Test(input.ConnectionType, ConfigurationUtil.GetSection(input.ConnectionString));
            }
            catch (Exception ex)
            {
                operateStatus.Msg = string.Format(ResourceSystem.数据库打开失败, ex.Message);
                return operateStatus;
            }
            var dataBase = await FindAsync(f => f.DataBaseId == input.DataBaseId);
            var currentUser = EipHttpContext.CurrentUser();
            if (dataBase == null)
            {
                input.CreateTime = DateTime.Now;
                input.CreateUserId = currentUser.UserId;
                input.CreateUserName = currentUser.Name;
                input.UpdateTime = DateTime.Now;
                input.UpdateUserId = currentUser.UserId;
                input.UpdateUserName = currentUser.Name;
                return await InsertAsync(input);
            }
            input.Id = dataBase.Id;
            input.CreateTime = dataBase.CreateTime;
            input.CreateUserId = dataBase.CreateUserId;
            input.CreateUserName = dataBase.CreateUserName;

            input.UpdateTime = DateTime.Now;
            input.UpdateUserId = currentUser.UserId;
            input.UpdateUserName = currentUser.Name;
            return await UpdateAsync(input);
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<SystemDataBase>> FindById(IdInput input)
        {
            return OperateStatus<SystemDataBase>.Success(await FindAsync(f => f.DataBaseId == input.Id));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> Delete(IdInput input)
        {
            return await DeleteAsync(f => f.DataBaseId == input.Id);
        }
        #endregion

    }
}