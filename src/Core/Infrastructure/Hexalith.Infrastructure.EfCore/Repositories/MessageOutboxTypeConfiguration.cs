namespace Hexalith.Infrastructure.EfCore.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class MessageOutboxTypeConfiguration : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage> builder)
        {
            builder.HasIndex(i => i.MessageId).IsUnique();
            builder.HasIndex(i => i.SentUtcDateTime);
            builder.HasIndex(i => i.SessionId);
        }
    }
}