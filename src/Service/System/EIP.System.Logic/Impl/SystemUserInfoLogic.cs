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
using EIP.Common.Repository.MicroOrm.SqlGenerator.Filters;
using EIP.System.Repository.Impl;
using Lazy.Captcha.Core;
using System.Text.RegularExpressions;
using SystemIo = System.IO;
namespace EIP.System.Logic.Impl
{
    /// <summary>
    /// 用户业务逻辑实现
    /// </summary>
    public class SystemUserInfoLogic : DapperAsyncLogic<SystemUserInfo>, ISystemUserInfoLogic
    {
        #region 构造函数
        private readonly ICaptcha _captcha;
        private readonly ISystemPermissionUserLogic _permissionUserLogic;
        private readonly ISystemUserInfoRepository _userInfoRepository;

        private readonly ISystemConfigLogic _systemConfigLogic;

        /// <summary>
        /// 系统人员
        /// </summary>
        /// <param name="userInfoRepository"></param>
        /// <param name="permissionUserLogic"></param>
        /// <param name="configOptions"></param>
        public SystemUserInfoLogic(ISystemConfigLogic systemConfigLogic,
            ISystemUserInfoRepository userInfoRepository,
            ISystemPermissionUserLogic permissionUserLogic,

            ICaptcha captcha)
        {
            _userInfoRepository = userInfoRepository;
            _permissionUserLogic = permissionUserLogic;

            _captcha = captcha;
            _systemConfigLogic = systemConfigLogic;
        }

        /// <summary>
        /// 系统用户
        /// </summary>
        /// <param name="configOptions"></param>
        /// <param name="organizationLogic"></param>
        public SystemUserInfoLogic(ISystemOrganizationLogic organizationLogic)
        {

            _userInfoRepository = new SystemUserInfoRepository();
        }

        #endregion

        #region 方法

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="input">分页参数</param>
        /// <returns></returns>
        public async Task<OperateStatus<PagedResults<SystemUserOutput>>> Find(SystemUserPagingInput input)
        {
            if (input.DataSql.IsNullOrEmpty()) return OperateStatus<PagedResults<SystemUserOutput>>.Success(new PagedResults<SystemUserOutput>());
            var datas = await _userInfoRepository.Find(input);
            //查询所有角色
            using (var fix = new SqlDatabaseFixture())
            {
                var roles = await fix.Db.SystemRole.SetSelect(s => new { s.RoleId, s.Name }).FindAllAsync();
                IList<Guid> privilegeMasterUserId = new List<Guid>();
                foreach (var item in datas.Data)
                {
                    privilegeMasterUserId.Add(item.UserId);
                }
                var userPermissions = await fix.Db.SystemPermissionUser.SetSelect(s => new { s.PrivilegeMasterUserId, s.PrivilegeMasterValue }).FindAllAsync(f => f.PrivilegeMaster == 0);
                foreach (var item in datas.Data)
                {
                    List<string> rolesName = new List<string>();
                    var userPermission = userPermissions.Where(w => w.PrivilegeMasterUserId == item.UserId).Select(s => s.PrivilegeMasterValue).ToList();
                    //查询对应角色
                    var userPermissionRole = roles.Where(s => userPermission.Contains(s.RoleId)).Select(s => s.Name);
                    item.Role = userPermissionRole.ExpandAndToString();
                    item.Mobile = item.Mobile.ConvertMask("phone");
                }
            }
            return OperateStatus<PagedResults<SystemUserOutput>>.Success(datas);
        }

        /// <summary>
        /// 用户公共信息
        /// </summary>
        /// <param name="input">分页参数</param>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemUserFindCommonOutput>>> FindCommon(SystemUserFindCommonInput input)
        {
            return OperateStatus<IEnumerable<SystemUserFindCommonOutput>>.Success(await _userInfoRepository.FindCommon(input));
        }

        /// <summary>
        /// 检测配置项代码是否已经具有重复项
        /// </summary>
        /// <param name="input">需要验证的参数</param>
        /// <returns></returns>
        public async Task<OperateStatus> CheckCode(SystemUserCheckUserCodeInput input)
        {
            var operateStatus = new OperateStatus();
            var count = !input.Id.IsEmptyGuid() ?
                await CountAsync(c => c.Code == input.Code && c.UserId != input.Id) :
                await CountAsync(c => c.Code == input.Code);
            if (count != 0)
            {
                operateStatus.Code = ResultCode.Error;
                operateStatus.Msg = string.Format(Chs.HaveCode, input.Code);
            }
            else
            {
                operateStatus.Code = ResultCode.Success;
                operateStatus.Msg = Chs.CheckSuccessful;
            }
            return operateStatus;
        }

        /// <summary>
        /// 保存人员信息
        /// </summary>
        /// <param name="input">人员信息</param>
        /// <returns></returns>
        public async Task<OperateStatus> Save(SystemUserSaveInput input)
        {
            OperateStatus operateStatus;
            var user = await FindAsync(f => f.UserId == input.UserId);
            var currentUser = EipHttpContext.CurrentUser();
            var organizationId = new List<Guid>();
            if (input.UserOrganizationIds.IsNotNullOrEmpty())
            {
                organizationId.AddRange(input.UserOrganizationIds.Split(',').Select(s => Guid.Parse(s)));
            }
            List<string> userOrganizationNames = new List<string>();
            if (organizationId.Any())
            {
                using (var fix = new SqlDatabaseFixture())
                {
                    userOrganizationNames = (await fix.Db.SystemOrganization.SetSelect(s => new { s.Name }).FindAllAsync(f => organizationId.Contains(f.OrganizationId))).Select(s => s.Name).ToList();
                }
            }
            if (user == null)
            {
                //新增
                if (!input.Code.IsNullOrEmpty())
                {
                    input.Password = DEncryptUtil.Encrypt("123456", ConfigurationUtil.GetPasswordKey());
                }
                SystemUserInfo userInfoMap = input.MapTo<SystemUserSaveInput, SystemUserInfo>();
                userInfoMap.UserOrganizationIds = organizationId.ExpandAndToString();
                userInfoMap.UserOrganizationNames = userOrganizationNames.ExpandAndToString();

                userInfoMap.CreateTime = DateTime.Now;
                userInfoMap.CreateUserId = currentUser.UserId;
                userInfoMap.CreateUserName = currentUser.Name;
                userInfoMap.UpdateTime = DateTime.Now;
                userInfoMap.UpdateUserId = currentUser.UserId;
                userInfoMap.UpdateUserName = currentUser.Name;
                userInfoMap.HeadImage = await CreateHeadImage(input) + "?t=" + DateTimeUtil.GetTimeStamp();
                operateStatus = await InsertAsync(userInfoMap);
                if (operateStatus.Code == ResultCode.Success)
                {
                    using (var fix = new SqlDatabaseFixture())
                    {
                        var privilegeMaster = EnumPrivilegeMaster.组织架构;
                        await fix.Db.SystemPermissionUser.DeleteAsync(d => d.PrivilegeMaster == privilegeMaster.ToShort() && d.PrivilegeMasterUserId == input.UserId);
                        IList<SystemPermissionUser> systemPermissionUsers =
                            organizationId.Select(roleId => new SystemPermissionUser
                            {
                                PrivilegeMaster = (byte)privilegeMaster,
                                PrivilegeMasterUserId = input.UserId,
                                PrivilegeMasterValue = roleId,
                                IsRelationOrganization = true,
                            }).ToList();
                        if (systemPermissionUsers.Any())
                        {
                            //批量保存
                            await fix.Db.SystemPermissionUser.BulkInsertAsync(systemPermissionUsers);
                        }

                        SystemPermissionUser systemPermissionUser = new SystemPermissionUser
                        {
                            PrivilegeMaster = (byte)privilegeMaster,
                            PrivilegeMasterUserId = input.UserId,
                            PrivilegeMasterValue = input.OrganizationId,
                            IsRelationOrganization = false,
                        };
                        await fix.Db.SystemPermissionUser.InsertAsync(systemPermissionUser);
                    }
                    operateStatus = await _permissionUserLogic.SavePermissionMasterValueBeforeDelete(EnumPrivilegeMaster.人员, input.UserId, new List<Guid> { input.UserId });
                    if (input.RoleId.IsNotNullOrEmpty())
                    {
                        await _permissionUserLogic.SavePermissionMasterValueBeforeDelete(EnumPrivilegeMaster.角色, input.UserId, input.RoleId.Split(',').Select(s => Guid.Parse(s)).ToList());
                    }
                    if (operateStatus.Code == ResultCode.Success)
                    {
                        return operateStatus;
                    }
                }
                else
                {
                    return operateStatus;
                }
            }
            else
            {
                input.Password = user.Password;
                SystemUserInfo userInfoMap = input.MapTo<SystemUserSaveInput, SystemUserInfo>();
                userInfoMap.UserOrganizationIds = organizationId.ExpandAndToString();
                userInfoMap.UserOrganizationNames = userOrganizationNames.ExpandAndToString();
                userInfoMap.Id = user.Id;

                userInfoMap.CreateTime = user.CreateTime;
                userInfoMap.CreateUserId = user.CreateUserId;
                userInfoMap.CreateUserName = user.CreateUserName;

                userInfoMap.UpdateTime = DateTime.Now;
                userInfoMap.UpdateUserId = currentUser.UserId;
                userInfoMap.UpdateUserName = currentUser.Name;

                if (user.Name != input.Name)
                {
                    userInfoMap.HeadImage = await CreateHeadImage(input) + "?t=" + DateTimeUtil.GetTimeStamp();
                }

                operateStatus = await UpdateAsync(userInfoMap);
                //添加用户到组织机构
                if (operateStatus.Code == ResultCode.Success)
                {
                    using (var fix = new SqlDatabaseFixture())
                    {
                        var privilegeMaster = EnumPrivilegeMaster.组织架构;
                        await fix.Db.SystemPermissionUser.DeleteAsync(d => d.PrivilegeMaster == privilegeMaster.ToShort() && d.PrivilegeMasterUserId == input.UserId);
                        IList<SystemPermissionUser> systemPermissionUsers =
                            organizationId.Select(roleId => new SystemPermissionUser
                            {
                                PrivilegeMaster = (byte)privilegeMaster,
                                PrivilegeMasterUserId = input.UserId,
                                PrivilegeMasterValue = roleId,
                                IsRelationOrganization = true,
                            }).ToList();
                        if (systemPermissionUsers.Any())
                        {
                            //批量保存
                            await fix.Db.SystemPermissionUser.BulkInsertAsync(systemPermissionUsers);
                        }

                        SystemPermissionUser systemPermissionUser = new SystemPermissionUser
                        {
                            PrivilegeMaster = (byte)privilegeMaster,
                            PrivilegeMasterUserId = input.UserId,
                            PrivilegeMasterValue = input.OrganizationId,
                            IsRelationOrganization = false,
                        };
                        await fix.Db.SystemPermissionUser.InsertAsync(systemPermissionUser);
                    }
                    operateStatus = await _permissionUserLogic.SavePermissionMasterValueBeforeDelete(EnumPrivilegeMaster.人员, input.UserId, new List<Guid> { input.UserId });
                    await _permissionUserLogic.DeletePrivilegeMasterUser(input.UserId, EnumPrivilegeMaster.角色);
                    if (input.RoleId.IsNotNullOrEmpty())
                    {
                        await _permissionUserLogic.SavePermissionMasterValueBeforeDelete(EnumPrivilegeMaster.角色, input.UserId, input.RoleId.Split(',').Select(s => Guid.Parse(s)).ToList());
                    }
                }
                return operateStatus;
            }
            return operateStatus;
        }

        /// <summary>
        /// 生成头像图片
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private async Task<string> CreateHeadImage(SystemUserSaveInput input)
        {
            //生成到临时文件夹
            var filePath = Path.Combine(ConfigurationUtil.GetTempPath(), $"{input.UserId}.png");
            ImageUtil.CreateImageText(input.Name, filePath);
            var filesStorageOptions = (await _systemConfigLogic.FindFilesStorageOptions()).Data;
            var FileBytes = SystemIo.File.ReadAllBytes(filePath);
            using (var ms = new SystemIo.MemoryStream(FileBytes))
            {
                IFormFile file = new FormFile(ms, 0, ms.Length,
                    SystemIo.Path.GetFileNameWithoutExtension(filePath),
                    SystemIo.Path.GetFileName(filePath));
                var fileName = file.FileName;
                var fileExt = SystemIo.Path.GetExtension(fileName).ToLowerInvariant();
                OssUtil ossUtil = new OssUtil();
                var result = await ossUtil.UpLoadFile(filesStorageOptions, fileExt, file);
                FileUtil.DeleteFile(filePath);
                return result;
            }
        }

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="input">用户id</param>
        /// <returns></returns>
        public async Task<OperateStatus> Delete(IdInput<string> input)
        {
            OperateStatus operateStatus = new OperateStatus();
            using (var fixture = new SqlDatabaseFixture())
            {
                var trans = fixture.Db.BeginTransaction();
                try
                {
                    foreach (var id in input.Id.Split(','))
                    {
                        var userId = Guid.Parse(id);
                        var userInfo = await FindAsync(f => f.UserId == userId);
                        if (userInfo == null)
                        {
                            operateStatus.Code = ResultCode.Error;
                            operateStatus.Msg = ResourceSystem.人员不存在;
                            return operateStatus;
                        }
                        if (!await fixture.Db.SystemUserInfo.DeleteAsync(d => d.UserId == userId, trans)) continue;
                        operateStatus.Msg = Chs.Successful;
                        operateStatus.Code = ResultCode.Success;
                    }

                    foreach (var id in input.Id.Split(','))
                    {
                        var userId = Guid.Parse(id);
                        await _permissionUserLogic.DeletePrivilegeMasterUser(userId);
                    }
                    trans.Commit();
                }
                catch (Exception e)
                {
                    trans.Rollback();
                    operateStatus.Msg = e.Message;
                }
            }
            return operateStatus;
        }

        /// <summary>
        /// 根据用户Id获取该用户信息
        /// </summary>
        /// <param name="input">用户Id</param>
        /// <returns></returns>
        public async Task<OperateStatus<SystemUserDetailOutput>> FindDetailByUserId(IdInput input)
        {
            using (var fixture = new SqlDatabaseFixture())
            {
                //获取用户基本信息
                var userDto = (await fixture.Db.SystemUserInfo.FindAsync(f => f.UserId == input.Id)).MapTo<SystemUserInfo, SystemUserOutput>();
                var allOrgs = (await fixture.Db.SystemOrganization.SetSelect(s => new { s.OrganizationId, s.ParentIdsName, s.ParentIds, s.Name }).FindAllAsync()).ToList();
                //转换
                var userDetailDto = userDto.MapTo<SystemUserOutput, SystemUserDetailOutput>();
                var userOrg = allOrgs.FirstOrDefault(f => f.OrganizationId == userDto.OrganizationId);
                if (userOrg != null)
                {
                    userDetailDto.OrganizationName = userOrg.ParentIdsName;
                }
                //获取角色、组、岗位数据
                var privilegeMaster = new List<short>
                {
                    EnumPrivilegeMaster.角色.ToShort(),
                    EnumPrivilegeMaster.组.ToShort(),
                    EnumPrivilegeMaster.岗位.ToShort()
                };
                var systemPermissionUsers = (await fixture.Db.SystemPermissionUser.FindAllAsync(f => f.PrivilegeMasterUserId == input.Id && privilegeMaster.Contains(f.PrivilegeMaster))).ToList();
                //角色集合
                var roleId = systemPermissionUsers.Where(w => w.PrivilegeMaster == EnumPrivilegeMaster.角色.ToShort()).Select(s => s.PrivilegeMasterValue).ToList();
                var roles = roleId.Any() ? (await fixture.Db.SystemRole.FindAllAsync(f => roleId.Contains(f.RoleId))).ToList() : new List<SystemRole>();

                //组
                var groupId = systemPermissionUsers.Where(w => w.PrivilegeMaster == EnumPrivilegeMaster.组.ToShort()).Select(s => s.PrivilegeMasterValue).ToList();
                var groups = groupId.Any() ? (await fixture.Db.SystemGroup.FindAllAsync(f => groupId.Contains(f.GroupId))).ToList() : new List<SystemGroup>(); ;

                //岗位
                var postId = systemPermissionUsers.Where(w => w.PrivilegeMaster == EnumPrivilegeMaster.岗位.ToShort()).Select(s => s.PrivilegeMasterValue).ToList();
                var posts = postId.Any() ? (await fixture.Db.SystemPost.FindAllAsync(f => postId.Contains(f.PostId))).ToList() : new List<SystemPost>();

                IList<SystemPrivilegeDetailListOutput> privilegeDetailDtos = new List<SystemPrivilegeDetailListOutput>();
                foreach (var permissionUser in systemPermissionUsers)
                {
                    if (permissionUser.PrivilegeMaster == EnumPrivilegeMaster.角色.ToShort())
                    {
                        //查询角色
                        var role = roles.FirstOrDefault(w => w.RoleId == permissionUser.PrivilegeMasterValue);
                        if (role == null) continue;
                        privilegeDetailDtos.Add(new SystemPrivilegeDetailListOutput
                        {
                            Name = role.Name,
                            OrganizationId = role.OrganizationId,
                            PrivilegeMaster = EnumPrivilegeMaster.角色,
                            PrivilegeMasterValue = permissionUser.PrivilegeMasterValue
                        });
                    }
                    else if (permissionUser.PrivilegeMaster == EnumPrivilegeMaster.组.ToShort())
                    {
                        var group = groups.FirstOrDefault(w => w.GroupId == permissionUser.PrivilegeMasterValue);
                        if (group == null) continue;
                        privilegeDetailDtos.Add(new SystemPrivilegeDetailListOutput
                        {
                            Name = group.Name,
                            OrganizationId = group.OrganizationId,
                            PrivilegeMaster = EnumPrivilegeMaster.组,
                            PrivilegeMasterValue = permissionUser.PrivilegeMasterValue
                        });
                    }
                    else if (permissionUser.PrivilegeMaster == EnumPrivilegeMaster.岗位.ToShort())
                    {
                        var post = posts.FirstOrDefault(w => w.PostId == permissionUser.PrivilegeMasterValue);
                        if (post == null) continue;
                        privilegeDetailDtos.Add(new SystemPrivilegeDetailListOutput
                        {
                            Name = post.Name,
                            OrganizationId = post.OrganizationId,
                            PrivilegeMaster = EnumPrivilegeMaster.岗位,
                            PrivilegeMasterValue = permissionUser.PrivilegeMasterValue
                        });
                    }
                }

                //便利
                foreach (var dto in privilegeDetailDtos)
                {
                    string description = string.Empty;
                    var organization = allOrgs.FirstOrDefault(w => w.OrganizationId == dto.OrganizationId);
                    if (organization != null && !organization.ParentIds.IsNullOrEmpty())
                    {
                        foreach (var parent in organization.ParentIds.Split(','))
                        {
                            //查找上级
                            var dicinfo = allOrgs.FirstOrDefault(w => w.OrganizationId.ToString() == parent);
                            if (dicinfo != null) description += dicinfo.Name + "/";
                        }

                        if (!description.IsNullOrEmpty())
                            description = description.TrimEnd('/');
                    }
                    dto.Organization = description;
                }

                //角色
                userDetailDto.Role = privilegeDetailDtos.Where(w => w.PrivilegeMaster == EnumPrivilegeMaster.角色).ToList();
                //组
                userDetailDto.Group = privilegeDetailDtos.Where(w => w.PrivilegeMaster == EnumPrivilegeMaster.组).ToList();
                //岗位
                userDetailDto.Post = privilegeDetailDtos.Where(w => w.PrivilegeMaster == EnumPrivilegeMaster.岗位).ToList();
                return OperateStatus<SystemUserDetailOutput>.Success(userDetailDto);
            }
        }

        /// <summary>
        /// 根据用户Id重置某人密码
        /// </summary>
        /// <param name="input">用户Id</param>
        /// <returns></returns>
        public async Task<OperateStatus> ResetPassword(SystemUserResetPasswordInput input)
        {
            var operateStatus = new OperateStatus();
            //将传入的密码加密
            var encryptPwd = DEncryptUtil.Encrypt(input.EncryptPassword, ConfigurationUtil.GetPasswordKey());
            //获取人员信息
            var user = await FindAsync(f => f.UserId == input.Id);
            if (user == null)
            {
                operateStatus.Msg = ResourceSystem.人员不存在;
                return operateStatus;
            }
            //查看密码强度规则是否满足
            operateStatus = await CheckPasswordRule(new SystemUserCheckPasswordRuleInput
            {
                Password = input.EncryptPassword
            });
            if (operateStatus.Code == ResultCode.Error)
            {
                return operateStatus;
            }

            user.Password = encryptPwd;
            operateStatus = await UpdateAsync(user);
            if (operateStatus.Code == ResultCode.Success)
            {
                operateStatus.Msg = string.Format(ResourceSystem.重置密码成功, input.EncryptPassword);


            }
            return operateStatus;
        }

        /// <summary>
        /// 保存用户头像
        /// </summary>
        /// <param name="input">用户头像</param>
        /// <returns></returns>
        public async Task<OperateStatus<string>> SaveHeadImage(SystemUserSaveHeadImageInput input)
        {
            var operateStatus = new OperateStatus<string>();
            var filePath = Path.Combine(ConfigurationUtil.GetTempPath(), $"{Guid.NewGuid()}.png");
            var hearderpath = "";
            //转换并保存图片
            input.HeadImage.ConvertBase64ToImage(filePath);
            var filesStorageOptions = (await _systemConfigLogic.FindFilesStorageOptions()).Data;
            var FileBytes = SystemIo.File.ReadAllBytes(filePath);
            using (var ms = new SystemIo.MemoryStream(FileBytes))
            {
                IFormFile file = new FormFile(ms, 0, ms.Length,
                    SystemIo.Path.GetFileNameWithoutExtension(filePath),
                    SystemIo.Path.GetFileName(filePath));
                var fileName = file.FileName;
                var fileExt = SystemIo.Path.GetExtension(fileName).ToLowerInvariant();
                OssUtil ossUtil = new OssUtil();
                hearderpath = await ossUtil.UpLoadFile(filesStorageOptions, fileExt, file);
                FileUtil.DeleteFile(filePath);
            }
            //获取人员信息
            var user = await FindAsync(f => f.UserId == input.UserId);
            if (user == null)
            {
                operateStatus.Msg = ResourceSystem.人员不存在;
                return operateStatus;
            }
            user.HeadImage = hearderpath + "?t=" + DateTimeUtil.GetTimeStamp(); ;
            await UpdateAsync(user);
            operateStatus.Code = ResultCode.Success;
            operateStatus.Msg = Chs.Successful;
            operateStatus.Data = hearderpath;
            return operateStatus;
        }

        /// <summary>
        /// 保存修改后密码信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus> SaveChangePassword(SystemUserChangePasswordInput input)
        {
            var operateStatus = new OperateStatus();
            //后台再次验证是否一致
            if (!input.NewPassword.Equals(input.ConfirmNewPassword))
            {
                operateStatus.Msg = "录入的新密码和确认密码不一致。";
                return operateStatus;
            }
            if (input.OldPassword == input.ConfirmNewPassword)
            {
                operateStatus.Msg = "旧密码不能与新密码一致。";
                return operateStatus;
            }
            //旧密码是否正确
            operateStatus = await CheckOldPassword(new CheckSameValueInput { Id = input.UserId, Param = input.OldPassword });
            if (operateStatus.Code == ResultCode.Error)
            {
                return operateStatus;
            }
            //查看密码强度规则是否满足
            operateStatus = await CheckPasswordRule(new SystemUserCheckPasswordRuleInput
            {
                Password = input.NewPassword
            });
            if (operateStatus.Code == ResultCode.Error)
            {
                return operateStatus;
            }
            //将传入的密码加密
            var encryptPwd = DEncryptUtil.Encrypt(input.NewPassword, ConfigurationUtil.GetPasswordKey());
            //获取人员信息
            var user = await FindAsync(f => f.UserId == input.UserId);
            if (user == null)
            {
                operateStatus.Msg = ResourceSystem.人员不存在;
                return operateStatus;
            }
            user.Password = encryptPwd;
            operateStatus = await UpdateAsync(user);
            if (operateStatus.Code == ResultCode.Success)
            {
                operateStatus.Msg = string.Format(ResourceSystem.修改密码成功, input.NewPassword);

                //写入修改历史
                using (var fix = new SqlDatabaseFixture())
                {
                    SystemChangePassword systemChangePassword = new SystemChangePassword
                    {
                        ChangePasswordId = CombUtil.NewComb(),
                        Password = encryptPwd,
                        CreateTime = DateTime.Now,
                        CreateUserId = input.UserId,
                        CreateUserName = user.Name
                    };
                    await fix.Db.SystemChangePassword.InsertAsync(systemChangePassword);
                }
            }
            return operateStatus;
        }

        /// <summary>
        /// 检测密码是否满足规则
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<OperateStatus> CheckPasswordRule(SystemUserCheckPasswordRuleInput input)
        {
            var operateStatus = new OperateStatus();
            var systemPasswordLengthConfig = await _systemConfigLogic.FindByKey(EnumConfigKey.systemPasswordLength.ToString());
            if (systemPasswordLengthConfig.IsNotNullOrEmpty())
            {
                var systemPasswordLength = Convert.ToInt32(systemPasswordLengthConfig);
                if (input.Password.Length < systemPasswordLength)
                {
                    operateStatus.Msg = $"密码长度需大于等于{systemPasswordLength}个字符串";
                    return operateStatus;
                }
            }

            var systemPasswordHaveNumberConfig = await _systemConfigLogic.FindByKey(EnumConfigKey.systemPasswordHaveNumber.ToString());
            if (systemPasswordHaveNumberConfig.IsNotNullOrEmpty() && Convert.ToBoolean(systemPasswordHaveNumberConfig))
            {
                var systemPasswordHaveNumberLength = await _systemConfigLogic.FindByKey(EnumConfigKey.systemPasswordHaveNumberLength.ToString());
                var systemPasswordHaveNumber = Regex.Replace(input.Password, @"[^0-9]+", ""); ;
                if (systemPasswordHaveNumber.Length < Convert.ToInt32(systemPasswordHaveNumberLength))
                {
                    operateStatus.Msg = $"必须包含至少{Convert.ToInt32(systemPasswordHaveNumberLength)}个数字";
                    return operateStatus;
                }
            }

            var systemPasswordHaveLettersConfig = await _systemConfigLogic.FindByKey(EnumConfigKey.systemPasswordHaveLetters.ToString());
            if (systemPasswordHaveLettersConfig.IsNotNullOrEmpty() && Convert.ToBoolean(systemPasswordHaveLettersConfig))
            {
                var systemPasswordHaveLettersLength = await _systemConfigLogic.FindByKey(EnumConfigKey.systemPasswordHaveLettersLength.ToString());
                var systemPasswordHaveLetters = Regex.Replace(input.Password, @"[^A-Za-z]+", ""); ;
                if (systemPasswordHaveLetters.Length < Convert.ToInt32(systemPasswordHaveLettersLength))
                {
                    operateStatus.Msg = $"必须包含至少{Convert.ToInt32(systemPasswordHaveLettersLength)}个字母";
                    return operateStatus;
                }
            }

            var systemPasswordHaveSpecialCharactersConfig = await _systemConfigLogic.FindByKey(EnumConfigKey.systemPasswordHaveSpecialCharacters.ToString());
            if (systemPasswordHaveSpecialCharactersConfig.IsNotNullOrEmpty() && Convert.ToBoolean(systemPasswordHaveLettersConfig))
            {
                var systemPasswordHaveSpecialCharactersLength = await _systemConfigLogic.FindByKey(EnumConfigKey.systemPasswordHaveSpecialCharactersLength.ToString());
                var systemPasswordHaveSpecialCharacters = Regex.Replace(input.Password, @"[^0-9a-zA-Z\u4e00-\u9fa5]+", ""); ;
                if ((input.Password.Length - systemPasswordHaveSpecialCharacters.Length) < Convert.ToInt32(systemPasswordHaveSpecialCharactersLength))
                {
                    operateStatus.Msg = $"必须包含至少{Convert.ToInt32(systemPasswordHaveSpecialCharactersLength)}个特殊字符";
                    return operateStatus;
                }
            }
            operateStatus.Code = ResultCode.Success;
            operateStatus.Msg = Chs.Successful;
            return operateStatus;
        }

        /// <summary>
        /// 验证旧密码是否输入正确
        /// </summary>
        /// <param name="input">需要验证的参数</param>
        /// <returns></returns>
        public async Task<OperateStatus> CheckOldPassword(CheckSameValueInput input)
        {
            var operateStatus = new OperateStatus();
            input.Param = DEncryptUtil.Encrypt(input.Param, ConfigurationUtil.GetPasswordKey());
            if ((await CountAsync(c => c.Password == input.Param && c.UserId == input.Id)) == 0)
            {
                operateStatus.Code = ResultCode.Error;
                operateStatus.Msg = ResourceSystem.旧密码不正确;
            }
            else
            {
                operateStatus.Code = ResultCode.Success;
            }
            return operateStatus;
        }

        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="input">分页参数</param>
        /// <returns></returns>
        public async Task<OperateStatus<IList<SystemUserFindOrganizationUserOutput>>> FindOrganizationUser()
        {
            IList<SystemUserFindOrganizationUserOutput> organizationUserOutputs = new List<SystemUserFindOrganizationUserOutput>();
            using (var fix = new SqlDatabaseFixture())
            {
                var orgs = (await fix.Db.SystemOrganization.FindAllAsync()).OrderBy(o => o.OrderNo);

                var users = (await _userInfoRepository.Find(new SystemUserPagingInput
                {
                    PrivilegeMaster = EnumPrivilegeMaster.组织架构,
                    Size = int.MaxValue
                })).Data.ToList();
                foreach (var item in orgs)
                {
                    SystemUserFindOrganizationUserOutput organizationUserOutput = new SystemUserFindOrganizationUserOutput
                    {
                        OrganizationId = item.OrganizationId,
                        Name = item.Name,
                        ParentId = item.ParentId,
                        ParentIdsName = item.ParentIdsName,
                        ParentIds = item.ParentIds,
                        ParentName = item.ParentName,
                        ShortName = item.ShortName
                    };
                    var user = users.Where(w => w.OrganizationId == item.OrganizationId);
                    foreach (var u in user)
                    {
                        organizationUserOutput.Users.Add(new FindOrganizationUserOutput
                        {
                            UserId = u.UserId,
                            OrganizationId = u.OrganizationId,
                            Code = u.Code,
                            Name = u.Name,
                            Mobile = u.Mobile,
                            ParentIdsName = u.ParentIdsName,
                            Email = u.Email,
                            OtherContactInformation = u.OtherContactInformation,
                            Remark = u.Remark,
                            HeadImage = u.HeadImage
                        });
                    }
                    organizationUserOutputs.Add(organizationUserOutput);
                }
            }
            return OperateStatus<IList<SystemUserFindOrganizationUserOutput>>.Success(organizationUserOutputs);
        }

        /// <summary>
        /// 获取组织机构人员树
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<JsTreeEntity>>> FindOrganizationUserTree()
        {
            using (var fix = new SqlDatabaseFixture())
            {
                var orgs = await fix.Db.SystemOrganization.FindAllAsync(f => f.IsFreeze == false);
                var users = (await FindAllAsync(f => f.IsFreeze == false)).ToList();
                IList<JsTreeEntity> treeEntities = new List<JsTreeEntity>();
                foreach (var org in orgs)
                {
                    treeEntities.Add(new JsTreeEntity
                    {
                        id = org.OrganizationId,
                        parent = org.ParentId == Guid.Empty ? "#" : org.ParentId.ToString(),
                        text = org.Name,
                        icon = "fa fa-cubes"
                    });
                    //查找人员
                    var user = users.Where(w => w.OrganizationId == org.OrganizationId);
                    foreach (var item in user)
                    {
                        treeEntities.Add(new JsTreeEntity
                        {
                            id = item.UserId,
                            parent = org.OrganizationId,
                            text = item.Name + $"({item.Code})",
                            icon = "fa fa-user"
                        });
                    }
                }
                return OperateStatus<IEnumerable<JsTreeEntity>>.Success(treeEntities);
            }
        }

        /// <summary>
        /// 根据用户Id获取
        /// </summary>
        /// <param name="input">用户Id</param>
        /// <returns></returns>
        public async Task<OperateStatus<SystemUserFindHeadByIdOutput>> FindById(IdInput input)
        {
            var user = (await FindAsync(f => f.UserId == input.Id)).MapTo<SystemUserInfo, SystemUserFindHeadByIdOutput>();
            return OperateStatus<SystemUserFindHeadByIdOutput>.Success(user);
        }

        /// <summary>
        /// 冻结
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus> IsFreeze(IdInput input)
        {
            var data = await FindAsync(f => f.UserId == input.Id);
            data.IsFreeze = !data.IsFreeze;
            return await UpdateAsync(data);
        }
        #endregion

        #region 登录

        /// <summary>
        /// 生成验证码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public OperateStatus<SystemIo.MemoryStream> Captcha(IdInput input)
        {
            try
            {
                var info = _captcha.Generate(input.Id.ToString());
                var stream = new SystemIo.MemoryStream(info.Bytes);
                return OperateStatus<SystemIo.MemoryStream>.Success(stream);
            }
            catch (Exception)
            {
                return OperateStatus<SystemIo.MemoryStream>.Error("生成异常");
            }
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<SystemLoginOutput>> Login(SystemLoginInput input)
        {
            RSACryptoService loginRsa = new RSACryptoService(ResourceKey.加密私钥, ResourceKey.加密公钥);
            try
            {
                input.Code = loginRsa.Decrypt(input.Code);
                input.Password = loginRsa.Decrypt(input.Password);
            }
            catch (Exception ex)
            {
                return OperateStatus<SystemLoginOutput>.Error("用户名或密码错误");
            }

            var configs = await _systemConfigLogic.FindByKey(EnumConfigKey.loginCaptcha.ToString());
            if (configs.IsNotNullOrEmpty())
            {
                var loginCaptcha = bool.Parse(configs);
                //是否具有登录验证码
                if (loginCaptcha)
                {
                    try
                    {
                        input.Captcha = loginRsa.Decrypt(input.Captcha);
                    }
                    catch (Exception ex)
                    {
                        return OperateStatus<SystemLoginOutput>.Error("验证码错误");
                    }
                    //查看验证码是否正确
                    var checkCaptcha = _captcha.Validate(input.LoginId, input.Captcha);
                    if (!checkCaptcha)
                    {
                        return OperateStatus<SystemLoginOutput>.Error("验证码错误");
                    }
                }
            }
            var code = input.Code;
            var checkLock = CheckLock(code);
            if (checkLock.Code != ResultCode.Success)
            {
                return checkLock;
            }

            //将传入的密码加密
            var encryptPwd = DEncryptUtil.Encrypt(input.Password, ConfigurationUtil.GetSection("EIP:PasswordKey"));
            //查询信息
            input.Password = encryptPwd;
            var systemUserInfo = new SystemUserInfo();
            var systemLoginOutput = new SystemLoginOutput();
            using (var fix = new SqlDatabaseFixture())
            {
                var data = await fix.Db.SystemUserInfo.SetSelect(s => new
                {
                    s.UserId,
                    s.OrganizationId,
                    s.OrganizationName,
                    s.Code,
                    s.Name,
                    s.IsFreeze,
                    s.IsAdmin,
                    s.HeadImage
                }).FindAsync(f => f.Code == input.Code && f.Password == encryptPwd);
                //是否存在
                if (data == null)
                {
                    return OperateStatus<SystemLoginOutput>.Error("用户名或密码错误");
                }
                var operateStatus = await GetSystemLoginOutput(data, fix);
                if (operateStatus.Code == ResultCode.Success)
                {
                    var numberLockMinuteKey = "SystemUserInfoErrorNumberMinute:" + code;
                    var numberLockKey = "SystemUserInfoErrorNumberLock:" + code;
                    //删除缓存
                    RedisHelper.Del(numberLockMinuteKey);
                    RedisHelper.Del(numberLockKey);

                    systemLoginOutput = operateStatus.Data;
                }
            }
            return OperateStatus<SystemLoginOutput>.Success(systemLoginOutput);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fix"></param>
        /// <returns></returns>
        private OperateStatus<SystemLoginOutput> CheckLock(string code)
        {
            OperateStatus<SystemLoginOutput> operateStatus = new OperateStatus<SystemLoginOutput>();
            //查询当前用户是否已被锁定
            var numberLockMinuteKey = "SystemUserInfoErrorNumberMinute:" + code;
            var numberLockHaveMinute = RedisHelper.Get(numberLockMinuteKey);
            if (numberLockHaveMinute.IsNotNullOrEmpty())
            {
                string error = $"错误次数过多,帐号已被锁定。请在{numberLockHaveMinute}后重试";
                if (numberLockHaveMinute == "0")
                {
                    error = "错误次数过多,帐号已被锁定。请联系管理员解除锁定";
                }
                return OperateStatus<SystemLoginOutput>.Error(error);
            }
            operateStatus.Code = ResultCode.Success;
            operateStatus.Msg = Chs.QuerySuccessful;
            return operateStatus;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fix"></param>
        /// <returns></returns>
        private async Task<OperateStatus<SystemLoginOutput>> GetSystemLoginOutput(SystemUserInfo data, SqlDatabaseFixture fix)
        {
            OperateStatus<SystemLoginOutput> operateStatus = new OperateStatus<SystemLoginOutput>();
            SystemLoginOutput systemLoginOutput = new SystemLoginOutput();
            //是否冻结
            if (data.IsFreeze)
            {
                return OperateStatus<SystemLoginOutput>.Error("登录用户已冻结");
            }
            //更新用户最后一次登录时间
            await fix.Db.SystemUserInfo.UpdateAsync(u => u.UserId == data.UserId, new { LastVisitTime = DateTime.Now });
            systemLoginOutput.UserId = data.UserId;
            systemLoginOutput.Code = data.Code;
            systemLoginOutput.OrganizationId = data.OrganizationId;
            systemLoginOutput.OrganizationName = data.OrganizationName;
            systemLoginOutput.Name = data.Name;
            systemLoginOutput.IsFreeze = data.IsFreeze;
            systemLoginOutput.IsAdmin = data.IsAdmin;
            systemLoginOutput.HeadImage = data.HeadImage;
            systemLoginOutput.LoginId = CombUtil.NewComb();

            //判断是否已修改密码
            var changePassword = await fix.Db.SystemChangePassword.SetOrderBy(OrderInfo.SortDirection.DESC, o => o.CreateTime).FindAsync(f => f.CreateUserId == data.UserId);
            if (changePassword == null)
            {
                systemLoginOutput.ChangePassword = true;
                systemLoginOutput.ChangePasswordType = EnumChangePasswordType.第一次登录修改密码.ToShort();
            }
            else
            {
                //是否到修改密码时间
                var changePasswordDay = await _systemConfigLogic.FindByKey(EnumConfigKey.systemPasswordCompelChangeDay.ToString());
                if (changePasswordDay.IsNotNullOrEmpty())
                {
                    var days = changePassword.CreateTime.AddDays(Convert.ToInt32(changePasswordDay));
                    if (days <= DateTime.Now)
                    {
                        systemLoginOutput.ChangePassword = true;
                        systemLoginOutput.ChangePasswordType = EnumChangePasswordType.超过系统规定修改密码时间修改密码.ToShort();
                    }
                }
            }
            operateStatus.Code = ResultCode.Success;
            operateStatus.Msg = Chs.QuerySuccessful;
            operateStatus.Data = systemLoginOutput;
            return operateStatus;
        }

        /// <summary>
        /// 根据用户Id获取
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<OperateStatus<SystemLoginOutput>> LoginByUserId(SystemLoginByUserIdInput input)
        {
            var operateStatus = new OperateStatus<SystemLoginOutput>();
            var userId = Guid.Empty;
            RSACryptoService loginRsa = new RSACryptoService(ResourceKey.加密私钥, ResourceKey.加密公钥);
            try
            {
                userId = Guid.Parse(loginRsa.Decrypt(input.UserId));
            }
            catch (Exception ex)
            {
                return OperateStatus<SystemLoginOutput>.Error("用户名或密码错误");
            }

            using (var fix = new SqlDatabaseFixture())
            {
                var data = (await fix.Db.SystemUserInfo.SetSelect(s => new
                {
                    s.UserId,
                    s.OrganizationId,
                    s.OrganizationName,
                    s.Code,
                    s.Name,
                    s.IsFreeze,
                    s.IsAdmin,
                    s.HeadImage
                }).FindAsync(
                    f => f.UserId == userId));

                //是否存在
                if (data == null)
                {
                    return OperateStatus<SystemLoginOutput>.Error("用户名或密码错误");
                }
                var code = data.Code;
                var checkLock = CheckLock(code);
                if (checkLock.Code != ResultCode.Success)
                {
                    return checkLock;
                }
                SystemLoginOutput systemLoginOutput = new SystemLoginOutput();
                var getSystemLoginOutput = await GetSystemLoginOutput(data, fix);
                if (getSystemLoginOutput.Code == ResultCode.Success)
                {
                    var numberLockMinuteKey = "SystemUserInfoErrorNumberMinute:" + code;
                    var numberLockKey = "SystemUserInfoErrorNumberLock:" + code;
                    //删除缓存
                    RedisHelper.Del(numberLockMinuteKey);
                    RedisHelper.Del(numberLockKey);
                    systemLoginOutput = getSystemLoginOutput.Data;
                }
                systemLoginOutput.LoginId = CombUtil.NewComb();
                return OperateStatus<SystemLoginOutput>.Success(systemLoginOutput);
            }
        }

        /// <summary>
        /// 根据用户帐号登录
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<OperateStatus<SystemLoginOutput>> LoginByCode(SystemLoginByCodeInput input)
        {
            var operateStatus = new OperateStatus<SystemLoginOutput>();

            using (var fix = new SqlDatabaseFixture())
            {
                var data = (await fix.Db.SystemUserInfo.SetSelect(s => new
                {
                    s.UserId,
                    s.OrganizationId,
                    s.OrganizationName,
                    s.Code,
                    s.Name,
                    s.IsFreeze,
                    s.IsAdmin,
                    s.HeadImage
                }).FindAsync(
                    f => f.Code == input.Code));

                //是否存在
                if (data == null)
                {
                    return OperateStatus<SystemLoginOutput>.Error("用户名或密码错误");
                }
                var code = data.Code;
                var checkLock = CheckLock(code);
                if (checkLock.Code != ResultCode.Success)
                {
                    return checkLock;
                }
                SystemLoginOutput systemLoginOutput = new SystemLoginOutput();
                var getSystemLoginOutput = await GetSystemLoginOutput(data, fix);
                if (getSystemLoginOutput.Code == ResultCode.Success)
                {
                    var numberLockMinuteKey = "SystemUserInfoErrorNumberMinute:" + code;
                    var numberLockKey = "SystemUserInfoErrorNumberLock:" + code;
                    //删除缓存
                    RedisHelper.Del(numberLockMinuteKey);
                    RedisHelper.Del(numberLockKey);
                    systemLoginOutput = getSystemLoginOutput.Data;
                }
                systemLoginOutput.LoginId = CombUtil.NewComb();
                return OperateStatus<SystemLoginOutput>.Success(systemLoginOutput);
            }
        }

        /// <summary>
        /// 根据钉钉临时授权码登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<SystemLoginOutput>> LoginDingTalk(SystemLoginByDingTalkInput input)
        {
            try
            {
                //解密
                RSACryptoService loginRsa = new RSACryptoService(ResourceKey.加密私钥, ResourceKey.加密公钥);
                input.Code = loginRsa.Decrypt(input.Code);
            }
            catch (Exception ex)
            {
                return OperateStatus<SystemLoginOutput>.Error("提交数据有误请重试");
            }
            var operateStatus = new OperateStatus<SystemLoginOutput>();
            try
            {
                var appKey = await _systemConfigLogic.FindByKey(EnumConfigKey.dingTalkAppKey.ToString());
                var appSecret = await _systemConfigLogic.FindByKey(EnumConfigKey.dingTalkAppSecret.ToString());

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

                IDingTalkClient clientGetUserInfo = new DefaultDingTalkClient("https://oapi.dingtalk.com/topapi/v2/user/getuserinfo");
                OapiV2UserGetuserinfoRequest req = new OapiV2UserGetuserinfoRequest();
                req.Code = input.Code;
                OapiV2UserGetuserinfoResponse rsp = clientGetUserInfo.Execute(req, accessToken.Body.AccessToken);
                if (rsp.Errmsg != "ok")
                {
                    operateStatus.Msg = rsp.Errmsg;
                    return operateStatus;
                }
                using (var fix = new SqlDatabaseFixture())
                {
                    var dingTalk = await fix.Db.SystemUserInfoThree.FindAsync(f => f.PlatformUserId == rsp.Result.Userid);
                    if (dingTalk == null)
                    {
                        return OperateStatus<SystemLoginOutput>.Error("未找到用户信息");
                    }
                    var data = (await fix.Db.SystemUserInfo.SetSelect(s => new
                    {
                        s.UserId,
                        s.OrganizationId,
                        s.OrganizationName,
                        s.Code,
                        s.Name,
                        s.IsFreeze,
                        s.IsAdmin,
                        s.HeadImage
                    }).FindAsync(
                        f => f.UserId == dingTalk.SystemUserId));

                    //是否存在
                    if (data == null)
                    {
                        return OperateStatus<SystemLoginOutput>.Error("用户名或密码错误");
                    }
                    var code = data.Code;
                    var checkLock = CheckLock(code);
                    if (checkLock.Code != ResultCode.Success)
                    {
                        return checkLock;
                    }
                    SystemLoginOutput systemLoginOutput = new SystemLoginOutput();
                    var getSystemLoginOutput = await GetSystemLoginOutput(data, fix);
                    if (getSystemLoginOutput.Code == ResultCode.Success)
                    {
                        var numberLockMinuteKey = "SystemUserInfoErrorNumberMinute:" + code;
                        var numberLockKey = "SystemUserInfoErrorNumberLock:" + code;
                        //删除缓存
                        RedisHelper.Del(numberLockMinuteKey);
                        RedisHelper.Del(numberLockKey);
                        systemLoginOutput = getSystemLoginOutput.Data;
                    }
                    systemLoginOutput.LoginId = CombUtil.NewComb();
                    return OperateStatus<SystemLoginOutput>.Success(systemLoginOutput);
                }
            }
            catch (Exception ex)
            {
                operateStatus.Msg = ex.Message;
                return operateStatus;
            }
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public OperateStatus LoginOut(IdInput input)
        {
            OperateStatus operateStatus = new OperateStatus();
            LoginOutLogHandler handler = new LoginOutLogHandler(new PrincipalUser
            { }, input.Id);
            handler.WriteLog();
            operateStatus.Code = ResultCode.Success;
            operateStatus.Msg = ResourceSystem.退出成功;
            return operateStatus;
        }

        /// <summary>
        /// 构建Oauth授权Url方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<OperateStatus<SystemLoginOutput>> OauthCallBack(SystemUserOauthCallBackInput input, string userId)
        {
            OperateStatus<SystemLoginOutput> operateStatus = new OperateStatus<SystemLoginOutput>();
            //判断类型
            SystemLoginOutput systemUserInfo = null;
            using (var fix = new SqlDatabaseFixture())
            {
                if (input.AuthSource == "WECHAT_ENTERPRISE_SCAN")
                {
                    //结合微信公众号授权表查看
                    var wechatWork = await fix.Db.SystemUserInfoThree.FindAsync(f => f.PlatformUserId == userId);
                    if (wechatWork != null)
                    {
                        var systemUserId = wechatWork.SystemUserId;
                        if (wechatWork.SystemUserId != Guid.Empty)
                        {
                            systemUserInfo = (await fix.Db.SystemUserInfo.SetSelect(s => new
                            {
                                s.UserId,
                                s.OrganizationId,
                                s.OrganizationName,
                                s.Code,
                                s.Name,
                                s.IsFreeze,
                                s.IsAdmin,
                                s.HeadImage
                            }).FindAsync(f => f.UserId == systemUserId)).MapTo<SystemUserInfo, SystemLoginOutput>();
                        }
                    }
                }
                else if (input.AuthSource == "DINGTALK_SCAN")
                {
                    var dingTalk = await fix.Db.SystemUserInfoThree.FindAsync(f => f.UnionId == userId);
                    if (dingTalk != null)
                    {
                        var systemUserId = dingTalk.SystemUserId;
                        systemUserInfo = (await fix.Db.SystemUserInfo.SetSelect(s => new
                        {
                            s.UserId,
                            s.OrganizationId,
                            s.OrganizationName,
                            s.Code,
                            s.Name,
                            s.IsFreeze,
                            s.IsAdmin,
                            s.HeadImage
                        }).FindAsync(f => f.UserId == systemUserId)).MapTo<SystemUserInfo, SystemLoginOutput>();
                    }
                }
                if (systemUserInfo == null)
                {
                    //有可能使用代码一致登录
                    systemUserInfo = (await fix.Db.SystemUserInfo.SetSelect(s => new
                    {
                        s.UserId,
                        s.OrganizationId,
                        s.OrganizationName,
                        s.Code,
                        s.Name,
                        s.IsFreeze,
                        s.IsAdmin,
                        s.HeadImage
                    }).FindAsync(f => f.Code == userId)).MapTo<SystemUserInfo, SystemLoginOutput>();
                }
                //是否存在
                if (systemUserInfo == null)
                {
                    return OperateStatus<SystemLoginOutput>.Error("用户不存在");
                }
                //是否冻结
                if (systemUserInfo.IsFreeze)
                {
                    return OperateStatus<SystemLoginOutput>.Error("登录用户已冻结");
                }
                //更新用户最后一次登录时间
                await fix.Db.SystemUserInfo.UpdateAsync(u => u.UserId == systemUserInfo.UserId, new { LastVisitTime = DateTime.Now });
                systemUserInfo.LoginId = CombUtil.NewComb();
            }
            return OperateStatus<SystemLoginOutput>.Success(systemUserInfo);
        }

        #endregion

        #region 注册
        /// <summary>
        /// 导入用户
        /// </summary>
        /// <param name="input">集合信息</param>
        /// <returns></returns>
        public async Task<OperateStatus> Register(SystemUserRegisterInput input)
        {
            var operateStatus = new OperateStatus();
            //解密密码
            RSACryptoService registerRsa = new RSACryptoService(ResourceKey.加密私钥, ResourceKey.加密公钥);
            try
            {
                input.Password = registerRsa.Decrypt(input.Password);
                input.OpenId = registerRsa.Decrypt(input.OpenId);
            }
            catch (Exception ex)
            {
                operateStatus.Msg = "提交数据异常,请稍后重试";
                return operateStatus;
            }

            using (var fix = new SqlDatabaseFixture())
            {
                //判断是否已存在用户帐号
                var have = await fix.Db.SystemUserInfo.FindAsync(f => f.Mobile == input.Mobile || f.Code == input.Mobile);
                if (have != null)
                {
                    operateStatus.Msg = "电话号码已存在";
                    return operateStatus;
                }
                var encryptPwd = DEncryptUtil.Encrypt(input.Password, ConfigurationUtil.GetPasswordKey());
                //添加人员及角色
                SystemUserInfo userInfoMap = new SystemUserInfo()
                {
                    UserId = CombUtil.NewComb(),
                    OrganizationId = input.OrganizationId,
                    OrganizationName = input.OrganizationName,
                    Mobile = input.Mobile,
                    Code = input.Mobile,
                    Password = encryptPwd,
                    Name = input.Name,
                    FirstVisitTime = DateTime.Now
                };

                operateStatus = await InsertAsync(userInfoMap);
                if (operateStatus.Code == ResultCode.Success)
                {
                    IList<Guid> organizationId = new List<Guid>();
                    organizationId.Add(input.OrganizationId);
                    operateStatus = await _permissionUserLogic.SavePermissionMasterValueBeforeDelete(EnumPrivilegeMaster.组织架构, userInfoMap.UserId, organizationId);
                    operateStatus = await _permissionUserLogic.SavePermissionMasterValueBeforeDelete(EnumPrivilegeMaster.人员, userInfoMap.UserId, new List<Guid> { userInfoMap.UserId });
                    var roleId = "515f231d-af2b-4acc-8783-315d201cc23f";
                    await _permissionUserLogic.SavePermissionMasterValueBeforeDelete(EnumPrivilegeMaster.角色, userInfoMap.UserId, roleId.Split(',').Select(s => Guid.Parse(s)).ToList());

                    //写入微信人员表
                    var weChatUser = await fix.Db.SystemUserInfoThree.FindAsync(f => f.PlatformUserId == input.OpenId);
                    if (weChatUser != null)
                    {
                        weChatUser.SystemUserId = userInfoMap.UserId;
                        await fix.Db.SystemUserInfoThree.UpdateAsync(weChatUser);
                    }

                    if (operateStatus.Code == ResultCode.Success)
                    {
                        operateStatus.Msg = "注册成功";
                        return operateStatus;
                    }
                }
                else
                {
                    return operateStatus;
                }
            }
            return operateStatus;
        }
        #endregion

        /// <summary>
        /// Excel导出方式
        /// </summary>
        /// <param name="paging">查询参数</param>
        /// <param name="excelReportDto"></param>
        /// <returns></returns>
        public async Task<OperateStatus> ReportExcelUserQuery(SystemUserPagingInput paging,
            ExcelReportDto excelReportDto)
        {
            var operateStatus = new OperateStatus();
            try
            {
                //组装数据
                IList<SystemUserOutput> dtos = (await _userInfoRepository.Find(paging)).Data.ToList();
                var tables = new Dictionary<string, DataTable>(StringComparer.OrdinalIgnoreCase);
                //组装需要导出数据
                operateStatus.Code = ResultCode.Success;
            }
            catch (Exception)
            {
                operateStatus.Code = ResultCode.Error;
            }
            return operateStatus;
        }

        /// <summary>
        /// 导入用户
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        public async Task<OperateStatus<List<string>>> Import(IList<SystemUserImportInput> users)
        {
            OperateStatus<List<string>> operateStatus = new OperateStatus<List<string>>();
            if (users.Count == 0)
            {
                operateStatus.Msg = "导入数据为空";
                return operateStatus;
            }
            var codeNull = users.Count(c => c.Code.IsNullOrEmpty());
            if (codeNull > 0)
            {
                operateStatus.Msg = "请确认账号均已填写";
                return operateStatus;
            }
            var nameNull = users.Count(c => c.Name.IsNullOrEmpty());
            if (nameNull > 0)
            {
                operateStatus.Msg = "请确认用户真实姓名均已填写";
                return operateStatus;
            }
            var orgNull = users.Count(c => c.ParentIdsName.IsNullOrEmpty());
            if (orgNull > 0)
            {
                operateStatus.Msg = "请确认组织机构名称均已填写";
                return operateStatus;
            }
            //数据检查
            var currentUser = EipHttpContext.CurrentUser();
            List<SystemUserInfo> systemUserSaveInputs = new List<SystemUserInfo>();
            List<SystemUserImportUserRole> userRole = new List<SystemUserImportUserRole>();
            List<string> errors = new List<string>();
            using (var fix = new SqlDatabaseFixture())
            {
                foreach (var user in users)
                {
                    //组织架构是否存在
                    var orgObj = await fix.Db.SystemOrganization.FindAsync(f => f.ParentIdsName == user.ParentIdsName.Trim());
                    if (orgObj == null)
                    {
                        errors.Add("组织:" + user.ParentIdsName + "不存在");
                    }

                    //检查账号是否存在
                    var count = await CountAsync(c => c.Code == user.Code.Trim());
                    if (count > 0)
                    {
                        errors.Add("账号:" + user.Code + "已存在");
                    }

                    SystemUserInfo input = new SystemUserInfo();
                    //新增
                    if (!user.Code.IsNullOrEmpty())
                    {
                        input.Password = DEncryptUtil.Encrypt("123456", ConfigurationUtil.GetPasswordKey());
                    }

                    input.UserId = CombUtil.NewComb();
                    if (user.Role.IsNotNullOrEmpty())
                    {
                        foreach (var item in user.Role.Split(","))
                        {
                            var role = await fix.Db.SystemRole.FindAsync(f => f.Name == item.Trim());
                            if (role == null)
                            {
                                errors.Add("角色:" + item.Trim() + "已存在");
                            }
                            else
                            {
                                userRole.Add(new SystemUserImportUserRole
                                {
                                    UserId = input.UserId,
                                    RoleId = role.RoleId
                                });
                            }
                        }
                    }
                    input.Code = user.Code.Trim();
                    input.Name = user.Name.Trim();
                    input.Mobile = user.Mobile.Trim();
                    input.OtherContactInformation = user.OtherContactInformation;
                    if (orgObj != null)
                    {
                        input.OrganizationId = orgObj.OrganizationId;
                        input.OrganizationName = orgObj.Name;
                    }
                    input.CreateTime = DateTime.Now;
                    input.CreateUserId = currentUser.UserId;
                    input.CreateUserName = currentUser.Name;

                    input.UpdateTime = DateTime.Now;
                    input.UpdateUserId = currentUser.UserId;
                    input.UpdateUserName = currentUser.Name;
                    systemUserSaveInputs.Add(input);
                }
            }
            if (errors.Any())
            {
                operateStatus.Data = errors;
                return operateStatus;
            }
            var operateStatusResult = new OperateStatus();
            foreach (var userInfoMap in systemUserSaveInputs)
            {
                operateStatusResult = await InsertAsync(userInfoMap);
                if (operateStatusResult.Code == ResultCode.Success)
                {
                    operateStatus.Msg = Chs.Successful;
                    operateStatus.Code = ResultCode.Success;
                    //添加用户到组织机构
                    await _permissionUserLogic.SavePermissionMasterValueBeforeDelete(EnumPrivilegeMaster.组织架构, userInfoMap.UserId, new List<Guid> { userInfoMap.OrganizationId });
                    await _permissionUserLogic.SavePermissionMasterValueBeforeDelete(EnumPrivilegeMaster.人员, userInfoMap.UserId, new List<Guid> { userInfoMap.UserId });
                    //判断是否具有角色
                    var role = userRole.Where(w => w.UserId == userInfoMap.UserId);
                    if (role.Count() > 0)
                    {
                        await _permissionUserLogic.SavePermissionMasterValueBeforeDelete(EnumPrivilegeMaster.角色, userInfoMap.UserId, role.Select(s => s.RoleId).ToList());
                    }
                }
                else
                {
                    return operateStatus;
                }
            }
            return operateStatus;
        }

        /// <summary>
        /// 导入用户
        /// </summary>
        /// <param name="users"></param>
        /// <returns></returns>
        public async Task<OperateStatus<List<string>>> ImportUserMobile(IList<SystemUserImportInput> users)
        {
            OperateStatus<List<string>> operateStatus = new OperateStatus<List<string>>();
            if (users.Count == 0)
            {
                operateStatus.Msg = "导入数据为空";
                return operateStatus;
            }
            var codeNull = users.Count(c => c.Code.IsNullOrEmpty());
            if (codeNull > 0)
            {
                operateStatus.Msg = "请确认账号均已填写";
                return operateStatus;
            }
            var nameNull = users.Count(c => c.Name.IsNullOrEmpty());
            if (nameNull > 0)
            {
                operateStatus.Msg = "请确认用户真实姓名均已填写";
                return operateStatus;
            }
            var orgNull = users.Count(c => c.ParentIdsName.IsNullOrEmpty());
            if (orgNull > 0)
            {
                operateStatus.Msg = "请确认组织机构名称均已填写";
                return operateStatus;
            }
            //数据检查
            var currentUser = EipHttpContext.CurrentUser();
            List<SystemUserInfo> systemUserSaveInputs = new List<SystemUserInfo>();
            List<SystemUserImportUserRole> userRole = new List<SystemUserImportUserRole>();
            List<string> errors = new List<string>();
            using (var fix = new SqlDatabaseFixture())
            {
                foreach (var user in users)
                {
                    user.Code = user.Code.Trim();
                    user.Name = user.Name.Trim();
                    user.ParentIdsName = user.ParentIdsName.Trim();
                    //组织架构是否存在
                    var orgObj = await fix.Db.SystemOrganization.FindAsync(f => f.ParentIdsName == user.ParentIdsName.Trim());
                    if (orgObj == null)
                    {
                        errors.Add("组织:" + user.ParentIdsName + "不存在");
                    }

                    //检查账号是否存在
                    var count = await CountAsync(c => c.Code == user.Code);
                    if (count > 0)
                    {
                        errors.Add("账号:" + user.Code + "已存在");
                    }

                    var input = await fix.Db.SystemUserInfo.FindAsync(f => f.Name == user.Name && f.OrganizationId == orgObj.OrganizationId);
                    if (input != null)
                    {
                        //新增
                        if (!user.Code.IsNullOrEmpty())
                        {
                            input.Password = DEncryptUtil.Encrypt("123456", ConfigurationUtil.GetPasswordKey());
                        }
                        if (user.Role.IsNotNullOrEmpty())
                        {
                            foreach (var item in user.Role.Split(","))
                            {
                                var role = await fix.Db.SystemRole.FindAsync(f => f.Name == item.Trim());
                                if (role == null)
                                {
                                    errors.Add("角色:" + item.Trim() + "已存在");
                                }
                                else
                                {
                                    userRole.Add(new SystemUserImportUserRole
                                    {
                                        UserId = input.UserId,
                                        RoleId = role.RoleId
                                    });
                                }
                            }
                        }
                        input.Code = user.Code.Trim();
                        input.Name = user.Name.Trim();
                        input.Mobile = user.Mobile.Trim();
                        input.OtherContactInformation = user.OtherContactInformation;
                        if (orgObj != null)
                        {
                            input.OrganizationId = orgObj.OrganizationId;
                            input.OrganizationName = orgObj.Name;
                        }
                        systemUserSaveInputs.Add(input);
                    }
                }
            }
            if (errors.Any())
            {
                operateStatus.Data = errors;
                return operateStatus;
            }
            var operateStatusResult = new OperateStatus();
            foreach (var userInfoMap in systemUserSaveInputs)
            {
                operateStatusResult = await UpdateAsync(userInfoMap);
                if (operateStatusResult.Code == ResultCode.Success)
                {
                    operateStatus.Msg = Chs.Successful;
                    operateStatus.Code = ResultCode.Success;
                    //判断是否具有角色
                    var role = userRole.Where(w => w.UserId == userInfoMap.UserId);
                    if (role.Count() > 0)
                    {
                        await _permissionUserLogic.SavePermissionMasterValueBeforeDelete(EnumPrivilegeMaster.角色, userInfoMap.UserId, role.Select(s => s.RoleId).ToList());
                    }
                }
                else
                {
                    return operateStatus;
                }
            }
            return operateStatus;
        }

    }
}