using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ShippingService.Insfrastructure.Utilities.Outboxes;

namespace ShippingService.Persistence.EntityConfigurations
{
    public class OutboxEntityConfiguration : IEntityTypeConfiguration<Outbox>
    {
        public void Configure(EntityTypeBuilder<Outbox> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
