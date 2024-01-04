using AdminIdentityService.Domain.AggregateModels;
using AdminIdentityService.Persistence.Extensions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
namespace AdminIdentityService.Persistence.Context
{
    /// <summary>
    /// Custom db context
    /// </summary>
    public class CoreDbContext : DbContext, ICoreDbContext
    {
        public const string DEFAULT_SCHEMA = "CoreDbContextSchema";
        public CoreDbContext(DbContextOptions<CoreDbContext> options, IMediator? mediator) : base(options)
        {
            _mediator = mediator;
        }
        public DbSet<AdminUser> AdminUser { get; set; }
        public DbSet<AdminRole> AdminRole { get; set; }
        public DbSet<AdminUserRole> AdminUserRole { get; set; }
        private IMediator? _mediator { get; set; }
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
                    using (var tx = Context.Database.BeginTransaction())
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
            catch (Exception ex)
            {
                if (exceptionAction == null)
                    throw;
                else
                    exceptionAction(ex);
            }
            return result;
        }
    }
}
