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

namespace EIP.System.Repository
{

    /// <summary>
    /// ������־
    /// </summary>
    public interface ISystemSmsLogRepository
    {
        /// <summary>
        /// ��ȡ������־
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        Task<PagedResults<SystemSmsLog>> FindPaging(SystemSmsLogFindPagingInput paging);
    }
}