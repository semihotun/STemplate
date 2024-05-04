using Microsoft.EntityFrameworkCore;
using STemplate.Domain.SeedWork;
using System.Reflection;
namespace STemplate.Persistence.Context;

/// <summary>
/// Custom db context
/// </summary>
public class CoreDbContext(DbContextOptions<CoreDbContext> options) : DbContext(options), ICoreDbContext
{
    public const string DEFAULT_SCHEMA = "CoreDbContextSchema";
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
    public IQueryable<TEntity> Query<TEntity>() where TEntity : BaseEntity
    {
        return Set<TEntity>().AsQueryable().Where(x => !x.Deleted);
    }
}
