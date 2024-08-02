/**************************************************************
* Copyright (C) 2022 www.eipflow.com ����ΰ��Ȩ����(����ؾ�)
*
* ����: ����ΰ(QQ 1039318332)
* ����ʱ��: 2022/01/12 22:40:15
* �ļ���: 
* ����: 
* 
* �޸���ʷ
* �޸��ˣ�
* ʱ�䣺
* �޸�˵����
*
**************************************************************/
namespace EIP.System.Logic
{
    /// <summary>
    /// �û�ҵ���߼�
    /// </summary>
    public interface ISystemUserInfoLogic : IAsyncLogic<SystemUserInfo>
    {
        /// <summary>
        /// ��ҳ��ѯ
        /// </summary>
        /// <param name="input">��ҳ����</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemUserInfoLogic_Cache")]
        Task<OperateStatus<PagedResults<SystemUserOutput>>> Find(SystemUserPagingInput input);

        /// <summary>
        /// �û�������Ϣ
        /// </summary>
        /// <param name="input">��ҳ����</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemUserInfoLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemUserFindCommonOutput>>> FindCommon(SystemUserFindCommonInput input);

        /// <summary>
        /// �������������Ƿ��Ѿ������ظ���
        /// </summary>
        /// <param name="input">��Ҫ��֤�Ĳ���</param>
        /// <returns></returns>
        Task<OperateStatus> CheckCode(SystemUserCheckUserCodeInput input);

        /// <summary>
        /// ������Ա��Ϣ
        /// </summary>
        /// <param name="input">��Ա��Ϣ</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemUserInfoLogic_Cache")]
        Task<OperateStatus> Save(SystemUserSaveInput input);

        /// <summary>
        /// ɾ���û���Ϣ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemUserInfoLogic_Cache")]
        Task<OperateStatus> Delete(IdInput<string> input);

        /// <summary>
        /// �����û�Id����ĳ������
        /// </summary>
        /// <param name="input">�û�Id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemUserInfoLogic_Cache")]
        Task<OperateStatus> ResetPassword(SystemUserResetPasswordInput input);

        /// <summary>
        /// �����û�ͷ��
        /// </summary>
        /// <param name="input">�û�ͷ��</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemUserInfoLogic_Cache")]
        Task<OperateStatus<string>> SaveHeadImage(SystemUserSaveHeadImageInput input);

        /// <summary>
        /// �����޸ĺ�������Ϣ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemUserInfoLogic_Cache")]
        Task< OperateStatus> SaveChangePassword(SystemUserChangePasswordInput input);

        /// <summary>
        /// ��֤�������Ƿ�������ȷ
        /// </summary>
        /// <param name="input">��Ҫ��֤�Ĳ���</param>
        /// <returns></returns>
        Task<OperateStatus> CheckOldPassword(CheckSameValueInput input);

        /// <summary>
        /// �����û�Id��ȡ���û���Ϣ
        /// </summary>
        /// <param name="input">�û�Id</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemUserInfoLogic_Cache")]
        Task<OperateStatus<SystemUserDetailOutput>> FindDetailByUserId(IdInput input);

        /// <summary>
        /// �����û�Id��ȡ���û���Ϣ
        /// </summary>
        /// <param name="input">�û�Id</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemUserInfoLogic_Cache")]
        Task<OperateStatus<SystemUserFindHeadByIdOutput>> FindById(IdInput input);

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemUserInfoLogic_Cache")]
        Task<OperateStatus> IsFreeze(IdInput input);

        #region ��¼

        /// <summary>
        /// ��֤��
        /// </summary>
        /// <returns></returns>
        OperateStatus<MemoryStream> Captcha(IdInput input);

        /// <summary>
        /// ���ݵ�¼����������ѯ�û���Ϣ
        /// </summary>
        /// <param name="input">�û����������</param>
        /// <returns></returns>
        Task<OperateStatus<SystemLoginOutput>> Login(SystemLoginInput input);

        /// <summary>
        /// �����û�Id��¼
        /// </summary>
        /// <param name="input">�����û�Id��</param>
        /// <returns></returns>
        Task<OperateStatus<SystemLoginOutput>> LoginByUserId(SystemLoginByUserIdInput input);

        /// <summary>
        /// �����û��ʺŵ�¼
        /// </summary>
        /// <param name="input">�����û��ʺŵ�¼��</param>
        /// <returns></returns>
        Task<OperateStatus<SystemLoginOutput>> LoginByCode(SystemLoginByCodeInput input);

        /// <summary>
        /// ���ݶ�����ʱ��Ȩ���¼
        /// </summary>
        /// <param name="input">�û����������</param>
        /// <returns></returns>
        Task<OperateStatus<SystemLoginOutput>> LoginDingTalk(SystemLoginByDingTalkInput input);

        /// <summary>
        /// �˳���¼
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemLoginLogLogic_Cache")]
        OperateStatus LoginOut(IdInput input);

        /// <summary>
        /// Oauth��Ȩ�ص�����
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

        #region ע��

        /// <summary>
        /// ע��
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemUserInfoLogic_Cache")]
        Task<OperateStatus> Register(SystemUserRegisterInput input);
        #endregion

        #region ���뵼��
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paging"></param>
        /// <param name="excelReportDto"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemUserInfoLogic_Cache")]
        Task<OperateStatus> ReportExcelUserQuery(SystemUserPagingInput paging, ExcelReportDto excelReportDto);

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemUserInfoLogic_Cache")]
        Task<OperateStatus<List<string>>> Import(IList<SystemUserImportInput> input);

        /// <summary>
        /// ����
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemUserInfoLogic_Cache")]
        Task<OperateStatus<List<string>>> ImportUserMobile(IList<SystemUserImportInput> input);
        #endregion
    }
}