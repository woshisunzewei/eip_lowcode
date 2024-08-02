using System.Data;
using EIP.Common.Repository.MicroOrm.SqlGenerator;

namespace EIP.Common.Repository.MicroOrm
{
    /// <summary>
    ///     Base Repository
    /// </summary>
    public partial class DapperRepository<TEntity> : ReadOnlyDapperRepository<TEntity>, IDapperRepository<TEntity>
        where TEntity : class
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        public DapperRepository(IDbConnection connection)
            : base(connection)
        {
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        public DapperRepository(IDbConnection connection, ISqlGenerator<TEntity> sqlGenerator)
            : base(connection, sqlGenerator)
        {
        }
    }
}
