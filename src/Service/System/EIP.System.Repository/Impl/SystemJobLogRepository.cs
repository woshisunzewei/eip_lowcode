/**************************************************************
* Copyright (C) 2018 www.sf-info.cn ʢ���Ȩ����(����ؾ�)
*
* ����: ����ΰ(QQ 3365690)
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
using EIP.System.Repository.Log.IRepository;

namespace EIP.System.Repository.Log.Repository
{
    /// <summary>
    /// ��ҵ��־
    /// </summary>
    public class SystemJobLogRepository : ISystemJobLogRepository
    {
        /// <summary>
        /// ��ȡ��ҵ��־
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        public Task<PagedResults<SystemJobLogFindPagingOutput>> FindSystemJobLog(SystemJobLogFindPagingInput paging)
        {
            var sql = new StringBuilder("SELECT Message,CreateTime,@rowNumber, @recordCount  FROM System_JobLog @where");
            if (paging.Correlation != null)
                sql.Append($" AND Correlation='{paging.Correlation}'");
            if (paging.Sidx.IsNullOrEmpty())
            {
                paging.Sidx = " CreateTime";
            }
            return new SqlMapperUtil().PagingQuerySqlAsync<SystemJobLogFindPagingOutput>(sql.ToString(), paging);
        }
    }
}