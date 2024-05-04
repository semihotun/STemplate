using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using STemplate.Domain.SeedWork;
namespace STemplate.Persistence.Context;

public interface ICoreDbContext
{
    DbSet<TEntity> Set<TEntity>() where TEntity : class;
    IQueryable<TEntity> Query<TEntity>() where TEntity : BaseEntity;
    DatabaseFacade Database { get; }
    ChangeTracker ChangeTracker { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
