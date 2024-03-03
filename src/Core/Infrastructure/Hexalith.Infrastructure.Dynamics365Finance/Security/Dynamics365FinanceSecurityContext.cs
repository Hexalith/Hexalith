// <copyright file="Dynamics365FinanceSecurityContext.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

/*
 * <Your-Product-Name>
 * Copyright (c) <Year-From>-<Year-To> <Your-Company-Name>
 *
 * Please configure this header in your SonarCloud/SonarQube quality profile.
 * You can also set it in SonarLint.xml additional file for SonarLint or standalone NuGet analyzer.
 */

namespace Hexalith.Infrastructure.Dynamics365Finance.Security;

using Hexalith.Infrastructure.AzureActiveDirectory;
using Hexalith.Infrastructure.Dynamics365Finance.Configurations;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

/// <summary>
/// Security context for Dynamics 365 Finance and Operations.
/// </summary>
public class Dynamics365FinanceSecurityContext : AzureActiveDirectoryApplicationSecurityContext, IDynamics365FinanceSecurityContext
{
    private readonly string[] _scopes;

    /// <summary>
    /// Initializes a new instance of the <see cref="Dynamics365FinanceSecurityContext" /> class.
    /// </summary>
    /// <param name="settings">The settings containing security configuration.</param>
    /// <param name="logger">The logger instance.</param>
    public Dynamics365FinanceSecurityContext(
        IOptions<Dynamics365FinanceClientSettings> settings,
        ILogger<AzureActiveDirectoryApplicationSecurityContext> logger)
        : base(
            settings?.Value?.Identity ?? throw new ArgumentNullException(nameof(settings)),
            logger)
    {
        Dynamics365FinanceClientSettings s = settings.Value ?? throw new ArgumentNullException(nameof(settings));
        if (string.IsNullOrWhiteSpace(s.Instance?.OriginalString))
        {
            throw new ArgumentException(
                $"The {nameof(s.Instance)} setting is not defined.",
                nameof(settings));
        }

        _scopes = [$"{s.Instance.OriginalString}/.default"];
    }

    /// <inheritdoc/>
    public async Task<string> AcquireTokenAsync(CancellationToken cancellationToken)
        => await AcquireTokenAsync(_scopes, cancellationToken).ConfigureAwait(false);
}