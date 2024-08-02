namespace EIP.Common.Repository.MicroOrm.SqlGenerator
{
    /// <summary>
    ///     Universal SqlGenerator for Tables
    /// </summary>
    public partial interface ISqlGenerator<TEntity> where TEntity : class
    {
        /// <summary>
        /// Get SQL for DELETE Query
        /// </summary>
        SqlQuery GetDeleteByIds(string ids);

        /// <summary>
        /// Get SQL for DELETE Query
        /// </summary>
        SqlQuery GetSelectByIds(string ids);
    }
}