using EIP.System.Models.Dtos.Dictionary;

namespace EIP.System.Repository
{
    /// <summary>
    /// �ֵ����ݷ��ʽӿ�
    /// </summary>
    public interface ISystemDictionaryRepository
    {
        /// <summary>
        /// ���ݸ�����ѯ�¼�
        /// </summary>
        /// <param name="input">����id</param>
        /// <returns></returns>
        Task<IEnumerable<SystemDictionaryFindOutput>> Find(SystemDictionaryFindInput input);
    }
}