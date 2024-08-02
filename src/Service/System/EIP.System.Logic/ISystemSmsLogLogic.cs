/**************************************************************
* Copyright (C) www.eipflow.com ����ΰ��Ȩ����(����ؾ�)
*
* ����: ����ΰ(QQ 1039318332)
* ����ʱ��: 2018/10/30 22:40:15
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

namespace EIP.System.Logic.Log.ILogic
{
    /// <summary>
    /// ������־
    /// </summary>
    public interface ISystemSmsLogLogic : IAsyncLogic<SystemSmsLog>
    {
        /// <summary>
        /// Excel������ʽ
        /// </summary>
        /// <param name="paging">��ѯ����</param>
        /// <param name="excelReportDto"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemSmsLogLogic_Cache")]
        Task<OperateStatus> Report(QueryParam paging, ExcelReportDto excelReportDto);

        /// <summary>
        /// ��ȡ
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemSmsLogLogic_Cache")]
        Task<OperateStatus<PagedResults<SystemSmsLog>>> FindPaging(SystemSmsLogFindPagingInput paging);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemSmsLogLogic_Cache")]
        Task<OperateStatus<SystemSmsLog>> FindById(IdInput input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemSmsLogLogic_Cache")]
        Task<OperateStatus> Delete(IdInput<string> input);
    }
}