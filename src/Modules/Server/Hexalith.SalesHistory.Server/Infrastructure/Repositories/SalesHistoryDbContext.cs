#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace Hexalith.SalesHistory.Infrastructure.Repositories
{
    using Hexalith.SalesHistory.Common.Application.States;

    using Microsoft.EntityFrameworkCore;

    public class SalesHistoryDbContext : DbContext
    {
        public SalesHistoryDbContext(DbContextOptions<SalesHistoryDbContext> options)
               : base(options)
        {
        }

        public DbSet<SalesHistoryState> SalesHistory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(SalesHistoryStateTypeConfguration).Assembly);
        }
    }
}