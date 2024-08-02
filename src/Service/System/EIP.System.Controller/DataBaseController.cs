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
using EIP.Common.Models.Dtos.DataBase;
using EIP.Common.Models.Tree;
using EIP.System.Models.Dtos.DataBase;
using Microsoft.Extensions.Configuration.UserSecrets;
using MiniExcelLibs;
using MiniSoftware;
using System.IO;
using SystemIo = System.IO;

namespace EIP.System.Controller
{
    /// <summary>
    /// 数据库控制器
    /// </summary>
    public class DataBaseController : BaseSystemController
    {
        #region 构造函数

        private readonly ISystemDataBaseLogic _systemDataBaseLogic;
        private readonly ISystemFileLogic _systemFileLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="systemDataBaseLogic"></param>
        public DataBaseController(ISystemDataBaseLogic systemDataBaseLogic, ISystemFileLogic systemFileLogic)
        {
            _systemDataBaseLogic = systemDataBaseLogic;
            _systemFileLogic = systemFileLogic;
        }

        #endregion

        #region 方法
        /// <summary>
        /// 获取所有应用数据库
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("应用数据库-方法-列表-获取所有应用数据库", RemarkFrom.System)]
        [Route("/system/database/table/tree")]
        public async Task<ActionResult> FindTableTree()
        {
            return JsonForTree((await _systemDataBaseLogic.FindTableTree()).Data);
        }

        /// <summary>
        /// 获取表空间使用情况
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("应用数据库-方法-列表-获取表空间使用情况", RemarkFrom.System)]
        [Route("/system/database/spaceused")]
        public async Task<ActionResult> FindDataBaseSpaceused()
        {
            return Ok(await _systemDataBaseLogic.FindDataBaseSpaceused());
        }

        /// <summary>
        /// 获取对应数据库视图信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("应用数据库-方法-列表-获取对应数据库表信息", RemarkFrom.System)]
        [Route("/system/database/view")]
        public async Task<ActionResult> FindDataBaseView(IdInput input)
        {
            return Ok(await _systemDataBaseLogic.FindDataBaseView(input));
        }

        /// <summary>
        /// 获取对应数据库视图信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("应用数据库-方法-列表-获取对应数据库表信息", RemarkFrom.System)]
        [Route("/system/database/proc")]
        public async Task<ActionResult> FindDataBaseProc(IdInput input)
        {
            return Ok(await _systemDataBaseLogic.FindDataBaseProc(input));
        }
        /// <summary>
        /// 获取对应数据库表信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("应用数据库-方法-列表-获取对应数据库表信息", RemarkFrom.System)]
        [Route("/system/database/table")]
        public async Task<ActionResult> FindDataBaseTable(IdInput input)
        {
            return Ok(await _systemDataBaseLogic.FindDataBaseTable(input));
        }

        /// <summary>
        /// 获取对应表列信息
        /// </summary>
        /// <param name="doubleWayDto"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("应用数据库-方法-列表-获取对应表列信息", RemarkFrom.System)]
        [Route("/system/database/column")]
        public async Task<ActionResult> FindDataBaseColumnsList(SystemDataBaseTableDto doubleWayDto)
        {
            doubleWayDto.Header = Request.Headers["Authorization"];
            return Ok(await _systemDataBaseLogic.FindDataBaseColumns(doubleWayDto));
        }

        /// <summary>
        /// 表是否存在
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("应用数据库-方法-列表-获取对应表列信息", RemarkFrom.System)]
        [Route("/system/database/tableexist")]
        public async Task<ActionResult> IsTableExist(SystemDataBaseIsTableExistInput input)
        {
            return Ok(await _systemDataBaseLogic.IsTableExist(input));
        }

        /// <summary>
        /// 获取字段树结构
        /// </summary>
        /// <param name="doubleWay"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("应用数据库-方法-列表-获取对应表列信息树结构", RemarkFrom.System)]
        public async Task<ActionResult> FindDataBaseColumnsTree(SystemDataBaseTableDto doubleWay)
        {
            var columns = (await _systemDataBaseLogic.FindDataBaseColumns(doubleWay)).Data;
            IList<JsTreeEntity> treeEntities = new List<JsTreeEntity>();
            var parentId = CombUtil.NewComb();
            JsTreeEntity treeEntity = new JsTreeEntity
            {
                id = parentId,
                text = doubleWay.Name
            };
            treeEntities.Add(treeEntity);
            foreach (var co in columns)
            {
                treeEntity = new JsTreeEntity
                {
                    parent = parentId,
                    text = "(" + co.Name + ")",
                    id = co.Name
                };
                treeEntities.Add(treeEntity);
            }
            return Ok(treeEntities);
        }

        /// <summary>
        /// 获取外键信息
        /// </summary>
        /// <param name="doubleWayDto"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("应用数据库-方法-列表-获取外键信息", RemarkFrom.System)]
        public async Task<ActionResult> FindDataBasefFkColumn(SystemDataBaseTableDto doubleWayDto)
        {
            return Ok(await _systemDataBaseLogic.FindDataBasefFkColumn(doubleWayDto));
        }

        /// <summary>
        /// 创建表
        /// </summary>/database/column
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("表单维护-方法-创建表", RemarkFrom.System, true)]
        [Route("/system/database/table/save")]
        public ActionResult SaveFormTable(SystemDataBaseSaveFormTableInput input)
        {
            return Ok(_systemDataBaseLogic.SaveFormTable(input));
        }

        /// <summary>
        /// 修改表字段
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("表单维护-方法-修改表字段", RemarkFrom.System, true)]
        [Route("/system/database/tablefield")]
        public async Task<ActionResult> SaveFormTableField(SystemDataBaseSaveFormTableFieldInput input)
        {
            return Ok(await _systemDataBaseLogic.SaveFormTableField(input));
        }

        /// <summary>
        /// 保存数据
        /// </summary>
        /// <param name="input">信息</param>
        /// <returns></returns>
        [FieldFilter]
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("敏捷开发-方法-新增/编辑-保存", RemarkFrom.System, true)]
        [Route("/system/database/businessdata")]
        public async Task<ActionResult> SaveBusinessData(SystemDataBaseSaveBusinessDataInput input)
        {
            input.UserId = CurrentUser.UserId;
            input.UserCode = CurrentUser.Code;
            input.UserName = CurrentUser.Name;
            input.OrganizationName = CurrentUser.OrganizationName;
            if (CurrentUser.OrganizationId != null) input.OrganizationId = (Guid)CurrentUser.OrganizationId;
            var result = await _systemDataBaseLogic.SaveBusinessData(input);
            return Ok(result);
        }

        /// <summary>
        /// 根据主键获取信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("敏捷开发-方法-列表-根据主键获取信息", RemarkFrom.System, true)]
        [Route("/system/database/businessdata/byid")]
        public async Task<ActionResult> FindBusinessDataById(SystemDataBaseFindBusinessDataByIdInput input)
        {
            return Ok(await _systemDataBaseLogic.FindBusinessDataById(input));
        }
        /// <summary>
        /// 逻辑删除业务数据
        /// </summary>
        /// <param name="input">Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("敏捷开发-方法-列表-删除", RemarkFrom.System, true)]
        [Route("/system/database/businessdata/del")]
        public async Task<ActionResult> DeleteBusinessData(SystemDataBaseDeleteBusinessDataInput input)
        {
            return Ok(await _systemDataBaseLogic.DeleteBusinessData(input));
        }

        /// <summary>
        /// 物理删除业务数据
        /// </summary>
        /// <param name="input">Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("敏捷开发-方法-列表-删除", RemarkFrom.System, true)]
        [Route("/system/database/businessdata/del/physics")]
        public async Task<ActionResult> DeleteBusinessDataPhysics(SystemDataBaseDeleteBusinessDataInput input)
        {
            return Ok(await _systemDataBaseLogic.DeleteBusinessDataPhysics(input));
        }
        /// <summary>
        /// 物理删除业务数据
        /// </summary>
        /// <param name="input">Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("敏捷开发-方法-列表-删除", RemarkFrom.System, true)]
        [Route("/system/database/businessdata/del/physics/all")]
        public async Task<ActionResult> DeleteBusinessDataPhysicsAll(IdInput input)
        {
            return Ok(await _systemDataBaseLogic.DeleteBusinessDataPhysicsAll(input));
        }

        /// <summary>
        /// 恢复删除业务数据
        /// </summary>
        /// <param name="input">Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("敏捷开发-方法-列表-恢复删除业务数据", RemarkFrom.System, true)]
        [Route("/system/database/businessdata/del/recovery")]
        public async Task<ActionResult> RecoveryDeleteBusinessDataPhysics(SystemDataBaseDeleteBusinessDataInput input)
        {
            return Ok(await _systemDataBaseLogic.RecoveryDeleteBusinessDataPhysics(input));
        }

        /// <summary>
        /// 获取所有信息
        /// </summary>
        /// <param name="input">获取所有信息</param>
        /// <returns></returns>
        [FieldFilter]
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("敏捷开发-方法-列表-获取所有信息", RemarkFrom.System, true)]
        [Route("/system/database/businessdata/list")]
        public async Task<ActionResult> FindBusinessData(SystemDataBaseFindPagingBusinessDataInput input)
        {
            input.Header = Request.Headers["Authorization"];
            return Ok(await _systemDataBaseLogic.FindBusinessData(input));
        }
        /// <summary>
        /// 过滤查询
        /// </summary>
        /// <param name="input">获取所有信息</param>
        /// <returns></returns>
        [FieldFilter]
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("敏捷开发-方法-列表-获取所有信息", RemarkFrom.System, true)]
        [Route("/system/database/businessdata/list/filtersearch")]
        public async Task<ActionResult> FindBusinessDataFilterSearch(SystemDataBaseFindBusinessDataFilterSearchInput input)
        {
            input.Header = Request.Headers["Authorization"];
            return Ok(await _systemDataBaseLogic.FindBusinessDataFilterSearch(input));
        }
        /// <summary>
        /// 获取所有信息
        /// </summary>
        /// <param name="input">获取所有信息</param>
        /// <returns></returns>
        [FieldFilter]
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("敏捷开发-方法-列表-获取所有信息", RemarkFrom.System, true)]
        [Route("/system/database/businessdata/page")]
        public async Task<ActionResult> FindBusinessDataPage(SystemDataBaseFindPagingBusinessDataInput input)
        {
            input.Header = Request.Headers["Authorization"];
            return Ok(await _systemDataBaseLogic.FindBusinessDataPage(input));
        }
        /// <summary>
        /// 获取表尾数据
        /// </summary>
        /// <param name="input">获取所有信息</param>
        /// <returns></returns>
        [FieldFilter]
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("敏捷开发-方法-列表-获取所有信息", RemarkFrom.System)]
        [Route("/system/database/businessdata/footer")]
        public async Task<ActionResult> FindBusinessDataFooter(SystemDataBaseFindPagingBusinessDataInput input)
        {
            return Ok(await _systemDataBaseLogic.FindBusinessDataFooter(input));
        }

        /// <summary>
        ///获取子表数据
        /// </summary>
        /// <returns></returns>
        [FieldFilter]
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("敏捷开发-方法-获取子表数据", RemarkFrom.Workflow, true)]
        [Route("/system/database/businessdata/batch/list")]
        public async Task<ActionResult> FindBatchData(DataBaseSubTableDto input)
        {
            return Ok(await _systemDataBaseLogic.FindBatchData(input));
        }

        /// <summary>
        /// 获取指定信息
        /// </summary>
        /// <param name="input">获取所有信息</param>
        /// <returns></returns>
        [FieldFilter]
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("敏捷开发-方法-列表-获取指定信息", RemarkFrom.System, true)]
        [Route("/system/database/businessdata/fromsource")]
        public async Task<ActionResult> FindFormSourceData(SystemDataBaseFindFormSourceDataInput input)
        {
            return Ok(await _systemDataBaseLogic.FindFormSourceData(input));
        }

        /// <summary>
        /// 获取指定信息
        /// </summary>
        /// <param name="input">获取所有信息</param>
        /// <returns></returns>
        [FieldFilter]
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("敏捷开发-方法-列表-获取指定信息", RemarkFrom.System, true)]
        [Route("/system/database/businessdata/fromsourcepaging")]
        public async Task<ActionResult> FindFormSourceDataPaging(SystemDataBaseFindFormSourceDataInput input)
        {
            return Ok(await _systemDataBaseLogic.FindFormSourceDataPaging(input));
        }

        /// <summary>
        /// 获取指定工作表信息
        /// </summary>
        /// <param name="input">获取所有信息</param>
        /// <returns></returns>
        [FieldFilter]
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("敏捷开发-方法-列表-获取指定信息", RemarkFrom.System, true)]
        [Route("/system/database/businessdata/fromsourcepaging/table")]
        public async Task<ActionResult> FindFormSourceDataPagingTable(SystemDataBaseFindTableInput input)
        {
            return Ok(await _systemDataBaseLogic.FindFormSourceDataPagingTable(input));
        }

        /// <summary>
        /// 获取导入数据
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [RequestSizeLimit(100_000_000)] //最大100m左右
        [CreateBy("孙泽伟")]
        [Remark("敏捷开发-方法-导入模板", RemarkFrom.System)]
        [Route("/system/database/businessdata/import/data")]
        public ActionResult GetImportBusinessData()
        {
            OperateStatus<dynamic> operateStatus = new OperateStatus<dynamic>();
            try
            {
                var files = Request.Form.Files;
                if (files.Any())
                {
                    var file = files[0];
                    //保存文件
                    var uploadPath = ConfigurationUtil.GetTempPath();
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }
                    string path = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    string filename = Path.Combine(uploadPath, path);
                    using (FileStream fs = SystemIo.File.Create(filename))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    }
                    operateStatus.Data = MiniExcel.Query(filename, useHeaderRow: true);
                    //删除文件
                    var t = Task.Run(async delegate
                    {
                        FileUtil.DeleteFile(filename);
                    });
                }
                operateStatus.Code = ResultCode.Success;
                operateStatus.Msg = Chs.Successful;
                return Ok(operateStatus);
            }
            catch (Exception ex)
            {
                operateStatus.Msg = ex.Message;
                return Ok(operateStatus);
            }
        }

        /// <summary>
        /// 导入模板
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [RequestSizeLimit(100_000_000)] //最大100m左右
        [CreateBy("孙泽伟")]
        [Remark("敏捷开发-方法-导入模板", RemarkFrom.System)]
        [Route("/system/database/businessdata/import")]
        public async Task<ActionResult> ImportBusinessData()
        {
            OperateStatus operateStatus = new OperateStatus();
            try
            {
                SystemDataBaseImportInput input = new SystemDataBaseImportInput();
                var files = Request.Form.Files;
                if (files.Any())
                {
                    input.ConfigId = Guid.Parse(Request.Form["configId"].ToString());
                    input.Type = Convert.ToInt32(Request.Form["type"].ToString());
                    input.Clos = Request.Form["cols"].ToString().JsonStringToList<SystemDataBaseImportColsInput>();
                    var file = files[0];
                    //保存文件
                    var uploadPath = ConfigurationUtil.GetTempPath();
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }
                    string path = Guid.NewGuid() + Path.GetExtension(file.FileName);
                    string filename = Path.Combine(uploadPath, path);
                    using (FileStream fs = SystemIo.File.Create(filename))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    }
                    input.UserId = CurrentUser.UserId;
                    input.UserCode = CurrentUser.Code;
                    input.UserName = CurrentUser.Name;
                    input.OrganizationName = CurrentUser.OrganizationName;
                    if (CurrentUser.OrganizationId != null) input.OrganizationId = (Guid)CurrentUser.OrganizationId;
                    //读取文件
                    input.Data = MiniExcel.Query(filename, useHeaderRow: true).Cast<IDictionary<string, object>>();
                    //写入数据库
                    operateStatus = await _systemDataBaseLogic.ImportBusinessData(input);

                    //删除文件
                    FileUtil.DeleteFile(filename);

                    return Ok(operateStatus);
                }
                operateStatus.Code = ResultCode.Success;
                operateStatus.Msg = Chs.Successful;
                return Ok(operateStatus);
            }
            catch (Exception ex)
            {
                operateStatus.Msg = ex.Message;
                return Ok(operateStatus);
            }
        }

        /// <summary>
        /// 导入模板
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [RequestSizeLimit(100_000_000)] //最大100m左右
        [CreateBy("孙泽伟")]
        [Remark("敏捷开发-方法-导入模板", RemarkFrom.System)]
        [Route("/system/database/businessdata/import/table")]
        public async Task<ActionResult> ImportBusinessDataTable()
        {
            OperateStatus operateStatus = new OperateStatus();
            try
            {
                SystemDataBaseImportInput input = new SystemDataBaseImportInput();
                input.ConfigId = Guid.Parse(Request.Form["configId"].ToString());
                input.Type = Convert.ToInt32(Request.Form["type"].ToString());
                input.Clos = Request.Form["cols"].ToString().JsonStringToList<SystemDataBaseImportColsInput>();
                input.UserId = CurrentUser.UserId;
                input.UserCode = CurrentUser.Code;

                input.UserName = CurrentUser.Name;
                input.OrganizationName = CurrentUser.OrganizationName;
                if (CurrentUser.OrganizationId != null) input.OrganizationId = (Guid)CurrentUser.OrganizationId;
                //写入数据库
                var table = Request.Form["table"].ToString().JsonToDictionary();
                input.Data = table;
                operateStatus = await _systemDataBaseLogic.ImportBusinessData(input);
                return Ok(operateStatus);
            }
            catch (Exception ex)
            {
                operateStatus.Msg = ex.Message;
                return Ok(operateStatus);
            }
        }

        /// <summary>
        /// 导出
        /// </summary>
        /// <param name="input">导出</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("敏捷开发-方法-列表-导出", RemarkFrom.System, true)]
        [Route("/system/database/businessdata/report")]
        public async Task<FileResult> ReportBusinessData(SystemDataBaseFindPagingBusinessDataInput input)
        {
            input.Size = int.MaxValue;
            var datas = await _systemDataBaseLogic.FindBusinessData(input);
            IList<object> lists = datas.Data.data as List<object>;
            var reportDatas = new List<Dictionary<string, object>>();
            if (lists != null)
            {
                foreach (var list in lists)
                {
                    var reportData = new Dictionary<string, object>();
                    var rows = list as IDictionary<string, object>;
                    foreach (var item in input.Cols)
                    {
                        foreach (var row in rows)
                        {
                            if (row.Key == item.Field)
                            {
                                var value = "";
                                if (row.Value != null)
                                {
                                    value = row.Value.ToString();
                                    //是否具有格式化
                                    if (item.Style.Any() && !value.IsNullOrEmpty())
                                    {
                                        var style = item.Style.FirstOrDefault(f => f.Value == value);
                                        if (style != null)
                                        {
                                            value = style.Content;
                                        }
                                    }
                                }
                                reportData[item.Title] = value;
                            }
                        }
                    }
                    reportDatas.Add(reportData);
                }
            }
            string downloadFilename = Path.Combine(ConfigurationUtil.GetTempPath(), $"{Guid.NewGuid()}.xlsx");
            MiniExcel.SaveAs(downloadFilename, reportDatas);
            string excel = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            var fileResult = File(FileUtil.ReadFileStream(downloadFilename), excel, input.ReportName);
            FileUtil.DeleteFile(downloadFilename);
            return fileResult;
        }

        /// <summary>
        /// 模版导出
        /// </summary>
        /// <param name="input">导出</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("敏捷开发-方法-列表-导出", RemarkFrom.System, true)]
        [Route("/system/database/businessdata/report/template")]
        public async Task<FileResult> ReportBusinessDataTemplate(SystemDataBaseExportTemplateInput input)
        {
            var file = await _systemFileLogic.FindAsync(f => f.CorrelationId == input.ButtonId.ToString());
            if (file != null)
            {
                input.Extension = file.Extension;
                var datas = await _systemDataBaseLogic.FindBusinessDataById(input);
                //Excel模版
                if (file.Extension.Contains("xls") || file.Extension.Contains("xlsx"))
                {
                    //查询对应模版
                    string downloadFilename = Path.Combine(ConfigurationUtil.GetTempPath(), $"{Guid.NewGuid()}.xlsx");
                    FileUtil.FileDownSave(file.Path, downloadFilename);
                    string downloadFilenameTemplate = Path.Combine(ConfigurationUtil.GetTempPath(), $"{Guid.NewGuid()}.xlsx");
                    MiniExcel.SaveAsByTemplate(downloadFilenameTemplate, downloadFilename, datas.Data);
                    string contentType = file.ContentType;
                    var fileResult = File(FileUtil.ReadFileStream(downloadFilenameTemplate), contentType, input.ReportName);
                    FileUtil.DeleteFile(downloadFilenameTemplate);
                    FileUtil.DeleteFile(downloadFilename);
                    return fileResult;
                }
                else if (file.Extension.Contains("doc") || file.Extension.Contains("docx"))
                {
                    //查询对应模版
                    string downloadFilename = Path.Combine(ConfigurationUtil.GetTempPath(), $"{Guid.NewGuid()}.docx");
                    FileUtil.FileDownSave(file.Path, downloadFilename);
                    string downloadFilenameTemplate = Path.Combine(ConfigurationUtil.GetTempPath(), $"{Guid.NewGuid()}.docx");
                    MiniWord.SaveAsByTemplate(downloadFilenameTemplate, downloadFilename, datas.Data);
                    string contentType = file.ContentType;
                    if (input.Pdf)
                    {
                        //判断是否为word，若为word转换为pdf
                        var doc = new Aspose.Words.Document(downloadFilenameTemplate);
                        var pdfPath = downloadFilenameTemplate.Replace(".docx", ".pdf").Replace(".doc", ".pdf");
                        doc.Save(pdfPath);
                        FileUtil.DeleteFile(downloadFilenameTemplate);
                        contentType = "application/pdf";
                        downloadFilenameTemplate = pdfPath;
                    }
                    var fileResult = File(FileUtil.ReadFileStream(downloadFilenameTemplate), contentType, input.ReportName);
                    FileUtil.DeleteFile(downloadFilenameTemplate);
                    FileUtil.DeleteFile(downloadFilename);

                    return fileResult;
                }
            }
            return null;

        }
        /// <summary>
        /// 下载子表导出模板
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("应用系统-方法-列表-下载子表导出模板", RemarkFrom.System)]
        [Route("/system/database/businessdata/import/template")]
        public FileResult ImportTemplateDownload(SystemDataBaseFindPagingBusinessDataInput input)
        {
            var reportDatas = new List<Dictionary<string, object>>();
            var reportData = new Dictionary<string, object>();
            foreach (var item in input.Cols)
            {
                reportData.Add(item.Title, "");
            }
            reportDatas.Add(reportData);
            string excel = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            string downloadFilename = Path.Combine(ConfigurationUtil.GetTempPath(), $"{Guid.NewGuid()}.xlsx");
            MiniExcel.SaveAs(downloadFilename, reportDatas);
            var fileResult = File(FileUtil.ReadFileStream(downloadFilename), excel, input.ReportName);
            FileUtil.DeleteFile(downloadFilename);
            return fileResult;
        }

        #endregion

        #region 表维护
        /// <summary>
        /// 获取对应数据库表信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("应用数据库-方法-列表-获取数据库", RemarkFrom.System)]
        [Route("/system/database/list")]
        public async Task<ActionResult> FindAll()
        {
            return Ok(await _systemDataBaseLogic.FindAll());
        }

        /// <summary>
        /// 获取外键信息
        /// </summary>
        /// <param name="dataBase"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("应用数据库-方法-新增/编辑-保存", RemarkFrom.System)]
        [Route("/system/database")]
        public async Task<ActionResult> Save(SystemDataBase dataBase)
        {
            return Ok(await _systemDataBaseLogic.Save(dataBase));
        }

        /// <summary>
        /// 根据Id获取
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("应用数据库-方法-根据Id获取", RemarkFrom.System)]
        [Route("/system/database/{id}")]
        public async Task<ActionResult> FindById([FromRoute] IdInput input)
        {
            return Ok(await _systemDataBaseLogic.FindById(input));
        }

        /// <summary>
        /// 获取外键信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("应用数据库-方法-列表-删除", RemarkFrom.System)]
        [Route("/system/database/delete")]
        public async Task<ActionResult> Delete(IdInput input)
        {
            return Ok(await _systemDataBaseLogic.Delete(input));
        }
        #endregion

    }
}