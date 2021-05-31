namespace Hexalith.Server
{
    using System;

    using Hexalith.Infrastructure.EfCore.Repositories;
    using Hexalith.SalesHistory.Infrastructure.Repositories;

    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;

    public static class Program
    {
        // EF Core uses this method at design time to access the DbContext
        public static IHostBuilder CreateHostBuilder(string[] args)
            => Host
                .CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(builder =>
                {
                    builder.ConfigureAppConfiguration(config => config.AddUserSecrets(typeof(Program).Assembly));
                    builder.ConfigureLogging(l => l.AddAzureWebAppDiagnostics());
                    builder.CaptureStartupErrors(true);
                    builder.UseSetting(WebHostDefaults.DetailedErrorsKey, "true");
                    builder.UseStartup<Startup>();
                    builder.UseShutdownTimeout(TimeSpan.FromSeconds(60));
                });

        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args)
                .Build();
            using (var scope = host.Services.CreateScope())
            {
                DbContext db = scope.ServiceProvider.GetRequiredService<StateStoreDbContext>();
                db.Database.Migrate();
                db = scope.ServiceProvider.GetRequiredService<SalesHistoryDbContext>();
                db.Database.Migrate();
                // db = scope.ServiceProvider.GetRequiredService<UblDocumentsDbContext>(); db.Database.Migrate();
            }
            host
                .Run();
        }
    }
}