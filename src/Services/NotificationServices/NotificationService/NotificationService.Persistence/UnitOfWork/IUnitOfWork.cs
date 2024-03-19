using NotificationService.Domain.Result;
using NotificationService.Insfrastructure.Utilities.Outboxes;

namespace NotificationService.Persistence.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task<T> BeginTransaction<T>(Func<Task<T>> action)
        where T : Result;
        Task<T> BeginTransactionAndCreateOutbox<T>(Func<Action<IOutboxMessage>, Task<T>> action)
           where T : Result;
        Task BeginTransactionAndCreateOutbox(Func<Action<IOutboxMessage>, Task> action);
        Task DispatchDomainEventsOutboxAsync(IOutboxMessage message);
    }
}
