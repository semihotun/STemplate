using Microsoft.EntityFrameworkCore;
using System.Reflection;
namespace DDDTemplateServices.Persistence.Context
{
    /// <summary>
    /// Custom db context
    /// </summary>
    public class CoreDbContext : DbContext
    {
        public const string DEFAULT_SCHEMA = "CoreDbContextSchema";
        public CoreDbContext(DbContextOptions<CoreDbContext> options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assm = Assembly.GetExecutingAssembly();
            modelBuilder.ApplyConfigurationsFromAssembly(assm);
        }
    }
}
