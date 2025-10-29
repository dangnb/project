﻿using System.Linq.Expressions;
using MNG.Domain.Abstractions.Repositories;
using Microsoft.EntityFrameworkCore;
using MNG.Domain.Abstractions;

namespace MNG.Persistence.Repositoty;
public class RepositoryBaseDbContext<TContext, TEntity, TKey> : IRepositoryBaseDBContext<TContext, TEntity, TKey>, IDisposable
        where TEntity : EntityBase<TKey>
        where TContext : DbContext
        where TKey : struct
{

    private readonly TContext _context;

    public RepositoryBaseDbContext(TContext context)
        => _context = context;

    public void Dispose()
    {
        _context?.Dispose();
    }

    public IQueryable<TEntity> FindAll(
        Expression<Func<TEntity, bool>>? predicate = null,
        params Expression<Func<TEntity, object>>[] includeProperties)
    {
        IQueryable<TEntity> items = _context.Set<TEntity>().AsNoTracking(); // Importance Always include AsNoTracking for Query Side
        if (includeProperties != null)
        {
            foreach (var includeProperty in includeProperties)
            {
                items = items.Include(includeProperty);
            }
        }

        if (predicate is not null)
        {
            items = items.Where(predicate);
        }

        return items;
    }

    public async Task<TEntity> FindByIdAsync(TKey id, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includeProperties) => await FindAll(null, includeProperties).AsTracking().SingleOrDefaultAsync(x => x.Id.Equals(id), cancellationToken);

    public async Task<TEntity> FindSingleAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken cancellationToken = default, params Expression<Func<TEntity, object>>[] includeProperties)
        => await FindAll(null, includeProperties).AsTracking().SingleOrDefaultAsync(predicate, cancellationToken);

    public void Add(TEntity entity)
        => _context.Add(entity);

    public void Remove(TEntity entity)
        => _context.Set<TEntity>().Remove(entity);

    public void RemoveMultiple(List<TEntity> entities)
        => _context.Set<TEntity>().RemoveRange(entities);

    public void Update(TEntity entity)
        => _context.Set<TEntity>().Update(entity);
}
