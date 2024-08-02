/**************************************************************
* Copyright (C) www.eipflow.com 孙泽伟版权所有(盗版必究)
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
namespace EIP.System.Logic.Impl
{
    /// <summary>
    /// 系统配置
    /// </summary>
    public class SystemConfigLogic : DapperAsyncLogic<SystemConfig>, ISystemConfigLogic
    {
        /// <summary>
        /// 获取系统配置
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<IEnumerable<SystemConfigFindAllOutput>>> FindAll()
        {
            return OperateStatus<IEnumerable<SystemConfigFindAllOutput>>.Success((await FindAllAsync()).MapToList<SystemConfig, SystemConfigFindAllOutput>());
        }

        /// <summary>
        /// 获取系统配置
        /// </summary>
        /// <returns></returns>
        public async Task<string> FindByKey(string key)
        {
            var data = await FindAsync(f => f.Key == key);
            return data == null ? "" : data.Value;
        }

        /// <summary>
        /// 获取系统配置
        /// </summary>
        /// <returns></returns>
        public async Task<OperateStatus<FilesStorageOptions>> FindFilesStorageOptions()
        {
            var configs = await FindAllAsync();
            var bindUrl = configs.FirstOrDefault(f => f.Key == EnumConfigKey.filesStorageBucketBindUrl.ToString())?.Value;
            bindUrl += bindUrl[bindUrl.Length - 1] == '/' ? "" : "/";
            FilesStorageOptions filesStorageOptions = new FilesStorageOptions()
            {
                StorageType = configs.FirstOrDefault(f => f.Key == EnumConfigKey.filesStorageType.ToString())?.Value,
                Path = configs.FirstOrDefault(f => f.Key == EnumConfigKey.filesStoragePath.ToString())?.Value,
                AccountId = configs.FirstOrDefault(f => f.Key == EnumConfigKey.filesStorageTencentAccountId.ToString())?.Value,
                CosRegion = configs.FirstOrDefault(f => f.Key == EnumConfigKey.filesStorageTencentCosRegion.ToString())?.Value,
                TencentBucketName = configs.FirstOrDefault(f => f.Key == EnumConfigKey.filesStorageTencentBucketName.ToString())?.Value,
                QiNiuBucketName = configs.FirstOrDefault(f => f.Key == EnumConfigKey.filesStorageQiNiuBucketName.ToString())?.Value,
                AccessKeyId = configs.FirstOrDefault(f => f.Key == EnumConfigKey.filesStorageAccessKeyId.ToString())?.Value,
                AccessKeySecret = configs.FirstOrDefault(f => f.Key == EnumConfigKey.filesStorageAccessKeySecret.ToString())?.Value,
                Endpoint = configs.FirstOrDefault(f => f.Key == EnumConfigKey.filesStorageAliYunEndpoint.ToString())?.Value,
                BucketName = configs.FirstOrDefault(f => f.Key == EnumConfigKey.filesStorageAliYunBucketName.ToString())?.Value,
                FileTypes = configs.FirstOrDefault(f => f.Key == EnumConfigKey.filesStorageFileSuffix.ToString())?.Value,
                MaxSize = Convert.ToInt32(configs.FirstOrDefault(f => f.Key == EnumConfigKey.filesStorageFileMaxSize.ToString())?.Value),
                BucketBindUrl = bindUrl
            };
            return OperateStatus<FilesStorageOptions>.Success(filesStorageOptions);
        }
        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="input">系统配置</param>
        /// <returns></returns>
        public async Task<OperateStatus> Save(SystemConfigSaveInput input)
        {
            OperateStatus operate = new OperateStatus();
            await DeleteAsync(d => d.CreateTime != null);
            var currentUser = EipHttpContext.CurrentUser();
            foreach (var item in input.Configs)
            {
                item.ConfigId = CombUtil.NewComb();
                item.CreateTime = DateTime.Now;
                item.CreateUserId = currentUser.UserId;
                item.CreateUserName = currentUser.Name;
                item.UpdateTime = DateTime.Now;
                item.UpdateUserId = currentUser.UserId;
                item.UpdateUserName = currentUser.Name;
            }
            await BulkInsertAsync(input.Configs);
            //获取所有信息
            var all = await FindAllAsync();
            IList<GlobalConfig> globalConfigs = new List<GlobalConfig>();
            foreach (var item in all)
            {
                globalConfigs.Add(new GlobalConfig
                {
                    Key = item.Key,
                    Value = item.Value
                });
            }
            GlobalParams.DeleteValue(globalConfigs);

            #region 动态修改配置文件
            var captchaJsonPath = "Settings/Captcha.json";
            var captchaJson = File.ReadAllText(captchaJsonPath);
            dynamic captchaJsonObj = JsonConvert.DeserializeObject(captchaJson);
            //验证码类型
            captchaJsonObj["CaptchaOptions"]["CaptchaType"] = input.Configs.FirstOrDefault(f => f.Key == EnumConfigKey.loginCaptchaType.ToString()).Value;
            //验证码长度, 要放在CaptchaType设置后  当类型为算术表达式时，长度代表操作的个数, 例如2
            captchaJsonObj["CaptchaOptions"]["CodeLength"] = input.Configs.FirstOrDefault(f => f.Key == EnumConfigKey.loginCaptchaCodeLength.ToString()).Value;
            // 验证码过期秒数
            captchaJsonObj["CaptchaOptions"]["ExpirySeconds"] = input.Configs.FirstOrDefault(f => f.Key == EnumConfigKey.loginCaptchaExpirySeconds.ToString()).Value;
            // 比较时是否忽略大小写
            captchaJsonObj["CaptchaOptions"]["StoreageKeyPrefix"] = input.Configs.FirstOrDefault(f => f.Key == EnumConfigKey.loginCaptchaStoreageKeyPrefix.ToString()).Value;
            // 比较时是否忽略大小写
            captchaJsonObj["CaptchaOptions"]["IgnoreCase"] = input.Configs.FirstOrDefault(f => f.Key == EnumConfigKey.loginCaptchaIgnoreCase.ToString()).Value;
            // 是否启用动画
            captchaJsonObj["CaptchaOptions"]["ImageOption"]["Animation"] = input.Configs.FirstOrDefault(f => f.Key == EnumConfigKey.loginCaptchaAnimation.ToString()).Value;
            // 字体大小
            captchaJsonObj["CaptchaOptions"]["ImageOption"]["FontSize"] = input.Configs.FirstOrDefault(f => f.Key == EnumConfigKey.loginCaptchaFontSize.ToString()).Value;
            // 验证码宽度
            captchaJsonObj["CaptchaOptions"]["ImageOption"]["Width"] = input.Configs.FirstOrDefault(f => f.Key == EnumConfigKey.loginCaptchaWidth.ToString()).Value;
            // 验证码高度
            captchaJsonObj["CaptchaOptions"]["ImageOption"]["Height"] = input.Configs.FirstOrDefault(f => f.Key == EnumConfigKey.loginCaptchaHeight.ToString()).Value;
            // 气泡最小半径
            captchaJsonObj["CaptchaOptions"]["ImageOption"]["BubbleMinRadius"] = input.Configs.FirstOrDefault(f => f.Key == EnumConfigKey.loginCaptchaBubbleMinRadius.ToString()).Value;
            // 气泡最大半径
            captchaJsonObj["CaptchaOptions"]["ImageOption"]["BubbleMaxRadius"] = input.Configs.FirstOrDefault(f => f.Key == EnumConfigKey.loginCaptchaBubbleMaxRadius.ToString()).Value;
            // 气泡数量
            captchaJsonObj["CaptchaOptions"]["ImageOption"]["BubbleCount"] = input.Configs.FirstOrDefault(f => f.Key == EnumConfigKey.loginCaptchaBubbleCount.ToString()).Value;
            // 气泡边沿厚度
            captchaJsonObj["CaptchaOptions"]["ImageOption"]["BubbleThickness"] = input.Configs.FirstOrDefault(f => f.Key == EnumConfigKey.loginCaptchaBubbleThickness.ToString()).Value;
            // 干扰线数量
            captchaJsonObj["CaptchaOptions"]["ImageOption"]["InterferenceLineCount"] = input.Configs.FirstOrDefault(f => f.Key == EnumConfigKey.loginCaptchaInterferenceLineCount.ToString()).Value;
            // 包含actionj,epilog,fresnel,headache,lexo,prefix,progbot,ransom,robot,scandal,kaiti
            captchaJsonObj["CaptchaOptions"]["ImageOption"]["FontFamily"] = input.Configs.FirstOrDefault(f => f.Key == EnumConfigKey.loginCaptchaFontFamily.ToString()).Value;
            // 每帧延迟,Animation=true时有效, 默认30
            captchaJsonObj["CaptchaOptions"]["ImageOption"]["FrameDelay"] = input.Configs.FirstOrDefault(f => f.Key == EnumConfigKey.loginCaptchaFrameDelay.ToString()).Value;
            //  格式: rgb, rgba, rrggbb, or rrggbbaa format to match web syntax, 默认#fff
            captchaJsonObj["CaptchaOptions"]["ImageOption"]["BackgroundColor"] = input.Configs.FirstOrDefault(f => f.Key == EnumConfigKey.loginCaptchaBackgroundColor.ToString()).Value;
            //  颜色格式同BackgroundColor,多个颜色逗号分割，随机选取。不填，空值，则使用默认颜色集
            captchaJsonObj["CaptchaOptions"]["ImageOption"]["ForegroundColors"] = input.Configs.FirstOrDefault(f => f.Key == EnumConfigKey.loginCaptchaForegroundColors.ToString()).Value;
            // 图片质量（质量越高图片越大，gif调整无效可能会更大）
            captchaJsonObj["CaptchaOptions"]["ImageOption"]["Quality"] = input.Configs.FirstOrDefault(f => f.Key == EnumConfigKey.loginCaptchaQuality.ToString()).Value;
            // 粗体，该配置2.0.3新增
            captchaJsonObj["CaptchaOptions"]["ImageOption"]["TextBold"] = input.Configs.FirstOrDefault(f => f.Key == EnumConfigKey.loginCaptchaTextBold.ToString()).Value;
            captchaJson = JsonConvert.SerializeObject(captchaJsonObj, Formatting.Indented);
            File.WriteAllText(captchaJsonPath, captchaJson);
            #endregion

            #region 微信帐号
            var senparcJsonPath = "Settings/Senparc.json";
            var senparcJson = File.ReadAllText(senparcJsonPath);
            dynamic senparcJsonObj = JsonConvert.DeserializeObject(senparcJson);
            //验证码类型
            senparcJsonObj["SenparcWeixinSetting"]["Token"] = input.Configs.FirstOrDefault(f => f.Key == EnumConfigKey.wechatMpToken.ToString()).Value;
            senparcJsonObj["SenparcWeixinSetting"]["EncodingAESKey"] = input.Configs.FirstOrDefault(f => f.Key == EnumConfigKey.wechatMpEncodingAESKey.ToString()).Value;
            senparcJsonObj["SenparcWeixinSetting"]["WeixinAppId"] = input.Configs.FirstOrDefault(f => f.Key == EnumConfigKey.wechatMpAppId.ToString()).Value;
            senparcJsonObj["SenparcWeixinSetting"]["WeixinAppSecret"] = input.Configs.FirstOrDefault(f => f.Key == EnumConfigKey.wechatMpAppSecret.ToString()).Value;
            senparcJsonObj["SenparcWeixinSetting"]["WeixinCorpId"] = input.Configs.FirstOrDefault(f => f.Key == EnumConfigKey.wechatWorkCorpId.ToString()).Value;
            senparcJsonObj["SenparcWeixinSetting"]["WeixinCorpAgentId"] = input.Configs.FirstOrDefault(f => f.Key == EnumConfigKey.wechatWorkAgentId.ToString()).Value;
            senparcJsonObj["SenparcWeixinSetting"]["WeixinCorpSecret"] = input.Configs.FirstOrDefault(f => f.Key == EnumConfigKey.wechatWorkCorpSecret.ToString()).Value;
            senparcJsonObj["SenparcWeixinSetting"]["WeixinCorpToken"] = input.Configs.FirstOrDefault(f => f.Key == EnumConfigKey.wechatWorkCorpToken.ToString()).Value;
            senparcJsonObj["SenparcWeixinSetting"]["WeixinCorpEncodingAESKey"] = input.Configs.FirstOrDefault(f => f.Key == EnumConfigKey.wechatWorkCorpEncodingAESKey.ToString()).Value;
            senparcJsonObj["SenparcWeixinSetting"]["WxOpenAppId"] = input.Configs.FirstOrDefault(f => f.Key == EnumConfigKey.wechatOpenAppId.ToString()).Value;
            senparcJsonObj["SenparcWeixinSetting"]["WxOpenAppSecret"] = input.Configs.FirstOrDefault(f => f.Key == EnumConfigKey.wechatOpenAppSecret.ToString()).Value;
            senparcJsonObj["SenparcWeixinSetting"]["WxOpenToken"] = input.Configs.FirstOrDefault(f => f.Key == EnumConfigKey.wechatOpenToken.ToString()).Value;
            senparcJsonObj["SenparcWeixinSetting"]["WxOpenEncodingAESKey"] = input.Configs.FirstOrDefault(f => f.Key == EnumConfigKey.wechatOpenEncodingAESKey.ToString()).Value;

            senparcJson = JsonConvert.SerializeObject(senparcJsonObj, Formatting.Indented);
            File.WriteAllText(senparcJsonPath, senparcJson);
            #endregion

            operate.Code = ResultCode.Success;
            operate.Msg = Chs.Successful;
            return operate;
        }

    }
}
