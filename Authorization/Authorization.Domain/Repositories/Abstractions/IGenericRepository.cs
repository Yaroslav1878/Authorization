using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;

namespace Authorization.Domain.Repositories.Abstractions;

public interface IGenericRepository<T>
    where T : class
{
    Task<List<T>> FindAll(Func<IQueryable<T>, IIncludableQueryable<T, object>> includes);
    Task<List<T>> FindAll(params Expression<Func<T, object>>[] includes);
    Task<List<T>> Find(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
    Task<List<T>> Find(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object?>> includes);
    T? FirstOrDefault(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
    T? FirstOrDefault(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object?>> includes);
    Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
    T FindFirst(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
    T FindFirst(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object?>> includes);
    ValueTask<EntityEntry<T>> InsertAsync(T entity);
    Task BulkInsertAsync(IQueryable<T> entities);
    Task BulkInsertAsync(IReadOnlyCollection<T> entities);
    EntityEntry<T> Update(T entity);
    void UpdateRange(IEnumerable<T> entity);
    EntityEntry<T> Delete(T entity);
    void DeleteRange(IEnumerable<T> entity);
    Task<List<T>> DeleteBy(Expression<Func<T, bool>> predicate);
    int Count(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes);
}
