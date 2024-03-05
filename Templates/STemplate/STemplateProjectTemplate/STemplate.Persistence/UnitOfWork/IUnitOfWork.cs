using STemplate.Domain.Result;
using STemplate.Insfrastructure.Utilities.Outboxes;

namespace STemplate.Persistence.UnitOfWork;

public interface IUnitOfWork
{
    Task<T> BeginTransaction<T>(Func<Task<T>> action)
    where T : Result;
    Task<T> BeginTransactionAndCreateOutbox<T>(Func<Action<IOutboxMessage>, Task<T>> action)
       where T : Result;
    Task BeginTransactionAndCreateOutbox(Func<Action<IOutboxMessage>, Task> action);
    Task DispatchDomainEventsOutboxAsync(IOutboxMessage message);
}
