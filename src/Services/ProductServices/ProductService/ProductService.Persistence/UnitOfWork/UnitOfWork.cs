using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Domain.Result;
using ProductService.Domain.SeedWork;
using ProductService.Insfrastructure.Utilities.Outboxes;
using ProductService.Persistence.Context;

namespace ProductService.Persistence.UnitOfWork
{
#nullable disable
    public class UnitOfWork(ICoreDbContext ctx, IMediator mediator) : IUnitOfWork
    {
        private readonly ICoreDbContext _ctx = ctx;
        private readonly IMediator _mediator = mediator;
        public async Task<T> BeginTransaction<T>(Func<Task<T>> action)
        where T : Result
        {
            var result = default(T);
            await _ctx.Database.CreateExecutionStrategy().ExecuteAsync(async () =>
            {
                await using var tx = _ctx.Database.BeginTransaction();
                try
                {
                    result = await action();
                    if (!result.Success) return;
                    await DispatchDomainEventsAsync();
                    await _ctx.SaveChangesAsync();
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                    throw new Exception("TransactionError", ex);
                }
            });
            return result;
        }
        public async Task<T> BeginTransactionAndCreateOutbox<T>(Func<Action<IOutboxMessage>, Task<T>> action)
           where T : Result
        {
            var result = default(T);
            await _ctx.Database.CreateExecutionStrategy().ExecuteAsync(async () =>
            {
                await using var tx = _ctx.Database.BeginTransaction();
                try
                {
                    IOutboxMessage message = null;
                    result = await action.Invoke(msg => message = msg);
                    if (!result.Success) return;
                    await DispatchDomainEventsOutboxAsync(message);
                    await _ctx.SaveChangesAsync();
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                    throw new Exception("TransactionError", ex);
                }
            });
            return result;
        }
        public async Task BeginTransactionAndCreateOutbox(Func<Action<IOutboxMessage>, Task> action)
        {
            await _ctx.Database.CreateExecutionStrategy().ExecuteAsync(async () =>
            {
                await using var tx = _ctx.Database.BeginTransaction();
                try
                {
                    IOutboxMessage message = null;
                    await action.Invoke(msg => message = msg);
                    await DispatchDomainEventsOutboxAsync(message);
                    await _ctx.SaveChangesAsync();
                    tx.Commit();
                }
                catch (Exception ex)
                {
                    tx.Rollback();
                    throw new Exception("TransactionError", ex);
                }
            });
        }
        public async Task DispatchDomainEventsOutboxAsync(IOutboxMessage message)
        {
            var outbox = new Outbox();
            outbox.InitOutbox(message);
            var domainEntities = _ctx.ChangeTracker
                            .Entries<BaseEntity>()
                            .Where(x => x.Entity.DomainEvents.Count != 0);
            var domainEvents = domainEntities.SelectMany(x => x.Entity.DomainEvents).ToList();
            domainEntities.ToList().ForEach(e => e.Entity.ClearDomainEvents());
            foreach (var domainEvent in domainEvents)
            {
                var domainEventData = await _mediator.Send(domainEvent);
                outbox.AddDomainEventDictionary(domainEventData?.GetType()?.Name ?? domainEvent.GetType().Name, domainEventData);
            }
            outbox.DomainEventDictionaryToContent();
            await _ctx.Set<Outbox>().AddAsync(outbox);
        }
        private async Task DispatchDomainEventsAsync()
        {
            var domainEntities = _ctx.ChangeTracker
                                    .Entries<BaseEntity>()
                                    .Where(x => x.Entity.DomainEvents.Count != 0);
            var domainEvents = domainEntities.SelectMany(x => x.Entity.DomainEvents).ToList();
            domainEntities.ToList().ForEach(e => e.Entity.ClearDomainEvents());
            foreach (var domainEvent in domainEvents)
                await _mediator.Send(domainEvent);
        }
    }
}
