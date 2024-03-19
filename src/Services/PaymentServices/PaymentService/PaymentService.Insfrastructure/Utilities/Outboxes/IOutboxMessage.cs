using PaymentService.Insfrastructure.Utilities.ServiceBus;

namespace PaymentService.Insfrastructure.Utilities.Outboxes
{
    public interface IOutboxMessage : IMessage
    {
        public Guid EventId { get; set; }
        public OutboxState State { get; set; }
    }
}
