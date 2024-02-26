using AdminIdentityService.Insfrastructure.Utilities.Outboxes;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AdminIdentityService.Persistence.EntityConfigurations;

public class OutboxEntityConfiguration : IEntityTypeConfiguration<Outbox>
{
    public void Configure(EntityTypeBuilder<Outbox> builder)
    {
        builder.HasKey(x => x.Id);
    }
}
