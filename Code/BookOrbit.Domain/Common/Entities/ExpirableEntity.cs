namespace BookOrbit.Domain.Common.Entities;

    public abstract class ExpirableEntity : Entity
    {
        protected ExpirableEntity()
        { }
        protected ExpirableEntity(Guid id)
            : base(id)
        {
        }
        public DateTimeOffset? ExpireAtUtc { get; set; }
    }

