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
namespace EIP.System.Logic
{
    /// <summary>
    /// 用户业务逻辑
    /// </summary>
    public interface ISystemUserInfoLogic : IAsyncLogic<SystemUserInfo>
    {
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="input">分页参数</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemUserInfoLogic_Cache")]
        Task<OperateStatus<PagedResults<SystemUserOutput>>> Find(SystemUserPagingInput input);

        /// <summary>
        /// 用户公共信息
        /// </summary>
        /// <param name="input">分页参数</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemUserInfoLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemUserFindCommonOutput>>> FindCommon(SystemUserFindCommonInput input);

        /// <summary>
        /// 检测配置项代码是否已经具有重复项
        /// </summary>
        /// <param name="input">需要验证的参数</param>
        /// <returns></returns>
        Task<OperateStatus> CheckCode(SystemUserCheckUserCodeInput input);

        /// <summary>
        /// 保存人员信息
        /// </summary>
        /// <param name="input">人员信息</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemUserInfoLogic_Cache")]
        Task<OperateStatus> Save(SystemUserSaveInput input);

        /// <summary>
        /// 删除用户信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemUserInfoLogic_Cache")]
        Task<OperateStatus> Delete(IdInput<string> input);

        /// <summary>
        /// 根据用户Id重置某人密码
        /// </summary>
        /// <param name="input">用户Id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemUserInfoLogic_Cache")]
        Task<OperateStatus> ResetPassword(SystemUserResetPasswordInput input);

        /// <summary>
        /// 保存用户头像
        /// </summary>
        /// <param name="input">用户头像</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemUserInfoLogic_Cache")]
        Task<OperateStatus<string>> SaveHeadImage(SystemUserSaveHeadImageInput input);

        /// <summary>
        /// 保存修改后密码信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemUserInfoLogic_Cache")]
        Task< OperateStatus> SaveChangePassword(SystemUserChangePasswordInput input);

        /// <summary>
        /// 验证旧密码是否输入正确
        /// </summary>
        /// <param name="input">需要验证的参数</param>
        /// <returns></returns>
        Task<OperateStatus> CheckOldPassword(CheckSameValueInput input);

        /// <summary>
        /// 根据用户Id获取该用户信息
        /// </summary>
        /// <param name="input">用户Id</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemUserInfoLogic_Cache")]
        Task<OperateStatus<SystemUserDetailOutput>> FindDetailByUserId(IdInput input);

        /// <summary>
        /// 根据用户Id获取该用户信息
        /// </summary>
        /// <param name="input">用户Id</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemUserInfoLogic_Cache")]
        Task<OperateStatus<SystemUserFindHeadByIdOutput>> FindById(IdInput input);

        /// <summary>
        /// 冻结
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemUserInfoLogic_Cache")]
        Task<OperateStatus> IsFreeze(IdInput input);

        #region 登录

        /// <summary>
        /// 验证码
        /// </summary>
        /// <returns></returns>
        OperateStatus<MemoryStream> Captcha(IdInput input);

        /// <summary>
        /// 根据登录代码和密码查询用户信息
        /// </summary>
        /// <param name="input">用户名、密码等</param>
        /// <returns></returns>
        Task<OperateStatus<SystemLoginOutput>> Login(SystemLoginInput input);

        /// <summary>
        /// 根据用户Id登录
        /// </summary>
        /// <param name="input">加密用户Id等</param>
        /// <returns></returns>
        Task<OperateStatus<SystemLoginOutput>> LoginByUserId(SystemLoginByUserIdInput input);

        /// <summary>
        /// 根据用户帐号登录
        /// </summary>
        /// <param name="input">加密用户帐号登录等</param>
        /// <returns></returns>
        Task<OperateStatus<SystemLoginOutput>> LoginByCode(SystemLoginByCodeInput input);

        /// <summary>
        /// 根据钉钉临时授权码登录
        /// </summary>
        /// <param name="input">用户名、密码等</param>
        /// <returns></returns>
        Task<OperateStatus<SystemLoginOutput>> LoginDingTalk(SystemLoginByDingTalkInput input);

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemLoginLogLogic_Cache")]
        OperateStatus LoginOut(IdInput input);

        /// <summary>
        /// Oauth授权回调方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<OperateStatus<SystemLoginOutput>> OauthCallBack(SystemUserOauthCallBackInput input, string userId);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<OperateStatus> CheckPasswordRule(SystemUserCheckPasswordRuleInput input);
        #endregion

        #region 注册

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemUserInfoLogic_Cache")]
        Task<OperateStatus> Register(SystemUserRegisterInput input);
        #endregion

        #region 导入导出
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paging"></param>
        /// <param name="excelReportDto"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemUserInfoLogic_Cache")]
        Task<OperateStatus> ReportExcelUserQuery(SystemUserPagingInput paging, ExcelReportDto excelReportDto);

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemUserInfoLogic_Cache")]
        Task<OperateStatus<List<string>>> Import(IList<SystemUserImportInput> input);

        /// <summary>
        /// 导入
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemUserInfoLogic_Cache")]
        Task<OperateStatus<List<string>>> ImportUserMobile(IList<SystemUserImportInput> input);
        #endregion
    }
}