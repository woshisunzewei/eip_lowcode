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

namespace EIP.System.Repository
{
    /// <summary>
    /// ��¼��־
    /// </summary>
    public interface ISystemLoginLogRepository 
    { 
        /// <summary>
        /// ��ȡ��¼��־
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        Task<PagedResults<SystemLoginLog>> FindSystemLoginLog(SystemLoginLogFindPagingInput paging);
    }
}