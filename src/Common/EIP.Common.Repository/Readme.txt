using Dapper;
using EIP.Common.Cache;
using System.Data;
using System.Threading.Tasks;

namespace EIP.Common.Repository.MicroOrm
{
    /// <summary>
    /// Base Repository
    /// </summary>
    public partial class DapperRepository<TEntity>
        where TEntity : class
    {
        /// <inheritdoc />
        public virtual TEntity FindById(object id)
        {
            return FindById(id, null);
        }

        /// <inheritdoc />
        public virtual TEntity FindById(object id, IDbTransaction transaction)
        {
            var queryResult = SqlGenerator.GetSelectById(id);
            return Connection.QuerySingleOrDefault<TEntity>(queryResult.GetSql(), queryResult.Param, transaction);
        }

        /// <inheritdoc />
        public virtual async Task<TEntity> FindByIdAsync(object id)
        {
            return await FindByIdAsync(id, null);
        }

        /// <inheritdoc />
        public virtual async Task<TEntity> FindByIdAsync(object id, IDbTransaction transaction)
        {
            if (NeedMemCache)
            {
                //先从缓存中获取
                var result = await TryGetByIdFromCache(id);
                if (result != null)
                {
                    return result;
                }
                //从数据库中获取
                result = await FindByIdAsync(id, null);
                if (result != null)
                {
                    //刷新单值缓存
                    var redisManager = new RedisManager(_dbNum);
                    var key = RecordCacheKey(id);
                    redisManager.KeyDelete(key);
                    redisManager.ListLeftPush(key, result);
                    redisManager.KeyExpire(key, Expiry);
                }
                return result;
            }
            var queryResult = SqlGenerator.GetSelectById(id);
            return await Connection.QuerySingleOrDefaultAsync<TEntity>(queryResult.GetSql(), queryResult.Param, transaction);
        }

        /// <summary>
        /// 从数据库获取
        /// </summary>
        /// <param name="id"></param>
        /// <param name="transaction"></param>
        /// <returns></returns>
        public virtual async Task<TEntity> FindByIdFromDataBaseAsync(object id, IDbTransaction transaction)
        {
            var queryResult = SqlGenerator.GetSelectById(id);
            return await Connection.QuerySingleOrDefaultAsync<TEntity>(queryResult.GetSql(), queryResult.Param, transaction);
        }


        #region 缓存
        /// <summary>
        /// 根据Id从数据库获取对象
        /// </summary>
        /// <param name="id">实体Id</param>
        /// <returns>实体对象</returns>
        protected virtual async Task<TEntity> TryGetByIdFromCache(object id)
        {
            return !NeedMemCache ? null : await new RedisManager(_dbNum).ListLeftPopAsync<TEntity>(RecordCacheKey(id));
        }
        #endregion
    }
}
