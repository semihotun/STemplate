using AdminIdentityService.Domain.Result;
using AdminIdentityService.Domain.SeedWork;
using AdminIdentityService.Insfrastructure.Utilities.Outboxes;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
namespace AdminIdentityService.Persistence.Context;

/// <summary>
/// Custom db context
/// </summary>
public class CoreDbContext(DbContextOptions<CoreDbContext> options, IMediator? mediator) : DbContext(options), ICoreDbContext
{
    public const string DEFAULT_SCHEMA = "CoreDbContextSchema";
    private IMediator? Mediator { get; } = mediator;
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
    public IQueryable<TEntity> Query<TEntity>() where TEntity : class
    {
        return Set<TEntity>().AsQueryable();
    }
#nullable disable
    public async Task<T> BeginTransaction<T>(Func<Task<T>> action)
        where T : Result
    {
        var result = default(T);
        var strategy = this.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await using var tx = this.Database.BeginTransaction();
            try
            {
                result = await action();
                if (!result.Success) return;
                await DispatchDomainEventsAsync();
                await this.SaveChangesAsync();
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
    public async Task<T> BeginTransactionNotDispatcher<T>(Func<Task<T>> action)
    where T : Result
    {
        var result = default(T);
        var strategy = this.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await using var tx = this.Database.BeginTransaction();
            try
            {
                result = await action();
                if (!result.Success) return;
                await this.SaveChangesAsync();
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
        var strategy = this.Database.CreateExecutionStrategy();
        await strategy.ExecuteAsync(async () =>
        {
            await using var tx = this.Database.BeginTransaction();
            try
            {
                IOutboxMessage message = null;
                result = await action.Invoke(msg => message = msg);
                if (!result.Success) return;
                await DispatchDomainEventsOutboxAsync(message);
                await this.SaveChangesAsync();
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
    public async Task DispatchDomainEventsOutboxAsync(IOutboxMessage message)
    {
        var outbox = new Outbox();
        outbox.InitOutbox(message);
        var domainEntities = this.ChangeTracker
                        .Entries<BaseEntity>()
                        .Where(x => x.Entity.DomainEvents.Count != 0);
        var domainEvents = domainEntities.SelectMany(x => x.Entity.DomainEvents).ToList();
        domainEntities.ToList().ForEach(e => e.Entity.ClearDomainEvents());
        foreach (var domainEvent in domainEvents)
        {
            var domainEventData = await Mediator.Send(domainEvent);
            outbox.AddDomainEventDictionary(domainEventData?.GetType()?.Name ?? domainEvent.GetType().Name, domainEventData);
        }
        outbox.DomainEventDictionaryToContent();
        await this.Set<Outbox>().AddAsync(outbox);
    }
    private async Task DispatchDomainEventsAsync()
    {
        var domainEntities = this.ChangeTracker
                                .Entries<BaseEntity>()
                                .Where(x => x.Entity.DomainEvents.Count != 0);
        var domainEvents = domainEntities.SelectMany(x => x.Entity.DomainEvents).ToList();
        domainEntities.ToList().ForEach(e => e.Entity.ClearDomainEvents());
        foreach (var domainEvent in domainEvents)
            await Mediator.Send(domainEvent);
    }
}