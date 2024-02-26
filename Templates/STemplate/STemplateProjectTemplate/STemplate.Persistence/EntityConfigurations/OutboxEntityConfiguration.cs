using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using STemplate.Insfrastructure.Utilities.Outboxes;

namespace STemplate.Persistence.EntityConfigurations;

public class OutboxEntityConfiguration : IEntityTypeConfiguration<Outbox>
{
    public void Configure(EntityTypeBuilder<Outbox> builder)
    {
        builder.HasKey(x => x.Id);
    }
}
