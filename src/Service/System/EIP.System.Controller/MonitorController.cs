/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
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
using EIP.Common.Controller;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace EIP.System.Controller
{
    /// <summary>
    /// 监控
    /// </summary>
    public class MonitorController : BaseSystemController
    {
        #region 构造函数
        private readonly IHubContext<WebSiteHub> _hub;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="hub"></param>
        public MonitorController(IHubContext<WebSiteHub> hub)
        {
            _hub = hub;
        }

        #endregion

        /// <summary>
        /// 获取所有程序集
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("系统监控-方法-获取所有程序集", RemarkFrom.System)]
        [Route("/system/monitor/assemblies")]
        public ActionResult FindAllAssemblies()
        {
            var list = new List<AssembliesOutput>();
            var deps = DependencyContext.Default;
            //var libs = deps.CompileLibraries.Where(lib => !lib.Serviceable && lib.Type != "package");//排除所有的系统程序集、Nuget下载包
            foreach (var lib in deps.CompileLibraries)
            {
                try
                {
                    var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(lib.Name));
                    list.Add(new AssembliesOutput
                    {
                        Name = assembly.GetName().Name,
                        Version = assembly.GetName().Version.ToString(),
                        ClrVersion = assembly.ImageRuntimeVersion,
                        Location = assembly.Location
                    });
                }
                catch (Exception)
                {
                }
            }
            return JsonForGridLoadOnce(list.OrderBy(o => o.Name));
        }

        /// <summary>
        /// 获取所有Api信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟", "2018-04-12")]
        [Remark("系统监控-方法-获取所有Api信息", RemarkFrom.System)]
        [Route("/system/monitor/api")]
        public ActionResult FindAllApi()
        {
            IList<MvcRote> rotes = new List<MvcRote>();
            var deps = DependencyContext.Default;
            foreach (var lib in deps.CompileLibraries.Where(w=>w.Name.StartsWith("EIP")))
            {
                if (lib.Name.Contains(".Controller"))
                {
                    try
                    {
                        var assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(lib.Name));
                        var types = Assembly.LoadFile(assembly.Location).GetTypes();
                        //控制器
                        var baseControllerType = typeof(BaseController);
                        var controllerType = typeof(Microsoft.AspNetCore.Mvc.Controller);

                        //方法
                        var jsonType = typeof(ActionResult);
                        var taskJsonType = typeof(Task<ActionResult>);

                        foreach (var type in types)
                        {
                            //是否为控制器类型:Controller或者是BaseController
                            var isController = controllerType.IsAssignableFrom(type) || baseControllerType.IsAssignableFrom(type);
                            // 跳过不是Controller的类型
                            if (!isController)
                            {
                                continue;
                            }
                            //控制器名称
                            var controller = type.Name.Substring(0, type.Name.Length - "Controller".Length);
                            var methodInfos = type.GetMethods();
                            foreach (var method in methodInfos)
                            {
                                //是否为方法
                                bool isAction = jsonType.IsAssignableFrom(method.ReturnType) ||
                                                taskJsonType.IsAssignableFrom(method.ReturnType);
                                // 跳过不是Action的方法
                                if (!isAction || method.Name.ToLower() == "json")
                                {
                                    continue;
                                }
                                //方法名称
                                string action = method.Name;
                                //该方法、界面的描述
                                string description,
                                    byDeveloperCode = string.Empty,
                                    byDeveloperTime = string.Empty;

                                // 如果打有描述文本标记则使用描述文本作为操作说明，否则试用Action方法名
                                var descriptionAttrs = method.GetCustomAttributes(typeof(RemarkAttribute), false);
                                if (descriptionAttrs.Length > 0)
                                {
                                    description = ((RemarkAttribute)descriptionAttrs[0]).Describe;
                                    if (string.IsNullOrEmpty(description))
                                    {
                                        description = action;
                                    }
                                }
                                else
                                {
                                    description = action;
                                }

                                string route = string.Empty;
                                var routeAttrs = method.GetCustomAttributes(typeof(RouteAttribute), false);
                                if (routeAttrs.Length > 0)
                                {
                                    route = ((RouteAttribute)routeAttrs[0]).Template;
                                }
                                // 如果打有描述文本标记则使用描述文本作为编写者说明
                                var byAttrs = method.GetCustomAttributes(typeof(CreateByAttribute), false);
                                if (byAttrs.Length > 0)
                                {
                                    byDeveloperCode = ((CreateByAttribute)byAttrs[0]).Name;
                                    byDeveloperTime = ((CreateByAttribute)byAttrs[0]).Time;

                                    rotes.Add(new MvcRote
                                    {
                                        Controller = controller,
                                        Action = action,
                                        Description = description,
                                        ByDeveloperCode = byDeveloperCode,
                                        ByDeveloperTime = byDeveloperTime,
                                        Url = route
                                    });
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        return JsonForGridLoadOnce(rotes, ex.Message);
                    }
                }
            }
            return JsonForGridLoadOnce(rotes);
        }

        /// <summary>
        /// 在线用户
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("系统监控-方法-在线用户", RemarkFrom.System)]
        [Route("/system/monitor/onlineuser")]
        public ActionResult Online()
        {
            return Ok(OperateStatus<IList<OnlineUserInput>>.Success(OnlineUsers.Users));
        }

        /// <summary>
        /// 强制下线
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("系统监控-方法-强制下线", RemarkFrom.System)]
        [Route("/system/monitor/onlineuser/kickout")]
        public async Task<ActionResult> KickOut(IdInput<string> input)
        {
            OperateStatus operateStatus = new OperateStatus();
            foreach (var id in input.Id.Split(","))
            {
                var item = OnlineUsers.Users.FirstOrDefault(f => f.ConnectionId == id);
                if (item != null)
                {
                    OnlineUsers.Users.Remove(item);
                    await _hub.Clients.Client(id).SendAsync("CompulsoryDownline", $"{JsonConvert.SerializeObject(item)}");
                    operateStatus.Msg = "下线成功";
                    operateStatus.Code = ResultCode.Success;
                }
                else
                {
                    operateStatus.Msg = "下线失败,请重试";
                }
            }
            return Ok(operateStatus);
        }

        /// <summary>
        /// 获取服务器配置信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("系统监控-方法-获取服务器配置信息", RemarkFrom.System)]
        [Route("/system/monitor/serverbase")]
        public dynamic GetServerBase()
        {
            OperateStatus<dynamic> operateStatus = new OperateStatus<dynamic>();
            operateStatus.Code = ResultCode.Success;
            operateStatus.Msg = Chs.Successful;
            operateStatus.Data = new
            {
                HostName = Environment.MachineName, // 主机名称
                SystemOs = RuntimeInformation.OSDescription, // 操作系统
                OsArchitecture = Environment.OSVersion.Platform.ToString() + " " + RuntimeInformation.OSArchitecture.ToString(), // 系统架构
                ProcessorCount = Environment.ProcessorCount + " 核", // CPU核心数
                SysRunTime = ComputerUtil.GetRunTime(), // 系统运行时间
                FrameworkDescription = RuntimeInformation.FrameworkDescription, // NET框架
            };
            return Ok(operateStatus);
        }
        /// <summary>
        /// 获取服务器配置信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("系统监控-方法-获取服务器配置信息", RemarkFrom.System)]
        [Route("/system/monitor/serverdisk")]
        public dynamic GetServerDisk()
        {
            OperateStatus<dynamic> operateStatus = new OperateStatus<dynamic>();
            operateStatus.Code = ResultCode.Success;
            operateStatus.Msg = Chs.Successful;
            operateStatus.Data = ComputerUtil.GetDiskInfos();
            return Ok(operateStatus);
        }

        /// <summary>
        /// 获取服务器使用信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("系统监控-方法-获取服务器使用信息", RemarkFrom.System)]
        [Route("/system/monitor/serverused")]
        public ActionResult GetServerUsed()
        {
            OperateStatus<dynamic> operateStatus = new OperateStatus<dynamic>();
            var programStartTime = Process.GetCurrentProcess().StartTime;
            var totalMilliseconds = (DateTime.Now - programStartTime).TotalMilliseconds.ToString();
            var ts = totalMilliseconds.Contains('.') ? totalMilliseconds.Split('.')[0] : totalMilliseconds;
            var programRunTime = DateTimeUtil.FormatTime(ts.ParseToLong());
            var memoryMetrics = ComputerUtil.GetComputerInfo();
            operateStatus.Code=ResultCode.Success;
            operateStatus.Msg = Chs.Successful;
            operateStatus.Data = new
            {
                memoryMetrics.FreeRam, // 空闲内存
                memoryMetrics.UsedRam, // 已用内存
                memoryMetrics.TotalRam, // 总内存
                memoryMetrics.RamRate, // 内存使用率
                memoryMetrics.CpuRate, // Cpu使用率
                StartTime = programStartTime.ToString("yyyy-MM-dd HH:mm:ss"), // 服务启动时间
                RunTime = programRunTime, // 服务运行时间
            };
            return Ok(operateStatus);
        }

    }
}