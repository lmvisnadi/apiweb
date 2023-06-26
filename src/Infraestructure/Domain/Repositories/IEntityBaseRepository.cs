using Infraestructure.Domain.Base;
using System.Linq.Expressions;

namespace Infraestructure.Domain.Repositories
{
    public interface IEntityBaseRepository<TEntity>
        where TEntity : EntityBase
    {
        Task<TEntity?> GetAsync(Guid id);
        Task InsertAsync(TEntity item);
        Task InsertRangeAsync(IEnumerable<TEntity> items);
        void Update(TEntity item);
        void UpdateRange(IEnumerable<TEntity> entities);
        Task<TEntity> InsertOrUpdateAsync(TEntity company);
        Task SaveAsync(CancellationToken cancellationToken = default);
        Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null);
        Task InsertOrUpdateRangeAsync(IEnumerable<TEntity> items);
        Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>>? predicate = null);
        void Remove(TEntity item);
        Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>>? predicate = null);
        IQueryable<TEntity> GetAll();
        void RemoveRange(IEnumerable<TEntity> items);
    }
}
