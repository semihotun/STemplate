using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NotificationService.Insfrastructure.Utilities.Outboxes;

namespace NotificationService.Persistence.EntityConfigurations
{
    public class OutboxEntityConfiguration : IEntityTypeConfiguration<Outbox>
    {
        public void Configure(EntityTypeBuilder<Outbox> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
