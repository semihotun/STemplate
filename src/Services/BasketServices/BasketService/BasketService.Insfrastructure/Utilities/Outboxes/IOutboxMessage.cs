using BasketService.Insfrastructure.Utilities.ServiceBus;

namespace BasketService.Insfrastructure.Utilities.Outboxes
{
    public interface IOutboxMessage : IMessage
    {
        public Guid EventId { get; set; }
        public OutboxState State { get; set; }
    }
}
