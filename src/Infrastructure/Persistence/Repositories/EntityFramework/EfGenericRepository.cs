using Domain.Common;
using Domain.Repositories;
using Domain.Result;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System.Linq;
using System.Linq.Expressions;

namespace Persistence.Repositories.EntityFramework;

public class EfGenericRepository<TEntity>(InslariDbContext dbContext) : IGenericRepository<TEntity>
    where TEntity : BaseEntity
{
    protected readonly InslariDbContext DbContext = dbContext;
    protected readonly DbSet<TEntity> DbSet = dbContext.Set<TEntity>();

    public TEntity? GetById(Guid id)
    {
        return DbSet.Find(id);
    }

    public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        return await DbSet.FindAsync([id], token);
    }

    public async Task<TEntity?> AddAsync(TEntity entity, CancellationToken token = default)
    {
        await DbSet.AddAsync(entity, token);
        return entity;
    }

    public TEntity? Add(TEntity entity)
    {
        DbSet.Add(entity);
        return entity;
    }

    public void Update(TEntity entity)
    {
        DbSet.Update(entity);
    }

    public void UpdateRange(IEnumerable<TEntity> entities)
    {
        DbSet.UpdateRange(entities);
    }

    public bool Delete(TEntity entity)
    {
        DbSet.Remove(entity);
        return true;
    }

    public bool Delete(Guid id)
    {
        var entity = DbSet.Find(id);
        if (entity is not null)
        {
            DbSet.Remove(entity);
            return true;
        }

        return false;
    }

    public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>>? predicate = null)
    {
        if (predicate == null) return DbSet.ToList();

        return DbSet.Where(predicate).ToList();
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken token = default)
    {
        if (predicate == null)
            return await DbSet.ToListAsync(token);

        return await DbSet.Where(predicate).ToListAsync(token);
    }

    public TEntity? Get(Expression<Func<TEntity, bool>> predicate)
    {
        return DbSet.FirstOrDefault(predicate);
    }

    public async Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default)
    {
        return await DbSet.FirstOrDefaultAsync(predicate, token);
    }

    private bool disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                DbContext.Dispose();
            }
        }
        this.disposed = true;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public async Task<bool> ExistsAsync(Guid id, CancellationToken token = default)
    {
        return await DbSet.AnyAsync(r => r.Id == id, token);
    }

    public async Task<PaginatedResult<TEntity>> GetPaginatedAsync(int page = 1, int pageSize = 10, Expression<Func<TEntity, bool>>? predicate = null)
    {
        IQueryable<TEntity> query = DbSet;

        if (predicate != null)
        {
            query = query.Where(predicate);
        }

        var totalCount = await query.CountAsync();
        var data = await query.Skip((page - 1) * pageSize)
                              .Take(pageSize)
                              .ToListAsync(); 

        return new PaginatedResult<TEntity>
        {
            Data = data,
            TotalCount = totalCount,
            PageSize = pageSize,
            CurrentPage = page
        };
    }
}
