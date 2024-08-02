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

namespace EIP.System.Repository.Impl
{
    /// <summary>
    /// ������־
    /// </summary>
    public class SystemRateLimitLogRepository :  ISystemRateLimitLogRepository
    {
        /// <summary>
        /// ��ȡ������־
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        public Task<PagedResults<SystemRateLimitLog>> FindPagingRateLimitLog(SystemRateLimitLogFindPagingInput paging)
        {
            var sql = new StringBuilder("SELECT RateLimitLogId,Message,RemoteIp,CreateUserCode,ControllerName,ActionName,RequestContentLength,RequestType,CreateUserName,Url,CreateTime,@rowNumber, @recordCount  FROM System_RateLimitLog @where");
            if (paging.Sidx.IsNullOrEmpty())
            {
                paging.Sidx = " CreateTime";
            }
            return new SqlMapperUtil().PagingQuerySqlAsync<SystemRateLimitLog>(sql.ToString(), paging);
        }
    }
}