using AdminIdentityService.Domain.Result;
using AdminIdentityService.Insfrastructure.Utilities.Outboxes;
using Microsoft.EntityFrameworkCore;
namespace AdminIdentityService.Persistence.Context;

public interface ICoreDbContext
{
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
    IQueryable<TEntity> Query<TEntity>() where TEntity : class;
    Task<T> BeginTransaction<T>(Func<Task<T>> action)
        where T : Result;
    Task<T> BeginTransactionAndCreateOutbox<T>(Func<Action<IOutboxMessage>,Task<T>> action)
       where T : Result;
    Task<T> BeginTransactionNotDispatcher<T>(Func<Task<T>> action)
        where T : Result;
    Task DispatchDomainEventsOutboxAsync(IOutboxMessage message);
}
