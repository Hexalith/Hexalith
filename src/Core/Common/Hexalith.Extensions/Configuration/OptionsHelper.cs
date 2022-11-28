// <copyright file="OptionsHelper.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Extensions.Configuration;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

/// <summary>
/// Helper class to configure settings.
/// </summary>
public static class OptionsHelper
{
	/// <summary>
	/// Configure settings.
	/// </summary>
	/// <typeparam name="T">The type of the settings object.</typeparam>
	/// <param name="services">The service collection.</param>
	/// <param name="configuration">The configuration instance.</param>
	/// <returns>The service collection.</returns>
	public static IServiceCollection ConfigureSettings<T>(this IServiceCollection services, IConfiguration configuration)
		where T : class, ISettings
	{
		return services.Configure<T>(configuration.GetSection(T.ConfigurationName()));
	}
}