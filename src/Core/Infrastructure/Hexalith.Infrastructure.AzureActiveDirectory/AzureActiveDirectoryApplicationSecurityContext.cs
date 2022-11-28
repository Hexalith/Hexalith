// <copyright file="AzureActiveDirectoryApplicationSecurityContext.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.AzureActiveDirectory;

using Hexalith.Infrastructure.AzureActiveDirectory.Configurations;
using Hexalith.Infrastructure.Security.Abstractions;

using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;

public abstract class AzureActiveDirectoryApplicationSecurityContext : IApplicationSecurityContext
{
	private readonly AzureActiveDirectoryApplicationSecurityContextConfiguration _configuration;
	private readonly ILogger _logger;
	private IConfidentialClientApplication? _application;

	/// <summary>
	/// Initializes a new instance of the <see cref="AzureActiveDirectoryApplicationSecurityContext"/> class.
	/// </summary>
	/// <param name="configuration"></param>
	/// <param name="logger"></param>
	protected AzureActiveDirectoryApplicationSecurityContext(AzureActiveDirectoryApplicationSecurityContextConfiguration configuration, ILogger<AzureActiveDirectoryApplicationSecurityContext> logger)
	{
		_configuration = configuration;
		_logger = logger;

		if (string.IsNullOrWhiteSpace(_configuration.Tenant))
		{
			throw new ArgumentException(
				$"The {nameof(_configuration.Tenant)} setting is not defined.",
				nameof(configuration));
		}

		if (string.IsNullOrWhiteSpace(_configuration.ApplicationId))
		{
			throw new ArgumentException(
				$"The {nameof(_configuration.ApplicationId)} setting is not defined.",
				nameof(configuration));
		}

		if (string.IsNullOrWhiteSpace(_configuration.ApplicationSecret))
		{
			throw new ArgumentException(
				$"The {nameof(_configuration.ApplicationSecret)} setting is not defined.",
				nameof(configuration));
		}
	}

	private IConfidentialClientApplication Application
		=> _application ??= ConfidentialClientApplicationBuilder
			.Create(_configuration.ApplicationId)
			.WithAuthority(AzureCloudInstance.AzurePublic, _configuration.Tenant)
			.WithClientSecret(_configuration.ApplicationSecret)
			.Build();

	/// <inheritdoc/>
	public async Task<string> AcquireTokenAsync(string[] scopes, CancellationToken cancellationToken)
	{
		try
		{
			return (await Application
				  .AcquireTokenForClient(scopes)
				  .ExecuteAsync(cancellationToken).ConfigureAwait(false))
				  .AccessToken;
		}
		catch (Exception e)
		{
			_logger.LogError(
				e,
				"Error while acquiring Token.\nScopes={Scope}\nAuthority={Authority}\nClientId={ClientId}",
				string.Join(';', scopes),
				Application.Authority,
				_configuration.ApplicationId);
			throw;
		}
	}
}