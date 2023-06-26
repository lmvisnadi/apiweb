using Infraestructure.Domain.Base;
using Infraestructure.Domain.Repositories;
using Infraestructure.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infraestructure.Repository
{
    public abstract class EntityBaseRepository<TEntity> : IEntityBaseRepository<TEntity>
        where TEntity : EntityBase
    {
        protected readonly DbContext _context;
        protected DbSet<TEntity> DbSet => _context.Set<TEntity>();

        protected EntityBaseRepository(
            DbContext context
        )
        {
            _context = context;
        }

        public virtual Task<bool> AnyAsync(Expression<Func<TEntity, bool>>? predicate = null)
            => ConfigureDefaultIncludes(Filter(predicate))
                .AnyAsync();

        public virtual async Task<TEntity?> FindAsync(Expression<Func<TEntity, bool>>? predicate = null)
            => await ConfigureDefaultIncludes(Filter(predicate))
                .FirstOrDefaultAsync();

        public IQueryable<TEntity> GetAll()
            => ConfigureDefaultIncludes(DbSet);

        public virtual async Task<TEntity?> GetAsync(Guid id)
            => await ConfigureDefaultIncludes(FilterById(id))
                .FirstOrDefaultAsync();

        public virtual Task<List<TEntity>> ListAsync(Expression<Func<TEntity, bool>>? predicate = null)
            => ConfigureDefaultIncludes(Filter(predicate))
                .ToListAsync();

        public virtual async Task InsertAsync(TEntity item)
            => await _context.AddAsync(item);

        public async Task InsertRangeAsync(IEnumerable<TEntity> items)
            => await DbSet.AddRangeAsync(items);

        public virtual void Update(TEntity item)
            => _context.Update(item);

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
                Update(entity);
        }

        public virtual async Task<TEntity> InsertOrUpdateAsync(TEntity entity)
        {
            var isUpdating = await AnyAsync(e => e.Id == entity.Id);

            if (isUpdating)
                Update(entity);
            else
                await InsertAsync(entity);

            return entity;
        }

        public virtual async Task SaveAsync(CancellationToken cancellationToken = default)
            => await _context.SaveChangesAsync(cancellationToken);

        public virtual async Task InsertOrUpdateRangeAsync(IEnumerable<TEntity> items)
        {
            foreach (var item in items)
            {
                await InsertOrUpdateAsync(item);
            }
        }

        public virtual void Remove(TEntity item)
                    => _context.Remove(item);

        public virtual void RemoveRange(IEnumerable<TEntity> items)
            => items.ForEach(i => { Remove(i); });

        protected virtual IQueryable<TEntity> ConfigureDefaultIncludes(
            IQueryable<TEntity> query
        )
            => query;

        private IQueryable<TEntity> Filter(
            Expression<Func<TEntity, bool>>? predicate = null
        )
            => predicate == null ? DbSet : DbSet.Where(predicate);

        private IQueryable<TEntity> FilterById(Guid id)
            => Filter(entity => entity.Id == id);

    }
}
