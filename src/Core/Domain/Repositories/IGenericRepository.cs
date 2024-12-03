using Domain.Common;
using System.Linq.Expressions;

namespace Domain.Repositories;

public interface IGenericRepository<TEntity> : IDisposable where TEntity : BaseEntity
{
    IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>>? predicate = null);
    Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>>? predicate = null, CancellationToken token = default);
    TEntity? Get(Expression<Func<TEntity, bool>> predicate);
    Task<TEntity?> GetAsync(Expression<Func<TEntity, bool>> predicate, CancellationToken token = default);
    TEntity? GetById(Guid id);
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken token = default);
    bool Delete(Guid id);
    bool Delete(TEntity entity);
    TEntity? Add(TEntity entity);
    Task<TEntity?> AddAsync(TEntity entity, CancellationToken token = default);
    void Update(TEntity entity);
    void UpdateRange(IEnumerable<TEntity> entities);
    Task<bool> ExistsAsync(Guid id, CancellationToken token = default);
}
