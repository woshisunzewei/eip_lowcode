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
    public class MobileController : BaseSystemController
    {
        #region 构造函数

        private readonly ISystemConfigLogic _configLogic;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configLogic"></param>
        public MobileController( ISystemConfigLogic configLogic)
        {
            _configLogic = configLogic;
        }

        #endregion

        #region 方法
        /// <summary>
        /// 获取登录界面配置
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [CreateBy("孙泽伟")]
        [Remark("系统参数配置-方法-获取登录界面配置", RemarkFrom.System)]
        [Route("/mobile/config/login")]
        public async  Task<ActionResult> Login()
        {
            var configs = GlobalParams.GetValuesByName();
            if (configs == null || configs.Count() == 0)
            {
                configs = new List<GlobalConfig>();
                var result = await _configLogic.FindAll();
                foreach (var item in result.Data)
                {
                    configs.Add(new GlobalConfig
                    {
                        Key = item.Key,
                        Value = item.Value
                    });
                }
            }

            return Ok(new
            {
                code = ResultCode.Success,
                msg = Chs.Successful,
                data = new
                {
                    loginTitle = configs.FirstOrDefault(f => f.Key == "loginTitle")?.Value,
                    loginSystemName = configs.FirstOrDefault(f => f.Key == "loginSystemName")?.Value,
                    loginType = configs.FirstOrDefault(f => f.Key == "loginType")?.Value,
                    loginCaptcha = configs.FirstOrDefault(f => f.Key == "loginCaptcha")?.Value,
                    loginParticles = configs.FirstOrDefault(f => f.Key == "loginParticles")?.Value,
                    loginSso = configs.FirstOrDefault(f => f.Key == "loginSso")?.Value,
                    loginBg = configs.FirstOrDefault(f => f.Key == "loginBg")?.Value,
                    loginTip = configs.FirstOrDefault(f => f.Key == "loginTip")?.Value,
                    loginCopyright = configs.FirstOrDefault(f => f.Key == "loginCopyright")?.Value,
                    systemWarterMark = configs.FirstOrDefault(f => f.Key == "systemWarterMark")?.Value,
                    systemLogo = configs.FirstOrDefault(f => f.Key == "systemLogo")?.Value,
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
        [Route("/mobile/config/system")]
        public async  Task<ActionResult> System()
        {
            var configs = GlobalParams.GetValuesByName();
            if (configs == null || configs.Count() == 0)
            {
                configs = new List<GlobalConfig>();
                var result = await _configLogic.FindAll();
                foreach (var item in result.Data)
                {
                    configs.Add(new GlobalConfig
                    {
                        Key = item.Key,
                        Value = item.Value
                    });
                }
            }
            return Ok(new
            {
                code = ResultCode.Success,
                msg = Chs.Successful,
                data = new
                {
                    systemTitle = configs.FirstOrDefault(f => f.Key == "systemTitle")?.Value,
                    systemShortName = configs.FirstOrDefault(f => f.Key == "systemShortName")?.Value,
                    systemLogo = configs.FirstOrDefault(f => f.Key == "systemLogo")?.Value,
                }
            });
        }
        #endregion
    }
}