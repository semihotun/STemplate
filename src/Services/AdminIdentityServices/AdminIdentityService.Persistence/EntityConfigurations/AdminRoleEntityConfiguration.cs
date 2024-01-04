using AdminIdentityService.Domain.AggregateModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace AdminIdentityService.Persistence.EntityConfigurations
{
    public class AdminRoleEntityConfiguration : IEntityTypeConfiguration<AdminRole>
    {
        public void Configure(EntityTypeBuilder<AdminRole> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Role).IsRequired();
        }
    }
}
