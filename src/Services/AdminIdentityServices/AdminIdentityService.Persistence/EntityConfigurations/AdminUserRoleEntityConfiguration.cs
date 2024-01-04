using AdminIdentityService.Domain.AggregateModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace AdminIdentityService.Persistence.EntityConfigurations
{
    public class AdminUserRoleEntityConfiguration : IEntityTypeConfiguration<AdminUserRole>
    {
        public void Configure(EntityTypeBuilder<AdminUserRole> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.AdminRoleId).IsRequired();
            builder.Property(x => x.AdminUserId).IsRequired();
        }
    }
}
