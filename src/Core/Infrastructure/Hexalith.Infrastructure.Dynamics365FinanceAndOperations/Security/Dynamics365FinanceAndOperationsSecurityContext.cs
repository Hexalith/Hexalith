// <copyright file="Dynamics365FinanceAndOperationsSecurityContext.cs" company="Fiveforty SAS Paris France">
//     Copyright (c) Fiveforty SAS Paris France. All rights reserved.
//     Licensed under the MIT license.
//     See LICENSE file in the project root for full license information.
// </copyright>

namespace Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Security;

using Hexalith.Infrastructure.AzureActiveDirectory;
using Hexalith.Infrastructure.Dynamics365FinanceAndOperations.Configurations;

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

/// <summary>
/// Security context for Dynamics 365 Finance and Operations.
/// </summary>
public class Dynamics365FinanceAndOperationsSecurityContext : AzureActiveDirectoryApplicationSecurityContext, IDynamics365FinanceAndOperationsSecurityContext
{
	private readonly string[] _scopes;

	/// <summary>
	/// Initializes a new instance of the <see cref="Dynamics365FinanceAndOperationsSecurityContext" /> class.
	/// </summary>
	/// <param name="settings">The settings containing security configuration.</param>
	/// <param name="logger">The logger instance.</param>
	public Dynamics365FinanceAndOperationsSecurityContext(
		IOptions<Dynamics365FinanceAndOperationsClientSettings> settings,
		ILogger<AzureActiveDirectoryApplicationSecurityContext> logger)
		: base(
			settings?.Value?.Identity ?? throw new ArgumentNullException(nameof(settings)),
			logger)
	{
		Dynamics365FinanceAndOperationsClientSettings s = settings.Value ?? throw new ArgumentNullException(nameof(settings));
		if (string.IsNullOrWhiteSpace(s.Instance?.OriginalString))
		{
			throw new ArgumentException(
				$"The {nameof(s.Instance)} setting is not defined.",
				nameof(settings));
		}

		_scopes = new[] { $"{s.Instance.OriginalString}/.default" };
	}

	/// <inheritdoc/>
	public Task<string> AcquireTokenAsync(CancellationToken cancellationToken)
	{
		return AcquireTokenAsync(_scopes, cancellationToken);
	}
}