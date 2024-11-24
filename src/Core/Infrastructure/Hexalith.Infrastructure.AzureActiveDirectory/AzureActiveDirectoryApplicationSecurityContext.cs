// <copyright file="AzureActiveDirectoryApplicationSecurityContext.cs" company="ITANEO">
// Copyright (c) ITANEO (https://www.itaneo.com). All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.AzureActiveDirectory;

using Hexalith.Infrastructure.AzureActiveDirectory.Configurations;

using Microsoft.Extensions.Logging;
using Microsoft.Identity.Client;

/// <summary>
/// The Azure Active Directory security context.
/// </summary>
public abstract partial class AzureActiveDirectoryApplicationSecurityContext : IApplicationSecurityContext
{
    private readonly AzureActiveDirectoryApplicationSecurityContextConfiguration _configuration;
    private readonly ILogger _logger;
    private IConfidentialClientApplication? _application;

    /// <summary>
    /// Initializes a new instance of the <see cref="AzureActiveDirectoryApplicationSecurityContext"/> class.
    /// </summary>
    /// <param name="configuration">Active directory configuration.</param>
    /// <param name="logger">The logger.</param>
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
                  .ExecuteAsync(cancellationToken)
                  .ConfigureAwait(false))
                  .AccessToken;
        }
        catch (Exception e)
        {
            LogAcquireTokenError(
                e,
                string.Join(';', scopes),
                Application.Authority,
                _configuration.ApplicationId ?? "Unknown");
            throw;
        }
    }

    /// <summary>
    /// Logs an error that occurred while acquiring a token.
    /// </summary>
    /// <param name="ex">The exception that was thrown.</param>
    /// <param name="scope">The scope for which the token was requested.</param>
    /// <param name="authority">The authority used for the token request.</param>
    /// <param name="clientId">The client ID used for the token request.</param>
    [LoggerMessage(
        EventId = 1,
        Level = Microsoft.Extensions.Logging.LogLevel.Error,
        Message = "Error while acquiring Token.\nScopes={Scope}\nAuthority={Authority}\nClientId={ClientId}.")]
    public partial void LogAcquireTokenError(Exception ex, string scope, string authority, string clientId);
}