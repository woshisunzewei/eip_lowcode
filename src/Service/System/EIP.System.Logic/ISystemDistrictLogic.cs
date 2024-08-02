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
using EIP.System.Models.Dtos.District;

namespace EIP.System.Logic
{
    /// <summary>
    /// ʡ������������
    /// </summary>
    public interface ISystemDistrictLogic : IAsyncLogic<SystemDistrict>
    {
        /// <summary>
        /// ���ݸ�����ѯ�����Ӽ����νṹ
        /// </summary>
        /// <param name="input">����Id</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemDistrictLogic_Cache")]
        Task<OperateStatus<IEnumerable<TreeEntity>>> FindSync(IdInput<string> input);

        /// <summary>
        /// ���ݸ�����ѯ�����Ӽ�
        /// </summary>
        /// <param name="input">����Id</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemDistrictLogic_Cache")]
        Task<OperateStatus<IEnumerable<SystemDistrict>>> FindByParentId(IdInput<string> input);

        /// <summary>
        /// ������Id��ȡʡ����Id
        /// </summary>
        /// <param name="input">��Id</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemDistrictLogic_Cache")]
        Task<OperateStatus<SystemDistrictFindDistrictByCountIdOutout>> FindByCountId(IdInput<string> input);

        /// <summary>
        /// �������Ƿ��Ѿ������ظ���
        /// </summary>
        /// <param name="input">��Ҫ��֤�Ĳ���</param>
        /// <returns></returns>
        [EasyCachingAble(CacheKeyPrefix = "ISystemDistrictLogic_Cache")]
        Task<OperateStatus> Check(CheckSameValueInput input);

        /// <summary>
        /// ����ʡ������Ϣ
        /// </summary>
        /// <param name="systemDistrict">ʡ������Ϣ</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemDistrictLogic_Cache")]
        Task<OperateStatus> Save(SystemDistrict systemDistrict);

        /// <summary>
        /// ɾ��ʡ���ؼ��¼�����
        /// </summary>
        /// <param name="input">����id</param>
        /// <returns></returns>
        [EasyCachingEvict(IsAll = true, CacheKeyPrefix = "ISystemDistrictLogic_Cache")]
        Task<OperateStatus> Delete(IdInput<string> input);
    }
}