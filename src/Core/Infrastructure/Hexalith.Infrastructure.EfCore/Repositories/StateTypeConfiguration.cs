namespace Hexalith.Infrastructure.EfCore.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class StateTypeConfiguration : IEntityTypeConfiguration<State>
    {
        public void Configure(EntityTypeBuilder<State> builder)
        {
            builder.HasKey(p => new { p.IdHash, p.Id });
            builder.Property(p => p.Version).IsConcurrencyToken();
        }
    }
}