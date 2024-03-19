using ProductService.Insfrastructure.Utilities.ServiceBus;

namespace ProductService.Insfrastructure.Utilities.Outboxes
{
    public interface IOutboxMessage : IMessage
    {
        public Guid EventId { get; set; }
        public OutboxState State { get; set; }
    }
}
