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
using EIP.Workflow.Repository;
using Microsoft.AspNetCore.Http;
using SystemIo = System.IO;
namespace EIP.Workflow.Logic.Impl
{
    /// <summary>
    /// 工作流处理界面按钮接口实现
    /// </summary>
    public class WorkflowProcessLogic : DapperAsyncLogic<WorkflowProcess>, IWorkflowProcessLogic
    {
        #region 构造函数

        private readonly IWorkflowProcessRepository _processRepository;
        private readonly ISystemConfigLogic _systemConfigLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="processRepository"></param>
        /// <param name="systemDictionaryRepository"></param>
        public WorkflowProcessLogic(IWorkflowProcessRepository processRepository, ISystemConfigLogic systemConfigLogic)
        {
            _systemConfigLogic = systemConfigLogic;
            _processRepository = processRepository;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 根据流程类型获取所有流程
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<PagedResults<WorkflowProcessFindOutput>>> Find(WorkflowProcessFindInput input)
        {
            return OperateStatus<PagedResults<WorkflowProcessFindOutput>>.Success(await _processRepository.Find(input));
        }

        /// <summary>
        /// 获取历史版本
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<WorkflowProcess>>> FindVersion(IdInput input)
        {
            return OperateStatus<IEnumerable<WorkflowProcess>>.Success(await FindAllAsync(f => f.ProcessParentId == input.Id));
        }

        /// <summary>
        /// 删除表单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> Delete(IdInput<string> input)
        {
            OperateStatus operateStatus = new OperateStatus();
            foreach (var item in input.Id.Split(","))
            {
                var processId = Guid.Parse(item);
                operateStatus = await UpdateAsync(u => u.ProcessId == processId, new { IsDelete = true });
            }
            return operateStatus;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        public async Task<OperateStatus> Save(WorkflowProcess process)
        {
            OperateStatus operateStatus = new OperateStatus();
            var workflowProcess = await FindAsync(f => f.ProcessId == process.ProcessId);
            var currentUser = EipHttpContext.CurrentUser();
            if (workflowProcess == null)
            {
                process.Version = (decimal)1.0;
                process.CreateTime = DateTime.Now;
                process.CreateUserId = currentUser.UserId;
                process.CreateUserName = currentUser.Name;
                process.UpdateTime = DateTime.Now;
                process.UpdateUserId = currentUser.UserId;
                process.UpdateUserName = currentUser.Name;

                process.SaveJson = process.SaveJson.Replace("{0}", process.Name);
                process.SaveJson = process.SaveJson.Replace("{1}", CombUtil.NewComb().ToString());
                process.SaveJson = process.SaveJson.Replace("{2}", CombUtil.NewComb().ToString());
                return await InsertAsync(process);

            }
            process.Id = workflowProcess.Id;
            process.Version = workflowProcess.Version;
            process.CreateTime = workflowProcess.CreateTime;
            process.CreateUserId = workflowProcess.CreateUserId;
            process.CreateUserName = workflowProcess.CreateUserName;
            process.UpdateTime = DateTime.Now;
            process.UpdateUserId = process.CreateUserId;
            process.UpdateUserName = process.CreateUserName;
            process.PublicJson = workflowProcess.PublicJson;
            process.SaveJson = workflowProcess.SaveJson;
            return await UpdateAsync(process);
        }
        /// 根据数据库Id返回对应数据库连接
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task<SystemDataBaseInput> GetSystemDataBase(IdInput input)
        {
            var cacheKey = $"ISystemDataBaseLogic_Cache:{input.Id}";
            var dataBase = await RedisHelper.CacheShellAsync(cacheKey, DateTimeUtil.TotalSeconds(20), async () =>
            {
                using (var fix = new SqlDatabaseFixture())
                {
                    return await fix.Db.SystemDataBase.FindAsync(f => f.DataBaseId == input.Id);
                }

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
        /// 复制
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        public async Task<OperateStatus> Copy(WorkflowProcess process)
        {
            using (var fixture = new SqlDatabaseFixture())
            {
                var workflowProcess = await FindByIdAsync(process.ProcessId);
                process.CreateTime = DateTime.Now;
                process.ProcessId = CombUtil.NewComb();
                var activitys =
                    (await fixture.Db.WorkflowProcessActivity.FindAllAsync(f => f.ProcessId == process.ProcessId))
                    .ToList();
                var links = (await fixture.Db.WorkflowProcessLink.FindAllAsync(f => f.ProcessId == process.ProcessId))
                    .ToList();

                foreach (var activity in activitys)
                {
                    activity.ProcessId = process.ProcessId;
                }

                foreach (var line in links)
                {
                    line.ProcessId = process.ProcessId;
                }

                if (activitys.Any())
                    await fixture.Db.WorkflowProcessActivity.BulkInsertAsync(activitys);
                if (links.Any())
                    await fixture.Db.WorkflowProcessLink.BulkInsertAsync(links);
                process.Version = (decimal)1.0;
                process.SaveJson = workflowProcess.SaveJson;
                process.PublicJson = workflowProcess.PublicJson;
                return await InsertAsync(process);
            }
        }

        /// <summary>
        /// 保存流程设计图
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        public async Task<OperateStatus> SaveDesignJson(WorkflowProcessSaveInput process)
        {
            return process.IsNewVersion ? await SaveNewDesignJson(process) : await SaveUpdateNewDesignJson(process);
        }

        /// <summary>
        /// 更新流程设计图
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        private async Task<OperateStatus> SaveUpdateNewDesignJson(WorkflowProcessSaveInput process)
        {
            using (var fixture = new SqlDatabaseFixture())
            {
                var operateStatus = new OperateStatus();
                var workflowProcess = await FindAsync(f => f.ProcessId == process.ProcessId);
                try
                {
                    bool insert = workflowProcess == null;
                    if (insert)
                    {
                        workflowProcess = new WorkflowProcess();
                        workflowProcess.ProcessId = process.ProcessId;
                        workflowProcess.Name = process.Name;
                        workflowProcess.Icon = process.Icon;
                        workflowProcess.CreateTime = DateTime.Now;
                        workflowProcess.CreateUserId = process.UpdateUserId;
                        workflowProcess.CreateUserName = process.UpdateUserName;
                        workflowProcess.Theme = process.Theme;
                        workflowProcess.Version = process.Version;
                        workflowProcess.ShowLibrary = process.ShowLibrary;
                        workflowProcess.FormId = process.FormId;
                        workflowProcess.Sn = process.Sn;
                    }
                    workflowProcess.SaveJson = process.DesignJson;
                    if (process.IsPublic)
                    {
                        if (workflowProcess.PublicJson.IsNullOrEmpty())
                        {
                            var menu = await fixture.Db.SystemMenu.FindAsync(f => f.MenuId == process.ProcessId);
                            if (menu != null)
                            {
                                var agile = await fixture.Db.SystemAgile.FindAsync(f => f.MenuId == menu.MenuId && f.EditConfigId != null);
                                var dataBase = await GetSystemDataBase(new IdInput { Id = agile.DataBaseId });
                                string connectionType = dataBase.ConnectionType;
                                string connectionString = dataBase.ConnectionString;
                                //判断是否存在字段
                                switch (connectionType)
                                {
                                    case ResourceDataBaseType.Mysql:
                                        new DbHelper(connectionString, connectionType).ExecuteSql($"" +
                                            $"ALTER TABLE {agile.DataFromName} ADD WorkflowStatus smallint NULL COMMENT '状态 -1待发起' FIRST;" +
                                            $"ALTER TABLE {agile.DataFromName} ADD WorkflowSn varchar(256) NULL COMMENT '流水号' FIRST;" +
                                            $"ALTER TABLE {agile.DataFromName} ADD WorkflowTitle varchar(1024) NULL COMMENT '标题' FIRST ");
                                        break;
                                    case ResourceDataBaseType.Postgresql:
                                        break;
                                    case ResourceDataBaseType.Dm:
                                        new DbHelper(connectionString, connectionType).ExecuteSql(($"ALTER TABLE {agile.DataFromName} ADD WorkflowStatus smallint NULL"));
                                        new DbHelper(connectionString, connectionType).ExecuteSql(($"ALTER TABLE {agile.DataFromName} ADD WorkflowSn varchar(256) NULL"));
                                        new DbHelper(connectionString, connectionType).ExecuteSql($"ALTER TABLE {agile.DataFromName} ADD WorkflowTitle varchar(1024) NULL");
                                        new DbHelper(connectionString, connectionType).ExecuteSql(($"COMMENT ON COLUMN {agile.DataFromName}.WorkflowStatus IS '状态 -1待发起'"));
                                        new DbHelper(connectionString, connectionType).ExecuteSql(($"COMMENT ON COLUMN {agile.DataFromName}.WorkflowSn  IS '流水号'"));
                                        new DbHelper(connectionString, connectionType).ExecuteSql($"COMMENT ON COLUMN {agile.DataFromName}.WorkflowTitle  IS '标题'");
                                        break;
                                    default:
                                        string fileds = $"ALTER TABLE {agile.DataFromName} ADD WorkflowStatus smallint NULL,WorkflowSn varchar(256) NULL,WorkflowTitle varchar(1024) NULL";
                                        new DbHelper(connectionString, connectionType).ExecuteSql(fileds);

                                        string description = 
                                            $" execute sp_addextendedproperty 'MS_Description','状态 -1待发起','user', 'dbo', 'table', '{agile.DataFromName}', 'column', 'WorkflowStatus' " +
                                            $" execute sp_addextendedproperty 'MS_Description','流水号','user', 'dbo', 'table', '{agile.DataFromName}', 'column', 'WorkflowSn'" +
                                            $" execute sp_addextendedproperty 'MS_Description','标题','user', 'dbo', 'table', '{agile.DataFromName}', 'column', 'WorkflowTitle' ";
                                        new DbHelper(connectionString, connectionType).ExecuteSql(description);
                                        break;
                                }
                            }
                        }
                        workflowProcess.PublicJson = process.DesignJson;
                    }
                    workflowProcess.UpdateUserId = process.UpdateUserId;
                    workflowProcess.UpdateUserName = process.UpdateUserName;
                    workflowProcess.UpdateTime = process.UpdateTime;
                    await fixture.Db.WorkflowProcessActivity.DeleteAsync(d => d.ProcessId == process.ProcessId);
                    await fixture.Db.WorkflowProcessLink.DeleteAsync(d => d.ProcessId == process.ProcessId);

                    if (process.Activities.Any())
                    {
                        IList<WorkflowProcessActivity> activities = new List<WorkflowProcessActivity>();
                        foreach (var activity in process.Activities)
                        {
                            activities.Add(new WorkflowProcessActivity
                            {
                                ActivityId = activity.ActivityId,
                                ProcessId = process.ProcessId,
                                FormId = activity.Base.FormId,
                                Type = activity.Type,
                                Title = activity.Title,
                                Json = activity.Json,
                            });
                        }

                        await fixture.Db.WorkflowProcessActivity.BulkInsertAsync(activities);
                    }

                    if (process.Links != null && process.Links.Any())
                    {
                        foreach (var link in process.Links)
                        {
                            link.ProcessId = process.ProcessId;
                        }
                        await fixture.Db.WorkflowProcessLink.BulkInsertAsync(process.Links);
                    }

                    operateStatus = insert ? await InsertAsync(workflowProcess) : await UpdateAsync(workflowProcess);
                }
                catch (Exception exception)
                {
                    operateStatus.Msg = exception.Message;
                }

                return operateStatus;
            }
        }

        /// <summary>
        /// 保存新流程设计图
        ///     1、新建一条流程老版本记录
        ///     2、更新老流程版本号及流程设计图
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        private async Task<OperateStatus> SaveNewDesignJson(WorkflowProcessSaveInput process)
        {
            using (var fixture = new SqlDatabaseFixture())
            {
                var operateStatus = new OperateStatus();
                var workflowProcess = await FindAsync(f => f.ProcessId == process.ProcessId);
                try
                {
                    bool insert = workflowProcess == null;
                    if (insert)
                    {
                        workflowProcess = new WorkflowProcess();
                        workflowProcess.ProcessId = process.ProcessId;
                        workflowProcess.Name = process.Name;
                        workflowProcess.Icon = process.Icon;
                        workflowProcess.CreateTime = DateTime.Now;
                        workflowProcess.CreateUserId = process.UpdateUserId;
                        workflowProcess.CreateUserName = process.UpdateUserName;
                        workflowProcess.Theme = process.Theme;
                        workflowProcess.Version = process.Version;
                        workflowProcess.ShowLibrary = process.ShowLibrary;
                        workflowProcess.FormId = process.FormId;
                        workflowProcess.Sn = process.Sn;
                        workflowProcess.SaveJson = process.DesignJson;
                        if (process.IsPublic)
                        {
                            workflowProcess.PublicJson = process.DesignJson;
                        }
                    }
                    #region 处理老版本

                    workflowProcess.ProcessId = CombUtil.NewComb();
                    workflowProcess.ProcessParentId = process.ProcessId;
                    //获取活动和连线
                    var allActivitys = (await fixture.Db.WorkflowProcessActivity.FindAllAsync(f => f.ProcessId == process.ProcessId)).ToList();
                    var allLinks = (await fixture.Db.WorkflowProcessLink.FindAllAsync(f => f.ProcessId == process.ProcessId)).ToList();
                    if (allActivitys.Any())
                    {
                        foreach (var item in allActivitys)
                        {
                            item.ActivityId = CombUtil.NewComb();
                            item.ProcessId = workflowProcess.ProcessId;
                        }

                        await fixture.Db.WorkflowProcessActivity.BulkInsertAsync(allActivitys);
                    }

                    if (allLinks.Any())
                    {
                        foreach (var item in allLinks)
                        {
                            item.LinkId = CombUtil.NewComb();
                            item.ProcessId = workflowProcess.ProcessId;
                        }

                        await fixture.Db.WorkflowProcessLink.BulkInsertAsync(allLinks);
                    }

                    var result = await InsertAsync(workflowProcess);

                    #endregion
                    workflowProcess = await FindAsync(f => f.ProcessId == process.ProcessId);
                    //更新
                    workflowProcess.SaveJson = process.DesignJson;
                    if (process.IsPublic)
                    {
                        workflowProcess.PublicJson = process.DesignJson;
                    }
                    workflowProcess.ProcessId = process.ProcessId;
                    workflowProcess.ProcessParentId = null;
                    workflowProcess.UpdateUserId = process.UpdateUserId;
                    workflowProcess.UpdateUserName = process.UpdateUserName;
                    workflowProcess.UpdateTime = process.UpdateTime;
                    workflowProcess.Version = workflowProcess.Version + (decimal)0.1;
                    await fixture.Db.WorkflowProcessActivity.DeleteAsync(d => d.ProcessId == process.ProcessId);
                    await fixture.Db.WorkflowProcessLink.DeleteAsync(d => d.ProcessId == process.ProcessId);

                    if (process.Activities.Any())
                    {
                        IList<WorkflowProcessActivity> activities = new List<WorkflowProcessActivity>();
                        foreach (var activity in process.Activities)
                        {
                            activities.Add(new WorkflowProcessActivity()
                            {
                                ActivityId = activity.ActivityId,
                                ProcessId = process.ProcessId,
                                Type = activity.Type,
                                FormId = activity.Base.FormId,
                                Title = activity.Title,
                                Json = activity.Json,
                            });
                        }
                        await fixture.Db.WorkflowProcessActivity.BulkInsertAsync(activities);
                    }

                    if (process.Links != null && process.Links.Any())
                    {
                        await fixture.Db.WorkflowProcessLink.BulkInsertAsync(process.Links);
                    }

                    operateStatus = insert ? await InsertAsync(workflowProcess) : await UpdateAsync(workflowProcess);
                }
                catch (Exception exception)
                {
                    operateStatus.Msg = exception.Message;
                }

                return operateStatus;
            }
        }

        /// <summary>
        /// 返回所有
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<IList<BaseTree>>> FindAll(IdInput input)
        {
            using (var fixture = new SqlDatabaseFixture())
            {
                IList<BaseTree> entities = new List<BaseTree>();
                //string parentIds = input.Id + ",";
                //var allProcess = (await _processRepository.FindAllProcess()).ToList();
                //foreach (var type in types)
                //{
                //    entities.Add(new BaseTree
                //    {
                //        id = type.TypeId,
                //        text = type.Name,
                //        disableCheckbox = true
                //    });
                //    var process = allProcess.Where(w => w.TypeId == type.TypeId);
                //    foreach (var pro in process)
                //    {
                //        entities.Add(new BaseTree
                //        {
                //            parent = type.TypeId,
                //            id = pro.ProcessId,
                //            text = pro.Name,
                //            icon = pro.Icon
                //        });
                //    }
                //}

                return OperateStatus<IList<BaseTree>>.Success(entities);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<WorkflowProcessFindByIdOutput>> FindById(IdInput input)
        {
            var data = (await FindAsync(f => f.ProcessId == input.Id)).MapTo<WorkflowProcess, WorkflowProcessFindByIdOutput>();
            return OperateStatus<WorkflowProcessFindByIdOutput>.Success(data);
        }

        /// <summary>
        /// 保存缩略图
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> SaveThumbnail(WorkflowProcessSaveThumbnailInput input)
        {
            //根据数据生成文件
            string hearderpath = "";
            var filePath = Path.Combine(ConfigurationUtil.GetTempPath(), $"{Guid.NewGuid()}.png");
            //转换并保存图片
            input.Thumbnail.ConvertBase64ToImage(filePath);
            var filesStorageOptions = (await _systemConfigLogic.FindFilesStorageOptions()).Data;
            var FileBytes = SystemIo.File.ReadAllBytes(filePath);
            using (var ms = new SystemIo.MemoryStream(FileBytes))
            {
                IFormFile file = new FormFile(ms, 0, ms.Length,
                    SystemIo.Path.GetFileNameWithoutExtension(filePath),
                    SystemIo.Path.GetFileName(filePath));
                var fileName = file.FileName;
                var fileExt = SystemIo.Path.GetExtension(fileName).ToLowerInvariant();
                OssUtil ossUtil = new OssUtil();
                hearderpath = await ossUtil.UpLoadFile(filesStorageOptions, fileExt, file);
                FileUtil.DeleteFile(filePath);
            }
            return await UpdateAsync(u => u.ProcessId == input.ProcessId, new { Thumbnail = hearderpath, UpdateTime = DateTime.Now });
        }
        #endregion
    }
}