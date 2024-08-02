namespace EIP.System.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISystemAgileRepository
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="paging"></param>
        /// <returns></returns>
        Task<PagedResults<SystemAgileFindOutput>> Find(SystemAgileFindInput paging);

        /// <summary>
        /// 获取基础
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<IEnumerable<SystemAgileFindBaseOutput>> FindBase(SystemAgileFindBaseInput input);
    }
}