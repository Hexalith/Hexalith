namespace Hexalith.SalesHistory.Infrastructure.Repositories
{
    using Hexalith.SalesHistory.Common.Application.States;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;

    public class SalesHistoryStateTypeConfguration : IEntityTypeConfiguration<SalesHistoryState>
    {
        public void Configure(EntityTypeBuilder<SalesHistoryState> builder)
        {
            builder.Property<int>("Id")
               .HasColumnType("int")
               .ValueGeneratedOnAdd();
            builder.HasKey("Id");
            builder.HasIndex(p => new { p.CompanyId, p.InvoiceId, p.LineId }).IsUnique();
            builder.HasIndex(p => p.CustomerId);
            builder.HasIndex(p => p.ItemId);
            builder.HasIndex(p => p.InvoiceDate);
        }
    }
}