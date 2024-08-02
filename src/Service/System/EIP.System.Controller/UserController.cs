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
namespace EIP.System.Controller
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public class UserController : BaseSystemController
    {
        #region 构造函数
        private readonly ISystemPermissionUserLogic _permissionUserLogic;
        private readonly ISystemUserInfoLogic _userInfoLogic;
        private readonly ISystemOrganizationLogic _organizationLogic;
        private readonly ISystemPermissionLogic _permissionLogic;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITokenBuilder _tokenBuilder;
        private readonly ISystemConfigLogic _systemConfigLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tokenBuilder"></param>
        /// <param name="userInfoLogic"></param>
        /// <param name="permissionUserLogic"></param>
        /// <param name="organizationLogic"></param>
        /// <param name="permissionLogic"></param>
        /// <param name="httpContextAccessor"></param>
        /// <param name="systemConfigLogic"></param>
        public UserController(ITokenBuilder tokenBuilder,
            ISystemUserInfoLogic userInfoLogic,
            ISystemPermissionUserLogic permissionUserLogic,
            ISystemOrganizationLogic organizationLogic,
            ISystemPermissionLogic permissionLogic,
            IHttpContextAccessor httpContextAccessor,
            ISystemConfigLogic systemConfigLogic)
        {
            _httpContextAccessor = httpContextAccessor;
            _permissionUserLogic = permissionUserLogic;
            _organizationLogic = organizationLogic;
            _permissionLogic = permissionLogic;
            _userInfoLogic = userInfoLogic;
            _tokenBuilder = tokenBuilder;
            _systemConfigLogic = systemConfigLogic;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 分页获取所有用户信息
        /// </summary>
        /// <param name="input">用户信息分页参数</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("用户维护-方法-列表-分页获取所有用户信息", RemarkFrom.System)]
        [Route("/system/user/list")]
        public async Task<ActionResult> Find(SystemUserPagingInput input)
        {
            #region 获取权限控制器信息
            input.DataSql = (await _permissionLogic.FindDataPermissionSql(new ViewRote { UserId = CurrentUser.UserId, MenuId = ResourceMenuId.系统用户.ToGuid() })).Data;
            #endregion
            return JsonForGridPaging(await _userInfoLogic.Find(input));
        }

        /// <summary>
        /// 读取组织机构树
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("组织机构维护-方法-列表-读取组织机构树", RemarkFrom.System)]
        [Route("/system/user/organization")]
        public async Task<ActionResult> FindOrganization()
        {
            #region 获取权限控制器信息
            SystemOrganizationDataPermissionInput input = new SystemOrganizationDataPermissionInput
            {
                PrincipalUser = CurrentUser,
                DataSql = (await _permissionLogic.FindDataPermissionSql(new ViewRote
                {
                    UserId = CurrentUser.UserId,
                    MenuId = ResourceMenuId.系统用户.ToGuid()
                })).Data
            };
            #endregion
            return JsonForTree((await _organizationLogic.FindDataPermission(input)).Data.ToList());
        }

        /// <summary>
        /// 根据主键获取用户信息
        /// </summary>
        /// <param name="input">用户Id</param>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("用户维护-方法-列表-根据主键获取用户信息", RemarkFrom.System)]
        [Route("/system/user/{id}")]
        public async Task<ActionResult> FindById([FromRoute] IdInput input)
        {
            return Ok(await _userInfoLogic.FindById(input));
        }

        /// <summary>
        /// 检测代码是否已经具有重复项
        /// </summary>
        /// <param name="input">需要验证的参数</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("用户维护-方法-新增/编辑-检测代码是否已经具有重复项", RemarkFrom.System)]
        [Route("/system/user/checkcode")]
        public async Task<ActionResult> CheckCode(SystemUserCheckUserCodeInput input)
        {
            return Ok(await _userInfoLogic.CheckCode(input));
        }

        /// <summary>
        /// 保存人员数据
        /// </summary>
        /// <param name="input">人员信息</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("用户维护-方法-新增/编辑-保存", RemarkFrom.System, true)]
        [Route("/system/user")]
        public async Task<ActionResult> Save(SystemUserSaveInput input)
        {
            return Ok(await _userInfoLogic.Save(input));
        }

        /// <summary>
        /// 删除人员数据
        /// </summary>
        /// <param name="input">人员Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("用户维护-方法-列表-删除", RemarkFrom.System, true)]
        [Route("/system/user/{id}")]
        public async Task<ActionResult> Delete(IdInput<string> input)
        {
            return Ok(await _userInfoLogic.Delete(input));
        }

        /// <summary>
        /// 根据用户Id获取用户详细情况
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("用户维护-方法-列表-根据用户Id获取用户详细情况", RemarkFrom.System)]
        [Route("/system/user/detail/{id}")]
        public async Task<ActionResult> FindDetailByUserId([FromRoute] IdInput input)
        {
            return Ok(await _userInfoLogic.FindDetailByUserId(input));
        }

        /// <summary>
        /// 根据用户Id重置某人密码
        /// </summary>
        /// <param name="input">用户Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("用户维护-方法-列表-重置密码", RemarkFrom.System, true)]
        [Route("/system/user/resetpassword")]
        public async Task<ActionResult> ResetPassword(SystemUserResetPasswordInput input)
        {
            return Ok(await _userInfoLogic.ResetPassword(input));
        }

        /// <summary>
        /// 保存用户头像
        /// </summary>
        /// <param name="input">用户Id</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("用户维护-方法-列表-保存用户头像", RemarkFrom.System)]
        [Route("/system/user/headimage")]
        public async Task<ActionResult> SaveHeadImage(IdInput<string> input)
        {
            //将上传的base64字符串保存为图片
            return Ok(await _userInfoLogic.SaveHeadImage(new SystemUserSaveHeadImageInput
            {
                HeadImage = input.Id,
                UserId = CurrentUser.UserId
            }));
        }

        /// <summary>
        /// 保存用户信息
        /// </summary>
        /// <param name="privilegeMasterUser">用户json字符串</param>
        /// <param name="privilegeMasterValue">角色信息</param>
        /// <param name="privilegeMaster">归属人员类型:组织机构、角色、岗位、组</param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("用户维护-方法-保存权限用户信息", RemarkFrom.System, true)]
        [Route("/system/user/privilegemaster")]
        public async Task<ActionResult> SavePrivilegeMasterUser(string privilegeMasterUser,
            Guid privilegeMasterValue,
            EnumPrivilegeMaster privilegeMaster)
        {
            var models = JsonConvert.DeserializeObject<IList<SystemPermissionSaveUserInput>>(privilegeMasterUser);
            IList<Guid> users = models.Select(m => m.U).ToList();
            return Ok(await _permissionUserLogic.SavePermissionUserBeforeDelete(privilegeMaster, privilegeMasterValue, users));
        }

        /// <summary>
        /// 检查密码是否符合规则
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("/system/user/checkpasswordrule")]
        public async Task<ActionResult> CheckPasswordRule(SystemUserCheckPasswordRuleInput input)
        {
            return Ok(await _userInfoLogic.CheckPasswordRule(input));
        }

        /// <summary>
        /// 保存修改密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("用户维护-方法-保存修改后密码信息", RemarkFrom.System, true)]
        [Route("/system/user/changepassword")]
        public async Task<ActionResult> SaveChangePassword(SystemUserChangePasswordInput input)
        {
            input.UserId = CurrentUser.UserId;
            return Ok(await _userInfoLogic.SaveChangePassword(input));
        }

        /// <summary>
        /// 验证旧密码是否输入正确
        /// </summary>
        /// <param name="input">需要验证的参数</param>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("用户维护-方法-验证旧密码是否输入正确", RemarkFrom.System, true)]
        [Route("/system/user/checkoldpassword")]
        public async Task<ActionResult> CheckOldPassword([FromRoute] CheckSameValueInput input)
        {
            input.Id = CurrentUser.UserId;
            return Ok(await _userInfoLogic.CheckOldPassword(input));
        }

        /// <summary>
        /// 冻结
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("用户维护-方法-冻结", RemarkFrom.System, true)]
        [Route("/system/user/isfreeze")]
        public async Task<ActionResult> IsFreeze(IdInput input)
        {
            return Ok(await _userInfoLogic.IsFreeze(input));
        }

        /// <summary>
        /// 查看具有特权的人员
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("用户控件-视图-查看具有特权的人员", RemarkFrom.System)]
        [Route("/system/user/toporg")]
        public async Task<ActionResult> FindTopOrg(SystemUserFindTopOrgInput input)
        {
            return Ok((await _userInfoLogic.Find(new SystemUserPagingInput
            {
                DataSql = " 1=1 ",
                Size = 99999999,
                PrivilegeMaster = EnumPrivilegeMaster.组织架构,
                TopOrg = input.TopOrg,
                SpecificOrg = input.SpecificOrg
            })).Data);
        }

        #endregion

        #region 登录

        #region Pc
        /// <summary>
        /// 验证码
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("/authorize/account/captcha/{id}")]
        public IActionResult Captcha([FromRoute] IdInput input)
        {
            return File(_userInfoLogic.Captcha(input).Data, "image/gif");
        }

        /// <summary>
        /// 授权中心登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("/authorize/account/login")]
        public async Task<ActionResult> Login(SystemLoginInput input)
        {
            return await CreateToke(await _userInfoLogic.Login(input), input.UserAgent, input.Browser);
        }

        /// <summary>
        /// 根据用户Id登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("/authorize/account/login/userid")]
        public async Task<ActionResult> LoginByUserId(SystemLoginByUserIdInput input)
        {
            return await CreateToke(await _userInfoLogic.LoginByUserId(input), input.UserAgent, input.Browser);
        }

        /// <summary>
        /// 根据用户帐号登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("/authorize/account/login/code")]
        public async Task<ActionResult> LoginByCode(SystemLoginByCodeInput input)
        {
            return await CreateToke(await _userInfoLogic.LoginByCode(input), input.UserAgent, input.Browser);
        }

        #region 钉钉,企微
        /// <summary>
        /// 构建Oauth授权Url方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns>RedirectUrl</returns>
        [AllowAnonymous]
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("登录-方法-构建授权Url方法", RemarkFrom.System)]
        [Route("/authorize/account/login/oauth")]
        public ActionResult OauthUrl(SystemUserOauthUrlInput input)
        {
            OperateStatus<SystemUserOauthUrlOutput> operateStatus = new OperateStatus<SystemUserOauthUrlOutput>();
            var request = getRequest(input.Source);
            var authorize = request.authorize(AuthStateUtils.createState());
            operateStatus.Data = new SystemUserOauthUrlOutput { Url = authorize };
            operateStatus.Code = ResultCode.Success;
            operateStatus.Msg = Chs.Successful;
            return Ok(operateStatus);
        }

        /// <summary>
        /// Oauth授权回调方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("登录-方法-授权回调方法", RemarkFrom.System)]
        [Route("/authorize/account/login/oauth/calback")]
        public async Task<ActionResult> OauthCallBack(SystemUserOauthCallBackInput input)
        {
            var request = getRequest(input.AuthSource);
            var authResponse = request.login(new AuthCallback
            {
                auth_code = input.AuthCode,
                state = input.State,
                code = input.Code
            });
            string userId = "";
            if (authResponse != null && authResponse.code != 2000)
            {
                OperateStatus operateStatus = new OperateStatus();
                operateStatus.Msg = authResponse.msg;
                return Ok(operateStatus);
            }
            else
            {
                if (input.AuthSource == "WECHAT_ENTERPRISE_SCAN")
                {
                    var data = ((AuthUser)authResponse.data).originalUser;
                    var wxData = (Dictionary<string, object>)data;
                    userId = wxData.FirstOrDefault(f => f.Key == "userid").Value.ToString();
                }
                else if (input.AuthSource == "DINGTALK_SCAN")
                {
                    var data = ((AuthUser)authResponse.data);
                    userId = data.uuid;
                }
            }
            return await CreateToke(await _userInfoLogic.OauthCallBack(input, userId), input.UserAgent, input.Browser);
        }

        #region 从Webconfig中获取默认配置（可以改造成从数据库中读取）
        Dictionary<string, ClientConfig> _clientConfigs;
        Dictionary<string, ClientConfig> ClientConfigs
        {
            get
            {
                if (_clientConfigs == null)
                {
                    _clientConfigs = new Dictionary<string, ClientConfig>();
                    var globalParams = GlobalParams.GetValuesByName();

                    _clientConfigs.Add("WECHAT_ENTERPRISE_SCAN", new ClientConfig
                    {
                        agentId = globalParams.FirstOrDefault(f => f.Key == EnumConfigKey.wechatWorkAgentId.ToString()).Value,
                        clientId = globalParams.FirstOrDefault(f => f.Key == "wechatWorkCorpId").Value,
                        clientSecret = globalParams.FirstOrDefault(f => f.Key == "wechatWorkCorpSecret").Value,
                        redirectUri = globalParams.FirstOrDefault(f => f.Key == EnumConfigKey.systemDomain.ToString()).Value + "/oauth?authSource=WECHAT_ENTERPRISE_SCAN",
                    });

                    _clientConfigs.Add("DINGTALK_SCAN", new ClientConfig
                    {
                        agentId = globalParams.FirstOrDefault(f => f.Key == "dingTalkAgentId").Value,
                        clientId = globalParams.FirstOrDefault(f => f.Key == "dingTalkAppKey").Value,
                        clientSecret = globalParams.FirstOrDefault(f => f.Key == "dingTalkAppSecret").Value,
                        redirectUri = globalParams.FirstOrDefault(f => f.Key == EnumConfigKey.systemDomain.ToString()).Value + "/oauth?authSource=DINGTALK_SCAN",
                    });
                }
                return _clientConfigs;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authSource"></param>
        /// <returns></returns>
        private ClientConfig GetClientConfig(string authSource)
        {
            if (authSource.IsNullOrWhiteSpace())
            {
                return null;
            }

            if (!ClientConfigs.ContainsKey(authSource))
            {
                return null;
            }
            else
            {
                return ClientConfigs[authSource];
            }
        }
        #endregion

        /// <summary>
        /// 返回AuthRequest对象
        /// </summary>
        /// <param name="authSource"></param>
        /// <returns></returns>
        private IAuthRequest getRequest(string authSource)
        {
            // 获取 CollectiveOAuth 中已存在的
            IAuthRequest authRequest = getDefaultRequest(authSource);
            return authRequest;
        }

        /// <summary>
        /// 获取默认的 Request
        /// </summary>
        /// <param name="authSource"></param>
        /// <returns>{@link AuthRequest}</returns>
        private IAuthRequest getDefaultRequest(string authSource)
        {
            ClientConfig clientConfig = GetClientConfig(authSource);
            IAuthStateCache authStateCache = new DefaultAuthStateCache();

            DefaultAuthSourceEnum authSourceEnum = GlobalAuthUtil.enumFromString<DefaultAuthSourceEnum>(authSource);

            switch (authSourceEnum)
            {
                case DefaultAuthSourceEnum.WECHAT_MP:
                    return new WeChatMpAuthRequest(clientConfig, authStateCache);

                case DefaultAuthSourceEnum.WECHAT_OPEN:
                    return new WeChatOpenAuthRequest(clientConfig, authStateCache);

                case DefaultAuthSourceEnum.WECHAT_ENTERPRISE:
                    return new WeChatEnterpriseAuthRequest(clientConfig, authStateCache);

                case DefaultAuthSourceEnum.WECHAT_ENTERPRISE_SCAN:
                    return new WeChatEnterpriseScanAuthRequest(clientConfig, authStateCache);

                case DefaultAuthSourceEnum.DINGTALK_SCAN:
                    return new DingTalkScanAuthRequest(clientConfig, authStateCache);

                default:
                    return null;
            }
        }
        #endregion

        #endregion

        #region 移动端登录

        /// <summary>
        /// 移动端根据临时Code钉钉免密登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("/authorize/account/login/dingtalk")]
        public async Task<ActionResult> LoginDingTalk(SystemLoginByDingTalkInput input)
        {
            return await CreateToke(await _userInfoLogic.LoginDingTalk(input), input.UserAgent, input.Browser);
        }

        #endregion

        /// <summary>
        /// 根据刷新Token重新获取Token
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("/authorize/account/refresh/token")]
        public async Task<ActionResult> RefreshToken()
        {
            var loginTime = DateTime.Now;
            var ip = HttpContext.Features.Get<Microsoft.AspNetCore.Http.Features.IHttpConnectionFeature>()?.RemoteIpAddress?.ToString();
            var claims = new[]
            {
                    new Claim("Name", CurrentUser.Name),
                    new Claim("IsAdmin", CurrentUser.IsAdmin.ToString()),
                    new Claim("Code", CurrentUser.Code),
                    new Claim("OrganizationId", CurrentUser.OrganizationId.ToString()),
                    new Claim("OrganizationName", CurrentUser.OrganizationName ?? ""),
                    new Claim("LoginId", CurrentUser.LoginId.ToString()),
                    new Claim("RemoteIp", CurrentUser.RemoteIp),
                    new Claim("RemoteIpAddress",CurrentUser.RemoteIpAddress),
                    new Claim("UserAgent", CurrentUser.UserAgent),
                    new Claim("OsInfo", CurrentUser.OsInfo),
                    new Claim("Browser", CurrentUser.Browser),
                    new Claim(JwtRegisteredClaimNames.Jti, CurrentUser.UserId.ToString())
                    };
            var tokenEx = await _systemConfigLogic.FindByKey(EnumConfigKey.loginTokenExpires.ToString());
            var token = _tokenBuilder.BuildJwtToken(claims, ip, DateTime.UtcNow, loginTime.AddMinutes(tokenEx.IsNullOrEmpty() ? 120 : Convert.ToInt32(tokenEx))).TokenValue;
            var refreshToken = _tokenBuilder.BuildJwtToken(claims, ip, DateTime.UtcNow, loginTime.AddMinutes(tokenEx.IsNullOrEmpty() ? 240 : Convert.ToInt32(tokenEx) * 2)).TokenValue;
            return Ok(new
            {
                Code = ResultCode.Success,
                Msg = "登录成功",
                Data = new
                {
                    Token = token,
                    RefreshToken = refreshToken,
                    Expire = DateTime.Now.AddMinutes(tokenEx.IsNullOrEmpty() ? 120 : Convert.ToInt32(tokenEx)).GetTimeStampLong(),
                }
            });
        }

        /// <summary>
        /// 生成token
        /// </summary>
        /// <param name="info"></param>
        /// <param name="userAgent"></param>
        /// <param name="browser"></param>
        /// <returns></returns>
        private async Task<ActionResult> CreateToke(OperateStatus<SystemLoginOutput> info, string userAgent, string browser)
        {
            var remoteIp = "";
            var remoteIpAddress = "";
            string token, refreshToken;
            string osInfo = StringUitl.GetOsInfo(userAgent);
            var tokenEx = await _systemConfigLogic.FindByKey(EnumConfigKey.loginTokenExpires.ToString());
            if (info.Data != null)
            {
                remoteIp = IpBrowserUtil.GetRemoteIp(_httpContextAccessor);
                var cacheKey = $"LoginRomoteAddress:{remoteIp}";
                remoteIpAddress = await RedisHelper.CacheShellAsync(cacheKey, DateTimeUtil.TotalSeconds(1), async () =>
                {
                    return await IpBrowserUtil.GetRemoteIpAddress(_httpContextAccessor);
                });
                var loginTime = DateTime.Now;
                var ip = HttpContext.Features.Get<Microsoft.AspNetCore.Http.Features.IHttpConnectionFeature>()?.RemoteIpAddress?.ToString();
                var claims = new[]
                {
                    new Claim("Name", info.Data.Name),
                    new Claim("IsAdmin", info.Data.IsAdmin.ToString()),
                    new Claim("Code", info.Data.Code),
                    new Claim("OrganizationId", info.Data.OrganizationId==Guid.Empty?"":info.Data.OrganizationId.ToString()),
                    new Claim("OrganizationName", info.Data.OrganizationName ?? ""),
                    new Claim("LoginId", info.Data.LoginId.ToString()),
                    new Claim("RemoteIp", remoteIp),
                    new Claim("RemoteIpAddress",remoteIpAddress),
                    new Claim("UserAgent", userAgent),
                    new Claim("OsInfo", osInfo),
                    new Claim("Browser", browser),
                    new Claim(JwtRegisteredClaimNames.Jti, info.Data.UserId.ToString())
                    };
                token = _tokenBuilder.BuildJwtToken(claims, ip, DateTime.UtcNow, loginTime.AddMinutes(tokenEx.IsNullOrEmpty() ? 120 : Convert.ToInt32(tokenEx))).TokenValue;
                refreshToken = _tokenBuilder.BuildJwtToken(claims, ip, DateTime.UtcNow, loginTime.AddMinutes(tokenEx.IsNullOrEmpty() ? 240 : Convert.ToInt32(tokenEx) * 2)).TokenValue;
            }
            else
            {
                return Ok(OperateStatus.Error(info.Msg.ToString()));
            }

            //发送信息给客户端
            LoginLogHandler handler = new LoginLogHandler(new PrincipalUser
            {
                RemoteIp = remoteIp,
                RemoteIpAddress = remoteIpAddress,
                UserAgent = userAgent,
                OsInfo = osInfo,
                Browser = browser,
                Code = info.Data.Code,
                UserId = info.Data.UserId,
                Name = info.Data.Name
            }, info.Data.LoginId);
            //通知所有人
            //var converter = new IsoDateTimeConverter();
            //converter.DateTimeFormat = "yyyy-MM-dd hh:mm:ss";
            //_hubContext.Clients.All.SendAsync("ReceiveSendToLogin", JsonConvert.SerializeObject(handler.Log, converter));
            //写入日志
            handler.WriteLog();
            return Ok(new
            {
                Code = ResultCode.Success,
                Msg = "登录成功",
                Data = new
                {
                    LoginId = info.Data.LoginId,
                    UserId = info.Data.UserId,
                    Code = info.Data.Code,
                    OrganizationId = info.Data.OrganizationId,
                    Token = token,
                    RefreshToken = refreshToken,
                    RemoteIp = remoteIp,
                    RemoteIpAddress = remoteIpAddress,
                    UserAgent = userAgent,
                    Browser = browser,
                    OsInfo = osInfo,
                    Name = info.Data.Name,
                    OrganizationName = info.Data.OrganizationName,
                    HeadImage = info.Data.HeadImage,
                    Expire = DateTime.Now.AddMinutes(tokenEx.IsNullOrEmpty() ? 120 : Convert.ToInt32(tokenEx)).GetTimeStampLong(),
                    Sso = Convert.ToBoolean(await _systemConfigLogic.FindByKey(EnumConfigKey.loginSso.ToString())),
                    ChangePassword = info.Data.ChangePassword,
                    ChangePasswordType = info.Data.ChangePasswordType,
                }
            });
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("主界面-方法-退出", RemarkFrom.System)]
        [Route("/authorize/account/logout")]
        public ActionResult LoginOut()
        {
            OperateStatus operateStatus = new OperateStatus();
            try
            {
                if (CurrentUser != null)
                {
                    operateStatus = _userInfoLogic.LoginOut(new IdInput(CurrentUser.LoginId));
                }
            }
            catch (EipException e)
            {
                return Ok(OperateStatus.Error(e.Message));
            }
            return Ok(operateStatus);
        }

        #endregion

        #region 注册

        #endregion

        #region 导入导出
        /// <summary>
        /// 导出到Excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("用户维护-方法-列表-导出到Excel", RemarkFrom.System)]
        public FileResult ExportExcel(SystemUserPagingInput paging)
        {
            ExcelReportDto excelReportDto = new ExcelReportDto()
            {
                //TemplatePath = Server.MapPath("/") + "Templates/System/User/用户导出模版.xlsx",
                DownTemplatePath = "用户信息" + string.Format("{0:yyyyMMddHHmmssffff}", DateTime.Now) + ".xlsx",
                Title = "用户信息.xlsx"
            };
            _userInfoLogic.ReportExcelUserQuery(paging, excelReportDto);
            //return File(new FileStream(excelReportDto.DownPath, FileMode.Open), "application/octet-stream", Server.UrlEncode(excelReportDto.Title));
            return null;
        }

        /// <summary>
        /// 下载用户导入模版
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("用户维护-方法-下载用户导入模版", RemarkFrom.System)]
        [Route("/system/user/import/template")]
        public ActionResult DownImportTemplate()
        {
            return GenerateTemplate<SystemUserImportInput>();
        }

        /// <summary>
        /// 导出人员:生成excel文件
        /// </summary>
        /// <param name="input">查询参数</param>
        /// <returns></returns>
        [HttpPost]
        [RequestSizeLimit(100_000_000)] //最大100m左右
        [CreateBy("孙泽伟")]
        [Remark("用户维护-方法-导入人员", RemarkFrom.System)]
        [Route("/system/user/export")]
        public async Task<FileResult> ExportUser(SystemUserPagingInput input)
        {
            #region 获取权限控制器信息
            input.Size = int.MaxValue;
            input.DataSql = (await _permissionLogic.FindDataPermissionSql(new ViewRote { UserId = CurrentUser.UserId, MenuId = ResourceMenuId.系统用户.ToGuid() })).Data;
            #endregion
            var users = (await _userInfoLogic.Find(input)).Data.Data;
            return Export(users);
        }

        /// <summary>
        /// 导入人员
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [RequestSizeLimit(100_000_000)] //最大100m左右
        [CreateBy("孙泽伟")]
        [Remark("用户维护-方法-导入人员", RemarkFrom.System)]
        [Route("/system/user/import")]
        public async Task<ActionResult> ImportUser()
        {
            var result = Import<SystemUserImportInput>();
            if (result.Code == ResultCode.Success)
            {
                return Ok(await _userInfoLogic.Import(result.Data));
            }
            else
            {
                return Ok(result);
            }
        }

        /// <summary>
        /// 导入人员手机和帐号
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [RequestSizeLimit(100_000_000)] //最大100m左右
        [CreateBy("孙泽伟")]
        [Remark("用户维护-方法-导入人员", RemarkFrom.System)]
        [Route("/system/user/import/mobile")]
        public async Task<ActionResult> ImportUserMobile()
        {
            var result = Import<SystemUserImportInput>();
            if (result.Code == ResultCode.Success)
            {
                return Ok(await _userInfoLogic.ImportUserMobile(result.Data));
            }
            else
            {
                return Ok(result);
            }
        }

        #endregion

    }
}