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
namespace EIP.System.Controller
{
    /// <summary>
    /// 系统配置
    /// </summary>
    public class ConfigController : BaseSystemController
    {
        #region 构造函数

        private readonly ISystemConfigLogic _configLogic;
      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configLogic"></param>
        public ConfigController(ISystemConfigLogic configLogic)
        {
            _configLogic = configLogic;
        }

        #endregion

        #region 方法

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        [CreateBy("孙泽伟")]
        [Remark("系统参数配置-方法-新增/编辑-保存", RemarkFrom.System)]
        [Route("/system/config")]
        public async  Task<ActionResult> Save(SystemConfigSaveInput input)
        {
            return Ok(await _configLogic.Save(input));
        }

        /// <summary>
        /// 获取配置
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("系统参数配置-方法-新增/编辑-获取配置", RemarkFrom.System)]
        [Route("/system/config")]
        public async  Task<ActionResult> FindAll()
        {
            return Ok(await _configLogic.FindAll());
        }

        /// <summary>
        /// 获取登录界面配置
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("系统参数配置-方法-获取登录界面配置", RemarkFrom.System)]
        [Route("/system/config/login")]
        public async  Task<ActionResult> Login()
        {
            var configs = (await _configLogic.FindAll()).Data;
            return Ok(new
            {
                code = ResultCode.Success,
                msg = Chs.Successful,
                data = new
                {
                    loginTitle = configs.FirstOrDefault(f => f.Key == EnumConfigKey.loginTitle.ToString())?.Value,
                    loginSystemName = configs.FirstOrDefault(f => f.Key == EnumConfigKey.loginSystemName.ToString())?.Value,
                    loginType = configs.FirstOrDefault(f => f.Key == EnumConfigKey.loginType.ToString())?.Value,
                    loginCaptcha = configs.FirstOrDefault(f => f.Key == EnumConfigKey.loginCaptcha.ToString())?.Value,
                    loginParticles = configs.FirstOrDefault(f => f.Key == EnumConfigKey.loginParticles.ToString())?.Value,
                    loginSso = configs.FirstOrDefault(f => f.Key == EnumConfigKey.loginSso.ToString())?.Value,
                    loginBg = configs.FirstOrDefault(f => f.Key == EnumConfigKey.loginBg.ToString())?.Value,
                    loginTip = configs.FirstOrDefault(f => f.Key == EnumConfigKey.loginTip.ToString())?.Value,
                    loginCopyright = configs.FirstOrDefault(f => f.Key == EnumConfigKey.loginCopyright.ToString())?.Value,
                    systemWarterMark = configs.FirstOrDefault(f => f.Key == EnumConfigKey.systemWarterMark.ToString())?.Value,
                    dingTalkCorpId = configs.FirstOrDefault(f => f.Key == EnumConfigKey.dingTalkCorpId.ToString())?.Value,
                }
            });
        }

        /// <summary>
        /// 获取首页界面配置
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("系统参数配置-方法-获取首页界面配置", RemarkFrom.System)]
        [Route("/system/config/system")]
        public async  Task<ActionResult> System()
        {
            var configs = (await _configLogic.FindAll()).Data;
            return Ok(new
            {
                code = ResultCode.Success,
                msg = Chs.Successful,
                data = new
                {
                    systemTitle = configs.FirstOrDefault(f => f.Key == EnumConfigKey.systemTitle.ToString())?.Value,
                    systemShortName = configs.FirstOrDefault(f => f.Key == EnumConfigKey.systemShortName.ToString())?.Value,
                    systemLogo = configs.FirstOrDefault(f => f.Key == EnumConfigKey.systemLogo.ToString())?.Value,
                    systemMaxDoCheckTime= configs.FirstOrDefault(f => f.Key == EnumConfigKey.systemMaxDoCheckTime.ToString())?.Value,
                    systemPasswordLength = configs.FirstOrDefault(f => f.Key == EnumConfigKey.systemPasswordLength.ToString())?.Value,
                    systemPasswordHaveNumber = configs.FirstOrDefault(f => f.Key == EnumConfigKey.systemPasswordHaveNumber.ToString())?.Value,
                    systemPasswordHaveNumberLength = configs.FirstOrDefault(f => f.Key == EnumConfigKey.systemPasswordHaveNumberLength.ToString())?.Value,
                    systemPasswordHaveLetters = configs.FirstOrDefault(f => f.Key == EnumConfigKey.systemPasswordHaveLetters.ToString())?.Value,
                    systemPasswordHaveLettersLength = configs.FirstOrDefault(f => f.Key == EnumConfigKey.systemPasswordHaveLettersLength.ToString())?.Value,
                    systemPasswordHaveSpecialCharacters = configs.FirstOrDefault(f => f.Key == EnumConfigKey.systemPasswordHaveSpecialCharacters.ToString())?.Value,
                    systemPasswordHaveSpecialCharactersLength = configs.FirstOrDefault(f => f.Key == EnumConfigKey.systemPasswordHaveSpecialCharactersLength.ToString())?.Value,
                }
            });
        }
        #endregion
    }
}