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
namespace EIP.System.Repository.Impl
{
    /// <summary>
    /// 数据库操作接口实现
    /// </summary>
    public class SystemDataBaseRepository : ISystemDataBaseRepository
    {
        #region 外键Sql
        const string FkSql = @"SELECT
                    ThisTable  = FK.TABLE_NAME,
                    ThisColumn = CU.COLUMN_NAME,
                    OtherTable  = PK.TABLE_NAME,
                    OtherColumn = PT.COLUMN_NAME, 
                    Constraint_Name = C.CONSTRAINT_NAME,
                    Owner = FK.TABLE_SCHEMA
                FROM INFORMATION_SCHEMA.REFERENTIAL_CONSTRAINTS C
                INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS FK ON C.CONSTRAINT_NAME = FK.CONSTRAINT_NAME
                INNER JOIN INFORMATION_SCHEMA.TABLE_CONSTRAINTS PK ON C.UNIQUE_CONSTRAINT_NAME = PK.CONSTRAINT_NAME
                INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE CU ON C.CONSTRAINT_NAME = CU.CONSTRAINT_NAME
                INNER JOIN
                    (	
                        SELECT i1.TABLE_NAME, i2.COLUMN_NAME
                        FROM  INFORMATION_SCHEMA.TABLE_CONSTRAINTS i1
                        INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE i2 ON i1.CONSTRAINT_NAME = i2.CONSTRAINT_NAME
                        WHERE i1.CONSTRAINT_TYPE = 'PRIMARY KEY'
                    ) 
                PT ON PT.TABLE_NAME = PK.TABLE_NAME
                WHERE FK.Table_NAME=@tableName OR PK.Table_NAME=@tableName";
        #endregion

        #region 表Sql
        const string MssqlTableSql = @"SELECT tbs.name [Name],ds.value [Description] ,crdate CreateTime FROM  sysobjects tbs LEFT JOIN  sys.extended_properties ds ON ds.major_id=tbs.id AND ds.minor_id=0 WHERE   xtype='U' ORDER BY tbs.name";
        const string MysqlTableSql = @"select table_name 'Name', table_comment 'Description', create_time 'CreateTime' from information_schema.tables where table_schema = (select database()) and table_comment!='VIEW'";
        const string DmTableSql = @"select a.object_name as Name,b.comment$ as Description,a.created as CreateTime from dba_objects a left join SYSTABLECOMMENTS b on b.tvname = a.object_name where a.object_type = 'TABLE' and a.owner = 'EIP' and a.object_name not like 'SREF_CON%' and a.object_name not like 'GEN_%'";

        const string MysqlViewSql = @"show table status where comment='view';";
        const string MssqlViewSql = @"SELECT s.Name,Convert(varchar(max),tbp.value) as Description FROM sysobjects s LEFT JOIN sys.extended_properties as tbp ON s.id=tbp.major_id and tbp.minor_id=0  AND (tbp.Name='MS_Description' OR tbp.Name is null) WHERE s.xtype IN('V') ";

        const string MysqlProcSql = @"SHOW PROCEDURE STATUS where db=(select database())";
        const string MssqlProcSql = @"SELECT s.Name,Convert(varchar(max),tbp.value) as Description FROM sysobjects s LEFT JOIN sys.extended_properties as tbp ON s.id=tbp.major_id and tbp.minor_id=0  AND (tbp.Name='MS_Description' OR tbp.Name is null) WHERE s.xtype IN('P') ";
        #endregion

        #region 列Sql
        const string MssqlColumnSql = @"SELECT 
                d.name TableName,
                Name= a.name,
                IsIdentity= case when COLUMNPROPERTY( a.id,a.name,'IsIdentity')=1 then '√'else '' end,
                DataType= b.name,
                MaxLength = COLUMNPROPERTY(a.id,a.name,'PRECISION'),
                IsNullable= case when a.isnullable=1 then '√'else '' end,
                DefaultSetting= isnull(e.text,''),
                Description= isnull(g.[value],'')
                FROM 
                    syscolumns a
                left join 
                    systypes b 
                on 
                    a.xusertype=b.xusertype
                inner join 
                    sysobjects d 
                on 
                    a.id=d.id  and (d.xtype='U' or d.xtype='V') and  d.name<>'dtproperties'
                left join 
                    syscomments e 
                on 
                    a.cdefault=e.id
                left join 
                sys.extended_properties   g 
                on 
                    a.id=G.major_id and a.colid=g.minor_id  
                left join
                sys.extended_properties f
                on 
                    d.id=f.major_id and f.minor_id=0
                where 
                    {0}
                group by 
                     a.name,d.name,b.name,a.id,a.isnullable,e.text,g.[value],a.colorder
                order by 
                    a.id,a.colorder";

        /// <summary>
        /// MySql字段
        /// </summary>
        string MysqlColumnSql = @"SELECT a.column_Name AS Name ,a.TABLE_NAME tableName,
                CASE WHEN a.extra = 'auto_increment' THEN '√' ELSE '' END  AS IsIdentity, 
                CASE Is_Nullable when 'YES' then '√' else '' end IsNullable,
	            data_type DataType,
	            Column_default DefaultSetting,
                Column_Key ColumnKey,
	            character_maximum_length MaxLength,
	            column_comment Description
                FROM information_schema.COLUMNS  a
                LEFT JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS p ON a.table_schema = p.table_schema AND a.table_name = p.TABLE_NAME AND a.COLUMN_NAME = p.COLUMN_NAME AND p.constraint_name='PRIMARY'
                WHERE a.table_schema = (select database()) and {0}
                ORDER BY a.ordinal_position  ";

        string DmColumnSql = "select a.data_length as MaxLength,a.column_name Name, (case when (a.nullable = 'N') then '' else '√' end) as IsNullable, c.comment$ as Description, a.data_type as DataType from all_tab_columns a left join (select top 1 * from user_ind_columns where  table_name =  {0}) b on b.column_name = a.column_name left join SYSCOLUMNCOMMENTS c on c.colname = a.column_name and c.tvname = a.table_name where a.owner='EIP' and a.Table_Name= {0}  order by a.column_id";
        #endregion

        /// <summary>
        /// 查看对应数据库空间占用情况
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<SystemDataBaseSpaceOutput>> FindDataBaseSpaceused()
        {
            const string procName = @"System_Proc_Spaceused";
            return await new SqlMapperUtil().StoredProcWithParams<SystemDataBaseSpaceOutput>(procName, null, false);
        }

        /// <summary>
        /// 获取对应数据库表信息
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<SystemDataBaseTableDto>> FindDataBaseTable(SystemDataBaseInput input)
        {
            string tableSql;
            switch (input.ConnectionType)
            {
                case ResourceDataBaseType.Mysql:
                    tableSql = MysqlTableSql;
                    break;
                case ResourceDataBaseType.Dm:
                    tableSql = DmTableSql;
                    break;
                default:
                    tableSql = MssqlTableSql;
                    break;
            }
            return await new SqlMapperUtil(string.Empty, input.ConnectionType, input.ConnectionString).SqlWithParams<SystemDataBaseTableDto>(tableSql, false);
        }
        /// <summary>
        /// 获取对应数据库表信息
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<SystemDataBaseTableDto>> FindDataBaseView(SystemDataBaseInput input)
        {
            string tableSql;
            switch (input.ConnectionType)
            {
                case ResourceDataBaseType.Mysql:
                    tableSql = MysqlViewSql;
                    break;
                default:
                    tableSql = MssqlViewSql;
                    break;
            }
            return await new SqlMapperUtil(string.Empty, input.ConnectionType, input.ConnectionString).SqlWithParams<SystemDataBaseTableDto>(tableSql, false);
        }
        /// <summary>
        /// 获取对应数据库表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SystemDataBaseTableDto>> FindDataBaseProc(SystemDataBaseInput input)
        {
            string tableSql;
            switch (input.ConnectionType)
            {
                case ResourceDataBaseType.Mysql:
                    tableSql = MysqlProcSql;
                    break;
                default:
                    tableSql = MssqlProcSql;
                    break;
            }
            return await new SqlMapperUtil(string.Empty, input.ConnectionType, input.ConnectionString).SqlWithParams<SystemDataBaseTableDto>(tableSql, false);
        }

        /// <summary>
        /// 获取对应表列信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<IEnumerable<SystemDataBaseColumnDto>> FindDataBaseColumns(SystemDataBaseTableDto input)
        {
            IList<SystemDataBaseColumnDto> dataBaseColumnDoubleWays = new List<SystemDataBaseColumnDto>();
            if (input.DataType == "proc")
            {
                var parms = new DynamicParameters();
                string procName = input.Name;
                var dataTable = await new SqlMapperUtil(string.Empty, input.ConnectionType, input.ConnectionString).StoredProcWithParamsDataTable<dynamic>(procName, parms, false);
                foreach (DataColumn item in dataTable.Columns)
                {
                    dataBaseColumnDoubleWays.Add(new SystemDataBaseColumnDto
                    {
                        Name = item.ColumnName,
                        Description = ""
                    });
                }
                return dataBaseColumnDoubleWays;
            }
            else if (input.DataType == "api")
            {
                var apiObj = JsonConvert.DeserializeObject<SystemDataBaseTableApiDto>(input.Name);
                QueryParam queryParam = new QueryParam();
                queryParam.Size = 1;
                switch (apiObj.Type.ToLower())
                {
                    case "post":
                        Dictionary<string, string> headers = new Dictionary<string, string>();
                        headers.Add("Authorization", input.Header);
                        var result =await RequestUtil.HttpPost(apiObj.Url.Trim(),JsonConvert.SerializeObject( queryParam), headers, "application/x-www-form-urlencoded");
                        if (apiObj.Paging)
                        {
                            var resultPaging = JsonConvert.DeserializeObject<OperateStatus<PagedResults<dynamic>>>(result);
                            if (resultPaging.Code == ResultCode.Success && resultPaging.Data.Data.Count > 0)
                            {
                                var firstRow = resultPaging.Data.Data.FirstOrDefault();
                                foreach (var col in firstRow)
                                {
                                    dataBaseColumnDoubleWays.Add(new SystemDataBaseColumnDto
                                    {
                                        Name = col.Name,
                                    });
                                }
                            }
                        }
                        else
                        {
                            var resultNoPaging = JsonConvert.DeserializeObject<OperateStatus<List<dynamic>>>(result);
                            if (resultNoPaging.Code == ResultCode.Success && resultNoPaging.Data.Count > 0)
                            {
                                var firstRow = resultNoPaging.Data.FirstOrDefault();
                                foreach (var col in firstRow)
                                {
                                    dataBaseColumnDoubleWays.Add(new SystemDataBaseColumnDto
                                    {
                                        Name = col.Name,
                                    });
                                }
                            }
                        }
                        break;
                    default:
                        RequestUtil.EventDoByApiGet(apiObj.Url, null, input.Header);
                        break;
                }
                return dataBaseColumnDoubleWays;
            }
            else
            {
                //根据不同类型的数据库
                string columnSql;
                switch (input.ConnectionType)
                {
                    case ResourceDataBaseType.Mysql:
                        columnSql = string.Format(MysqlColumnSql, $"  a.table_name in ({input.Name.InSql()})");
                        break;
                    case ResourceDataBaseType.Dm:
                        columnSql = string.Format(DmColumnSql, $" {input.Name.InSql()}");
                        break;
                    default:
                        columnSql = string.Format(MssqlColumnSql, $" d.name in ({input.Name.InSql()})");
                        break;
                }

                return await new SqlMapperUtil(string.Empty, input.ConnectionType, input.ConnectionString).SqlWithParams<SystemDataBaseColumnDto>(columnSql,
                    new
                    {
                        tablename = input.Name,
                    });
            }
        }

        /// <summary>
        /// 获取外键信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<IEnumerable<SystemDataBaseFkColumnOutput>> FinddatabsefFkColumn(SystemDataBaseTableDto input)
        {
            return new SqlMapperUtil().SqlWithParams<SystemDataBaseFkColumnOutput>(FkSql);
        }

        /// <summary>
        /// 判断数据库表是否存在
        /// </summary>
        public async Task<bool> IsTableExist(SystemDataBaseIsTableExistInput input)
        {
            string createDbStr = string.Empty;
            switch (input.ConnectionType)
            {
                case ResourceDataBaseType.Mysql:
                    createDbStr = $"select 1 from information_schema.TABLES t where t.TABLE_NAME='{input.DataFromName}'"; ;
                    if (!input.ConfigId.IsEmptyGuid())
                    {
                        createDbStr += $" AND t.TABLE_NAME!='{input.DataFromName}'";
                    }
                    break;
                case ResourceDataBaseType.Postgresql:
                    break;
                default:
                    createDbStr = $"select 1 from  sysobjects where  id = object_id('{input.DataFromName}') and type = 'U'";
                    if (!input.ConfigId.IsEmptyGuid())
                    {
                        createDbStr += $" AND id!=object_id('{input.DataFromName}')";
                    }
                    break;
            }
            DataTable dt = new DbHelper(input.ConnectionString).CreateSqlDataTable(createDbStr);
            return dt.Rows.Count == 0;
        }


    }
}