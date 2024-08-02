using EIP.System.Models.Dtos.Dictionary;

namespace EIP.System.Repository.Impl
{
    /// <summary>
    /// �ֵ����ݷ��ʽӿ�ʵ��
    /// </summary>
    public class SystemDictionaryRepository : ISystemDictionaryRepository
    {
        /// <summary>
        /// �����ֵ�����ȡ��Ӧ�¼�ֵ
        /// </summary>
        /// <param name="input">����ֵ</param>
        /// <returns></returns>
        public Task<IEnumerable<SystemDictionaryFindOutput>> Find(SystemDictionaryFindInput input)
        {
            var sql = new StringBuilder();
            sql.Append("select dic.DictionaryId, dic.ParentId,dic.Name,dic.Value,dic.IsFreeze,dic.OrderNo,dic.Remark,dic.CreateTime,dic.CreateUserName,dic.UpdateTime,dic.UpdateUserName from System_Dictionary dic where 1=1 ");
            sql.Append(input.Sql);
            return new SqlMapperUtil().SqlWithParams<SystemDictionaryFindOutput>(sql.ToString());
        }
    }
}