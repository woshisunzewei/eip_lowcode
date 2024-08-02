/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/5/29 15:57:26
* 文件名: FileController
* 描述: 文件表控制器
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Base.Models.Entities.DataRoom;
using EIP.Common.Controller.Attribute;
using EIP.Common.Core.Resource;
using EIP.Common.Core.Util;
using EIP.Common.Models;
using EIP.Common.Models.Dtos;
using EIP.Common.Models.Paging;
using EIP.DataRoom.Logic;
using EIP.DataRoom.Models.Dtos;
using EIP.System.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SystemIo = System.IO;
namespace EIP.DataRoom.Controller
{
    /// <summary>
    /// 文件表
    /// </summary>
    public class FileController : BaseDataRoomController
    {
        #region 构造函数
        private readonly IDashboardFileLogic _dashboardFileLogic;
        private readonly ISystemConfigLogic _systemConfigLogic;
        /// <summary>
        /// 文件表构造函数
        /// </summary>
        /// <param name="dashboardFileLogic"></param>
        public FileController(IDashboardFileLogic dashboardFileLogic, ISystemConfigLogic systemConfigLogic)
        {
            _systemConfigLogic = systemConfigLogic;
            _dashboardFileLogic = dashboardFileLogic;
        }
        #endregion

        #region 方法

        /// <summary>
        /// 获取分页
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("文件表-方法-列表-获取分页", RemarkFrom.System)]
        [Route("/dashboard/file")]
        [ProducesResponseType(typeof(OperateStatus<PagedResults<DashboardFileFindOutput>>), 1)]
        public async Task<ActionResult> Find([FromQuery] DashboardFileFindInput input)
        {
            return JsonForGridPagingDataRoom(await _dashboardFileLogic.Find(input), input);
        }
        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [CreateBy("孙泽伟")]
        [RequestSizeLimit(1000_000_000)]//最大100m左右
        [Route("/dashboard/file/upload")]
        public async Task<ActionResult> UploadFileForm([FromForm] DashboardFileUploadInput input)
        {
            OperateStatus operateStatus = new OperateStatus();
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
                    OssUtil ossUtil = new OssUtil();

                    var file = files.FirstOrDefault();
                    var fileName = file.FileName;
                    var fileExt = Path.GetExtension(fileName).ToLowerInvariant();
                    var url = await ossUtil.UpLoadFile(filesStorageOptions, fileExt, file);
                    var dashboardFile = new DashboardFile
                    {
                        module = input.module,
                        extension = Path.GetExtension(file.FileName).Substring(1, file.Name.Length - 1),
                        new_name = fileName,
                        original_name = fileName,
                        path = url,
                        size = file.Length / 1024,
                        url = url,
                        create_date = DateTime.Now,
                        update_date = DateTime.Now
                    };
                    var result = await _dashboardFileLogic.Save(dashboardFile);

                    return Ok(new
                    {
                        code = result.Code,
                        msg = result.Msg,
                        data = dashboardFile
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
        /// 删除
        /// </summary>
        /// <param name="input">主键Id</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("文件表-方法-列表-删除", RemarkFrom.System)]
        [Route("/dashboard/file/delete/{id}")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async Task<ActionResult> Delete([FromRoute] IdInput<string> input)
        {
            return Ok(await _dashboardFileLogic.Delete(input));
        }
        #endregion
    }
}
