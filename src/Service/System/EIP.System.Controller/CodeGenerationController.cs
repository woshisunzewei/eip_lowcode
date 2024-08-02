/**************************************************************
* Copyright (C) www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2018/10/30 22:40:15
* 文件名: 
* 描述: 
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
namespace EIP.System.Controller
{
    /// <summary>
    /// 代码生成器
    /// </summary>
    public class CodeGenerationController : BaseSystemController
    {
        #region 构造函数
        private readonly IHostingEnvironment _hostingEnvironment;
        private readonly ISystemDataBaseLogic _systemDataBaseLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="systemDataBaseLogic"></param>
        /// <param name="hostingEnvironment"></param>
        public CodeGenerationController(ISystemDataBaseLogic systemDataBaseLogic,
            IHostingEnvironment hostingEnvironment)
        {
            _systemDataBaseLogic = systemDataBaseLogic;
            _hostingEnvironment = hostingEnvironment;
        }

        #endregion

        #region 生成代码

        /// <summary>
        /// 生成代码
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("代码生成器-方法-生成代码", RemarkFrom.System, true)]
        [Route("/system/codegeneration/code")]
        public async Task<ActionResult> CreateCode([FromBody]IList<SystemCodeGenerationBaseInput> inputs)
        {
            var outpus = new List<SystemCodeGenerationOutput>();
            foreach (var input in inputs)
            {
                var tableSplit = input.Table.Split('_');
                //表头
                var tableTitle = tableSplit[0];
                //除开表头
                var tableTitleOther = "";
                for (int i = 0; i < tableSplit.Count(); i++)
                {
                    if (i != 0)
                    {
                        tableTitleOther += tableSplit[i].ReplaceFistUpper();
                    }
                }
                outpus.Add(new SystemCodeGenerationOutput
                {
                    TableTitle = tableTitle.ReplaceFistUpper(),
                    TableTitleOther = tableTitleOther.ReplaceFistUpper(),
                    Entities = await CreateEntity(input),
                    FindDto = await CreateFindDto(input),
                    LogicImpl = CreteLogicImpl(input),
                    Logic = CreteLogic(input),
                    Controller = CreateController(input),
                    Repository = CreteRepository(input),
                    RepositoryImpl = await CreteRepositoryImpl(input),
                    IDbContext = CreteIDbContext(input),
                    SqlDbContext = CreteSqlDbContext(input),
                });
            }

            return Ok(outpus);
        }

        /// <summary>
        ///  生成实体
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task<string> CreateEntity(SystemCodeGenerationBaseInput input)
        {
            //返回值
            var columnContent = new StringBuilder();
            //获取文件内容
            var returnContent = FileUtil.ReadFile(_hostingEnvironment.ContentRootPath + "/CodeGeneration/Entities.txt");
            //获取对应列
            var columns = (await _systemDataBaseLogic.FindDataBaseColumns(new Models.Dtos.DataBase.SystemDataBaseTableDto
            {
                DataBaseId = input.Id,
                Name = input.Table
            })).Data.ToList();
            //替换操作
            var tableSplit = input.Table.Split('_');
            //表头
            var tableTitle = tableSplit[0];
            //除开表头
            var tableTitleOther = "";
            for (int i = 0; i < tableSplit.Count(); i++)
            {
                if (i != 0)
                {
                    tableTitleOther += tableSplit[i].ReplaceFistUpper();
                }
            }
            var className = tableTitle.ReplaceFistUpper() + tableTitleOther.ReplaceFistUpper();

            var tableKey = columns.FirstOrDefault(f => f.ColumnKey == "PRI") != null ? columns.FirstOrDefault(f => f.ColumnKey == "PRI").Name : "";
            //1:命名空间
            returnContent = returnContent.Replace("{{NameSpace}}", tableTitle.ReplaceFistUpper());
            returnContent = returnContent.Replace("{{TableName}}", input.Table);
            returnContent = returnContent.Replace("{{ClassName}}", className);
            returnContent = returnContent.Replace("{{KeyName}}", tableKey);
            //5:动态生成列
            foreach (var column in columns.Where(column => column.Name != tableKey))
            {
                columnContent.Append("        /// <summary>\r\n");
                columnContent.Append($"        /// {column.Description}\r\n");
                columnContent.Append("        /// </summary>\r\n");
                columnContent.Append("        public " + ChangeToCSharpType(column.DataType, column.MaxLength, column.IsNullable) + " " + column.Name + " { get; set; }\r\n\r\n");
            }
            returnContent = returnContent.Replace("{{Body}}", columnContent.ToString());
            //描述
            returnContent = returnContent.Replace("{{Description}}", input.Description);
            returnContent = returnContent.Replace("{{CreateDateTime}}", DateTime.Now.ToString());
            return returnContent;
        }

        /// <summary>
        ///  生成查询实体
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task<string> CreateFindDto(SystemCodeGenerationBaseInput input)
        {
            //返回值
            var columnContent = new StringBuilder();
            //获取文件内容
            var returnContent = FileUtil.ReadFile(_hostingEnvironment.ContentRootPath + "/CodeGeneration/FindDto.txt");
            //获取对应列
            var columns = (await _systemDataBaseLogic.FindDataBaseColumns(new Models.Dtos.DataBase.SystemDataBaseTableDto
            {
                DataBaseId = input.Id,
                Name = input.Table
            })).Data.ToList();
            //替换操作
            var tableSplit = input.Table.Split('_');
            //表头
            var tableTitle = tableSplit[0];
            //除开表头
            var tableTitleOther = "";
            for (int i = 0; i < tableSplit.Count(); i++)
            {
                if (i != 0)
                {
                    tableTitleOther += tableSplit[i].ReplaceFistUpper();
                }
            }
            var className = tableTitle.ReplaceFistUpper() + tableTitleOther.ReplaceFistUpper();
            var tableKey = columns.FirstOrDefault(f => f.ColumnKey == "PRI") != null ? columns.FirstOrDefault(f => f.ColumnKey == "PRI").Name : "";
            //1:命名空间
            returnContent = returnContent.Replace("{{NameSpace}}", tableTitle.ReplaceFistUpper());
            returnContent = returnContent.Replace("{{ClassName}}", className);
            returnContent = returnContent.Replace("{{NameSpaceTable}}", tableTitleOther);
            //5:动态生成列
            foreach (var column in columns.Where(column => column.Name != tableKey))
            {
                columnContent.Append("        /// <summary>\r\n");
                columnContent.Append($"        /// {column.Description}\r\n");
                columnContent.Append("        /// </summary>\r\n");
                columnContent.Append("        public " + ChangeToCSharpType(column.DataType, column.MaxLength, column.IsNullable) + " " + column.Name + " { get; set; }\r\n\r\n");
            }
            returnContent = returnContent.Replace("{{Body}}", columnContent.ToString());
            //描述
            returnContent = returnContent.Replace("{{Description}}", input.Description);
            returnContent = returnContent.Replace("{{CreateDateTime}}", DateTime.Now.ToString());
            return returnContent;
        }

        /// <summary>
        /// 生成Logic接口
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string CreteLogic(SystemCodeGenerationBaseInput input)
        {
            //获取文件内容
            var returnContent = FileUtil.ReadFile(_hostingEnvironment.ContentRootPath + "/CodeGeneration/ILogic.txt");
            //替换操作
            var tableSplit = input.Table.Split('_');
            //表头
            var tableTitle = tableSplit[0];
            //除开表头
            var tableTitleOther = "";
            for (int i = 0; i < tableSplit.Count(); i++)
            {
                if (i != 0)
                {
                    tableTitleOther += tableSplit[i].ReplaceFistUpper();
                }
            }
            var className = tableTitle.ReplaceFistUpper() + tableTitleOther.ReplaceFistUpper();
            //1:命名空间
            returnContent = returnContent.Replace("{{NameSpace}}", tableTitle.ReplaceFistUpper());
            returnContent = returnContent.Replace("{{NameSpaceTable}}", tableTitleOther);
            returnContent = returnContent.Replace("{{ClassName}}", className);
            returnContent = returnContent.Replace("{{Description}}", input.Description);
            returnContent = returnContent.Replace("{{CreateDateTime}}", DateTime.Now.ToString());
            return returnContent;
        }

        /// <summary>
        /// 生成Logic接口实现
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string CreteLogicImpl(SystemCodeGenerationBaseInput input)
        {
            //获取文件内容
            var returnContent = FileUtil.ReadFile(_hostingEnvironment.ContentRootPath + "/CodeGeneration/Logic.txt");
            //替换操作
            var tableSplit = input.Table.Split('_');
            //表头
            var tableTitle = tableSplit[0];
            //除开表头
            var tableTitleOther = "";
            for (int i = 0; i < tableSplit.Count(); i++)
            {
                if (i != 0)
                {
                    tableTitleOther += tableSplit[i].ReplaceFistUpper();
                }
            }
            var className = tableTitle.ReplaceFistUpper() + tableTitleOther.ReplaceFistUpper();

            returnContent = returnContent.Replace("{{NameSpace}}", tableTitle.ReplaceFistUpper());
            returnContent = returnContent.Replace("{{NameSpaceTable}}", tableTitleOther);
            returnContent = returnContent.Replace("{{ClassName}}", className);
            returnContent = returnContent.Replace("{{ClassNameLower}}", className.ReplaceFistLower());
            returnContent = returnContent.Replace("{{Description}}", input.Description);
            returnContent = returnContent.Replace("{{CreateDateTime}}", DateTime.Now.ToString());
            return returnContent;
        }
        /// <summary>
        /// 生成IDbContext
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string CreteIDbContext(SystemCodeGenerationBaseInput input)
        {
            //获取文件内容
            var returnContent = FileUtil.ReadFile(_hostingEnvironment.ContentRootPath + "/CodeGeneration/IDbContext.txt");
            //替换操作
            var tableSplit = input.Table.Split('_');
            //表头
            var tableTitle = tableSplit[0];
            //除开表头
            var tableTitleOther = "";
            for (int i = 0; i < tableSplit.Count(); i++)
            {
                if (i != 0)
                {
                    tableTitleOther += tableSplit[i].ReplaceFistUpper();
                }
            }
            var className = tableTitle.ReplaceFistUpper() + tableTitleOther.ReplaceFistUpper();

            returnContent = returnContent.Replace("{{NameSpaceTable}}", tableTitle.ReplaceFistUpper());
            returnContent = returnContent.Replace("{{ClassName}}", className);
            returnContent = returnContent.Replace("{{Description}}", input.Description);
            returnContent = returnContent.Replace("{{CreateDateTime}}", DateTime.Now.ToString());
            return returnContent;
        }

        /// <summary>
        /// 生成SqlDbContext
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string CreteSqlDbContext(SystemCodeGenerationBaseInput input)
        {
            //获取文件内容
            var returnContent = FileUtil.ReadFile(_hostingEnvironment.ContentRootPath + "/CodeGeneration/SqlDbContext.txt");
            //替换操作
            var tableSplit = input.Table.Split('_');
            //表头
            var tableTitle = tableSplit[0];
            //除开表头
            var tableTitleOther = "";
            for (int i = 0; i < tableSplit.Count(); i++)
            {
                if (i != 0)
                {
                    tableTitleOther += tableSplit[i].ReplaceFistUpper();
                }
            }
            var className = tableTitle.ReplaceFistUpper() + tableTitleOther.ReplaceFistUpper();

            returnContent = returnContent.Replace("{{NameSpaceTable}}", tableTitle.ReplaceFistUpper());
            returnContent = returnContent.Replace("{{ClassName}}", className);
            returnContent = returnContent.Replace("{{ClassNameLower}}", className.ReplaceFistLower());
            returnContent = returnContent.Replace("{{Description}}", input.Description);
            returnContent = returnContent.Replace("{{CreateDateTime}}", DateTime.Now.ToString());
            return returnContent;
        }

        /// <summary>
        /// 生成Repository接口
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string CreteRepository(SystemCodeGenerationBaseInput input)
        {
            //获取文件内容
            var returnContent = FileUtil.ReadFile(_hostingEnvironment.ContentRootPath + "/CodeGeneration/IRepository.txt");
            //替换操作

            var tableSplit = input.Table.Split('_');
            //表头
            var tableTitle = tableSplit[0];
            //除开表头
            var tableTitleOther = "";
            for (int i = 0; i < tableSplit.Count(); i++)
            {
                if (i != 0)
                {
                    tableTitleOther += tableSplit[i].ReplaceFistUpper();
                }
            }
            var className = tableTitle.ReplaceFistUpper() + tableTitleOther.ReplaceFistUpper();
            returnContent = returnContent.Replace("{{NameSpace}}", tableTitle.ReplaceFistUpper());
            returnContent = returnContent.Replace("{{NameSpaceTable}}", tableTitleOther);
            returnContent = returnContent.Replace("{{ClassName}}", className);
            returnContent = returnContent.Replace("{{Description}}", input.Description);
            returnContent = returnContent.Replace("{{CreateDateTime}}", DateTime.Now.ToString());
            return returnContent;
        }

        /// <summary>
        /// 生成Repository接口实现
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task<string> CreteRepositoryImpl(SystemCodeGenerationBaseInput input)
        {
            //获取文件内容
            var returnContent = FileUtil.ReadFile(_hostingEnvironment.ContentRootPath + "/CodeGeneration/Repository.txt");
            //替换操作
            //1:命名空间
            var tableSplit = input.Table.Split('_');
            //表头
            var tableTitle = tableSplit[0];
            //除开表头
            var tableTitleOther = "";
            for (int i = 0; i < tableSplit.Count(); i++)
            {
                if (i != 0)
                {
                    tableTitleOther += tableSplit[i].ReplaceFistUpper();
                }
            }

            //获取对应列
            var columns = (await _systemDataBaseLogic.FindDataBaseColumns(new Models.Dtos.DataBase.SystemDataBaseTableDto
            {
                DataBaseId = input.Id,
                Name = input.Table
            })).Data.ToList();
            string filed = "\r\n                                                  " + columns.Select(s => s.Name).ExpandAndToString(",\r\n                                                  ") + ",\r\n                                                  ";
            var className = tableTitle.ReplaceFistUpper() + tableTitleOther.ReplaceFistUpper();
            returnContent = returnContent.Replace("{{NameSpace}}", tableTitle.ReplaceFistUpper());
            returnContent = returnContent.Replace("{{NameSpaceTable}}", tableTitleOther);
            returnContent = returnContent.Replace("{{ClassName}}", className);
            returnContent = returnContent.Replace("{{TableName}}", input.Table);
            returnContent = returnContent.Replace("{{Description}}", input.Description);
            returnContent = returnContent.Replace("{{CreateDateTime}}", DateTime.Now.ToString());

            returnContent = returnContent.Replace("{{Filed}}", filed);

            return returnContent;
        }


        /// <summary>
        /// 生成控制器
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private string CreateController(SystemCodeGenerationBaseInput input)
        {
            //获取文件内容
            var returnContent = FileUtil.ReadFile(_hostingEnvironment.ContentRootPath + "/CodeGeneration/Controller.txt");
            //替换操作

            //1:命名空间
            var tableSplit = input.Table.Split('_');
            //表头
            var tableTitle = tableSplit[0];
            //除开表头
            var tableTitleOther = "";
            for (int i = 0; i < tableSplit.Count(); i++)
            {
                if (i != 0)
                {
                    tableTitleOther += tableSplit[i].ReplaceFistUpper();
                }
            }
            var className = tableTitle.ReplaceFistUpper() + tableTitleOther.ReplaceFistUpper();

            returnContent = returnContent.Replace("{{NameSpace}}", tableTitle.ReplaceFistUpper());
            returnContent = returnContent.Replace("{{NameSpaceLower}}", tableTitle.ToLower());
            returnContent = returnContent.Replace("{{NameSpaceTableLower}}", tableTitleOther.ToLower());
            //2:表名
            returnContent = returnContent.Replace("{{ClassName}}", className);
            //3:表名转换为小写
            returnContent = returnContent.Replace("{{ClassNameLower}}", className.ReplaceFistLower());
            //4:控制器名称
            returnContent = returnContent.Replace("{{ControllerName}}", tableTitleOther);
            //描述
            returnContent = returnContent.Replace("{{Description}}", input.Description);
            returnContent = returnContent.Replace("{{CreateDateTime}}", DateTime.Now.ToString());
            return returnContent;
        }
        /// <summary>
        /// 数据库中与C#中的数据类型对照
        /// </summary>
        /// <param name="type"></param>
        /// <param name="maxLength"></param>
        /// <param name="isNullable"></param>
        /// <returns></returns>
        private string ChangeToCSharpType(string type, string maxLength, string isNullable)
        {
            string reval;
            switch (type.ToLower())
            {
                case "int":
                case "bigint":
                case "smallint":
                    reval = "int" + (isNullable == "√" ? "?" : "");
                    break;
                case "text":
                case "nchar":
                case "ntext":
                case "nvarchar":
                case "varchar":
                    reval = "string";
                    break;
                case "char":
                    reval = "string";
                    //判断是否为Mysql
                    var connectionType = ConfigurationUtil.GetDbConnectionType();
                    if (connectionType == ResourceDataBaseType.Mysql && maxLength == "36")
                    {
                        reval = "Guid" + (isNullable == "√" ? "?" : "");
                    }
                    break;
                case "binary":
                case "varbinary":
                case "tinyint":
                case "image":
                    reval = "byte[]" + (isNullable == "√" ? "?" : "");
                    break;
                case "bit":
                    reval = "bool" + (isNullable == "√" ? "?" : "");
                    break;
                case "datetime":
                case "smalldatetime":
                case "timestamp":
                    reval = "DateTime" + (isNullable == "√" ? "?" : "");
                    break;
                case "decimal":
                case "smallmoney":
                case "money":
                case "numeric":
                    reval = "decimal" + (isNullable == "√" ? "?" : "");
                    break;
                case "float":
                    reval = "double" + (isNullable == "√" ? "?" : "");
                    break;
                case "real":
                    reval = "System.Single";
                    break;
                case "uniqueidentifier":
                    reval = "Guid" + (isNullable == "√" ? "?" : "");
                    break;
                case "Variant":
                    reval = "Object";
                    break;
                default:
                    reval = "String";
                    break;
            }
            return reval;
        }

        #endregion

        #region 生成文件
        /// <summary>
        /// 生成文件
        /// </summary>
        /// <param name="inputs"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("/system/codegeneration/file")]
        public ActionResult CreateFile(IList<SystemCodeGenerationOutput> inputs)
        {
            OperateStatus operateStatus = new OperateStatus();
            try
            {
                var globalParams = GlobalParams.GetValuesByName();
                var basePath = globalParams.FirstOrDefault(f => f.Key == EnumConfigKey.systemCodeGenerationPath.ToString()).Value;
                foreach (var model in inputs)
                {
                    //实体
                    FileUtil.WriteFile(basePath + "\\Service\\Base\\EIP.Base.Models\\Entities\\" + model.TableTitle + "\\" + model.TableTitle + model.TableTitleOther + ".cs", model.Entities);
                    //查询实体
                    FileUtil.WriteFile(basePath + "\\Service\\" + model.TableTitle + "\\EIP." + model.TableTitle + ".Models\\Dtos\\" + model.TableTitleOther + "\\" + model.TableTitle + model.TableTitleOther + "FindDto.cs", model.FindDto);
                    //Logic接口
                    FileUtil.WriteFile(basePath + "\\Service\\" + model.TableTitle + "\\EIP." + model.TableTitle + ".Logic\\I" + model.TableTitle + model.TableTitleOther + "Logic.cs", model.Logic);
                    //Logic实现
                    FileUtil.WriteFile(basePath + "\\Service\\" + model.TableTitle + "\\EIP." + model.TableTitle + ".Logic\\Impl\\" + model.TableTitle + model.TableTitleOther + "Logic.cs", model.LogicImpl);
                    //Repository接口
                    FileUtil.WriteFile(basePath + "\\Service\\" + model.TableTitle + "\\EIP." + model.TableTitle + ".Repository\\I" + model.TableTitle + model.TableTitleOther + "Repository.cs", model.Repository);
                    //Repository实现
                    FileUtil.WriteFile(basePath + "\\Service\\" + model.TableTitle + "\\EIP." + model.TableTitle + ".Repository\\Impl\\" + model.TableTitle + model.TableTitleOther + "Repository.cs", model.RepositoryImpl);
                    //控制器
                    FileUtil.WriteFile(basePath + "\\Service\\" + model.TableTitle + "\\EIP." + model.TableTitle + ".Controller\\" + model.TableTitleOther + "Controller.cs", model.Controller);
                    //IDbContext
                    FileUtil.WriteFile(basePath + "\\Service\\Base\\EIP.Base.Repository\\Fixture\\" + model.TableTitle + "\\IDbContext.cs", model.IDbContext);
                    //SqlDbContext
                    FileUtil.WriteFile(basePath + "\\Service\\Base\\EIP.Base.Repository\\Fixture\\" + model.TableTitle + "\\SqlDbContext.cs", model.SqlDbContext);
                }
            }
            catch (Exception ex)
            {
                operateStatus.Msg = ex.Message;
                return Ok(operateStatus);
            }
            operateStatus.Msg = "生成成功";
            operateStatus.Code = ResultCode.Success;
            return Ok(operateStatus);
        }

        #endregion
    }
}