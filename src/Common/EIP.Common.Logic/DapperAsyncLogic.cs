/**************************************************************
* Copyright (C) 2023 www.eipflow.com 孙泽伟版权所有(盗版必究)
*
* 作者: 孙泽伟(QQ 1039318332)
* 创建时间: 2018/10/30 22:40:15
* 文件名: 
* 描述: 
* 
* 修改历史
* 修改人：
* 时间：
* 修改说明：
*
**************************************************************/
using EIP.Common.Core.Resource;
using EIP.Common.Models;
using EIP.Common.Models.Paging;
using EIP.Common.Repository;
using EIP.Common.Repository.MicroOrm;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EIP.Common.Logic
{
    public class DapperAsyncLogic<TEntity> : IAsyncLogic<TEntity> where TEntity : class, new()
    {
        public int Count()
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return sqlMapper.Db.Repository.Count();
            }
        }

        public int Count(IDbTransaction transaction)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return sqlMapper.Db.Repository.Count(transaction);
            }
        }

        public int Count(Expression<Func<TEntity, bool>> predicate)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return sqlMapper.Db.Repository.Count(predicate);
            }
        }

        public int Count(Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return sqlMapper.Db.Repository.Count(predicate, transaction);
            }
        }

        public int Count(Expression<Func<TEntity, object>> distinctField)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return sqlMapper.Db.Repository.Count(distinctField);
            }
        }

        public int Count(Expression<Func<TEntity, object>> distinctField, IDbTransaction transaction)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return sqlMapper.Db.Repository.Count(distinctField, transaction);
            }
        }

        public int Count(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> distinctField)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return sqlMapper.Db.Repository.Count(predicate, distinctField);
            }
        }

        public int Count(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> distinctField, IDbTransaction transaction)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return sqlMapper.Db.Repository.Count(predicate, distinctField, transaction);
            }
        }

        public async Task<int> CountAsync()
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return await sqlMapper.Db.Repository.CountAsync();
            }
        }

        public async Task<int> CountAsync(IDbTransaction transaction)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return await sqlMapper.Db.Repository.CountAsync(transaction);
            }
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return await sqlMapper.Db.Repository.CountAsync(predicate);
            }
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return await sqlMapper.Db.Repository.CountAsync(predicate, transaction);
            }
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, object>> distinctField)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return await sqlMapper.Db.Repository.CountAsync(distinctField);
            }
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, object>> distinctField, IDbTransaction transaction)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return await sqlMapper.Db.Repository.CountAsync(distinctField, transaction);
            }
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> distinctField)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return await sqlMapper.Db.Repository.CountAsync(predicate, distinctField);
            }
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> distinctField, IDbTransaction transaction)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return await sqlMapper.Db.Repository.CountAsync(predicate, distinctField, transaction);
            }
        }

        public TEntity Find()
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return sqlMapper.Db.Repository.Find();
            }
        }

        public TEntity Find(Expression<Func<TEntity, bool>> predicate)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return sqlMapper.Db.Repository.Find(predicate);
            }
        }

        public TEntity Find(IDbTransaction transaction)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return sqlMapper.Db.Repository.Find(transaction);
            }
        }

        public TEntity Find(Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return sqlMapper.Db.Repository.Find(predicate, transaction);
            }
        }

        public TEntity Find<TChild1>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> tChild1, IDbTransaction transaction = null)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return sqlMapper.Db.Repository.Find<TChild1>(predicate, tChild1, transaction);
            }
        }

        public TEntity Find<TChild1, TChild2>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> tChild1, Expression<Func<TEntity, object>> tChild2,
            IDbTransaction transaction = null)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return sqlMapper.Db.Repository.Find<TChild1, TChild2>(predicate, tChild1, tChild2, transaction);
            }
        }

        public TEntity Find<TChild1, TChild2, TChild3>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> tChild1, Expression<Func<TEntity, object>> tChild2, Expression<Func<TEntity, object>> tChild3,
            IDbTransaction transaction = null)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return sqlMapper.Db.Repository.Find<TChild1, TChild2, TChild3>(predicate, tChild1, tChild2, tChild3, transaction);
            }
        }

        public TEntity Find<TChild1, TChild2, TChild3, TChild4>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> tChild1, Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3, Expression<Func<TEntity, object>> tChild4, IDbTransaction transaction = null)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return sqlMapper.Db.Repository.Find<TChild1, TChild2, TChild3, TChild4>(predicate, tChild1, tChild2, tChild3, tChild4, transaction);
            }
        }

        public TEntity Find<TChild1, TChild2, TChild3, TChild4, TChild5>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> tChild1, Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3, Expression<Func<TEntity, object>> tChild4, Expression<Func<TEntity, object>> tChild5, IDbTransaction transaction = null)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return sqlMapper.Db.Repository.Find<TChild1, TChild2, TChild3, TChild4, TChild5>(predicate, tChild1, tChild2, tChild3, tChild4, tChild5, transaction);
            }
        }

        public TEntity Find<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2, Expression<Func<TEntity, object>> tChild3, Expression<Func<TEntity, object>> tChild4, Expression<Func<TEntity, object>> tChild5, Expression<Func<TEntity, object>> tChild6,
            IDbTransaction transaction = null)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return sqlMapper.Db.Repository.Find<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(predicate, tChild1, tChild2, tChild3, tChild4, tChild5, tChild6, transaction);
            }
        }

        public TEntity FindById(object id)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return sqlMapper.Db.Repository.FindById(id);
            }
        }

        public TEntity FindById(object id, IDbTransaction transaction)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return sqlMapper.Db.Repository.FindById(id, transaction);
            }
        }

        public TEntity FindById<TChild1>(object id, Expression<Func<TEntity, object>> tChild1, IDbTransaction transaction = null)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return sqlMapper.Db.Repository.FindById<TChild1>(id, tChild1, transaction);
            }
        }

        public TEntity FindById<TChild1, TChild2>(object id, Expression<Func<TEntity, object>> tChild1, Expression<Func<TEntity, object>> tChild2, IDbTransaction transaction = null)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return sqlMapper.Db.Repository.FindById<TChild1, TChild2>(id, tChild1, tChild2, transaction);
            }
        }

        public TEntity FindById<TChild1, TChild2, TChild3>(object id, Expression<Func<TEntity, object>> tChild1, Expression<Func<TEntity, object>> tChild2, Expression<Func<TEntity, object>> tChild3,
            IDbTransaction transaction = null)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return sqlMapper.Db.Repository.FindById<TChild1, TChild2, TChild3>(id, tChild1, tChild2, tChild3, transaction);
            }
        }

        public TEntity FindById<TChild1, TChild2, TChild3, TChild4>(object id, Expression<Func<TEntity, object>> tChild1, Expression<Func<TEntity, object>> tChild2, Expression<Func<TEntity, object>> tChild3,
            Expression<Func<TEntity, object>> tChild4, IDbTransaction transaction = null)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return sqlMapper.Db.Repository.FindById<TChild1, TChild2, TChild3, TChild4>(id, tChild1, tChild2, tChild3, tChild4, transaction);
            }
        }

        public TEntity FindById<TChild1, TChild2, TChild3, TChild4, TChild5>(object id, Expression<Func<TEntity, object>> tChild1, Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3, Expression<Func<TEntity, object>> tChild4, Expression<Func<TEntity, object>> tChild5, IDbTransaction transaction = null)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return sqlMapper.Db.Repository.FindById<TChild1, TChild2, TChild3, TChild4, TChild5>(id, tChild1, tChild2, tChild3, tChild4, tChild5, transaction);
            }
        }

        public TEntity FindById<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(object id, Expression<Func<TEntity, object>> tChild1, Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3, Expression<Func<TEntity, object>> tChild4, Expression<Func<TEntity, object>> tChild5, Expression<Func<TEntity, object>> tChild6, IDbTransaction transaction = null)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return sqlMapper.Db.Repository.FindById<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(id, tChild1, tChild2, tChild3, tChild4, tChild5, tChild6, transaction);
            }
        }

        public async Task<TEntity> FindByIdAsync(object id)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return await sqlMapper.Db.Repository.FindByIdAsync(id);
            }
        }

        public async Task<TEntity> FindByIdAsync(object id, IDbTransaction transaction)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return await sqlMapper.Db.Repository.FindByIdAsync(id, transaction);
            }
        }

        public async Task<TEntity> FindByIdAsync<TChild1>(object id, Expression<Func<TEntity, object>> tChild1, IDbTransaction transaction = null)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return await sqlMapper.Db.Repository.FindByIdAsync<TChild1>(id, tChild1, transaction);
            }
        }

        public async Task<TEntity> FindByIdAsync<TChild1, TChild2>(object id, Expression<Func<TEntity, object>> tChild1, Expression<Func<TEntity, object>> tChild2,
            IDbTransaction transaction = null)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return await sqlMapper.Db.Repository.FindByIdAsync<TChild1, TChild2>(id, tChild1, tChild2, transaction);
            }
        }

        public async Task<TEntity> FindByIdAsync<TChild1, TChild2, TChild3>(object id, Expression<Func<TEntity, object>> tChild1, Expression<Func<TEntity, object>> tChild2, Expression<Func<TEntity, object>> tChild3,
            IDbTransaction transaction = null)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return await sqlMapper.Db.Repository.FindByIdAsync<TChild1, TChild2, TChild3>(id, tChild1, tChild2, tChild3, transaction);
            }
        }

        public async Task<TEntity> FindByIdAsync<TChild1, TChild2, TChild3, TChild4>(object id, Expression<Func<TEntity, object>> tChild1, Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3, Expression<Func<TEntity, object>> tChild4, IDbTransaction transaction = null)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return await sqlMapper.Db.Repository.FindByIdAsync<TChild1, TChild2, TChild3, TChild4>(id, tChild1, tChild2, tChild3, tChild4, transaction);
            }
        }

        public async Task<TEntity> FindByIdAsync<TChild1, TChild2, TChild3, TChild4, TChild5>(object id, Expression<Func<TEntity, object>> tChild1, Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3, Expression<Func<TEntity, object>> tChild4, Expression<Func<TEntity, object>> tChild5, IDbTransaction transaction = null)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return await sqlMapper.Db.Repository.FindByIdAsync<TChild1, TChild2, TChild3, TChild4, TChild5>(id, tChild1, tChild2, tChild3, tChild4, tChild5, transaction);
            }
        }

        public async Task<TEntity> FindByIdAsync<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(object id, Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2, Expression<Func<TEntity, object>> tChild3, Expression<Func<TEntity, object>> tChild4, Expression<Func<TEntity, object>> tChild5, Expression<Func<TEntity, object>> tChild6,
            IDbTransaction transaction = null)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return await sqlMapper.Db.Repository.FindByIdAsync<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(id, tChild1, tChild2, tChild3, tChild4, tChild5, tChild6, transaction);
            }
        }

        public async Task<TEntity> FindAsync()
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return await sqlMapper.Db.Repository.FindAsync();
            }
        }

        public async Task<TEntity> FindAsync(IDbTransaction transaction)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return await sqlMapper.Db.Repository.FindAsync(transaction);
            }
        }

        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return await sqlMapper.Db.Repository.FindAsync(predicate);
            }
        }

        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return await sqlMapper.Db.Repository.FindAsync(predicate, transaction);
            }
        }

        public async Task<TEntity> FindAsync<TChild1>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> tChild1, IDbTransaction transaction = null)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return await sqlMapper.Db.Repository.FindAsync<TChild1>(predicate, tChild1, transaction);
            }
        }

        public async Task<TEntity> FindAsync<TChild1, TChild2>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> tChild1, Expression<Func<TEntity, object>> tChild2,
            IDbTransaction transaction = null)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return await sqlMapper.Db.Repository.FindAsync<TChild1, TChild2>(predicate, tChild1, tChild2, transaction);
            }
        }

        public async Task<TEntity> FindAsync<TChild1, TChild2, TChild3>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> tChild1, Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3, IDbTransaction transaction = null)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return await sqlMapper.Db.Repository.FindAsync<TChild1, TChild2, TChild3>(predicate, tChild1, tChild2, tChild3, transaction);
            }
        }

        public async Task<TEntity> FindAsync<TChild1, TChild2, TChild3, TChild4>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> tChild1, Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3, Expression<Func<TEntity, object>> tChild4, IDbTransaction transaction = null)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return await sqlMapper.Db.Repository.FindAsync<TChild1, TChild2, TChild3, TChild4>(predicate, tChild1, tChild2, tChild3, tChild4, transaction);
            }
        }

        public async Task<TEntity> FindAsync<TChild1, TChild2, TChild3, TChild4, TChild5>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2, Expression<Func<TEntity, object>> tChild3, Expression<Func<TEntity, object>> tChild4, Expression<Func<TEntity, object>> tChild5, IDbTransaction transaction = null)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return await sqlMapper.Db.Repository.FindAsync<TChild1, TChild2, TChild3, TChild4, TChild5>(predicate, tChild1, tChild2, tChild3, tChild4, tChild5, transaction);
            }
        }

        public async Task<TEntity> FindAsync<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2, Expression<Func<TEntity, object>> tChild3, Expression<Func<TEntity, object>> tChild4, Expression<Func<TEntity, object>> tChild5, Expression<Func<TEntity, object>> tChild6,
            IDbTransaction transaction = null)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return await sqlMapper.Db.Repository.FindAsync<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(predicate, tChild1, tChild2, tChild3, tChild4, tChild5, tChild6, transaction);
            }
        }

        public IEnumerable<TEntity> FindAll(IDbTransaction transaction = null)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return sqlMapper.Db.Repository.FindAll(transaction);
            }
        }

        public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction = null)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return sqlMapper.Db.Repository.FindAll(predicate, transaction);
            }
        }

        public IEnumerable<TEntity> FindAll<TChild1>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> tChild1, IDbTransaction transaction = null)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return sqlMapper.Db.Repository.FindAll<TChild1>(predicate, tChild1, transaction);
            }
        }

        public IEnumerable<TEntity> FindAll<TChild1, TChild2>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> tChild1, Expression<Func<TEntity, object>> tChild2,
            IDbTransaction transaction = null)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return sqlMapper.Db.Repository.FindAll<TChild1, TChild2>(predicate, tChild1, tChild2, transaction);
            }
        }

        public IEnumerable<TEntity> FindAll<TChild1, TChild2, TChild3>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> tChild1, Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3, IDbTransaction transaction = null)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return sqlMapper.Db.Repository.FindAll<TChild1, TChild2, TChild3>(predicate, tChild1, tChild2, tChild3, transaction);
            }
        }

        public IEnumerable<TEntity> FindAll<TChild1, TChild2, TChild3, TChild4>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> tChild1, Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3, Expression<Func<TEntity, object>> tChild4, IDbTransaction transaction = null)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return sqlMapper.Db.Repository.FindAll<TChild1, TChild2, TChild3, TChild4>(predicate, tChild1, tChild2, tChild3, tChild4, transaction);
            }
        }

        public IEnumerable<TEntity> FindAll<TChild1, TChild2, TChild3, TChild4, TChild5>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2, Expression<Func<TEntity, object>> tChild3, Expression<Func<TEntity, object>> tChild4, Expression<Func<TEntity, object>> tChild5, IDbTransaction transaction = null)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return sqlMapper.Db.Repository.FindAll<TChild1, TChild2, TChild3, TChild4, TChild5>(predicate, tChild1, tChild2, tChild3, tChild4, tChild5, transaction);
            }
        }

        public IEnumerable<TEntity> FindAll<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2, Expression<Func<TEntity, object>> tChild3, Expression<Func<TEntity, object>> tChild4, Expression<Func<TEntity, object>> tChild5, Expression<Func<TEntity, object>> tChild6,
            IDbTransaction transaction = null)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return sqlMapper.Db.Repository.FindAll<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(predicate, tChild1, tChild2, tChild3, tChild4, tChild5, tChild6, transaction);
            }
        }

        public async Task<IEnumerable<TEntity>> FindAllAsync(IDbTransaction transaction = null)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return await sqlMapper.Db.Repository.FindAllAsync(transaction);
            }
        }

        public async Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction = null)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return await sqlMapper.Db.Repository.FindAllAsync(predicate, transaction);
            }
        }

        public async Task<IEnumerable<TEntity>> FindAllAsync<TChild1>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> tChild1, IDbTransaction transaction = null)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return await sqlMapper.Db.Repository.FindAllAsync<TChild1>(predicate, tChild1, transaction);
            }
        }

        public async Task<IEnumerable<TEntity>> FindAllAsync<TChild1, TChild2>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> tChild1, Expression<Func<TEntity, object>> tChild2,
            IDbTransaction transaction = null)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return await sqlMapper.Db.Repository.FindAllAsync<TChild1, TChild2>(predicate, tChild1, tChild2, transaction);
            }
        }

        public async Task<IEnumerable<TEntity>> FindAllAsync<TChild1, TChild2, TChild3>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> tChild1, Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3, IDbTransaction transaction = null)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return await sqlMapper.Db.Repository.FindAllAsync<TChild1, TChild2, TChild3>(predicate, tChild1, tChild2, tChild3, transaction);
            }
        }

        public async Task<IEnumerable<TEntity>> FindAllAsync<TChild1, TChild2, TChild3, TChild4>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> tChild1, Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3, Expression<Func<TEntity, object>> tChild4, IDbTransaction transaction = null)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return await sqlMapper.Db.Repository.FindAllAsync<TChild1, TChild2, TChild3, TChild4>(predicate, tChild1, tChild2, tChild3, tChild4, transaction);
            }
        }

        public async Task<IEnumerable<TEntity>> FindAllAsync<TChild1, TChild2, TChild3, TChild4, TChild5>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2, Expression<Func<TEntity, object>> tChild3, Expression<Func<TEntity, object>> tChild4, Expression<Func<TEntity, object>> tChild5, IDbTransaction transaction = null)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return await sqlMapper.Db.Repository.FindAllAsync<TChild1, TChild2, TChild3, TChild4, TChild5>(predicate, tChild1, tChild2, tChild3, tChild4, tChild5, transaction);
            }
        }

        public async Task<IEnumerable<TEntity>> FindAllAsync<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2, Expression<Func<TEntity, object>> tChild3, Expression<Func<TEntity, object>> tChild4, Expression<Func<TEntity, object>> tChild5, Expression<Func<TEntity, object>> tChild6,
            IDbTransaction transaction = null)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return await sqlMapper.Db.Repository.FindAllAsync<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(predicate, tChild1, tChild2, tChild3, tChild4, tChild5, tChild6, transaction);
            }
        }

        public OperateStatus Insert(TEntity instance)
        {
            try
            {
                using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
                {
                    var result = sqlMapper.Db.Repository.Insert(instance);
                    return result ? OperateStatus.Success() : OperateStatus.Error();
                }
            }
            catch (Exception exception)
            {
                return OperateStatus.Error(string.Format(Chs.Error, exception.Message));
            }
        }

        public OperateStatus Insert(TEntity instance, IDbTransaction transaction)
        {
            try
            {
                using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
                {
                    var result = sqlMapper.Db.Repository.Insert(instance, transaction);
                    return result ? OperateStatus.Success() : OperateStatus.Error();
                }
            }
            catch (Exception exception)
            {
                return OperateStatus.Error(string.Format(Chs.Error, exception.Message));
            }
        }

        public async Task<OperateStatus> InsertAsync(TEntity instance)
        {
            try
            {
                using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
                {
                    var result = await sqlMapper.Db.Repository.InsertAsync(instance);
                    return result ? OperateStatus.Success() : OperateStatus.Error();
                }
            }
            catch (Exception exception)
            {
                return OperateStatus.Error(string.Format(Chs.Error, exception.Message));
            }
        }

        public async Task<OperateStatus> InsertAsync(TEntity instance, IDbTransaction transaction)
        {
            try
            {
                using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
                {
                    var result = await sqlMapper.Db.Repository.InsertAsync(instance, transaction);
                    return result ? OperateStatus.Success() : OperateStatus.Error();
                }
            }
            catch (Exception exception)
            {
                return OperateStatus.Error(string.Format(Chs.Error, exception.Message));
            }
        }

        public OperateStatus BulkInsert(IEnumerable<TEntity> instances, IDbTransaction transaction = null)
        {
            try
            {
                using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
                {
                    var result = sqlMapper.Db.Repository.BulkInsert(instances, transaction);
                    return result > 0 ? OperateStatus.Success() : OperateStatus.Error();
                }
            }
            catch (Exception exception)
            {
                return OperateStatus.Error(string.Format(Chs.Error, exception.Message));
            }
        }

        public OperateStatus BulkCopyInsert<T>(IList<T> instances) where T : new()
        {
            try
            {
                using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
                {
                    var result = sqlMapper.Db.Repository.BulkCopyInsert<T>(instances);
                    return result > 0 ? OperateStatus.Success() : OperateStatus.Error();
                }
            }
            catch (Exception exception)
            {
                return OperateStatus.Error(string.Format(Chs.Error, exception.Message));
            }
        }

        public async Task<OperateStatus> BulkInsertAsync(IEnumerable<TEntity> instances, IDbTransaction transaction = null)
        {
            try
            {
                using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
                {
                    var result = await sqlMapper.Db.Repository.BulkInsertAsync(instances, transaction);
                    return result > 0 ? OperateStatus.Success() : OperateStatus.Error();
                }
            }
            catch (Exception exception)
            {
                return OperateStatus.Error(string.Format(Chs.Error, exception.Message));
            }
        }

        public OperateStatus Delete(TEntity instance, IDbTransaction transaction = null)
        {
            try
            {
                using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
                {
                    var result = sqlMapper.Db.Repository.Delete(instance, transaction);
                    return result ? OperateStatus.Success() : OperateStatus.Error();
                }
            }
            catch (Exception exception)
            {
                return OperateStatus.Error(string.Format(Chs.Error, exception.Message));
            }
        }

        public async Task<OperateStatus> DeleteAsync(TEntity instance, IDbTransaction transaction = null)
        {
            try
            {
                using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
                {
                    var result = await sqlMapper.Db.Repository.DeleteAsync(instance, transaction);
                    return result ? OperateStatus.Success() : OperateStatus.Error();
                }
            }
            catch (Exception exception)
            {
                return OperateStatus.Error(string.Format(Chs.Error, exception.Message));
            }
        }

        public OperateStatus Delete(Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction = null)
        {
            try
            {
                using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
                {
                    var result = sqlMapper.Db.Repository.Delete(predicate, transaction);
                    return result ? OperateStatus.Success() : OperateStatus.Error();
                }
            }
            catch (Exception exception)
            {
                return OperateStatus.Error(string.Format(Chs.Error, exception.Message));
            }
        }

        public async Task<OperateStatus> DeleteAsync(Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction = null)
        {
            try
            {
                using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
                {
                    var result = await sqlMapper.Db.Repository.DeleteAsync(predicate, transaction);
                    return result ? OperateStatus.Success() : OperateStatus.Error();
                }
            }
            catch (Exception exception)
            {
                return OperateStatus.Error(string.Format(Chs.Error, exception.Message));
            }
        }

        public OperateStatus Update(TEntity instance)
        {
            try
            {
                using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
                {
                    var result = sqlMapper.Db.Repository.Update(instance);
                    return result ? OperateStatus.Success() : OperateStatus.Error();
                }
            }
            catch (Exception exception)
            {
                return OperateStatus.Error(string.Format(Chs.Error, exception.Message));
            }
        }
        public OperateStatus Update(Expression<Func<TEntity, bool>> predicate, object setPropertyObj)
        {
            try
            {
                using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
                {
                    var result = sqlMapper.Db.Repository.Update(predicate, setPropertyObj);
                    return result ? OperateStatus.Success() : OperateStatus.Error();
                }
            }
            catch (Exception exception)
            {
                return OperateStatus.Error(string.Format(Chs.Error, exception.Message));
            }
        }
        public OperateStatus Update(Expression<Func<TEntity, bool>> predicate, Dictionary<string, object> setPropertyDict)
        {
            try
            {
                using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
                {
                    var result = sqlMapper.Db.Repository.Update(predicate, setPropertyDict);
                    return result ? OperateStatus.Success() : OperateStatus.Error();
                }
            }
            catch (Exception exception)
            {
                return OperateStatus.Error(string.Format(Chs.Error, exception.Message));
            }
        }
        public OperateStatus Update(TEntity instance, IDbTransaction transaction)
        {
            try
            {
                using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
                {
                    var result = sqlMapper.Db.Repository.Update(instance, transaction);
                    return result ? OperateStatus.Success() : OperateStatus.Error();
                }
            }
            catch (Exception exception)
            {
                return OperateStatus.Error(string.Format(Chs.Error, exception.Message));
            }
        }

        public async Task<OperateStatus> UpdateAsync(TEntity instance)
        {
            try
            {
                using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
                {
                    var result = await sqlMapper.Db.Repository.UpdateAsync(instance);
                    return result ? OperateStatus.Success() : OperateStatus.Error();
                }
            }
            catch (Exception exception)
            {
                return OperateStatus.Error(string.Format(Chs.Error, exception.Message));
            }
        }

        public async Task<OperateStatus> UpdateAsync(TEntity instance, IDbTransaction transaction)
        {
            try
            {
                using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
                {
                    var result = await sqlMapper.Db.Repository.UpdateAsync(instance, transaction);
                    return result ? OperateStatus.Success() : OperateStatus.Error();
                }
            }
            catch (Exception exception)
            {
                return OperateStatus.Error(string.Format(Chs.Error, exception.Message));
            }
        }

        public OperateStatus Update(Expression<Func<TEntity, bool>> predicate, TEntity instance)
        {
            try
            {
                using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
                {
                    var result = sqlMapper.Db.Repository.Update(predicate, instance);
                    return result ? OperateStatus.Success() : OperateStatus.Error();
                }
            }
            catch (Exception exception)
            {
                return OperateStatus.Error(string.Format(Chs.Error, exception.Message));
            }
        }

        public OperateStatus Update(Expression<Func<TEntity, bool>> predicate, TEntity instance, IDbTransaction transaction)
        {
            try
            {
                using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
                {
                    var result = sqlMapper.Db.Repository.Update(predicate, instance, transaction);
                    return result ? OperateStatus.Success() : OperateStatus.Error();
                }
            }
            catch (Exception exception)
            {
                return OperateStatus.Error(string.Format(Chs.Error, exception.Message));
            }
        }
        public async Task<OperateStatus> UpdateAsync(Expression<Func<TEntity, bool>> predicate, object setPropertyObj)
        {
            try
            {
                using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
                {
                    var result = await sqlMapper.Db.Repository.UpdateAsync(predicate, setPropertyObj);
                    return result ? OperateStatus.Success() : OperateStatus.Error();
                }
            }
            catch (Exception exception)
            {
                return OperateStatus.Error(string.Format(Chs.Error, exception.Message));
            }
        }
        public async Task<OperateStatus> UpdateAsync(Expression<Func<TEntity, bool>> predicate, Dictionary<string, object> setPropertyDict)
        {
            try
            {
                using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
                {
                    var result = await sqlMapper.Db.Repository.UpdateAsync(predicate, setPropertyDict);
                    return result ? OperateStatus.Success() : OperateStatus.Error();
                }
            }
            catch (Exception exception)
            {
                return OperateStatus.Error(string.Format(Chs.Error, exception.Message));
            }
        }
        public async Task<OperateStatus> UpdateAsync(Expression<Func<TEntity, bool>> predicate, TEntity instance)
        {
            try
            {
                using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
                {
                    var result = await sqlMapper.Db.Repository.UpdateAsync(predicate, instance);
                    return result ? OperateStatus.Success() : OperateStatus.Error();
                }
            }
            catch (Exception exception)
            {
                return OperateStatus.Error(string.Format(Chs.Error, exception.Message));
            }
        }

        public async Task<OperateStatus> UpdateAsync(Expression<Func<TEntity, bool>> predicate, TEntity instance, IDbTransaction transaction)
        {
            try
            {
                using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
                {
                    var result = await sqlMapper.Db.Repository.UpdateAsync(predicate, instance, transaction);
                    return result ? OperateStatus.Success() : OperateStatus.Error();
                }
            }
            catch (Exception exception)
            {
                return OperateStatus.Error(string.Format(Chs.Error, exception.Message));
            }
        }

        public async Task<OperateStatus> BulkUpdateAsync(IEnumerable<TEntity> instances)
        {
            try
            {
                using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
                {
                    var result = await sqlMapper.Db.Repository.BulkUpdateAsync(instances);
                    return result ? OperateStatus.Success() : OperateStatus.Error();
                }
            }
            catch (Exception exception)
            {
                return OperateStatus.Error(string.Format(Chs.Error, exception.Message));
            }
        }

        public async Task<OperateStatus> BulkUpdateAsync(IEnumerable<TEntity> instances, IDbTransaction transaction)
        {
            try
            {
                using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
                {
                    var result = await sqlMapper.Db.Repository.BulkUpdateAsync(instances, transaction);
                    return result ? OperateStatus.Success() : OperateStatus.Error();
                }
            }
            catch (Exception exception)
            {
                return OperateStatus.Error(string.Format(Chs.Error, exception.Message));
            }
        }

        public OperateStatus BulkUpdate(IEnumerable<TEntity> instances)
        {
            try
            {
                using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
                {
                    var result = sqlMapper.Db.Repository.BulkUpdate(instances);
                    return result ? OperateStatus.Success() : OperateStatus.Error();
                }
            }
            catch (Exception exception)
            {
                return OperateStatus.Error(string.Format(Chs.Error, exception.Message));
            }
        }

        public OperateStatus BulkUpdate(IEnumerable<TEntity> instances, IDbTransaction transaction)
        {
            try
            {
                using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
                {
                    var result = sqlMapper.Db.Repository.BulkUpdate(instances, transaction);
                    return result ? OperateStatus.Success() : OperateStatus.Error();
                }
            }
            catch (Exception exception)
            {
                return OperateStatus.Error(string.Format(Chs.Error, exception.Message));
            }
        }

        public IEnumerable<TEntity> FindAllBetween(object from, object to, Expression<Func<TEntity, object>> btwField, IDbTransaction transaction = null)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return sqlMapper.Db.Repository.FindAllBetween(from, to, btwField, transaction);
            }
        }

        public IEnumerable<TEntity> FindAllBetween(object from, object to, Expression<Func<TEntity, object>> btwField, Expression<Func<TEntity, bool>> predicate = null,
            IDbTransaction transaction = null)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return sqlMapper.Db.Repository.FindAllBetween(from, to, btwField, predicate, transaction);
            }
        }

        public IEnumerable<TEntity> FindAllBetween(DateTime from, DateTime to, Expression<Func<TEntity, object>> btwField, IDbTransaction transaction = null)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return sqlMapper.Db.Repository.FindAllBetween(from, to, btwField, transaction);
            }
        }

        public IEnumerable<TEntity> FindAllBetween(DateTime from, DateTime to, Expression<Func<TEntity, object>> btwField, Expression<Func<TEntity, bool>> predicate,
            IDbTransaction transaction = null)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return sqlMapper.Db.Repository.FindAllBetween(from, to, btwField, predicate, transaction);
            }
        }

        public async Task<IEnumerable<TEntity>> FindAllBetweenAsync(object from, object to, Expression<Func<TEntity, object>> btwField, IDbTransaction transaction = null)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return await sqlMapper.Db.Repository.FindAllBetweenAsync(from, to, btwField, transaction);
            }
        }

        public async Task<IEnumerable<TEntity>> FindAllBetweenAsync(object from, object to, Expression<Func<TEntity, object>> btwField, Expression<Func<TEntity, bool>> predicate,
            IDbTransaction transaction = null)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return await sqlMapper.Db.Repository.FindAllBetweenAsync(from, to, btwField, predicate, transaction);
            }
        }

        public async Task<IEnumerable<TEntity>> FindAllBetweenAsync(DateTime from, DateTime to, Expression<Func<TEntity, object>> btwField, IDbTransaction transaction = null)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return await sqlMapper.Db.Repository.FindAllBetweenAsync(from, to, btwField, transaction);
            }
        }

        public async Task<IEnumerable<TEntity>> FindAllBetweenAsync(DateTime from, DateTime to, Expression<Func<TEntity, object>> btwField, Expression<Func<TEntity, bool>> predicate,
            IDbTransaction transaction = null)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return await sqlMapper.Db.Repository.FindAllBetweenAsync(from, to, btwField, predicate, transaction);
            }
        }

        public async Task<OperateStatus> DeleteByIdsAsync(string ids)
        {

            try
            {
                using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
                {
                    var result = await sqlMapper.Db.Repository.DeleteByIdsAsync(ids);
                    return result > 0 ? OperateStatus.Success() : OperateStatus.Error();
                }
            }
            catch (Exception exception)
            {
                return OperateStatus.Error(string.Format(Chs.Error, exception.Message));
            }

        }

        public async Task<IEnumerable<TEntity>> FindByIdsAsync(string ids)
        {
            using (MicroOrmUtil<TEntity> sqlMapper = new MicroOrmUtil<TEntity>())
            {
                return await sqlMapper.Db.Repository.FindByIdsAsync(ids);
            }
        }

        /// <summary>
        /// 存储过程分页
        /// </summary>
        /// <param name="queryParam"></param>
        /// <returns></returns>
        public Task<PagedResults<TEntity>> PagingQueryProcAsync(QueryParam queryParam)
        {
            return new SqlMapperUtil().PagingQueryProcAsync<TEntity>(queryParam);
        }

        /// <summary>
        /// 单表分页
        /// </summary>
        /// <param name="queryParam"></param>
        /// <returns></returns>
        public Task<PagedResults<TEntity>> PagingQuerySqlSingleTableAsync(QueryParam queryParam)
        {
            return new SqlMapperUtil().PagingQuerySqlSingleTableAsync<TEntity>(queryParam);
        }
    }
}