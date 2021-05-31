namespace Hexalith.SalesHistory.Infrastructure.Repositories
{
    using System;
    using System.IO;

    using Hexalith.Infrastructure.Helpers;
    using Hexalith.SalesHistory.Settings;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Design;
    using Microsoft.Extensions.Configuration;

    public class SalesHistoryContextFactory : IDesignTimeDbContextFactory<SalesHistoryDbContext>
    {
        public SalesHistoryDbContext CreateDbContext(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json")
                 .Build();
            var settings = configuration.GetSettings<SalesHistorySettings>();
            var dbContextBuilder = new DbContextOptionsBuilder<SalesHistoryDbContext>();

            if (!string.IsNullOrWhiteSpace(settings.ConnectionString))
            {
                dbContextBuilder.UseSqlServer(settings.ConnectionString);
            }
            else
            {
                Console.WriteLine($"Info: Database settings ({nameof(SalesHistorySettings) + ":" + nameof(SalesHistorySettings.ConnectionString)}) not set for sales history module.");
            }
            return new SalesHistoryDbContext(dbContextBuilder.Options);
        }
    }
}