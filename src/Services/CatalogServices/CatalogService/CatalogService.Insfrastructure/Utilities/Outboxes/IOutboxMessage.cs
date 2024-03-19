using CatalogService.Insfrastructure.Utilities.ServiceBus;

namespace CatalogService.Insfrastructure.Utilities.Outboxes
{
    public interface IOutboxMessage : IMessage
    {
        public Guid EventId { get; set; }
        public OutboxState State { get; set; }
    }
}
