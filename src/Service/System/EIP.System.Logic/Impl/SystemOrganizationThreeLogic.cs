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
using EIP.System.Models.Dtos.DingTalk;
using EIP.System.Models.Dtos.OrganizationThree;
using EIP.System.Models.Dtos.WeChat.Work;

namespace EIP.System.Logic.Impl
{
    /// <summary>
    /// 
    /// </summary>
    public class SystemOrganizationThreeLogic : DapperAsyncLogic<SystemOrganizationThree>, ISystemOrganizationThreeLogic
    {
        #region 构造函数
        private readonly ISystemConfigLogic _configLogic;
        private readonly ISystemOrganizationThreeRepository _systemOrganizationThreeRepository;
        private readonly ISystemOrganizationLogic _organizationLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="organizationLogic"></param>
        public SystemOrganizationThreeLogic(ISystemConfigLogic configLogic, ISystemOrganizationLogic organizationLogic,
            ISystemOrganizationThreeRepository systemOrganizationThreeRepository)
        {
            _systemOrganizationThreeRepository = systemOrganizationThreeRepository;
            _organizationLogic = organizationLogic;
            _configLogic = configLogic;
        }

        #endregion

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="input">分页参数</param>
        /// <returns></returns>
        public async Task<OperateStatus<PagedResults<SystemOrganizationThree>>> Find(SystemOrganizationThreePagingInput input)
        {
            var datas = await _systemOrganizationThreeRepository.Find(input);
            return OperateStatus<PagedResults<SystemOrganizationThree>>.Success(datas);
        }

        /// <summary>
        /// 获取组织架构值
        /// </summary>
        /// <returns></returns>
        public OperateStatus<List<BaseTree>> Tree(int type)
        {
            try
            {
                using (var fix = new SqlDatabaseFixture())
                {
                    //查看是否已具有组织架构数据
                    var department = fix.Db.SystemOrganizationThree.FindAll(f => f.Type == type);
                    List<BaseTree> trees = new List<BaseTree>();
                    if (department.Any())
                    {
                        foreach (var item in department)
                        {
                            trees.Add(new BaseTree
                            {
                                id = item.PlatformOrganizationId,
                                text = item.Name,
                                parent = item.PlatformOrganizationParentId
                            });
                        }
                    }
                    return OperateStatus<List<BaseTree>>.Success(trees);
                }
            }
            catch (Exception ex)
            {
                return OperateStatus<List<BaseTree>>.Error(ex.Message);
            }
        }

        /// <summary>
        /// 同步组织到系统
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus> AsyncToSystem(int type)
        {
            try
            {
                using (var fix = new SqlDatabaseFixture())
                {
                    var datas = fix.Db.SystemOrganizationThree.FindAll(f => f.Type == type);
                    fix.Db.SystemOrganization.Delete(d => d.Id > 0);
                    //写入组织架构
                    foreach (var item in datas)
                    {
                        //获取父级
                        var parent = datas.FirstOrDefault(f => f.PlatformOrganizationId == item.PlatformOrganizationParentId);
                        var systemOrganization = new SystemOrganization()
                        {
                            OrganizationId = item.SystemOrganizationId,
                            Name = item.Name,
                            Nature = item.Nature
                        };
                        if (parent != null)
                        {
                            systemOrganization.ParentId = parent.SystemOrganizationId;
                            systemOrganization.ParentName = parent.Name;
                        }
                        await _organizationLogic.Save(systemOrganization);
                    }
                    return OperateStatus.Success();
                }
            }
            catch (Exception ex)
            {
                return OperateStatus.Error(ex.Message);
            }
        }

        #region 钉钉
        /// <summary>
        /// 获取组织架构值
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
                    List<BaseTree> trees = new List<BaseTree>();
                    AlibabaCloud.SDK.Dingtalkoauth2_1_0.Client client = DingTalkCreateClient();
                    AlibabaCloud.SDK.Dingtalkoauth2_1_0.Models.GetAccessTokenRequest getAccessTokenRequest = new AlibabaCloud.SDK.Dingtalkoauth2_1_0.Models.GetAccessTokenRequest()
                    {
                        AppKey = appKey,
                        AppSecret = appSecret,
                    };
                    var accessToken = client.GetAccessToken(getAccessTokenRequest);
                    var datas = new List<DingTalkDepartmentOutput>();

                    //顶级
                    IDingTalkClient departmentDetailClient = new DefaultDingTalkClient("https://oapi.dingtalk.com/topapi/v2/department/get");
                    OapiV2DepartmentGetRequest departmentDetailReq = new OapiV2DepartmentGetRequest();
                    departmentDetailReq.DeptId = 1;
                    OapiV2DepartmentGetResponse departmentDetailRsp = departmentDetailClient.Execute(departmentDetailReq, accessToken.Body.AccessToken);
                    var detailDto = departmentDetailRsp.Body.JsonStringToObject<DingTalkDepartmentDetailDto>();
                    if (detailDto.ErrCode == 0)
                    {
                        datas.Add(new DingTalkDepartmentOutput
                        {
                            dept_id = detailDto.Result.dept_id,
                            name = detailDto.Result.name,
                            parent_id = null,
                            OrganizationId = CombUtil.NewComb()
                        });
                    }
                    else
                    {
                        return OperateStatus<List<BaseTree>>.Error(detailDto.ErrMsg);
                    }
                    //递归获取
                    DingTalkFindRecursion(datas, accessToken.Body.AccessToken, "1", null);
                    foreach (var item in datas)
                    {
                        trees.Add(new BaseTree
                        {
                            id = item.dept_id,
                            text = item.name,
                            parent = item.parent_id
                        });
                    }

                    fix.Db.SystemOrganizationThree.Delete(d => d.Id > 0 && d.Type == 4);
                    var currentUser = EipHttpContext.CurrentUser();
                    List<SystemOrganizationThree> systemOrganizationThrees = new List<SystemOrganizationThree>();
                    foreach (var item in datas)
                    {
                        SystemOrganizationThree dingTalkDepartment = new SystemOrganizationThree()
                        {
                            ThreeOrganizationId = CombUtil.NewComb(),
                            Name = item.name,
                            PlatformOrganizationParentId = item.parent_id,
                            PlatformOrganizationId = item.dept_id,
                            SystemOrganizationId = item.OrganizationId,
                            Type = 4
                        };
                        dingTalkDepartment.CreateTime = DateTime.Now;
                        dingTalkDepartment.CreateUserId = currentUser.UserId;
                        dingTalkDepartment.CreateUserName = currentUser.Name;
                        dingTalkDepartment.UpdateTime = DateTime.Now;
                        dingTalkDepartment.UpdateUserId = currentUser.UserId;
                        dingTalkDepartment.UpdateUserName = currentUser.Name;
                        systemOrganizationThrees.Add(dingTalkDepartment);
                    }
                    if (systemOrganizationThrees.Any())
                    {
                        fix.Db.SystemOrganizationThree.BulkInsert(systemOrganizationThrees);
                    }
                    return OperateStatus<List<BaseTree>>.Success(trees);
                }
            }
            catch (Exception ex)
            {
                return OperateStatus<List<BaseTree>>.Error(ex.Message);
            }
        }

        /// <summary>
        /// 递归获取
        /// </summary>
        /// <param name="dingTalkDepartmentOutputs"></param>
        /// <param name="accessToken"></param>
        /// <param name="deptId"></param>
        private void DingTalkFindRecursion(List<DingTalkDepartmentOutput> dingTalkDepartmentOutputs, string accessToken, string deptId, Guid? parentId)
        {
            IDingTalkClient dingTalkClient = new DefaultDingTalkClient("https://oapi.dingtalk.com/topapi/v2/department/listsub");
            OapiV2DepartmentListsubRequest req = new OapiV2DepartmentListsubRequest();
            if (deptId.IsNotNullOrEmpty())
            {
                req.DeptId = Convert.ToInt32(deptId);
            }
            OapiV2DepartmentListsubResponse rsp = dingTalkClient.Execute(req, accessToken);
            var datas = rsp.Body.JsonStringToObject<DingTalkDepartmentDto>();
            if (datas.ErrCode == 0)
            {
                foreach (var item in datas.Result)
                {
                    item.OrganizationId = CombUtil.NewComb();
                    item.ParentId = parentId;
                    dingTalkDepartmentOutputs.Add(item);
                    DingTalkFindRecursion(dingTalkDepartmentOutputs, accessToken, item.dept_id, item.OrganizationId);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private AlibabaCloud.SDK.Dingtalkoauth2_1_0.Client DingTalkCreateClient()
        {
            AlibabaCloud.OpenApiClient.Models.Config config = new AlibabaCloud.OpenApiClient.Models.Config();
            config.Protocol = "https";
            config.RegionId = "central";
            return new AlibabaCloud.SDK.Dingtalkoauth2_1_0.Client(config);
        }
        #endregion

        #region 企业微信

        /// <summary>
        /// 获取组织架构值
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus> WeChatWorkAsync()
        {
            try
            {
                using (var fix = new SqlDatabaseFixture())
                {
                    List<BaseTree> trees = new List<BaseTree>();

                    var workCorpId = await _configLogic.FindByKey(EnumConfigKey.wechatWorkCorpId.ToString());
                    var workCorpSecret = await _configLogic.FindByKey(EnumConfigKey.wechatWorkCorpSecret.ToString());

                    var accessToken = AccessTokenContainer.GetToken(workCorpId, workCorpSecret);
                    List<WeChatWorkDepartmentOutput> weChatWorkDepartmentOutputs = new List<WeChatWorkDepartmentOutput>();
                    var data = Senparc.Weixin.Work.AdvancedAPIs.MailListApi.GetDepartmentList(accessToken);
                    if (data.errcode == 0)
                    {
                        foreach (var item in data.department)
                        {
                            WeChatWorkDepartmentOutput weChatWorkDepartmentOutput = new WeChatWorkDepartmentOutput();
                            weChatWorkDepartmentOutput.OrganizationId = CombUtil.NewComb();
                            weChatWorkDepartmentOutput.name = item.name;
                            weChatWorkDepartmentOutput.id = item.id;
                            weChatWorkDepartmentOutput.parentid = item.parentid;
                            weChatWorkDepartmentOutputs.Add(weChatWorkDepartmentOutput);
                        }
                    }
                    else
                    {
                        return OperateStatus<List<BaseTree>>.Error(data.errmsg);
                    }
                    foreach (var item in weChatWorkDepartmentOutputs)
                    {
                        //获取上级
                        var parent = weChatWorkDepartmentOutputs.FirstOrDefault(f => f.id == item.parentid);
                        if (parent != null)
                        {
                            item.ParentId = parent.OrganizationId;
                        }
                    }

                    foreach (var item in weChatWorkDepartmentOutputs)
                    {
                        trees.Add(new BaseTree
                        {
                            id = item.id,
                            text = item.name,
                            parent = item.parentid
                        });
                    }

                    fix.Db.SystemOrganizationThree.Delete(d => d.Id > 0 && d.Type == 3);
                    var currentUser = EipHttpContext.CurrentUser();
                    List<SystemOrganizationThree> systemOrganizationThrees = new List<SystemOrganizationThree>();
                    foreach (var item in weChatWorkDepartmentOutputs)
                    {
                        SystemOrganizationThree weChatWorkDepartment = new SystemOrganizationThree()
                        {
                            ThreeOrganizationId = CombUtil.NewComb(),
                            Name = item.name,
                            PlatformOrganizationParentId = item.parentid.ToString(),
                            PlatformOrganizationId = item.id.ToString(),
                            SystemOrganizationId = item.OrganizationId,
                            Type = 3
                        };
                        weChatWorkDepartment.CreateTime = DateTime.Now;
                        weChatWorkDepartment.CreateUserId = currentUser.UserId;
                        weChatWorkDepartment.CreateUserName = currentUser.Name;
                        weChatWorkDepartment.UpdateTime = DateTime.Now;
                        weChatWorkDepartment.UpdateUserId = currentUser.UserId;
                        weChatWorkDepartment.UpdateUserName = currentUser.Name;
                        systemOrganizationThrees.Add(weChatWorkDepartment);
                    }
                    if (systemOrganizationThrees.Any())
                    {
                        fix.Db.SystemOrganizationThree.BulkInsert(systemOrganizationThrees);
                    }
                    return OperateStatus<List<BaseTree>>.Success(trees);
                }
            }
            catch (Exception ex)
            {
                return OperateStatus<List<BaseTree>>.Error(ex.Message);
            }
        }

        #endregion
    }
}