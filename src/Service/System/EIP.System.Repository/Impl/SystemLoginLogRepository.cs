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
    /// ��¼��־
    /// </summary>
    public class SystemLoginLogRepository : ISystemLoginLogRepository
    {
        /// <summary>
        /// ��ȡ��¼��־
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        public Task<PagedResults<SystemLoginLog>> FindSystemLoginLog(SystemLoginLogFindPagingInput paging)
        {
            var sql = new StringBuilder("SELECT *,@rowNumber, @recordCount FROM System_LoginLog @where");
            if (paging.Sidx.IsNullOrEmpty())
            {
                paging.Sidx = " CreateTime";
            }
            return new SqlMapperUtil().PagingQuerySqlAsync<SystemLoginLog>(sql.ToString(), paging);
        }
    }
}