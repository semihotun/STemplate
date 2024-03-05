using AdminIdentityService.Domain.Result;
using AdminIdentityService.Insfrastructure.Utilities.Outboxes;

namespace AdminIdentityService.Persistence.UnitOfWork;

public interface IUnitOfWork
{
    Task<T> BeginTransaction<T>(Func<Task<T>> action)
    where T : Result;
    Task<T> BeginTransactionAndCreateOutbox<T>(Func<Action<IOutboxMessage>, Task<T>> action)
       where T : Result;
    Task BeginTransactionAndCreateOutbox(Func<Action<IOutboxMessage>, Task> action);
    Task DispatchDomainEventsOutboxAsync(IOutboxMessage message);
}
