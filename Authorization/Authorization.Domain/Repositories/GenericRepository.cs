using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Authorization.Domain.Exceptions;
using Authorization.Domain.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Query;

namespace Authorization.Domain.Repositories;

public abstract class GenericRepository<T>(DbSet<T> dbSet) : IGenericRepository<T>
    where T : class
{
    public Task<List<T>> FindAll(Func<IQueryable<T>, IIncludableQueryable<T, object>> includes)
    {
        try
        {
            IQueryable<T> query = dbSet;

            query = includes(query);

            return query.ToListAsync();
        }
        catch (Exception)
        {
            var tableName = typeof(T).Name;
            throw new EntityNotFoundException(tableName);
        }
    }

    public Task<List<T>> FindAll(params Expression<Func<T, object>>[] includes)
    {
        try
        {
            IQueryable<T> query = dbSet.AsNoTracking();

            if (includes.Any())
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return query.ToListAsync();
        }
        catch (Exception)
        {
            var tableName = typeof(T).Name;
            throw new EntityNotFoundException(tableName);
        }
    }

    public Task<List<T>> Find(
        Expression<Func<T, bool>> predicate,
        params Expression<Func<T, object>>[] includes)
    {
        var query = dbSet.Where(predicate);

        if (includes.Any())
        {
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
        }

        return query.ToListAsync();
    }

    public Task<List<T>> Find(
        Expression<Func<T, bool>> predicate,
        Func<IQueryable<T>, IIncludableQueryable<T, object?>> includes)
    {
        IQueryable<T> query = dbSet;

        query = includes(query);

        return query.Where(predicate).ToListAsync();
    }

    public T? FirstOrDefault(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
    {
        try
        {
            IQueryable<T> query = dbSet;

            if (includes.Any())
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return query.FirstOrDefault(predicate);
        }
        catch (Exception)
        {
            var tableName = typeof(T).Name;
            throw new EntityNotFoundException(tableName);
        }
    }

    public T? FirstOrDefault(Expression<Func<T, bool>> predicate, Func<IQueryable<T>, IIncludableQueryable<T, object?>> includes)
    {
        IQueryable<T> query = dbSet;

        query = includes(query);

        return query.FirstOrDefault(predicate);
    }

    public Task<T?> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
    {
        try
        {
            IQueryable<T> query = dbSet;

            if (includes.Any())
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return query.FirstOrDefaultAsync(predicate);
        }
        catch (Exception)
        {
            throw new EntityNotFoundException(typeof(T).Name);
        }
    }

    public T FindFirst(
        Expression<Func<T, bool>> predicate,
        params Expression<Func<T, object>>[] includes)
    {
        try
        {
            IQueryable<T> query = dbSet;

            if (includes.Any())
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return query.First(predicate);
        }
        catch (Exception)
        {
            throw new EntityNotFoundException(typeof(T).Name);
        }
    }

    public T FindFirst(
        Expression<Func<T, bool>> predicate,
        Func<IQueryable<T>, IIncludableQueryable<T, object?>> include)
    {
        try
        {
            IQueryable<T> query = dbSet;

            query = include(query);

            return query.First(predicate);
        }
        catch (Exception)
        {
            throw new EntityNotFoundException(typeof(T).Name);
        }
    }

    public ValueTask<EntityEntry<T>> InsertAsync(T entity)
    {
        return dbSet.AddAsync(entity);
    }

    public Task BulkInsertAsync(IQueryable<T> entities)
    {
        return dbSet.AddRangeAsync(entities);
    }

    public Task BulkInsertAsync(IReadOnlyCollection<T> entities)
    {
        return dbSet.AddRangeAsync(entities);
    }

    public EntityEntry<T> Update(T entity)
    {
        return dbSet.Update(entity);
    }

    public void UpdateRange(IEnumerable<T> entity)
    {
        dbSet.UpdateRange(entity);
    }

    public EntityEntry<T> Delete(T entity)
    {
        return dbSet.Remove(entity);
    }

    public void DeleteRange(IEnumerable<T> entity)
    {
        dbSet.RemoveRange(entity);
    }

    public async Task<List<T>> DeleteBy(Expression<Func<T, bool>> predicate)
    {
        List<T> entitiesToDelete = await Find(predicate);

        dbSet.RemoveRange(entitiesToDelete);

        return entitiesToDelete;
    }

    public int Count(Expression<Func<T, bool>> predicate, params Expression<Func<T, object>>[] includes)
    {
        try
        {
            IQueryable<T> query = dbSet.AsNoTracking();

            if (includes.Any())
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }

            return query.Count(predicate);
        }
        catch (Exception)
        {
            throw new EntityNotFoundException(typeof(T).Name);
        }
    }
}