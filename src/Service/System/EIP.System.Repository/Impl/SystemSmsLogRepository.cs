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

namespace EIP.System.Repository.Impl
{
    /// <summary>
    /// ������־
    /// </summary>
    public class SystemSmsLogRepository :  ISystemSmsLogRepository
    {
        /// <summary>
        /// ��ȡ������־
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        public Task<PagedResults<SystemSmsLog>> FindPaging(SystemSmsLogFindPagingInput paging)
        {
            var sql = new StringBuilder("SELECT SmsLogId,Phone,Message,Provide,Code,CreateTime,IsSend,@rowNumber, @recordCount  FROM System_SmsLog @where");
            if (paging.Sidx.IsNullOrEmpty())
            {
                paging.Sidx = " CreateTime";
            }
            return new SqlMapperUtil().PagingQuerySqlAsync<SystemSmsLog>(sql.ToString(), paging);
        }
    }
}