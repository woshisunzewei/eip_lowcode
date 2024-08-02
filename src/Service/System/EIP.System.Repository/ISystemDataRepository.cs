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
using EIP.System.Models.Dtos.Data;

namespace EIP.System.Repository
{
    /// <summary>
    /// ������־
    /// </summary>
    public interface ISystemDataRepository 
    {
        /// <summary>
        /// ����ģ��Id��ȡ����Ȩ�޹���
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResults<SystemDataFindOutput>> Find(SystemDataFindInput input);

        /// <summary>
        /// ����ģ���ȡ��������Ϣ
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SystemDataOutput>> FindHaveDataPermission(IdInput input);

        /// <summary>
        /// ����ģ��Id��ȡ����Ȩ�޹���
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SystemDataOutput>> FindDataByMenuId(IList<Guid> menuId = null);
    }
}