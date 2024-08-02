using EIP.System.Models.Dtos.Dictionary;

namespace EIP.System.Repository.Impl
{
    /// <summary>
    /// 字典数据访问接口实现
    /// </summary>
    public class SystemDictionaryRepository : ISystemDictionaryRepository
    {
        /// <summary>
        /// 根据字典代码获取对应下级值
        /// </summary>
        /// <param name="input">代码值</param>
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