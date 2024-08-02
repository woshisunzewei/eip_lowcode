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
    /// �쳣��־
    /// </summary>
    public class SystemExceptionLogRepository : ISystemExceptionLogRepository
    {
        /// <summary>
        /// ��ȡ�쳣��־
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        public Task<PagedResults<SystemExceptionLog>> FindSystemExceptionLog(SystemExceptionLogFindPagingInput paging)
        {
            var sql = new StringBuilder("SELECT ExceptionLogId,Message,InnerException,RemoteIp,RemoteIpAddress,UserAgent,OsInfo,Browser,RequestUrl,HttpMethod,CreateUserId,CreateUserCode,CreateUserName,CreateTime,@rowNumber, @recordCount  FROM System_ExceptionLog @where");
            if (paging.Sidx.IsNullOrEmpty())
            {
                paging.Sidx = " CreateTime";
            }
            return new SqlMapperUtil().PagingQuerySqlAsync<SystemExceptionLog>(sql.ToString(), paging);
        }
    }
}