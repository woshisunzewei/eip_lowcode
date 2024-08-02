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
using EIP.Common.Models.Tree;
using EIP.System.Logic.Log.ILogic;
using EIP.System.Models.Dtos.Log;
using System.IO;

namespace EIP.System.Controller
{
    /// <summary>
    /// 日志管理控制器
    /// </summary>

    public class LogController : BaseSystemController
    {
        #region 构造函数
        private readonly ISystemExceptionLogLogic _exceptionLogLogic;
        private readonly ISystemLoginLogLogic _loginLogLogic;
        private readonly ISystemOperationLogLogic _operationLogLogic;
        private readonly ISystemRateLimitLogLogic _rateLimitLogLogic;
        private readonly ISystemSmsLogLogic _systemSmsLogLogic;
        private readonly ISystemJobLogLogic _systemJobLogLogic;
        private readonly IHostingEnvironment _hostingEnvironment;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exceptionLogLogic"></param>
        /// <param name="systemSmsLogLogic"></param>
        /// <param name="loginLogLogic"></param>
        /// <param name="operationLogLogic"></param>
        /// <param name="rateLimitLogLogic"></param>
        public LogController(ISystemJobLogLogic systemJobLogLogic, ISystemExceptionLogLogic exceptionLogLogic,
            ISystemSmsLogLogic systemSmsLogLogic,
            ISystemLoginLogLogic loginLogLogic,
            ISystemOperationLogLogic operationLogLogic,
            ISystemRateLimitLogLogic rateLimitLogLogic,
            IHostingEnvironment hostingEnvironment)
        {
            _systemSmsLogLogic = systemSmsLogLogic;
            _operationLogLogic = operationLogLogic;
            _exceptionLogLogic = exceptionLogLogic;
            _loginLogLogic = loginLogLogic;
            _rateLimitLogLogic = rateLimitLogLogic;
            _hostingEnvironment = hostingEnvironment;
            _systemJobLogLogic = systemJobLogLogic;
        }

        #endregion

        #region 异常日志

        /// <summary>
        /// 获取所有异常信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("异常日志-方法-列表-获取所有异常信息", RemarkFrom.System)]
        [Route("/system/log/exception")]
        public async Task<ActionResult> FindPagingExceptionLog(SystemExceptionLogFindPagingInput paging)
        {
            return JsonForGridPaging(await _exceptionLogLogic.FindSystemExceptionLog(paging));
        }

        /// <summary>
        /// 根据主键获取异常明细
        /// </summary>
        /// <param name="input">主键Id</param>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("异常日志-方法-列表-根据主键获取异常明细", RemarkFrom.System)]
        [Route("/system/log/exception/{id}")]
        public async Task<ActionResult> FindExceptionLogById([FromRoute] IdInput<Guid> input)
        {
            return Ok(OperateStatus<SystemExceptionLog>.Success(await _exceptionLogLogic.FindAsync(f => f.ExceptionLogId == input.Id)));
        }

        /// <summary>
        /// 根据主键删除异常信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("异常日志-方法-列表-根据主键删除异常信息", RemarkFrom.System)]
        [Route("/system/log/exception/{id}")]
        public async Task<ActionResult> DeleteExceptionLogById(IdInput<string> input)
        {
            return Ok(await _exceptionLogLogic.DeleteByIdsAsync(input.Id));
        }

        /// <summary>
        /// 导出到Excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("异常日志-方法-列表-导出到Excel", RemarkFrom.System)]
        [Route("/system/log/exception/export")]
        public async Task<FileResult> ExportExcelToExceptionLog(QueryParam paging)
        {
            ExcelReportDto excelReportDto = new ExcelReportDto()
            {
                //TemplatePath = Server.MapPath("/") + "DataUser/Templates/System/Log/异常日志.xlsx",
                DownTemplatePath = "异常日志" + string.Format("{0:yyyyMMddHHmmssffff}", DateTime.Now) + ".xlsx",
                Title = "异常日志.xlsx"
            };
            await _exceptionLogLogic.ReportExcelExceptionLogQuery(paging, excelReportDto);
            //return File(new FileStream(excelReportDto.DownPath, FileMode.Open), "application/octet-stream", Server.UrlEncode(excelReportDto.Title));
            return null;
        }

        #endregion

        #region 登录日志

        /// <summary>
        /// 获取所有登录日志信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("登录日志-方法-列表-获取所有登录日志信息", RemarkFrom.System)]
        [Route("/system/log/login")]
        public async Task<ActionResult> FindPagingLoginLog(SystemLoginLogFindPagingInput paging)
        {
            paging.UserId = CurrentUser.UserId;
            return JsonForGridPaging(await _loginLogLogic.Find(paging));
        }

        /// <summary>
        /// 获取所有登录日志信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("登录日志-方法-列表-获取所有登录日志信息", RemarkFrom.System)]
        [Route("/system/log/login/{id}")]
        public async Task<ActionResult> FindLoginLogById([FromRoute] IdInput input)
        {
            return Ok(await _loginLogLogic.FindById(input));
        }

        /// <summary>
        /// 获取所有登录日志信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("登录日志-方法-列表-获取所有登录日志分析", RemarkFrom.System)]
        [Route("/system/log/login/analysis")]
        public async Task<ActionResult> FindLoginLogAnalysis(SystemLoginLogFindPagingInput paging)
        {
            var datas = await _loginLogLogic.FindAnalysis(paging);
            IList<string> xdata = new List<string>();
            IList<int> ydata = new List<int>();
            if (datas.Data.Any())
            {
                int days = ((!paging.CreateTime.IsNullOrEmpty() ? Convert.ToDateTime(paging.EndCreateTime.ToString("yyyy-MM-dd")) : DateTime.Now) - Convert.ToDateTime(datas.Data.Min(m => m.CreateTime).ToString("yyyy-MM-dd"))).Days;
                for (int i = 0; i < days + 1; i++)
                {
                    var time = datas.Data.Min(m => m.CreateTime).AddDays(i);
                    time = Convert.ToDateTime(time.ToString("yyyy-MM-dd"));
                    xdata.Add(time.ToString("MM-dd"));
                    ydata.Add(datas.Data.Where(w => w.CreateTime >= time.AddDays(0).Date && w.CreateTime <= time.AddDays(1).Date).Count());
                }
            }
            return Ok(new
            {
                analysis = new
                {
                    xdata,
                    ydata
                }
            });
        }

        /// <summary>
        /// 根据主键删除登录日志
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("异常日志-方法-列表-根据主键删除登录日志", RemarkFrom.System)]
        [Route("/system/log/login/{id}")]
        public async Task<ActionResult> DeleteLoginLogById(IdInput<string> input)
        {
            return Ok(await _loginLogLogic.Delete(input));
        }

        /// <summary>
        /// 导出到Excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("登录日志-方法-列表-导出到Excel", RemarkFrom.System)]
        [Route("/system/log/login/export")]
        public async Task<FileResult> ExportExcelToLoginLog(QueryParam paging)
        {
            //ExcelReportDto excelReportDto = new ExcelReportDto()
            //{
            //    TemplatePath = Server.MapPath("/") + "DataUser/Templates/System/Log/登录日志.xlsx",
            //    DownTemplatePath = "登录日志" + string.Format("{0:yyyyMMddHHmmssffff}", DateTime.Now) + ".xlsx",
            //    Title = "登录日志.xlsx"
            //};
            //await _loginLogLogic.ReportExcelLoginLogQuery(paging, excelReportDto);
            //return File(new FileStream(excelReportDto.DownPath, FileMode.Open), "application/octet-stream", Server.UrlEncode(excelReportDto.Title));
            return null;
        }
        #endregion

        #region 操作日志

        /// <summary>
        /// 获取所有操作日志信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("操作日志-方法-列表-获取所有操作日志信息", RemarkFrom.System)]
        [Route("/system/log/operation")]
        public async Task<ActionResult> FindPagingOperationLog(SystemOperationLogFindPagingInput paging)
        {
            return JsonForGridPaging(await _operationLogLogic.FindPaging(paging));
        }

        /// <summary>
        /// 根据主键获取操作日志信息明细
        /// </summary>
        /// <param name="input">主键Id</param>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("操作日志-方法-列表-根据主键获取操作日志信息明细", RemarkFrom.System)]
        [Route("/system/log/operation/{id}")]
        public async Task<ActionResult> FindOperationLogById([FromRoute] IdInput input)
        {
            return Ok(await _operationLogLogic.FindById(input));
        }

        /// <summary>
        /// 根据主键删除操作日志
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("异常日志-方法-列表-根据主键删除操作日志", RemarkFrom.System)]
        [Route("/system/log/operation/{id}")]
        public async Task<ActionResult> DeleteOperationLogById(IdInput<string> input)
        {
            return Ok(await _operationLogLogic.Delete(input));
        }

        /// <summary>
        /// 导出到Excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("操作日志-方法-列表-导出到Excel", RemarkFrom.System)]
        [Route("/system/log/operation/export")]
        public async Task<FileResult> ExportExcelToOperationLog(QueryParam paging)
        {
            //ExcelReportDto excelReportDto = new ExcelReportDto()
            //{
            //    TemplatePath = Server.MapPath("/") + "DataUser/Templates/System/Log/操作日志.xlsx",
            //    DownTemplatePath = "操作日志" + string.Format("{0:yyyyMMddHHmmssffff}", DateTime.Now) + ".xlsx",
            //    Title = "操作日志.xlsx"
            //};
            //await _operationLogLogic.ReportExcelOperationLogQuery(paging, excelReportDto);
            //return File(new FileStream(excelReportDto.DownPath, FileMode.Open), "application/octet-stream", Server.UrlEncode(excelReportDto.Title));
            return null;

        }
        #endregion

        #region 限流日志

        /// <summary>
        /// 获取所有限流信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("限流日志-方法-列表-获取所有限流信息", RemarkFrom.System)]
        [Route("/system/log/ratelimit")]
        public async Task<ActionResult> FindPagingRateLimitLog(SystemRateLimitLogFindPagingInput input)
        {
            return JsonForGridPaging(await _rateLimitLogLogic.FindPagingRateLimitLog(input));
        }

        /// <summary>
        /// 根据主键获取限流明细
        /// </summary>
        /// <param name="input">主键Id</param>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("限流日志-方法-列表-根据主键获取限流明细", RemarkFrom.System)]
        [Route("/system/log/ratelimit/{id}")]
        public async Task<ActionResult> FindRateLimitLogById([FromRoute] IdInput<Guid> input)
        {
            return Ok(OperateStatus<SystemRateLimitLog>.Success(await _rateLimitLogLogic.FindAsync(f => f.RateLimitLogId == input.Id)));
        }

        /// <summary>
        /// 根据主键删除限流信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("限流日志-方法-列表-根据主键删除限流信息", RemarkFrom.System)]
        [Route("/system/log/ratelimit/{id}")]
        public async Task<ActionResult> DeleteRateLimitLogById(IdInput<string> input)
        {
            return Ok(await _rateLimitLogLogic.DeleteByIdsAsync(input.Id));
        }

        /// <summary>
        /// 导出到Excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("限流日志-方法-列表-导出到Excel", RemarkFrom.System)]
        [Route("/system/log/ratelimit/export")]
        public async Task<FileResult> ExportExcelToRateLimitLog(QueryParam paging)
        {
            ExcelReportDto excelReportDto = new ExcelReportDto()
            {
                //TemplatePath = Server.MapPath("/") + "DataUser/Templates/System/Log/限流日志.xlsx",
                DownTemplatePath = "限流日志" + string.Format("{0:yyyyMMddHHmmssffff}", DateTime.Now) + ".xlsx",
                Title = "限流日志.xlsx"
            };
            await _rateLimitLogLogic.ReportExcelRateLimitLogQuery(paging, excelReportDto);
            //return File(new FileStream(excelReportDto.DownPath, FileMode.Open), "application/octet-stream", Server.UrlEncode(excelReportDto.Title));
            return null;
        }

        #endregion

        #region 作业日志
        /// <summary>
        /// 获取所有数据日志
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("Sql日志-方法-列表-获取所有作业日志", RemarkFrom.System)]
        [Route("/system/log/job")]
        public async Task<ActionResult> FindPagingJobLog(SystemJobLogFindPagingInput paging)
        {
            return JsonForGridPaging(await _systemJobLogLogic.FindSystemJobLog(paging));
        }
        #endregion

        #region 短信日志
        /// <summary>
        /// 获取所有短信日志信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("操作日志-方法-列表-获取所有短信日志信息", RemarkFrom.System)]
        [Route("/system/log/sms")]
        public async Task<ActionResult> FindPagingSmsLog(SystemSmsLogFindPagingInput input)
        {
            return JsonForGridPaging(await _systemSmsLogLogic.FindPaging(input));
        }

        /// <summary>
        /// 根据主键获取短信日志
        /// </summary>
        /// <param name="input">主键Id</param>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("短信日志-方法-列表-根据主键获取明细", RemarkFrom.System)]
        [Route("/system/log/sms/{id}")]
        public async Task<ActionResult> FindSmsLogById([FromRoute] IdInput input)
        {
            return Ok(await _systemSmsLogLogic.FindById(input));
        }

        /// <summary>
        /// 根据主键删除短信日志
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("短信日志-方法-列表-根据主键删除信息", RemarkFrom.System)]
        [Route("/system/log/sms/{id}")]
        public async Task<ActionResult> DeleteSmsLogById(IdInput<string> input)
        {
            return Ok(await _systemSmsLogLogic.Delete(input));
        }

        /// <summary>
        /// 导出到Excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("短信日志-方法-列表-导出到Excel", RemarkFrom.System)]
        [Route("/system/log/sms/export")]
        public async Task<FileResult> ExportExcelToSmsLog(SystemSmsLogFindPagingInput input)
        {
            input.Size = int.MaxValue;
            var data = (await _systemSmsLogLogic.FindPaging(input)).Data.Data;
            return Export(data);
        }
        #endregion

        #region 文本日志
        /// <summary>
        ///     获取目录树
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("操作日志-方法-列表-获取所有文本日志信息", RemarkFrom.System)]
        [Route("/system/log/txt")]
        public ActionResult FindTxtLog()
        {
            OperateStatus<List<TreeEntity>> operateStatus = new OperateStatus<List<TreeEntity>>()
            {
                Code = ResultCode.Success,
                Msg = "读取成功"
            };
            var tree = new List<TreeEntity>();
            var path = _hostingEnvironment.ContentRootPath + "\\App_Data";
            var strDirectories = Directory.GetDirectories(path);
            foreach (var dir in strDirectories)
            {
                var directoryName = dir.Substring(dir.LastIndexOf(@"\", StringComparison.Ordinal) + 1);
                var treeEntity = new TreeEntity
                {
                    Key = directoryName,
                    Title = directoryName
                };
                var strFileNames = Directory.GetFiles(dir);
                foreach (var filename in strFileNames)
                {
                    var txtTreeEntity = new TreeEntity
                    {
                        Key = filename.Replace(path, ""),
                        Title = Path.GetFileName(filename) + "(" + FileUtil.GetFileSize(filename) + ")",
                        IsLeaf = true
                    };
                    treeEntity.Children.Add(txtTreeEntity);
                }
                tree.Add(treeEntity);
            }
            operateStatus.Data = tree;
            return Ok(operateStatus);
        }

        /// <summary>
        /// 显示内容
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("操作日志-方法-列表-获取所有文本日志信息", RemarkFrom.System)]
        [Route("/system/log/txt/path")]
        public ActionResult FindTxtLogDetail(SystemTxtLogDetailInput input)
        {
            OperateStatus<string> operateStatus = new OperateStatus<string>()
            {
                Code = ResultCode.Success,
                Msg = "读取成功"
            };
            input.FilePath = _hostingEnvironment.ContentRootPath + "\\App_Data\\" + input.FilePath;
            operateStatus.Data = FileUtil.ReadFile(input.FilePath);
            return Ok(operateStatus);
        }
        #endregion

    }
}