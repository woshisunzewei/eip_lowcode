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
using EIP.System.Models.Dtos.Log;

namespace EIP.System.Logic
{
    /// <summary>
    /// ��¼��־
    /// </summary>
    public interface ISystemLoginLogLogic : IAsyncLogic<SystemLoginLog>
    {
        /// <summary>
        /// Excel������ʽ
        /// </summary>
        /// <param name="paging">��ѯ����</param>
        /// <param name="excelReportDto"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemLoginLogLogic_Cache")]
        Task<OperateStatus> Report(QueryParam paging, ExcelReportDto excelReportDto);

        /// <summary>
        /// ��ȡ��¼��־
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemLoginLogLogic_Cache")]
        Task<OperateStatus<PagedResults<SystemLoginLog>>> Find(SystemLoginLogFindPagingInput paging);

        /// <summary>
        /// ��ȡ��¼��־����
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemLoginLogLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemLoginLog>>> FindAnalysis(SystemLoginLogFindPagingInput paging);

        /// <summary>
        /// �˳���¼
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemLoginLogLogic_Cache")]
        Task<OperateStatus> LoginOut(IdInput input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemLoginLogLogic_Cache")]
        Task<SystemLoginLog> FindById(IdInput input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemLoginLogLogic_Cache")]
        Task<OperateStatus> Delete(IdInput<string> input);
    }
}