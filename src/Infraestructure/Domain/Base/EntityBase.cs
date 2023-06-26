namespace Infraestructure.Domain.Base
{
    public abstract class EntityBase : IEquatable<EntityBase>
    {
        public Guid Id { get; protected set; }
        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public Guid? DeletedBy { get; set; }
        public virtual bool Equals(EntityBase? other)
            => other == null ? false
            : this.Id == other.Id;
    }
}
