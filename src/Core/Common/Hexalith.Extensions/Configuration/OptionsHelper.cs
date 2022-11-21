// Fiveforty S.A. Paris France (2022)
namespace Hexalith.Extensions.Configuration;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class OptionsHelper
{
	public static IServiceCollection ConfigureSettings<T>(this IServiceCollection services, IConfiguration configuration)
		where T : class, ISettings
		=> services.Configure<T>(configuration.GetSection(T.ConfigurationName()));
}