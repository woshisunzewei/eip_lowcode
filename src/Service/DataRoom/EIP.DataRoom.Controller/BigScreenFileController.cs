/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2024/6/1 16:54:21
* 文件名: ScreenFileController
* 描述: 文件表控制器
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Base.Models.Entities.DataRoom;
using EIP.Big.Models.Dtos.ScreenFile;
using EIP.Common.Controller.Attribute;
using EIP.Common.Core.Resource;
using EIP.Common.Core.Util;
using EIP.Common.Models;
using EIP.Common.Models.Dtos;
using EIP.Common.Models.Paging;
using EIP.DataRoom.Logic;
using EIP.DataRoom.Logic.Impl;
using EIP.DataRoom.Models.Dtos;
using EIP.System.Logic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace EIP.DataRoom.Controller
{
    /// <summary>
    /// 文件表
    /// </summary>
    public class ScreenFileController : BaseDataRoomController
    {
        #region 构造函数
        private readonly IBigScreenFileLogic _bigScreenFileLogic;
        private readonly ISystemConfigLogic _systemConfigLogic;
        /// <summary>
        /// 文件表构造函数
        /// </summary>
        /// <param name="bigScreenFileLogic"></param>
        public ScreenFileController(IBigScreenFileLogic bigScreenFileLogic, ISystemConfigLogic systemConfigLogic)
        {
            _bigScreenFileLogic = bigScreenFileLogic;
            _systemConfigLogic = systemConfigLogic;
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
        [Route("/bigScreen/file")]
        [ProducesResponseType(typeof(OperateStatus<PagedResults<BigScreenFileFindOutput>>), 1)]
        public async Task<ActionResult> Find([FromQuery] BigScreenFileFindInput input)
        {
            return JsonForGridPagingDataRoom(await _bigScreenFileLogic.Find(input), input);
        }

        /// <summary>
        /// 上传文件
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [CreateBy("孙泽伟")]
        [RequestSizeLimit(1000_000_000)]//最大100m左右
        [Route("/bigScreen/file/upload")]
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
                    var dashboardFile = new BigScreenFile
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
                    var result = await _bigScreenFileLogic.Save(dashboardFile);

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
        [Route("/bigScreen/file/delete/{id}")]
        [ProducesResponseType(typeof(OperateStatus), 1)]
        public async Task<ActionResult> Delete([FromRoute] IdInput<string> input)
        {
            return Ok(await _bigScreenFileLogic.Delete(input));
        }
        #endregion
    }
}
