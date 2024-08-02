namespace EIP.System.Models.Enums
{
    /// <summary>
    /// 配置项Key
    /// </summary>
    public enum EnumConfigKey
    {
        /// <summary>
        /// 请输入Title:头部显示
        /// </summary>
        loginTitle,
        /// <summary>
        /// 系统名:如EIP权限工作流平台
        /// </summary>
        loginSystemName,

        /// <summary>
        /// 登录方式
        /// </summary>
        loginType,
        /// <summary>
        /// 显示验证码
        /// </summary>
        loginCaptcha,
        /// <summary>
        /// 粒子特效
        /// </summary>
        loginParticles,
        /// <summary>
        /// 单点登录 帐号只能在一处登录,若在另外一处已登录,将会强制跳回登录界面
        /// </summary>
        loginSso,
        /// <summary>
        /// 登录错误次数锁定,选择后,超过登录次数将锁定
        /// </summary>
        loginErrorNumberLock,
        /// <summary>
        /// 超过登录次数将锁定
        /// </summary>
        loginErrorNumber,
        /// <summary>
        ///  超过登录次数将锁定,锁定多少分钟后才能重新登录，若是0则永久锁住，需要联系管理员删除锁定
        /// </summary>
        loginErrorNumberLockMinute,

        /// <summary>
        /// Token分钟数
        /// </summary>
        loginTokenExpires,

        /// <summary>
        /// 背景图
        /// </summary>
        loginBg,
        /// <summary>
        /// 提示信息
        /// </summary>
        loginTip,
        /// <summary>
        /// 备案号:如Copyright @2023 蜀ICP备18012364号-1
        /// </summary>
        loginCopyright,

        /// <summary>
        /// 验证码类型
        /// </summary>
        loginCaptchaType,
        loginCaptchaCodeLength,
        loginCaptchaExpirySeconds,
        loginCaptchaIgnoreCase,
        loginCaptchaStoreageKeyPrefix,
        loginCaptchaAnimation,
        loginCaptchaFontSize,
        loginCaptchaWidth,
        loginCaptchaHeight,
        loginCaptchaBubbleMinRadius,
        loginCaptchaBubbleMaxRadius,
        loginCaptchaBubbleCount,
        loginCaptchaBubbleThickness,
        loginCaptchaInterferenceLineCount,
        loginCaptchaFontFamily,
        loginCaptchaFrameDelay,
        loginCaptchaBackgroundColor,
        loginCaptchaForegroundColors,
        loginCaptchaQuality,
        loginCaptchaTextBold,

        /// <summary>
        /// 系统全称:如EIP权限工作流平台
        /// </summary>
        systemTitle,
        /// <summary>
        /// 系统域名
        /// </summary>
        systemDomain,
        /// <summary>
        /// 系统接口地址
        /// </summary>
        systemDomainApi,
        /// <summary>
        /// 代码生成地址
        /// </summary>
        systemCodeGenerationPath,
        /// <summary>
        /// 系统简称:折叠后显示名称
        /// </summary>
        systemShortName,
        /// <summary>
        /// 系统Logo
        /// </summary>
        systemLogo,
        /// <summary>
        /// 除开登录界面将姓名作为数字水印
        /// </summary>
        systemWarterMark,

        /// <summary>
        /// 超过最长未操作时间后系统将强制登录
        /// </summary>
        systemMaxDoCheckTime,

        /// <summary>
        /// 最少个字符串
        /// </summary>
        systemPasswordLength,
        /// <summary>
        /// 天强制修改密码
        /// </summary>
        systemPasswordCompelChangeDay,
        /// <summary>
        /// 数字
        /// </summary>
        systemPasswordHaveNumber,
        /// <summary>
        /// 数字个字符串
        /// </summary>
        systemPasswordHaveNumberLength,
        /// <summary>
        /// 字母
        /// </summary>
        systemPasswordHaveLetters,
        /// <summary>
        /// 字母个字符串
        /// </summary>
        systemPasswordHaveLettersLength,

        /// <summary>
        /// 特殊字符
        /// </summary>
        systemPasswordHaveSpecialCharacters,
        /// <summary>
        /// 特殊字符个字符串
        /// </summary>
        systemPasswordHaveSpecialCharactersLength,

        /// <summary>
        /// SMTP服务器
        /// </summary>
        emailSmtpServer,
        /// <summary>
        /// SMTP端口
        /// </summary>
        emailSmtpServerPort,
        /// <summary>
        /// SMTP-SSL
        /// </summary>
        emailSmtpSsl,
        /// <summary>
        /// 邮箱登录名
        /// </summary>
        emailLoginName,
        /// <summary>
        /// 邮箱密码
        /// </summary>
        emailLoginPassword,
        /// <summary>
        /// 
        /// </summary>
        wechatWorkCorpId,
        /// <summary>
        /// 
        /// </summary>
        wechatWorkAgentId,
        /// <summary>
        /// 
        /// </summary>
        wechatWorkCorpSecret,
        /// <summary>
        /// 
        /// </summary>
        wechatWorkCorpToken,
        /// <summary>
        /// 
        /// </summary>
        wechatWorkCorpEncodingAESKey,
        /// <summary>
        /// 
        /// </summary>
        wechatMpAppId,
        /// <summary>
        /// 
        /// </summary>
        wechatMpAppSecret,
        /// <summary>
        /// 
        /// </summary>
        wechatMpToken,
        /// <summary>
        /// 
        /// </summary>
        wechatMpEncodingAESKey,
        /// <summary>
        /// 
        /// </summary>
        wechatOpenAppId,
        /// <summary>
        /// 
        /// </summary>
        wechatOpenAppSecret,
        /// <summary>
        /// 
        /// </summary>
        wechatOpenToken,
        /// <summary>
        /// 
        /// </summary>
        wechatOpenEncodingAESKey,
        /// <summary>
        /// 钉钉企业ID
        /// </summary>
        dingTalkCorpId,
        /// <summary>
        /// 钉钉ApiToken
        /// </summary>
        dingTalkApiToken,
        /// <summary>
        /// 钉钉AgentId
        /// </summary>
        dingTalkAgentId,
        /// <summary>
        /// 钉钉AppKey
        /// </summary>
        dingTalkAppKey,
        /// <summary>
        /// 钉钉AppSecret
        /// </summary>
        dingTalkAppSecret,
        /// <summary>
        /// 钉钉EncodingAesKey
        /// </summary>
        dingTalkEncodingAesKey,
        /// <summary>
        /// 钉钉Token
        /// </summary>
        dingTalkToken,

        /// <summary>
        /// 存储方式
        /// </summary>
        filesStorageType,

        /// <summary>
        /// 存储路径
        /// </summary>
        filesStoragePath,

        /// <summary>
        /// 文件后缀类型
        /// </summary>
        filesStorageFileSuffix,

        /// <summary>
        /// 文件最大大小
        /// </summary>
        filesStorageFileMaxSize,

        /// <summary>
        /// 绑定域名
        /// </summary>
        filesStorageBucketBindUrl,

        /// <summary>
        /// 云存储授权账户
        /// </summary>
        filesStorageAccessKeyId,

        /// <summary>
        /// 云存储授权密钥
        /// </summary>
        filesStorageAccessKeySecret,

        /// <summary>
        /// 阿里云节点
        /// </summary>
        filesStorageAliYunEndpoint,

        /// <summary>
        /// 阿里云桶名称
        /// </summary>
        filesStorageAliYunBucketName,

        /// <summary>
        /// 腾讯云账户标识
        /// </summary>
        filesStorageTencentAccountId,

        /// <summary>
        /// 腾讯云桶地域
        /// </summary>
        filesStorageTencentCosRegion,

        /// <summary>
        /// 腾讯云桶名称
        /// </summary>
        filesStorageTencentBucketName,

        /// <summary>
        /// 钉钉Token
        /// </summary>
        filesStorageQiNiuBucketName,

        /// <summary>
        /// 物流快递AppId
        /// </summary>
        expressAppId,

        /// <summary>
        /// 物流快递Secret
        /// </summary>
        expressSecret,
    }
}
