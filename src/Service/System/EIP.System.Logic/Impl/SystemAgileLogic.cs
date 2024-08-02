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
using SystemIo = System.IO;
namespace EIP.System.Logic.Impl
{
    /// <summary>
    /// 敏捷开发业务逻辑接口实现
    /// </summary>
    public class SystemAgileLogic : DapperAsyncLogic<SystemAgile>, ISystemAgileLogic
    {
        #region 构造函数
        private readonly ISystemDataBaseRepository _agileDataBaseRepository;
        private readonly ISystemAgileRepository _agileConfigRepository;
        private readonly ISystemConfigLogic _systemConfigLogic;
        /// <summary>
        /// 模块
        /// </summary>
        /// <param name="agileConfigRepository"></param>
        public SystemAgileLogic(ISystemAgileRepository agileConfigRepository, ISystemDataBaseRepository agileDataBaseRepository, ISystemConfigLogic systemConfigLogic)
        {
            _systemConfigLogic = systemConfigLogic;
            _agileConfigRepository = agileConfigRepository;
            _agileDataBaseRepository = agileDataBaseRepository;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task<OperateStatus> Save(SystemAgile input)
        {
            OperateStatus operateStatus = new OperateStatus();
            SystemAgile agileConfig = await FindAsync(f => f.ConfigId == input.ConfigId);
            var currentUser = EipHttpContext.CurrentUser();
            if (agileConfig == null)
            {
                input.CreateTime = DateTime.Now;
                input.CreateUserId = currentUser.UserId;
                input.CreateUserName = currentUser.Name;
                input.UpdateTime = DateTime.Now;
                input.UpdateUserId = currentUser.UserId;
                input.UpdateUserName = currentUser.Name;
                operateStatus = await InsertAsync(input);
            }
            else
            {
                if (input.DataFromName != agileConfig.DataFromName)
                {
                    agileConfig.SaveJson = null;
                    agileConfig.PublicJson = null;
                    agileConfig.ColumnJson = null;
                }
                agileConfig.DataBaseId = input.DataBaseId;
                agileConfig.OrderNo = input.OrderNo;
                agileConfig.Name = input.Name;
                agileConfig.DataFrom = input.DataFrom;
                agileConfig.DataFromName = input.DataFromName;
                agileConfig.IsFreeze = input.IsFreeze;
                agileConfig.Remark = input.Remark;
                agileConfig.EditConfigId = input.EditConfigId;
                agileConfig.FormCategory = input.FormCategory;
                agileConfig.FormPcUrl = input.FormPcUrl;
                agileConfig.FormMobileUrl = input.FormMobileUrl;
                agileConfig.UpdateTime = DateTime.Now;
                agileConfig.UpdateUserId = currentUser.UserId;
                agileConfig.UpdateUserName = currentUser.Name;

                operateStatus = await UpdateAsync(agileConfig);
            }
            if (operateStatus.Code == ResultCode.Success)
            {
                operateStatus.Msg = "保存成功";
            }
            return operateStatus;
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task<OperateStatus> SaveType(SystemAgile input)
        {
            OperateStatus operateStatus = new OperateStatus();
            SystemAgile agileConfig = await FindAsync(f => f.ConfigId == input.ConfigId);
            agileConfig.Name = input.Name;
            agileConfig.OrderNo = input.OrderNo;
            agileConfig.Remark = input.Remark;
            operateStatus = await UpdateAsync(agileConfig);
            var allSystemAgile = await FindAllAsync(f => f.MenuId == agileConfig.MenuId);
            foreach (var item in allSystemAgile)
            {
                item.DataBaseId = input.DataBaseId;
                operateStatus = await UpdateAsync(item);
            }
            if (operateStatus.Code == ResultCode.Success)
            {
                operateStatus.Msg = "保存成功";
            }
            return operateStatus;
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> SaveJson(SystemAgile input)
        {
            return await UpdateAsync(u => u.ConfigId == input.ConfigId, new { SaveJson = input.SaveJson, UpdateTime = DateTime.Now });
        }

        /// <summary>
        /// 保存缩略图
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> SaveThumbnail(SystemAgile input)
        {
            string hearderpath = "";
            var filePath = ConfigurationUtil.GetTempPath() + $"/{Guid.NewGuid()}.png";
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
                //删除临时文件
                FileUtil.DeleteFile(filePath);
            }
            return await UpdateAsync(u => u.ConfigId == input.ConfigId, new { Thumbnail = hearderpath, UpdateTime = DateTime.Now });
        }

        /// <summary>
        /// 发布
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> PublicJson(SystemAgile input)
        {
            return await UpdateAsync(u => u.ConfigId == input.ConfigId, new { SaveJson = input.SaveJson, PublicJson = input.PublicJson, ColumnJson = input.ColumnJson, UpdateTime = DateTime.Now });
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        public async Task<OperateStatus<PagedResults<SystemAgileFindOutput>>> Find(SystemAgileFindInput paging)
        {
            return OperateStatus<PagedResults<SystemAgileFindOutput>>.Success(await _agileConfigRepository.Find(paging));
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
        /// 获取
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        public async Task<OperateStatus<SystemAgile>> FindById(IdInput input)
        {
            return OperateStatus<SystemAgile>.Success(await FindAsync(f => f.ConfigId == input.Id));
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        public async Task<OperateStatus<SystemAgile>> FindPublicJsonById(IdInput input, bool searchPublicJson = false)
        {
            SystemAgile agileConfig = new SystemAgile();
            using (var fix = new SqlDatabaseFixture())
            {
                if (searchPublicJson)
                {
                    agileConfig = await fix.Db.SystemAgile.SetSelect(s => new { s.EditConfigId, s.DataBaseId, s.DataFrom, s.DataFromName, s.PublicJson }).FindAsync(f => f.ConfigId == input.Id);
                }
                else
                {
                    agileConfig = await fix.Db.SystemAgile.SetSelect(s => new { s.EditConfigId, s.DataBaseId, s.DataFrom, s.DataFromName }).FindAsync(f => f.ConfigId == input.Id);
                }
            }
            return OperateStatus<SystemAgile>.Success(agileConfig);
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        public async Task<OperateStatus<SystemAgile>> FindColumnJsonJsonById(IdInput input)
        {
            SystemAgile agileConfig = new SystemAgile();
            using (var fix = new SqlDatabaseFixture())
            {
                agileConfig = await fix.Db.SystemAgile.SetSelect(s => new { s.EditConfigId, s.DataBaseId, s.DataFrom, s.DataFromName, s.ColumnJson }).FindAsync(f => f.ConfigId == input.Id);
            }
            return OperateStatus<SystemAgile>.Success(agileConfig);
        }

        /// <summary>
        /// 根据菜单获取
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemAgileFindByMenuIdOutput>>> FindByMenuId(SystemAgileFindByMenuIdInput input)
        {
            List<SystemAgileFindByMenuIdOutput> outputs = new List<SystemAgileFindByMenuIdOutput>();
            using (var fix = new SqlDatabaseFixture())
            {
                var agile = await fix.Db.SystemAgile.SetSelect(s => new { s.DataBaseId, s.DataFromName, s.ConfigId, s.Name, s.ConfigType }).FindAllAsync(f => f.MenuId == input.MenuId);
                if (input.ConfigType == EnumAgileConfigType.列表配置.ToShort())
                {
                    foreach (var item in agile.ToList().Where(w => w.ConfigType == input.ConfigType))
                    {
                        SystemAgileFindByMenuIdOutput findByMenuIdOutput = new SystemAgileFindByMenuIdOutput()
                        {
                            ConfigId = item.ConfigId,
                            Name = item.Name,
                        };
                        outputs.Add(findByMenuIdOutput);
                    }
                }
                if (input.ConfigType == EnumAgileConfigType.表单配置.ToShort() || input.ConfigType == EnumAgileConfigType.移动端自定义.ToShort())
                {
                    var menu = await fix.Db.SystemMenu.SetSelect(s => new { s.Icon, s.Theme }).FindAsync(f => f.MenuId == input.MenuId);
                    foreach (var item in agile.ToList().Where(w => w.ConfigType == input.ConfigType))
                    {
                        SystemAgileFindByMenuIdOutput findByMenuIdOutput = new SystemAgileFindByMenuIdOutput()
                        {
                            DataBaseId = item.DataBaseId,
                            ConfigId = item.ConfigId,
                            Name = item.Name,
                            Icon = menu.Icon,
                            Theme = menu.Theme,
                            DataFromName = item.DataFromName,
                        };
                        outputs.Add(findByMenuIdOutput);
                    }
                }
            }
            return OperateStatus<IEnumerable<SystemAgileFindByMenuIdOutput>>.Success(outputs);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> Delete(IdInput<string> input)
        {
            foreach (var id in input.Id.Split(','))
            {
                var configId = Guid.Parse(id);
                await UpdateAsync(u => u.ConfigId == configId, new { IsDelete = true });
            }
            return OperateStatus.Success();
        }

        /// <summary>
        /// 获取基础配置信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemAgileFindBaseOutput>>> FindBase(SystemAgileFindBaseInput input)
        {
            return OperateStatus<IEnumerable<SystemAgileFindBaseOutput>>.Success(await _agileConfigRepository.FindBase(input));
        }

        /// <summary>
        /// 获取基础配置信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<SystemAgileFindColumnJsonDataFromNameOutput>> FindColumnJsonByDataFromName(SystemAgileFindColumnJsonDataFromNameInput input)
        {
            OperateStatus<SystemAgileFindColumnJsonDataFromNameOutput> operateStatus = new OperateStatus<SystemAgileFindColumnJsonDataFromNameOutput>();
            using (var fix = new SqlDatabaseFixture())
            {
                var config = await fix.Db.SystemAgile.SetSelect(s => new { s.ColumnJson, s.MenuId }).FindAsync(f => f.DataFromName == input.DataFromName && f.ConfigType == EnumAgileConfigType.表单配置.ToShort());
                if (config != null && config.ColumnJson.IsNotNullOrEmpty())
                {
                    operateStatus.Msg = "存在配置表";
                    operateStatus.Code = ResultCode.Success;
                    operateStatus.Data = new SystemAgileFindColumnJsonDataFromNameOutput
                    {
                        ColumnJson = config.ColumnJson,
                        MenuId = config.MenuId
                    };
                    return operateStatus;
                }
            }
            operateStatus.Msg = "非配置表";
            return operateStatus;
        }

        /// <summary>
        /// 获取基础配置信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<SystemAgileFindColumnJsonByMenuIdOutput>> FindColumnJsonByMenuId(SystemAgileFindColumnJsonByMenuIdInput input)
        {
            OperateStatus<SystemAgileFindColumnJsonByMenuIdOutput> operateStatus = new OperateStatus<SystemAgileFindColumnJsonByMenuIdOutput>();
            using (var fix = new SqlDatabaseFixture())
            {
                var config = await fix.Db.SystemAgile.SetSelect(s => new { s.ColumnJson, s.DataFrom, s.DataFromName, s.DataBaseId }).FindAsync(f => f.MenuId == input.MenuId && f.ConfigType == EnumAgileConfigType.表单配置.ToShort());
                if (config != null && config.ColumnJson.IsNotNullOrEmpty())
                {
                    operateStatus.Msg = "存在配置表";
                    operateStatus.Code = ResultCode.Success;
                    operateStatus.Data = new SystemAgileFindColumnJsonByMenuIdOutput
                    {
                        ColumnJson = config.ColumnJson,
                        DataBaseId = config.DataBaseId,
                        DataFrom = config.DataFrom,
                        DataFromName = config.DataFromName
                    };
                    return operateStatus;
                }
            }
            operateStatus.Msg = "非配置表";
            return operateStatus;
        }

        /// <summary>
        /// 根据关键词查询相关发布设计内容
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<List<SystemAgileFindByKeyOutput>>> Key(SystemAgileFindByKeyInput input)
        {
            OperateStatus<List<SystemAgileFindByKeyOutput>> operateStatus = new OperateStatus<List<SystemAgileFindByKeyOutput>>();
            using (var fix = new SqlDatabaseFixture())
            {
                var data = await fix.Db.SystemAgile.SetSelect(s => new { s.MenuId, s.Name, s.ConfigType, s.ConfigId, s.Remark, s.IsFreeze, s.IsDelete, s.CreateTime, s.CreateUserName, s.UpdateTime, s.UpdateUserName }).FindAllAsync(f => f.PublicJson.Contains(input.Key));
                operateStatus.Code = ResultCode.Success;
                operateStatus.Msg = Chs.QuerySuccessful;
                operateStatus.Data = data.MapToList<SystemAgile, SystemAgileFindByKeyOutput>();
                return operateStatus;
            }
        }
        /// <summary>
        /// 获取表单字段,若为第三方则检测表名字段是否具有,若有则读取对于的表字段
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<List<SystemAgileFindFormColumnsOutput>>> FindFormColumns(IdInput input)
        {
            List<SystemAgileFindFormColumnsOutput> outputs = new List<SystemAgileFindFormColumnsOutput>();
            var config = await FindAsync(f => f.ConfigId == input.Id);
            if (config != null)
            {
                if (config.ColumnJson.IsNotNullOrEmpty())
                {
                    var columns = config.ColumnJson.JsonStringToList<SystemAgileFindFormColumnsJsonOutput>();
                    foreach (var item in columns)
                    {
                        SystemAgileFindFormColumnsOutput formColumnsOutput = new SystemAgileFindFormColumnsOutput
                        {
                            Name = item.Model,
                            Description = item.Label,
                            Type = item.Type
                        };
                        outputs.Add(formColumnsOutput);
                    }
                    return OperateStatus<List<SystemAgileFindFormColumnsOutput>>.Success(outputs);
                }
                //是否具有表名
                if (config.DataFromName.IsNotNullOrEmpty())
                {
                    var columns = await _agileDataBaseRepository.FindDataBaseColumns(new SystemDataBaseTableDto { Name = config.DataFromName });
                    foreach (var item in columns)
                    {
                        SystemAgileFindFormColumnsOutput formColumnsOutput = new SystemAgileFindFormColumnsOutput
                        {
                            Name = item.Name,
                            Description = item.Description,
                            Type = item.Type
                        };
                        outputs.Add(formColumnsOutput);
                    }
                }
            }

            return OperateStatus<List<SystemAgileFindFormColumnsOutput>>.Success(outputs);
        }
        #endregion
    }
}