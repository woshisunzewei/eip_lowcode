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

namespace EIP.System.Repository.Log.IRepository
{
    /// <summary>
    /// ��ҵ��־
    /// </summary>
    public interface ISystemJobLogRepository
    {
        /// <summary>
        /// ��ȡ��ҵ��־
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        Task<PagedResults<SystemJobLogFindPagingOutput>> FindSystemJobLog(SystemJobLogFindPagingInput paging);
    }
}