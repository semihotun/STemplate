using MediatR;
namespace AdminIdentityService.Domain.SeedWork
{
    /// <summary>
    /// Base Entity
    /// </summary>
    public abstract class BaseEntity : IEntity
    {
        public virtual Guid Id { get; protected set; }
        public bool Deleted { get; set; }
        public DateTime CreateDate { get; set; }
        int? _requestedHashCode;
        private List<INotification>? domainEvents;
        public IReadOnlyCollection<INotification>? DomainEvents => domainEvents?.AsReadOnly();
        public void AddDomainEvent(INotification eventItem)
        {
            domainEvents ??= [];
            domainEvents.Add(eventItem);
        }
        public void RemoveDomainEvents(INotification eventItem)
        {
            domainEvents?.Remove(eventItem);
        }
        public void ClearDomainEvents()
        {
            domainEvents?.Clear();
        }
        public bool IsTransient()
        {
            return Id == default;
        }
        public override bool Equals(object? obj)
        {
            if (obj == null || obj is not BaseEntity)
                return false;
            if (Object.ReferenceEquals(this, obj))
                return true;
            if (this.GetType() != obj.GetType())
                return false;
            BaseEntity item = (BaseEntity)obj;
            if (item.IsTransient() || this.IsTransient())
                return false;
            else
                return item.Id == this.Id;
        }
        public override int GetHashCode()
        {
            if (!IsTransient())
            {
                if (!_requestedHashCode.HasValue)
                    _requestedHashCode = this.Id.GetHashCode() ^ 31;
                return _requestedHashCode.Value;
            }
            else
            {
                return base.GetHashCode();
            }
        }
        public static bool operator ==(BaseEntity left, BaseEntity right)
        {
            if (Object.Equals(left, null))
                return Equals(right, null);
            else
                return left.Equals(right);
        }
        public static bool operator !=(BaseEntity left, BaseEntity right)
        {
            return !(left == right);
        }
    }
}
