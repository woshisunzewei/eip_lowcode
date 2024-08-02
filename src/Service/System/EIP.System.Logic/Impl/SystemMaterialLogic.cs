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
using EIP.System.Models.Dtos.Material;
namespace EIP.System.Logic.Impl
{
    /// <summary>
    /// 素材业务逻辑接口实现
    /// </summary>
    public class SystemMaterialLogic : DapperAsyncLogic<SystemMaterial>, ISystemMaterialLogic
    {
        #region 构造函数
        private readonly ISystemMaterialRepository _systemTypeRepository;
        private readonly ISystemConfigLogic _systemConfigLogic;
        /// <summary>
        /// 模块
        /// </summary>
        /// <param name="systemTypeRepository"></param>
        public SystemMaterialLogic(ISystemMaterialRepository systemTypeRepository,
            ISystemConfigLogic systemConfigLogic)
        {
            _systemTypeRepository = systemTypeRepository;
            _systemConfigLogic = systemConfigLogic;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="input">实体</param>
        /// <returns></returns>
        public async Task<OperateStatus> Save(SystemMaterialSaveInput input)
        {
            OperateStatus operateStatus = new OperateStatus();
            SystemMaterial systemMaterial = await FindAsync(f => f.MaterialId == input.MaterialId);
            var currentUser = EipHttpContext.CurrentUser();
            if (systemMaterial == null)
            {
                var filesStorageOptions = (await _systemConfigLogic.FindFilesStorageOptions()).Data;
                OssUtil ossUtil = new OssUtil();
                foreach (var file in input.Files)
                {
                    var msg = ossUtil.UpLoadFileCheck(filesStorageOptions, file);
                    if (msg.IsNotNullOrEmpty())
                    {
                        operateStatus.Msg = msg;
                        return operateStatus;
                    }
                }
                //上传文件
                input.Path = await Upload(input, filesStorageOptions);
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
                systemMaterial.Name = input.Name;
                systemMaterial.UpdateTime = DateTime.Now;
                systemMaterial.UpdateUserId = currentUser.UserId;
                systemMaterial.UpdateUserName = currentUser.Name;
                operateStatus = await UpdateAsync(systemMaterial);
            }
            if (operateStatus.Code == ResultCode.Success)
            {
                operateStatus.Msg = "保存成功";
            }
            return operateStatus;
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        public async Task<OperateStatus<PagedResults<SystemMaterial>>> Find(SystemMaterialFindInput paging)
        {
            paging.Size = int.MaxValue;
            return OperateStatus<PagedResults<SystemMaterial>>.Success(await _systemTypeRepository.Find(paging));
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        public async Task<OperateStatus<SystemMaterial>> FindById(IdInput input)
        {
            return OperateStatus<SystemMaterial>.Success(await FindAsync(f => f.MaterialId == input.Id));
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
                var config = await FindAsync(f => f.MaterialId == configId);
                config.IsDelete = true;
                await UpdateAsync(config);
            }
            return OperateStatus.Success();
        }

        /// <summary>
        /// 上传
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task<string> Upload(SystemMaterialSaveInput input, FilesStorageOptions filesStorageOptions)
        {
            string materialPath = "";
            try
            {
                var files = input.Files;
                if (files.Any())
                {
                    OssUtil ossUtil = new OssUtil();
                    foreach (var file in files)
                    {
                        var fileName = file.FileName;
                        var fileExt = Path.GetExtension(fileName).ToLowerInvariant();
                        materialPath =await ossUtil.UpLoadFile(filesStorageOptions, fileExt, file);
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return materialPath;
        }
        #endregion
    }
}