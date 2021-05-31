namespace Hexalith.Infrastructure.Helpers
{
    using System;

    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;

    public static class ConfigurationHelper
    {
        public static IServiceCollection ConfigureSettings<TSettings>(this IServiceCollection services, IConfiguration configuration)
            where TSettings : class
        {
            _ = configuration ?? throw new ArgumentNullException(nameof(configuration));
            return services.Configure<TSettings>(configuration.GetSection(typeof(TSettings).Name));
        }

        public static TSettings GetSettings<TSettings>(this IConfiguration configuration)
                    where TSettings : new()
            => configuration == null ? new TSettings() : configuration.GetSection(typeof(TSettings).Name).Get<TSettings>() ?? new TSettings();
    }
}