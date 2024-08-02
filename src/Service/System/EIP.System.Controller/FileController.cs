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
using System.ComponentModel;
using System.IO;
using System.Net.Http;
using SystemIo = System.IO;
namespace EIP.System.Controller
{
    /// <summary>
    /// 文件存储表
    /// </summary>
    public class FileController : BaseSystemController
    {
        #region 构造函数
        private readonly ISystemFileLogic _systemFileLogic;
        private readonly ISystemConfigLogic _systemConfigLogic;
        /// <summary>
        /// 文件存储表构造函数
        /// </summary>
        /// <param name="systemFileLogic"></param>
        /// <param name="systemConfigLogic"></param>
        public FileController(ISystemFileLogic systemFileLogic,
            ISystemConfigLogic systemConfigLogic)
        {
            _systemFileLogic = systemFileLogic;
            _systemConfigLogic = systemConfigLogic;
        }

        #endregion

        #region 方法
        /// <summary>
        /// 一次性获取
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Description("文件存储表-方法-列表-一次性获取")]
        [Route("/system/file/query")]
        public async Task<ActionResult> FindLoadonce()
        {
            return Ok(await _systemFileLogic.FindAllAsync());
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="input">主键信息</param>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Description("文件存储表-方法-编辑-根据Id获取")]
        [Route("/system/file/{id}")]
        public async Task<ActionResult> FindById([FromRoute] IdInput input)
        {
            return Ok(await _systemFileLogic.FindByIdAsync(input.Id));
        }

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="input">主键信息</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("文件存储表-方法-编辑-保存")]
        [Route("/system/file/save")]
        public async Task<ActionResult> Save(SystemFile input)
        {
            return Ok(await _systemFileLogic.Save(input));
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="input">主键集合</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Description("文件存储表-方法-列表-删除")]
        [Route("/system/file/{id}")]
        public async Task<ActionResult> Delete(IdInput<string> input)
        {
            return Ok(await _systemFileLogic.DeleteByIdsAsync(input.Id));
        }

        /// <summary>
        /// 获取文件
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Route("/system/file/ids")]
        public async Task<ActionResult> FindFile([FromRoute] IdInput<string> input)
        {
            return Ok(await _systemFileLogic.FindFile(input));
        }

        /// <summary>
        /// 获取文件
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Route("/system/file/correlation/{id}")]
        public async Task<ActionResult> FindFileByCorrelationId([FromRoute] IdInput<string> input)
        {
            return Ok(await _systemFileLogic.FindFileByCorrelationId(input));
        }

        /// <summary>
        /// 获取文件
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Route("/system/file/image")]
        public async Task<ActionResult> FindImage([FromRoute] IdInput input)
        {
            OperateStatus<string> operateStatus = new OperateStatus<string>();
            var file = await _systemFileLogic.FindByIdAsync(input.Id);
            if (file != null)
            {
                operateStatus.Data = file.Path;
                operateStatus.Msg = Chs.Successful;
                operateStatus.Code = ResultCode.Success;
            }
            return Ok(operateStatus);
        }

        /// <summary>
        /// 下载文件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Route("/system/file/download")]
        public async Task<IActionResult> DownLoadFile([FromRoute] IdInput input)
        {
            var fileInfo = await _systemFileLogic.FindByIdAsync(input.Id);
            HttpClient httpClient = new HttpClient();
            var bt = await httpClient.GetByteArrayAsync(fileInfo.Path);
            string downloadFilename = Path.Combine(ConfigurationUtil.GetTempPath(), fileInfo.FileId + fileInfo.Extension);
            Stream responseStream = new MemoryStream(bt);
            Stream stream = new FileStream(downloadFilename, FileMode.OpenOrCreate);
            byte[] bArr = new byte[1024];
            int size = responseStream.Read(bArr, 0, bArr.Length);
            while (size > 0)
            {
                stream.Write(bArr, 0, size);
                size = responseStream.Read(bArr, 0, bArr.Length);
            }
            stream.Close();
            responseStream.Close();
            var file = File(stream, fileInfo.ContentType, fileInfo.Name);
            //删除文件
            FileUtil.DeleteFile(downloadFilename);
            return file;
        }

        /// <summary>
        /// 删除文件
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Route("/system/file/delete")]
        public async Task<ActionResult> DeleteFile(IdInput input)
        {
            return Ok(await _systemFileLogic.DeleteFile(input));
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [RequestSizeLimit(1000_000_000)]//最大100m左右
        [Route("/system/file/uploadform")]
        public async Task<ActionResult> UploadFileForm([FromForm] FileUploadInput input)
        {
            OperateStatus<IList<SystemFile>> operateStatus = new OperateStatus<IList<SystemFile>>();
            try
            {
                var filesStorageOptions = (await _systemConfigLogic.FindFilesStorageOptions()).Data;
                var filesStorageFileSuffix = filesStorageOptions.FileTypes;
                var filesStorageFileMaxSize = filesStorageOptions.MaxSize;
                var filesStorageType = filesStorageOptions.StorageType;
                var maxSize = 1024 * 1024 * filesStorageFileMaxSize; //上传大小5M
                var files = Request.Form.Files;
                if (files.Any())
                {
                    IList<SystemFile> instanceFiles = new List<SystemFile>();
                    OssUtil ossUtil = new OssUtil();
                    foreach (var file in files)
                    {
                        var msg = ossUtil.UpLoadFileCheck(filesStorageOptions, file);
                        if (msg.IsNotNullOrEmpty())
                        {
                            operateStatus.Msg = msg;
                            return Ok(operateStatus);
                        }
                    }
                    foreach (var file in files)
                    {
                        var fileName = file.FileName;
                        var fileExt = Path.GetExtension(fileName).ToLowerInvariant();
                        var url = await ossUtil.UpLoadFile(filesStorageOptions, fileExt, file);
                        instanceFiles.Add(new SystemFile
                        {
                            FileId = CombUtil.NewComb(),
                            IsDelete = false,
                            Name = file.FileName,
                            Path = url,
                            Extension = SystemIo.Path.GetExtension(file.FileName),
                            ContentType = file.ContentType,
                            Size = file.Length / 1024,
                            CreateTime = DateTime.Now,
                            CorrelationId = input.CorrelationId,
                            CreateUserId = CurrentUser.UserId
                        });

                        if (input.Single)
                        {
                            await _systemFileLogic.DeleteFileByRelationId(new IdInput<string>(input.CorrelationId));
                        }
                        var result = await _systemFileLogic.UploadFile(instanceFiles);
                    }

                    return Ok(new
                    {
                        code = ResultCode.Success,
                        msg = "上传成功",
                        data = new
                        {
                            fileId = instanceFiles[0].FileId,
                            url = instanceFiles[0].Path + "?t=" + DateTimeUtil.GetTimeStamp(),
                        }
                    });
                }
                else
                {
                    operateStatus.Code = ResultCode.Success;
                    operateStatus.Msg = Chs.Successful;
                }
            }
            catch (Exception ex)
            {
                operateStatus.Msg = ex.Message;
            }
            return Ok(operateStatus);
        }

        /// <summary>
        /// 文件上传，返回文件存储的相对路径数组
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("文件上传，返回文件存储的相对路径数组", RemarkFrom.System)]
        [RequestSizeLimit(1000_000_000)]//最大100m左右
        [Route("/system/file/uploadreturnpath")]
        public async Task<ActionResult> UploadFileFormReurnPaths([FromForm] FileUploadInput input)
        {
            OperateStatus<List<SystemFile>> operateStatus = new OperateStatus<List<SystemFile>>();
            try
            {
                var filesStorageOptions = (await _systemConfigLogic.FindFilesStorageOptions()).Data;
                var files = Request.Form.Files;
                if (files.Any())
                {
                    List<SystemFile> filePathList = new List<SystemFile>();
                    IList<SystemFile> instanceFiles = new List<SystemFile>();
                    OssUtil ossUtil = new OssUtil();
                    foreach (var file in files)
                    {
                        var msg = ossUtil.UpLoadFileCheck(filesStorageOptions, file);
                        if (msg.IsNotNullOrEmpty())
                        {
                            operateStatus.Msg = msg;
                            return Ok(operateStatus);
                        }
                    }

                    foreach (var file in files)
                    {
                        var fileName = file.FileName;
                        var fileExt = Path.GetExtension(fileName).ToLowerInvariant();
                        var url = await ossUtil.UpLoadFile(filesStorageOptions, fileExt, file);
                        var fileId = CombUtil.NewComb();
                        filePathList.Add(new SystemFile
                        {
                            FileId = fileId,
                            Path = url
                        });
                        instanceFiles.Add(new SystemFile
                        {
                            FileId = fileId,
                            IsDelete = false,
                            Name = file.FileName,
                            Path = url,
                            Extension = SystemIo.Path.GetExtension(file.FileName),
                            ContentType = file.ContentType,
                            Size = file.Length / 1024,
                            CreateTime = DateTime.Now,
                            CorrelationId = input.CorrelationId,
                            CreateUserId = CurrentUser.UserId
                        });
                        if (input.Single)
                        {
                            await _systemFileLogic.DeleteFileByRelationId(new IdInput<string>(input.CorrelationId));
                        }
                        var result = await _systemFileLogic.UploadFile(instanceFiles);
                        if (result.Code == ResultCode.Error)
                        {
                            return Ok(result);
                        }
                    }
                    operateStatus.Data = filePathList;
                    operateStatus.Code = ResultCode.Success;
                    operateStatus.Msg = Chs.Successful;
                }
                else
                {
                    operateStatus.Code = ResultCode.Success;
                    operateStatus.Msg = Chs.Successful;
                }
            }
            catch (Exception ex)
            {
                operateStatus.Msg = ex.Message;
            }
            return Ok(operateStatus);
        }
        #endregion
    }
}
