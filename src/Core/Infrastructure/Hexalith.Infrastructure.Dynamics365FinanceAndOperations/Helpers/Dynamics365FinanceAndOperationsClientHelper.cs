// Fiveforty S.A. Paris France (2022)

namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Helpers;

using Hexalith.Extensions.Configuration;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Client;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Configurations;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Security;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public static class Dynamics365FinanceAndOperationsClientHelper
{
	/// <summary>
	/// Adds a Dynamics 365 Finance and Operations client to the service collection.
	/// </summary>
	/// <param name="services">Service collection</param>
	/// <param name="configuration">Configuration containing the vlient settings values</param>
	/// <returns></returns>
	public static IServiceCollection AddDynamics365FinanceAndOperationsClient(this IServiceCollection services, IConfiguration configuration)
	{
		return services
			.AddHttpClient()
			.ConfigureSettings<Dynamics365FinanceAndOperationsClientSettings>(configuration)
			.AddSingleton<IDynamics365FinanceAndOperationsSecurityContext, Dynamics365FinanceAndOperationsSecurityContext>()
			.AddScoped<IDynamics365FinanceAndOperationsClient, Dynamics365FinanceAndOperationsClient>();
	}
}