using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SearchBarService.Insfrastructure.Utilities.Outboxes;

namespace SearchBarService.Persistence.EntityConfigurations
{
    public class OutboxEntityConfiguration : IEntityTypeConfiguration<Outbox>
    {
        public void Configure(EntityTypeBuilder<Outbox> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
