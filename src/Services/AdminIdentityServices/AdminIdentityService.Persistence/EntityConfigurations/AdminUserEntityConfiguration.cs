using AdminIdentityService.Domain.AggregateModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
namespace AdminIdentityService.Persistence.EntityConfigurations
{
    public class AdminUserEntityConfiguration : IEntityTypeConfiguration<AdminUser>
    {
        public void Configure(EntityTypeBuilder<AdminUser> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Email).IsRequired();
            builder.Ignore(x => x.DomainEvents);
            builder.Ignore(x => x.CreateDate);
        }
    }
}
