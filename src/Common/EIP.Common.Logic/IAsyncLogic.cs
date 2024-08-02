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
using EIP.Common.Models;
using EIP.Common.Models.Paging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace EIP.Common.Logic
{
    public interface IAsyncLogic<TEntity> where TEntity : class
    {

        /// <summary>
        /// Get number of rows
        /// </summary>
        int Count();

        /// <summary>
        /// Get number of rows
        /// </summary>
        int Count(IDbTransaction transaction);

        /// <summary>
        /// Get number of rows with WHERE clause
        /// </summary>
        int Count(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Get number of rows with WHERE clause
        /// </summary>
        int Count(Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction);

        /// <summary>
        /// Get number of rows with DISTINCT clause
        /// </summary>
        int Count(Expression<Func<TEntity, object>> distinctField);

        /// <summary>
        /// Get number of rows with DISTINCT clause
        /// </summary>
        int Count(Expression<Func<TEntity, object>> distinctField, IDbTransaction transaction);

        /// <summary>
        /// Get number of rows with DISTINCT and WHERE clause
        /// </summary>
        int Count(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> distinctField);

        /// <summary>
        /// Get number of rows with DISTINCT and WHERE clause
        /// </summary>
        int Count(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> distinctField, IDbTransaction transaction);

        /// <summary>
        /// Get number of rows
        /// </summary>
        Task<int> CountAsync();

        /// <summary>
        /// Get number of rows
        /// </summary>
        Task<int> CountAsync(IDbTransaction transaction);

        /// <summary>
        /// Get number of rows with WHERE clause
        /// </summary>
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Get number of rows with WHERE clause
        /// </summary>
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction);

        /// <summary>
        /// Get number of rows with DISTINCT clause
        /// </summary>
        Task<int> CountAsync(Expression<Func<TEntity, object>> distinctField);

        /// <summary>
        /// Get number of rows with DISTINCT clause
        /// </summary>
        Task<int> CountAsync(Expression<Func<TEntity, object>> distinctField, IDbTransaction transaction);

        /// <summary>
        /// Get number of rows with DISTINCT and WHERE clause
        /// </summary>
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> distinctField);

        /// <summary>
        /// Get number of rows with DISTINCT and WHERE clause
        /// </summary>
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> distinctField, IDbTransaction transaction);

        /// <summary>
        /// Get first object
        /// </summary>
        TEntity Find();

        /// <summary>
        /// Get first object
        /// </summary>
        TEntity Find(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Get first object
        /// </summary>
        TEntity Find(IDbTransaction transaction);

        /// <summary>
        /// Get first object
        /// </summary>
        TEntity Find(Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction);

        /// <summary>
        /// Get first object with join objects
        /// </summary>
        TEntity Find<TChild1>(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> tChild1,
            IDbTransaction transaction = null);

        /// <summary>
        /// Get first object with join objects
        /// </summary>
        TEntity Find<TChild1, TChild2>(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            IDbTransaction transaction = null);

        /// <summary>
        /// Get first object with join objects
        /// </summary>
        TEntity Find<TChild1, TChild2, TChild3>(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3,
            IDbTransaction transaction = null);

        /// <summary>
        /// Get first object with join objects
        /// </summary>
        TEntity Find<TChild1, TChild2, TChild3, TChild4>(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3,
            Expression<Func<TEntity, object>> tChild4,
            IDbTransaction transaction = null);

        /// <summary>
        /// Get first object with join objects
        /// </summary>
        TEntity Find<TChild1, TChild2, TChild3, TChild4, TChild5>(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3,
            Expression<Func<TEntity, object>> tChild4,
            Expression<Func<TEntity, object>> tChild5,
            IDbTransaction transaction = null);

        /// <summary>
        /// Get first object with join objects
        /// </summary>
        TEntity Find<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3,
            Expression<Func<TEntity, object>> tChild4,
            Expression<Func<TEntity, object>> tChild5,
            Expression<Func<TEntity, object>> tChild6,
            IDbTransaction transaction = null);

        /// <summary>
        /// Get object by Id
        /// </summary>
        TEntity FindById(object id);

        /// <summary>
        /// Get object by Id
        /// </summary>
        TEntity FindById(object id, IDbTransaction transaction);

        /// <summary>
        /// Get object by Id with join objects
        /// </summary>
        TEntity FindById<TChild1>(object id,
            Expression<Func<TEntity, object>> tChild1,
            IDbTransaction transaction = null);

        /// <summary>
        /// Get object by Id with join objects
        /// </summary>
        TEntity FindById<TChild1, TChild2>(object id,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            IDbTransaction transaction = null);

        /// <summary>
        /// Get object by Id with join objects
        /// </summary>
        TEntity FindById<TChild1, TChild2, TChild3>(object id,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3,
            IDbTransaction transaction = null);

        /// <summary>
        /// Get object by Id with join objects
        /// </summary>
        TEntity FindById<TChild1, TChild2, TChild3, TChild4>(object id,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3,
            Expression<Func<TEntity, object>> tChild4,
            IDbTransaction transaction = null);

        /// <summary>
        /// Get object by Id with join objects
        /// </summary>
        TEntity FindById<TChild1, TChild2, TChild3, TChild4, TChild5>(object id,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3,
            Expression<Func<TEntity, object>> tChild4,
            Expression<Func<TEntity, object>> tChild5,
            IDbTransaction transaction = null);

        /// <summary>
        /// Get object by Id with join objects
        /// </summary>
        TEntity FindById<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(object id,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3,
            Expression<Func<TEntity, object>> tChild4,
            Expression<Func<TEntity, object>> tChild5,
            Expression<Func<TEntity, object>> tChild6,
            IDbTransaction transaction = null);

        /// <summary>
        /// Get object by Id
        /// </summary>
        Task<TEntity> FindByIdAsync(object id);

        /// <summary>
        /// Get object by Id
        /// </summary>
        Task<TEntity> FindByIdAsync(object id, IDbTransaction transaction);

        /// <summary>
        /// Get object by Id with join objects
        /// </summary>
        Task<TEntity> FindByIdAsync<TChild1>(object id,
            Expression<Func<TEntity, object>> tChild1,
            IDbTransaction transaction = null);

        /// <summary>
        /// Get object by Id with join objects
        /// </summary>
        Task<TEntity> FindByIdAsync<TChild1, TChild2>(object id,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            IDbTransaction transaction = null);

        /// <summary>
        /// Get object by Id with join objects
        /// </summary>
        Task<TEntity> FindByIdAsync<TChild1, TChild2, TChild3>(object id,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3,
            IDbTransaction transaction = null);

        /// <summary>
        /// Get object by Id with join objects
        /// </summary>
        Task<TEntity> FindByIdAsync<TChild1, TChild2, TChild3, TChild4>(object id,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3,
            Expression<Func<TEntity, object>> tChild4,
            IDbTransaction transaction = null);

        /// <summary>
        /// Get object by Id with join objects
        /// </summary>
        Task<TEntity> FindByIdAsync<TChild1, TChild2, TChild3, TChild4, TChild5>(object id,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3,
            Expression<Func<TEntity, object>> tChild4,
            Expression<Func<TEntity, object>> tChild5,
            IDbTransaction transaction = null);

        /// <summary>
        /// Get object by Id with join objects
        /// </summary>
        Task<TEntity> FindByIdAsync<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(object id,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3,
            Expression<Func<TEntity, object>> tChild4,
            Expression<Func<TEntity, object>> tChild5,
            Expression<Func<TEntity, object>> tChild6,
            IDbTransaction transaction = null);


        /// <summary>
        /// Get first object
        /// </summary>
        Task<TEntity> FindAsync();

        /// <summary>
        /// Get first object
        /// </summary>
        Task<TEntity> FindAsync(IDbTransaction transaction);

        /// <summary>
        /// Get first object
        /// </summary>
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        /// Get first object
        /// </summary>
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction);

        /// <summary>
        /// Get first object with join objects
        /// </summary>
        Task<TEntity> FindAsync<TChild1>(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> tChild1,
            IDbTransaction transaction = null);

        /// <summary>
        /// Get first object with join objects
        /// </summary>
        Task<TEntity> FindAsync<TChild1, TChild2>(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            IDbTransaction transaction = null);

        /// <summary>
        /// Get first object with join objects
        /// </summary>
        Task<TEntity> FindAsync<TChild1, TChild2, TChild3>(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3,
            IDbTransaction transaction = null);

        /// <summary>
        /// Get first object with join objects
        /// </summary>
        Task<TEntity> FindAsync<TChild1, TChild2, TChild3, TChild4>(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3,
            Expression<Func<TEntity, object>> tChild4,
            IDbTransaction transaction = null);

        /// <summary>
        /// Get first object with join objects
        /// </summary>
        Task<TEntity> FindAsync<TChild1, TChild2, TChild3, TChild4, TChild5>(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3,
            Expression<Func<TEntity, object>> tChild4,
            Expression<Func<TEntity, object>> tChild5,
            IDbTransaction transaction = null);

        /// <summary>
        /// Get first object with join objects
        /// </summary>
        Task<TEntity> FindAsync<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3,
            Expression<Func<TEntity, object>> tChild4,
            Expression<Func<TEntity, object>> tChild5,
            Expression<Func<TEntity, object>> tChild6,
            IDbTransaction transaction = null);

        /// <summary>
        /// Get all objects
        /// </summary>
        IEnumerable<TEntity> FindAll(IDbTransaction transaction = null);

        /// <summary>
        /// Get all objects
        /// </summary>
        IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction = null);


        /// <summary>
        /// Get all objects with join objects
        /// </summary>
        IEnumerable<TEntity> FindAll<TChild1>(Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, object>> tChild1, IDbTransaction transaction = null);


        /// <summary>
        /// Get all objects with join objects
        /// </summary>
        IEnumerable<TEntity> FindAll<TChild1, TChild2>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            IDbTransaction transaction = null);

        /// <summary>
        /// Get all objects with join objects
        /// </summary>
        IEnumerable<TEntity> FindAll<TChild1, TChild2, TChild3>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3,
            IDbTransaction transaction = null);

        /// <summary>
        /// Get all objects with join objects
        /// </summary>
        IEnumerable<TEntity> FindAll<TChild1, TChild2, TChild3, TChild4>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3,
            Expression<Func<TEntity, object>> tChild4,
            IDbTransaction transaction = null);

        /// <summary>
        /// Get all objects with join objects
        /// </summary>
        IEnumerable<TEntity> FindAll<TChild1, TChild2, TChild3, TChild4, TChild5>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3,
            Expression<Func<TEntity, object>> tChild4,
            Expression<Func<TEntity, object>> tChild5,
            IDbTransaction transaction = null);

        /// <summary>
        /// Get all objects with join objects
        /// </summary>
        IEnumerable<TEntity> FindAll<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3,
            Expression<Func<TEntity, object>> tChild4,
            Expression<Func<TEntity, object>> tChild5,
            Expression<Func<TEntity, object>> tChild6,
            IDbTransaction transaction = null);

        /// <summary>
        /// Get all objects
        /// </summary>
        Task<IEnumerable<TEntity>> FindAllAsync(IDbTransaction transaction = null);

        /// <summary>
        /// Get all objects
        /// </summary>
        Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction = null);


        /// <summary>
        /// Get all objects with join objects
        /// </summary>
        Task<IEnumerable<TEntity>> FindAllAsync<TChild1>(Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> tChild1,
            IDbTransaction transaction = null);

        /// <summary>
        /// Get all objects with join objects
        /// </summary>
        Task<IEnumerable<TEntity>> FindAllAsync<TChild1, TChild2>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            IDbTransaction transaction = null);


        /// <summary>
        /// Get all objects with join objects
        /// </summary>
        Task<IEnumerable<TEntity>> FindAllAsync<TChild1, TChild2, TChild3>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3,
            IDbTransaction transaction = null);

        /// <summary>
        /// Get all objects with join objects
        /// </summary>
        Task<IEnumerable<TEntity>> FindAllAsync<TChild1, TChild2, TChild3, TChild4>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3,
            Expression<Func<TEntity, object>> tChild4,
            IDbTransaction transaction = null);

        /// <summary>
        /// Get all objects with join objects
        /// </summary>
        Task<IEnumerable<TEntity>> FindAllAsync<TChild1, TChild2, TChild3, TChild4, TChild5>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3,
            Expression<Func<TEntity, object>> tChild4,
            Expression<Func<TEntity, object>> tChild5,
            IDbTransaction transaction = null);

        /// <summary>
        /// Get all objects with join objects
        /// </summary>
        Task<IEnumerable<TEntity>> FindAllAsync<TChild1, TChild2, TChild3, TChild4, TChild5, TChild6>(
            Expression<Func<TEntity, bool>> predicate,
            Expression<Func<TEntity, object>> tChild1,
            Expression<Func<TEntity, object>> tChild2,
            Expression<Func<TEntity, object>> tChild3,
            Expression<Func<TEntity, object>> tChild4,
            Expression<Func<TEntity, object>> tChild5,
            Expression<Func<TEntity, object>> tChild6,
            IDbTransaction transaction = null);

        /// <summary>
        /// Insert object to DB
        /// </summary>
        OperateStatus Insert(TEntity instance);

        /// <summary>
        /// Insert object to DB
        /// </summary>
        OperateStatus Insert(TEntity instance, IDbTransaction transaction);

        /// <summary>
        /// Insert object to DB
        /// </summary>
        Task<OperateStatus> InsertAsync(TEntity instance);

        /// <summary>
        /// Insert object to DB
        /// </summary>
        Task<OperateStatus> InsertAsync(TEntity instance, IDbTransaction transaction);

        /// <summary>
        ///  使用BulkCopy进行插入
        /// </summary>
        OperateStatus BulkCopyInsert<T>(IList<T> instances) where T : new();

        /// <summary>
        /// Bulk Insert objects to DB
        /// </summary>
        OperateStatus BulkInsert(IEnumerable<TEntity> instances, IDbTransaction transaction = null);

        /// <summary>
        /// Bulk Insert objects to DB
        /// </summary>
        Task<OperateStatus> BulkInsertAsync(IEnumerable<TEntity> instances, IDbTransaction transaction = null);

        /// <summary>
        /// Delete object from DB
        /// </summary>
        OperateStatus Delete(TEntity instance, IDbTransaction transaction = null);

        /// <summary>
        /// Delete object from DB
        /// </summary>
        Task<OperateStatus> DeleteAsync(TEntity instance, IDbTransaction transaction = null);

        /// <summary>
        /// Delete objects from DB
        /// </summary>
        OperateStatus Delete(Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction = null);

        /// <summary>
        /// Delete objects from DB
        /// </summary>
        Task<OperateStatus> DeleteAsync(Expression<Func<TEntity, bool>> predicate, IDbTransaction transaction = null);


        /// <summary>
        /// Update object in DB
        /// </summary>
        OperateStatus Update(TEntity instance);

        /// <summary>
        /// Update object in DB
        /// </summary>
        OperateStatus Update(TEntity instance, IDbTransaction transaction);


        /// <summary>
        /// Update object in DB
        /// </summary>
        Task<OperateStatus> UpdateAsync(TEntity instance);

        /// <summary>
        /// Update object in DB
        /// </summary>
        Task<OperateStatus> UpdateAsync(TEntity instance, IDbTransaction transaction);

        /// <summary>
        /// Update object in DB
        /// </summary>
        OperateStatus Update(Expression<Func<TEntity, bool>> predicate, TEntity instance);


        /// <summary>
        /// Update object in DB
        /// </summary>
        OperateStatus Update(Expression<Func<TEntity, bool>> predicate, TEntity instance, IDbTransaction transaction);

        /// <summary>
        /// Update object in DB
        /// </summary>
        Task<OperateStatus> UpdateAsync(Expression<Func<TEntity, bool>> predicate, TEntity instance);

        /// <summary>
        /// Update object in DB
        /// </summary>
        Task<OperateStatus> UpdateAsync(Expression<Func<TEntity, bool>> predicate, TEntity instance, IDbTransaction transaction);


        /// <summary>
        /// Bulk Update objects in DB
        /// </summary>
        Task<OperateStatus> BulkUpdateAsync(IEnumerable<TEntity> instances);

        /// <summary>
        /// Bulk Update objects in DB
        /// </summary>
        Task<OperateStatus> BulkUpdateAsync(IEnumerable<TEntity> instances, IDbTransaction transaction);

        /// <summary>
        /// Bulk Update objects in DB
        /// </summary>
        OperateStatus BulkUpdate(IEnumerable<TEntity> instances);

        /// <summary>
        /// Bulk Update objects in DB
        /// </summary>
        OperateStatus BulkUpdate(IEnumerable<TEntity> instances, IDbTransaction transaction);


        /// <summary>
        /// Get all objects with BETWEEN query
        /// </summary>
        IEnumerable<TEntity> FindAllBetween(object from, object to, Expression<Func<TEntity, object>> btwField, IDbTransaction transaction = null);

        /// <summary>
        /// Get all objects with BETWEEN query
        /// </summary>
        IEnumerable<TEntity> FindAllBetween(object from, object to, Expression<Func<TEntity, object>> btwField, Expression<Func<TEntity, bool>> predicate = null,
            IDbTransaction transaction = null);

        /// <summary>
        /// Get all objects with BETWEEN query
        /// </summary>
        IEnumerable<TEntity> FindAllBetween(DateTime from, DateTime to, Expression<Func<TEntity, object>> btwField, IDbTransaction transaction = null);

        /// <summary>
        /// Get all objects with BETWEEN query
        /// </summary>
        IEnumerable<TEntity> FindAllBetween(DateTime from, DateTime to, Expression<Func<TEntity, object>> btwField, Expression<Func<TEntity, bool>> predicate,
            IDbTransaction transaction = null);

        /// <summary>
        /// Get all objects with BETWEEN query
        /// </summary>
        Task<IEnumerable<TEntity>> FindAllBetweenAsync(object from, object to, Expression<Func<TEntity, object>> btwField, IDbTransaction transaction = null);

        /// <summary>
        /// Get all objects with BETWEEN query
        /// </summary>
        Task<IEnumerable<TEntity>> FindAllBetweenAsync(object from, object to, Expression<Func<TEntity, object>> btwField, Expression<Func<TEntity, bool>> predicate,
            IDbTransaction transaction = null);

        /// <summary>
        /// Get all objects with BETWEEN query
        /// </summary>
        Task<IEnumerable<TEntity>> FindAllBetweenAsync(DateTime from, DateTime to, Expression<Func<TEntity, object>> btwField, IDbTransaction transaction = null);

        /// <summary>
        /// Get all objects with BETWEEN query
        /// </summary>
        Task<IEnumerable<TEntity>> FindAllBetweenAsync(DateTime from, DateTime to, Expression<Func<TEntity, object>> btwField, Expression<Func<TEntity, bool>> predicate,
            IDbTransaction transaction = null);

        /// <summary>
        /// Delete objects from DB
        /// </summary>
        Task<OperateStatus> DeleteByIdsAsync(string ids);

        /// <summary>
        /// Delete objects from DB
        /// </summary>
        Task<IEnumerable<TEntity>> FindByIdsAsync(string ids);

        /// <summary>
        /// 存储过程分页
        /// </summary>
        /// <param name="queryParam"></param>
        /// <returns></returns>
        Task<PagedResults<TEntity>> PagingQueryProcAsync(QueryParam queryParam);

        /// <summary>
        /// 单表分页
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="queryParam"></param>
        /// <returns></returns>
        Task<PagedResults<TEntity>> PagingQuerySqlSingleTableAsync(QueryParam queryParam);
    }
}