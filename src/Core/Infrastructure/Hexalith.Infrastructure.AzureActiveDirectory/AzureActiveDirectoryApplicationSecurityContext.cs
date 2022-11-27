// Fiveforty S.A. Paris France (2022)
namespace Hexalith.Infrastructure.AzureActiveDirectory;

using Hexalith.Infrastructure.AzureActiveDirectory.Configurations;
using Hexalith.Infrastructure.Security.Abstractions;

using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;

using System;

public abstract class AzureActiveDirectoryApplicationSecurityContext : IApplicationSecurityContext
{
	private readonly AzureActiveDirectoryApplicationSecurityContextConfiguration _configuration;
	private readonly ILogger _logger;
	private IConfidentialClientApplication? _application;

	protected AzureActiveDirectoryApplicationSecurityContext(AzureActiveDirectoryApplicationSecurityContextConfiguration configuration, ILogger<AzureActiveDirectoryApplicationSecurityContext> logger)
	{
		_configuration = configuration;
		_logger = logger;

		if (string.IsNullOrWhiteSpace(_configuration.Tenant))
		{
			throw new ArgumentException($"The {nameof(_configuration.Tenant)} setting is not defined.",
										nameof(configuration));
		}
		if (string.IsNullOrWhiteSpace(_configuration.ApplicationId))
		{
			throw new ArgumentException($"The {nameof(_configuration.ApplicationId)} setting is not defined.",
										nameof(configuration));
		}
		if (string.IsNullOrWhiteSpace(_configuration.ApplicationSecret))
		{
			throw new ArgumentException($"The {nameof(_configuration.ApplicationSecret)} setting is not defined.",
										nameof(configuration));
		}
	}

	private IConfidentialClientApplication Application
		=> _application ??= ConfidentialClientApplicationBuilder
			.Create(_configuration.ApplicationId)
			.WithAuthority(AzureCloudInstance.AzurePublic, _configuration.Tenant)
			.WithClientSecret(_configuration.ApplicationSecret)
			.Build();

	public async Task<string> AcquireToken(string[] scopes, CancellationToken cancellationToken)
	{
		try
		{
			return (await Application
				  .AcquireTokenForClient(scopes)
				  .ExecuteAsync(cancellationToken))
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