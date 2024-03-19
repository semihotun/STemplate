using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BasketService.Persistence.Context
{
    public interface ICoreDbContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        IQueryable<TEntity> Query<TEntity>() where TEntity : class;
        DatabaseFacade Database { get; }
        ChangeTracker ChangeTracker { get; }
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
