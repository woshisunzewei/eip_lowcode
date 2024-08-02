using EIP.System.Models.Dtos.Dictionary;

namespace EIP.System.Repository
{
    /// <summary>
    /// 字典数据访问接口
    /// </summary>
    public interface ISystemDictionaryRepository
    {
        /// <summary>
        /// 根据父级查询下级
        /// </summary>
        /// <param name="input">父级id</param>
        /// <returns></returns>
        Task<IEnumerable<SystemDictionaryFindOutput>> Find(SystemDictionaryFindInput input);
    }
}