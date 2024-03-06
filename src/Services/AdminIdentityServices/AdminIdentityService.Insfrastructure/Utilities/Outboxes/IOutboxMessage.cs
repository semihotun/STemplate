using AdminIdentityService.Insfrastructure.Utilities.ServiceBus;

namespace AdminIdentityService.Insfrastructure.Utilities.Outboxes;

public interface IOutboxMessage : IMessage
{
    public Guid EventId { get; set; }
    public OutboxState State { get; set; }
}
