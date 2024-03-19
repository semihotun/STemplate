using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderService.Insfrastructure.Utilities.Outboxes;

namespace OrderService.Persistence.EntityConfigurations
{
    public class OutboxEntityConfiguration : IEntityTypeConfiguration<Outbox>
    {
        public void Configure(EntityTypeBuilder<Outbox> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
