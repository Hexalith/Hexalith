namespace Hexalith.Infrastructure.EfCore.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class StateStreamItemTypeConfiguration : IEntityTypeConfiguration<StateStreamItem>
    {
        public void Configure(EntityTypeBuilder<StateStreamItem> builder)
            => builder.HasIndex(i => i.IdHash);
    }
}