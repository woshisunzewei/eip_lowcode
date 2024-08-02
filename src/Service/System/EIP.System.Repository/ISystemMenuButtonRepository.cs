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
using EIP.System.Models.Dtos.MenuButton;

namespace EIP.System.Repository
{
    /// <summary>
    /// ģ�鰴ť
    /// </summary>
    public interface ISystemMenuButtonRepository
    {
        /// <summary>
        /// ����ģ���ȡ��������Ϣ
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResults<SystemMenuButtonFindOutput>> Find(SystemMenuButtonFindInput input);

        /// <summary>
        /// ����ģ���ȡ��������Ϣ
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SystemMenuButtonOutput>> FindHaveMenuButtonPermission(IdInput input);

        /// <summary>
        /// ����ģ���ȡ��������Ϣ
        /// </summary>
        /// <param name="menuId"></param>
        /// <returns></returns>
        Task<IEnumerable<SystemMenuButtonOutput>> FindMenuButtonByMenuId(IList<Guid> menuId = null);

        /// <summary>
        /// ����ģ��Id���û�Id��ȡ��ťȨ������
        /// </summary>
        /// <param name="viewRote"></param>
        /// <returns></returns>
        Task<IEnumerable<SystemMenuButtonByViewRote>> FindMenuButton(ViewRote viewRote);

    }
}