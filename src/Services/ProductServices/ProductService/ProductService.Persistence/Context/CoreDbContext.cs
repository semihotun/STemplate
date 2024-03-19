using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ProductService.Persistence.Context
{
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
        public IQueryable<TEntity> Query<TEntity>() where TEntity : class
        {
            return Set<TEntity>().AsQueryable();
        }
    }
}
