using MediatR;
using STemplate.Domain.SeedWork;
using STemplate.Persistence.Context;
namespace STemplate.Persistence.Extensions
{
    /// <summary>
    /// Clear domaint event
    /// </summary>
#nullable disable
    public static class MediatorExtension
    {
        public static async Task DispatchDomainEventsAsync(this IMediator mediator, CoreDbContext ctx)
        {
            var domainEntities = ctx.ChangeTracker
                                    .Entries<BaseEntity>()
                                    .Where(x => x.Entity.DomainEvents.Count != 0);
            var domainEvents = domainEntities.SelectMany(x => x.Entity.DomainEvents).ToList();
            domainEntities.ToList().ForEach(e => e.Entity.ClearDomainEvents());
            foreach (var domainEvent in domainEvents)
                await mediator.Publish(domainEvent);
        }
    }
}
