using Microsoft.EntityFrameworkCore;
using STemplate.Domain.SeedWork;
using System.Reflection;

namespace STemplate.Persistence.Context
{
    public class CoreDbReadContext : DbContext
    {
        public CoreDbReadContext(DbContextOptions<CoreDbReadContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
        public IQueryable<TEntity> Query<TEntity>() where TEntity : BaseEntity
        {
            return Set<TEntity>().AsQueryable().Where(x => !x.Deleted);
        }
    }
}
