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
using EIP.System.Models.Dtos.UserThree;
namespace EIP.System.Logic.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemUserInfoThreeLogic : DapperAsyncLogic<SystemUserInfoThree>, ISystemUserInfoThreeLogic
    {
        #region 构造函数
        private readonly ISystemUserInfoThreeRepository _systemUserThreeRepository;
        private readonly ISystemUserInfoLogic _userInfoLogic;
        private readonly ISystemConfigLogic _configLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userInfoLogic"></param>
        public SystemUserInfoThreeLogic(ISystemConfigLogic configLogic, 
            ISystemUserInfoLogic userInfoLogic,
            ISystemUserInfoThreeRepository systemUserThreeRepository)
        {
            _userInfoLogic = userInfoLogic;
            _systemUserThreeRepository = systemUserThreeRepository;
            _configLogic = configLogic;
        }

        #endregion

        #region 钉钉
        /// <summary>
        /// 获取人员
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus> DingTalkAsync()
        {
            try
            {
                var appKey = await _configLogic.FindByKey(EnumConfigKey.dingTalkAppKey.ToString());
                var appSecret = await _configLogic.FindByKey(EnumConfigKey.dingTalkAppSecret.ToString());
                using (var fix = new SqlDatabaseFixture())
                {
                    //查看是否已具有组织架构数据
                    var department = fix.Db.SystemOrganizationThree.FindAll();
                    AlibabaCloud.OpenApiClient.Models.Config config = new AlibabaCloud.OpenApiClient.Models.Config();
                    config.Protocol = "https";
                    config.RegionId = "central";
                    AlibabaCloud.SDK.Dingtalkoauth2_1_0.Client client = new AlibabaCloud.SDK.Dingtalkoauth2_1_0.Client(config);
                    AlibabaCloud.SDK.Dingtalkoauth2_1_0.Models.GetAccessTokenRequest getAccessTokenRequest = new AlibabaCloud.SDK.Dingtalkoauth2_1_0.Models.GetAccessTokenRequest()
                    {
                        AppKey = appKey,
                        AppSecret = appSecret,
                    };
                    var accessToken = client.GetAccessToken(getAccessTokenRequest);
                    List<DingTalkUserListOutput> DingTalkUserListOutputs = new List<DingTalkUserListOutput>();
                    //获取组织架构下人员
                    foreach (var item in department)
                    {
                        DingTalkFindRecursion(DingTalkUserListOutputs, accessToken.Body.AccessToken, item.PlatformOrganizationId, 0);
                    }

                    fix.Db.SystemUserInfoThree.Delete(d => d.Id > 0 && d.Type == 4);
                    var currentUser = EipHttpContext.CurrentUser();
                    List<SystemUserInfoThree> systemUserInfoThrees = new List<SystemUserInfoThree>();
                    foreach (var item in DingTalkUserListOutputs)
                    {
                        var departmentDetail = department.FirstOrDefault(f => f.PlatformOrganizationId == item.deptId);
                        List<string> deptNames = new List<string>();
                        foreach (var deptId in item.dept_id_list)
                        {
                            var deptDetail = department.FirstOrDefault(f => f.PlatformOrganizationId == deptId);
                            deptNames.Add(deptDetail.Name);
                        }
                        SystemUserInfoThree dingTalkUser = new SystemUserInfoThree()
                        {
                            ThreeUserId = CombUtil.NewComb(),
                            Name = item.name,
                            PlatformUserId = item.userid,
                            UnionId = item.unionid,
                            Avatar = item.avatar,
                            StateCode = item.state_code,
                            Mobile = item.mobile,
                            JobNumber = item.job_number,
                            Title = item.title,
                            Email = item.email,
                            Remark = item.remark,
                            Type = 4,
                            SystemOrganizationId = departmentDetail.SystemOrganizationId,
                            OrganizationIds = item.dept_id_list.ExpandAndToString(),
                            OrganizationNames = deptNames.ExpandAndToString("/")
                        };
                        dingTalkUser.CreateTime = DateTime.Now;
                        dingTalkUser.CreateUserId = currentUser.UserId;
                        dingTalkUser.CreateUserName = currentUser.Name;
                        dingTalkUser.UpdateTime = DateTime.Now;
                        dingTalkUser.UpdateUserId = currentUser.UserId;
                        dingTalkUser.UpdateUserName = currentUser.Name;
                        systemUserInfoThrees.Add(dingTalkUser);
                    }
                    if (systemUserInfoThrees.Any())
                    {
                        fix.Db.SystemUserInfoThree.BulkInsert(systemUserInfoThrees);
                    }
                    return OperateStatus.Success();
                }
            }
            catch (Exception ex)
            {
                return OperateStatus.Error(ex.Message);
            }
        }

        /// <summary>
        /// 递归获取
        /// </summary>
        /// <param name="dingTalkDepartmentOutputs"></param>
        /// <param name="accessToken"></param>
        /// <param name="deptId"></param>
        private void DingTalkFindRecursion(List<DingTalkUserListOutput> dingTalkUserListOutputs, string accessToken, string deptId, long cursor)
        {
            IDingTalkClient userClient = new DefaultDingTalkClient("https://oapi.dingtalk.com/topapi/v2/user/list");
            OapiV2UserListRequest req = new OapiV2UserListRequest();
            req.DeptId = Convert.ToInt32(deptId);
            req.Cursor = cursor;
            req.Size = 100;
            OapiV2UserListResponse rsp = userClient.Execute(req, accessToken);
            var datas = rsp.Body.JsonStringToObject<DingTalkUserDto>();
            if (datas.ErrCode == 0)
            {
                foreach (var item in datas.Result.list)
                {
                    var have = dingTalkUserListOutputs.FirstOrDefault(f => f.unionid == item.unionid);
                    if (have == null)
                    {
                        item.deptId = deptId;
                        dingTalkUserListOutputs.Add(item);
                        if (datas.Result.has_more)
                        {
                            DingTalkFindRecursion(dingTalkUserListOutputs, accessToken, deptId, datas.Result.next_cursor);
                        }
                    }
                }
            }
        }

        #endregion

        #region 企业微信
        /// <summary>
        /// 获取人员
        /// </summary>
        /// <returns></returns>
        public OperateStatus WeChatWorkAsync()
        {
            try
            {
                using (var fix = new SqlDatabaseFixture())
                {
                    //查看是否已具有组织架构数据
                    var department = fix.Db.SystemOrganizationThree.FindAll(f => f.Type == 3);
                    var workWeixinSetting = Config.SenparcWeixinSetting.WorkSetting;
                    var accessToken = AccessTokenContainer.GetToken(workWeixinSetting.WeixinCorpId, workWeixinSetting.WeixinCorpSecret);
                    List<GetMemberResult> userlist = new List<GetMemberResult>();
                    var members = Senparc.Weixin.Work.AdvancedAPIs.MailListApi.GetDepartmentMemberInfo(accessToken, 1, 1);

                    //获取组织架构下人员
                    if (members.errcode == 0)
                    {
                        fix.Db.SystemUserInfoThree.Delete(d => d.Id > 0 && d.Type == 3);
                        var currentUser = EipHttpContext.CurrentUser();
                        List<SystemUserInfoThree> systemUserInfoThrees = new List<SystemUserInfoThree>();
                        foreach (var item in members.userlist)
                        {
                            var departmentDetail = department.FirstOrDefault(f => f.PlatformOrganizationId == item.main_department.ToString());
                            List<string> deptNames = new List<string>();
                            foreach (var deptId in item.department)
                            {
                                var deptDetail = department.FirstOrDefault(f => f.PlatformOrganizationId == deptId.ToString());
                                deptNames.Add(deptDetail.Name);
                            }
                            SystemUserInfoThree weChatWorkUser = new SystemUserInfoThree()
                            {
                                ThreeUserId = CombUtil.NewComb(),
                                Name = item.name,
                                PlatformUserId = item.userid,
                                UnionId = item.open_userid,
                                Avatar = item.avatar,
                                Mobile = item.mobile,
                                Title = item.position,
                                Email = item.email,
                                Type = 3,
                                SystemOrganizationId = departmentDetail.SystemOrganizationId,
                                OrganizationIds = item.department.ExpandAndToString(),
                                OrganizationNames = deptNames.ExpandAndToString("/")
                            };
                            weChatWorkUser.CreateTime = DateTime.Now;
                            weChatWorkUser.CreateUserId = currentUser.UserId;
                            weChatWorkUser.CreateUserName = currentUser.Name;
                            weChatWorkUser.UpdateTime = DateTime.Now;
                            weChatWorkUser.UpdateUserId = currentUser.UserId;
                            weChatWorkUser.UpdateUserName = currentUser.Name;
                            systemUserInfoThrees.Add(weChatWorkUser);
                        }
                        if (systemUserInfoThrees.Any())
                        {
                            fix.Db.SystemUserInfoThree.BulkInsert(systemUserInfoThrees);
                        }
                    }
                    else
                    {
                        return OperateStatus.Error(members.errmsg);
                    }
                    return OperateStatus.Success();
                }
            }
            catch (Exception ex)
            {
                return OperateStatus.Error(ex.Message);
            }
        }
        /// <summary>
        /// 获取所有标签
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public OperateStatus<GetTagListResult> WeChatWorkTagList()
        {
            OperateStatus<GetTagListResult> operateStatus = new OperateStatus<GetTagListResult>();
            try
            {
                var workWeixinSetting = Config.SenparcWeixinSetting.WorkSetting;
                var accessToken = AccessTokenContainer.GetToken(workWeixinSetting.WeixinCorpId, workWeixinSetting.WeixinCorpSecret);
                List<GetMemberResult> userlist = new List<GetMemberResult>();
                var members = Senparc.Weixin.Work.AdvancedAPIs.MailListApi.GetTagList(accessToken);
                operateStatus.Code = ResultCode.Success;
                operateStatus.Msg = Chs.QuerySuccessful;
                operateStatus.Data = members;
            }
            catch (Exception ex)
            {
                operateStatus.Msg = ex.Message;
            }
            return operateStatus;
        }

        /// <summary>
        /// 获取人员根据Tag
        /// </summary>
        /// <returns></returns>
        public OperateStatus<GetTagMemberResult> WeChatWorkAsyncTag(int tagId)
        {
            OperateStatus<GetTagMemberResult> operateStatus = new OperateStatus<GetTagMemberResult>();
            try
            {
                using (var fix = new SqlDatabaseFixture())
                {
                    //查看是否已具有组织架构数据
                    var department = fix.Db.SystemOrganizationThree.FindAll(f => f.Type == 3);
                    var workWeixinSetting = Config.SenparcWeixinSetting.WorkSetting;
                    var accessToken = AccessTokenContainer.GetToken(workWeixinSetting.WeixinCorpId, workWeixinSetting.WeixinCorpSecret);
                    List<GetMemberResult> userlist = new List<GetMemberResult>();
                    var members = Senparc.Weixin.Work.AdvancedAPIs.MailListApi.GetTagMember(accessToken, tagId);
                    foreach (var item in members.userlist)
                    {
                        var user = Senparc.Weixin.Work.AdvancedAPIs.MailListApi.GetMember(accessToken, item.userid);
                    }
                    operateStatus.Code = ResultCode.Success;
                    operateStatus.Msg = Chs.Successful;
                    operateStatus.Data = members;
                }
            }
            catch (Exception ex)
            {
                operateStatus.Msg = ex.Message;
            }
            return operateStatus;
        }
        #endregion

        /// <summary>
        /// 绑定用户(已在系统中导入用户)
        /// </summary>
        /// <returns></returns>
        public OperateStatus Bind()
        {
            try
            {
                using (var fix = new SqlDatabaseFixture())
                {
                    var dingTalkUsers = fix.Db.SystemUserInfoThree.FindAll(f => f.Type == 4);
                    var userInfos = fix.Db.SystemUserInfo.FindAll();
                    foreach (var item in userInfos)
                    {
                        var dingTalkUser = dingTalkUsers.FirstOrDefault(w => w.Name == item.Name.Trim());
                        if (dingTalkUser != null)
                        {
                            item.HeadImage = dingTalkUser.Avatar;
                            dingTalkUser.SystemUserId = item.UserId;
                            dingTalkUser.SystemOrganizationId = item.OrganizationId;
                            fix.Db.SystemUserInfoThree.Update(dingTalkUser);
                            fix.Db.SystemUserInfo.Update(item);
                        }
                    }
                    return OperateStatus.Success();
                }
            }
            catch (Exception ex)
            {
                return OperateStatus.Error(ex.Message);
            }
        }

        /// <summary>
        /// 获取人员
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus> AsyncToSystem(int type)
        {
            try
            {
                using (var fix = new SqlDatabaseFixture())
                {
                    //删除所有人员
                    var systemUsers = fix.Db.SystemUserInfo.FindAll(f => f.IsAdmin == false);
                    foreach (var item in systemUsers.OrderBy(o => o.Id))
                    {
                        var result = await _userInfoLogic.Delete(new Common.Models.Dtos.IdInput<string> { Id = item.UserId.ToString() });
                        if (result.Code != ResultCode.Success)
                        {
                            return result;
                        }
                    }

                    var users = fix.Db.SystemUserInfoThree.FindAll(f => f.Type == type);
                    var department = fix.Db.SystemOrganizationThree.FindAll();
                    foreach (var user in users)
                    {
                        var org = department.FirstOrDefault(f => f.SystemOrganizationId == user.SystemOrganizationId);
                        var orgIds = "";
                        if (user.OrganizationIds.IsNotNullOrEmpty())
                        {
                            foreach (var item in user.OrganizationIds.Split(","))
                            {
                                var orgItem = department.FirstOrDefault(f => f.PlatformOrganizationId == item);
                                if (orgItem.SystemOrganizationId != user.SystemOrganizationId)
                                {
                                    orgIds += orgItem.SystemOrganizationId + ",";
                                }
                            }
                        }
                        SystemUserSaveInput systemUserSaveInput = new SystemUserSaveInput()
                        {
                            UserId = CombUtil.NewComb(),
                            OrganizationId = user.SystemOrganizationId,
                            OrganizationName = org.Name,
                            Code = user.Mobile,
                            Name = user.Name,
                            Mobile = user.Mobile,
                            Email = user.Email,
                            Remark = user.Remark,
                            HeadImage = user.Avatar,
                            UserOrganizationIds = orgIds.TrimEnd(','),
                        };
                        user.SystemUserId = systemUserSaveInput.UserId;
                        await fix.Db.SystemUserInfoThree.UpdateAsync(user);

                        await _userInfoLogic.Save(systemUserSaveInput);
                    }
                    return OperateStatus.Success();
                }
            }
            catch (Exception ex)
            {
                return OperateStatus.Error(ex.Message);
            }
        }

        /// <summary>
        /// 同步人员
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus> TransferToSystem(SystemUserThreeToSystemInput input)
        {
            try
            {
                OperateStatus operateStatus = new OperateStatus();
                using (var fix = new SqlDatabaseFixture())
                {
                    foreach (var item in input.UserIds.Split(','))
                    {
                        var userId = Guid.Parse(item);
                        var user = fix.Db.SystemUserInfoThree.Find(f => f.SystemUserId == userId);
                        var org = fix.Db.SystemOrganization.SetSelect(s => new { s.Name, s.OrganizationId }).Find(f => f.OrganizationId == input.OrganizationId);
                        SystemUserSaveInput systemUserSaveInput = new SystemUserSaveInput()
                        {
                            UserId = CombUtil.NewComb(),
                            OrganizationId = input.OrganizationId,
                            OrganizationName = org.Name,
                            Code = user.Mobile,
                            Name = user.Name,
                            Mobile = user.Mobile,
                            Email = user.Email,
                            Remark = user.Remark,
                            HeadImage = user.Avatar
                        };
                        user.SystemUserId = systemUserSaveInput.UserId;
                        operateStatus = await _userInfoLogic.Save(systemUserSaveInput);
                    }
                    return operateStatus;
                }
            }
            catch (Exception ex)
            {
                return OperateStatus.Error(ex.Message);
            }
        }
       
        /// <summary>
        /// 同步人员
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus> BindUserId(UserThreeBindInput input)
        {
            try
            {
                var user = await FindAsync(f => f.ThreeUserId == input.ThreeUserId);
                user.SystemUserId = input.SystemUserId;
                return await UpdateAsync(user);
            }
            catch (Exception ex)
            {
                return OperateStatus.Error(ex.Message);
            }
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="input">分页参数</param>
        /// <returns></returns>
        public async Task<OperateStatus<PagedResults<UserThreePagingOutput>>> Find(UserThreePagingInput input)
        {
            var datas = await _systemUserThreeRepository.Find(input);
            return OperateStatus<PagedResults<UserThreePagingOutput>>.Success(datas);
        }
    }
}