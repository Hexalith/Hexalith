using System.IO;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Hexalith.Infrastructure.EfCore.Repositories
{
    public class StateStoreContextFactory : IDesignTimeDbContextFactory<StateStoreDbContext>
    {
        public StateStoreDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json")
                 .Build();

            var dbContextBuilder = new DbContextOptionsBuilder<StateStoreDbContext>();

            var connectionString = configuration.GetSection("StateStoreConnectionString").Value;

            dbContextBuilder.UseSqlServer(connectionString);

            return new StateStoreDbContext(dbContextBuilder.Options);
        }
    }
}