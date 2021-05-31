#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

namespace Hexalith.Infrastructure.EfCore.Repositories
{
    using Microsoft.EntityFrameworkCore;

    public class StateStoreDbContext
        : DbContext
    {
        public StateStoreDbContext(DbContextOptions<StateStoreDbContext> options)
               : base(options)
        {
        }

        public DbSet<OutboxMessage> MessageOutbox { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<StateStreamItem> StateStreams { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(MessageOutboxTypeConfiguration).Assembly);
        }
    }
}