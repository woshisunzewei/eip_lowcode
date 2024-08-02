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
using EIP.System.Models.Dtos.Menu;
using EIP.System.Models.Dtos.MenuButton;

namespace EIP.System.Repository
{
    /// <summary>
    /// Ȩ��
    /// </summary>
    public interface ISystemPermissionRepository
    {
        /// <summary>
        /// ����Ȩ�޹���Id��ѯģ��Ȩ����Ϣ
        /// </summary>
        /// <param name="input">Ȩ������:ģ�顢��������ݡ��ֶΡ��ļ�</param>
        /// <returns></returns>
        Task<IEnumerable<SystemPermission>> FindPermissionByPrivilegeMasterValue(SystemPermissionByPrivilegeMasterValueInput input);

        /// <summary>
        /// �����û�Id��ȡ�û����е�ģ��Ȩ��
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IEnumerable<SystemPermissionFindMenuByUserIdOutput>> FindSystemPermissionMenuByUserId(SystemPermissionMenuInput input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IEnumerable<SystemMenuFindAgileOutput>> FindSystemPermissionMenuAgileByUserId(SystemPermissionMenuInput input);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="menuId"></param>
        /// <returns></returns>
        Task<IEnumerable<SystemMenuButtonByViewRote>> FindMenuButton(Guid userId, string menuId);

        /// <summary>
        /// �����û�Id��ȡ�û����е�ģ��Ȩ��
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<SystemPermissionFindMenuByUserIdOutput>> FindSystemPermissionMenuByAdmin(SystemPermissionMenuInput input);

        /// <summary>
        /// �����û�Id��ȡ�û����е�ģ��Ȩ��
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<BaseTree>> FindSystemPermissionMenuAllByUserId(Guid userId);
        /// <summary>
        /// �����û�Id��ȡ�û����е�ģ��Ȩ��
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<BaseTree>> FindSystemPermissionMobileMenuAllByUserId(Guid userId);

        /// <summary>
        /// ���ݽ�ɫId,��λId,��Id,��ԱId��ȡ���е�ģ����Ϣ
        /// </summary>
        /// <param name="input">�������</param>
        /// <returns>����ģ����Ϣ</returns>
        Task<IEnumerable<BaseTree>> FindMenuHavePermissionByPrivilegeMasterValue(SystemPermissiontMenuHaveByPrivilegeMasterValueInput input);
        /// <summary>
        /// ���ݽ�ɫId,��λId,��Id,��ԱId��ȡ���е�ģ����Ϣ
        /// </summary>
        /// <param name="input">�������</param>
        /// <returns>����ģ����Ϣ</returns>
        Task<IEnumerable<BaseTree>> FindMobileMenuHavePermissionByPrivilegeMasterValue(SystemPermissiontMenuHaveByPrivilegeMasterValueInput input);

        /// <summary>
        /// ��ȡ����Ȩ��Sql
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IEnumerable<SystemData>> FindDataPermission(ViewRote input);
    }
}