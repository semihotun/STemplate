using MediatR;
using Microsoft.EntityFrameworkCore;
using STemplate.Persistence.Extensions;
using System.Reflection;
namespace STemplate.Persistence.Context
{
    /// <summary>
    /// Custom db context
    /// </summary>
    public class CoreDbContext(DbContextOptions<CoreDbContext> options, IMediator? mediator) : DbContext(options), ICoreDbContext
    {
        public const string DEFAULT_SCHEMA = "CoreDbContextSchema";
        private readonly IMediator? _mediator = mediator;
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assm = Assembly.GetExecutingAssembly();
            modelBuilder.ApplyConfigurationsFromAssembly(assm);
        }
        public IQueryable<TEntity> Query<TEntity>() where TEntity : class
        {
            return Set<TEntity>().AsQueryable();
        }
#nullable disable
        public async Task<TResult> BeginTransaction<TResult>(Func<Task<TResult>> action, Action successAction = null, Action<Exception> exceptionAction = null)
        {
            var result = default(TResult);
            var Context = this;
            var strategy = Context.Database.CreateExecutionStrategy();
            try
            {
                await strategy.ExecuteAsync(async () =>
                {
                    await using (var tx = Context.Database.BeginTransaction())
                    {
                        try
                        {
                            result = await action();
                            await _mediator.DispatchDomainEventsAsync(Context);
                            await Context.SaveChangesAsync();
                            tx.Commit();
                        }
                        catch (Exception)
                        {
                            tx.Rollback();
                            throw;
                        }
                    }
                    successAction?.Invoke();
                });
            }
            catch (Exception ex) when (exceptionAction != null)
            {
                exceptionAction(ex);
            }
            return result;
        }
    }
}
