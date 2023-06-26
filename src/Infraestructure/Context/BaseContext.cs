using Infraestructure.Domain.Base;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Infraestructure.Extensions;

namespace Infraestructure.Context
{
    public abstract class BaseContext : DbContext
    {
        protected Guid _userId;

        public BaseContext(DbContextOptions<BaseContext> options, Guid userId) : base(options)
        {
            _userId = userId;
        }
        private IEnumerable<EntityBase> GetAddedEntities()
        {
            return from e in ChangeTracker.Entries()
                   where e.Entity is EntityBase && e.State == EntityState.Added
                   select ((EntityBase)e.Entity);
        }
        private IEnumerable<EntityBase> GetUpdatedEntities()
        {
            return from e in ChangeTracker.Entries()
                   where e.Entity is EntityBase && e.State == EntityState.Modified
                   select ((EntityBase)e.Entity);
        }
        protected virtual void SetInfoAdded()
        {
            GetAddedEntities()
                .ForEach(entity =>
                {
                    entity.CreatedAt = DateTime.UtcNow;
                    entity.CreatedBy = _userId;
                });
        }
        protected virtual void SetAuditInfoUpdated()
        {
            GetUpdatedEntities()
                .ForEach(entity =>
                {
                    entity.UpdatedAt = DateTime.UtcNow;
                    entity.UpdatedBy = _userId;
                });
        }

        public override int SaveChanges()
        {
            SetInfoAdded();
            SetAuditInfoUpdated();
            return base.SaveChanges();
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            SetInfoAdded();
            SetAuditInfoUpdated();
            return base.SaveChangesAsync(cancellationToken);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var type in modelBuilder.Model.GetEntityTypes().Where(e => e.ClrType.IsSubclassOf(typeof(EntityBase)) && e.BaseType == null))
            {
                var method = SetGlobalQueryMethod.MakeGenericMethod(type.ClrType);
                method.Invoke(this, new object[] { modelBuilder });
            }

            base.OnModelCreating(modelBuilder);
        }

        static readonly MethodInfo SetGlobalQueryMethod = typeof(BaseContext)
            .GetMethods(BindingFlags.Public | BindingFlags.Instance)
            .Single(t => t.IsGenericMethod && t.Name == "SetGlobalQuery");

        public virtual void SetGlobalQuery<T>(ModelBuilder builder) where T : EntityBase
        {
            builder
                .Entity<T>();
        }
        public void SetUserId(Guid userId)
            => _userId = userId;

    }
}
