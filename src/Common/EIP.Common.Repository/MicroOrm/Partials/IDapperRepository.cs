using System.Collections.Generic;
using System.Threading.Tasks;

namespace EIP.Common.Repository.MicroOrm
{
    public partial interface IDapperRepository<TEntity> where TEntity : class
    {
        /// <summary>
        /// Delete objects from DB
        /// </summary>
        Task<int> DeleteByIdsAsync(string ids);

        /// <summary>
        /// 根据ids查询
        /// </summary>
        Task<IEnumerable<TEntity>> FindByIdsAsync(string ids);

        /// <summary>
        /// Bulk Insert objects to DB
        /// </summary>
        int BulkCopyInsert<T>(IList<T> instances) where T : new();

    }
}
